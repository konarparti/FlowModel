using System;
using System.Collections.Generic;

namespace FlowModelDesktop.Models
{
    public partial class Parameter
    {
        public Parameter()
        {
            ParameterValues = new HashSet<ParameterValue>();
        }

        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public long IdTypeParam { get; set; }
        public long? IdMeasure { get; set; }

        public virtual Measure? IdMeasureNavigation { get; set; }
        public virtual TypeParameter IdTypeParamNavigation { get; set; } = null!;
        public virtual ICollection<ParameterValue> ParameterValues { get; set; }
    }
}
