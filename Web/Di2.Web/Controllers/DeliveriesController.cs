using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Di2.Data.Models;
using Di2.Services.Data;
using Di2.Web.ViewModels.Categories.ViewModels;
using Di2.Web.ViewModels.Deliveries;
using Di2.Web.ViewModels.Orders.InputModels;
using Di2.Web.ViewModels.Orders.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Di2.Services.Mapping;
//using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc;
using Di2.Services;
using Microsoft.AspNetCore.Http;

namespace Di2.Web.Controllers
{
    [Route("[controller]/[action]")]
    public class DeliveriesController : BaseController
    {
        private readonly IDeliveriesService deliveriesService;
        private readonly IMaterialsService materialsService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IOrderService orderService;
        private readonly ICategoriesService categoriesService;
        private readonly ICloudinaryService cloudinaryService;

        public DeliveriesController(
            IDeliveriesService deliveriesService,
            IMaterialsService materialsService,
            UserManager<ApplicationUser> userManager,
            IOrderService orderService,
            ICategoriesService categoriesService,
            ICloudinaryService cloudinaryService)
        {
            this.deliveriesService = deliveriesService;
            this.materialsService = materialsService;
            this.userManager = userManager;
            this.orderService = orderService;
            this.categoriesService = categoriesService;
            this.cloudinaryService = cloudinaryService;
        }

        [HttpGet("{materialId}")]
        //[Route("Deliveries/ById/{materialId}")]
        public IActionResult ById(int materialId)
        {
            var viewModel = this.deliveriesService
                .GetByMaterialId<CategoryProductsViewModel>(materialId);
            if (viewModel == null)
            {
                return this.NotFound();
            }

            //return this.RedirectToAction(nameof(this.ById), new { id = materialId});
            return this.View(viewModel);
        }

        [HttpPost] //(Name = "Order")]
        [Authorize]
        public async Task<IActionResult> Order(OrderInputModel input)
        {
            if (this.CheckDeliveredQty(input.MaterialId, input.Quantity) == null)
            {
                //return this.ValidationProblem();
                this.ModelState.AddModelError("Quantity", "Too much qty!");
            }

            List<IFormFile> files = input.PicturesFormFiles;
            List<string> customerImages = new List<string>();
            string pic;
            //OrderViewModel viewModel = input.To<OrderViewModel>();
            if (this.ModelState.IsValid)
            {
                var category = this.materialsService.GetById(input.MaterialId).Category;
                var catEng = this.categoriesService.GetByNameBg<CategoryViewModel>(category.Name).NameEng;
                var user = await this.userManager.GetUserAsync(this.User);
                if (category.Id == 1 && files != null)
                {
                    foreach (var file in files.Where(x => x.Length > 0))
                    {
                        pic = await this.cloudinaryService.UploadPictureAsync(file, input.MaterialName);
                        customerImages.Add(pic);
                    }
                }

                await this.orderService.CreateOrder(input, user.Id, customerImages = null);
                /*else
                {
                    await this.orderService.CreateOrder(input, user.Id, customerImages = null);
                }*/

                return this.RedirectToAction("ByName", "Categories", new { name = catEng });
            }

            //return this.View(input);
            return this.RedirectToAction("ById", "Deliveries", new { materialId = input.MaterialId });
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult CheckDeliveredQty(int materialId, double quantity)
        {
            var comparison = this.deliveriesService.GetDeliveredQuantityPerProduct(materialId, quantity);
            if (!comparison)
            {
                return null; //this.Json("Order less!");
            }

            return this.Ok();//this.Json(true);
        }
    }
}
