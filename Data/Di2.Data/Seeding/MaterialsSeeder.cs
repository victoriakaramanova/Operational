using Di2.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Di2.Data.Seeding
{
    public class MaterialsSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Materials.Any())
            {
                return;
            }

            var categories = dbContext.Categories.ToList();
            var materials = new[]
            {
                new Material
                {
                    Name = "100x50 полугланц, силнолепнещ",
                    Description = "f76 x 1 x 3 000",
                    ExtraInfo = "Най-разпространеният етикет при средноголемите каротнени опаковки",
                    CategoryId = 1,
                    SubCategoryId = 1,
                    Image = "https://res.cloudinary.com/stopify-cloud-v/image/upload/v1587150550/Di2pix/agefllipvolzxhiekhhf.gif",
                },
                new Material
                {
                    Name = "80x300 m OUT AWX FH черна",
                    Description = "Wax Premium FH",
                    CategoryId = 2,
                    SubCategoryId = 5,
                    Image = "https://res.cloudinary.com/stopify-cloud-v/image/upload/v1587150779/Di2pix/lqfujwkadyqgdbm52z3b.jpg",
                },
                new Material
                {
                    Name = "18x26 полугланц, силнолепнещ",
                    Description = "f76 x 5 x 27 000",
                    CategoryId = 1,
                    SubCategoryId = 1,
                    Image = "https://res.cloudinary.com/stopify-cloud-v/image/upload/v1587150612/Di2pix/valqcqk4xjbngsygvk6s.gif",
                },
                new Material
                {
                    Name = "56x25 TD eco репер ф12x1x600",
                    Description = "f12 x 1 x 600",
                    CategoryId = 1,
                    SubCategoryId = 4,
                    Image = "https://res.cloudinary.com/stopify-cloud-v/image/upload/v1587150687/Di2pix/xjts6tasbehmrji0wqam.gif",
                },
                new Material
                {
                    Name = "65x360 m  OUT AWX FH черна",
                    Description = "Wax Premium FH",
                    CategoryId = 2,
                    SubCategoryId = 6,
                    Image = "https://res.cloudinary.com/stopify-cloud-v/image/upload/v1587150817/Di2pix/duutexqzc3uestlri6fk.jpg",
                },
                new Material
                {
                    Name = "74x47 оранж-бял",
                    Description = "f76 x 1 x 3 000",
                    CategoryId = 1,
                    SubCategoryId = 2,
                    Image = "https://res.cloudinary.com/stopify-cloud-v/image/upload/v1587150727/Di2pix/x94qhp3ygx1qymuzjsgg.gif",
                },
            };

            await dbContext.AddRangeAsync(materials);
        }
    }
}
