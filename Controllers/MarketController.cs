using Market.Models;
using Market.Models.DTO;
using Market.repo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Market.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarketController : ControllerBase
    {
        private readonly IProductRepository _repository;

        public MarketController(IProductRepository repository)
        {
            _repository = repository;
        }
        // GET: api/<MarketController>
        [HttpGet(template:"getProducts")]
        public ActionResult<IEnumerable<Product>> GetProducts()
        {
            var products = _repository.GetProducts();
            return Ok(products);
        }

        // POST api/<MarketController>
        [HttpPost(template: "addProduct")]
        public IActionResult AddProduct(ProductModel product)
        {
            var result = _repository.AddProduct(product);
            return Ok(result);
        }

        [HttpGet(template: "getGroups")]
        public ActionResult<IEnumerable<Product>> GetGroups()
        {
            var groups = _repository.GetGroups();
            return Ok(groups);
        }

        // POST api/<MarketController>
        [HttpPost(template: "addGroup")]
        public IActionResult AddGroup(CategoryModel group)
        {
            var result = _repository.AddGroup(group);
            return Ok(result);
        }


        // PUT api/<MarketController>/5
        [HttpPut(template: "UpdatePrice {id}")]
        public IActionResult UpdatePrice(int id, int price)
        {
            if (_repository.UpdatePrice(id, price))
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        // DELETE api/<MarketController>/5
        [HttpDelete(template: "{id}")]
        public IActionResult Delete(int id)
        {
            if (_repository.DeleteProduct(id))
            {
                return Ok();
            }
            else { return BadRequest(); }
        }

    }
}
