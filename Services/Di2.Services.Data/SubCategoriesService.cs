namespace Di2.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Di2.Data.Common.Repositories;
    using Di2.Data.Models;
    using Di2.Services.Mapping;
    using Di2.Web.ViewModels.Categories.InputModels;
    using Di2.Web.ViewModels.SubCategories;
    using Di2.Web.ViewModels.SubCategories.InputModels;
    using Di2.Web.ViewModels.SubCategories.ViewModels;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class SubCategoriesService : ISubCategoriesService
    {
        private readonly IDeletableEntityRepository<SubCategory> subCategoriesRepository;
        private readonly IDeletableEntityRepository<Category> categoriesRepository;

        public SubCategoriesService(
            IDeletableEntityRepository<SubCategory> subCategoriesRepository,
            IDeletableEntityRepository<Category> categoriesRepository)
        {
            this.subCategoriesRepository = subCategoriesRepository;
            this.categoriesRepository = categoriesRepository;
        }

        public IEnumerable<T> GetCategories<T>()
        {
            IQueryable<Category> query =
                this.categoriesRepository.All();
            return query.To<T>();
        }

        public async Task AddAsync(CreateSubCategoryInputModel input, string userId)
        {
            var category = this.categoriesRepository.All()
                .FirstOrDefault(x => x.Name == input.Category) as Category;

            var subCategory = new SubCategory
            {
                Name = input.Name,
                CategoryId = category.Id,
                Description = input.Description,
                UserId = userId,
            };

            await this.subCategoriesRepository.AddAsync(subCategory);
            await this.subCategoriesRepository.SaveChangesAsync();
        }

        public IEnumerable<T> GetAllSubCategories<T>(int? categoryId = null)
        {
            IQueryable<SubCategory> query = this.subCategoriesRepository.All();
            if (categoryId.HasValue)
            {
                query = query.Where(x => x.CategoryId == categoryId);
            }

            return query.To<T>().ToList();
        }

        public T GetById<T>(int id)
        {
            var subCategory = this.subCategoriesRepository.All().Where(x => x.Id == id)
                .To<T>().FirstOrDefault();
            return subCategory;
        }

        public int GetCount()
        {
            return this.subCategoriesRepository.All().Count();
        }

        public async Task<int> DeleteAsync(int id)
        {
            var subCategory = this.subCategoriesRepository.All()
                    .FirstOrDefault(x => x.Id == id);
            this.subCategoriesRepository.Delete(subCategory);
            await this.subCategoriesRepository.SaveChangesAsync();
            return subCategory.Id;
        }
    }
}
