using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generators
{
    public class ListGenerator : IGenerator
    {
        public object Generate(GeneratorInfo info)
        {
            var list = (System.Collections.IList)Activator.CreateInstance(info.target);
            for (int i = 0; i < info.random.Next(5, 15); i++)
            {
                list.Add(info.faker.CreateDTO(info.target.GetGenericArguments()[0]));
            }
            return list;
        }
    }
}
