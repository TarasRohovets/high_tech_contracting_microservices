using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DC.Business.Application.Contracts.Dtos.Organization.Users;
using DC.Business.Application.Contracts.Interfaces.Organization.Users;
using DC.Business.Application.Contracts.Interfaces.Services;
using DC.Business.Application.Services.Pipeline;
using DC.Business.Domain.Entities.Organization;
using DC.Business.Domain.Repositories.Organization;
using DC.Core.Contracts.Application.Pipeline.Dtos;
using DC.Core.Contracts.Application.Pipeline.Dtos.Errors;
using DC.Core.Contracts.Application.Pipeline.Dtos.Output;

namespace DC.Business.Application.Services.Organization.Users
{
    public class UpdateUserService : BusinessService<UserDto, VoidOutputDto>, IUpdateUserService
    {
        private readonly IUserRepository _userRepository;
        // private readonly IImageService _imageService;
        private IMapper _mapper;

        public UpdateUserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            //_imageService = imageService ?? throw new ArgumentNullException(nameof(imageService));
        }

        protected override async Task<OperationResultDto<VoidOutputDto>> ExecuteAsync(UserDto inputDto, CancellationToken cancellationToken = default)
        {

            //if (CurrentUser == default)
            //    return BuildOperationResultDto(new ErrorDto(ErrorCodes.UNAUTHORIZED_ACTION_FOR_CALLING_USER));

            //User user = await userRepository.GetUserByUsernameAsync(inputDto.Username, UserRelatedComponentsEnum.None).ConfigureAwait(false);
            //if (user == default)
            //    return BuildOperationResultDto(new ErrorDto(BusinessErrorCodes.USER_NOT_FOUND));

            //if (!AuthorizationHelper.CanChangeUser(user, CurrentUser))
            //    return BuildOperationResultDto(new ErrorDto(ErrorCodes.UNAUTHORIZED_ACTION_FOR_CALLING_USER));


            User newUser = _mapper.Map<User>(inputDto);

            // TODO Image logic
            byte[] imageBytes;
            //using (var memoryStream = new MemoryStream())
            //{
            //    await inputDto.File.CopyToAsync(memoryStream);
            //    imageBytes = memoryStream.ToArray();
            //}

            //var imageFilePath = await _imageService.UploadFileToBlob(inputDto.File.FileName, imageBytes, "mimeType");

            //if (!string.IsNullOrEmpty(imageFilePath)) {
            //    newUser.ImagePath = imageFilePath;
            //}

            await _userRepository.UpdateUserAsync(newUser).ConfigureAwait(false);

            return BuildOperationResultDto(new VoidOutputDto());
        }

        private List<ErrorDto> ValidateInput(UserDto inputDto)
        {
            List<ErrorDto> validationsErros = new List<ErrorDto>();

            //if (string.IsNullOrEmpty(inputDto.Email))
            //    validationsErros.Add(new ErrorDto(ErrorCodes.REQUIRED_FILED_IS_EMPTY, nameof(inputDto.Email)));
            // else if (!ValidationHelper.IsValidEmail(inputDto.Email))
            //     validationErrors.Add(new ErrorDto(BusinessErrorCodes.FAILD_EMAIL_VALIDATION, nameof(inputDto.Email)));

            //if (string.IsNullOrEmpty(inputDto.UserName))
            //    validationsErros.Add(new ErrorDto(ErrorCodes.REQUIRED_FILED_IS_EMPTY, nameof(inputDto.UserName)));


            //if (string.IsNullOrEmpty(inputDto.Password))
            //    validationsErros.Add(new ErrorDto(ErrorCodes.REQUIRED_FILED_IS_EMPTY, nameof(inputDto.Password)));

            //if (!ValidationHelper.IsValidPassword(inputDto.Password))
            //     validationErrors.Add(new ErrorDto(BusinessErrorCodes.FAILD_PASSWORD_COMPLEXITY, nameof(inputDto.Password)));


            //if (string.IsNullOrEmpty(inputDto.Name))
            //    validationsErros.Add(new ErrorDto(ErrorCodes.REQUIRED_FILED_IS_EMPTY, nameof(inputDto.Name)));

            return validationsErros;
        }
    }
}
