using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SolaraNet.Models;

namespace SolaraNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : BaseController
    {
        public ImageController(IMapper mapper, ILogger<ImageController> logger) : base(mapper, logger)
        {
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadImages([FromForm] FileUpload file, CancellationToken cancellationToken)
        {
            ImageLoader uploader = new();
            var result = await uploader.UploadImage(file.Image);
            return Ok(result);
        }


        [HttpGet("{name}")]
        public async Task<IActionResult> GetImage(string name)
        {
            byte[] b = System.IO.File.ReadAllBytes(name);
            return File(b, "image/jpeg");
        }

    }
}