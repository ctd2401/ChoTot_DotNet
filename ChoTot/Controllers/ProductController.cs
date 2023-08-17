using ChoTot.BUS;
using ChoTot.MOD;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
namespace ChoTot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        [HttpPost]
        [Route("AddProduct")]
        public IActionResult AddProduct([FromBody] Product item)
        {
            if (item == null) return BadRequest();
            var Result = new ProductBUS().AddProduct(item);
            if (Result != null) return Ok(Result);
            else return NotFound();
        }
        [HttpPost]
        [Route("ViewProduct")]
        public IActionResult ViewProduct(PasePagingParams page)
        {
            if (page==null) return BadRequest();
            var Result = new ProductBUS().ViewProduct(page);
            if (Result != null) return Ok(Result);
            else return NotFound();
        }
        [HttpDelete]
        [Route("DeleteProduct/{ProductCode}")]
        public IActionResult DeleteProduct( string ProductCode)
        {
            if (ProductCode == null) return BadRequest();
            var Result = new ProductBUS().DeleteProduct(ProductCode);
            if (Result != null) return Ok(Result);
            else return NotFound();
        }
        [HttpPatch]
        [Route("UpdateProduct/{ProductCode}")]
        public IActionResult UpdateProduct(string ProductCode,Product changevalue)
        {
            if (ProductCode == null) return BadRequest();
            var Result = new ProductBUS().UpdateProduct(ProductCode, changevalue);
            if (Result != null) return Ok(Result);
            else return NotFound();
        }
    }
}
