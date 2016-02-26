using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Models.Entities
{
    public class Type : BaseEntity
    {
        [Required(ErrorMessage ="Enter the type name")]
        [Display(Name ="Name")]
        public string Name { get; set; }
        public virtual Guid ParentTypeId { get; set; }
        [Display(Name = "Parent type")]
        public virtual Type ParentType { get; set; }
        //public virtual Guid ImageId { get; set; }
        public virtual Image Image { get; set; }
        public virtual ICollection<Property> Properties { get; set; }
        public virtual ICollection<Item> Items { get; set; }
        public Type() : base()
        {
            Properties = new HashSet<Property>();
            Items = new HashSet<Item>();
            ParentTypeId = Guid.Empty;
        }

    }
}