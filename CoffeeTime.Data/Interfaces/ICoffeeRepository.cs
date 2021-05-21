using CoffeeTime.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoffeeTime.Data.Interfaces
{
    public interface ICoffeeRepository
    {
        Task<CoffeeData> GetCoffeeDataAsync(string name);
        Task<List<CoffeeData>> GetAllCoffeeDataAsync();
        Task AddAsync(Coffee coffee);
    }
}
