using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowModelDesktop.Models.Data.Abstract
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAllUsers();
        bool VerifyUser(string username, string password);
        void SaveUser(User user);
        User GetById(long id);

        //TODO: возможно стоит возвращать удаленного user 
        void DeleteUser(long id);
    }
}
