using System;
using System.Collections.Generic;

namespace FlowModelDesktop.Models
{
    public partial class Measure
    {
        public Measure()
        {
            Parameters = new HashSet<Parameter>();
        }

        public long Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Parameter> Parameters { get; set; }
    }
}
