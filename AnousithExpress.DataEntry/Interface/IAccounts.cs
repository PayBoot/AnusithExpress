using AnousithExpress.DataEntry.Models;
using AnousithExpress.DataEntry.ViewModels.Admin;
using System.Collections.Generic;

namespace AnousithExpress.DataEntry.Interface
{
    public interface IAccounts
    {

        TbUser UserLogin(string username, string password);
        List<AccountModel> GetAll();
        AccountModel GetSingle(int id);
        int CreateAccount(AccountModel model);
        int UpdateAccount(AccountModel modal);
        bool DeleteAccount(int id);

        bool CheckExistingAccount(string username);
        bool CheckExistingAccount(string username, int id);

        List<TbRole> GetRoles();
        List<TbStatus> GetStatus();
    }
}
