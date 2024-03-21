using LedController.Services;
using Microsoft.AspNetCore.Mvc;

namespace LedController.Controllers
{
    [ApiController]
    [Route("api/led")]
    public class LedController : ControllerBase
    {
        private readonly ILedStripService _ledStrip;
        public LedController(ILedStripService ledStrip)
        {
            _ledStrip = ledStrip;
        }
        [HttpGet("/on")]
        public IActionResult On()
        {
            if (_ledStrip.ON())
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpGet("/off")]
        public IActionResult Off()
        {
            if (_ledStrip.OFF())
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
