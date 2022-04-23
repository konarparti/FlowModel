using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlowModelDesktop.Models.Data.Abstract;

namespace FlowModelDesktop.Models.Data.EntityFramework
{
    public class EFMeasureRepository : IRepository<Measure>
    {
        private readonly FlowModelDbContext _context;

        public EFMeasureRepository(FlowModelDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Measure> GetAll()
        {
            throw new NotImplementedException();
        }

        public Measure GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Save(Measure obj)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
