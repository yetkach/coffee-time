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
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CoffeeTime.UnitTests
{
    public class OrderServiceTests
    {
        private readonly IMapper mapper;
        public OrderServiceTests()
        {
            mapper = new MapperConfiguration(config =>
                config.AddProfile(typeof(MapperConfigure))
            ).CreateMapper();
        }

        [Fact]
        public async Task GetCurrentOrderAsync__WhenInvalidCurrentOrder_ThrowsNotFoundException()
        {
            //Arrange
            const string GuidId = "test";
            Order order = null;

            var orderRepositoryMock = new Mock<IOrderRepository>();
            orderRepositoryMock
                .Setup(repo => repo.GetUnpaidOrderAsync(GuidId))
                .ReturnsAsync(order);

            var dataManagerMock = new Mock<IUnitOfWork>();
            dataManagerMock.SetupGet(d => d.Orders).Returns(orderRepositoryMock.Object);
            var orderGuidServiceMock = new Mock<IOrderGuidService>();

            var service = new OrderService(dataManagerMock.Object, orderGuidServiceMock.Object, mapper);

            //Act and Assert
            await Assert.ThrowsAsync<NotFoundException>(async () => await service.GetCurrentOrderAsync());
        }

        [Fact]
        public async Task GetCurrentOrderAsync__WhenGetCurrentOrderWithInvalidCoffeeCartItems_ThrowsNotFoundException()
        {
            //Arrange
            const string GuidId = "test";
            var order = new Order();

            var orderGuidServiceMock = new Mock<IOrderGuidService>();
            orderGuidServiceMock
                .Setup(o => o.GetCurrentGuid())
                .Returns(GuidId);

            var orderRepositoryMock = new Mock<IOrderRepository>();
            orderRepositoryMock
                .Setup(repo => repo.GetUnpaidOrderAsync(GuidId))
                .ReturnsAsync(order);

            var dataManagerMock = new Mock<IUnitOfWork>();
            dataManagerMock.SetupGet(d => d.Orders).Returns(orderRepositoryMock.Object);

            var service = new OrderService(dataManagerMock.Object, orderGuidServiceMock.Object, mapper);

            //Act and Assert
            await Assert.ThrowsAsync<NotFoundException>(async () => await service.GetCurrentOrderAsync());
        }

        [Fact]
        public async Task GetCurrentOrderAsync_ReturnsOrderDto()
        {
            //Arrange
            const string GuidId = "test";
            var coffee = new Coffee();
            var order = new Order();
            order.CoffeeCartItems.Add(coffee);

            var orderGuidServiceMock = new Mock<IOrderGuidService>();
            orderGuidServiceMock
                .Setup(o => o.GetCurrentGuid())
                .Returns(GuidId);

            var orderRepositoryMock = new Mock<IOrderRepository>();
            orderRepositoryMock
                .Setup(repo => repo.GetUnpaidOrderAsync(GuidId))
                .ReturnsAsync(order);

            var dataManagerMock = new Mock<IUnitOfWork>();
            dataManagerMock.SetupGet(d => d.Orders).Returns(orderRepositoryMock.Object);

            var service = new OrderService(dataManagerMock.Object, orderGuidServiceMock.Object, mapper);

            //Act 
            var currentOrder = await service.GetCurrentOrderAsync();
            //Assert
            Assert.IsType<OrderDto>(currentOrder);
        }

        [Fact]
        public async Task GetUnpaidOrderAsync_ReturnsOrderDto()
        {
            //Arrange
            const string GuidId = "test";
            var order = new Order();

            var orderGuidServiceMock = new Mock<IOrderGuidService>();
            orderGuidServiceMock
                .Setup(o => o.GetCurrentGuid())
                .Returns(GuidId);
            
            var orderRepositoryMock = new Mock<IOrderRepository>();
            orderRepositoryMock
                .Setup(repo => repo.GetUnpaidOrderAsync(GuidId))
                .ReturnsAsync(order);

            var dataManagerMock = new Mock<IUnitOfWork>();
            dataManagerMock
                .SetupGet(d => d.Orders)
                .Returns(orderRepositoryMock.Object);

            var service = new OrderService(dataManagerMock.Object, orderGuidServiceMock.Object, mapper);

            //Act 
            var unpaidOrder = await service.GetUnpaidOrderAsync();
            //Assert
            Assert.IsType<OrderDto>(unpaidOrder);
        }

        [Fact]
        public async Task CreateNewOrderAsync_ReturnsOrderDtoWithCorrectId()
        {
            // Arrange
            var order = new Order();
            var options = new DbContextOptionsBuilder<CoffeeDbContext>()
                .UseInMemoryDatabase("createOrderDb").Options;

            var context = new CoffeeDbContext(options);
            var orderRepository = new OrderRepository(context);

            var orderGuidServiceMock = new Mock<IOrderGuidService>();

            var dataManagerMock = new Mock<IUnitOfWork>();
            dataManagerMock
                .SetupGet(d => d.Orders)
                .Returns(orderRepository);
            dataManagerMock
                .Setup(d => d.CompleteAsync())
                .Callback(() => context.SaveChangesAsync());

            var service = new OrderService(dataManagerMock.Object, orderGuidServiceMock.Object, mapper);

            // Act
            var newOrder = await service.CreateNewOrderAsync();
            var newOrderFromDb = context.Orders.FirstOrDefault();

            // Assert
            Assert.IsType<OrderDto>(newOrder);
            Assert.Equal(newOrder.Id, newOrderFromDb.Id);
        }

        [Fact]
        public async Task CheckoutAsync_WhenOrderIsNull_ThrowsNotFoundException()
        {
            //Arrange
            const string GuidId = "test";
            Order order = null;
            var orderDto = new OrderDto();

            var orderGuidServiceMock = new Mock<IOrderGuidService>();
            orderGuidServiceMock
                .Setup(o => o.GetCurrentGuid())
                .Returns(GuidId);

            
            var orderRepositoryMock = new Mock<IOrderRepository>();
            orderRepositoryMock
                .Setup(repo => repo.GetOrderAsync(GuidId))
                .ReturnsAsync(order);

            var dataManagerMock = new Mock<IUnitOfWork>();
            dataManagerMock
                .SetupGet(d => d.Orders)
                .Returns(orderRepositoryMock.Object);

            var service = new OrderService(dataManagerMock.Object, orderGuidServiceMock.Object, mapper);

            // Act and Assert
            await Assert.ThrowsAsync<NotFoundException>(async () => await service.CheckoutAsync(orderDto));
        }

        [Fact]
        public async Task CheckoutAsync_UpdatesOrder()
        {
            // Arrange
            const string GuidId = "test";
            var testOrder = new Order();
            testOrder.GuidId = GuidId;

            var options = new DbContextOptionsBuilder<CoffeeDbContext>()
                .UseInMemoryDatabase("orderDb").Options;
            var context = new CoffeeDbContext(options);

            var orderRepository = new OrderRepository(context);

            var orderGuidServiceMock = new Mock<IOrderGuidService>();
            orderGuidServiceMock.Setup(repo => repo.GetCurrentGuid())
                .Returns(GuidId);
            
            await context.Orders.AddAsync(testOrder);

            var checkTime = new CheckTime();
            checkTime.DeletionTime = new DateTime(2222, 09, 09);
            await context.CheckTimes.AddAsync(checkTime);
            await context.SaveChangesAsync();

            var dataManagerMock = new Mock<IUnitOfWork>();
            dataManagerMock.SetupGet(d => d.Orders).Returns(orderRepository);
            dataManagerMock.Setup(d => d.CompleteAsync()).Callback(() => context.SaveChangesAsync());

            var service = new OrderService(dataManagerMock.Object, orderGuidServiceMock.Object, mapper);

            var orderDto = new OrderDto();
            orderDto.UserFirstName = "Andrew";
            orderDto.UserLastName = "Stepanov";
            orderDto.UserPhoneNumber = "44-444-44";
            orderDto.Price = 10m;

            // Act
            await service.CheckoutAsync(orderDto);
            var updatedOrder = await context.Orders.FirstOrDefaultAsync();

            // Assert
            Assert.Equal(1, updatedOrder.Id);
            Assert.Equal(orderDto.Price, updatedOrder.Price);
            Assert.Equal(orderDto.UserFirstName, updatedOrder.UserFirstName);
            Assert.Equal(orderDto.UserLastName, updatedOrder.UserLastName);
            Assert.Equal(orderDto.UserPhoneNumber, updatedOrder.UserPhoneNumber);
            Assert.True(updatedOrder.IsCheckedOut);
        }
    }
}
