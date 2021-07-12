using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using DC.Business.Application.Contracts.Dtos.Account;
using DC.Business.Application.Contracts.Interfaces.Account;
using DC.Business.Application.Services.Helpers;
using DC.Business.Application.Services.Pipeline;
using DC.Business.Domain.Entities.Organization;
using DC.Business.Domain.Repositories.Organization;
using DC.Core.Contracts.Application.Pipeline.Dtos;
using DC.Core.Contracts.Application.Pipeline.Dtos.Errors;
using DC.Core.Contracts.Infrastructure.Security;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using DC.Business.WebApi.Helpers;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace DC.Business.Application.Services.Account
{
    public class GetTokenByEmailAndPasswordService : BusinessService<EmailAndPasswordInputDto,  string>, IGetTokenByEmailAndPasswordService
    {
        private static readonly string[] PasswordHashingAlgorithms = { "md5", "sha1", "sha256" };
        private readonly IUserRepository userRepository;
        private readonly ITokenService tokenService;
        private readonly AppSettings _appSettings;


        public GetTokenByEmailAndPasswordService(IUserRepository userRepository,
            ITokenService tokenService,
            IOptions<AppSettings> appSettings)
        {
            this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            this.tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            this._appSettings = appSettings.Value ?? throw new ArgumentNullException(nameof(appSettings.Value));
        }

        protected override async Task<OperationResultDto<string>> ExecuteAsync(EmailAndPasswordInputDto inputDto, CancellationToken cancellationToken = default)
        {
            // Guid newSession = Guid.NewGuid();

            List<ErrorDto> executionErrors = ValidateInput(inputDto);
            if (executionErrors.Any()) return BuildOperationResultDto(executionErrors);

            List<string> hashedPasswords = new List<string>();
            foreach (string algorithm in PasswordHashingAlgorithms)
                hashedPasswords.Add(AuthenticationHelper.GetHashedPasswordForDatabase(inputDto.Password, algorithm));

            User user = await userRepository.GetUserByEmailAndPasswordsAsync(inputDto.Email, hashedPasswords);
            if (user == null)
                return BuildOperationResultDto(new ErrorDto(ErrorCodes.INVALID_AUTHENTICATION_DATA, nameof(inputDto.Email), inputDto.Email));

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
              Subject = new ClaimsIdentity(new Claim[]
              {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, "Admin")  // TODO user.Role
              }),
              Expires = DateTime.UtcNow.AddDays(7),
              SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(securityToken);

            //List<Claim> userClaims = new List<Claim>
            //{
            //    new Claim(DCClaimTypes.NameIdentifier, user.Id.ToString()),
            //    new Claim(DCClaimTypes.UniqueNameClaim, user.Email), // Change to email
            //    new Claim(DCClaimTypes.SessionId, newSession.ToString()),
            //    //new Claim(DCClaimTypes.SystemAdmin, AuthorizationHelper.IsSuperUser(user).ToString()),
            //    new Claim(DCClaimTypes.OriginalUsername, user.Email) // Change to email
            //    //new Claim(DCClaimTypes.OriginalSystem, tokenService.CurrentAudience)
            //};
            //Token
            return BuildOperationResultDto(token); 
        }

        private List<ErrorDto> ValidateInput(EmailAndPasswordInputDto inputDto)
        {
            if (inputDto is null)
                return new List<ErrorDto> { new ErrorDto(ErrorCodes.REQUIRED_FILED_IS_EMPTY, nameof(inputDto)) };

            List<ErrorDto> validationErrors = new List<ErrorDto>();

            if (string.IsNullOrEmpty(inputDto.Email))
                validationErrors.Add(new ErrorDto(ErrorCodes.REQUIRED_FILED_IS_EMPTY, nameof(inputDto.Email)));

            if (string.IsNullOrEmpty(inputDto.Password))
                validationErrors.Add(new ErrorDto(ErrorCodes.REQUIRED_FILED_IS_EMPTY, nameof(inputDto.Password)));

            return validationErrors;
        }
    }
}
