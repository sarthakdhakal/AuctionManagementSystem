using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable

namespace SEIIIAssignment.Models
{
    public partial class Category
    {
        public Category()
        {
            Items = new HashSet<Item>();
        }

        public int CategoryId { get; set; }
        [DisplayName("Category Name")]
        public string CategoryName { get; set; }

        public virtual ICollection<Item> Items { get; set; }
    }
}
