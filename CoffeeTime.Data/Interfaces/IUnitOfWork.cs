using System.Threading.Tasks;

namespace CoffeeTime.Data.Interfaces
{
    public interface IUnitOfWork
    {
        ICoffeeRepository Coffees { get; }
        IOrderRepository Orders { get; }
        Task CompleteAsync();
    }
}
