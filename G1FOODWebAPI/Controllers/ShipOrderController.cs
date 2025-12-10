using DataAccess.DTO;
using G1FoodLibrary.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace G1FOODWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShipOrderController : ControllerBase
    {
        IOrderRepository _orderRepository;

        public ShipOrderController() => _orderRepository = new OrderRepository();

        // GET: api/<ShipOrderController>
        [HttpGet]
        public void Get()
        {
        }

        // GET api/<ShipOrderController>/5
        [HttpGet("{id}")]
        public string Get(string id)
        {
            return _orderRepository.IsDelivering(id);
        }

        // POST api/<ShipOrderController>
        [HttpPost]
        public void Post([FromBody] OrderDTO orderDTO)
        {
            _orderRepository.UpdateOrderStatus(orderDTO.OrderId, orderDTO.AccountId, orderDTO.OrderStatus);
        }

        // PUT api/<ShipOrderController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ShipOrderController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
