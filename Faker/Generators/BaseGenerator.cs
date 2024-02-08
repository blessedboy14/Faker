using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generators
{
    public abstract class BaseGenerator<T> : IGenerator
    {
        public abstract T generate(Random random);
        public object Generate(GeneratorInfo info)
        {
            return generate(info.random);
        }
    }
}
