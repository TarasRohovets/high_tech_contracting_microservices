using System;
using System.Collections.Generic;
using System.Text;

namespace DC.Business.Application.Contracts.Dtos
{
    public class SearchSuggestionDto
    {
        public string Text { get; set; }
        public string Highlight { get; set; }
    }
}
