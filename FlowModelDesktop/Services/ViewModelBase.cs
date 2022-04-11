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

        ///
        /// Метод показа ViewModel в окне
        ///
        /// viewModel">
        protected void Show(ViewModelBase viewModel)
        {
            viewModel._window = new ChartsWindow
            {
                DataContext = viewModel
            };
            viewModel._window.Closed += (sender, e) => Closed();
            viewModel._window.Show();
        }
    }
}
