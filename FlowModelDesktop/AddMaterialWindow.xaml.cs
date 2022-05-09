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
    /// Логика взаимодействия для AddMaterialWindow.xaml
    /// </summary>
    public partial class AddMaterialWindow : Window
    {
        public AddMaterialWindow()
        {
            InitializeComponent();
        }

        private void OnValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            Ro.Text = Ro.Text.Replace(',', '.');
            C.Text = C.Text.Replace(',', '.');
            To.Text = To.Text.Replace(',', '.');
            Mu.Text = Mu.Text.Replace(',', '.');
            B.Text = B.Text.Replace(',', '.');
            Tr.Text = Tr.Text.Replace(',', '.');
            N.Text = N.Text.Replace(',', '.');
            Alpha.Text = Alpha.Text.Replace(',', '.');
        }
        
    }
}
