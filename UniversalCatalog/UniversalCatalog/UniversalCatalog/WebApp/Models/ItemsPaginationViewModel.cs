using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Models.Entities;

namespace WebApp.Models
{
    public class ItemsPaginationViewModel
    {
        public IEnumerable<Item> Items { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}