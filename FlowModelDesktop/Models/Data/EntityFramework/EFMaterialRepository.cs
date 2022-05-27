using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlowModelDesktop.Models.Data.Abstract;
using Microsoft.EntityFrameworkCore;

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
            {
                value.ParameterValues.Clear();
                _context.Materials.Remove(value);

                var saved = false;
                while (!saved)
                {
                    try
                    {
                        _context.SaveChanges();
                        saved = true;
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        foreach (var entry in ex.Entries)
                        {
                            if (entry.Entity is ParameterValue)
                            {
                                var proposedValues = entry.CurrentValues;
                                var databaseValues = entry.GetDatabaseValues();

                                foreach (var property in proposedValues.Properties)
                                {
                                    var proposedValue = proposedValues[property];
                                    var databaseValue = databaseValues[property];

                                }
                                entry.OriginalValues.SetValues(databaseValues);
                            }
                            else
                            {
                                throw new NotSupportedException(
                                    "Don't know how to handle concurrency conflicts for "
                                    + entry.Metadata.Name);
                            }
                        }
                    }
                }
            }
        }
    }
}
