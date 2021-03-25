using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace RealEstates.Models
{
    public class Estate
    {
        public Estate()
        {
            this.Tags = new HashSet<PropertiesTags>();
        }
        public int Id { get; set; }

        public int Size { get; set; }

        public int? Floor { get; set; }

        public int? TotalFloors { get; set; }

        public int DistrictId { get; set; }
        public virtual District District { get; set; }

        public int? Year { get; set; }

        public int TypePropertyId { get; set; }
        public virtual TypeProperty TypeProperty { get; set; }

        public int TypeBuildingId { get; set; }
        public virtual TypeBuilding TypeBuilding { get; set; }

        public int Price { get; set; }

        public virtual ICollection<PropertiesTags> Tags { get; set; }
    }
}
