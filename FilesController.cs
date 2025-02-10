using System.Diagnostics;
using System.IO;
using System.Net.Mime;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace asp2.Controllers
{
    [Route("api/v{version:apiVersion}/files")]
    [Authorize]
    [ApiController]
    [ApiVersion(0.1,Deprecated =true)]
    public class FilesController : ControllerBase
    {
        private readonly FileExtensionContentTypeProvider _fileExtensionContentTypeProvider;

        public FilesController(

                FileExtensionContentTypeProvider fileExtensionContentTypeProvider)
        {
            _fileExtensionContentTypeProvider = fileExtensionContentTypeProvider
             ?? throw new System.ArgumentNullException(
                 nameof(fileExtensionContentTypeProvider));
        }
        [HttpGet("{fileId}")]
        public ActionResult GetFile(string fileId)
        {
            // looku up the actual file,depending on the field..
            //demo code 

            var pathToFile = "C:\\Users\\Oluwademilade.Fajolu\\source\\repos\\asp2\\1. Process Management.pdf";
            // check whether the file exists
            var check = System.IO.File.Exists(pathToFile);
            if (!System.IO.File.Exists(pathToFile))
            {
                return NotFound();
            }
            if (!_fileExtensionContentTypeProvider.TryGetContentType(pathToFile, out var contentType))
            {
                contentType = "application/octet-stream";
            }
            var bytes = System.IO.File.ReadAllBytes(pathToFile);
            return File(bytes, contentType, Path.GetFileName(pathToFile));
        }
        [HttpPost]

        public async Task<ActionResult> CreateFile(IFormFile file)
        {
            // Validate the input. Put a limit on filesize to avoid large uploads attacks.
            // Only accept .pdf files (check content-type)
            if (file.Length == 0 || file.Length > 20971520 || file.ContentType != "application/pdf")
            {
                return BadRequest("No file or an invalid one has been inputted. ");
            }


            // Create the file path. Avoid using file.FileName, as an attacker can provide a
            // malicious one, including full paths or relative paths.
            var path = Path.Combine(
            Directory.GetCurrentDirectory(),
            $"uploaded_file_{Guid.NewGuid()}.pdf");



            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return Ok("Your file has been uploaded successfully.");
        }

    }
            
}
