using FlowModelDesktop.Services;
using LiveCharts;
using System.Collections.Generic;

namespace FlowModelDesktop.ViewModel
{
    public class ChartsWindowViewModel : ViewModelBase
    {
        #region Variables

        private ChartValues<decimal> _temperatureChart = new();
        private ChartValues<decimal> _viscosityChart = new();

        #endregion

        #region Constructors

        public ChartsWindowViewModel(IEnumerable<decimal> Tp, IEnumerable<decimal> Eta)
        {
            _temperatureChart.AddRange(Tp);
            _viscosityChart.AddRange(Eta);
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
        
        #endregion


    }
}
