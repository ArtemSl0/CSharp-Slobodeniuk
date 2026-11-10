using System;
using System.Collections.Generic;
using System.Linq;

namespace DeliveryModule
{
    public interface IDeliveryService
    {
        void AddTransport(Transport transport);
        void CreateOrder(Order order);
        void ShowAvailableTransportsWithCost(Order order);
        void AssignOrder(Order order, Transport transport);
        void DeliverOrder(Order order, Transport transport, string paymentMethod);
        void ShowActiveOrders();
        void ShowHistory();
    }

    public class Order
    {
        public int Number { get; set; }
        public string Description { get; set; }
        public double Weight { get; set; }
        public double Distance { get; set; }

        public override string ToString() => $"Замовлення #{Number} ({Description}: {Weight} кг, {Distance} км)";
    }

    public abstract class Transport
    {
        public string Name { get; set; }
        public double MaxWeight { get; set; }
        public bool IsBusy { get; set; } = false;

        public Transport(string name, double maxWeight)
        {
            Name = name;
            MaxWeight = maxWeight;
        }

        public abstract decimal CalculateCost(Order order);

        public virtual bool CanTake(Order order)
        {
            return order.Weight <= MaxWeight;
        }
    }

    public class Bike : Transport
    {
        public Bike(string name, double maxWeight = 15) : base(name, maxWeight) { }

        public override decimal CalculateCost(Order order)
        {
            return 20m + 5m * (decimal)order.Distance;
        }

        public override bool CanTake(Order order)
        {
            return base.CanTake(order) && order.Distance <= 10;
        }
    }

    public class Car : Transport
    {
        public Car(string name, double maxWeight = 500) : base(name, maxWeight) { }

        public override decimal CalculateCost(Order order)
        {
            return 50m + 8m * (decimal)order.Distance + 1m * (decimal)order.Weight;
        }
    }

    public class Truck : Transport
    {
        public Truck(string name, double maxWeight = 10000) : base(name, maxWeight) { }

        public override decimal CalculateCost(Order order)
        {
            return 200m + 15m * (decimal)order.Distance + 0.5m * (decimal)order.Weight;
        }
    }

    public class DeliveryService : IDeliveryService
    {
        private List<Transport> _transports = new List<Transport>();
        private List<Order> _pendingOrders = new List<Order>();
        private Dictionary<Order, Transport> _activeDeliveries = new Dictionary<Order, Transport>();
        private List<string> _history = new List<string>();

        public void AddTransport(Transport transport) => _transports.Add(transport);
        public void CreateOrder(Order order) => _pendingOrders.Add(order);

        public void ShowAvailableTransportsWithCost(Order order)
        {
            Console.WriteLine($"\n--- Розрахунок для: {order} ---");
            var available = _transports.Where(t => t.CanTake(order)).ToList();
            
            if (!available.Any())
            {
                Console.WriteLine("Немає транспорту, який може взяти це замовлення за обмеженнями.");
                return;
            }

            foreach (var t in available)
            {
                Console.WriteLine($"- {t.Name} (Вільний: {!t.IsBusy}): {t.CalculateCost(order)} грн");
            }
        }

        public void AssignOrder(Order order, Transport transport)
        {
            if (transport.IsBusy)
            {
                Console.WriteLine($"\n[Помилка] Транспорт '{transport.Name}' зараз зайнятий.");
                return;
            }
            if (!transport.CanTake(order))
            {
                Console.WriteLine($"\n[Помилка] Транспорт '{transport.Name}' не може взяти {order.Number} через обмеження.");
                return;
            }

            transport.IsBusy = true;
            _pendingOrders.Remove(order);
            _activeDeliveries.Add(order, transport);
            Console.WriteLine($"\n[Призначення] Замовлення #{order.Number} закріплено за '{transport.Name}'. Транспорт став зайнятим.");
        }

        public void DeliverOrder(Order order, Transport transport, string paymentMethod)
        {
            if (_activeDeliveries.ContainsKey(order) && _activeDeliveries[order] == transport)
            {
                decimal cost = transport.CalculateCost(order);
                transport.IsBusy = false;
                _activeDeliveries.Remove(order);
                
                string record = $"Замовлення #{order.Number} доставлено ('{transport.Name}'). Оплата: {paymentMethod}, Сума: {cost} грн.";
                _history.Add(record);
                Console.WriteLine($"\n[Доставка Успішна] {record} Транспорт звільнився.");
            }
            else
            {
                Console.WriteLine($"\n[Помилка] Замовлення #{order.Number} не перебуває в доставці у транспорту '{transport.Name}'.");
            }
        }

        public void ShowActiveOrders()
        {
            Console.WriteLine("\n--- Активні замовлення ---");
            if (!_activeDeliveries.Any()) Console.WriteLine("Наразі немає активних доставок.");
            foreach (var kvp in _activeDeliveries)
            {
                Console.WriteLine($"{kvp.Key} -> Доставляє: {kvp.Value.Name}");
            }
        }

        public void ShowHistory()
        {
            Console.WriteLine("\n--- Історія доставок ---");
            if (!_history.Any()) Console.WriteLine("Історія порожня.");
            foreach (var record in _history)
            {
                Console.WriteLine(record);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== Модульна робота №1: Служба доставки ===\n");

            var service = new DeliveryService();

            var bike = new Bike("Велокур'єр", maxWeight: 10);
            var car = new Car("Легкове авто", maxWeight: 400);
            var truck = new Truck("Вантажівка MAN", maxWeight: 8000);
            
            service.AddTransport(bike);
            service.AddTransport(car);
            service.AddTransport(truck);

            var order1 = new Order { Number = 1, Description = "Піца", Weight = 1.5, Distance = 5 };
            var order2 = new Order { Number = 2, Description = "Телевізор", Weight = 25, Distance = 8 };
            var order3 = new Order { Number = 3, Description = "Ноутбук", Weight = 2, Distance = 15 };
            var order4 = new Order { Number = 4, Description = "Меблі", Weight = 1500, Distance = 50 };

            service.CreateOrder(order1);
            service.CreateOrder(order2);
            service.CreateOrder(order3);
            service.CreateOrder(order4);

            service.ShowAvailableTransportsWithCost(order1);
            service.ShowAvailableTransportsWithCost(order2);
            service.ShowAvailableTransportsWithCost(order3);
            service.ShowAvailableTransportsWithCost(order4);

            service.AssignOrder(order1, bike);
            service.AssignOrder(order2, car);
            service.AssignOrder(order4, truck);

            service.AssignOrder(order3, bike);

            service.ShowActiveOrders();

            service.DeliverOrder(order1, bike, "Карткою");
            service.DeliverOrder(order4, truck, "Безготівковий рахунок");

            service.AssignOrder(order3, car);
            service.DeliverOrder(order2, car, "Готівкою");
            service.AssignOrder(order3, car);

            service.ShowActiveOrders();
            service.ShowHistory();
            
            Console.WriteLine("\n=== Симуляцію завершено ===");
            Console.ReadLine();
        }
    }
}
