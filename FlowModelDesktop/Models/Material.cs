using System;
using System.Collections.Generic;

namespace FlowModelDesktop.Models
{
    public partial class Material
    {
        public Material()
        {
            ParameterValues = new HashSet<ParameterValue>();
        }

        public long Id { get; set; }
        public string Type { get; set; } = null!;

        public virtual ICollection<ParameterValue> ParameterValues { get; set; }
    }
}
