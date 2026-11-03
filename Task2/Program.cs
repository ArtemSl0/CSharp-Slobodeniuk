using System;

namespace Task2_Hunter
{
    class Hunter
    {
        public string Name { get; private set; }
        public int Health { get; private set; }
        public int Gold { get; private set; }
        public int Arrows { get; private set; }
        public int PreyCount { get; private set; }

        private static Random rnd = new Random();

        public Hunter(string name, int health, int gold, int arrows)
        {
            Name = name;
            Health = health;
            Gold = gold;
            Arrows = arrows;
            PreyCount = 0;
        }

        // Метод виведення інформації
        public void PrintInfo()
        {
            Console.WriteLine($"[{Name}] Здоров'я: {Health} | Золото: {Gold} | Стріли: {Arrows} | Здобич: {PreyCount}");
        }

        // Метод полювання
        public void Hunt()
        {
            if (Health <= 0)
            {
                Console.WriteLine($"{Name} занадто слабкий для полювання (Здоров'я: {Health}).");
                return;
            }
            if (Arrows <= 0)
            {
                Console.WriteLine($"{Name} не має стріл для полювання!");
                return;
            }

            Arrows--;
            Console.WriteLine($"{Name} вирушає на полювання та витрачає 1 стрілу...");

            int eventChance = rnd.Next(1, 101);

            if (eventChance <= 60)
            {
                int earnedGold = rnd.Next(10, 31);
                int damage = rnd.Next(0, 15);
                
                Gold += earnedGold;
                Health -= damage;
                PreyCount++;

                Console.WriteLine($"-> Успіх! {Name} вполював здобич. Отримано золота: {earnedGold}. Втрачено здоров'я: {damage}.");
            }
            else
            {
                int damage = rnd.Next(5, 25);
                Health -= damage;
                Console.WriteLine($"-> Невдача... Здобич втекла. {Name} поранився. Втрачено здоров'я: {damage}.");
            }
        }

        public void BuyArrows(int count)
        {
            int costPerArrow = 2;
            int totalCost = count * costPerArrow;

            if (Gold >= totalCost)
            {
                Gold -= totalCost;
                Arrows += count;
                Console.WriteLine($"{Name} успішно купив {count} стріл за {totalCost} золота.");
            }
            else
            {
                Console.WriteLine($"[{Name}] Недостатньо золота! Потрібно {totalCost}, а є лише {Gold}.");
            }
        }
    }

    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            
            Hunter player = new Hunter("Артем", 100, 20, 5);
            Hunter rival = new Hunter("Бот-Суперник", 100, 20, 5);

            Console.WriteLine("=== Гра: Змагання Мисливців ===");
            Console.WriteLine("Початкова інформація:");
            player.PrintInfo();
            rival.PrintInfo();
            Console.WriteLine("===============================\n");

            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\nОберіть дію для вашого мисливця:");
                Console.WriteLine("1. Полювати");
                Console.WriteLine("2. Купити стріли (5 шт. за 10 золота)");
                Console.WriteLine("3. Показати поточну інформацію");
                Console.WriteLine("4. Завершити змагання (підбити підсумки)");
                Console.Write("Ваш вибір: ");

                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        player.Hunt();
                        // Суперник теж автоматично робить хід
                        Console.WriteLine("\nХід суперника:");
                        if (rival.Arrows > 0) rival.Hunt();
                        else rival.BuyArrows(5);
                        break;
                    case "2":
                        player.BuyArrows(5);
                        Console.WriteLine("\nХід суперника:");
                        if (rival.Arrows < 2 && rival.Gold >= 10) rival.BuyArrows(5);
                        else rival.Hunt();
                        break;
                    case "3":
                        Console.WriteLine("--- Поточний стан ---");
                        player.PrintInfo();
                        rival.PrintInfo();
                        break;
                    case "4":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Невірний вибір. Спробуйте ще раз.");
                        break;
                }
                
                if (player.Health <= 0 && rival.Health <= 0)
                {
                    Console.WriteLine("\nОбидва мисливці виснажені. Змагання завершено достроково!");
                    exit = true;
                }
            }

            Console.WriteLine("\n=== РЕЗУЛЬТАТИ ЗМАГАННЯ ===");
            player.PrintInfo();
            rival.PrintInfo();

            Console.WriteLine("\nПорівняння здобичі:");
            Console.WriteLine($"{player.Name}: {player.PreyCount} шт.");
            Console.WriteLine($"{rival.Name}: {rival.PreyCount} шт.");

            if (player.PreyCount > rival.PreyCount)
            {
                Console.WriteLine($"\n🏆 ПЕРЕМОЖЕЦЬ: {player.Name}!");
            }
            else if (rival.PreyCount > player.PreyCount)
            {
                Console.WriteLine($"\n🏆 ПЕРЕМОЖЕЦЬ: {rival.Name}!");
            }
            else
            {
                Console.WriteLine("\n🤝 НІЧИЯ! Обидва мисливці отримали однакову кількість здобичі.");
            }
        }
    }
}
