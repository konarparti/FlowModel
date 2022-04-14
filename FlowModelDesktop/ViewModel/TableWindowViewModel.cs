using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlowModelDesktop.Models;
using FlowModelDesktop.Services;

namespace FlowModelDesktop.ViewModel
{
    internal class TableWindowViewModel : ViewModelBase
    {
        private List<Result> _tableData;

        public List<Result> TableData
        {
            get => _tableData;
            set
            {
                _tableData = value;
                OnPropertyChanged();
            }
        }

        public TableWindowViewModel(List<decimal> TableTemperature, List<decimal> TableViscosity, decimal step)
        {
            _tableData = new List<Result>();
            for (int i = 0; i < TableTemperature.Count; i++)
            {
                _tableData.Add(new Result(i * step, TableTemperature[i], TableViscosity[i]));
            }
        }
    }
}
