namespace Di2.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using Di2.Common;
    using Di2.Services;
    using Di2.Services.Data;
    using Di2.Services.Messaging;
    using Di2.Web.ViewModels;
    using Di2.Web.ViewModels.Home;
    using Di2.Web.ViewModels.Materials.ViewModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Sandbox;

    public class HomeController : BaseController
    {
        private readonly ICategoriesService categoriesService;

        public HomeController(
            ICategoriesService categoriesService)
        {
            this.categoriesService = categoriesService;
        }

        //[AllowAnonymous]
        public IActionResult Index()
        {
            var viewModel = new IndexViewModel
            {
                Categories = this.categoriesService.GetAllCategories<IndexCategoriesViewModel>(),
            };
            return this.View(viewModel);

            // return this.RedirectToAction("All", "Materials");
            /* var viewModel = new IndexViewModel
             {
                 Materials = this.materialsService
                     .GetAllMaterials<IndexMaterialViewModel>(),
             };*/

            /*StringBuilder sb = new StringBuilder();
            IEnumerable<IndexMaterialViewModel> materials = this.materialsService.GetAllMaterials<IndexMaterialViewModel>();
            foreach (var material in materials)
            {
                sb.AppendFormat(
                    "{0};{1};{2};{3};{4}",
                    material.Name,
                    material.Description,
                    material.ExtraInfo,
                    material.SubCategory.Name,
                    Environment.NewLine);
            }
            var data = Encoding.UTF8.GetBytes(sb.ToString());
            var res = Encoding.UTF8.GetPreamble().Concat(data).ToArray();
            var attachmentFileName = $"materials-{DateTime.UtcNow.ToShortDateString()}.csv";
            var mimeType = "text/csv"; // charset=UTF-8
            var attch = new EmailAttachment
            {
                MimeType = mimeType,
                FileName = attachmentFileName,
                Content = res,
            };
            var attchList = new List<EmailAttachment>();
            attchList.Add(attch);*/

            //UNMARK HERE TO SEND EMAIL!
            // await this.sender.SendEmailAsync(GlobalConstants.Email, "Я сега", "victoriakaramanova@gmail.com", $"Поръчка за {DateTime.UtcNow.ToShortDateString()}", $"Здравейте, приложена е поръчката. Поздрави - {GlobalConstants.SystemName}", attchList);
            // return this.View(viewModel);
        }

        //[HttpPost]
        //public async Task<IActionResult> Upload(ICollection<IFormFile> files)
        //{
        //    var result = await CloudinaryExtension.Upload(this.cloudinary, files);

        //    this.ViewBag.Links = result;
        //    return this.Redirect("/");
        //}


        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return this.View();
        }

        [AllowAnonymous]
        public IActionResult About()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
