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
            return _context.ParameterValues.ToList();
        }

        //TODO: Уникальности нет из-за того, что тут составной ключ
        public ParameterValue GetById(long id)
        {
            return _context.ParameterValues.First(m => m.IdMat == id);
        }

        public void Save(ParameterValue obj)
        {
            if (obj.IdMat == 0)
                _context.ParameterValues.Add(obj);
            else
            {
                var dbEntry = _context.ParameterValues.FirstOrDefault(m => m.IdMat == obj.IdMat);
                if (dbEntry != null)
                {
                    dbEntry.IdMatNavigation = obj.IdMatNavigation;
                    dbEntry.IdParam = obj.IdParam;
                    dbEntry.IdParamNavigation = obj.IdParamNavigation;
                    dbEntry.Value = obj.Value;
                }
            }
            _context.SaveChanges();
        }

        public void Delete(long id)
        {
            var value = _context.ParameterValues.Find(id);
            if (value != null)
                _context.ParameterValues.Remove(value);
            _context.SaveChanges();
        }
    }
}
