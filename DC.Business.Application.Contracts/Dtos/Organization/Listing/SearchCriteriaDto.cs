using System;
using System.Collections.Generic;
using System.Text;

namespace DC.Business.Application.Contracts.Dtos.Organization.Listing
{
    public class SearchCriteriaDto
    {
        public string Criteria { get; set; }

        public long PropertyTypeId { get; set; }
        public long OperationTypeId { get; set; }
        public long PriceFrom { get; set; }
        public long PriceTo { get; set; }
        public string Bedrooms { get; set; } // TODO
        public string Bathrooms { get; set; }
        public string Conditions { get; set; }
        public long SizeTo { get; set; }
        public long SizeFrom { get; set; }
        public long YearBuiltFrom { get; set; }
        public long YearBuiltTo { get; set; }
        public string Characteristics { get; set; } // TODO

        public string Field { get; set; }
        public int Page { get; set; }
        public int Size { get; set; }
    }
}
