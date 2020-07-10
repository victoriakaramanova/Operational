using Di2.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Di2.Data.Seeding
{
    public class SuppliersSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Suppliers.Any())
            {
                return;
            }

            var suppliers = new[]
            {
                new Supplier
                {
                    Name = "Веберетикетен",
                    Address = "с. Мерданя, ул. Първа 1",
                    Email = "mariamileva7824@gmail.com",
                    Phone = "08986176821",
                },
                new Supplier
                {
                    Name = "SYS-45",
                    Address = "гр. Варна, ул. Пристанищна 3 В",
                    Email = "e_karamanova@abv.bg",
                    Phone = "0898400235",
                },
            };

            await dbContext.AddRangeAsync(suppliers);
        }
    }
}
