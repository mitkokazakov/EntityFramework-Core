using AutoMapper;
using ProductShop.DTO;
using ProductShop.Models;
using System.Linq;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            this.CreateMap<Product, ProductRange>()
                .ForMember(x => x.Seller, y => y.MapFrom(x => x.Seller.FirstName + ' ' + x.Seller.LastName));

            this.CreateMap<Product, ProductInfoDTO>()
                .ForMember(x => x.BuyerFirstName, y => y.MapFrom(x => x.Buyer.FirstName))
                .ForMember(x => x.BuyerLastName, y => y.MapFrom(x => x.Buyer.LastName));

            this.CreateMap<User, UserSoldDTO>()
                .ForMember(x => x.SoldProducts, y => y.MapFrom(x => x.ProductsSold));

            this.CreateMap<Category, CategoriesDTO>()
                .ForMember(x => x.ProductsCount, y => y.MapFrom(x => x.CategoryProducts.Count))
                .ForMember(x => x.AveragePrice, y => y.MapFrom(x => $"{x.CategoryProducts.Average(p => p.Product.Price):f2}"))
                .ForMember(x => x.TotalRevenue, y => y.MapFrom(x => $"{x.CategoryProducts.Sum(p => p.Product.Price):f2}"));

            this.CreateMap<Product, SingleProductInfoDTO>();

            this.CreateMap<User, ProductCommonInfoDTO>()
                .ForMember(x => x.Count, y => y.MapFrom(x => x.ProductsSold.Count))
                .ForMember(x => x.Products, y => y.MapFrom(x => x.ProductsSold));

            this.CreateMap<User, UserPersonalInfoDTO>()
                .ForMember(x => x.SoldProducts, y => y.MapFrom(x => x.ProductsSold));


        }
    }
}
