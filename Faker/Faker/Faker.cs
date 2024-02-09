using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using Generators;
using System.Data;

namespace Faker
{
    public class Faker : IFaker
    {
        private Dictionary<Type, IGenerator> gens;
        private Stack<Type> usedTypes;

        private bool isBasicGenerator(Type given, Type required)
        {
            Type start = given;
            if (start.BaseType != null && start.BaseType != typeof(object))
            {
                if (start.BaseType.IsGenericType && start.BaseType.GetGenericTypeDefinition() == required)
                {
                    return true;
                }
            }
            return false;
        }

        private void LoadPlugins()
        {
            string path = Path.GetDirectoryName(
                Path.GetDirectoryName(
                    Path.GetDirectoryName(
                        Path.GetDirectoryName(AppContext.BaseDirectory)))) + "\\Plugins\\";
            foreach (var f in Directory.GetFiles(path))
            {
                var DLL = Assembly.LoadFrom(f);
                
                foreach (Type t in DLL.GetTypes())
                {
                    if (isBasicGenerator(t, typeof(BaseGenerator<>)))
                    {
                        gens.Add(t.BaseType.GetGenericArguments()[0], (IGenerator)Activator.CreateInstance(t));
                    }
                }
            }
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
            LoadPlugins();
        }

        private IGenerator FindGen(Type t)
        {
            if (t.IsGenericType)
            {
                t = t.GetGenericTypeDefinition();
            }
            if (gens.ContainsKey(t))
            {
                return gens[t];
            }
            return null;
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

            IGenerator SimpleGen = FindGen(type);
            if (SimpleGen != null)
            {
                usedTypes.Pop();
                GeneratorInfo genInfo = new GeneratorInfo(new Random(DateTime.Now.Microsecond), type);
                return SimpleGen.Generate(genInfo);
            }

            object dtoObj = Instantiate(type);
            dtoObj = FillObject(dtoObj);
            usedTypes.Pop();
            return dtoObj;
        }

        private bool IsSet(object dtoObj, MemberInfo member)
        {
            if (member is FieldInfo)
            {
                FieldInfo fi = (FieldInfo)member;
                if (fi.FieldType.IsValueType)
                {
                    return !fi.GetValue(dtoObj).Equals(Activator.CreateInstance(fi.FieldType));
                } 
                else if (fi.GetValue(dtoObj) == null) 
                {
                    return false;
                }
            }
            else if (member is PropertyInfo)
            {
                PropertyInfo pi = (PropertyInfo)member;
                if (pi.PropertyType.IsValueType)
                {
                    return !pi.GetValue(dtoObj).Equals(Activator.CreateInstance(pi.PropertyType));
                } 
                else if (pi.GetValue(dtoObj) == null)
                {
                    return false;
                }
            }
            return true;
        }

        private object FillObject(object dtoObj)
        {
            if (dtoObj != null)
            {
                PropertyInfo[] props = dtoObj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                FieldInfo[] fields = dtoObj.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
                foreach(FieldInfo field in fields)
                {
                    if (!IsSet(dtoObj, field))
                    {
                        IGenerator SimpleType = FindGen(field.FieldType);
                        if (SimpleType != null)
                        {
                            GeneratorInfo genInfo = new GeneratorInfo(new Random(DateTime.Now.Microsecond), field.FieldType);
                            field.SetValue(dtoObj, SimpleType.Generate(genInfo));
                        }
                        else
                        {
                            field.SetValue(dtoObj, CreateDTO(field.FieldType));
                        }
                    }
                }
                foreach (PropertyInfo prop in props)
                {
                    if (prop.CanWrite && !IsSet(dtoObj, prop))
                    {
                        IGenerator SimpleType = FindGen(prop.PropertyType);
                        if (SimpleType != null)
                        {
                            GeneratorInfo genInfo = new GeneratorInfo(new Random(DateTime.Now.Microsecond), prop.PropertyType);
                            prop.SetValue(dtoObj, SimpleType.Generate(genInfo));
                        }
                        else
                        {
                            prop.SetValue(dtoObj, CreateDTO(prop.PropertyType));
                        }
                    }
                }
            }
            return dtoObj;
        }

        private object Instantiate(Type type)
        {
            ConstructorInfo[] constructors = type.GetConstructors();
            IEnumerable<ConstructorInfo> constr = constructors.OrderByDescending(c => c.GetParameters().Length);

            object dtoObj = null;

            foreach (ConstructorInfo constructor in constr)
            {
                ParameterInfo[] Params = constructor.GetParameters();
                object[] constrInput = new object[Params.Length];
                for (int i = 0; i < Params.Length; i++)
                {
                    IGenerator SimpleType = FindGen(Params[i].ParameterType);
                    if (SimpleType != null)
                    {
                        GeneratorInfo genInfo = new GeneratorInfo(new Random(DateTime.Now.Microsecond), type);
                        constrInput[i] = SimpleType.Generate(genInfo);
                    } else
                    {
                        constrInput[i] = CreateDTO(Params[i].ParameterType);
                    }
                }
                try
                {
                    dtoObj = constructor.Invoke(constrInput);
                    break;
                } catch (Exception ex){
                    continue;
                }
            }
            if (dtoObj == null && type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }

            return dtoObj;
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
