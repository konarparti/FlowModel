using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using FlowModelDesktop.Models;
using WPF_MVVM_Classes;
using Math = FlowModelDesktop.Models.Math;

namespace FlowModelDesktop.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private InputData _inputData = new InputData();
        private DbData _dbData = new DbData();

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

        private RelayCommand? _calculateCommand;

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
    }
}
