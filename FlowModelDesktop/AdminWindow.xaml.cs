using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FlowModelDesktop
{
    /// <summary>
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        private List<string> _names = new List<string>()
        {
            "ParameterValues", "Parameters", "IdMeasureNavigation", "IdTypeParamNavigation", "IdMatNavigation",
            "IdParamNavigation"
        };
        public AdminWindow()
        {
            InitializeComponent();
        }

        private void OnAutoGeneratingColumn(object? sender, DataGridAutoGeneratingColumnEventArgs e)
        {

            if (e.PropertyDescriptor is PropertyDescriptor descriptor)
            {
                e.Column.Header = descriptor.DisplayName;
                if (_names.Contains(descriptor.DisplayName))
                {
                    e.Column.Visibility = Visibility.Hidden;
                }
            }
            
        }

        private void AdminWindow_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            FontSize = (ActualHeight + ActualHeight / ActualWidth * ActualWidth) / 64.29;
        }
    }
}
