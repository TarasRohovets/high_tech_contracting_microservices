using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DC.Business.Application.Contracts.Dtos.Organization.Users;
using DC.Business.Application.Contracts.Interfaces.Organization.Users;
using DC.Business.Application.Services.Pipeline;
using DC.Business.Domain.Entities.Organization;
using DC.Business.Domain.Repositories.Organization;
using DC.Core.Contracts.Application.Pipeline.Dtos;
using DC.Core.Contracts.Application.Pipeline.Dtos.Errors;
using DC.Business.Application.Contracts.Dtos.Constants;
using DC.Business.Application.Services.Helpers;
using DC.Business.Domain.Repositories.ElasticSearch;
using DC.Core.Contracts.Infrastructure.RabbitMQ;
using System.Text.Json;
using DC.Business.Domain.Enums;
using DC.Business.WebApi.Helpers;
using Microsoft.Extensions.Configuration;

namespace DC.Business.Application.Services.Organization.Users
{
   
    public class CreateUserService : BusinessService<CreateUserDto, long>, ICreateUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUsersElasticRepository _usersElasticRepository; // TODO testing
        private readonly IRabbitMQClient _rabbitMqClient;
        private readonly IConfiguration _configuration;

        public CreateUserService(IConfiguration configuration, IUserRepository userRepository, IUsersElasticRepository usersElasticRepository, IRabbitMQClient rabbitMqClient)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _usersElasticRepository = usersElasticRepository ?? throw new ArgumentNullException(nameof(usersElasticRepository));
            _rabbitMqClient = rabbitMqClient ?? throw new ArgumentNullException(nameof(rabbitMqClient));
        }

        protected override async Task<OperationResultDto<long>> ExecuteAsync(CreateUserDto inputDto, CancellationToken cancellationToken = default)
        {
            List<ErrorDto> executionErrors = ValidateInput(inputDto);
            if (executionErrors.Any()) return BuildOperationResultDto(executionErrors);

            //if (CurrentUser == default)
            //    return BuildOperationResultDto(new ErrorDto(ErrorCodes.UNAUTHORIZED_ACTION_FOR_CALLING_USER));

            //if(!AuthorizationHelper.CanCreateUser(newUser: inputDto, requestingUser: CurrentUser))
            //    return BuildOperationResultDto(new ErrorDto(ErrorCodes.UNAUTHORIZED_ACTION_FOR_CALLING_USER));

            // User user = await _userRepository.GetUserByUsernameAsync(inputDto.UserName); OBSOLETE
            User user = await _userRepository.GetUserByEmailAsync(inputDto.Email);
            if (user != default)
                return BuildOperationResultDto(new ErrorDto(BusinessErrorCodes.EMAIL_ALREADY_EXISTS));

            var isAdmin = _configuration["admin:email"];
            User newUser = null;
            if(inputDto.Email == isAdmin)
            {
                newUser = CreateUser(inputDto, UserType.Admin);
            } else
            {
                newUser = CreateUser(inputDto, UserType.User);
            }

            /* encrypt the password */
            string hashedPassword = AuthenticationHelper.GetHashedPasswordForDatabase(inputDto.Password, AuthenticationHelper.AlgorithmSHA1);

            long newUserId = await _userRepository.CreateUserAsync(newUser, hashedPassword).ConfigureAwait(false);

            // await _usersElasticRepository.AddUserAsync(newUser); // TODO ElasticSearch
            //  await _usersElasticRepository.GetAllUsersAsync(); // TODO ElasticSearch think no, just properties

            // TEST
            var payload = JsonSerializer.Serialize(newUser);
            await _rabbitMqClient.PublishMessageAsync(payload, "user.created");

            return BuildOperationResultDto(newUserId);
        }

        private User CreateUser(CreateUserDto inputDto, UserType type)
        {
            User newUser = new User
            {
                Name = inputDto.Name,
                Email = inputDto.Email,
                Type = type,
                Deleted = false,
                CreationDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow
            };

            return newUser;
        }

        private List<ErrorDto> ValidateInput(CreateUserDto inputDto)
        {
            List<ErrorDto> validationsErros = new List<ErrorDto>();

            if (string.IsNullOrEmpty(inputDto.Email))
                validationsErros.Add(new ErrorDto(ErrorCodes.REQUIRED_FILED_IS_EMPTY, nameof(inputDto.Email)));
           // else if (!ValidationHelper.IsValidEmail(inputDto.Email))
           //     validationErrors.Add(new ErrorDto(BusinessErrorCodes.FAILD_EMAIL_VALIDATION, nameof(inputDto.Email)));

            //if (string.IsNullOrEmpty(inputDto.UserName))
            //    validationsErros.Add(new ErrorDto(ErrorCodes.REQUIRED_FILED_IS_EMPTY, nameof(inputDto.UserName)));


            if (string.IsNullOrEmpty(inputDto.Password))
                validationsErros.Add(new ErrorDto(ErrorCodes.REQUIRED_FILED_IS_EMPTY, nameof(inputDto.Password)));

            //if (!ValidationHelper.IsValidPassword(inputDto.Password))
           //     validationErrors.Add(new ErrorDto(BusinessErrorCodes.FAILD_PASSWORD_COMPLEXITY, nameof(inputDto.Password)));


            if (string.IsNullOrEmpty(inputDto.Name))
                validationsErros.Add(new ErrorDto(ErrorCodes.REQUIRED_FILED_IS_EMPTY, nameof(inputDto.Name)));

            return validationsErros;
        }
    }
}
