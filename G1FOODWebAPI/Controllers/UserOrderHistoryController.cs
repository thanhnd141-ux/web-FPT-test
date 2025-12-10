using DataAccess.DTO;
using G1FoodLibrary.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace G1FOODWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserOrderHistoryController : ControllerBase
    {
        IOrderRepository orderRepository;

        public UserOrderHistoryController() => orderRepository = new OrderRepository();

        // GET: api/<UserOrderHistoryController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<UserOrderHistoryController>/5
        [HttpGet("{id}")]
        public IEnumerable<OrderDTO> Get(string id)
        {
            var orders = orderRepository.GetListOrderByAccountID(id);
            return orders;
        }

        // POST api/<UserOrderHistoryController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<UserOrderHistoryController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UserOrderHistoryController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
