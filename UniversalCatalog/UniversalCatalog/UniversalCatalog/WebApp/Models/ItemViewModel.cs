using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebApp.Models.Entities;

namespace WebApp.Models
{
    public class ItemViewModel
    {
        //public Guid Id { get; set; }
        [Required(ErrorMessage = "Enter the product name")]
        public string Name { get; set; }
        public Guid TypeId { get; set; }
        public Entities.Type Type { get; set; }
        [Required(AllowEmptyStrings =true)]
        public decimal Price { get; set; }
        public Guid[] IdsOfProps { get; set; }
        public string[] props { get; set; }

        public IEnumerable<HttpPostedFileBase> Images { get; set; }
    }
}