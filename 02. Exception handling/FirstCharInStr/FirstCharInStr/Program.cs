using System;

namespace FirstCharInStr
{
    class Program
    {
        static void Main(string[] args)
        {
            string s;
            while (true)
            {
                Console.WriteLine("Введите любую строку. Для завершения работы введите exit:");
                s = Console.ReadLine();
                if (s.ToLower().Equals("exit")) break;
                else
                    try
                    {
                        Console.WriteLine("Первый символ введенной строки: {0}", s[0]);
                    }
                    catch (IndexOutOfRangeException e)
                    {
                        Console.WriteLine("Перехвачено исключение IndexOutOfRangeException - вы ввели пустую строку. Повторите попытку.");
                    }
            }
        }
    }
}
