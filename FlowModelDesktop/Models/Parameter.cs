using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace FlowModelDesktop.Models
{
    public partial class Parameter
    {
        public Parameter()
        {
            ParameterValues = new HashSet<ParameterValue>();
        }
        [DisplayName("Идентификатор")]
        public long Id { get; set; }
        [DisplayName("Наименование")]
        public string Name { get; set; } = null!;
        [DisplayName("Идентификатор типа параметра")]
        public long IdTypeParam { get; set; }
        [DisplayName("Идентификатор единицы измерения")]
        public long? IdMeasure { get; set; }

        public virtual Measure? IdMeasureNavigation { get; set; }
        public virtual TypeParameter IdTypeParamNavigation { get; set; } = null!;
        public virtual ICollection<ParameterValue> ParameterValues { get; set; }
    }
}
