using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace BlazorAPIBlog.Controllers
{
    [Route("api/upload")]
    [ApiController]

    public class UploadController : ControllerBase
    {

        // metodo asistente para meter el archivo en una carpeta
        [HttpPost]
        [ApiExplorerSettings(IgnoreApi = true)] // para que no se vea en swagger
        public IActionResult Upload()
        {
            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("ImagenesPost");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(),folderName);
                
                if (file.Length > 0)
                {
                    var fileName = Guid.NewGuid() + ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName + "/", fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    return Ok(dbPath);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
    }
}
