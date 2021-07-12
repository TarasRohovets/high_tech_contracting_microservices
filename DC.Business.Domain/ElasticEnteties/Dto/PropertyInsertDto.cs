using DC.Business.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DC.Business.Domain.ElasticEnteties.Dto
{
    public class PropertyInsertDto
    {
        public long? Id { get; set; }
        public ulong MySqlId { get; set; }
        public long PropertyTypeId { get; set; }
        public long OperationTypeId { get; set; }
        public long UserId { get; set; }
        public long Price { get; set; }
        public long NetAream2 { get; set; }
        public long PriceNetAream2 { get; set; }
        public long GrossAream2 { get; set; }
        public string Typology { get; set; }
        public string OpeartionType { get; set; }
        public long Floor { get; set; }
        public long YearOfConstruction { get; set; }
        public long NumberOfBathrooms { get; set; }
        public string EnerergyCertificate { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public List<PropertyImage> Images { get; set; }
        public string[] Characteristics { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public PropertyState State { get; set; }
    }
}
