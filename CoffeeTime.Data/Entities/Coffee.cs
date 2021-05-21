namespace CoffeeTime.Data.Entities
{
    public class Coffee
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string Name { get; set; }
        public string Volume { get; set; }
        public int Sugar { get; set; }
        public bool Milk { get; set; }
        public bool CupCap { get; set; }
        public decimal Price { get; set; }
    }
}
