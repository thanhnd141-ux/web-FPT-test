using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO
{
    public class AccountDTO
    {
        public string AccountId { get; set; }
        public string AccountEmail { get; set; }
        public string AccountStatus { get; set; }
        public string AccountPassword { get; set; }
        public string AccountName { get; set; }
        public string AccountAddress { get; set; }
        public string AccountPhone { get; set; }
        public DateTime AccountBirthDay { get; set; }
        public DateTime AccountStartDate { get; set; }
    }
}
