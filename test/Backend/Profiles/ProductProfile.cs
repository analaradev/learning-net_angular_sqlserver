using AutoMapper;
using Backend.Dtos;
using Backend.Models;

namespace Backend.Profiles;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductDto>();
        CreateMap<Product, ProductDetailDto>();
    }
}
