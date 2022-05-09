using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для AddParameterValueWindow.xaml
    /// </summary>
    public partial class AddParameterValueWindow : Window
    {
        public AddParameterValueWindow()
        {
            InitializeComponent();
            

        }

        private void On_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            Value.Text = Value.Text.Replace(',', '.');
        }

        private void AddParameterValueWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (Title.Contains("Изменение"))
            {
                Materials.IsEnabled = false;
                Parameters.IsEnabled = false;
            }
        }
    }
}
