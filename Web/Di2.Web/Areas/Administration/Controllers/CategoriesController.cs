namespace Di2.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Di2.Data.Models;
    using Di2.Services.Data;
    using Di2.Web.ViewModels.Categories.InputModels;
    using Di2.Web.ViewModels.Categories.ViewModels;
    using Di2.Web.ViewModels.Deliveries;
    using Di2.Web.ViewModels.Materials.ViewModels;
    using Di2.Web.ViewModels.OrderSuppliers;
    using Di2.Web.ViewModels.SubCategories.ViewModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    
    public class CategoriesController : AdministrationController
    {
        private readonly ICategoriesService categoriesService;
        private readonly Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager;
        private readonly ISubCategoriesService subCategoriesService;
        private readonly IOrderSupplierService orderSupplierService;
        private readonly IDeliveriesService deliveriesService;
        private readonly IMaterialsService materialsService;

        public CategoriesController(
            ICategoriesService categoriesService,
            Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager,
            ISubCategoriesService subCategoriesService,
            IOrderSupplierService orderSupplierService,
            IDeliveriesService deliveriesService,
            IMaterialsService materialsService)
        {
            this.categoriesService = categoriesService;
            this.userManager = userManager;
            this.subCategoriesService = subCategoriesService;
            this.orderSupplierService = orderSupplierService;
            this.deliveriesService = deliveriesService;
            this.materialsService = materialsService;
        }

        [Authorize]
        public IActionResult Add()
        {
            return this.View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Add(CreateCategoryInputModel input, string userId)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            // CORRECT WAY TO FIND A USER - var user = await this.userManager.GetUserAsync(this.User);
            // userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value; - ALSO OK!!!
            var user = await this.userManager.GetUserAsync(this.User);

            await this.categoriesService.AddAsync(input, user.Id);

            return this.RedirectToAction(nameof(this.All));
        }

        public IActionResult All()
        {
            var viewModel = this.categoriesService
                .GetAllCategories<CategoryViewModel>();

            return this.View(viewModel);
        }

        public IActionResult ByName(string name)
        {
            var catId = this.categoriesService.GetByName<CategoryViewModel>(name).Id;
            //var viewModel = this.categoriesService.GetByName<DeliveryViewModel>(name);
            var viewModel = this.categoriesService.GetByName<DeliveryViewModel>(name);
            viewModel.Deliveries = this.deliveriesService.GetAllProducts<CategoryProductsViewModel>(catId);
            if (viewModel == null)
            {
                return this.NotFound();
            }

             //viewModel.Deliveries = this.deliveriesService.GetAllProducts<CategoryProductsViewModel>(viewModel.Id);

            return this.View(viewModel);
        }
    }
}
