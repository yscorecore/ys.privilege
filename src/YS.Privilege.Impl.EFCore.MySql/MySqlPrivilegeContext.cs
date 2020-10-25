using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace YS.Privilege.Impl.EFCore.MySql
{
    [MySqlDbContext("privilege_mysql")]
    public class MySqlPrivilegeContext : PrivilegeContext
    {
    }
}
