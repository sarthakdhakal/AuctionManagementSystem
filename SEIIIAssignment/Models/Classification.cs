using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable

namespace SEIIIAssignment.Models
{
    public partial class Classification
    {
        public Classification()
        {
            Items = new HashSet<Item>();
        }

        public int ClassificationId { get; set; }
        [DisplayName("Classification Name")]
        public string ClassificationName { get; set; }

        public virtual ICollection<Item> Items { get; set; }
    }
}
