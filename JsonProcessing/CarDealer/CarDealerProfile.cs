using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using CarDealer.Data;
using CarDealer.DTO;
using CarDealer.Models;

namespace CarDealer
{
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            this.CreateMap<CarsInsertDTO, Car>();

            this.CreateMap<Customer, OrderedCustomersDTO>();

            this.CreateMap<Car, CarsFromToyotaDTO>();

            this.CreateMap<Supplier, LocalSuppliersDTO>()
                .ForMember(x => x.PartsCount, y => y.MapFrom(x => x.Parts.Count));

            this.CreateMap<Customer, CustomerTotalSalesDTO>()
                .ForMember(x => x.FullName, y => y.MapFrom(x => x.Name))
                .ForMember(x => x.BoughtCars, y => y.MapFrom(x => x.Sales.Count))
                .ForMember(x => x.SpentMoney, y => y.MapFrom(x => x.Sales.Select(s => s.Car.PartCars.Select(pc => pc.Part).Sum(p => p.Price)).Sum()));

            this.CreateMap<Sale, SalesDiscountDTO>()
                .ForMember(x => x.CustomerName, y => y.MapFrom(x => x.Customer.Name))
                .ForMember(x => x.Discount, y => y.MapFrom(x => x.Discount))
                .ForMember(x => x.Price, y => y.MapFrom(x => x.Car.PartCars.Sum(pc => pc.Part.Price)));
            
        }
    }
}
