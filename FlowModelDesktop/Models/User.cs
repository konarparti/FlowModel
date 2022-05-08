using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace FlowModelDesktop.Models
{
    public partial class User
    {
        [DisplayName("Идентификатор")]
        public long Id { get; set; }
        [DisplayName("Роль")]
        public string Role { get; set; } = null!;
        [DisplayName("Имя пользователя")]
        public string Username { get; set; } = null!;
        [DisplayName("Пароль")]
        public string Password { get; set; } = null!;
    }
}
