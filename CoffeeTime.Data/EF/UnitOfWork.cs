using CoffeeTime.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeTime.Data.EF
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CoffeeDbContext db;
        public ICoffeeRepository Coffees { get; }
        public IOrderRepository Orders { get; }

        public UnitOfWork(
            CoffeeDbContext db,
            ICoffeeRepository coffeeRepository,
            IOrderRepository orderRepository
        )
        {
            this.db = db;
            Coffees = coffeeRepository;
            Orders = orderRepository;
        }

        public async Task CompleteAsync()
        {
            await db.SaveChangesAsync();
        }
    }
}
