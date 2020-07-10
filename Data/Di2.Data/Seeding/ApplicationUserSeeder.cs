using Di2.Common;
using Di2.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Di2.Data.Seeding
{
    public class ApplicationUserSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var userId = await SeedUserAsync(userManager);

            //if (!dbContext.Categories.Any())
            //{
            //    await CreateCategories(dbContext, userId);
            //}

            if (!dbContext.SubCategories.Any())
            {
                await CreateSubCategories(dbContext, userId);
            }

            if (!dbContext.Materials.Any())
            {
                await CreateMaterials(dbContext, userId);
            }

            if (!dbContext.Suppliers.Any())
            {
                await CreateSuppliers(dbContext, userId);
            }
        }

        private static async Task<string> SeedUserAsync(UserManager<ApplicationUser> userManager)
        {
            if (userManager.FindByNameAsync("Admin").Result == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "admin@admin.com",
                    Email = "admin@admin.com",
                };

                var result = userManager.CreateAsync(user, "AQAAAAEAACcQAAAAEMJ58ERdGCIjpO4PIsbi92Na3Gj8PhUGkTiKEkZKNEhhGHYPD2oc0m2gNXGr9GgBsg==").Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, GlobalConstants.AdministratorRoleName).Wait();
                }
                return user.Id;
            }
            return null;
        }

        private static async Task CreateCategories(ApplicationDbContext dbContext)
        {
            
            var categories = new List<(string Name, string NameEng)>
            {
                ("Cамозалепващи етикети", "Labels"),
                ("Термотранферни ленти", "Bands"),
            };
            //foreach (var category in categories)
            //{
            //    await dbContext.Categories.AddAsync(new Category
            //    {
            //        Name = category.Name,
            //        NameEng = category.NameEng,
            //        UserId = userId,
            //    });
            //}
        }

        private static async Task CreateSubCategories(ApplicationDbContext dbContext, string userId)
        {
            var subCategories = new List<string>
            {
                "Термотрансферни полугланц с постоянно лепило",
                "Фолийни самозалепващи етикети",
                "Текстилни етикети",
                "Термодиректни ECO с постоянно лепило",
                "Восъчни ленти",
                "Гумено-восъчни ленти",
                "Гумени ленти",
            };

            for (int i = 0; i < 4; i++)
            {
                await dbContext.SubCategories.AddAsync(new SubCategory
                {
                    Name = subCategories[i],
                    CategoryId = 1,
                    UserId = userId,
                });
            }

            for (int i = 5; i < subCategories.Count; i++)
            {
                await dbContext.SubCategories.AddAsync(new SubCategory
                {
                    Name = subCategories[i],
                    CategoryId = 2,
                    UserId = userId,
                });
            }
        }

        private static async Task CreateMaterials(ApplicationDbContext dbContext, string userId)
        {
            var materials = new[]
           {
                new Material
                {
                    Name = "100x50 полугланц, силнолепнещ",
                    Description = "f76 x 1 x 3 000",
                    //CategoryId=1,
                    SubCategoryId = 1,
                    UserId = userId,
                    Image = "https://res.cloudinary.com/stopify-cloud-v/image/upload/v1587150550/Di2pix/agefllipvolzxhiekhhf.gif",
                },
                new Material
                {
                    Name = "80x300 m OUT AWX FH черна",
                    Description = "Wax Premium FH",
                    //CategoryId=2,
                    SubCategoryId = 2,
                    UserId = userId,
                    Image = "https://res.cloudinary.com/stopify-cloud-v/image/upload/v1587150779/Di2pix/lqfujwkadyqgdbm52z3b.jpg",
                },
                new Material
                {
                    Name = "18x26 полугланц, силнолепнещ",
                    Description = "f76 x 5 x 27 000",
                    //CategoryId=1,
                    SubCategoryId = 1,
                    UserId = userId,
                    Image = "https://res.cloudinary.com/stopify-cloud-v/image/upload/v1587150612/Di2pix/valqcqk4xjbngsygvk6s.gif",
                },
                new Material
                {
                    Name = "56x25 TD eco репер ф12x1x600",
                    Description = "f12 x 1 x 600",
                    //CategoryId=1,
                    SubCategoryId = 4,
                    UserId = userId,
                    Image = "https://res.cloudinary.com/stopify-cloud-v/image/upload/v1587150687/Di2pix/xjts6tasbehmrji0wqam.gif",
                },
                new Material
                {
                    Name = "65x360 m  OUT AWX FH черна",
                    Description = "Wax Premium FH",
                    //CategoryId=2,
                    SubCategoryId = 7,
                    UserId = userId,
                    Image = "https://res.cloudinary.com/stopify-cloud-v/image/upload/v1587150817/Di2pix/duutexqzc3uestlri6fk.jpg",
                },
                new Material
                {
                    Name = "74x47 оранж-бял",
                    Description = "f76 x 1 x 3 000",
                   // CategoryId=1,
                    SubCategoryId = 3,
                    UserId = userId,
                    Image = "https://res.cloudinary.com/stopify-cloud-v/image/upload/v1587150727/Di2pix/x94qhp3ygx1qymuzjsgg.gif",
                },
            };

            await dbContext.AddRangeAsync(materials);
        }

        private static async Task CreateSuppliers(ApplicationDbContext dbContext, string userId)
        {
            var suppliers = new[]
            {
                new Supplier
                {
                    Name = "Веберетикетен",
                    Address = "с. Мерданя, ул. Първа 1",
                    Email = "mariamileva7824@gmail.com",
                    Phone = "08986176821",
                    UserId = userId,
                },
                new Supplier
                {
                    Name = "SYS-45",
                    Address = "гр. Варна, ул. Пристанищна 3 В",
                    Email = "e_karamanova@abv.bg",
                    Phone = "0898400235",
                    UserId = userId,
                },
            };

            await dbContext.AddRangeAsync(suppliers);
        }
    }
}