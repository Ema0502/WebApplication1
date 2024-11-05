using AutoMapper;
using WebApplication1.Models;
using WebApplication1.Dtos;

namespace WebApplication1.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDTO>();
            CreateMap<ProductDTO, Product>();
        }
    }
}
