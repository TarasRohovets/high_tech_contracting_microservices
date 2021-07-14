using DC.Business.Application.Contracts.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage;
using System.IO;
using DC.Business.Application.Contracts.Dtos.Image;
using Microsoft.AspNetCore.Http;
using DC.Business.Domain.Repositories.Organization;
using System.Linq;
using DC.Business.Domain.Repositories.ElasticSearch;
using DC.Business.Domain.ElasticEnteties;

namespace DC.Business.Application.Services.Services
{
    public class ImageService : IImageService
    {
        private readonly IUserRepository _userRepository;
        private readonly IListingRepository _listingRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IPropertiesElasticRepository _propertiesElasticRepository;
        protected const string _connectionStringKey = "DC.AzureBlobStorage";
        private string _connectionString = string.Empty;
        private BlobServiceClient _blobServiceClient;
        private string _strContainerName = "images";
        private BlobContainerClient _containerClient;

        //private string blobUrlName = string.Empty;
        //private string accountKey = string.Empty;
        //protected const string _blobUrlName = "ImageBlobName";
        //protected const string _accountKey = "AccountKeyImage";

        private IConfigurationSection azureSection ;

        public ImageService(IConfiguration configuration, IUserRepository userRepository, IListingRepository listingRepository, IImageRepository imageRepository, IPropertiesElasticRepository propertiesElasticRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _listingRepository = listingRepository ?? throw new ArgumentNullException(nameof(listingRepository));
            _imageRepository = imageRepository ?? throw new ArgumentNullException(nameof(imageRepository));
            _propertiesElasticRepository = propertiesElasticRepository ?? throw new ArgumentNullException(nameof(propertiesElasticRepository));
            _connectionString = configuration.GetConnectionString(_connectionStringKey);
            _blobServiceClient = new BlobServiceClient(_connectionString);
            azureSection = configuration.GetSection("Azure");
            _containerClient = _blobServiceClient.GetBlobContainerClient(_strContainerName);
            // blobUrlName = configuration.GetSection(_blobUrlName);
            // accountKey = configuration.GetConnectionString(_accountKey);
        }

        public async Task<List<string>> UploadPropertyImagesToBlob(List<IFormFile> photos, long propertyId)
        {
            //TODO validation
            try
            {
                var fileUrls = new List<string>();
                var propertyImages = new List<PropertyImage>();

                foreach(var photo in photos)
                {
                    var imageBytes = ConvertToBytes(photo);

                    var fileUrl = await UploadFileToBlobAsync(photo.FileName, imageBytes, "image/png");
                    // var fileUrl = "url1";
                    await AddPropertyImagePath(propertyId, photo.FileName, fileUrl);

                    fileUrls.Add(fileUrl);
                    propertyImages.Add(new PropertyImage() { ImageUrl = fileUrl, ImageName = photo.FileName });
                }
                await AddPropertyImagePathToElastic((int)propertyId, propertyImages);
                return fileUrls;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public async Task<string> UploadUserImageProfileImageToBlob(IFormFile photo, string userEmail)
        {
            //TODO validation
            try
            {
                var imageBytes = ConvertToBytes(photo);

                var fileUrl = await UploadFileToBlobAsync(photo.FileName, imageBytes, "image/png");

                await UpdateUserImagePath(userEmail, photo.FileName, fileUrl);

                return fileUrl;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public async Task DeletePropertyProfileImages(List<string> imageUrls, string propertyElasticId, long propertyMySqlId)
        {
            foreach(var imageUrl in imageUrls)
            {
                Uri uriObj = new Uri(imageUrl);

                string BlobName = Path.GetFileName(uriObj.LocalPath);

                var blob = _containerClient.GetBlobClient(BlobName);

                await blob.DeleteAsync();

                await DeletePropertyImagePath(imageUrl, propertyMySqlId);
            }
        }

        public async Task DeleteUserProfileImage(UserProfileImageDto input)
        {
            Uri uriObj = new Uri(input.ImageUrl);

            string BlobName = Path.GetFileName(uriObj.LocalPath);

            var blob = _containerClient.GetBlobClient(BlobName);
            
            await blob.DeleteAsync();

            await DeleteUserImagePath(input.ImageUrl, input.UserEmail);
        }

        private async Task AddPropertyImagePathToElastic(int propertyId, List<PropertyImage> propertyImages)
        {
            var property = await _propertiesElasticRepository.GetPropertyByMySqlId(propertyId);

            if (property != null)
            {
                var imagePaths = property?.Images;
                var listOfImages = imagePaths != null ? imagePaths : new List<Domain.ElasticEnteties.PropertyImage>();
                foreach(var photo in propertyImages)
                {
                    listOfImages.Add(photo);
                }
                await _propertiesElasticRepository.UpdatePropertyImagesByMySqlId(property._id, listOfImages);
            }
        }

        private async Task AddPropertyImagePath(long propertyId, string imageName, string imagePath)
        {
            var property = await _listingRepository.GetPropertyById(propertyId);

            if (property != null)
            {
                await _imageRepository.AddPropertyPhoto(property.Id, imageName, imagePath);
            }
        }

        private async Task UpdateUserImagePath(string userEmail, string imageName, string imagePath)
        {
            var currentUser = await _userRepository.GetUserByEmailAsync(userEmail);

            if(currentUser != null)
            {
                await _userRepository.UpdateUserProfilePhoto(currentUser.Id, imageName, imagePath);
            }
        }

        //TODO delete for elastic

        private async Task DeletePropertyImagePath(string imagePath, long propertyId)
        {
            var property = await _listingRepository.GetPropertyById(propertyId);

            if (property != null)
            {
                await _imageRepository.DeletePropertyPhoto(imagePath, property.Id);
            }
        }

        private async Task DeleteUserImagePath(string imagePath, string userEmail)
        {
            var currentUser = await _userRepository.GetUserByEmailAsync(userEmail);

            if (currentUser != null)
            {
                await _userRepository.DeleteUserProfilePhoto(currentUser.Id);
            }
        }

        private async Task<string> UploadFileToBlobAsync(string strFileName, byte[] fileData, string fileMimeType)
        {
            try
            {
                string fileName = this.GenerateFileName(strFileName);

                var blobUrlName = azureSection.GetSection("ImageBlobName");
                var accountKey = azureSection.GetSection("AccountKeyImage");

                // Create a URI to the blob
                Uri blobUri = new Uri("https://" +
                                      blobUrlName.Value +
                                      ".blob.core.windows.net/" +
                                      _strContainerName +
                                      "/" + fileName);

                StorageSharedKeyCredential storageCredentials =
                      new StorageSharedKeyCredential(blobUrlName.Value, accountKey.Value);

                if (fileName != null && fileData != null)
                {
                    BlobClient blobClient = new BlobClient(blobUri, storageCredentials);

                    MemoryStream stream = new MemoryStream(fileData); // TODO add using

                    await blobClient.UploadAsync(stream, true);

                    return blobUri.AbsoluteUri;
                }
                return "";
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private byte[] ConvertToBytes(IFormFile image)
        {
            byte[] CoverImageBytes = null;
            BinaryReader reader = new BinaryReader(image.OpenReadStream());
            CoverImageBytes = reader.ReadBytes((int)image.Length);
            return CoverImageBytes;
        }

        private string GenerateFileName(string fileName)
        {
            string strFileName = string.Empty;
            string[] strName = fileName.Split('.');
            strFileName = DateTime.Now.ToUniversalTime().ToString("yyyyMMdd\\THHmmssfff") + "." + strName[strName.Length - 1];
            return strFileName;
        }
    }
}
