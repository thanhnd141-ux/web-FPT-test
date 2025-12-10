using DataAccess.DTO;
using G1FoodLibrary.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace G1FOODWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        IOrderRepository orderRepository;

        public OrderController() => orderRepository = new OrderRepository();

        // GET: api/<OrderController>
        [HttpGet]
        public IEnumerable<OrderDTO> Get()
        {
            var orderDTOs = orderRepository.GetAllOrder();
            return orderDTOs;
        }

        // GET api/<OrderController>/5
        [HttpGet("{id}")]
        public OrderDTO Get(string id)
        {
            var orderDTO = orderRepository.GetOrderByOrderID(id);
            return orderDTO;
        }

        // POST api/<OrderController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<OrderController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<OrderController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
