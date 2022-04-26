using System;
using System.Collections.Generic;

namespace FlowModelDesktop.Models
{
    public partial class User
    {
        public long Id { get; set; }
        public string Role { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
