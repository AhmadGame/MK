using Mk.Models;
using Mk.ViewModels;

namespace Mk.Mappers
{
    public class AccountMapper
    {
        public AccountViewModel ToModel(Account account)
        {
            return new AccountViewModel
            {
                email = account.Email,
                password = Util.Decrypt(account.Password),
                isAdmin = account.IsAdmin
            };
        }

        public Account FromModel(AccountViewModel account)
        {
            return new Account
            {
                Email = account.email,
                Password = Util.Crypt(account.password),
                IsAdmin = account.isAdmin
            };
        }
    }
}
