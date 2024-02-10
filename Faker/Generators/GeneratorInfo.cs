using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generators
{
    public class GeneratorInfo
    {
        public Random random { get; }
        public Type target {  get; }
        public Faker.Faker faker { get; }
        public GeneratorInfo(Random random, Type target)
        {
            this.random = random;
            this.target = target;
            this.faker = new Faker.Faker();
        }
    }
}
