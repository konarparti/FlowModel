using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlowModelDesktop.Models.Data.Abstract;

namespace FlowModelDesktop.Models.Data.EntityFramework
{
    public class EFParameterRepository : IRepository<Parameter>
    {
        private readonly FlowModelDbContext _context;

        public EFParameterRepository(FlowModelDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Parameter> GetAll()
        {
            throw new NotImplementedException();
        }

        public Parameter GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Save(Parameter obj)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
