namespace Di2.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Di2.Data.Common.Repositories;
    using Di2.Data.Models;
    using Di2.Services.Data;
    using Di2.Web.ViewModels.Totals;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    [ApiController]
    [Route("api/[area]/[controller]")]

    public class TotalsCController : AdministrationController
    {
        private readonly ITotalsCustomerService totalsCustomerService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IDeletableEntityRepository<Order> ordersRepository;

        public TotalsCController(
            ITotalsCustomerService totalsCustomerService, 
            UserManager<ApplicationUser> userManager,
            IDeletableEntityRepository<Order> ordersRepository)
        {
            this.totalsCustomerService = totalsCustomerService;
            this.userManager = userManager;
            this.ordersRepository = ordersRepository;
        }

       // [Authorize]
        [HttpPost]
        public async Task<ActionResult<int>> Post(TotalsCInputModel input)
        {
            var order = this.ordersRepository.All().FirstOrDefault(x => x.Id == input.OrderId);
            var ordererId = order.OrdererId;
            var orderer = await this.userManager.FindByIdAsync(ordererId);
            if (!this.totalsCustomerService.IsAvailableQtyEnough(order))
            {
                return this.View("InsufficientQuantityError");
            }

            await this.totalsCustomerService.DecreaseDeliveriesAsync(order);
            var statusId = await this.totalsCustomerService.ChangeOrderStatus(input.OrderId, input.IsCompleted, orderer);
            return statusId;
        }
    }
}
