using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace FlowModelDesktop.Models
{
    public partial class TypeParameter
    {
        public TypeParameter()
        {
            Parameters = new HashSet<Parameter>();
        }
        [DisplayName("Идентификатор")]
        public long Id { get; set; }
        [DisplayName("Наименование")]
        public string Name { get; set; } = null!;

        public virtual ICollection<Parameter> Parameters { get; set; }
    }
}
