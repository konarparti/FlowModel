using System;
using FlowModelDesktop.Services;
using LiveCharts;
using System.Collections.Generic;
using LiveCharts.Wpf;

namespace FlowModelDesktop.ViewModel
{
    public class ChartsWindowViewModel : ViewModelBase
    {
        #region Variables

        private ChartValues<decimal> _temperatureChart = new();
        private ChartValues<decimal> _viscosityChart = new();

        private List<string> _length = new List<string>();

        #endregion

        #region Constructors

        public ChartsWindowViewModel(IEnumerable<decimal> Tp, IEnumerable<decimal> Eta, decimal step)
        {
            _temperatureChart.AddRange(Tp);
            _viscosityChart.AddRange(Eta);

           
            for (int i = 0; i < _temperatureChart.Count; i++)
            {
                _length.Add((i * step).ToString());
            }

            YFormatter = value => value.ToString("N");
        }

        #endregion

        #region Properties

        public ChartValues<decimal> Temperature
        {
            get => _temperatureChart;
            set
            {
                _temperatureChart = value;
                OnPropertyChanged();
            }
        }

        public ChartValues<decimal> Viscosity
        {
            get => _viscosityChart;
            set
            {
                _viscosityChart = value;
                OnPropertyChanged();
            }
        }

        public List<string> Length
        {
            get => _length;
            set
            {
                _length = value;
                OnPropertyChanged();
            }
        }

        public Func<double, string> YFormatter { get; set; }
        #endregion


    }
}
