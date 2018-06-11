using AnousithExpress.Data.Interfaces;
using AnousithExpress.Data.Models;
using AnousithExpress.Data.SingleViewModels;
using System;
using System.Collections.Generic;

namespace AnousithExpress.Data.Implementation
{
    public class SystemUserService : ISystemUser
    {
        public bool Create(UserSingleModel model)
        {
            throw new NotImplementedException();
        }

        public bool Delete()
        {
            throw new NotImplementedException();
        }

        public bool Edit(UserSingleModel model)
        {
            throw new NotImplementedException();
        }

        public List<UserSingleModel> GetAll()
        {
            throw new NotImplementedException();
        }

        public TbUser Login(string username, string password)
        {
            throw new NotImplementedException();
        }
    }
}
