using DataAccess.DTO;
using G1FoodLibrary.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace G1FOODWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        IAccountRepository accountRepository;

        public UserController() => accountRepository = new AccountRepository();
        // GET: api/<UserController>
        [HttpGet]
        public IEnumerable<AccountDTO> Get()
        {
            var accounts = accountRepository.GetAllUser();
            return accounts;
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public AccountDTO Get(string id)
        {
            var account = accountRepository.GetProfile(id);
            return account;
        }

        // POST api/<UserController>
        [HttpPost]
        public void Post([FromBody] AccountDTO accountDTO)
        {
            if(!accountRepository.IsEmailExist(accountDTO.AccountEmail))
            {
                accountRepository.AddNewUser(accountDTO);
            }
            
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public void Put(string id, [FromBody] AccountDTO accountDTO)
        {
            
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
