using System;
using System.Collections.Generic;

#nullable disable

namespace SEIIIAssignment.Models
{
    public partial class User
    {
        public User()
        {
            AuctionBoughtbies = new HashSet<Auction>();
            AuctionPostedbies = new HashSet<Auction>();
            Bids = new HashSet<Bid>();
        }

        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int? Role { get; set; }
        public string Password { get; set; }

        public virtual ICollection<Auction> AuctionBoughtbies { get; set; }
        public virtual ICollection<Auction> AuctionPostedbies { get; set; }
        public virtual ICollection<Bid> Bids { get; set; }
    }
}
