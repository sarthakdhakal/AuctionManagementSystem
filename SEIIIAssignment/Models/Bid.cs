using System;
using System.Collections.Generic;

#nullable disable

namespace SEIIIAssignment.Models
{
    public partial class Bid
    {
        public int BidId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? BidderId { get; set; }
        public int? AuctionId { get; set; }
        public double? Amount { get; set; }

        public virtual Auction Auction { get; set; }
        public virtual User Bidder { get; set; }
    }
}
