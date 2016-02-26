using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Models.Entities
{
    public class Comment : BaseEntity
    {
        [Required]
        public virtual ApplicationUser User { get; set; }
        [Required]
        public virtual Item Item { get; set; }
        
        public DateTime Date { get; set; }
        [Required]
        public string Value { get; set; }

        public Comment() : base()
        {

        }
    }
}