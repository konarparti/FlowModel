using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlowModelDesktop.Models.Data.Abstract;

namespace FlowModelDesktop.Models.Data.EntityFramework
{
    public class EFMaterialRepository : IRepository<Material>
    {
        private readonly FlowModelDbContext _context;

        public EFMaterialRepository(FlowModelDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Material> GetAll()
        {
            throw new NotImplementedException();
        }

        public Material GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Save(Material obj)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
