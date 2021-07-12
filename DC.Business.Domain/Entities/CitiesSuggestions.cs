using System;
using System.Collections.Generic;
using System.Text;

namespace DC.Business.Domain.Entities
{
    public class CitiesSuggestions
    {
        // public int Id { get; set; }
        // public string Title { get; set; }
        // public string[] Keywords { get; set; }
        public string[] Suggestions { get; set; }
        public DateTime UploadedAt { get; set; }
        public DateTime? IndexedAt { get; set; }
    }
}
