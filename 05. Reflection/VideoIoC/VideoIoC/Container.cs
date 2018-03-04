using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace VideoIoC
{
    public class Container
    {
        public readonly List<Assembly> assemblyList;
//        public List<Type> typeList;
        public Dictionary<Type, Type> typeDictionary;

        public Container()
        {
            assemblyList = new List<Assembly>();
//            typeList = new List<Type>();
            typeDictionary = new Dictionary<Type, Type>();
        }

        public void AddAssembly(Assembly assembly)
        {
            assemblyList.Add(assembly);

            var types1 = assembly.GetTypes()
                .Where(t => t.IsClass && t.GetCustomAttributes<ImportConstructorAttribute>().Any());
            foreach (Type t in types1)
                AddType(t);

            var types2 = assembly.GetTypes()
                .Where(t => t.GetProperties().Any(p => p.GetCustomAttributes<ImportAttribute>().Any()));
            foreach (Type t in types2)
                AddType(t);

            var interfaces = assembly.GetTypes()
                .Where(t => t.IsInterface);
            foreach (Type intrfc in interfaces)
            {
                var types = assembly.GetTypes()
                    .Where(t => t.IsClass && t.GetCustomAttributes<ExportAttribute>().Any() && t.GetInterfaces().Any(i => i == intrfc));
                foreach (Type t in types)
                    AddType(intrfc, t);
            }
        }

        public void AddType(Type type)
        {
            AddType(type, type);
        }

        public void AddType(Type baseType, Type type)
        {
            typeDictionary.Add(baseType, type);
        }

        public object CreateInstance(Type type)
        {
            if (!typeDictionary.Where(t => t.Key == type || t.Key == type.GetInterface(t.Key.Name)).Any()) throw new TypeNotSupported(string.Format("Тип ({0}) не поддерживается", type.Name));

            if (type.IsAbstract) throw new CanNotCreateAbstractType(string.Format("Тип ({0}) является абстрактным. Объекты с таким типом нельзя создавать", type.Name));

            ConstructorInfo сonstructor;
            object obj;
            List<object> objects = new List<object>();
            List<Type> types = new List<Type>();

            if (!type.GetConstructors().Any()) throw new ConstructorNotFound(string.Format("Тип ({0}) не содержит доступного конструктора", type.Name));

            if (type.GetCustomAttribute<ImportConstructorAttribute>() != null)
            {
                foreach (var c in type.GetConstructors())
                    foreach (var p in c.GetParameters())
                        {
                            Type t;
                            if (typeDictionary.TryGetValue(p.ParameterType, out t))
                            {
                                object objProp = CreateInstance(t);

                                objects.Add(objProp);
                                types.Add(p.ParameterType);
                            }
                        }
            }

            сonstructor = type.GetConstructor(types.ToArray());
            obj = сonstructor.Invoke(objects.ToArray());

            foreach (var t in typeDictionary)
                foreach (PropertyInfo p in type.GetProperties().Where(p => p.PropertyType == t.Key && p.GetCustomAttributes<ImportAttribute>().Any()))
                {
                    if (t.Value.GetCustomAttributes<ExportAttribute>().Any())
                    {
                        Type objType = t.Value.GetCustomAttribute<ExportAttribute>().AttributeType;
                        if (objType != null && !objType.Equals(p.PropertyType))
                            throw new TypeMismatchObjectAndProperty(string.Format("Несоответствие типов обхекта ({0}) и свойства ({1})", objType.Name, p.PropertyType.Name));
                    }

                    object objProp = CreateInstance(t.Value);

                    p.SetValue(obj, objProp);
                }

            return obj;
        }

        public T CreateInstance<T>()
        {
            return (T)CreateInstance(typeof(T));
        }
    }
}
