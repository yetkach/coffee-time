using CoffeeTime.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoffeeTime.Data.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> GetOrderAsync(string userId);
        Task<Order> GetUnpaidOrderAsync(string userId);
        Task<CheckTime> GetDeletionDateAsync();
        Task<IEnumerable<Order>> GetOrdersForRemovingAsync();
        Task AddAsync(Order order);
        void Remove(Order order);
        void Update(Order order);
        void UpdateDeletionDate(CheckTime deletionDate);
    }
}
