using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Baker_Point.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        [Required]
        public int BlogsId { get; set; }
        [Required]
        public int UserId { get; set; }
        public virtual UserProfile User { get; set; }
        [Required]
        public DateTime PostTime { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        public string Context { get; set; }
    }
}