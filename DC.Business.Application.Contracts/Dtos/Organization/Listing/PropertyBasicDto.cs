using DC.Business.Application.Contracts.Dtos.Enums;
using System;
namespace DC.Business.Application.Contracts.Dtos.Organization.Listing
{
    public class PropertyBasicDto
    {
        public long? Id { get; set; }
        public long? MySqlId { get; set; }
        public long UserId { get; set; }
        public long Price { get; set; }
        public long NetAream2 { get; set; }
        public string Typology { get; set; }
        public long NumberOfBathrooms { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string MainPhotoUrl { get; set; }

        public PropertyStateDto State { get; set; }
    }
}
