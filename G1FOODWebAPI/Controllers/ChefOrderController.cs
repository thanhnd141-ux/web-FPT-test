using DataAccess.DTO;
using G1FoodLibrary.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace G1FOODWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChefOrderController : ControllerBase
    {
        IOrderRepository _orderRepository;

        public ChefOrderController() => _orderRepository = new OrderRepository();

        // GET: api/<ChefOrderController>
        [HttpGet]
        public IEnumerable<OrderDTO> Get()
        {
            var orders = _orderRepository.GetListOrderProcessing();
            return orders;
        }

        // GET api/<ChefOrderController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ChefOrderController>
        [HttpPost]
        public void Post([FromBody] OrderDTO orderDTO)
        {
            _orderRepository.UpdateOrderStatus(orderDTO.OrderId, orderDTO.AccountId, orderDTO.OrderStatus);
        }

        // PUT api/<ChefOrderController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ChefOrderController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
