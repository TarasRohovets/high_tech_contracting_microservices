using System;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;

namespace DC.Core.Contracts.Infrastructure.Security
{
    public interface ITokenService
    {
        string GenerateToken(IEnumerable<Claim> claims);
        string DecodeToken(string token);
        IPrincipal ValidateToken(string token);
        bool ValidateExternalToken(string token, string issuer, string audience, SecurityKey secretKey);
        string GetClaimValue(string token, string claim);
    }
}
