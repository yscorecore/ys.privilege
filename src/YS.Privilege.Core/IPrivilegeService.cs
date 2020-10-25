using System.Collections.Generic;
using System.Threading.Tasks;

namespace YS.Privilege
{
    public interface IPrivilegeService
    {
        Task<bool> Assert(List<LogicRole> logicRoles, Resource resource);
        Task<List<Resource>> Query(List<LogicRole> logicRoles, string resourceCategory);
        Task Assign(LogicRole logicRole, List<Resource> allowResources, List<Resource> denyResources, List<Resource> noneResources);
    }

}
