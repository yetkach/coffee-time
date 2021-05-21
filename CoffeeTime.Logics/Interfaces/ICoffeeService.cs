using CoffeeTime.Logics.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoffeeTime.Logics.Interfaces
{
    public interface ICoffeeService
    {
        Task<List<CoffeeDataDto>> GetAllCoffeesDataAsync();
        Task<CoffeeDataDto> GetCoffeeDataAsync(string name);
        Task CreateNewCoffeeAsync(CoffeeDto dto);
    }
}
