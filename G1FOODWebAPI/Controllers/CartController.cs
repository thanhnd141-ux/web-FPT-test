using DataAccess.DTO;
using G1FoodLibrary.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace G1FOODWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        ICartRepository cartRepository;

        public CartController() => cartRepository= new CartRepository();

        // GET: api/<CartController>
        [HttpGet]
        public void Get()
        {
            
        }

        // GET api/<CartController>/5
        [HttpGet("{id}")]
        public IEnumerable<CartDTO> Get(string id)
        {
            var listCart = cartRepository.GetListCartByAccountID(id);
            return listCart;
        }

        // POST api/<CartController>
        [HttpPost]
        public void Post([FromBody] CartDTO cartDTO)
        {
            cartRepository.AddCart(cartDTO);
        }

        // PUT api/<CartController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CartController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            cartRepository.DeleteCart(id);
        }
    }
}
