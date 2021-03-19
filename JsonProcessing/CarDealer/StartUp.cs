using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CarDealer.Data;
using CarDealer.DTO;
using CarDealer.Models;
using Newtonsoft.Json;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            Mapper.Initialize(cfg => cfg.AddProfile(new CarDealerProfile()));

            var carDealerContext = new CarDealerContext();
            //SetUpDatabase(carDealerContext);

            //string inputJson = File.ReadAllText("../../../Datasets/sales.json");

            string result = GetSalesWithAppliedDiscount(carDealerContext);

            Console.WriteLine(result);


        }

        private static void SetUpDatabase(CarDealerContext carDealerContext)
        {
            carDealerContext.Database.EnsureDeleted();
            carDealerContext.Database.EnsureCreated();
            Console.WriteLine("Database has been successfully created");
        }

        // Task 1
        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            Supplier[] suppliers = JsonConvert.DeserializeObject<Supplier[]>(inputJson);

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Length}.";
        }

        // Task 2

        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            var suppliers = context.Suppliers.Select(s => s.Id).ToArray();

            Part[] parts = JsonConvert.DeserializeObject<Part[]>(inputJson)
                .Where(p => suppliers.Contains(p.SupplierId)).ToArray();

            context.Parts.AddRange(parts);
            context.SaveChanges();

            return $"Successfully imported {parts.Length}.";
        }

        // Task 3

        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            CarsInsertDTO[] cars = JsonConvert.DeserializeObject<CarsInsertDTO[]>(inputJson).ToArray();

            List<Car> mappedCars = new List<Car>();

            foreach (var car in cars)
            {
                Car vehicle = Mapper.Map<CarsInsertDTO, Car>(car);
                mappedCars.Add(vehicle);

                var partIds = car.PartsId.Distinct().ToList();

                if (partIds == null)
                {
                    continue;
                }

                foreach (var id in partIds)
                {
                    PartCar partCar = new PartCar
                    {
                        PartId = id,
                        Car = vehicle
                    };

                    vehicle.PartCars.Add(partCar);
                }


            }

            context.Cars.AddRange(mappedCars);
            context.SaveChanges();

            return $"Successfully imported {mappedCars.Count}.";
        }

        // Task 4

        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            Customer[] customers = JsonConvert.DeserializeObject<Customer[]>(inputJson).ToArray();

            context.Customers.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {customers.Length}.";
        }

        // Task 5

        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            Sale[] sales = JsonConvert.DeserializeObject<Sale[]>(inputJson).ToArray();

            context.Sales.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Length}.";
        }

        // Task 6

        public static string GetOrderedCustomers(CarDealerContext context)
        {
            var orderedCustomers = context.Customers
                .ProjectTo<OrderedCustomersDTO>()
                .OrderBy(c => c.BirthDate)
                .ThenBy(c => c.IsYoungDriver);

            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                DateFormatString = "dd/MM/yyyy",
                Formatting = Formatting.Indented
            };
            string result = JsonConvert.SerializeObject(orderedCustomers, settings);

            return result;
        }

        // Task 7

        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {
            var cars = context.Cars
                .Where(c => c.Make == "Toyota")
                .ProjectTo<CarsFromToyotaDTO>()
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TravelledDistance);

            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented
            };

            string result = JsonConvert.SerializeObject(cars, settings);

            return result;

        }

        // Task 8

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var suppliers = context.Suppliers
                .Where(s => s.IsImporter == false)
                .ProjectTo<LocalSuppliersDTO>();

            string result = JsonConvert.SerializeObject(suppliers,Formatting.Indented);

            return result;
        }

        // Task 9

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {

            CarsDTO[] cars = context.Cars.Select(c => new CarsDTO()
            {
                Cars = new SingleCarDTO()
                {
                    Make = c.Make,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance
                },
                Parts = c.PartCars.Select(pc => new SinglePartDTO()
                {
                    Name = pc.Part.Name,
                    Price = $"{pc.Part.Price:f2}"
                })
                .ToArray()
            })
                .ToArray();

            string result = JsonConvert.SerializeObject(cars,Formatting.Indented);

            return result;
        }

        // Task 10
        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var customers = context.Customers
                .Where(c => c.Sales.Count() >= 1)
                .ProjectTo<CustomerTotalSalesDTO>()
                .OrderByDescending(c => c.SpentMoney)
                .ThenByDescending(c => c.BoughtCars);

            string result = JsonConvert.SerializeObject(customers,Formatting.Indented);

            return result;

        }

        // Task 11

        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var sales = context.Sales.Take(10).ProjectTo<SalesDiscountDTO>();

            string result = JsonConvert.SerializeObject(sales,Formatting.Indented);

            return result;
        }
    }
}