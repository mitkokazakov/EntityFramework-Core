using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using RealEstates.Data;
using RealEstates.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace RealEstates.Importer
{
    class Program
    {
       
        static void Main(string[] args)
        {
            //Console.OutputEncoding = Encoding.UTF8;

            RealEstateDbContext context = new RealEstateDbContext();
            IPropertiesService propertiesService = new PropertyService(context);

            string inputJson = File.ReadAllText("imot.bg-raw-data-2021-03-18.json", Encoding.UTF8);


            ICollection<InputJsonEstates> estates = JsonConvert.DeserializeObject<ICollection<InputJsonEstates>>(inputJson);

            var est = estates.Skip(1);

            foreach (var e in est)
            {
                var district = e.District;
                var size = e.Size;
                var floor = e.Floor;
                var maxFloors = e.TotalFloors;
                var year = e.Year;
                var price = e.Price;
                var propertyType = e.Type;
                var buildingType = e.BuildingType;

                propertiesService.Create(district,size,floor,maxFloors,year,propertyType,buildingType,price);
                //Console.WriteLine($"{e.BuildingType} ---- {e.Type}");
            }

            //Console.WriteLine(inputJson);
        }
    }
}
