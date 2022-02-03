using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class Enclosure
    {
        public Guid ID { get; set; }
        public int Number { get; set; }

        public List<Animal> Animals { get; set; } = new();

        public Animal? GetByName(string name)
        {
            return (from animal in this.Animals
                    where String.Equals(animal.Name, name,
                    StringComparison.InvariantCultureIgnoreCase)
                    select animal).SingleOrDefault();
        }
    }
}
