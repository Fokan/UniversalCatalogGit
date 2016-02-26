using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class PageInfo
    {
        public int Number { get; set; }
        public int Size { get; set; }
        public int TotalSize { get; set; }
        public int TotalPages
        {
            get
            {
                return (int)Math.Ceiling((decimal)TotalSize / Size);
            }
        }
    }
}