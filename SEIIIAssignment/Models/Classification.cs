﻿using System;
using System.Collections.Generic;

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
        public string ClassificationName { get; set; }

        public virtual ICollection<Item> Items { get; set; }
    }
}