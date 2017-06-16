using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Baker_Point.Models
{
    public class Changelog
    {
        public int ChangelogId { get; set; }
        public string Type { get; set; }
        public int UserId { get; set; }
        public DateTime Time { get; set; }
        public int BlogsId { get; set; }
    }
}