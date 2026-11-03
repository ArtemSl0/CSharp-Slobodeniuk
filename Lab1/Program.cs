using System;

namespace Lab1_Slobodeniuk_Artem_PD21
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("Лабораторна робота №1. Робота з трикутником");

            double a = ReadSide("a");
            double b = ReadSide("b");
            double c = ReadSide("c");

            if (!AreSidesPositive(a, b, c))
            {
                Console.WriteLine("Помилка: Сторони повинні бути додатними числами.");
                return;
            }

            if (!IsTriangleValid(a, b, c))
            {
                Console.WriteLine("Помилка: Трикутник з такими сторонами не може існувати (сума двох сторін має бути більша за третю).");
                return;
            }

            double perimeter = CalculatePerimeter(a, b, c);
            double area = CalculateArea(a, b, c);
            string type = DetermineTriangleType(a, b, c);

            Console.WriteLine("\nРезультати:");
            Console.WriteLine($"Периметр: {perimeter}");
            Console.WriteLine($"Площа: {area}");
            Console.WriteLine($"Вид трикутника: {type}");
        }

        static double ReadSide(string sideName)
        {
            while (true)
            {
                Console.Write($"Введіть сторону {sideName}: ");
                if (double.TryParse(Console.ReadLine(), out double side))
                {
                    return side;
                }
                Console.WriteLine("Некоректне значення. Будь ласка, введіть число.");
            }
        }

        static bool AreSidesPositive(double a, double b, double c)
        {
            return a > 0 && b > 0 && c > 0;
        }

        static bool IsTriangleValid(double a, double b, double c)
        {
            return (a + b > c) && (a + c > b) && (b + c > a);
        }

        static double CalculatePerimeter(double a, double b, double c)
        {
            return a + b + c;
        }

        static double CalculateArea(double a, double b, double c)
        {
            double p = CalculatePerimeter(a, b, c) / 2.0; // Півпериметр
            return Math.Sqrt(p * (p - a) * (p - b) * (p - c));
        }

        static string DetermineTriangleType(double a, double b, double c)
        {
            if (a == b && b == c)
            {
                return "рівносторонній";
            }
            
            bool isRightAngled = IsRightAngled(a, b, c);
            bool isIsosceles = (a == b) || (b == c) || (a == c);

            if (isRightAngled && isIsosceles) return "рівнобедрений прямокутний";
            if (isRightAngled) return "прямокутний";
            if (isIsosceles) return "рівнобедрений";

            return "довільний";
        }

        static bool IsRightAngled(double a, double b, double c)
        {
            double[] sides = { a, b, c };
            Array.Sort(sides); 
            
            double tolerance = 0.0001; 
            return Math.Abs(Math.Pow(sides[0], 2) + Math.Pow(sides[1], 2) - Math.Pow(sides[2], 2)) < tolerance;
        }
    }
}
