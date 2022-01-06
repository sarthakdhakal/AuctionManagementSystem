using System;
using System.Collections.Generic;

namespace SEIIIAssignment.Models
{
    public class ItemAuctionViewModel
    {
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
        public int AuctionId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsActive { get; set; }
        
        public int? PostedbyId { get; set; }
        public int? BoughtbyId { get; set; }
      
        public double? SellingAmount { get; set; }

        public virtual User Boughtby { get; set; }
        public virtual User Postedby { get; set; }
        public virtual ICollection<Bid> Bids { get; set; }
        public virtual Category Category { get; set; }
        public virtual Classification Classification { get; set; }
    }
}