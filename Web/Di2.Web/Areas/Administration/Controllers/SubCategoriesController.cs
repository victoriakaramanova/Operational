namespace Di2.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Di2.Data.Models;
    using Di2.Services.Data;
    using Di2.Web.ViewModels.Categories.ViewModels;
    using Di2.Web.ViewModels.SubCategories.InputModels;
    using Di2.Web.ViewModels.SubCategories.ViewModels;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class SubCategoriesController : AdministrationController
    {
        private readonly ISubCategoriesService subCategoriesService;
        private readonly UserManager<ApplicationUser> userManager;

        public SubCategoriesController(
            ISubCategoriesService subCategoriesService,
            Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager)
        {
            this.subCategoriesService = subCategoriesService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Add()
        {
            var viewModel = new CreateSubCategoryInputModel
            {
                Categories = this.subCategoriesService.GetCategories<SelectListItem>(),
            };
            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(CreateSubCategoryInputModel input, string userId)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var user = await this.userManager.GetUserAsync(this.User);

            await this.subCategoriesService.AddAsync(input, user.Id);

            return this.RedirectToAction(nameof(this.All));
        }

        public IActionResult All()
        {
            var allSubCategories = this
                .subCategoriesService
                .GetAllSubCategories<SubCategoriesViewModel>();

            return this.View(allSubCategories);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            await this.subCategoriesService.DeleteAsync(id);
            return this.RedirectToAction(nameof(this.All));
        }
    }
}
