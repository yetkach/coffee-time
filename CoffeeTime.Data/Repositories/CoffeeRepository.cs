using CoffeeTime.Data.EF;
using CoffeeTime.Data.Entities;
using CoffeeTime.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoffeeTime.Data.Repositories
{
    public class CoffeeRepository : ICoffeeRepository
    {
        private readonly CoffeeDbContext db;

        public CoffeeRepository(CoffeeDbContext db)
        {
            this.db = db;
        }

        public async Task<CoffeeData> GetCoffeeDataAsync(string name)
        {
            return await db.CoffeeData
                .Include(c => c.Volumes)
                .ThenInclude(v => v.PriceData)
                .FirstOrDefaultAsync(c => c.Name == name);
        }

        public async Task AddAsync(Coffee coffee)
        {
            await db.Coffees.AddAsync(coffee);
        }

        public async Task<List<CoffeeData>> GetAllCoffeeDataAsync()
        {
            return await db.CoffeeData.ToListAsync();
        }
    }
}
