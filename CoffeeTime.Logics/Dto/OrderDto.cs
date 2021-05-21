using System.Collections.Generic;

namespace CoffeeTime.Logics.Dto
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string GuidId { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string UserPhoneNumber { get; set; }
        public decimal Price { get; set; }
        public List<CoffeeDto> CoffeeCartItems { get; set; } = new List<CoffeeDto>();
    }
}
