using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowModelDesktop.Models.Data.Abstract
{
    public interface IParameterValueRepository
    {
        IEnumerable<ParameterValue> GetAll();
        IEnumerable<ParameterValue> GetByMaterialId(long matId);
        IEnumerable<ParameterValue> GetByParameterId(long paramId);
        ParameterValue GetByBothId(long paramId, long matId);
        void Save(ParameterValue obj);
        void Delete(long matId);
    }
}
