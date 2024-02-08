using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using Generators;

namespace Faker
{
    public class Faker : IFaker
    {
        private Dictionary<Type, IGenerator> gens;
        private Stack<Type> usedTypes;

        private bool isBasicGenerator(Type given, Type required)
        {
            Type start = given;
            while (start != null && start != typeof(object))
            {
                Type temp;
                if (start.IsGenericType)
                {
                    temp = start.GetGenericTypeDefinition();
                } else
                {
                    temp = start;
                }
                if (required == temp)
                {
                    return true;
                }
                start = start.BaseType;
            }
            return false;
        }

        public Faker() 
        {
            usedTypes = new Stack<Type>();
            gens = new Dictionary<Type, IGenerator>();
            Type[] types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (Type t in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (isBasicGenerator(t, typeof(BaseGenerator<>)))
                {

                    if (t.Namespace == "Generators" && t.BaseType.IsGenericType)
                    {
                        gens.Add(t.BaseType.GetGenericArguments()[0], (IGenerator)Activator.CreateInstance(t));
                    }
                }
            }
            int b = 0;
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
            return null;
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
