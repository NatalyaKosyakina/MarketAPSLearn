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
        [HttpGet("getProducts")]
        public ActionResult<IEnumerable<Product>> GetProducts()
        {
            var products = _repository.GetProducts();
            return Ok(products);
        }

        // POST api/<MarketController>
        [HttpPost("addProduct")]
        public IActionResult AddProduct(ProductModel product)
        {
            var result = _repository.AddProduct(product);
            return Ok(result);
        }

        [HttpGet("getGroups")]
        public ActionResult<IEnumerable<Product>> GetGroups()
        {
            var groups = _repository.GetGroups();
            return Ok(groups);
        }

        // POST api/<MarketController>
        [HttpPost("addGroup")]
        public IActionResult AddGroup(CategoryModel group)
        {
            var result = _repository.AddGroup(group);
            return Ok(result);
        }


        //// PUT api/<MarketController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<MarketController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
