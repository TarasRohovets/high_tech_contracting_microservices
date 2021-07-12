using System;
using System.Collections.Generic;
using System.Text;

namespace DC.Business.Application.Contracts.Dtos.Organization.Listing
{
    public class PagedPropertyBasicDto
    {
        public int TotalItems { get; set; }
        public List<PropertyBasicDto> Properties { get; set; }
    }
}
