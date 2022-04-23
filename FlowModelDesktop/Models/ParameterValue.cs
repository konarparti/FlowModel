using System;
using System.Collections.Generic;

namespace FlowModelDesktop.Models
{
    public partial class ParameterValue
    {
        public long IdParam { get; set; }
        public long IdMat { get; set; }
        public double Value { get; set; }

        public virtual Material IdMatNavigation { get; set; } = null!;
        public virtual Parameter IdParamNavigation { get; set; } = null!;
    }
}
