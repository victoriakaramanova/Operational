namespace Di2.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Di2.Data.Models;
    using Di2.Services.Data;
    using Di2.Services.Mapping;
    using Di2.Web.ViewModels.Suppliers.InputModels;
    using Di2.Web.ViewModels.Suppliers.ViewModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class SuppliersController : BaseController
    {
        private readonly ISuppliersService suppliersService;
        private readonly UserManager<ApplicationUser> userManager;

        public SuppliersController(ISuppliersService suppliersService, UserManager<ApplicationUser> userManager)
        {
            this.suppliersService = suppliersService;
            this.userManager = userManager;
        }

        [Authorize]
        public IActionResult Add()
        {
            return this.View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Add(CreateSupplierInputModel input)
        {
            var supplier = AutoMapperConfig.MapperInstance.Map<Supplier>(input);
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var user = await this.userManager.GetUserAsync(this.User);
            int supplierId = await this.suppliersService.AddAsync(input.Name, input.Address, input.Email, input.Phone, user.Id);
            return this.RedirectToAction(nameof(this.ById), new { id = supplierId });
        }

        public IActionResult All()
        {
            var allSuppliers = this.suppliersService.GetAllSuppliers<SupplierViewModel>();

            return this.View(allSuppliers);
        }


        public IActionResult ById(int id)
        {
            var supplierViewModel = this.suppliersService.GetById(id);
            if (supplierViewModel == null)
            {
                return this.NotFound();
            }

            return this.View(supplierViewModel);
        }
    }
}
