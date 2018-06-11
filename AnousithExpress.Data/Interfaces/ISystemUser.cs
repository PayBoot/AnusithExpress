using AnousithExpress.Data.Models;
using AnousithExpress.Data.SingleViewModels;
using System.Collections.Generic;

namespace AnousithExpress.Data.Interfaces
{
    public interface ISystemUser
    {
        List<UserSingleModel> GetAll();
        bool Create(UserSingleModel model);
        bool Edit(UserSingleModel model);
        bool Delete();

        TbUser Login(string username, string password);
    }
}
