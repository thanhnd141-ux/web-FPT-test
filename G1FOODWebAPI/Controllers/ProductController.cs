using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using DataAccess.DTO;
using G1FoodLibrary.Repository;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace G1FOODWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        IProductRepository _productRepository;

        public ProductController() => _productRepository = new ProductRepository();

        // GET: api/<ProductController>
        [HttpGet]
        public IEnumerable<ProductDTO> Get()
        {
            var products = _productRepository.GetProductHome();
            return products;
        }

        // GET api/<ProductController>/5
        [HttpGet("{id}")]
        public ProductDTO Get(string id)
        {
            ProductDTO product = _productRepository.GetProductByProductID(id);
            return product;
        }

        // POST api/<ProductController>
        [HttpPost]
        public void Post([FromBody] ProductDTO product)
        {
            product.ProductId = _productRepository.GetNewProductID(product.ProductName);
            _productRepository.AddProduct(product);
        }

        // PUT api/<ProductController>/5
        [HttpPut("{id}")]
        public void Put(string id, [FromBody] ProductDTO product)
        {
            if (id.Equals(product.ProductId))
            {
                _productRepository.UpdateProduct(product);
            }
            
        }

        // DELETE api/<ProductController>/5
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            _productRepository.DeleteProduct(id);
        }
    }
}
