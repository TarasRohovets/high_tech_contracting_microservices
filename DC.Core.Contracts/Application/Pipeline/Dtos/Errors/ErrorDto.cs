using System;
using System.Collections.Generic;

namespace DC.Core.Contracts.Application.Pipeline.Dtos.Errors
{
    public class ErrorDto
    {
        public string Code { get; set; }

        public string Field { get; set; }

        public HashSet<string> Values { get; set; }

        public ErrorDto()
        {
            Values = new HashSet<string>();
        }

        public ErrorDto(string code)
        {
            Code = code;
            Values = new HashSet<string>();
        }

        public ErrorDto(string code, string field): this(code)
        {
            Field = field;
        }

        public ErrorDto(string code, string field = null, params string[] values)
            : this(code, field)
        {
            if (values != null)
                Values = new HashSet<string>(values);
        }

        public ErrorDto(string code, string field = null, IEnumerable<string> values = null)
            : this(code, field)
        {
            if (values != null)
                Values = new HashSet<string>(values);
        }
    }
}
