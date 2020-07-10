namespace Di2.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class ErrorsController : BaseController
    {
        [Route("error/403")]
        public IActionResult InsufficientQuantityError()
        {
            return this.View();
        }
    }
}
