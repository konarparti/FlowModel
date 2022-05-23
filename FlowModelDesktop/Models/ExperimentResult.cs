using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowModelDesktop.Models
{
    public class ExperimentResult
    {
        [DisplayName("Варьруемый параметр")]
        public decimal Param { get; set; }
        [DisplayName("Критериальное значение")]
        public decimal CriteriaValue { get; set; }

        public ExperimentResult(decimal param, decimal criteriaValue)
        {
            Param = param;
            CriteriaValue = criteriaValue;
        }
    }
}
