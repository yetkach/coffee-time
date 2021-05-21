namespace CoffeeTime.Logics.Interfaces
{
    public interface IOrderGuidService
    {
        string GetCurrentGuid();
        void SetNewGuid();
    }
}
