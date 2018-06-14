using AnousithExpress.Data.Interfaces;
using AnousithExpress.Data.Models;
using AnousithExpress.Data.SingleViewModels;
using AnousithExpress.Data.UtilityClasses;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace AnousithExpress.Data.Implementation
{
    public class SystemUserService : ISystemUser
    {
        private UserUtility userUtility;
        public SystemUserService(UserUtility user)
        {
            userUtility = user;
        }
        public bool Create(UserSingleModel model)
        {
            using (var db = new EntityContext())
            {
                var user = new TbUser
                {
                    Username = model.Username,
                    Password = model.Password,
                    Status = db.tbStatuses.FirstOrDefault(s => s.Status == model.Status),
                    Role = db.tbRoles.FirstOrDefault(r => r.Role == model.Role),
                    Phonenumber = model.Phonenumber,
                    isDelete = false
                };
                db.tbUsers.Add(user);
                db.SaveChanges();
                return true;
            }
        }

        public bool Delete(int id)
        {
            using (var db = new EntityContext())
            {
                var user = userUtility.GetSingleUser(id, db);
                if (user == null)
                {
                    return false;
                };
                db.Entry(user).State = EntityState.Modified;
                user.isDelete = true;
                db.SaveChanges();
                return true;
            }
        }

        public bool Edit(UserSingleModel model)
        {
            using (var db = new EntityContext())
            {
                var user = userUtility.GetSingleUser(model.Id ?? default(int), db);
                if (user != null)
                {
                    db.Entry(user).State = EntityState.Modified;
                    user.Password = model.Password;
                    user.Phonenumber = model.Phonenumber;
                    user.Role = db.tbRoles.FirstOrDefault(x => x.Role == model.Role);
                    user.Status = db.tbStatuses.FirstOrDefault(x => x.Status == model.Status);
                    user.Username = model.Username;
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public List<UserSingleModel> GetAll()
        {
            using (var db = new EntityContext())
            {
                var users = userUtility.GetAllUser(db);
                if (users == null)
                {
                    return new List<UserSingleModel>();
                }
                List<UserSingleModel> model = userUtility.AssignUserListProperty(users);
                return model;
            }
        }

        public List<TbRole> GetRoles()
        {
            using (var db = new EntityContext())
            {
                return db.tbRoles.OrderBy(r => r.Id).ToList() ?? new List<TbRole>();
            }
        }

        public UserSingleModel GetSingle(int id)
        {
            using (var db = new EntityContext())
            {
                var user = userUtility.GetSingleUser(id, db);
                if (user != null)
                {
                    var model = new UserSingleModel
                    {
                        Id = user.Id,
                        Status = user.Status.Status,
                        Password = user.Password,
                        Phonenumber = user.Phonenumber,
                        Role = user.Role.Role,
                        Username = user.Username
                    };
                    return model;
                }
                else
                {
                    return null;
                }

            }
        }

        public List<TbStatus> GetStatus()
        {
            using (var db = new EntityContext())
            {
                return db.tbStatuses.OrderBy(s => s.Id).ToList() ?? new List<TbStatus>();
            }
        }

        public TbUser Login(string username, string password)
        {
            using (var db = new EntityContext())
            {
                var user = db.tbUsers
                    .Include(u => u.Role)
                    .Include(u => u.Status)
                    .FirstOrDefault(u => u.Username == username && u.Password == password && u.Status.Id == 1 && u.isDelete == false);
                return user ?? null;
            }
        }
    }
}
