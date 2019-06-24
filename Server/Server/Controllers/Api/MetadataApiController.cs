using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace Server.Controllers.Api
{
    //[EnableCors(origins: "*", headers: "*", methods: "*")]
    [Produces("application/json")]
    public class MetadataApiController : Controller
    {
        private readonly IHostingEnvironment hostingEnvironment;

        public MetadataApiController(IHostingEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
        }

        // GET: api/datasource/metadata
        [HttpGet("api/datasource/metadata")]
        public IActionResult GetMetadata()
        {
            var fileName = Path.Combine(hostingEnvironment.ContentRootPath, "App_Data", "metadata_cli.json");
            var json = System.IO.File.ReadAllText(fileName);
            return Content(json);
        }

    }
}