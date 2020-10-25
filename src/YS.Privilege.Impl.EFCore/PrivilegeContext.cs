using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;


namespace YS.Privilege.Impl.EFCore
{
    public class PrivilegeContext : DbContext
    {
        public virtual DbSet<PrivilegeAssign> Privileges { get; set; }
    }

    public class PrivilegeAssign
    {
        public string Id { get; set; }
        public string RoleUid { get; set; }
        public string RoleProvider { get; set; }
        public string RoleId { get; set; }
        public string ResourceUid { get; set; }
        public string ResourceCategory { get; set; }
        public string ResourceCode { get; set; }
        public ResourceRank Rank { get; set; }
    }
}
