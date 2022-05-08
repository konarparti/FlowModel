using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FlowModelDesktop.Models
{
    public partial class Material
    {
        public Material()
        {
            ParameterValues = new HashSet<ParameterValue>();
        }
        [DisplayName("Идентификатор")]
        public long Id { get; set; }
        [DisplayName("Тип материала")]
        public string Type { get; set; } = null!;
        public virtual ICollection<ParameterValue> ParameterValues { get; set; }
    }
}
