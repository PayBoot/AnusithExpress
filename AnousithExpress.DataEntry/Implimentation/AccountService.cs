using AnousithExpress.DataEntry.Interface;
using AnousithExpress.DataEntry.Models;
using AnousithExpress.DataEntry.Utility;
using System.Linq;

namespace AnousithExpress.DataEntry.Implimentation
{
    public class AccountService : IAccounts
    {
        private UserUtility _user = new UserUtility();
        public TbUser UserLogin(string username, string password)
        {
            using (var db = new EntityContext())
            {
                var result = _user.GetAll(db).FirstOrDefault(u => u.Username == username && u.Password == password);
                return result;
            }
        }
    }
}
