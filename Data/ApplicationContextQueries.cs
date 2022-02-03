using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public partial class ApplicationContext
    {
        public Enclosure? GetEnclosureByNumber(int number)
        {
            return (from enc in this.Enclosures
                    where enc.Number == number
                    select enc).SingleOrDefault();
        }
    }
}
