using AutoMapper;
using CoffeeTime.Data.EF;
using CoffeeTime.Data.Entities;
using CoffeeTime.Data.Interfaces;
using CoffeeTime.Data.Repositories;
using CoffeeTime.Logics.Dto;
using CoffeeTime.Logics.Infrastructure;
using CoffeeTime.Logics.Interfaces;
using CoffeeTime.Logics.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CoffeeTime.UnitTests
{
    public class CoffeeServiceTests
    {
        private readonly IMapper mapper;

        public CoffeeServiceTests()
        {
            mapper = new MapperConfiguration(config =>
                config.AddProfile(typeof(MapperConfigure))
            ).CreateMapper();
        }

        [Fact]
        public async Task GetAllCoffeesDataAsync_ReturnsIEnumerableOfCoffeeDataDto()
        {
            // Arrange
            var coffeesData = new List<CoffeeData>()
            {
                new CoffeeData(),
                new CoffeeData(),
                new CoffeeData()
            };

            var coffeeRepositoryMock = new Mock<ICoffeeRepository>();
            coffeeRepositoryMock
                .Setup(repo => repo.GetAllCoffeeDataAsync())
                .ReturnsAsync(coffeesData);

            var dataManagerMock = new Mock<IUnitOfWork>();
            dataManagerMock
                .SetupGet(d => d.Coffees)
                .Returns(coffeeRepositoryMock.Object);

            var orderServiceMock = new Mock<IOrderService>();
            var service = new CoffeeService(dataManagerMock.Object, orderServiceMock.Object, mapper);

            // Act
            var coffeesDataDto = await service.GetAllCoffeesDataAsync();

            // Assert
            Assert.Equal(coffeesData.Count, coffeesDataDto.Count());
            Assert.All(coffeesDataDto, dto => Assert.IsType<CoffeeDataDto>(dto));
        }

        [Fact]
        public async Task GetCoffeeDataAsync_WhenValidName_ReturnsCoffeeDataDto()
        {
            // Arrange
            const string Name = "Americano";
            var coffeeData = new CoffeeData();
            coffeeData.Name = Name;

            var coffeeRepositoryMock = new Mock<ICoffeeRepository>();
            coffeeRepositoryMock
                .Setup(repo => repo.GetCoffeeDataAsync(Name))
                .ReturnsAsync(coffeeData);

            var dataManagerMock = new Mock<IUnitOfWork>();
            dataManagerMock
                .SetupGet(d => d.Coffees)
                .Returns(coffeeRepositoryMock.Object);

            var orderServiceMock = new Mock<IOrderService>();
            var service = new CoffeeService(dataManagerMock.Object, orderServiceMock.Object, mapper);

            // Act
            var coffeeDataDto = await service.GetCoffeeDataAsync(Name);

            // Assert
            Assert.IsType<CoffeeDataDto>(coffeeDataDto);
            Assert.Equal(Name, coffeeDataDto.Name);
        }

        [Fact]
        public async Task GetCoffeeData_WhenInvalidName_ThrowsNotFoundException()
        {
            // Arrange
            CoffeeData coffeeData = null;

            var coffeeRepositoryMock = new Mock<ICoffeeRepository>();
            coffeeRepositoryMock
                .Setup(repo => repo.GetCoffeeDataAsync("name1"))
                .ReturnsAsync(coffeeData);

            var dataManagerMock = new Mock<IUnitOfWork>();
            dataManagerMock.SetupGet(d => d.Coffees).Returns(coffeeRepositoryMock.Object);

            var orderServiceMock = new Mock<IOrderService>();
            var service = new CoffeeService(dataManagerMock.Object, orderServiceMock.Object, mapper);

            // Act and Assert
            await Assert.ThrowsAsync<NotFoundException>(() => service.GetCoffeeDataAsync("name2"));
        }

        [Fact]
        public async Task CreateNewCoffeeAsync_WhenInvalidName_ThrowsNotFoundException()
        {
            // Arrange
            CoffeeData coffeeData = null;
            var coffeeDto = new CoffeeDto();
            coffeeDto.Name = "name1";

            var coffeeRepositoryMock = new Mock<ICoffeeRepository>();
            coffeeRepositoryMock
                .Setup(repo => repo.GetCoffeeDataAsync("name2"))
                .ReturnsAsync(coffeeData);

            var dataManagerMock = new Mock<IUnitOfWork>();
            dataManagerMock
                .SetupGet(d => d.Coffees)
                .Returns(coffeeRepositoryMock.Object);

            var orderServiceMock = new Mock<IOrderService>();
            var service = new CoffeeService(dataManagerMock.Object, orderServiceMock.Object, mapper);

            // Act and Assert
            await Assert.ThrowsAsync<NotFoundException>(() => service.CreateNewCoffeeAsync(coffeeDto));
        }

        [Fact]
        public async Task CreateNewCoffeeAsync_WhenInvalidVolume_ThrowsNotFoundException()
        {
            // Arrange
            const string Name = "Americano";
            const string Volume = "1000";

            var coffeeData = new CoffeeData();
            coffeeData.Volumes = new List<VolumeData>() { new VolumeData { Volume = "0.133L" } };

            var coffeeDto = new CoffeeDto();
            coffeeDto.Name = Name;
            coffeeDto.Volume = Volume;

            var coffeeRepositoryMock = new Mock<ICoffeeRepository>();
            coffeeRepositoryMock
                .Setup(repo => repo.GetCoffeeDataAsync(Name))
                .ReturnsAsync(coffeeData);

            var dataManagerMock = new Mock<IUnitOfWork>();
            dataManagerMock
                .SetupGet(d => d.Coffees)
                .Returns(coffeeRepositoryMock.Object);

            var orderServiceMock = new Mock<IOrderService>();
            var service = new CoffeeService(dataManagerMock.Object, orderServiceMock.Object, mapper);

            // Act and Assert
            await Assert.ThrowsAsync<NotFoundException>(() => service.CreateNewCoffeeAsync(coffeeDto));
        }

        [Fact]
        public async Task CreateNewCoffeeAsync_WhenValidArgumentsProvided_CreatesNewCoffee()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<CoffeeDbContext>()
                .UseInMemoryDatabase("CoffeesDb").Options;
            var context = new CoffeeDbContext(options);

            var coffeeData = new CoffeeData()
            {
                Name = "Americano",
                Image = "image"
            };
            context.CoffeeData.Add(coffeeData);
            await context.SaveChangesAsync();

            coffeeData.Volumes = new List<VolumeData>() {
                new VolumeData() { Volume = "100ml", CoffeeDataId = coffeeData.Id },
                new VolumeData() { Volume = "200ml", CoffeeDataId = coffeeData.Id },
            };
            await context.SaveChangesAsync();

            coffeeData.Volumes[0].PriceData = new PriceData() { VolumeDataId = 1, Price = 10 };
            coffeeData.Volumes[1].PriceData = new PriceData() { VolumeDataId = 2, Price = 20 };
            await context.SaveChangesAsync();

            var coffeeRepository = new CoffeeRepository(context);
            var dataManagerMock = new Mock<IUnitOfWork>();
            dataManagerMock
                .SetupGet(d => d.Coffees)
                .Returns(coffeeRepository);
            dataManagerMock
                .Setup(d => d.CompleteAsync())
                .Callback(() => context.SaveChangesAsync());

            var orderServiceMock = new Mock<IOrderService>();
            orderServiceMock
                .Setup(service => service.CreateNewOrderAsync())
                .ReturnsAsync(new OrderDto() { Id = 1 });

            var service = new CoffeeService(dataManagerMock.Object, orderServiceMock.Object, mapper);

            var coffeeDto = new CoffeeDto()
            {
                Name = coffeeData.Name,
                Volume = coffeeData.Volumes[0].Volume,
                Sugar = 3,
                Milk = true,
                CupCap = true
            };

            // Act
            await service.CreateNewCoffeeAsync(coffeeDto);
            var coffee = await context.Coffees.FirstOrDefaultAsync();

            // Assert
            Assert.Equal(1, coffee.Id);
            Assert.Equal(1, coffee.OrderId);
            Assert.Equal(3, coffee.Sugar);
            Assert.True(coffee.Milk);
            Assert.True(coffee.CupCap);
            Assert.Equal(coffeeDto.Name, coffee.Name);
            Assert.Equal(coffeeDto.Volume, coffee.Volume);
        }
    }
}
