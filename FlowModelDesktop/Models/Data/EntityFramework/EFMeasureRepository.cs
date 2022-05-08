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
            return _context.Measures.ToList();
        }

        public Measure GetById(long id)
        {
            return _context.Measures.First(m => m.Id == id);
        }

        public Measure GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public void Save(Measure obj)
        {
            if (obj.Id == 0)
                _context.Measures.Add(obj);
            else
            {
                var dbEntry = _context.Measures.FirstOrDefault(m => m.Id == obj.Id);
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
            var value = _context.Measures.Find(id);
            if (value != null)
                _context.Measures.Remove(value);
            _context.SaveChanges();
        }
    }
}
