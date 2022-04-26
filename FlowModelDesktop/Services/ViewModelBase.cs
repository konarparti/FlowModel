using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using FlowModelDesktop.ViewModel;

namespace FlowModelDesktop.Services
{
    public abstract class ViewModelBase : DependencyObject, INotifyPropertyChanged, IDisposable
    {
        public void Dispose()
        {
            OnDispose();
        }


        public event PropertyChangedEventHandler PropertyChanged;


        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }


        protected virtual void OnDispose()
        {
        }

        ///
        /// Окно в котором показывается текущий ViewModel
        ///
        private ChartsWindow _window = null;
        private TableWindow _tableWindow = null;
        private AuthorizationWindow _authorizationWindow = null;
        private AdminWindow _adminWindow = null;

        protected virtual void Closed()
        {

        }

        ///
        /// Методы вызываемый для закрытия окна связанного с ViewModel
        ///
        public bool Close()
        {
            var result = false;
            if (_window != null)
            {
                _window.Close();
                _window = null;
                result = true;
            }
            return result;
        }

        public bool CloseAuthorizationWindow()
        {
            var result = false;
            if (_authorizationWindow != null)
            {
                _authorizationWindow.Close();
                _authorizationWindow = null;
                result = true;
            }

            return result;
        }

        ///
        /// Метод показа ViewModel в окне
        ///
        /// viewModel">
        protected void ShowChart(ViewModelBase viewModel)
        {
            viewModel._window = new ChartsWindow
            {
                DataContext = viewModel
            };
            viewModel._window.Closed += (sender, e) => Closed();
            viewModel._window.Show();
        }

        protected void ShowTable(ViewModelBase viewModel)
        {
            viewModel._tableWindow = new TableWindow()
            {
                DataContext = viewModel
            };
            viewModel._tableWindow.Closed += (sender, e) => Closed();
            viewModel._tableWindow.Show();
        }

        protected void ShowAuthorization(ViewModelBase viewModel)
        {
            viewModel._authorizationWindow = new AuthorizationWindow()
            {
                DataContext = viewModel
            };
            viewModel._authorizationWindow.Closed += (sender, e) => Closed();
            viewModel._authorizationWindow.Show();
        }

        protected void ShowAdmin(ViewModelBase viewModel)
        {
            viewModel._adminWindow = new AdminWindow()
            {
                DataContext = viewModel
            };
            viewModel._adminWindow.Closed += (sender, e) => Closed();
            viewModel._adminWindow.Show();

        }
    }
}
