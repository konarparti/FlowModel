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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit.Core.Input;

namespace FlowModelDesktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void L_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            L.Text = L.Text.Replace(',', '.');
        }

        private void W_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            W.Text = W.Text.Replace(',', '.');
        }

        private void H_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            H.Text = H.Text.Replace(',', '.');
        }
        private void Tu_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            Tu.Text = Tu.Text.Replace(',', '.');
        }

        private void Vu_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            Vu.Text = Vu.Text.Replace(',', '.');
        }

        private void DeltaZ_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            DeltaZ.Text = DeltaZ.Text.Replace(',', '.');
        }

        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
    }
}
