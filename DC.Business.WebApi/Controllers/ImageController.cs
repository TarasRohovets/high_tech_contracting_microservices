using DC.Business.Application.Contracts.Dtos.Image;
using DC.Business.Application.Contracts.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DC.Business.WebApi.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    [Authorize]
    public class ImageController : ControllerBase
    {
        private readonly IImageService _imageService;

        public ImageController(IImageService imageService)
        {
            _imageService = imageService ?? throw new ArgumentNullException(nameof(imageService));
        }

        #region User profile

        [HttpPost]
        [Route("updateUserProfileImage")]
        public async Task<IActionResult> UpdateUserProfileImage([FromForm] IFormFile photo, [FromForm] string userEmail, CancellationToken cancellationToken = default)
        {
            var result = await _imageService.UploadUserImageProfileImageToBlob(photo, userEmail);
            return Ok(result);
        }

        [HttpDelete]
        [Route("deleteUserProfileImage")]
        public async Task<IActionResult> DeleteUserProfileImage([FromBody] UserProfileImageDto input, CancellationToken cancellationToken = default)
        {
            await _imageService.DeleteUserProfileImage(input);
            return Ok();
        }

        #endregion

        #region Property photos

        [HttpPost]
        [Route("addUserPropertyImages")]
        public async Task<IActionResult> AddUserPropertyImages([FromForm]  List<IFormFile> files, [FromForm] long propertyId, CancellationToken cancellationToken = default)
        {
            var result = await _imageService.UploadPropertyImagesToBlob(files, propertyId);
            return Ok();
        }

        [HttpDelete]
        [Route("deleteUserPropertyImages")]
        public async Task<IActionResult>  DeleteImages(List<string> imageIds, string propertyElasticId, long propertyMySqlId, CancellationToken cancellationToken = default)
        {
            await _imageService.DeletePropertyProfileImages(imageIds, propertyElasticId, propertyMySqlId);
            return Ok();
        }

        #endregion
    }
}
