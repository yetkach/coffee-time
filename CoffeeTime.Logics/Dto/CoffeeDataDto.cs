using System.Collections.Generic;

namespace CoffeeTime.Logics.Dto
{
    public class CoffeeDataDto
    {
        public string Name { get; set; }
        public List<VolumeDataDto> Volumes { get; set; }
        public string Image { get; set; }
    }
}
