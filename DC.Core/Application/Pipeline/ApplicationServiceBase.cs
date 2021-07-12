using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DC.Core.Contracts.Application.Pipeline;
using DC.Core.Contracts.Application.Pipeline.Dtos;
using DC.Core.Contracts.Application.Pipeline.Dtos.Errors;

namespace DC.Core.Application.Pipeline
{
    public abstract class ApplicationServiceBase<TIn, TOut> : IApplicationService<TIn, TOut>
    {
        public ApplicationServiceBase()
        {
        }

        protected abstract Task<OperationResultDto<TOut>> ExecuteAsync(TIn inputDto, CancellationToken cancellationToken = default);

        public virtual async Task<OperationResultDto<TOut>> ExecuteServiceAsync(TIn inputDto, CancellationToken cancellationToken = default)
        {
            OperationResultDto<TOut> result = new OperationResultDto<TOut>();
            result = await ExecuteAsync(inputDto, cancellationToken).ConfigureAwait(false);
            return result;
        }

        /// Build Success Operation Result Dto
        protected virtual OperationResultDto<TOut> BuildOperationResultDto(TOut data, IEnumerable<ErrorDto> executionWarnings = null)
        {
            return new OperationResultDto<TOut>
            {
                IsValid = true,
                Data = data,
                Errors = executionWarnings?.ToHashSet() ?? new HashSet<ErrorDto>()
            };
        }

        /// Build Operation Result Dto With Validation Errors
        protected OperationResultDto<TOut> BuildOperationResultDto(IEnumerable<ErrorDto> executionErrors) =>
            new OperationResultDto<TOut>
            {
                IsValid = false,
                Errors = executionErrors.ToHashSet()
            };

        /// Build Operation Result Dto With Validation Errors
        protected OperationResultDto<TOut> BuildOperationResultDto(params ErrorDto[] errors) =>
            new OperationResultDto<TOut>
            { IsValid = false,
              Errors = errors.ToHashSet()
            };
    }
}
