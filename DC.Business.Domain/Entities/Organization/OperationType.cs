using System;
namespace DC.Business.Domain.Entities.Organization
{
    public class OperationType
    {
        public long Id { get; set; }
        public string Type { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime Deleted { get; set; }
    }
}
