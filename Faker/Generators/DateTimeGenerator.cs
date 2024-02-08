using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generators
{
    public class DateTimeGenerator : BaseGenerator<DateTime>
    {
        public override DateTime generate(Random random)
        {
            DateTime start = new DateTime(1879, 1, 1);
            int range = (DateTime.Today - start).Days;
            return start.AddDays(random.Next(range));
        }
    }
}
