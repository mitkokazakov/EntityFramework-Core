using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RealEstates.Models
{
    public class TypeProperty
    {
        public TypeProperty()
        {
            this.Properties = new HashSet<Estate>();
        }
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }

        public virtual ICollection<Estate> Properties { get; set; }
    }
}