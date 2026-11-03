using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab4_LibrarySystem
{
    class Book
    {
        public string Id { get; private set; }
        public string Title { get; private set; }
        public string Author { get; private set; }
        public bool IsAvailable { get; private set; }

        public Book(string id, string title, string author)
        {
            Id = id;
            Title = title;
            Author = author;
            IsAvailable = true;
        }

        public void SetAvailable(bool status)
        {
            IsAvailable = status;
        }

        public override string ToString()
        {
            string status = IsAvailable ? "Доступна" : "В оренді";
            return $"[ID: {Id}] \"{Title}\" - {Author} ({status})";
        }
    }

    class User
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        
        private List<Book> _rentedBooks;
        public IReadOnlyList<Book> RentedBooks => _rentedBooks.AsReadOnly();

        public User(string id, string name)
        {
            Id = id;
            Name = name;
            _rentedBooks = new List<Book>();
        }

        public void RentBook(Book book)
        {
            _rentedBooks.Add(book);
        }

        public void ReturnBook(Book book)
        {
            _rentedBooks.Remove(book);
        }

        public void ShowRentedBooks()
        {
            Console.WriteLine($"\nКниги в оренді у користувача {Name}:");
            if (_rentedBooks.Count == 0)
            {
                Console.WriteLine("- Немає орендованих книг.");
                return;
            }
            foreach (var book in _rentedBooks)
            {
                Console.WriteLine($"- {book.Title}");
            }
        }
    }

    class Library
    {
        private List<Book> _books;
        private List<User> _users;

        public Library()
        {
            _books = new List<Book>();
            _users = new List<User>();
        }

        public void AddBook(Book book) => _books.Add(book);
        public void AddUser(User user) => _users.Add(user);

        public void RentBookToUser(string bookId, string userId)
        {
            var book = _books.FirstOrDefault(b => b.Id == bookId);
            var user = _users.FirstOrDefault(u => u.Id == userId);

            if (book == null)
            {
                Console.WriteLine($"[Помилка] Книгу з ID {bookId} не знайдено.");
                return;
            }
            if (user == null)
            {
                Console.WriteLine($"[Помилка] Користувача з ID {userId} не знайдено.");
                return;
            }
            if (!book.IsAvailable)
            {
                Console.WriteLine($"[Відмова] Книгу \"{book.Title}\" вже видано іншому користувачу.");
                return;
            }

            book.SetAvailable(false);
            user.RentBook(book);
            Console.WriteLine($"[Успіх] Книгу \"{book.Title}\" успішно видано користувачу {user.Name}.");
        }

        public void ReturnBookFromUser(string bookId, string userId)
        {
            var book = _books.FirstOrDefault(b => b.Id == bookId);
            var user = _users.FirstOrDefault(u => u.Id == userId);

            if (book == null || user == null) return;

            if (!user.RentedBooks.Contains(book))
            {
                Console.WriteLine($"[Помилка] Користувач {user.Name} не орендував книгу \"{book.Title}\".");
                return;
            }

            book.SetAvailable(true);
            user.ReturnBook(book);
            Console.WriteLine($"[Успіх] Користувач {user.Name} повернув книгу \"{book.Title}\".");
        }

        public void ShowAvailableBooks()
        {
            Console.WriteLine("\n--- Список доступних книг ---");
            var available = _books.Where(b => b.IsAvailable).ToList();
            if (available.Count == 0) Console.WriteLine("Всі книги наразі в оренді.");
            foreach (var b in available) Console.WriteLine(b);
        }
    }

    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Library library = new Library();

            library.AddBook(new Book("B1", "1984", "Джордж Орвелл"));
            library.AddBook(new Book("B2", "Кобзар", "Тарас Шевченко"));
            library.AddBook(new Book("B3", "Тигроволови", "Іван Багряний"));

            library.AddUser(new User("U1", "Артем Слободенюк"));
            library.AddUser(new User("U2", "Іван Франко"));

            Console.WriteLine("Систему ініціалізовано. Книги та користувачів додано.");

            library.ShowAvailableBooks();

            Console.WriteLine("\n--- Спроба видати книгу ---");
            library.RentBookToUser("B1", "U1");

            Console.WriteLine("\n--- Спроба повторної видачі тієї ж книги ---");
            library.RentBookToUser("B1", "U2");

            library.ShowAvailableBooks();

            Console.WriteLine("\n--- Повернення книги ---");
            library.ReturnBookFromUser("B1", "U1");

            library.ShowAvailableBooks();
        }
    }
}
