using System;
using System.Threading;
using ORM;
using DAL;
using Web;
using Microsoft.Practices.Unity;

namespace Patterns
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Запущен процесс загрузки пользователей, с периодичностью в 5 минут:");
            string url = @"https://jsonplaceholder.typicode.com/users";

            using (UnityContainer unityContainer = new UnityContainer())
            {
                unityContainer.RegisterType<IAddressFactory, AddressFactory>();
                unityContainer.RegisterType<IUserFactory, UserFactory>();
                unityContainer.RegisterType<IProxy, DBUsersProxy>();
                unityContainer.RegisterType<IRepository<User>, UserRepository>();
                unityContainer.RegisterType<IService<UserExternal, User>, UserService>();
                unityContainer.RegisterType<UserExternal>();
                unityContainer.RegisterType<MyDBUsers>();

                using (var userService = unityContainer.Resolve<IService<UserExternal, User>>())
                {
                    while (true)
                    {
                        var allUserByUrl = userService.GetAllByUrl(url);
                        var usersConvertedForDb = userService.ConvertForDb(allUserByUrl);
                        userService.SaveAllToDb(usersConvertedForDb);
                        Console.WriteLine("Произведена очередная загрузка... для завершения нажмите Ctrl+C");
                        Thread.Sleep(1000 * 60 * 1);
                    }

                }
            }
        }
    }
}
