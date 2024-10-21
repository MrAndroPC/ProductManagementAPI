
using Microsoft.EntityFrameworkCore;
using ProductManagementAPI.Models;

namespace ProductManagementAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();


            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<ApplicationDbContext>();
                var env = services.GetRequiredService<IWebHostEnvironment>();

                // Проводим миграции на старте
                context.Database.Migrate();

                // Заполняем тестоыми данными бд в дев моде
                if (env.IsDevelopment())
                {
                    SeedTestData(context);
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static void SeedTestData(ApplicationDbContext context)
        {
            // Не заполняем бд если уже есть данные
            if (!context.ProductCategories.Any())
            {
                var categories = new List<ProductCategory>
                {
                new ProductCategory { Name = "Electronics", Description = "Electronic gadgets and devices" },
                new ProductCategory { Name = "Books", Description = "Wide variety of books" },
                new ProductCategory { Name = "Clothing", Description = "Clothing and accessories" },
                new ProductCategory { Name = "Test Category", Description = "This is a test category."}
                };

                context.ProductCategories.AddRange(categories);
                context.SaveChanges();
            }
            if (!context.Products.Any())
            {
                var electronicsCategory = context.ProductCategories.First(c => c.Name == "Electronics");
                var booksCategory = context.ProductCategories.First(c => c.Name == "Books");
                var testCategory = context.ProductCategories.First(c => c.Name == "Test Category");

                var products = new List<Product>
                {
                new Product
                {
                    Name = "Smartphone",
                    Description = "Latest model with advanced features",
                    Price = 999.99m,
                    CategoryId = electronicsCategory.Id
                },
                new Product
                {
                    Name = "Laptop",
                    Description = "Powerful laptop for professionals",
                    Price = 1500.00m,
                    CategoryId = electronicsCategory.Id
                },
                new Product
                {
                    Name = "Science Fiction Novel",
                    Description = "A captivating science fiction novel",
                    Price = 15.99m,
                    CategoryId = booksCategory.Id
                },
                new Product
                {
                    Name = "Test Product",
                    Description = "This is a test product.",
                    Price = 19.99m,
                    CategoryId = testCategory.Id
                }};

                context.Products.AddRange(products);
                context.SaveChanges();
            }
        }
    }
}
