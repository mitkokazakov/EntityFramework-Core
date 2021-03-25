using RealEstates.Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstates.Services
{
    public interface IPropertiesService
    {
        void Create(string district,int size,int? floor,int? maxFloors,int? year, string propertyType,string buildingType,int price);

        IEnumerable<PropertyViewModel> SearchByPrice(int minPrice, int maxPrice);

        IEnumerable<PropertyViewModel> SearchBySizeAndYear(int minYear, int maxYear, int minSize, int mazSize);

        void UpdateTags(int id);
    }
}
