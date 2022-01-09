using System;
using System.Collections.Generic;

#nullable disable

namespace SEIIIAssignment.Models
{
    public partial class User
    {
        public User()
        {
            Bids = new HashSet<Bid>();
            ItemBoughtbies = new HashSet<Item>();
            ItemPostedbies = new HashSet<Item>();
        }

        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }

        public virtual ICollection<Bid> Bids { get; set; }
        public virtual ICollection<Item> ItemBoughtbies { get; set; }
        public virtual ICollection<Item> ItemPostedbies { get; set; }
    }
}
