namespace Di2.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using AutoMapper.QueryableExtensions;
    using Di2.Data.Common.Repositories;
    using Di2.Data.Models;
    using Di2.Services.Mapping;
    using Di2.Web.ViewModels.Categories.InputModels;
    using Di2.Web.ViewModels.Deliveries;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;

    public class CategoriesService : ICategoriesService
    {
        private readonly IDeletableEntityRepository<Category> categoriesRepository;

        public CategoriesService(
            IDeletableEntityRepository<Category> categoriesRepository)
        {
            this.categoriesRepository = categoriesRepository;
        }

        public async Task AddAsync(CreateCategoryInputModel input, string userId)
        {
            var category = new Category
            {
                Name = input.Name,
                UserId = userId,
            };

            await this.categoriesRepository.AddAsync(category);
            await this.categoriesRepository.SaveChangesAsync();
        }

        public IEnumerable<T> GetAllCategories<T>()
        {
            IQueryable<Category> query =
                this.categoriesRepository.All().OrderBy(x => x.Name);
            var categories = this.categoriesRepository.All();
            return query.To<T>().ToList();
        }

        public T GetByName<T>(string name)
        {
            var category = this.categoriesRepository.All()
                .Where(x => x.NameEng == name)
                .To<T>().FirstOrDefault();
            return category;
        }

        public T GetByNameBg<T>(string name)
        {
            var category = this.categoriesRepository.All()
                .Where(x => x.Name == name)
                .To<T>().FirstOrDefault();
            return category;
        }

        public T GetById<T>(int catId)
        {
            var category = this.categoriesRepository.All()
                .Where(x => x.Id == catId)
                //.Select(x => x.Name)
                .To<T>().FirstOrDefault();
            return category;
        }
    }
}
