using System;
using System.Collections.Generic;

#nullable disable

namespace SEIIIAssignment.Models
{
    public partial class Item
    {
        public Item()
        {
            Auctions = new HashSet<Auction>();
        }

        public int ItemId { get; set; }
        public int? ProducedYear { get; set; }
        public string TextualDescription { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string Artist { get; set; }
        public string ItemType { get; set; }
        public string Material { get; set; }
        public double? Weight { get; set; }
        public double? Height { get; set; }
        public double? Length { get; set; }
        public string Medium { get; set; }
        public bool? IsFramed { get; set; }
        public string Type { get; set; }
        public string ProductName { get; set; }
        public int? CategoryId { get; set; }
        public int? ClassificationId { get; set; }
        public string Image { get; set; }

        public virtual Category Category { get; set; }
        public virtual Classification Classification { get; set; }
        public virtual ICollection<Auction> Auctions { get; set; }
    }
}
