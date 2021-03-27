using AutoMapper.QueryableExtensions;
using RealEstates.Data;
using RealEstates.Models;
using RealEstates.Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RealEstates.Services
{
    public class PropertyService : AutoMapperConfig, IPropertiesService
    {
        private RealEstateDbContext db;
        public PropertyService(RealEstateDbContext db)
        {
            this.db = db;
        }
        public void Create(string district, int size, int? floor, int? maxFloors, int? year, string propertyType, string buildingType, int price)
        {
            Estate estate = new Estate()
            {
                Size = size,
                Floor = floor,
                TotalFloors = maxFloors,
                Year = year,
                Price = price
            };

            if (estate.Floor == 0)
            {
                estate.Floor = null;
            }

            if (estate.TotalFloors == 0)
            {
                estate.TotalFloors = null;
            }

            if (estate.Year == 0)
            {
                estate.Year = null;
            }

            District district1 = this.db.Districts.FirstOrDefault(d => d.Name.Trim() == district.Trim());

            if (district1 == null)
            {
                district1 = new District()
                {
                    Name = district
                };
            }

            TypeProperty typeProperty = this.db.TypesProperty.FirstOrDefault(t => t.Name.Trim() == propertyType);

            if (typeProperty == null)
            {
                typeProperty = new TypeProperty()
                {
                    Name = propertyType
                };
            }

            TypeBuilding typeBuilding = this.db.TypesBuilding.FirstOrDefault(b => b.Name.Trim() == buildingType.Trim());

            if (typeBuilding == null)
            {
                typeBuilding = new TypeBuilding()
                {
                    Name = buildingType
                };
            }

            estate.District = district1;
            estate.TypeBuilding = typeBuilding;
            estate.TypeProperty = typeProperty;

            this.db.Properties.Add(estate);
            this.db.SaveChanges();

            this.UpdateTags(estate.Id);
        }

        public IEnumerable<PropertyViewModel> SearchByPrice(int minPrice, int maxPrice)
        {
            var properties = this.db.Properties
                .Where(p => p.Price >= minPrice && p.Price <= maxPrice)
                .ProjectTo<PropertyViewModel>(this.Mapper.ConfigurationProvider)
                .OrderBy(x => x.Price)
                .ToList();

            return properties;
        }

        public IEnumerable<PropertyViewModel> SearchBySizeAndYear(int minYear, int maxYear, int minSize, int maxSize)
        {
            var properties = this.db.Properties
                .Where(p => p.Year >= minYear && p.Year <= maxYear && p.Size >= minSize && p.Size <= maxSize)
                .ProjectTo<PropertyViewModel>(this.Mapper.ConfigurationProvider)
                .OrderBy(x => x.Price)
                .ToList();

            return properties;
        }

        public void UpdateTags(int id)
        {
            Estate propertyToUpdate = this.db.Properties.FirstOrDefault(p => p.Id == id);

            Tag tag = null;

            if (propertyToUpdate.Year.HasValue && propertyToUpdate.Year >= 2018)
            {
                tag = this.db.Tags.FirstOrDefault(t => t.Name == "HasParkingLot");

                if (tag == null)
                {
                    tag = new Tag() { Name = "HasParkingLot" };
                }

                propertyToUpdate.Tags.Add(new PropertiesTags()
                {
                    Tag = tag
                });

            }

            if (propertyToUpdate.Year.HasValue && propertyToUpdate.Year <= 1990)
            {
                tag = this.db.Tags.FirstOrDefault(t => t.Name == "OldBuilding");

                if (tag == null)
                {
                    tag = new Tag() { Name = "OldBuilding" };
                }

                propertyToUpdate.Tags.Add(new PropertiesTags()
                {
                    Tag = tag
                });

            }

            if (propertyToUpdate.Size > 120)
            {
                tag = this.db.Tags.FirstOrDefault(t => t.Name == "HugeEstate");

                if (tag == null)
                {
                    tag = new Tag() { Name = "HugeEstate" };
                }

                propertyToUpdate.Tags.Add(new PropertiesTags()
                {
                    Tag = tag
                });

            }

            if (propertyToUpdate.Price > 200000)
            {
                tag = this.db.Tags.FirstOrDefault(t => t.Name == "ExpensiveEstate");

                if (tag == null)
                {
                    tag = new Tag() { Name = "ExpensiveEstate" };
                }

                propertyToUpdate.Tags.Add(new PropertiesTags()
                {
                    Tag = tag
                });

            }

            if (propertyToUpdate.Price < 50000)
            {
                tag = this.db.Tags.FirstOrDefault(t => t.Name == "CheapEstate");

                if (tag == null)
                {
                    tag = new Tag() { Name = "CheapEstate" };
                }

                propertyToUpdate.Tags.Add(new PropertiesTags()
                {
                    Tag = tag
                });

            }

            this.db.SaveChanges();
        }
    }
}
