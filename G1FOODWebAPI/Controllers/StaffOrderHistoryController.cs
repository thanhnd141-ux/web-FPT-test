using DataAccess.DTO;
using G1FoodLibrary.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace G1FOODWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffOrderHistoryController : ControllerBase
    {
        IOrderRepository orderRepository;

        public StaffOrderHistoryController() => orderRepository = new OrderRepository();
        // GET: api/<StaffOrderHistoryController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<StaffOrderHistoryController>/5
        [HttpGet("{id}")]
        public IEnumerable<OrderDTO> Get(string id)
        {
            if (id == null) return null;

            IEnumerable<OrderDTO> orders;

            if (id.Contains("CH"))
            {
                orders = orderRepository.GetListOrderByAccChef(id);
            } else
            {
                orders = orderRepository.GetListOrderByAccShip(id);
            }
            
            return orders;
        }

        // POST api/<StaffOrderHistoryController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<StaffOrderHistoryController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<StaffOrderHistoryController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
