using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstates.Services.ViewModels
{
    public class DistrictViewModel
    {
        public string Name { get; set; }

        public int MinValue { get; set; }

        public int MaxValue { get; set; }

        public int TotalEstates { get; set; }
    }
}
