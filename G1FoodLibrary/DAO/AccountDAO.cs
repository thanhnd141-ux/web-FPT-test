using G1FoodLibrary.Context;
using DataAccess.Entities;
using DataAccess.DTO;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using G1FoodLibrary.Hash;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace G1FoodLibrary.DAO
{
    public class AccountDAO
    {
        private DBContext _context;
        private static AccountDAO instance = null;
        private static readonly object instanceLock = new object();

        public static AccountDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new AccountDAO();
                    }
                    return instance;
                }
            }
        }

        public AccountDAO() => _context = new DBContext();

        public AccountDTO Login(string email, string password)
        {
            Account account = null;
            try
            {
                account = _context.Accounts.Where(a => a.AccountEmail.ToLower().Equals(email.ToLower()) && a.AccountPassword.Equals(password)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            _context.Entry(account).State = EntityState.Detached;

            if(account != null)
            {
                AccountDTO accountDTO = new AccountDTO
                {
                    AccountId = account.AccountId,
                    AccountEmail = account.AccountEmail,
                    AccountName = account.AccountName,
                    AccountPhone = account.AccountPhone,
                    AccountAddress = account.AccountAddress,
                    AccountBirthDay = account.AccountBirthDay,
                    AccountStartDate = account.AccountStartDate,
                    AccountStatus = account.AccountStatus
                };

                return accountDTO;
            }

            return null; 
        }

        public AccountDTO GetProfile(string accountID)
        {
            Account account = null;
            try
            {
                account = _context.Accounts.Where(a => a.AccountId.Equals(accountID)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            _context.Entry(account).State = EntityState.Detached;

            if (account != null)
            {
                AccountDTO accountDTO = new AccountDTO
                {
                    AccountId = account.AccountId,
                    AccountEmail = account.AccountEmail,
                    AccountName = account.AccountName,
                    AccountPhone = account.AccountPhone,
                    AccountAddress = account.AccountAddress,
                    AccountBirthDay = account.AccountBirthDay,
                    AccountStartDate = account.AccountStartDate,
                    AccountStatus = account.AccountStatus
                };

                return accountDTO;
            }

            return null;
        }

        public Account GetAccountByAccountID(string accountID)
        {
            Account account = null;
            try
            {
                account = _context.Accounts.Where(a => a.AccountId.Equals(accountID)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return account;
        }

        public void UpdateAccount(AccountDTO accountDTO)
        {
            try
            {
                Account _account = GetAccountByAccountID(accountDTO.AccountId);
                if (_account != null)
                {
                    _account.AccountEmail = accountDTO.AccountEmail;
                    _account.AccountName = accountDTO.AccountName;
                    _account.AccountPhone = accountDTO.AccountPhone;
                    _account.AccountAddress = accountDTO.AccountAddress;
                    _account.AccountBirthDay = accountDTO.AccountBirthDay;
                    _account.AccountStartDate = accountDTO.AccountStartDate;
                    _context.SaveChanges();
                }
                else
                {
                    throw new Exception("The account is not already exist.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<AccountDTO> GetAllUser()
        {
            string s = "US";

            List<Account> users = _context.Accounts.Where(a => a.AccountId.Contains(s)).ToList();
            List<AccountDTO> userDTOs = new List<AccountDTO>();

            foreach (Account account in users)
            {
                userDTOs.Add(new AccountDTO
                {
                    AccountId = account.AccountId,
                    AccountEmail = account.AccountEmail,
                    AccountName = account.AccountName,
                    AccountPhone = account.AccountPhone,
                    AccountAddress = account.AccountAddress,
                    AccountBirthDay = account.AccountBirthDay,
                    AccountStartDate = account.AccountStartDate,
                    AccountStatus = account.AccountStatus
                });
            }

            return userDTOs;
        }

        public IEnumerable<AccountDTO> GetAllStaff()
        {
            string ch = "CH";
            string sp = "SP";

            List<Account> staffs = _context.Accounts.Where(a => a.AccountId.Contains(ch) || a.AccountId.Contains(sp)).ToList();
            List<AccountDTO> staffDTOs = new List<AccountDTO>();

            foreach (Account account in staffs)
            {
                staffDTOs.Add(new AccountDTO
                {
                    AccountId = account.AccountId,
                    AccountEmail = account.AccountEmail,
                    AccountName = account.AccountName,
                    AccountPhone = account.AccountPhone,
                    AccountAddress = account.AccountAddress,
                    AccountBirthDay = account.AccountBirthDay,
                    AccountStartDate = account.AccountStartDate,
                    AccountStatus = account.AccountStatus
                });
            }

            return staffDTOs;
        }

        public void DeleteAccount(string accountID)
        {
            try
            {
                Account _account = GetAccountByAccountID(accountID);
                if (_account != null)
                {
                    _account.AccountStatus = "REMOVED";
                    _context.SaveChanges();
                }
                else
                {
                    throw new Exception("The account is not already exist.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Boolean IsEmailExist(string email)
        {
            try
            {
                Account _account = _context.Accounts.Where(a => a.AccountEmail.ToLower().Contains(email.ToLower())).FirstOrDefault();
                return _account != null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private string GetConnectionString()
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true).Build();
            var strConn = config["ConnectionString:G1FoodDB"];
            return strConn;
        }
        public string GetLastAccountID()
        {
            var connectionString = GetConnectionString();
            var sqlQuery = @"SELECT TOP 1 AccountID
                     FROM Account
                     ORDER BY CAST(RIGHT(AccountID, 4) AS INT) DESC";

            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(sqlQuery, connection))
            {
                connection.Open();
                var result = command.ExecuteScalar();
                return result?.ToString();
            }
        }

        public string GetNewAccountID(string prefix)
        {
            GenerageID g = new GenerageID();
            string accountID = g.GenerateNewID(prefix, GetLastAccountID());

            return accountID;
        }

        public void AddUserAccount(AccountDTO accountDTO)
        {
            try
            {
                Account _account = new Account
                {
                    AccountId = GetNewAccountID("US"),
                    AccountName = accountDTO.AccountName,
                    AccountEmail = accountDTO.AccountEmail,
                    AccountAddress = accountDTO.AccountAddress,
                    AccountPhone = accountDTO.AccountPhone,
                    AccountPassword = accountDTO.AccountPassword,
                    AccountStatus = "ACTIVED",
                    AccountBirthDay = accountDTO.AccountBirthDay,
                    AccountStartDate = DateTime.Now
                };
                
                _context.Accounts.Add(_account);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
