using AutoMapper;
using CoffeeTime.Data.Entities;
using CoffeeTime.Logics.Dto;

namespace CoffeeTime.Logics.Services
{
    public class MapperConfigure : Profile
    {
        public MapperConfigure()
        {
            CreateMap<OrderDto, Order>().ReverseMap();
            CreateMap<Coffee, CoffeeDto>().ReverseMap();
            CreateMap<CoffeeData, CoffeeDataDto>();
            CreateMap<VolumeData, VolumeDataDto>();
            CreateMap<PriceData, PriceDataDto>();
        }
    }
}
