using System;
using System.Collections.Generic;
using System.Text;

namespace DC.Business.Domain.ElasticEnteties
{
    public class PropertyImage
    {
        public ulong MySqlId { get; set; }
        public string ImageUrl { get; set; }

        public string ImageName { get; set; }
    }
}
