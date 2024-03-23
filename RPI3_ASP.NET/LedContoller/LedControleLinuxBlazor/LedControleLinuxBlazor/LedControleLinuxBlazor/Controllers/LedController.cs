using LedControleLinuxBlazor.Services;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;

namespace LedControleLinuxBlazor.Controllers
{
    [ApiController]
    [Route("api/led")]
    public class LedController : ControllerBase
    {
        private readonly ILedStripService _ledStrip;
        private readonly IAntiforgery _antiforgery;
        public LedController(ILedStripService ledStrip, IAntiforgery antiforgery)
        {
            _ledStrip = ledStrip;
            _antiforgery = antiforgery;
        }
        [HttpGet("/on")]
        [ValidateAntiForgeryToken]
        public IActionResult On()
        {
            if (_ledStrip.ON())
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpGet("/off")]
        [ValidateAntiForgeryToken]
        public IActionResult Off()
        {
            if (_ledStrip.OFF())
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpGet("api/antiforgery/token")]
        public IActionResult GetAntiForgeryToken()
        {
            var tokens = _antiforgery.GetAndStoreTokens(HttpContext);
            Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken, new CookieOptions { HttpOnly = false });
            return NoContent();
        }
    }
}
