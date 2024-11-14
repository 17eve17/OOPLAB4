using System;
using System.Collections.Generic;
using System.Linq;

public interface ISearchable
{
    List<Product> SearchByPrice(double minPrice, double maxPrice);
    List<Product> SearchByCategory(string category);
    List<Product> SearchByRating(double minRating);
}

public class Product
{
    public string Title { get; set; }
    public double Price { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public double Rating { get; set; } 

    public Product(string title, double price, string description, string category, double rating)
    {
        Title = title;
        Price = price;
        Description = description;
        Category = category;
        Rating = rating;
    }

    public override string ToString()
    {
        return $"{Title} ({Category}) - {Price} грн, рейтинг: {Rating}/5";
    }
}

public class User
{
    public string Username { get; set; }
    public string Password { get; set; }
    public List<Order> OrderHistory { get; set; }

    public User(string username, string password)
    {
        Username = username;
        Password = password;
        OrderHistory = new List<Order>();
    }

    public void AddOrder(Order order)
    {
        OrderHistory.Add(order);
    }
}

public class Order
{
    public List<Product> Products { get; set; }
    public double TotalPrice { get; set; }
    public string Status { get; set; } 

    public Order(List<Product> products)
    {
        Products = products;
        TotalPrice = products.Sum(p => p.Price);
        Status = "Прийнято";
    }

    public override string ToString()
    {
        return $"Замовлення: {Products.Count} товарів, загальна вартість: {TotalPrice} грн, статус: {Status}";
    }
}

public class Store : ISearchable
{
    public List<Product> Products { get; set; }
    public List<User> Users { get; set; }

    public Store()
    {
        Products = new List<Product>();
        Users = new List<User>();
    }

    public void AddProduct(Product product)
    {
        Products.Add(product);
    }

    public void AddUser(User user)
    {
        Users.Add(user);
    }

    public List<Product> SearchByPrice(double minPrice, double maxPrice)
    {
        return Products.Where(p => p.Price >= minPrice && p.Price <= maxPrice).ToList();
    }

    public List<Product> SearchByCategory(string category)
    {
        return Products.Where(p => p.Category.Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public List<Product> SearchByRating(double minRating)
    {
        return Products.Where(p => p.Rating >= minRating).ToList();
    }

    public void DisplayProducts(List<Product> products)
    {
        if (products.Count == 0)
        {
            Console.WriteLine("Жодних товарів не знайдено.");
            return;
        }

        foreach (var product in products)
        {
            Console.WriteLine(product);
        }
    }

    public void MakePurchase(User user, List<Product> products)
    {
        var order = new Order(products);
        user.AddOrder(order);
        Console.WriteLine("Замовлення успішно оформлене!");
        Console.WriteLine(order);
    }
}

public class Program
{
    public static void Main()
    {
        var store = new Store();

        store.AddProduct(new Product("Книга 1", 150, "Це цікава книга про програмування.", "Книги", 4.5));
        store.AddProduct(new Product("Книга 2", 200, "Книга з історії України.", "Книги", 4.7));
        store.AddProduct(new Product("Книга 3", 120, "Навчальний посібник по математиці.", "Навчальні", 3.8));
        store.AddProduct(new Product("Книга 4", 180, "Роман для підлітків.", "Романи", 4.2));

        var user = new User("ivan", "password123");
        store.AddUser(user);

        while (true)
        {
            Console.WriteLine("\n---- Меню ----");
            Console.WriteLine("1. Пошук товарів за ціною");
            Console.WriteLine("2. Пошук товарів за категорією");
            Console.WriteLine("3. Пошук товарів за рейтингом");
            Console.WriteLine("4. Оформити замовлення");
            Console.WriteLine("5. Переглянути історію покупок");
            Console.WriteLine("6. Вийти");
            Console.Write("Виберіть опцію: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Write("Введіть мінімальну ціну: ");
                    double minPrice = Convert.ToDouble(Console.ReadLine());
                    Console.Write("Введіть максимальну ціну: ");
                    double maxPrice = Convert.ToDouble(Console.ReadLine());
                    var productsByPrice = store.SearchByPrice(minPrice, maxPrice);
                    store.DisplayProducts(productsByPrice);
                    break;

                case "2":
                    Console.Write("Введіть категорію товару: ");
                    string category = Console.ReadLine();
                    var productsByCategory = store.SearchByCategory(category);
                    store.DisplayProducts(productsByCategory);
                    break;

                case "3":
                    Console.Write("Введіть мінімальний рейтинг: ");
                    double minRating = Convert.ToDouble(Console.ReadLine());
                    var productsByRating = store.SearchByRating(minRating);
                    store.DisplayProducts(productsByRating);
                    break;

                case "4":
                    Console.WriteLine("Оберіть товари для замовлення:");
                    store.DisplayProducts(store.Products);
                    Console.Write("Введіть індекси товарів через кому (наприклад: 0,1): ");
                    var indexes = Console.ReadLine().Split(',');
                    List<Product> selectedProducts = new List<Product>();

                    foreach (var index in indexes)
                    {
                        int idx = Convert.ToInt32(index.Trim());
                        if (idx >= 0 && idx < store.Products.Count)
                        {
                            selectedProducts.Add(store.Products[idx]);
                        }
                    }

                    store.MakePurchase(user, selectedProducts);
                    break;

                case "5":
                    Console.WriteLine("Історія покупок:");
                    foreach (var order in user.OrderHistory)
                    {
                        Console.WriteLine(order);
                    }
                    break;

                case "6":
                    Console.WriteLine("До побачення!");
                    return;

                default:
                    Console.WriteLine("Невірний вибір. Спробуйте ще раз.");
                    break;
            }
        }
    }
}
