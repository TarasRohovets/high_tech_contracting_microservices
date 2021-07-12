using System;
using System.Collections.Generic;

namespace DC.Business.Application.Contracts.Dtos.Organization.Users
{
    public class UserSearchInputDto
    {
        public string Username { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
