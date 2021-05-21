using CoffeeTime.Logics.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoffeeTime.Web.Models
{
    public class CoffeeViewModel
    {
        public string Name { get; set; }
        public string Volume { get; set; }
        public int Sugar { get; set; }
        public bool Milk { get; set; }
        [Display(Name = "Cup Cap")]
        public bool CupCap { get; set; }
        public string Image { get; set; }
        public List<VolumeDataDto> Volumes { get; set; }
    }
}
