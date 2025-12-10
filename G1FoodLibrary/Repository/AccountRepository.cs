using G1FoodLibrary.DAO;
using DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G1FoodLibrary.Repository
{
    public class AccountRepository : IAccountRepository
    {
        public IEnumerable<AccountDTO> GetAllStaff() => AccountDAO.Instance.GetAllStaff();

        public IEnumerable<AccountDTO> GetAllUser() => AccountDAO.Instance.GetAllUser();

        public AccountDTO GetProfile(string accountID) => AccountDAO.Instance.GetProfile(accountID);

        public AccountDTO Login(string email, string password) => AccountDAO.Instance.Login(email, password);

        public void UpdateProfile(AccountDTO accountDTO) => AccountDAO.Instance.UpdateAccount(accountDTO);
        public void AddNewUser(AccountDTO accountDTO) => AccountDAO.Instance.AddUserAccount(accountDTO);

        public Boolean IsEmailExist(string email) => AccountDAO.Instance.IsEmailExist(email);
    }
}
