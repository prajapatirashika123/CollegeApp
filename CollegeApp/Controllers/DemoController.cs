using CollegeApp.MyLogging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CollegeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DemoController : ControllerBase
    {
        private readonly ILogger<DemoController> _myLogger;

        public DemoController(ILogger<DemoController> myLogger)
        {
            _myLogger = myLogger;
        }

        [HttpGet]
        public ActionResult Index()
        {
            _myLogger.LogCritical("LogCritical");
            _myLogger.LogDebug("LogDebug");
            _myLogger.LogError("LogError");
            _myLogger.LogInformation("LogInformation");
            _myLogger.LogTrace("LogTrace");
            _myLogger.LogWarning("LogWarning");
            return Ok();
        }
    }
}