using System;
using System.Threading;
using System.Threading.Tasks;
using DC.Core.Contracts.Application.Pipeline.Dtos;

namespace DC.Core.Contracts.Application.Pipeline
{
    public interface IApplicationService<in TIn, TOut>
    {
        Task<OperationResultDto<TOut>> ExecuteServiceAsync(TIn inputDto, CancellationToken cancellationToken = default);
    }
}
