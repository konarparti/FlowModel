using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlowModelDesktop.Models.Data.Abstract;

namespace FlowModelDesktop.Models.Data.EntityFramework
{
    public class EFTypeParameterRepository : IRepository<TypeParameter>
    {
        private readonly FlowModelDbContext _context;

        public EFTypeParameterRepository(FlowModelDbContext context)
        {
            _context = context;
        }

        public IEnumerable<TypeParameter> GetAll()
        {
            return _context.TypeParameters.ToList();
        }

        public TypeParameter GetById(long id)
        {
            return _context.TypeParameters.First(m => m.Id == id);
        }

        public TypeParameter GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public void Save(TypeParameter obj)
        {
            if (obj.Id == 0)
                _context.TypeParameters.Add(obj);
            else
            {
                var dbEntry = _context.TypeParameters.FirstOrDefault(m => m.Id == obj.Id);
                if (dbEntry != null)
                {
                    dbEntry.Name = obj.Name;
                    dbEntry.Parameters = obj.Parameters;
                }
            }
            _context.SaveChanges();
        }

        public void Delete(long id)
        {
            var value = _context.TypeParameters.Find(id);
            if (value != null)
                _context.TypeParameters.Remove(value);
            _context.SaveChanges();
        }
    }
}
