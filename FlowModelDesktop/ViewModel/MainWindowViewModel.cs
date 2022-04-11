using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using FlowModelDesktop.Models;
using FlowModelDesktop.Services;
using WPF_MVVM_Classes;
using Math = FlowModelDesktop.Models.MathModel;
using ViewModelBase = FlowModelDesktop.Services.ViewModelBase;

namespace FlowModelDesktop.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private InputData _inputData = new InputData();
        private DbData _dbData = new DbData();
        private IEnumerable<decimal> _temperatureP;
        private IEnumerable<decimal> _viscosity;
        private RelayCommand? _calculateCommand;
        private RelayCommand? _openChartsCommand;

        public InputData InputData
        {
            get => _inputData;
            set
            {
                _inputData = value;
                OnPropertyChanged();
            }
        }

        public DbData DbData
        {
            get => _dbData;
            set
            {
                _dbData = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<decimal> TemperatureP
        {
            get => _temperatureP;
            set
            {
                _temperatureP = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<decimal> Viscosity
        {
            get => _viscosity;
            set
            {
                _viscosity = value;
                OnPropertyChanged();
            }
        }


        public RelayCommand CalculateCommand
        {
            get
            {
                return _calculateCommand ??= new RelayCommand(x =>
                {
                    var math = new Math();
                    math.Calculation(InputData, DbData, out decimal Q, out List<decimal> Tp, out List<decimal> Etap);
                    TemperatureP = Tp;
                    Viscosity = Etap;
                });
            }
        }

        public RelayCommand OpenChartsCommand
        {
            get
            {
                return _openChartsCommand ??= new RelayCommand(x =>
                {
                    var child = new ChartsWindowViewModel(TemperatureP, Viscosity);
                    Show(child);
                });
            }
        }
    }
}
