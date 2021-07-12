using DC.Business.Application.Contracts.Dtos.Image;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DC.Business.Application.Contracts.Interfaces.Services
{
    public interface IImageService
    {
        Task<List<string>> UploadPropertyImagesToBlob(List<IFormFile> photos, long propertyId);

        Task DeletePropertyProfileImages(List<string> imageUrls, string propertyElasticId, long propertyMySqlId);
        Task<string> UploadUserImageProfileImageToBlob(IFormFile photo, string userEmail);
        Task DeleteUserProfileImage(UserProfileImageDto input);
    }
}
