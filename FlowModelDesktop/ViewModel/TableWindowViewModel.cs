using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlowModelDesktop.Models;
using WPF_MVVM_Classes;
using ViewModelBase = FlowModelDesktop.Services.ViewModelBase;
using Excel = Microsoft.Office.Interop.Excel;

namespace FlowModelDesktop.ViewModel
{
    internal class TableWindowViewModel : ViewModelBase
    {
        private List<Result> _tableData;
        private RelayCommand? _createReportCommand;
        private List<decimal> _temperature = new List<decimal>();
        private List<decimal> _viscosity = new List<decimal>();
        private List<decimal> _step = new List<decimal>();

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
                _step.Add(i * step);
            }
            _temperature = TableTemperature;
            _viscosity = TableViscosity;
        }

        public RelayCommand CreateReportCommand
        {
            get
            {
                //TODO: Добавить обработку ситуации когда экселя нет на целевом ПК

                return _createReportCommand ??= new RelayCommand(c =>
                {
                    Excel.Application excelApp = new Excel.Application();
                    Excel.Workbook workBook;
                    Excel.Worksheet workSheet;
                    excelApp.SheetsInNewWorkbook = 3;

                    workBook = excelApp.Workbooks.Add();
                    workSheet = (Excel.Worksheet)workBook.Worksheets.get_Item(1);
                    workSheet.Name = "Поливинилхлорид";

                    workSheet.Cells[1, 1] = "Координата по длине канала, м";
                    workSheet.Cells[1, 2] = "Температура материала, °C";
                    workSheet.Cells[1, 3] = "Вязкость материала, Па*с";

                    workSheet.Columns[1].AutoFit();
                    workSheet.Columns[2].AutoFit();
                    workSheet.Columns[3].AutoFit();

                    for (int i = 2; i < _step.Count; i++)
                    {
                        workSheet.Cells[i, 1] = _step[i - 2];
                        workSheet.Cells[i, 2] = _temperature[i - 2];
                        workSheet.Cells[i, 3] = _viscosity[i - 2];
                    }
                    excelApp.Visible = true;
                });
            }

        }
    }
}
