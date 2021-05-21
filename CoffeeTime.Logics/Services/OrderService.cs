using AutoMapper;
using CoffeeTime.Data.Entities;
using CoffeeTime.Data.Interfaces;
using CoffeeTime.Logics.Dto;
using CoffeeTime.Logics.Infrastructure;
using CoffeeTime.Logics.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeeTime.Logics.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IOrderGuidService orderGuidService;
        private readonly IMapper mapper;

        public OrderService(IUnitOfWork unitOfWork, IOrderGuidService orderGuidService, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.orderGuidService = orderGuidService;
            this.mapper = mapper;
        }

        public async Task<OrderDto> GetCurrentOrderAsync()
        {
            string guidId = orderGuidService.GetCurrentGuid();
            Order currentOrder = await unitOfWork.Orders.GetUnpaidOrderAsync(guidId);

            if (currentOrder == null)
            {
                throw new NotFoundException();
            }

            if (!currentOrder.CoffeeCartItems.Any())
            {
                throw new NotFoundException();
            }

            var currentOrderDto = new OrderDto();
            currentOrderDto.Price = 0;
            currentOrderDto.GuidId = currentOrder.GuidId;
            currentOrderDto.Id = currentOrder.Id;
            currentOrderDto.CoffeeCartItems = mapper.Map<List<CoffeeDto>>(currentOrder.CoffeeCartItems);
            currentOrderDto.Price = currentOrder.CoffeeCartItems.Select(item => item.Price).Sum();

            return currentOrderDto;
        }

        public async Task<OrderDto> GetUnpaidOrderAsync()
        {
            string guidId = orderGuidService.GetCurrentGuid();
            Order unpaidOrder = await unitOfWork.Orders.GetUnpaidOrderAsync(guidId);

            if (unpaidOrder == null)
            {
                return null;
            }

            OrderDto unpaidOrderDto = new OrderDto();
            unpaidOrderDto.Price = 0;
            unpaidOrderDto.GuidId = unpaidOrder.GuidId;
            unpaidOrderDto.Id = unpaidOrder.Id;
            unpaidOrderDto.CoffeeCartItems = mapper.Map<List<CoffeeDto>>(unpaidOrder.CoffeeCartItems);
            unpaidOrderDto.Price = unpaidOrderDto.CoffeeCartItems.Select(item => item.Price).Sum();

            return unpaidOrderDto;
        }

        public async Task<OrderDto> CreateNewOrderAsync()
        {
            orderGuidService.SetNewGuid();
            string guidId = orderGuidService.GetCurrentGuid();

            var newOrder = new Order();
            newOrder.GuidId = guidId;
            await unitOfWork.Orders.AddAsync(newOrder);
            await unitOfWork.CompleteAsync();

            OrderDto newOrderDto = new OrderDto();
            newOrderDto.Id = newOrder.Id;

            return newOrderDto;
        }

        private async Task DeleteUnpaidOrdersAsync()
        {
            CheckTime deletionDate = await unitOfWork.Orders.GetDeletionDateAsync();

            if (DateTime.UtcNow >= deletionDate.DeletionTime)
            {
                var ordersForRemoving = await unitOfWork.Orders.GetOrdersForRemovingAsync();

                foreach (var order in ordersForRemoving)
                {
                    unitOfWork.Orders.Remove(order);
                }

                unitOfWork.Orders.UpdateDeletionDate(deletionDate);
                await unitOfWork.CompleteAsync();
            }
        }

        public async Task CheckoutAsync(OrderDto orderDto)
        {
            string guidId = orderGuidService.GetCurrentGuid();
            var order = await unitOfWork.Orders.GetOrderAsync(guidId);

            if (order == null)
            {
                throw new NotFoundException();
            }

            order.UserFirstName = orderDto.UserFirstName;
            order.UserLastName = orderDto.UserLastName;
            order.UserPhoneNumber = orderDto.UserPhoneNumber;
            order.Price = orderDto.Price;
            order.OrderTime = DateTime.UtcNow;
            order.IsCheckedOut = true;

            unitOfWork.Orders.Update(order);
            await unitOfWork.CompleteAsync();
            await DeleteUnpaidOrdersAsync();
        }
    }
}
