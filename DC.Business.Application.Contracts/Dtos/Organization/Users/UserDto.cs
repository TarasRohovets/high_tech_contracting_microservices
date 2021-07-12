using DC.Business.Application.Contracts.Dtos.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System;
using System.Collections.Generic;

namespace DC.Business.Application.Contracts.Dtos.Organization.Users
{
    public class UserDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string City { get; set; } 
        public string Country { get; set; } 
        public string Address { get; set; } 
        public string TaxNumber { get; set; }
        public UserTypeDto Type { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }

        public bool? Active { get; set; }
        public bool? EmailConfirmed { get; set; }
        public bool? Deleted { get; set; } 
        public DateTime? CreationDate { get; set; } 
        public DateTime? UpdateDate { get; set; }

        public string ImageName { get; set; }
        public string ImagePath { get; set; }
    }
}
