using AutoMapper;
using CoffeeTime.Logics.Dto;
using CoffeeTime.Web.Models;

namespace CoffeeTime.Web.Mapping
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<CoffeeDataDto, CoffeeForShowcaseViewModel>();
            CreateMap<CoffeeViewModel, CoffeeDto>();
            CreateMap<OrderDto, OrderViewModel>().ReverseMap();
            CreateMap<CoffeeDataDto, CoffeeViewModel>();
        }
    }
}
