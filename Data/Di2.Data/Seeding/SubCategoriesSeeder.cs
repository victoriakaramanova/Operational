namespace Di2.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Di2.Data.Models;

    public class SubCategoriesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.SubCategories.Any())
            {
                return;
            }

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
                });
            }

            for (int i = 5; i < subCategories.Count; i++)
            {
                await dbContext.SubCategories.AddAsync(new SubCategory
                {
                    Name = subCategories[i],
                    CategoryId = 2,
                });
            }
        }
    }
}
