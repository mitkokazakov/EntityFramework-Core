using AutoMapper.QueryableExtensions;
using RealEstates.Data;
using RealEstates.Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RealEstates.Services
{
    public class DistrictsService : AutoMapperConfig, IDistrictsService
    {
        private RealEstateDbContext db;
        public DistrictsService(RealEstateDbContext db)
        {
            this.db = db;
        }

        public ICollection<DistrictViewModel> DistrictWithNewestEstates()
        {
            var districts = this.db.Districts
                .Where(d => d.Properties.Any(p => p.Year > 2017))
                .ProjectTo<DistrictViewModel>(this.Mapper.ConfigurationProvider)
                .OrderByDescending(d => d.TotalEstates)
            .ThenBy(d => d.Name).ToList();

            return districts;
        }

        public ICollection<DistrictViewModel> InfoForDistrict()
        {
            var districts = this.db.Districts
                .ProjectTo<DistrictViewModel>(this.Mapper.ConfigurationProvider)
                .OrderByDescending(d => d.TotalEstates)
            .ThenBy(d => d.Name).ToList();

            return districts;
        }
    }
}
