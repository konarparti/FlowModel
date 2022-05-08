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
            return _context.Parameters.ToList();
        }

        public Parameter GetById(long id)
        {
            return _context.Parameters.First(m => m.Id == id);
        }

        public Parameter GetByName(string name)
        {
            return _context.Parameters.FirstOrDefault(m => m.Name.Contains(name));
        }

        public void Save(Parameter obj)
        {
            if (obj.Id == 0)
                _context.Parameters.Add(obj);
            else
            {
                var dbEntry = _context.Parameters.FirstOrDefault(m => m.Id == obj.Id);
                if (dbEntry != null)
                {
                    dbEntry.Name = obj.Name;
                    dbEntry.IdMeasure = obj.IdMeasure;
                    dbEntry.IdTypeParam = obj.IdTypeParam;
                    //TODO: Может тут умереть
                    dbEntry.IdTypeParamNavigation = obj.IdTypeParamNavigation;
                    dbEntry.IdMeasureNavigation = obj.IdMeasureNavigation; 
                    dbEntry.ParameterValues = obj.ParameterValues;
                }
            }
            _context.SaveChanges();
        }

        public void Delete(long id)
        {
            var value = _context.Parameters.Find(id);
            if (value != null)
                _context.Parameters.Remove(value);
            _context.SaveChanges();
        }
    }
}
