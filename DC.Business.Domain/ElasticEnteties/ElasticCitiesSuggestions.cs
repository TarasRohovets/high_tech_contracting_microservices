using System;
using System.Collections.Generic;
using System.Text;

namespace DC.Business.Domain.ElasticEnteties
{
    public class ElasticCitiesSuggestions
    {
        public string Id { get; set; }
        public string Title { get; set; }
        // public string[] Keywords { get; set; }
        public string[] Suggestions { get; set; }
        public DateTime IndexedOn { get; set; }
    }
}
