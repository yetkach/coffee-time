using CoffeeTime.Logics.Dto;
using System.Threading.Tasks;

namespace CoffeeTime.Logics.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDto> GetUnpaidOrderAsync();
        Task<OrderDto> GetCurrentOrderAsync();
        Task<OrderDto> CreateNewOrderAsync();
        Task CheckoutAsync(OrderDto orderDto);
    }
}
