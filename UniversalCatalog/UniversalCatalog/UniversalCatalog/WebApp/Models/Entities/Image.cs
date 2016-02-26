using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Models.Entities
{
    public class Image : BaseEntity
    {
        [Required]
        public byte[] Data { get; set; }
        [Required]
        public string ImageType { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Item> Item { get; set; }
        public Image() : base()
        {

        }
    }
}