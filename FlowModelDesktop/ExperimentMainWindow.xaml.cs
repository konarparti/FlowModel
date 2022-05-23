using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
    /// Логика взаимодействия для ExperimentMainWindow.xaml
    /// </summary>
    public partial class ExperimentMainWindow : Window
    {
        public ExperimentMainWindow()
        {
            InitializeComponent();
        }

        private void ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            DeltaZ.Text = DeltaZ.Text.Replace(',', '.');
            H.Text = H.Text.Replace(',', '.');
            L.Text = L.Text.Replace(',', '.');
            W.Text = W.Text.Replace(',', '.');
            Min.Text = Min.Text.Replace(',', '.');
            Max.Text = Max.Text.Replace(',', '.');
            StepRange.Text = StepRange.Text.Replace(',', '.');
            Mode.Text = Mode.Text.Replace(',', '.');
        }

        private void ToggleButton_OnChecked(object sender, RoutedEventArgs e)
        {
            if ((bool)Temperature.IsChecked)
            {
                Range.Content = "≤ T ≤";
                ModeLabel.Content = "Скорость крышки, м/с";
            }

            if ((bool)Velocity.IsChecked)
            {
                Range.Content = "≤ v ≤";
                ModeLabel.Content = "Температура крышки, °C";
            }
        }

       

    }
}
