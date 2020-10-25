using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YS.Knife.Rest.Api;

namespace YS.Privilege.Rest.Api
{
    [ApiController]
    [Route("privilege")]
    public class PrivilegeController : ControllerBase
    {
        [HttpGet]
        [Route("assert/{resource}")]
        public Task<bool> Assert([FromQuery]List<LogicRole> role, [FromRoute]Resource resource)
        {
            throw new NotImplementedException();
        }

        //public Task Assign(LogicRole logicRole, List<Resource> allowResources, List<Resource> denyResources, List<Resource> noneResources)
        //{
        //    throw new NotImplementedException();
        //}
        [HttpGet]
        [Route("query/{category}")]
        public Task<List<Resource>> Query([FromQuery] List<LogicRole> role, string category)
        {
            throw new NotImplementedException();
        }
    }
}
