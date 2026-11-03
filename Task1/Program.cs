using System;

namespace Task1_Slobodeniuk_Artem_PD21
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("Завдання 1. Середній бал у групах студентів (зубчастий масив)\n");

            Random random = new Random();
            
            int numGroups = random.Next(3, 6); 
            int[][] grades = new int[numGroups][];

            for (int i = 0; i < numGroups; i++)
            {
                int numStudents = random.Next(10, 31); 
                grades[i] = new int[numStudents];
                
                for (int j = 0; j < numStudents; j++)
                {
                    grades[i][j] = random.Next(1, 101);
                }
            }

            int streamMin = int.MaxValue;
            int streamMax = int.MinValue;
            double streamSum = 0;
            int streamTotalStudents = 0;

            Console.WriteLine("=== Статистика по кожній групі ===");

            for (int i = 0; i < grades.Length; i++)
            {
                int groupMin = int.MaxValue;
                int groupMax = int.MinValue;
                double groupSum = 0;

                for (int j = 0; j < grades[i].Length; j++)
                {
                    int grade = grades[i][j];
                    
                    if (grade < groupMin) groupMin = grade;
                    if (grade > groupMax) groupMax = grade;
                    groupSum += grade;

                    if (grade < streamMin) streamMin = grade;
                    if (grade > streamMax) streamMax = grade;
                    streamSum += grade;
                }
                
                streamTotalStudents += grades[i].Length;
                double groupAvg = groupSum / grades[i].Length;

                Console.WriteLine($"Група {i + 1} (Студентів: {grades[i].Length}):");
                Console.WriteLine($"- Середній бал: {groupAvg:F2}");
                Console.WriteLine($"- Мінімальна оцінка: {groupMin}");
                Console.WriteLine($"- Максимальна оцінка: {groupMax}\n");
            }

            double streamAvg = streamSum / streamTotalStudents;
            Console.WriteLine("=== Статистика по всьому потоку ===");
            Console.WriteLine($"Загальна кількість студентів: {streamTotalStudents}");
            Console.WriteLine($"- Середній бал: {streamAvg:F2}");
            Console.WriteLine($"- Мінімальна оцінка: {streamMin}");
            Console.WriteLine($"- Максимальна оцінка: {streamMax}");
        }
    }
}
