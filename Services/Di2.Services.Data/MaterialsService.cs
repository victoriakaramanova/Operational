namespace Di2.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Di2.Data.Common.Repositories;
    using Di2.Data.Models;
    using Di2.Services.Mapping;
    using Di2.Web.ViewModels.Materials.InputModels;
    using Di2.Web.ViewModels.Materials.ViewModels;
    using Microsoft.EntityFrameworkCore;

    public class MaterialsService : IMaterialsService
    {
        private readonly IDeletableEntityRepository<Material> materialRepository;
        private readonly IDeletableEntityRepository<SubCategory> subCategoryRepository;
        private readonly IDeletableEntityRepository<Category> categoryRepository;
        private readonly IDeletableEntityRepository<SubCategoryMaterial> subCategoryMaterialRepository;

        public MaterialsService(
            IDeletableEntityRepository<Material> materialRepository, 
            IDeletableEntityRepository<SubCategory> subCategoryRepository,
            IDeletableEntityRepository<Category> categoryRepository,
            IDeletableEntityRepository<SubCategoryMaterial> subCategoryMaterialRepository)
        {
            this.materialRepository = materialRepository;
            this.subCategoryRepository = subCategoryRepository;
            this.categoryRepository = categoryRepository;
            this.subCategoryMaterialRepository = subCategoryMaterialRepository;
        }

        public async Task<int> AddAsync(CreateMaterialInputModel input, string imageUrl, string userId)
        {
            // using (var fileStream = new FileStream(@"D:\img.jpg", FileMode.Create))
            // {
            /*var expectedFileExt = new[] { ".pdf", ".doc", ".docx", ".jpg", ".png" };
            if (expectedFileExt.Any(x => input.Image.First().FileName.EndsWith(x)))
            {
            }*/
            // await input.Image.CopyToAsync(fileStream);
            // }
            var subCategoryId = this.subCategoryRepository
                .All()
                .Where(y => y.Id == input.SubCategoryId)
                .Select(x => x.Id).FirstOrDefault();

            var categoryId = this.subCategoryRepository.All()
                .Where(x => x.Id == subCategoryId)
                .Select(x => x.CategoryId).FirstOrDefault();
            //int.Parse(this.categoryRepository
            //.All()
            //.Select(y => y.SubCategories
            //.Select(z => z.Id == subCategoryId)
            //.FirstOrDefault()).FirstOrDefault().ToString());
            var category = this.categoryRepository.All()
                .FirstOrDefault(x => x.Id == categoryId);
            var material = new Material
            {
                Name = input.Name,
                Description = input.Description,
                ExtraInfo = input.ExtraInfo,
                SubCategoryId = input.SubCategoryId,
                CategoryId = categoryId,
                Category = category,
                Image = imageUrl,
                UserId = userId,
            };

            await this.materialRepository.AddAsync(material);
            await this.materialRepository.SaveChangesAsync();
            return material.Id;
        }

        public IEnumerable<T> GetAllMaterials<T>()
        {
            IQueryable<Material> query = this.materialRepository
            .All();
            return query.To<T>().ToList();
        }

        public MaterialsViewModel GetById(int id)
        {
            var m = this.materialRepository.All().Where(x => x.Id == id).FirstOrDefault();
            var material = this.materialRepository.All()
                .Where(x => x.Id == m.Id).To<MaterialsViewModel>()
                .FirstOrDefault();
            material.Name = m.Name;
            material.Category = this.categoryRepository.AllWithDeleted()
                .Where(x => x.Id == m.Category.Id).FirstOrDefault();
            
            material.SubCategoryName = this.subCategoryRepository.All()
                .Where(x => x.Id == m.SubCategoryId).FirstOrDefault().Name;
            return material;
        }

        public MaterialsViewModel GetByName(string name)
        {
            var material = this.materialRepository.All()
                .Where(x => x.Name == name).To<MaterialsViewModel>()
                .FirstOrDefault();
            return material;
        }

        public IEnumerable<T> GetByCategoryName<T>(string categoryName)
        {
            var categoryId = this.categoryRepository.All()
                
                .Where(x => x.Name == categoryName).Select(x => x.Id).FirstOrDefault();
            IQueryable<Material> query = this.materialRepository
            .All();
            query = query.Where(x => x.CategoryId == categoryId);
            
            return query.To<T>().ToList();
        }

        public int GetCount()
        {
            return this.materialRepository.All().Count();
        }

        public async Task<int> DeleteAsync(int id)
        {
            var material = this.materialRepository.All()
                    .FirstOrDefault(x => x.Id == id);
            //var materialIdToDelete = id;
            //var subcategoryIdToDelete = material.SubCategoryId;
            material.SubCategoryId = null;
            this.materialRepository.Update(material);
            await this.materialRepository.SaveChangesAsync();
            this.materialRepository.Delete(material);
            await this.materialRepository.SaveChangesAsync();
            return material.Id;
        }
    }
}
