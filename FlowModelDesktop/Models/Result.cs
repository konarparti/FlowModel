using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowModelDesktop.Models
{
    internal class Result
    {
        public int Step { get; set; }
        public decimal Temperature { get; set; }
        public decimal Viscosity { get; set; }

        public Result(int step, decimal temperature, decimal viscosity)
        {
            Step = step;
            Temperature = temperature;
            Viscosity = viscosity;
        }
    }
}
