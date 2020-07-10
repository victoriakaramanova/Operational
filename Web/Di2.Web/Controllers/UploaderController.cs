using Di2.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Di2.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UploaderController : BaseController
    {
        private readonly ICloudinaryService cloudinaryService;

        public UploaderController(ICloudinaryService cloudinaryService)
        {
            this.cloudinaryService = cloudinaryService;
        }

        [HttpPost]
        public async Task<List<string>> Post(IFormFileCollection files)
        {
            foreach (var source in files)
            {

                await this.cloudinaryService.UploadPictureAsync(source, source.FileName);
            }

            return null;
        }
    }
}
