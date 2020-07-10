namespace Di2.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Di2.Data.Models;
    using Di2.Services;
    using Di2.Services.Data;
    using Di2.Services.Mapping;
    using Di2.Web.ViewModels.Categories.ViewModels;
    using Di2.Web.ViewModels.Materials.InputModels;
    using Di2.Web.ViewModels.Materials.ViewModels;
    using Di2.Web.ViewModels.SubCategories.ViewModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class MaterialsController : BaseController
    {
        private readonly IMaterialsService materialsService;
        private readonly ISubCategoriesService subCategoriesService;
        private readonly ICloudinaryService cloudinaryService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ICategoriesService categoriesService;

        public MaterialsController(IMaterialsService materialsService, ISubCategoriesService subCategoriesService, ICloudinaryService cloudinaryService, UserManager<ApplicationUser> userManager, ICategoriesService categoriesService)
        {
            this.materialsService = materialsService;
            this.subCategoriesService = subCategoriesService;
            this.cloudinaryService = cloudinaryService;
            this.userManager = userManager;
            this.categoriesService = categoriesService;
        }

        [Authorize]
        public IActionResult Add()
        {
            var categories = this.categoriesService.GetAllCategories<CategoryViewModel>();
            var subCategories = this.subCategoriesService.GetAllSubCategories<SubCategoryViewModel>();
            var viewModel = new CreateMaterialInputModel
            {
                Categories = categories,
                SubCategories = subCategories,
            };
            return this.View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Add(CreateMaterialInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var material = AutoMapperConfig.MapperInstance.Map<Material>(input);
            string imageUrl = await this.cloudinaryService.UploadPictureAsync(input.Image, input.Name);
            var user = await this.userManager.GetUserAsync(this.User);

            // var material = input.To<CreateMaterialInputModel, Material>();
            //var material = Mapper.Map<Material>(input);
            //material.Image = imageUrl;

            //int materialId = await this.materialsService.AddAsync(input.Name, input.Description, input.ExtraInfo, input.SubCategoryName, imageUrl, user.Id);
            int materialId = await this.materialsService.AddAsync(input, imageUrl, user.Id);

            return this.RedirectToAction(nameof(this.ById), new { id = materialId });
        }

        // Download file sample
       //  public IActionResult Download(string filename)
        //{
            // System.IO.File.ReadAllBytesAsync(); - an alternative
            // return this.File();
            // FIND SANITIZE FILENAME ARTICLES to ignore xss etc!!!
          //  return this.PhysicalFile(@$"D:\{filename}", "application/pdf");
        // }

        public IActionResult All()
        {
            var allMaterials = this.materialsService.GetAllMaterials<MaterialsViewModel>();

            return this.View(allMaterials);
        }

        public IActionResult ById(int id)
        {
            var materialViewModel = this.materialsService
                .GetById(id);
            if (materialViewModel == null)
            {
                return this.NotFound();
            }

            return this.View(materialViewModel);
        }
    }
}
