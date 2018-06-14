using AnousithExpress.Data.Models;
using AnousithExpress.Data.SingleViewModels;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace AnousithExpress.Data.UtilityClasses
{
    public class UserUtility
    {
        public IQueryable<TbUser> GetAllUser(EntityContext db)
        {
            return db.tbUsers.Include(u => u.Role).Include(u => u.Status).Where(u => u.isDelete == false);
        }

        public TbUser GetSingleUser(int id, EntityContext db)
        {
            return GetAllUser(db).FirstOrDefault(u => u.Id == id) ?? null;
        }

        public List<UserSingleModel> AssignUserListProperty(IQueryable<TbUser> users)
        {
            return users.Select(r => new UserSingleModel
            {
                Id = r.Id,
                Status = r.Status.Status,
                Password = r.Password,
                Phonenumber = r.Phonenumber,
                Role = r.Role.Role,
                Username = r.Username
            }).ToList();
        }
    }
}
