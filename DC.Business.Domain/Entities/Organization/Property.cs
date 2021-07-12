using DC.Business.Domain.Enums;
using System;
namespace DC.Business.Domain.Entities.Organization
{
    public class Property
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long Price { get; set; }
        public long NetAream2 { get; set; }
        public long PriceNetAream2 { get; set; }
        public long GrossAream2 { get; set; }
        public string Typology { get; set; }
        public long Floor { get; set; }
        public long YearOfConstruction { get; set; }
        public long NumberOfBathrooms { get; set; }
        public string EnerergyCertificate { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool Deleted { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public PropertyState State {get;set;}

        public long PropertyTypeId { get; set; }
        public long OperationTypeId { get; set; }
        //public PropertyType propertyType { get; set; }
        //public OperationType operationType { get; set; }
    }
}
