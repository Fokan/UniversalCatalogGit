using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Models.Entities
{
    public class Item : BaseEntity
    {
        [Required(ErrorMessage = "Enter the product name")]
        [Display(Name="Name")]
        public string Name { get; set; }
        public virtual Guid TypeId { get; set; }
        [Display(Name ="Type")]
        public virtual Type Type { get; set; }
        [Display(Name="Price")]
        public decimal Price { get; set; }
        public virtual ICollection<Description> Descriptions { get; set; }
        public virtual ICollection<Image> Images { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public Item() : base()
        {
            Descriptions = new HashSet<Description>();
            Images = new HashSet<Image>();
            Comments = new HashSet<Comment>();
        }
    }
}