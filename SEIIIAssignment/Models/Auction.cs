using System;
using System.Collections.Generic;

#nullable disable

namespace SEIIIAssignment.Models
{
    public partial class Auction
    {
        public Auction()
        {
            Bids = new HashSet<Bid>();
        }

        public int AuctionId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? PostedbyId { get; set; }
        public int? BoughtbyId { get; set; }
        public int? ItemId { get; set; }
        public double? SellingAmount { get; set; }

        public virtual User Boughtby { get; set; }
        public virtual User Postedby { get; set; }
        public virtual ICollection<Bid> Bids { get; set; }
    }
}