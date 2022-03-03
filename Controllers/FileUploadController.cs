using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
		private readonly IWebHostEnvironment _webHostEnvironment;

		public FileUploadController(IWebHostEnvironment webHostEnviroment)
		{
			_webHostEnvironment = webHostEnviroment;
		}

		[HttpPost]

		public async Task<IActionResult> Post([FromForm] IFormFile image)
		{
			if (image == null || image.Length == 0) return BadRequest("Upload Any Image");
		

	string fileName = image.FileName;
			string extension = Path.GetExtension(fileName);

			string[] allow = { ".jpg", ".png" };

			if (!allow.Contains(extension.ToLower())) return BadRequest("Invalid Image, Try Another");

			string fileNameNew = $"{Guid.NewGuid()}{extension}";
			string filePath = Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot", "Files", fileNameNew);

			using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
			{
				await image.CopyToAsync(fileStream);
			}

			return Ok($"Files/{fileNameNew}");
		}
	}
}
