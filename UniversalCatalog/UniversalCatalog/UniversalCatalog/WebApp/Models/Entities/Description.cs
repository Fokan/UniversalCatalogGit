using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Models.Entities
{
    public class Description : BaseEntity
    {
        [Required]
        public string Value { get; set; }
        public virtual Property Property { get; set; }
        public virtual ICollection<Item> Items { get; set; }
        public Description() : base()
        {
            Items = new HashSet<Item>();
        }
    }
}