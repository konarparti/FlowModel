using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace FlowModelDesktop.Models
{
    public partial class Measure
    {
        public Measure()
        {
            Parameters = new HashSet<Parameter>();
        }
        [DisplayName("Идентификатор")]
        public long Id { get; set; }
        [DisplayName("Сокращенное наименование")]
        public string Name { get; set; } = null!;

        public virtual ICollection<Parameter> Parameters { get; set; }
    }
}
