using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlowModelDesktop.Models.Data.Abstract;

namespace FlowModelDesktop.Models.Data.EntityFramework
{
    public class EFUserRepository : IUserRepository
    {
        public IEnumerable<User> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        public bool VerifyUser(string username, string password)
        {
            throw new NotImplementedException();
        }

        public void SaveUser(User user)
        {
            throw new NotImplementedException();
        }

        public void DeleteUser(long id)
        {
            throw new NotImplementedException();
        }
    }
}
