using System;
using System.Collections.Generic;

namespace FlowModelDesktop.Models
{
    public partial class TypeParameter
    {
        public TypeParameter()
        {
            Parameters = new HashSet<Parameter>();
        }

        public long Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Parameter> Parameters { get; set; }
    }
}
