using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P01_HospitalDatabase.Data.Models
{
    public class PatientMedicament
    {
        [Required]
        [ForeignKey("Patient")]
        public int PatientId { get; set; }

        [Required]
        public virtual Patient Patient { get; set; }

        [Required]
        [ForeignKey("Medicament")]
        public int MedicamentId { get; set; }

        [Required]
        public virtual Medicament Medicament { get; set; }
    }
}
