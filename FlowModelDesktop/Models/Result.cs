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
        [DisplayName("Координата по длине канала, м")]
        public decimal Step { get; set; }
        [DisplayName("Температура материала, °C")]
        public decimal Temperature { get; set; }
        [DisplayName("Вязкость материала, Па*с")]
        public decimal Viscosity { get; set; }

        public Result(decimal step, decimal temperature, decimal viscosity)
        {
            Step = step;
            Temperature = temperature;
            Viscosity = viscosity;
        }
    }
}
