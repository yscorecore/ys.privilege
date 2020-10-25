using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using YS.Knife;

namespace YS.Privilege.Impl.EFCore
{
    [Service(Lifetime = ServiceLifetime.Singleton)]
    public class PrivilegeService : IPrivilegeService
    {
        private readonly PrivilegeContext context;
        public PrivilegeService(PrivilegeContext context)
        {
            this.context = context;
        }
        public Task<bool> Assert(List<LogicRole> logicRoles, Resource resource)
        {
            _ = resource ?? throw new ArgumentNullException(nameof(resource));
            var roleUids = (from p in (logicRoles ?? Enumerable.Empty<LogicRole>())
                            where p != null
                            select p.ToString()).ToList();
            if (roleUids.Count == 0) return Task.FromResult(false);
            var resourceUid = resource.ToString();
            var ranks = (from p in context.Privileges
                         where p.ResourceUid == resourceUid && roleUids.Contains(p.RoleUid)
                         select p.Rank).ToList();
            return Task.FromResult(ranks.Count > 0 && ranks.All(p => p == ResourceRank.Allow));
        }
        public Task<List<Resource>> Query(List<LogicRole> logicRoles, string resourceCategory)
        {

            var roleUids = (from p in (logicRoles ?? Enumerable.Empty<LogicRole>())
                            where p != null
                            select p.ToString()).ToList();
            var datas = (from p in context.Privileges
                         where p.ResourceCategory == resourceCategory && roleUids.Contains(p.RoleUid)
                         select new Resource
                         {
                             Category = p.ResourceCategory,
                             Code = p.ResourceCode
                         }).ToList();
            return Task.FromResult(datas);
        }



        public Task Assign(LogicRole logicRole, List<Resource> allowResources, List<Resource> denyResources, List<Resource> noneResources)
        {
            _ = logicRole ?? throw new ArgumentNullException(nameof(logicRole));
            Dictionary<string, (Resource Res, ResourceRank Rank)> datas = new Dictionary<string, (Resource, ResourceRank)>();
            foreach (var res in (allowResources ?? Enumerable.Empty<Resource>()).Where(p => p != null))
            {
                datas[res.ToString()] = (res, ResourceRank.Allow);
            }
            foreach (var res in (denyResources ?? Enumerable.Empty<Resource>()).Where(p => p != null))
            {
                datas[res.ToString()] = (res, ResourceRank.Deny);
            }
            foreach (var res in (noneResources ?? Enumerable.Empty<Resource>()).Where(p => p != null))
            {
                datas[res.ToString()] = (res, ResourceRank.None);
            }
            if (datas.Count == 0)
            {
                return Task.CompletedTask;
            }
            var logicRoleUid = logicRole.ToString();
            var dataInDb = (from p in context.Privileges
                            where p.RoleUid == logicRoleUid && datas.Keys.Contains(p.ResourceUid)
                            select p)
                            .ToDictionary(p => p.ResourceUid);


            foreach (var data in datas)
            {
                if (dataInDb.Keys.Contains(data.Key))
                {
                    if (data.Value.Rank == ResourceRank.None)
                    {
                        context.Privileges.Remove(dataInDb[data.Key]);
                    }
                    else
                    {
                        dataInDb[data.Key].Rank = data.Value.Rank;
                    }
                }
                else
                {
                    if (data.Value.Rank != ResourceRank.None)
                    {
                        context.Privileges.Add(new PrivilegeAssign
                        {
                            Id = Guid.NewGuid().ToString(),
                            Rank = data.Value.Rank,
                            RoleId = logicRole.Id,
                            RoleProvider = logicRole.Provider,
                            RoleUid = logicRoleUid,
                            ResourceUid = data.Key,
                            ResourceCategory = data.Value.Res.Category,
                            ResourceCode = data.Value.Res.Code
                        });
                    }
                }
            }
            return context.SaveChangesAsync();
        }
    }
}
