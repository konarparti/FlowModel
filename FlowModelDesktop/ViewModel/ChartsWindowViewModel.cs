using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using FlowModelDesktop.Services;

namespace FlowModelDesktop.ViewModel
{
    public class ChartsWindowViewModel : ViewModelBase
    {
        public ChartsWindowViewModel(int test)
        {
            MessageBox.Show(test.ToString());
        }
    }
}
