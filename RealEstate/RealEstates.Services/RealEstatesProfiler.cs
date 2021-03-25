using AutoMapper;
using RealEstates.Models;
using RealEstates.Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RealEstates.Services
{
    public class RealEstatesProfiler : Profile
    {
        public RealEstatesProfiler()
        {
            this.CreateMap<Estate, PropertyViewModel>()
                .ForMember(x => x.District, y => y.MapFrom(x => x.District.Name))
                .ForMember(x => x.BuildingType, y => y.MapFrom(x => x.TypeBuilding.Name))
                .ForMember(x => x.PropertyType, y => y.MapFrom(x => x.TypeProperty.Name))
                .ForMember(x => x.Floor, y => y.MapFrom(x => (x.Floor ?? 0).ToString() + "/" + (x.TotalFloors ?? 0).ToString()));

            this.CreateMap<District, DistrictViewModel>()
                .ForMember(x => x.MinValue, y => y.MapFrom(x => x.Properties.Min(p => p.Price)))
                .ForMember(x => x.MaxValue, y => y.MapFrom(x => x.Properties.Max(p => p.Price)))
                .ForMember(x => x.TotalEstates, y => y.MapFrom(x => x.Properties.Count()));
        }
    }
}
