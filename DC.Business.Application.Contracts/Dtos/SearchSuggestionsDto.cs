using System;
using System.Collections.Generic;
using System.Text;

namespace DC.Business.Application.Contracts.Dtos
{
    public class SearchSuggestionsDto
    {
        public string Query { get; set; }

        public SearchSuggestionDto[] Results { get; set; }
    }
}
