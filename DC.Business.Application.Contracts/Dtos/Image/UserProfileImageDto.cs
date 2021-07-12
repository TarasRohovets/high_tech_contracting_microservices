using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace DC.Business.Application.Contracts.Dtos.Image
{
    public class UserProfileImageDto
    {
        public string UserEmail { get; set; }
        public string ImageName { get; set; }
        public string FileMimeType { get; set; }
        public string ImageUrl { get; set; } // for delete
    }
}
