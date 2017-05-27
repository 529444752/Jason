﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Baker_Point.Models
{
    public class Blogs
    {
        public int BlogsId { get; set; }
        [Required]
        public string BlogTitle { get; set; }
        [Required]
        public int UserId { get; set; }
        public virtual UserProfile User { get; set; }
        public int Comments { get; set; }
        [Required]
        public DateTime PostTime { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        public string Context { get; set; }

        public string titleImg { get; set; }
        public bool IsFeatured { get; set; }

        public virtual ICollection<FeaturedList> featured { get; set; }
    }

    public class FeaturedList
    {
        public int FeaturedListId { get; set; }
        public int BlogsId { get; set; }
        public virtual Blogs blog { get; set; }
    }
}