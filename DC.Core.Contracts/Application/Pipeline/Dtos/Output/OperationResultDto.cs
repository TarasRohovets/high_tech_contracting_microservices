using System;
using System.Collections.Generic;
using DC.Core.Contracts.Application.Pipeline.Dtos.Errors;

namespace DC.Core.Contracts.Application.Pipeline.Dtos
{
    public class OperationResultDto<TOut>
    {
        public Guid CorrelationId { get; set; }

        public bool IsValid { get; set; }

        public HashSet<ErrorDto> Errors { get; set; }

        public TOut Data { get; set; }

        public OperationResultDto()
        {
            Errors = new HashSet<ErrorDto>();
            IsValid = true;
        }
    }
}
