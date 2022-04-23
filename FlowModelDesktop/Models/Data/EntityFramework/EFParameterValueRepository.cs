using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlowModelDesktop.Models.Data.Abstract;

namespace FlowModelDesktop.Models.Data.EntityFramework
{
    public class EFParameterValueRepository : IRepository<ParameterValue>
    {
        private readonly FlowModelDbContext _context;

        public EFParameterValueRepository(FlowModelDbContext context)
        {
            _context = context;
        }

        public IEnumerable<ParameterValue> GetAll()
        {
            throw new NotImplementedException();
        }

        public ParameterValue GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Save(ParameterValue obj)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
