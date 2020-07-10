namespace Di2.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Di2.Data.Models;
    using Di2.Services.Data;
    using Di2.Services.Mapping;
    using Di2.Web.ViewModels.Materials.ViewModels;
    using Di2.Web.ViewModels.PriceLists.InputModels;
    using Di2.Web.ViewModels.PriceLists.ViewModels;
    using Di2.Web.ViewModels.Suppliers.ViewModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class PriceListsController : AdministrationController
    {
        private readonly IPriceListsService priceListsService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMaterialsService materialsService;
        private readonly ISuppliersService suppliersService;

        public PriceListsController(IPriceListsService priceListsService, UserManager<ApplicationUser> userManager, IMaterialsService materialsService, ISuppliersService suppliersService)
        {
            this.priceListsService = priceListsService;
            this.userManager = userManager;
            this.materialsService = materialsService;
            this.suppliersService = suppliersService;
        }

        [Authorize]
        public IActionResult Create()
        {
            var materials = this.materialsService.GetAllMaterials<MaterialViewModel>();
            var suppliers = this.suppliersService.GetAllSuppliers<SupplierViewModel>();
            var viewModel = new CreatePriceListInputModel
            {
                Materials = materials,
                Suppliers = suppliers,
            };

            return this.View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreatePriceListInputModel input)
        {
            // var priceList = AutoMapperConfig.MapperInstance.Map<PriceList>(input);
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var user = await this.userManager.GetUserAsync(this.User);
            await this.priceListsService.CreateAsync(input, user.Id);
            return this.RedirectToAction(nameof(this.All));
        }

        public IActionResult All()
        {
            var allPriceLists = this.priceListsService.GetAllPriceLists<PriceListViewModel>();

            return this.View(allPriceLists);
        }

        public IActionResult ById()
        {
            var priceListViewModel = this.priceListsService.GetById<PriceListViewModel>();
            if (priceListViewModel == null)
            {
                return this.NotFound();
            }

            return this.View(priceListViewModel);
        }

        [HttpGet("{id}/{supplierid}/{mqo}/{unitprice}")]
        public async Task<IActionResult> Delete(int id, int supplierid, double mqo, decimal unitprice)
        {
            
            await this.priceListsService.DeleteAsync(id,supplierid,mqo,unitprice);
            return this.RedirectToAction(nameof(this.All));
        }
    }
}
