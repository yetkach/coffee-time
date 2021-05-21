namespace CoffeeTime.Data.Entities
{
    public class VolumeData
    {
        public int Id { get; set; }
        public int CoffeeDataId { get; set; }
        public string Volume { get; set; }
        public PriceData PriceData { get; set; }
    }
}
