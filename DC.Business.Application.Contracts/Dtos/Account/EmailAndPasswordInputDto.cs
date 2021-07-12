using System;
using System.ComponentModel.DataAnnotations;

namespace DC.Business.Application.Contracts.Dtos.Account
{
    [Serializable]
    public class EmailAndPasswordInputDto
    {
       [Required]
        public string Email { get; set; }
       [Required]
        public string Password { get; set; }
    }
}
