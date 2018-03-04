using System;
using System.Linq;
using System.Reflection;
using VideoIoC;
using VideoLib;

namespace VideoConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Assembly assembly = Assembly.Load("VideoLib");
            Console.WriteLine(assembly.FullName);
            Console.WriteLine("------------");
            Console.WriteLine();

            Console.WriteLine("классы помеченные ImportConstructorAttribute");
            var types1 = assembly.GetTypes()
                .Where(t => t.IsClass && t.GetCustomAttributes<ImportConstructorAttribute>().Any());
            foreach (Type t in types1)
                Console.WriteLine(t.FullName);
            Console.WriteLine();

            Console.WriteLine("классы со свойствами помеченными ImportAttribute");
            var types2 = assembly.GetTypes()
                .Where(t => t.IsClass && t.GetProperties().Any(p => p.GetCustomAttributes<ImportAttribute>().Any()));
            foreach (Type t in types2)
            {
                Console.WriteLine(t.FullName);
                var propType = t.GetProperties().Where(p => p.GetCustomAttributes<ImportAttribute>().Any());
                foreach (PropertyInfo pt in propType)
                    Console.WriteLine(" - " + pt.Name + " - " + pt.PropertyType);

            }
            Console.WriteLine();

            Console.WriteLine("интерфейсы");
            var types3 = assembly.GetTypes()
                .Where(t => t.IsInterface);
            foreach (Type t in types3)
                Console.WriteLine(t.FullName);
            Console.WriteLine();

            Console.WriteLine("классы помеченные ExportAttribute");
            var types4 = assembly.GetTypes()
                .Where(t => t.IsClass && t.GetCustomAttributes<ExportAttribute>().Any() && t.GetInterfaces().Any(i => i == typeof(IVideoProvider)));
            foreach (Type t in types4)
                Console.WriteLine(t.FullName);
            Console.WriteLine();

            Console.WriteLine("атрибут ImportConstructorAttribute класса VideoManager2");
            var Attribs = typeof(VideoManager2).GetCustomAttributes<ImportConstructorAttribute>();
            foreach (Attribute a in Attribs)
                Console.WriteLine(a.GetType());
            Console.WriteLine();

            Console.WriteLine("свойства VideoManager1 помеченные ImportAttribute");
            var prop = typeof(VideoManager1).GetProperties().Where(p => p.GetCustomAttributes<ImportAttribute>().Any());
            foreach (PropertyInfo p in prop)
                Console.WriteLine(p.Name);
            Console.WriteLine();

            Console.WriteLine("Конструктор VideoManager2");
            var constr = typeof(VideoManager2).GetConstructors();
            foreach (ConstructorInfo c in constr)
            {
                Console.WriteLine(c.Name);
                var constrPar = c.GetParameters();
                foreach (ParameterInfo p in constrPar)
                    Console.WriteLine(" - " + p.Name);
            }
            Console.WriteLine();

            Container container = new Container();
            container.AddAssembly(assembly);

            Console.WriteLine("создаем объект VideoManager1 через публичные свойства ");
            VideoManager1 videoManager1 = (VideoManager1)container.CreateInstance(typeof(VideoManager1));
            videoManager1.VideoProvider.PlayVideo();
            Console.WriteLine();

            Console.WriteLine("создаем объект VideoManager2 через конструктор");
            VideoManager2 videoManager2 = (VideoManager2)container.CreateInstance(typeof(VideoManager2));
            videoManager2.VideoProvider.PlayVideo();
            Console.WriteLine();

            Console.ReadKey();
        }
    }
}
