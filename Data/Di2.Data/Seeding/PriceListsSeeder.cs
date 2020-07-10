using Di2.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Di2.Data.Seeding
{
    public class PriceListsSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.PriceLists.Any())
            {
                return;
            }

            var priceLists = new[]
            {
                new PriceList
                {
                    MaterialId = 1,
                    SupplierId = 1,
                    MinimumQuantityPerOrder = 36000,
                    UnitPrice = 5.42m,
                },

                new PriceList
                {
                    MaterialId = 1,
                    SupplierId = 2,
                    MinimumQuantityPerOrder = 31000,
                    UnitPrice = 5.89m,
                },

                new PriceList
                {
                    MaterialId = 2,
                    SupplierId = 1,
                    MinimumQuantityPerOrder = 10,
                    UnitPrice = 6.86m,
                },

                new PriceList
                {
                    MaterialId = 2,
                    SupplierId = 2,
                    MinimumQuantityPerOrder = 15,
                    UnitPrice = 5.98m,
                },

                new PriceList
                {
                    MaterialId = 3,
                    SupplierId = 1,
                    MinimumQuantityPerOrder = 81000,
                    UnitPrice = 0.69m,
                },

                new PriceList
                {
                    MaterialId = 3,
                    SupplierId = 2,
                    MinimumQuantityPerOrder = 70000,
                    UnitPrice = 1.12m,
                },

                new PriceList
                {
                    MaterialId = 4,
                    SupplierId = 1,
                    MinimumQuantityPerOrder = 84000,
                    UnitPrice = 1.82m,
                },

                new PriceList
                {
                    MaterialId = 4,
                    SupplierId = 2,
                    MinimumQuantityPerOrder = 84000,
                    UnitPrice = 1.65m,
                },

                new PriceList
                {
                    MaterialId = 5,
                    SupplierId = 1,
                    MinimumQuantityPerOrder = 10,
                    UnitPrice = 6.69m,
                },

                new PriceList
                {
                    MaterialId = 5,
                    SupplierId = 2,
                    MinimumQuantityPerOrder = 20,
                    UnitPrice = 5.73m,
                },

                new PriceList
                {
                    MaterialId = 6,
                    SupplierId = 1,
                    MinimumQuantityPerOrder = 36000,
                    UnitPrice = 4.57m,
                },

                new PriceList
                {
                    MaterialId = 6,
                    SupplierId = 2,
                    MinimumQuantityPerOrder = 33000,
                    UnitPrice = 4.80m,
                },
            };

            await dbContext.AddRangeAsync(priceLists);
        }
    }
}
