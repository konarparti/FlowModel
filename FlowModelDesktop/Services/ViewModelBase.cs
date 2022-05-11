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
        private AddMaterialWindow _addMaterialWindow = null;
        private AddParameterWindow _addParameterWindow = null;
        private AddParameterValueWindow _addParameterValueWindow = null;
        private AddUserWindow _addUserWindow = null;
        private AddMeasureWindow _addMeasureWindow = null;

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
        public bool CloseAddMaterialWindow()
        {
            var result = false;
            if (_addMaterialWindow != null)
            {
                _addMaterialWindow.Close();
                _addMaterialWindow = null;
                result = true;
            }

            return result;
        }
        public bool CloseAddParameterWindow()
        {
            var result = false;
            if (_addParameterWindow != null)
            {
                _addParameterWindow.Close();
                _addParameterWindow = null;
                result = true;
            }

            return result;
        }
        public bool CloseAddParameterValueWindow()
        {
            var result = false;
            if (_addParameterValueWindow != null)
            {
                _addParameterValueWindow.Close();
                _addParameterValueWindow = null;
                result = true;
            }

            return result;
        }
        public bool CloseAddUserWindow()
        {
            var result = false;
            if (_addUserWindow != null)
            {
                _addUserWindow.Close();
                _addUserWindow = null;
                result = true;
            }

            return result;
        }
        public bool CloseAddMeasureWindow()
        {
            var result = false;
            if (_addMeasureWindow != null)
            {
                _addMeasureWindow.Close();
                _addMeasureWindow = null;
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
        protected void ShowAddMaterialWindow(ViewModelBase viewModel, string title)
        {
            viewModel._addMaterialWindow = new AddMaterialWindow()
            {
                DataContext = viewModel,
                Title = title
            };
            viewModel._addMaterialWindow.Closed += (sender, e) => Closed();
            viewModel._addMaterialWindow.Show();

        }
        protected void ShowAddParameterWindow(ViewModelBase viewModel, string title)
        {
            viewModel._addParameterWindow = new AddParameterWindow()
            {
                DataContext = viewModel,
                Title = title
            };
            viewModel._addParameterWindow.Closed += (sender, e) => Closed();
            viewModel._addParameterWindow.Show();

        }
        protected void ShowAddParameterValueWindow(ViewModelBase viewModel, string title)
        {
            viewModel._addParameterValueWindow = new AddParameterValueWindow()
            {
                DataContext = viewModel,
                Title = title
            };
            viewModel._addParameterValueWindow.Closed += (sender, e) => Closed();
            viewModel._addParameterValueWindow.Show();

        }
        protected void ShowAddUserWindow(ViewModelBase viewModel, string title)
        {
            viewModel._addUserWindow = new AddUserWindow()
            {
                DataContext = viewModel,
                Title = title
            };
            viewModel._addUserWindow.Closed += (sender, e) => Closed();
            viewModel._addUserWindow.Show();

        }
        protected void ShowAddMeasureWindow(ViewModelBase viewModel, string title)
        {
            viewModel._addMeasureWindow = new AddMeasureWindow()
            {
                DataContext = viewModel,
                Title = title
            };
            viewModel._addMeasureWindow.Closed += (sender, e) => Closed();
            viewModel._addMeasureWindow.Show();

        }
    }
}
