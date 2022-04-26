using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowModelDesktop.Models.Data.Abstract
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();
        T GetById(long id);
        void Save(T obj);
        void Delete(long id);
    }
}
