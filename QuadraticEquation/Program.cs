using System.Collections;

namespace QuadraticEquation
{
    internal class Program
    {          
        static void Main(string[] args)
        {
            Console.WriteLine("a * x^2 + b * x + c = 0");
            while (true)
            {
                try
                {
                    List<int> ints = GetUserInput("a", "b", "c");
                    FindRoots(ints);
                }
                catch (FormatException e)
                {
                    FormatData(e.Message, Severity.Error, e.Data);
                }               
                catch (Exception e)
                {
                    FormatData(e.Message, Severity.Warning, null);
                }
            }
        }

        public static List<int> GetUserInput(params string[] strings)
        {
            bool noError = true;
            Console.Write("Введите коэффициент a: ");
            string aStr = Console.ReadLine();
            Console.Write("Введите коэффициент b: ");
            string bStr = Console.ReadLine();
            Console.Write("Введите коэффициент c: ");
            string cStr = Console.ReadLine();

            var list = new List<int>();
            var inputList = new Dictionary<string, string>()
                {
                    {"a", aStr },
                    {"b", bStr },
                    {"c", cStr }
                };

            if (aStr == "0")
            {
                Console.BackgroundColor = ConsoleColor.Magenta;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Коэффициент a не может быть равен 0");
                Console.ResetColor();
            }
            foreach (var pair in inputList)
            {
                if (!int.TryParse(pair.Value, out int result))
                {
                    noError = false;
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.ForegroundColor = ConsoleColor.White;
                    string separator = new string('-', 50);
                    Console.WriteLine(separator);
                    Console.WriteLine($"Неверный формат коэффициента {pair.Key}");
                    Console.ResetColor();

                    try
                    {
                        checked
                        {
                            bool longResult = long.TryParse(pair.Value, out var parsed);
                            int value = (int)parsed;
                        }
                    }
                    catch (OverflowException)
                    {
                        Console.BackgroundColor = ConsoleColor.Green;
                        Console.ForegroundColor = ConsoleColor.Black;                        
                        Console.WriteLine($"{pair.Key} = {pair.Value}");
                        Console.WriteLine("Введите значение в пределах от -2147483648 до 2147483647");
                    }
                }               

                else
                {
                    list.Add(result);
                }
            }

            if (!noError)
            {
                var e = new FormatException("");
                e.Data.Add("a", aStr);
                e.Data.Add("b", bStr);
                e.Data.Add("c", cStr);
                throw e;
            }
            return list;
        }

        static void FormatData(string message, Severity severity, IDictionary data)
        {
            string separator = new string('-', 50);

            if (severity == Severity.Error)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(separator);
                Console.WriteLine(message);

                foreach (DictionaryEntry entry in data)
                {
                    Console.WriteLine("{0} = {1}", entry.Key, entry.Value);
                }
                Console.ResetColor();
            }
            else if (severity == Severity.Warning)
            {
                Console.BackgroundColor = ConsoleColor.Yellow;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine(separator);
                Console.WriteLine(message);
                Console.ResetColor();
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.Green;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine(separator);
                Console.WriteLine(message);
                foreach (DictionaryEntry entry in data)
                {
                    Console.WriteLine("{0} = {1}", entry.Key, entry.Value);
                }
                Console.ResetColor();
            }
        }

        static void FindRoots(List<int> ints)
        {
            int a = ints[0];
            int b = ints[1];
            int c = ints[2];

            double D = b * b - 4 * a * c;

            double scrtD = Math.Sqrt(D);

            if (D > 0)
            {
                double[] twoRoots = new double[2];
                twoRoots[0] = (-b + scrtD) / (2 * a);
                twoRoots[1] = (-b - scrtD) / (2 * a);
                Console.WriteLine($"x1 = {twoRoots[0]}, x2 = {twoRoots[1]}");
            }
            else if (D == 0)
            {
                double[] oneRoot = new double[1];
                oneRoot[0] = (-b + scrtD) / (2 * a);
                Console.WriteLine($"x = {oneRoot[0]}");
            }
            else
            {
                var e = new NoRootsException("Вещественных значений не найдено");
                throw e;
            }
        }
    }
}