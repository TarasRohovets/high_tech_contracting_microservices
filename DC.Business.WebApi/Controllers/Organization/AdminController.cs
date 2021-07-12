using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DC.Business.WebApi.Controllers.Organization
{
    [Route("api/organization/[controller]")]
    [Produces("application/json")]
    [ApiController]
    [Authorize]
    public class AdminController : ControllerBase
    {
        public AdminController()
        {

        }
    }
}
