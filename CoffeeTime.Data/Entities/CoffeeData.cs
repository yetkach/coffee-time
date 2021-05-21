using System.Collections.Generic;

namespace CoffeeTime.Data.Entities
{
    public class CoffeeData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<VolumeData> Volumes { get; set; }
        public string Image { get; set; }
    }
}
