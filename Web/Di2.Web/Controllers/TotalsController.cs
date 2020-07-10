using Di2.Services.Data;
using Di2.Web.ViewModels.Totals;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Di2.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class TotalsController : ControllerBase
    {
        private readonly ITotalsService totalsService;

        public TotalsController(ITotalsService totalsService)
        {
            this.totalsService = totalsService;
        }

        // [Authorize]
        [HttpPost]
        public async Task<ActionResult<int>> Post(TotalsInputModel input)
        {
            var statusId = await this.totalsService.ChangeOrderStatus(input.OrderId, input.IsCompleted);
            return statusId;
        }
    }
}
