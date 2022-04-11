using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using FlowModelDesktop.Models;
using FlowModelDesktop.Services;
using WPF_MVVM_Classes;
using Math = FlowModelDesktop.Models.Math;
using ViewModelBase = FlowModelDesktop.Services.ViewModelBase;

namespace FlowModelDesktop.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private InputData _inputData = new InputData();
        private DbData _dbData = new DbData();
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


        public RelayCommand CalculateCommand
        {
            get
            {
                return _calculateCommand ??= new RelayCommand(x =>
                {
                    var math = new Math();
                    math.Calculation(InputData, DbData, out decimal Q, out decimal Tp, out decimal Etap);
                    MessageBox.Show($"Q = {System.Math.Round(Q,2)}\nTp = {System.Math.Round(Tp,2)}\nEtap = {System.Math.Round(Etap,2)}");
                });
            }
        }

        public RelayCommand OpenChartsCommand
        {
            get
            {
                return _openChartsCommand ??= new RelayCommand(x =>
                {
                    var child = new ChartsWindowViewModel(1);
                    Show(child);
                });
            }
        }
    }
}
