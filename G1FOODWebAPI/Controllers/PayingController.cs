using DataAccess.DTO;
using G1FoodLibrary.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace G1FOODWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayingController : ControllerBase
    {
        IOrderRepository orderRepository;

        public PayingController() => orderRepository = new OrderRepository();

        // GET: api/<PayingController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<PayingController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<PayingController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<PayingController>/5
        [HttpPut("{id}")]
        public void Put(string id, [FromBody] OrderDTO orderDTO)
        {
            orderRepository.CreateOrder(orderDTO);
            orderRepository.CreateOrderDetails(orderDTO);
        }

        // DELETE api/<PayingController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
