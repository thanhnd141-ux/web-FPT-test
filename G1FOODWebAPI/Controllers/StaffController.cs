using DataAccess.DTO;
using G1FoodLibrary.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace G1FOODWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        IAccountRepository accountRepository;

        public StaffController() => accountRepository = new AccountRepository();
        // GET: api/<UserController>
        [HttpGet]
        public IEnumerable<AccountDTO> Get()
        {
            var accounts = accountRepository.GetAllStaff();
            return accounts;
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public AccountDTO Get(string id)
        {
            var account = accountRepository.GetProfile(id);
            return account;
        }

        // POST api/<StaffController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<StaffController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<StaffController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
