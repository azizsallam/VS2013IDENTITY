using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Basha.Models
{
    public class flowerDisc
    {   [Key]
        public int flowerDiscId { get; set; }
        [Required]
        public int FlowerId { get; set; }
        [Display(Name = "warda")]
        public virtual Flower FlowerName { get; set; }
        public string fcolor { get; set; }

    }
}