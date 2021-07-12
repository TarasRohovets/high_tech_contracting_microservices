using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DC.Business.Domain.Repositories.Organization
{
    public interface IImageRepository
    {
        Task AddPropertyPhoto(long propertyId, string imageName, string imagePath);

        Task DeletePropertyPhoto(string imagePath, long propertyId);
    }
}
