using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_MVVM_Classes;
using ViewModelBase = FlowModelDesktop.Services.ViewModelBase;

namespace FlowModelDesktop.ViewModel
{
    public class AuthorizationWindowViewModel : ViewModelBase
    {
        #region Variables

        private RelayCommand? _authorizeCommand;

        #endregion

        #region Properties

        public string AuthorizationLogin { get; set; }
        public string AuthorizationPassword { get; set; }

        #endregion

        #region Commands

        public RelayCommand AuthorizeCommand
        {
            get
            {
                return _authorizeCommand ??= new RelayCommand(x =>
                {

                });
            }
        }

        #endregion
    }
}
