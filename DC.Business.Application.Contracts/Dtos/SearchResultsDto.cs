using System;
using System.Collections.Generic;
using System.Text;

namespace DC.Business.Application.Contracts.Dtos
{
    public class SearchResultsDto
    {
        public string Query { get; set; }

        public SearchResultDto[] Results { get; set; }
    }
}
