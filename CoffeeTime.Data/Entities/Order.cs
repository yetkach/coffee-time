using System;
using System.Collections.Generic;

namespace CoffeeTime.Data.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public string GuidId { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string UserPhoneNumber { get; set; }
        public DateTime OrderTime { get; set; }
        public decimal Price { get; set; }
        public bool IsCheckedOut { get; set; }
        public List<Coffee> CoffeeCartItems { get; set; } = new List<Coffee>();
    }
}
