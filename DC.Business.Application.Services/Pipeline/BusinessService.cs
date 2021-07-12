using System;
using System.Threading;
using System.Threading.Tasks;
using DC.Business.Domain.Entities.Organization;
using DC.Core.Application.Pipeline;
using DC.Core.Contracts.Application.Pipeline.Dtos;

namespace DC.Business.Application.Services.Pipeline
{
    public abstract class BusinessService<TIn, TOut> : ApplicationServiceBase<TIn, TOut>
    {
        public BusinessService()
        {
        }

        public User CurrentUser { get; private set; }

        public override async Task<OperationResultDto<TOut>> ExecuteServiceAsync(TIn inputDto, CancellationToken cancellationToken = default)
        {
            OperationResultDto<TOut> result = await base.ExecuteServiceAsync(inputDto, cancellationToken).ConfigureAwait(false);

            return result;

        }
    }
}
