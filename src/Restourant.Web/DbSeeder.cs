using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Restaurant.Data;
using Restaurant.Data.Contracts;
using Restaurant.Data.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Restourant.Web
{
    public class DbSeeder
    {
        private static readonly string AdminRole = "Admin";
        private static readonly string WaiterRole = "Waiter";
        private static readonly string BarmanRole = "Barman";
        private static readonly string ChefRole = "Chef";

        public async Task SeedAsync()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider(true);

            using (var serviceScope = serviceProvider.CreateScope())
            {
                serviceProvider = serviceScope.ServiceProvider;
                await SeedDatabaseAsync(serviceProvider);
            }

        }

        private static async Task SeedDatabaseAsync(IServiceProvider serviceProvider)
        {
            SeedDefaultAdminRolesAndTwoUsers(serviceProvider);
            await SeedTables(serviceProvider);
            SeedCategories(serviceProvider);
            SeedProducts(serviceProvider);
            SeedOrderStatuses(serviceProvider);

        }

        private static async Task SeedTables(IServiceProvider serviceProvider)
        {
            var db = serviceProvider.GetService<RestaurantAppContext>();
            var petkoEmployee = await db.Users.FirstAsync(user => user.FirstName == "Petko");
            var ivanEmployee = await db.Users.FirstAsync(user => user.FirstName == "Ivan");

            if (!db.Tables.AnyAsync().Result)
            {
                var tables = new List<Table>();

                for (int i = 0; i < 5; i++)
                {
                    var table = new Table();
                    table.Name = $"Table №{i}";
                    if (i % 2 == 0)
                    {
                        table.UserId = petkoEmployee.Id;
                    }
                    else
                    {
                        table.UserId = ivanEmployee.Id;
                    }
                    tables.Add(table);
                }

                db.Tables.AddRange(tables);
                db.SaveChanges();

                Console.WriteLine("Successfully inserted 5 table entities into the database.");
            }
        }

        private static void SeedProducts(IServiceProvider serviceProvider)
        {
            var db = serviceProvider.GetService<RestaurantAppContext>();

            if (!db.Products.AnyAsync().Result)
            {
                var products = GetProducts(db);

                db.Products.AddRange(products);
                db.SaveChanges();

                Console.WriteLine("10 different products successfully inserted into the database!");
            }
        }

        private static void SeedCategories(IServiceProvider serviceProvider)
        {
            var db = serviceProvider.GetService<RestaurantAppContext>();

            if (!db.Categories.AnyAsync().Result)
            {
                var categories = GetCategories();

                db.Categories.AddRange(categories);
                db.SaveChanges();

                Console.WriteLine("All product categories have been seeded succesfully to the database!");
            }
        }

        private static void SeedOrderStatuses(IServiceProvider serviceProvider)
        {
            var db = serviceProvider.GetService<RestaurantAppContext>();

            if (!db.OrderStatuses.AnyAsync().Result)
            {
                var orderStatuses = GetOrderStatuses();

                db.OrderStatuses.AddRange(orderStatuses);
                db.SaveChanges();

                Console.WriteLine("All order statuses have been succesfully seeded to the database!");
            }
        }

        private static List<Product> GetProducts(RestaurantAppContext db)
        {
            var products = new List<Product>();

            var bar = db.Categories.First(c => c.Name == Restaurant.Data.Models.Enums.Categories.Bar);
            var kitchen = db.Categories.First(c => c.Name == Restaurant.Data.Models.Enums.Categories.Kitchen);
            var dessert = db.Categories.First(c => c.Name == Restaurant.Data.Models.Enums.Categories.Dessert);
            var other = db.Categories.First(c => c.Name == Restaurant.Data.Models.Enums.Categories.Other);
            var barSlow = db.Categories.First(c => c.Name == Restaurant.Data.Models.Enums.Categories.BarSlow);

            //Bar products
            var product0 = new Product() { Name = "Coca-Cola", Category = bar, Description = "gazirarno i vredno", Price = 1.20m, ImageUrl = "http://www.cantina-ola.com/media/7/21.jpg" };
            var product1 = new Product() { Name = "Pepsi", Category = bar, Description = "pak gazirano i vredno", Price = 1.30m, ImageUrl = "http://tuidagroup.com/2994-large_default/PEPSI-05L12.jpg" };
            var product2 = new Product() { Name = "Water", Category = bar, Description = "polezno i vkusno", Price = 1.00m, ImageUrl = "http://www.homebag.bg/media/7/8017.jpg" };

            //Kitchen products
            var product3 = new Product() { Name = "Musaka", Category = kitchen, Description = "Best meal in the world ever", Price = 5.60m, ImageUrl = "https://i.ytimg.com/vi/S8j6Tc-1APk/maxresdefault.jpg" };
            var product4 = new Product() { Name = "Gyuveche", Category = kitchen, Description = "Bulgarian traditional meal, kind of", Price = 5.20m, ImageUrl = "https://gotvach.bg/files/lib/600x350/ovcharsko-guveche-sirene.JPG" };
            var product5 = new Product() { Name = "Banitsa", Category = kitchen, Description = "Another Bulgarian traditional meal. Delicious!", Price = 5.50m, ImageUrl = "https://gotvach.bg/files/lib/600x350/spoluchliva-banica-sirene.JPG" };
            var product6 = new Product() { Name = "Chicken with rice", Category = kitchen, Description = "Cicken with rice only for post-traiing meals.", Price = 6.00m, ImageUrl = "https://recepti.ezine.bg/files/lib/500x350/pile-oriz-zvezdev.jpg" };

            //Dessert products
            var product7 = new Product() { Name = "Ice-cream", Category = dessert, Description = "ice-cream for sore throat", Price = 5.10m, ImageUrl = "https://recepti.gotvach.bg/files/lib/500x350/melba3.jpg" };
            var product8 = new Product() { Name = "Fruit salad", Category = dessert, Description = "best in vitamins and stuff", Price = 8.40m, ImageUrl = "http://assets.kulinaria.bg/attachments/pictures-images/0000/5689/MAIN-2015-07-31-11-14-39-0300-0-1-2.jpg?1438330483" };
            var product9 = new Product() { Name = "Pancakes", Category = dessert, Description = "tastiest pankaces ever lol", Price = 2.50m, ImageUrl = "https://ezine.bg/files/lib/600x350/palachinki18.jpg" };

            //Bar slow products
            var product10 = new Product() { Name = "Mojito", Category = barSlow, Description = "best Mojito ever to be made", Price = 10.90m, ImageUrl = "https://cdn.liquor.com/wp-content/uploads/2018/09/04153106/mojito-720x720-recipe.jpg" };
            var product11 = new Product() { Name = "Sex on the beach", Category = barSlow, Description = "best sex on the beach ever to be made lol", Price = 9.50m, ImageUrl = "https://makemeacocktail.com/images/cocktails/6798/sex_on_the_breach_2.jpg" };

            products.Add(product0);
            products.Add(product1);
            products.Add(product2);
            products.Add(product3);
            products.Add(product4);
            products.Add(product5);
            products.Add(product6);
            products.Add(product7);
            products.Add(product8);
            products.Add(product9);
            products.Add(product10);
            products.Add(product11);

            return products;
        }

        private static void SeedDefaultAdminRolesAndTwoUsers(IServiceProvider serviceProvider)
        {
            var db = serviceProvider.GetService<RestaurantAppContext>();

            if (!db.Roles.AnyAsync().Result)
            {
                var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

                Task.Run(async () =>
                {
                    var adminRole = AdminRole;
                    var waiterRole = WaiterRole;
                    var barmanRole = BarmanRole;
                    var chefRole = ChefRole;

                    await roleManager.CreateAsync(new IdentityRole
                    {
                        Name = adminRole
                    });

                    await roleManager.CreateAsync(new IdentityRole
                    {
                        Name = waiterRole
                    });

                    await roleManager.CreateAsync(new IdentityRole
                    {
                        Name = barmanRole
                    });

                   await roleManager.CreateAsync(new IdentityRole
                   {
                       Name = chefRole
                   });

                }).Wait();
            }
            if (!db.Users.AnyAsync().Result)
            {
                var userManager = serviceProvider.GetService<UserManager<RestaurantUser>>();

                Task.Run(async () =>
                {
                    var adminPasswrod = "admin123";
                    var admin = new RestaurantUser()
                    {
                        UserName = "Admin",
                        Email = "admin@admin.com",
                        FirstName = "Admin",
                        LastName = "Adminov"
                    };

                    var waiterPassword = "waiter123";
                    var waiter = new RestaurantUser()
                    {
                        UserName = "Waiter1",
                        Email = "nqkuv.tup@student.com",
                        FirstName = "Ivan",
                        LastName = "Pticata"
                    };
                    var waiter2 = new RestaurantUser()
                    {
                        UserName = "Waiter2",
                        Email = "drug.tup@sudent.com",
                        FirstName = "Petko",
                        LastName = "Bagera"
                    };

                    var barmanPassword = "barman123";
                    var barman = new RestaurantUser()
                    {
                        UserName = "Barman",
                        Email = "barman.stranen@barman.com",
                        FirstName = "Pencho",
                        LastName = "Slaveykov"
                    };

                    var chefPassword = "cheff123";
                    var chef = new RestaurantUser()
                    {
                        UserName = "Chef",
                        Email = "chef.manchev@chef.com",
                        FirstName = "Chef",
                        LastName = "Manchev"
                    };

                    await userManager.CreateAsync(admin, adminPasswrod);
                    await userManager.AddToRoleAsync(admin, AdminRole);

                    await userManager.CreateAsync(waiter, waiterPassword);
                    await userManager.AddToRoleAsync(waiter, WaiterRole);

                    await userManager.CreateAsync(waiter2, waiterPassword);
                    await userManager.AddToRoleAsync(waiter2, WaiterRole);

                    await userManager.CreateAsync(barman, barmanPassword);
                    await userManager.AddToRoleAsync(barman, BarmanRole);

                   await userManager.CreateAsync(chef, chefPassword);
                   await userManager.AddToRoleAsync(chef, ChefRole);

                }).Wait();

                db.SaveChanges();
            }

            Console.WriteLine("Admin and default roles successfully seeded into database!");
        }

        private static void ConfigureServices(ServiceCollection services)
        {
            var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .Build();

            services.AddDbContext<RestaurantAppContext>(options =>
                options.UseSqlServer("Server=.;Database=RestaurantApp;Trusted_Connection=True;MultipleActiveResultSets=true"));

            services.AddIdentity<RestaurantUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
            })
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<RestaurantAppContext>();

            services.AddScoped(typeof(IRepository<>), typeof(DbRepository<>));
        }

        private static List<Category> GetCategories()
        {
            var categories = new List<Category>();

            var bar = new Category()
            {
                Name = Restaurant.Data.Models.Enums.Categories.Bar
            };
            var kitchen = new Category()
            {
                Name = Restaurant.Data.Models.Enums.Categories.Kitchen
            };
            var dessert = new Category()
            {
                Name = Restaurant.Data.Models.Enums.Categories.Dessert
            };
            var other = new Category()
            {
                Name = Restaurant.Data.Models.Enums.Categories.Other
            };
            var barSlow = new Category()
            {
                Name = Restaurant.Data.Models.Enums.Categories.BarSlow
            };

            categories.Add(bar);
            categories.Add(kitchen);
            categories.Add(dessert);
            categories.Add(other);
            categories.Add(barSlow);

            return categories;
        }

        private static List<OrderStatus> GetOrderStatuses()
        {
            var statuses = new List<OrderStatus>();

            var preparing = new OrderStatus()
            {
                Status = Restaurant.Data.Models.Enums.OrderStatus.Preapring
            };
            var ready = new OrderStatus()
            {
                Status = Restaurant.Data.Models.Enums.OrderStatus.Ready
            };

            statuses.Add(preparing);
            statuses.Add(ready);

            return statuses;
        }
    }
}
