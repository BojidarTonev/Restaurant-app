﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Restaurant.Data;
using Restaurant.Data.Contracts;
using Restaurant.Data.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using TattooShop.Data;

namespace Restaurant.Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine($"{typeof(Program).Namespace} ({string.Join(" ", args)}) starts working...");
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider(true);

            using (var serviceScope = serviceProvider.CreateScope())
            {
                serviceProvider = serviceScope.ServiceProvider;
                SandboxCode(serviceProvider);
            }
        }

        private static void SandboxCode(IServiceProvider serviceProvider)
        {
            //Add code here..
            SeedDatabase(serviceProvider);
        }

        private static void SeedDatabase(IServiceProvider serviceProvider)
        {
            SeedDefaultAdminRolesAndTwoUsers(serviceProvider);
            SeedTables(serviceProvider);
            SeedProducts(serviceProvider);
        }

        private async static void SeedTables(IServiceProvider serviceProvider)
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
                    if(i % 2 == 0)
                    {
                        table.UserId = petkoEmployee.Id;
                    } else
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
                var products = GetSampleProductsData();

                db.Products.AddRange(products);
                db.SaveChanges();

                Console.WriteLine("10 different products successfully inserted into the database!");
            }
        }

        private static List<Product> GetSampleProductsData()
        {
            var products = new List<Product>();

            //Bar products
            var product0 = new Product() { Name = "Coca-Cola", Category = Data.Models.Enums.Categories.Bar, Description = "gazirarno i vredno", Price = 1.20m, ImageUrl = "http://www.cantina-ola.com/media/7/21.jpg" };
            var product1 = new Product() { Name = "Pepsi", Category = Data.Models.Enums.Categories.Bar, Description = "pak gazirano i vredno", Price = 1.30m, ImageUrl = "http://tuidagroup.com/2994-large_default/PEPSI-05L12.jpg" };
            var product2 = new Product() { Name = "Water", Category = Data.Models.Enums.Categories.Bar, Description = "polezno i vkusno", Price = 1.00m, ImageUrl = "http://www.homebag.bg/media/7/8017.jpg" };

            //Kithchen products
            var product3 = new Product() { Name = "Musaka", Category = Data.Models.Enums.Categories.Kitchen, Description = "Best meal in the world ever", Price = 5.60m, ImageUrl = "https://i.ytimg.com/vi/S8j6Tc-1APk/maxresdefault.jpg" };
            var product4 = new Product() { Name = "Gyuveche", Category = Data.Models.Enums.Categories.Kitchen, Description = "Bulgarian traditional meal, kind of", Price = 5.20m, ImageUrl = "https://gotvach.bg/files/lib/600x350/ovcharsko-guveche-sirene.JPG" };
            var product5 = new Product() { Name = "Banitsa", Category = Data.Models.Enums.Categories.Kitchen, Description = "Another Bulgarian traditional meal. Delicious!", Price = 5.50m, ImageUrl = "https://gotvach.bg/files/lib/600x350/spoluchliva-banica-sirene.JPG" };
            var product6 = new Product() { Name = "Chicken with rice", Category = Data.Models.Enums.Categories.Kitchen, Description = "Cicken with rice only for post-traiing meals.", Price = 6.00m, ImageUrl = "https://recepti.ezine.bg/files/lib/500x350/pile-oriz-zvezdev.jpg" };

            //Dessert products
            var product7 = new Product() { Name = "Ice-cream", Category = Data.Models.Enums.Categories.Dessert, Description = "ice-cream for sore throat", Price = 5.10m, ImageUrl = "https://recepti.gotvach.bg/files/lib/500x350/melba3.jpg" };
            var product8 = new Product() { Name = "Fruit salad", Category = Data.Models.Enums.Categories.Dessert, Description = "best in vitamins and stuff", Price = 8.40m, ImageUrl = "http://assets.kulinaria.bg/attachments/pictures-images/0000/5689/MAIN-2015-07-31-11-14-39-0300-0-1-2.jpg?1438330483" };
            var product9 = new Product() { Name = "Pancakes", Category = Data.Models.Enums.Categories.Dessert, Description = "tastiest pankaces ever lol", Price = 2.50m, ImageUrl = "https://ezine.bg/files/lib/600x350/palachinki18.jpg" };

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
                    var adminRole = GlobalConstants.AdminRole;
                    var staffRole = GlobalConstants.StaffRole;

                    await roleManager.CreateAsync(new IdentityRole
                    {
                        Name = adminRole
                    });

                    await roleManager.CreateAsync(new IdentityRole
                    {
                        Name = staffRole
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

                    var staffPassword = "staff123";
                    var staff = new RestaurantUser()
                    {
                        UserName = "Nqkuv",
                        Email = "nqkuv.tup@student.com",
                        FirstName = "Ivan",
                        LastName = "Pticata"
                    };
                    var staff2 = new RestaurantUser()
                    {
                        UserName = "Drug",
                        Email = "drug.tup@sudent.com",
                        FirstName = "Petko",
                        LastName = "Bagera"
                    };

                    await userManager.CreateAsync(admin, adminPasswrod);
                    await userManager.AddToRoleAsync(admin, GlobalConstants.AdminRole);

                    await userManager.CreateAsync(staff, staffPassword);
                    await userManager.AddToRoleAsync(staff, GlobalConstants.StaffRole);

                    await userManager.CreateAsync(staff2, staffPassword);
                    await userManager.AddToRoleAsync(staff2, GlobalConstants.StaffRole);

                }).Wait();

                db.SaveChanges();
            }

            Console.WriteLine("Admin and default roles successfully seeded into database!");
        }

        private static void ConfigureServices(ServiceCollection services)
        {
            var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
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
    }
}