using CoffeeTime.Data.EF;
using CoffeeTime.Data.Entities;
using CoffeeTime.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeeTime.Data.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly CoffeeDbContext db;

        public OrderRepository(CoffeeDbContext db)
        {
            this.db = db;
        }

        public async Task<Order> GetOrderAsync(string guidId)
        {
            return await db.Orders.FirstOrDefaultAsync(o => o.GuidId == guidId);
        }

        public async Task<Order> GetUnpaidOrderAsync(string guidId)
        {
            return await db.Orders.Include(o => o.CoffeeCartItems)
                .FirstOrDefaultAsync(o => o.GuidId == guidId && o.IsCheckedOut == false);
        }

        public async Task<CheckTime> GetDeletionDateAsync()
        {
            return await db.CheckTimes.Where(t => t.Id == 1).FirstOrDefaultAsync();
        }

        public async Task AddAsync(Order order)
        {
            await db.Orders.AddAsync(order);
        }

        public void Update(Order order)
        {
            db.Update(order);
        }

        public void Remove(Order order)
        {
            db.Remove(order);
        }

        public void UpdateDeletionDate(CheckTime deletionDate)
        {
            deletionDate.DeletionTime = deletionDate.DeletionTime.AddDays(3.0);
        }

        public async Task<IEnumerable<Order>> GetOrdersForRemovingAsync()
        {
            return await db.Orders.Where(o => o.IsCheckedOut == false &&
                o.OrderTime.AddDays(1) <= DateTime.UtcNow).ToListAsync();
        }
    }
}
