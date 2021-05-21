using CoffeeTime.Logics.Dto;
using System.Collections.Generic;

namespace CoffeeTime.Web.Models
{
    public class OrderViewModel
    {
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string UserPhoneNumber { get; set; }
        public decimal Price { get; set; }
        public List<CoffeeDto> CoffeeCartItems { get; set; } = new List<CoffeeDto>();
    }
}
