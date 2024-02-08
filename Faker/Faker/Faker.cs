using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Faker
{
    public class Faker : IFaker
    {
        private Stack<Type> usedTypes;

        public Faker() 
        {
            usedTypes = new Stack<Type>();

        }

        public T Create<T>()
        {
            return (T)CreateDTO(typeof(T));
        }

        private object CreateDTO(Type type)
        {
            if (usedTypes.Where(t => t == type).Count() >= 2)
            {
                Console.WriteLine("Circled dependencies");
                return GetDefaultValue(type);
            }
            usedTypes.Push(type);

            object dtoObj = Instantiate(type);
        }

        private object Instantiate(Type type)
        {
            ConstructorInfo[] constructors = type.GetConstructors();
            IEnumerable<ConstructorInfo> constr = constructors.OrderByDescending(c => c.GetParameters().Length);

            object dtoObj = null;

            foreach (ConstructorInfo constructor in constr)
            {
                ParameterInfo[] Params = constructor.GetParameters();
            }

            return null;
        }

        private object GetDefaultValue(Type type) 
        {
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }
            return null;
        }

    }
}
