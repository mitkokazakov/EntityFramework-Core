using RealEstates.Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstates.Services
{
    public interface IDistrictsService
    {
        ICollection<DistrictViewModel> InfoForDistrict();
        ICollection<DistrictViewModel> DistrictWithNewestEstates();
    }
}
