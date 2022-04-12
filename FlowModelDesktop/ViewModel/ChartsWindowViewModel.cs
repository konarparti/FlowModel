using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using FlowModelDesktop.Services;
using LiveCharts;
using LiveCharts.Wpf;

namespace FlowModelDesktop.ViewModel
{
    public class ChartsWindowViewModel : ViewModelBase
    {
        #region Variables

        private ChartValues<decimal> temperatureChart = new ChartValues<decimal>();
        private ChartValues<decimal> viscosityChart = new ChartValues<decimal>();

        private SeriesCollection _temperature;
        private SeriesCollection _viscosity;

        #endregion

        #region Constructors

        public ChartsWindowViewModel(IEnumerable<decimal> Tp, IEnumerable<decimal> Eta)
        {
            temperatureChart.AddRange(Tp);
            viscosityChart.AddRange(Eta);

            //_temperature = new SeriesCollection
            //{
            //    new LineSeries
            //    {
            //        Values = temperatureChart
            //    },
            //};

            //_viscosity = new SeriesCollection
            //{
            //    new LineSeries
            //    {
            //        Values = viscosityChart
            //    },
            //};
        }

        #endregion

        #region Properties

        public ChartValues<decimal> Temperature
        {
            get => temperatureChart;
            set
            {
                temperatureChart = value;
                OnPropertyChanged();
            }
        }

        public ChartValues<decimal> Viscosity
        {
            get => viscosityChart;
            set
            {
                viscosityChart = value;
                OnPropertyChanged();
            }
        }
        
        #endregion


    }
}
