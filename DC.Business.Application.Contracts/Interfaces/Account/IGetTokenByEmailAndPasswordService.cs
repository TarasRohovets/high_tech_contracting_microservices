using System;
using DC.Business.Application.Contracts.Dtos.Account;
using DC.Core.Contracts.Application.Pipeline;

namespace DC.Business.Application.Contracts.Interfaces.Account
{
    public interface IGetTokenByEmailAndPasswordService : IApplicationService<EmailAndPasswordInputDto, string>
    {
    }
}
