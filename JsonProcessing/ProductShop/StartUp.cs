using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.DTO;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            ProductShopContext context = new ProductShopContext();
            Mapper.Initialize(cfg => cfg.AddProfile(new ProductShopProfile()));

            //ResetDatabase(context);

            //string inputJSON = File.ReadAllText("../../../Datasets/categories-products.json");


            string result = GetUsersWithProducts(context);

            Console.WriteLine(result);

        }

        private static void ResetDatabase(ProductShopContext db)
        {
            db.Database.EnsureCreated();
            Console.WriteLine("Database was successfully created!");
        }

        // Task 1
        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            User[] users = JsonConvert.DeserializeObject<User[]>(inputJson);

            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Length}";
        }

        // Task 2
        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            Product[] products = JsonConvert.DeserializeObject<Product[]>(inputJson);

            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Length}";
        }

        // Task 3
        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            Category[] categories = JsonConvert.DeserializeObject<Category[]>(inputJson)
                .Where(c => c.Name != null).ToArray();

            context.Categories.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Length}";
        }

        // Task 4

        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            CategoryProduct[] categoryProducts = JsonConvert.DeserializeObject<CategoryProduct[]>(inputJson);

            context.CategoryProducts.AddRange(categoryProducts);
            context.SaveChanges();

            return $"Successfully imported {categoryProducts.Length}";
        }

        // Task 5

        public static string GetProductsInRange(ProductShopContext context)
        {
           
            ProductRange[] products = context.Products.ProjectTo<ProductRange>()
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .ToArray();

            string outputJSON = JsonConvert.SerializeObject(products,Formatting.Indented);

            return outputJSON;
        }

        // Task 6

        public static string GetSoldProducts(ProductShopContext context)
        {
            UserSoldDTO[] users = context.Users
                .Where(u => u.ProductsSold.Any(b => b.Buyer != null))
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .ProjectTo<UserSoldDTO>().ToArray();

            string result = JsonConvert.SerializeObject(users,Formatting.Indented);

            return result;
        }

        // Task 7

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            CategoriesDTO[] categories = context.Categories.ProjectTo<CategoriesDTO>().OrderByDescending(c => c.ProductsCount).ToArray();

            string result = JsonConvert.SerializeObject(categories,Formatting.Indented);

            return result;
        }


        // Task 8

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var users = context.Users.Where(u => u.ProductsSold.Any(b => b.Buyer != null)).ProjectTo<UserPersonalInfoDTO>();

            string result = JsonConvert.SerializeObject(users,Formatting.Indented);

            return result;
        }
    }
}