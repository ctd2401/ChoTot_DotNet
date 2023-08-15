using Microsoft.AspNetCore.Mvc;
using ChoTot.MOD;
using ChoTot.BUS;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Configuration;
namespace ChoTot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        [HttpPost]
        [Route("Register")]
        public IActionResult Register([FromBody] UserRegister item)
        {
            if (item == null) return BadRequest();
            var Result = new UserBUS().Register(item);
            if (Result != null) return Ok(Result);
            else return NotFound();
        }
        [HttpPost]
        [Route("Login")]
        public IActionResult Login( UserLogin item)
        {
            if (item == null) return BadRequest();
            var Result = new UserBUS().Login(item);
            if (Result != null) return Ok(Result);
            else return NotFound();
        }
        [HttpPost]
        [Route("ViewUser")]
        public IActionResult ViewUser(int quantity)
        {
            if (quantity <=0) return BadRequest();
            var Result = new UserBUS().ViewUser(quantity);
            if (Result != null) return Ok(Result);
            else return NotFound();
        }
    }
}
