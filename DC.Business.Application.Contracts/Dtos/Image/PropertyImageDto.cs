using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace DC.Business.Application.Contracts.Dtos.Image
{
    public class PropertyImageDto
    {
        public ulong MySqlId { get; set; }
        public string ImageUrl { get; set; }
        public string ImageName { get; set; }
    }
}
