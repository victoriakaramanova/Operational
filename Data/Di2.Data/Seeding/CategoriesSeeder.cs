namespace Di2.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Di2.Common;
    using Di2.Data.Models;
    using Microsoft.Extensions.DependencyInjection;

    public class CategoriesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
       
            if (dbContext.Categories.Any())
            {
                return;
            }

            var categories = new List<(string Name, string NameEng)>
            {
                ("Cамозалепващи етикети", "Labels"),
                ("Термотранферни ленти", "Bands"),
            };
            foreach (var category in categories)
            {
                await dbContext.Categories.AddAsync(new Category
                {
                    Name = category.Name,
                    NameEng = category.NameEng,
                });
            }
        }
    }
}
