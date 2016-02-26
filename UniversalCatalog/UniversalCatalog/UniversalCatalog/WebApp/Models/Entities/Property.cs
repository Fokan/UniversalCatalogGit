using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Models.Entities
{
    public class Property : BaseEntity
    {
        [Required(ErrorMessage ="Enter the property name")]
        [Display(Name ="Name")]
        public string Name { get; set; }
        public virtual ICollection<Type> Types { get; set; }
        public virtual ICollection<Description> Descriptions { get; set; }
        public Property() : base()
        {
            Descriptions = new HashSet<Description>();
            Types = new HashSet<Type>();
        }
    }
}