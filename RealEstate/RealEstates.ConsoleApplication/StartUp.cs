using Microsoft.EntityFrameworkCore;
using RealEstates.Data;
using RealEstates.Services;
using System;

namespace RealEstates.ConsoleApplication
{
    class StartUp
    {
        static void Main(string[] args)
        {
            
            RealEstateDbContext context = new RealEstateDbContext();

            //context.Database.Migrate();
            //Console.WriteLine("Database is set up!");

            IPropertiesService propertiesService = new PropertyService(context);
            IDistrictsService districtsService = new DistrictsService(context);

            Console.WriteLine("Welcome in Imot.bg!");
            Console.WriteLine("Choose what do you want to do:");
            Console.WriteLine("1. Find property by price.");
            Console.WriteLine("2. Find property by size and year.");
            Console.WriteLine("3. Find districts with the most new properties.");
            Console.WriteLine("4. Find general info about districts in Sofia.");

            Console.WriteLine();

            var option = Console.ReadLine();

            if (option == "1")
            {
                Console.Write("Enter min Price: ");
                int minPrice = int.Parse(Console.ReadLine());
                Console.Write("Enter max Price: ");
                int maxPrice = int.Parse(Console.ReadLine());

                var estates = propertiesService.SearchByPrice(minPrice, maxPrice);

                foreach (var est in estates)
                {
                    Console.WriteLine($"Квартал: {est.District}, Квадратура: {est.Size},Тип: {est.PropertyType}, Цена: {est.Price}, Етаж: {est.Floor}, Година: {est.Year}, Строителство: {est.BuildingType}");
                }
            }
            else if (option == "2")
            {
                int minSize = int.Parse(Console.ReadLine());
                int maxSize = int.Parse(Console.ReadLine());
                int minYear = int.Parse(Console.ReadLine());
                int maxYear = int.Parse(Console.ReadLine());

                var estates = propertiesService.SearchBySizeAndYear(minYear, maxYear,minSize,maxSize);

                foreach (var est in estates)
                {
                    Console.WriteLine($"Квартал: {est.District}, Квадратура: {est.Size},Тип: {est.PropertyType}, Цена: {est.Price}, Етаж: {est.Floor}, Година: {est.Year}, Строителство: {est.BuildingType}");
                }
            }
            else if (option == "3")
            {
                var newestDistricts = districtsService.DistrictWithNewestEstates();

                foreach (var d in newestDistricts)
                {
                    Console.WriteLine($"Квартал: {d.Name}, Мин. Цена: {d.MinValue}, Макс. Цена: {d.MaxValue}, Общо имоти: {d.TotalEstates}");
                }
            }
            else if (option == "4")
            {
                var globalDistricts = districtsService.InfoForDistrict();

                foreach (var d in globalDistricts)
                {
                    Console.WriteLine($"Квартал: {d.Name}, Мин. Цена: {d.MinValue}, Макс. Цена: {d.MaxValue}, Общо имоти: {d.TotalEstates}");
                }
            }

            
        }
    }
}
