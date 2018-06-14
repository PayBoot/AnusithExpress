using AnousithExpress.Data.Interfaces;
using AnousithExpress.Data.Models;
using AnousithExpress.Data.SingleViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

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
            using (var db = new EntityContext())
            {
                var user = db.tbUsers
                    .Include(u => u.Role)
                    .Include(u => u.Status)
                    .FirstOrDefault(u => u.Username == username && u.Password == password && u.Status.Id == 1);
                return user ?? null;
            }
        }
    }
}
