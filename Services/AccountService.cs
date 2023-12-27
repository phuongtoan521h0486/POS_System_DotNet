using POS_System_DotNet.Data;
using POS_System_DotNet.Models.Account;

namespace POS_System_DotNet.Services
{
    public class AccountService
    {
        private MyContext ct;
        public AccountService(MyContext ct)
        {
            this.ct = ct;
        }

        public Account findAccountById(int id)
        {
            return ct.Accounts.FirstOrDefault(x => x.AccountId == id);
        }
    }
}
