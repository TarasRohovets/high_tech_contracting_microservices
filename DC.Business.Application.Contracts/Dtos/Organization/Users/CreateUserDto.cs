using System;
using System.Collections.Generic;
using System.Text;

namespace DC.Business.Application.Contracts.Dtos.Organization.Users
{
    public class CreateUserDto
    {
        public string Name {get;set;}
           public string Email {get;set;}

        public string Password { get; set; }

    }
}
