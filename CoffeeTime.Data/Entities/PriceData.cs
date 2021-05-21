namespace CoffeeTime.Data.Entities
{
    public class PriceData
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public int VolumeDataId { get; set; }
    }
}
