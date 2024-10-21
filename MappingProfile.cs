using AutoMapper;
using ProductManagementAPI.DTOs;
using ProductManagementAPI.Models;

namespace ProductManagementAPI
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ProductDto, Product>();
            CreateMap<ProductCategoryDto, ProductCategory>();
            CreateMap<Product, ProductDto>();
            CreateMap<ProductCategory, ProductCategoryDto>();
        }
    }
}
