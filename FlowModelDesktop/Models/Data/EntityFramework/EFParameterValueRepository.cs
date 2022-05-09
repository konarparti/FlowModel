using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlowModelDesktop.Models.Data.Abstract;

namespace FlowModelDesktop.Models.Data.EntityFramework
{
    public class EFParameterValueRepository : IParameterValueRepository
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

        public IEnumerable<ParameterValue> GetByMaterialId(long matId)
        {
            return _context.ParameterValues.Where(pv => pv.IdMat == matId);
        }

        public IEnumerable<ParameterValue> GetByParameterId(long paramId)
        {
            return _context.ParameterValues.Where(pv => pv.IdParam == paramId);
        }

        public ParameterValue GetByBothId(long paramId, long matId)
        {
            var paramValue = _context.ParameterValues.First(pv => pv.IdMat == matId && pv.IdParam == paramId);
            return paramValue;
        }

        public void Save(ParameterValue obj)
        {
            if (obj.IdMat == 0)
                _context.ParameterValues.Add(obj);
            else
            {
                var dbEntry = _context.ParameterValues.FirstOrDefault(m => m.IdMat == obj.IdMat && m.IdParam == obj.IdParam);
                if (dbEntry != null)
                {
                    dbEntry.Value = obj.Value;
                }
            }
            _context.SaveChanges();
        }

        public void Delete(long matId)
        {
            var value = _context.ParameterValues.First(pv => pv.IdMat == matId);
            if (value != null)
                _context.ParameterValues.Remove(value);
            _context.SaveChanges();
        }
    }
}
