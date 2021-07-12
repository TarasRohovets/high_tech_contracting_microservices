using System;
namespace DC.Core.Contracts.Infrastructure.Security
{
    public class DCClaimTypes
    {
		public const string Audience = "aud";
		public const string Issuer = "iss";
		public const string NameIdentifier = "nameid";
		public const string ExpirationtimeClaim = "exp";
		public const string UniqueNameClaim = "unique_name";

		public const string SessionId = "session_id";
		public const string SystemAdmin = "system_admin";

		public const string OriginalUsername = "external_username";
		public const string OriginalSystem = "external_system_audience";
	}
}
