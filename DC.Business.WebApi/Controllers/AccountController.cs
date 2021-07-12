using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DC.Business.Application.Contracts.Dtos.Account;
using DC.Business.Application.Contracts.Interfaces.Account;
using DC.Core.Contracts.Application.Pipeline.Dtos;
using Microsoft.AspNetCore.Mvc;


namespace DC.Business.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class AccountController : ControllerBase
    {
        private readonly IGetTokenByEmailAndPasswordService _getTokenByEmailAndPasswordService;

        public AccountController(IGetTokenByEmailAndPasswordService getTokenByUsernameAndPasswordService){
            _getTokenByEmailAndPasswordService = getTokenByUsernameAndPasswordService ?? throw new ArgumentNullException(nameof(getTokenByUsernameAndPasswordService));
        }

        [HttpPost, Route("token")]
        public async Task<IActionResult> GetTokenByEmailAndPasswordAsync([FromBody] EmailAndPasswordInputDto emailAndPassword, CancellationToken cancellationToken)
        {
            OperationResultDto<string> result = await _getTokenByEmailAndPasswordService.ExecuteServiceAsync(emailAndPassword, cancellationToken);

            return Ok(result);
        }


    }
}
