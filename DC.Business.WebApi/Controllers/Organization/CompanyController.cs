using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DC.Business.WebApi.Controllers.Organization
{
    [Route("api/organization/[controller]")]
    [Produces("application/json")]
    [ApiController]
    [Authorize]
    public class CompanyController : ControllerBase
    {
        public CompanyController()
        {
        }

        [HttpPost]
        [Route("createNewCompany")]
        public async Task<IActionResult> CreateNewCompany(CancellationToken cancellationToken = default)
        {
            // var result = await _createUserService.ExecuteServiceAsync(user, cancellationToken).ConfigureAwait(false);
            return Ok();
        }
    }
}
