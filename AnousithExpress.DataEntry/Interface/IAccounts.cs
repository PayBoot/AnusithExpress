using AnousithExpress.DataEntry.Models;

namespace AnousithExpress.DataEntry.Interface
{
    public interface IAccounts
    {

        TbUser UserLogin(string username, string password);

    }
}
