using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using DC.Core.Contracts.Infrastructure.Security;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace DC.Core.Security.Jwt
{
    public class JwtTokenService : ITokenService
    {
        private const int MaxLagTimeBetweenServer = -60; // This is the maximum allowed time off sync between server of this application cluster. Required for VMWare 6.5.
        private readonly string issuer;
        private readonly string secretKey;
        private readonly long timeToLive;

        private JwtSecurityTokenHandler JwtSecurityTokenHandler { get; }

        private SecurityTokenDescriptor securityTokenDescriptor;

        private SecurityTokenDescriptor SecurityTokenDescriptor => securityTokenDescriptor
                                                                   ?? (securityTokenDescriptor =
                                                                           new SecurityTokenDescriptor
                                                                           {
                                                                               Audience = CurrentAudience,
                                                                               Issuer = issuer,
                                                                               IssuedAt = null,
                                                                               Expires = null,
                                                                               Subject = null,
                                                                               SigningCredentials =
                                                                                   new SigningCredentials(new SymmetricSecurityKey(Convert.FromBase64String(secretKey)),
                                                                                                          "HS256")
                                                                           });

        public string CurrentAudience { get; }

        public JwtTokenService(string audience, string issuer, string secretKey, long timeToLive)
        {
#if (DEBUG)
            IdentityModelEventSource.ShowPII = true;
#endif
            if (string.IsNullOrEmpty(audience))
                throw new ArgumentNullException(nameof(audience));

            if (string.IsNullOrEmpty(issuer))
                throw new ArgumentNullException(nameof(issuer));

            if (string.IsNullOrEmpty(secretKey))
                throw new ArgumentNullException(nameof(secretKey));

            CurrentAudience = audience;
            this.issuer = issuer;
            this.secretKey = secretKey;
            this.timeToLive = timeToLive;

            JwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        }

        public string DecodeToken(string token)
        {
            throw new NotImplementedException();
        }

        public string GenerateToken(IEnumerable<Claim> claims)
        {
            if (claims == null)
                throw new ArgumentNullException(nameof(claims));

            DateTime currentTime = DateTime.Now;
            SecurityTokenDescriptor.IssuedAt = currentTime.AddSeconds(MaxLagTimeBetweenServer);
            SecurityTokenDescriptor.Expires = currentTime.AddMinutes(timeToLive);
            SecurityTokenDescriptor.Subject = new ClaimsIdentity(claims);

            return JwtSecurityTokenHandler.WriteToken(JwtSecurityTokenHandler.CreateToken(securityTokenDescriptor));
        }

        public string GetClaimValue(string token, string claim)
        {
            throw new NotImplementedException();
        }

        public bool ValidateExternalToken(string token, string issuer, string audience, SecurityKey secretKey)
        {
            throw new NotImplementedException();
        }

        public IPrincipal ValidateToken(string token)
        {
            throw new NotImplementedException();
        }
    }
}
