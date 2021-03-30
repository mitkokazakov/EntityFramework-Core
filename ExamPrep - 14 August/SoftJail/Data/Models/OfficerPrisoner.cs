using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SoftJail.Data.Models
{
    public class OfficerPrisoner
    {
        [Required]
        [ForeignKey("Officer")]
        public int OfficerId { get; set; }

        [Required]
        public virtual Officer Officer { get; set; }

        [Required]
        [ForeignKey("Prisoner")]
        public int PrisonerId { get; set; }

        [Required]
        public virtual Prisoner Prisoner { get; set; }
    }
}
