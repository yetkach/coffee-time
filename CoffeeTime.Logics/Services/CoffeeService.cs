using AutoMapper;
using CoffeeTime.Data.Entities;
using CoffeeTime.Data.Interfaces;
using CoffeeTime.Logics.Dto;
using CoffeeTime.Logics.Infrastructure;
using CoffeeTime.Logics.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeeTime.Logics.Services
{
    public class CoffeeService : ICoffeeService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IOrderService orderService;
        private readonly IMapper mapper;

        public CoffeeService(IUnitOfWork unitOfWork, IOrderService orderService, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.orderService = orderService;
            this.mapper = mapper;
        }

        public async Task<List<CoffeeDataDto>> GetAllCoffeesDataAsync()
        {
            List<CoffeeData> allCoffeesData = await unitOfWork.Coffees.GetAllCoffeeDataAsync();
            var coffeeDataDtos = mapper.Map<List<CoffeeDataDto>>(allCoffeesData);

            return coffeeDataDtos;
        }

        public async Task<CoffeeDataDto> GetCoffeeDataAsync(string name)
        {
            CoffeeData coffeeData = await unitOfWork.Coffees.GetCoffeeDataAsync(name);

            if (coffeeData == null)
            {
                throw new NotFoundException();
            }

            CoffeeDataDto coffeeDataDto = mapper.Map<CoffeeDataDto>(coffeeData);
            return coffeeDataDto;
        }

        public async Task CreateNewCoffeeAsync(CoffeeDto coffeeDto)
        {
            var coffeeData = await unitOfWork.Coffees.GetCoffeeDataAsync(coffeeDto.Name);

            if (coffeeData == null)
            {
                throw new NotFoundException();
            }

            var volumeData = coffeeData.Volumes.FirstOrDefault(data => data.Volume == coffeeDto.Volume);

            if (volumeData == null)
            {
                throw new NotFoundException();
            }

            var price = volumeData.PriceData;
            var order = await orderService.GetUnpaidOrderAsync();

            if (order == null)
            {
                order = await orderService.CreateNewOrderAsync();
            }

            Coffee coffee = mapper.Map<Coffee>(coffeeDto);
            coffee.Price = price.Price;
            coffee.OrderId = order.Id;

            await unitOfWork.Coffees.AddAsync(coffee);
            await unitOfWork.CompleteAsync();
        }
    }
}
