namespace Di2.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Di2.Data.Models;
    using Di2.Data.Models.Enums;
    using Di2.Services.Data;
    using Di2.Web.ViewModels.Materials.ViewModels;
    using Di2.Web.ViewModels.OrderSuppliers;
    using Di2.Web.ViewModels.PriceLists.ViewModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class OrderSuppliersController : AdministrationController
    {
        private readonly IOrderSupplierService orderSupplierService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ISuppliersService suppliersService;
        private readonly IMaterialsService materialsService;
        private readonly IPriceListsService priceListsService;
        private readonly ITotalsService totalsService;

        public OrderSuppliersController(
            IOrderSupplierService orderSupplierService,
            UserManager<ApplicationUser> userManager,
            ISuppliersService suppliersService,
            IMaterialsService materialsService,
            IPriceListsService priceListsService,
            ITotalsService totalsService)
        {
            this.orderSupplierService = orderSupplierService;
            this.userManager = userManager;
            this.suppliersService = suppliersService;
            this.materialsService = materialsService;
            this.priceListsService = priceListsService;
            this.totalsService = totalsService;
        }

        [Authorize]
        public IActionResult Create()
        {
            var priceLists = this.priceListsService.GetAllPriceLists<PriceListViewModel>();
            var orderSub = new CreateOrderSupplierInputModel[priceLists.Count];
            for (int i = 0; i < orderSub.Length; i++)
            {
                orderSub[i] = new CreateOrderSupplierInputModel();
                orderSub[i].OrderDate = DateTime.UtcNow;
                orderSub[i].Quantity = 0;
                orderSub[i].TotalPrice = 0;
            }

            var viewModel = new OrderSuppliersListViewModel
            {
                Pricelists = priceLists,
                OrderSub = orderSub.ToList(),
            };
            return this.View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(OrderSuppliersListViewModel input)
        {

            var priceLists = input.Pricelists;
            var orderSub = input.OrderSub;
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            int materialId; 
            string materialName;
            int supplierId;
            string supplierName;
            double minQty;
            decimal unitPrice;
            int supplierIdInput;
            int materialIdInput;
            var priceListsDb = this.priceListsService.GetAllPriceLists<PriceListViewModel>();
            List<OrderSupplier> emailSuppliers = new List<OrderSupplier>();
            for (int i = 0; i < priceLists.Count; i++)
            {
                if (input.OrderSub.Select(x => x.Quantity).ToList()[i] > 0)
                {
                    materialName = input.Pricelists.Select(x => x.Material.Name).ToList()[i];
                    materialIdInput = this.materialsService.GetByName(materialName).Id;
                    materialId = this.materialsService.GetById(materialIdInput).Id;
                    supplierName = input.Pricelists.Select(x => x.Supplier.Name).ToList()[i];
                    supplierIdInput = this.suppliersService.GetByName(supplierName).Id;
                    supplierId = this.suppliersService.GetById(supplierIdInput).Id;
                    minQty = input.Pricelists.Select(x => x.MinimumQuantityPerOrder).ToList()[i];
                    unitPrice = input.Pricelists.Select(x => x.UnitPrice).ToList()[i];

                    var priceList = this.priceListsService.GetByElements<PriceListViewModel>(materialIdInput, supplierId, minQty, unitPrice);
                    var user = await this.userManager.GetUserAsync(this.User);
                    OrderSupplier orderSupplier = await this.orderSupplierService.CreateAsync(input.OrderSub[i], priceList, user.Id);
                    
                    //await this.totalsService.ChangeOrderStatus(orderSupplier.Id, true);


                    /*SupplyOrderStatus supplyOrderStatus = new SupplyOrderStatus
                    {
                        OrderId = orderSupplier.Id,
                        OrderStatus = orderSupplier.Status,
                    };*/

                    emailSuppliers.Add(orderSupplier);

                }
            }

            await this.orderSupplierService.SendMailSupplier(emailSuppliers);
            return this.RedirectToAction(nameof(this.All));
        }

        public IActionResult All()
        {
            var viewModel = this.orderSupplierService
                .GetAllOrderSuppliers<OrderSupplierViewModel>();
                //.Where(x=>x.Status==OrderStatus.Completed || x.Status==OrderStatus.Canceled);

            return this.View(viewModel);
        }

        [HttpGet("{id}/{supplierid}/{qty}/{unitprice}/{totalprice}/{odate}")]
        public async Task<IActionResult> Delete(int id, int supplierid, double qty, decimal unitprice, decimal totalprice, DateTime odate)
        {
            await this.orderSupplierService.DeleteAsync(id, supplierid, qty, unitprice, totalprice, odate);
            return this.RedirectToAction(nameof(this.All));
        }
    }
}
