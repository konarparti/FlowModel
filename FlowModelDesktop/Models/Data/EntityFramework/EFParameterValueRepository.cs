﻿using System;
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
            if (obj.IdMat == obj.IdMatNavigation.Id)
                _context.ParameterValues.Add(obj);
            else
            {
                var dbEntry = _context.ParameterValues.FirstOrDefault(m => m.IdMatNavigation.Id == obj.IdMat);
                if (dbEntry != null)
                {
                    dbEntry.IdMat = obj.IdMat;
                    dbEntry.IdParam = obj.IdParam;
                    dbEntry.Value = obj.Value;
                }
            }
            _context.SaveChanges();
        }

        public void Delete(long matId, long paramId)
        {
            var value = _context.ParameterValues.First(pv => pv.IdMat == matId && pv.IdParam == paramId);
            if (value != null)
                _context.ParameterValues.Remove(value);
            _context.SaveChanges();
        }
    }
}