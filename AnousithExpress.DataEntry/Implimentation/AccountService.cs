using AnousithExpress.DataEntry.Interface;
using AnousithExpress.DataEntry.Models;
using AnousithExpress.DataEntry.Utility;
using AnousithExpress.DataEntry.ViewModels.Admin;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace AnousithExpress.DataEntry.Implimentation
{
    public class AccountService : IAccounts
    {
        private UserUtility _user = new UserUtility();

        public bool CheckExistingAccount(string username)
        {
            using (var db = new EntityContext())
            {
                return _user.GetAll(db).Any(u => u.Username == username);
            }
        }

        public bool CheckExistingAccount(string username, int id)
        {
            using (var db = new EntityContext())
            {
                return _user.GetAll(db).Where(x => x.Id != id).Any(x => x.Username == username);
            }
        }

        public int CreateAccount(AccountModel model)
        {
            using (var db = new EntityContext())
            {
                if (!CheckExistingAccount(model.Username))
                {
                    TbUser newUser = new TbUser
                    {
                        isDelete = false,
                        Username = model.Username,
                        Password = model.Password,
                        Phonenumber = model.Phonenumber,
                        Role = db.tbRoles.FirstOrDefault(r => r.Role == model.Role),
                        Status = db.tbStatuses.First(s => s.Status == model.Status)
                    };
                    db.tbUsers.Add(newUser);
                    db.SaveChanges();
                    return newUser.Id;
                }
                else
                {
                    return 0;
                }

            }
        }

        public bool DeleteAccount(int id)
        {
            using (var db = new EntityContext())
            {
                var source = _user.GetAll(db).FirstOrDefault(x => x.Id == id);
                if (source == null)
                {
                    return false;
                };
                db.Entry(source).State = EntityState.Modified;
                source.isDelete = true;
                db.SaveChanges();
                return true;
            }
        }

        public List<AccountModel> GetAll()
        {
            using (var db = new EntityContext())
            {
                var source = _user.GetAll(db);
                List<AccountModel> accounts = new List<AccountModel>();
                accounts = source.Select(r => new AccountModel
                {
                    Username = r.Username,
                    Password = r.Password,
                    Status = r.Status.Status,
                    Role = r.Role.Role,
                    Phonenumber = r.Phonenumber,
                    Id = r.Id
                }).ToList();
                return accounts;
            }
        }

        public List<TbUser> GetDeliveryMan()
        {
            using (var db = new EntityContext())
            {
                var source = _user.GetAll(db).Where(u => u.Role.Id == 3);
                return source.ToList();
            }
        }

        public List<TbRole> GetRoles()
        {
            using (var db = new EntityContext())
            {
                return db.tbRoles.ToList();
            }
        }

        public AccountModel GetSingle(int id)
        {
            using (var db = new EntityContext())
            {
                var source = _user.GetAll(db).FirstOrDefault(x => x.Id == id);
                if (source == null)
                {
                    return null;
                };
                var result = new AccountModel
                {
                    Username = source.Username,
                    Password = source.Password,
                    Status = source.Status.Status,
                    Role = source.Role.Role,
                    Phonenumber = source.Phonenumber,
                    Id = source.Id
                };
                return result;
            }
        }

        public List<TbStatus> GetStatus()
        {
            using (var db = new EntityContext())
            {
                return db.tbStatuses.ToList();
            }
        }

        public int UpdateAccount(AccountModel model)
        {
            using (var db = new EntityContext())
            {
                var source = _user.GetAll(db).FirstOrDefault(x => x.Id == model.Id);
                if (source == null)
                {
                    return 0;
                };
                if (CheckExistingAccount(model.Username, model.Id ?? default(int)))
                {
                    return 0;
                };
                db.Entry(source).State = EntityState.Modified;
                source.Username = model.Username;
                source.Password = model.Password;
                source.Phonenumber = model.Phonenumber;
                source.Status = db.tbStatuses.FirstOrDefault(s => s.Status == model.Status);
                source.Role = db.tbRoles.FirstOrDefault(r => r.Role == model.Role);
                db.SaveChanges();
                return model.Id ?? default(int);
            }
        }

        public TbUser UserLogin(string username, string password)
        {
            using (var db = new EntityContext())
            {
                var result = _user.GetAll(db).FirstOrDefault(u => u.Username == username && u.Password == password && u.Status.Id == 1);
                return result;
            }
        }
    }
}
