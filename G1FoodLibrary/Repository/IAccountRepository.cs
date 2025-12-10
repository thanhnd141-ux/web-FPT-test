using DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G1FoodLibrary.Repository
{
    public interface IAccountRepository
    {
        public AccountDTO Login(string email, string password);
        public AccountDTO GetProfile(string accountID);
        public void UpdateProfile(AccountDTO accountDTO);
        public IEnumerable<AccountDTO> GetAllUser();
        public IEnumerable<AccountDTO> GetAllStaff();
        public void AddNewUser(AccountDTO accountDTO);
        public Boolean IsEmailExist(string email);
    }
}
