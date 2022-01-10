using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace SEIIIAssignment.Models
{
    public partial class Bid
    {
        public int BidId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? BidderId { get; set; }
        public int? ItemId { get; set; }
        [Required]
        public double? Amount { get; set; }

        public virtual User Bidder { get; set; }
        public virtual Item Item { get; set; }
    }
}
