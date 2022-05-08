using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace FlowModelDesktop.Models
{
    public partial class ParameterValue
    {
        [DisplayName("Идентификатор параметра")]
        public long IdParam { get; set; }
        [DisplayName("Идентификатор материала")]
        public long IdMat { get; set; }
        [DisplayName("Значение параметра")]
        public double Value { get; set; }

        public virtual Material IdMatNavigation { get; set; } = null!;
        public virtual Parameter IdParamNavigation { get; set; } = null!;
    }
}
