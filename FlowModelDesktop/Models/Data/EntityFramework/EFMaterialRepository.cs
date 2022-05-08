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
            return _context.Materials.ToList();
        }

        public Material GetById(long id)
        {
            return _context.Materials.First(m => m.Id == id);
        }

        public Material GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public void Save(Material obj)
        {
            if (obj.Id == 0)
                _context.Materials.Add(obj);
            else
            {
                var dbEntry = _context.Materials.FirstOrDefault(m => m.Id == obj.Id);
                if (dbEntry != null)
                {
                    dbEntry.ParameterValues = obj.ParameterValues;
                    dbEntry.Type = obj.Type;
                }
            }
            _context.SaveChanges();
        }

        public void Delete(long id)
        {
            var value = _context.Materials.Find(id);
            if (value != null)
                _context.Materials.Remove(value);
            _context.SaveChanges();
        }
    }
}
