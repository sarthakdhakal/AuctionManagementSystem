using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable

namespace SEIIIAssignment.Models
{
    public partial class Item
    {
        public Item()
        {
            Bids = new HashSet<Bid>();
        }

        public int ItemId { get; set; }
        [DisplayName("Produced Year")]
        public int? ProducedYear { get; set; }
        [DisplayName("Textual Description")]
        public string TextualDescription { get; set; }
        public DateTime? CreatedAt { get; set; }
       
        public string Artist { get; set; }
        public string Material { get; set; }
        public double? Weight { get; set; }
        public double? Height { get; set; }
        public double? Length { get; set; }
        public string Medium { get; set; }
        [DisplayName("Framed?")]
        public int? IsFramed { get; set; }
        public int? Width { get; set; }
        [DisplayName("Product Name")]
        public string ProductName { get; set; }
        [DisplayName("Category Name")]
        public int? CategoryId { get; set; }
        [DisplayName("Classification Name")]
        public int? ClassificationId { get; set; }
        public string Image { get; set; }
        [DisplayName("Auction Date")]
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? PostedbyId { get; set; }
        public int? BoughtbyId { get; set; }
        public double? SellingAmount { get; set; }
        [DisplayName("Initial Price")]
        public double? EstimatedAmount { get; set; }
        public int? ArchiveStatus { get; set; }
        [DisplayName("Image Type")]
        public string ImageType { get; set; }
        public virtual User Boughtby { get; set; }
        public virtual Category Category { get; set; }
        public virtual Classification Classification { get; set; }
        public virtual User Postedby { get; set; }
        public virtual ICollection<Bid> Bids { get; set; }
    }
}
