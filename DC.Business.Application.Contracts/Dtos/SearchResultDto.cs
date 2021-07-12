using System;
using System.Collections.Generic;
using System.Text;

namespace DC.Business.Application.Contracts.Dtos
{
    public class SearchResultDto
    {
        public string Identifier { get; set; }
        public string Title { get; set; }
        public string[] Keywords { get; set; }
        public string[] Matches { get; set; }
    }
}
