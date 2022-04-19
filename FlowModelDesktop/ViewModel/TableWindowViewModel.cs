using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
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
        private InputData _input = new InputData();
        private DbData _dbData = new DbData();
        private decimal _q;
        private TimeSpan _time;
        private decimal _memory;

        public List<Result> TableData
        {
            get => _tableData;
            set
            {
                _tableData = value;
                OnPropertyChanged();
            }
        }

        public TableWindowViewModel(List<decimal> TableTemperature, List<decimal> TableViscosity, InputData inputData, DbData dbData, decimal q, TimeSpan time, decimal memory)
        {
            _tableData = new List<Result>();
            for (int i = 0; i < TableTemperature.Count; i++)
            {
                _tableData.Add(new Result(i * inputData.DeltaZ, TableTemperature[i], TableViscosity[i]));
                _step.Add(i * inputData.DeltaZ);
            }
            _temperature = TableTemperature;
            _viscosity = TableViscosity;
           
            _input = inputData;
            _dbData = dbData;
            _q = q;
            _time = time;
            _memory = memory;
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

                    

                    for (int i = 2; i < _step.Count; i++)
                    {
                        workSheet.Cells[i, 1] = _step[i - 2];
                        workSheet.Cells[i, 2] = _temperature[i - 2];
                        workSheet.Cells[i, 3] = _viscosity[i - 2];
                    }

                    workSheet.Cells[1, 4] = "Входные данные"; workSheet.Cells[1, 4].Font.Bold = true; 
                    workSheet.Cells[1, 4].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    workSheet.Range[workSheet.Cells[1, 4], workSheet.Cells[1, 5]].Merge();

                    workSheet.Cells[2, 4] = "Тип материала";
                    workSheet.Cells[2, 4].Font.Bold = true;
                    workSheet.Range[workSheet.Cells[2, 4], workSheet.Cells[2, 5]].Merge();
                    workSheet.Cells[2, 5] = "Поливинилхлорид";

                    workSheet.Cells[3, 4] = "Геометрические параметры канала:"; 
                    workSheet.Cells[3, 4].Font.Bold = true;
                    workSheet.Range[workSheet.Cells[3, 4], workSheet.Cells[3, 5]].Merge();
                    workSheet.Cells[4, 4] = "Ширина канала, м";
                    workSheet.Cells[4, 5] = _input.W;
                    workSheet.Cells[5, 4] = "Глубина канала, м";
                    workSheet.Cells[5, 5] = _input.H;
                    workSheet.Cells[6, 4] = "Длина канала, м";
                    workSheet.Cells[6, 5] = _input.L;

                    workSheet.Cells[7, 4] = "Параметры свойств материала:"; 
                    workSheet.Cells[7, 4].Font.Bold = true;
                    workSheet.Range[workSheet.Cells[7, 4], workSheet.Cells[7, 5]].Merge();
                    workSheet.Cells[8, 4] = "Плотность материала, кг/м³";
                    workSheet.Cells[8, 5] = _dbData.ro;
                    workSheet.Cells[9, 4] = "Удельная темлоёмкость, Дж/(кг*°C)";
                    workSheet.Cells[9, 5] = _dbData.c;
                    workSheet.Cells[10, 4] = "Температура плавления, °C";
                    workSheet.Cells[10, 5] = _dbData.To;

                    workSheet.Cells[11, 4] = "Варьируемые режимные параметры:"; 
                    workSheet.Cells[11, 4].Font.Bold = true;
                    workSheet.Cells[11, 4].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    workSheet.Range[workSheet.Cells[11, 4], workSheet.Cells[11, 5]].Merge();
                    workSheet.Cells[12, 4] = "Скорость движения крышки, м/с";
                    workSheet.Cells[12, 5] = _input.Vu;
                    workSheet.Cells[13, 4] = "Температура крышки, °C";
                    workSheet.Cells[13, 5] = _input.Tu;

                    workSheet.Cells[14, 4] = "Параметры модели"; 
                    workSheet.Cells[14, 4].Font.Bold = true; 
                    workSheet.Cells[14, 4].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    workSheet.Range[workSheet.Cells[14, 4], workSheet.Cells[14, 5]].Merge();

                    workSheet.Cells[15, 4] = "Параметры метода решения:"; 
                    workSheet.Cells[15, 4].Font.Bold = true;
                    workSheet.Range[workSheet.Cells[15, 4], workSheet.Cells[15, 5]].Merge();
                    workSheet.Cells[16, 4] = "Шаг расчета по длине канала, м";
                    workSheet.Cells[16, 5] = _input.DeltaZ;

                    workSheet.Cells[17, 4] = "Эмпирические коэффициенты математической модели:"; 
                    workSheet.Cells[17, 4].Font.Bold = true;
                    workSheet.Range[workSheet.Cells[17, 4], workSheet.Cells[17, 5]].Merge();
                    workSheet.Cells[18, 4] = "Коэффициент консистенции при температуре приведения, Па*с^n";
                    workSheet.Cells[18, 5] = _dbData.Mu;
                    workSheet.Cells[19, 4] = "Температурный коэффициент вязкости, 1/°C";
                    workSheet.Cells[19, 5] = _dbData.b;
                    workSheet.Cells[20, 4] = "Температура приведения, °C";
                    workSheet.Cells[20, 5] = _dbData.Tr;
                    workSheet.Cells[21, 4] = "Индекс течения материала";
                    workSheet.Cells[21, 5] = _dbData.n;
                    workSheet.Cells[22, 4] = "Коэффициент теплоотдачи к материалу, Вт/(м²*°C)";
                    workSheet.Cells[22, 5] = _dbData.alpha_u;


                    workSheet.Cells[23, 4] = "Результаты расчета";
                    workSheet.Cells[23, 4].Font.Bold = true;
                    workSheet.Cells[23, 4].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    workSheet.Range[workSheet.Cells[23, 4], workSheet.Cells[23, 5]].Merge();

                    workSheet.Cells[24, 4] = "Критериальные показатели объекта:"; 
                    workSheet.Cells[24, 4].Font.Bold = true;
                    workSheet.Range[workSheet.Cells[24, 4], workSheet.Cells[24, 5]].Merge();
                    workSheet.Cells[25, 4] = "Производительность канала, кг/ч";
                    workSheet.Cells[25, 5] = Math.Round(_q * 3600, 2);
                    workSheet.Cells[26, 4] = "Температура продукта, ºС";
                    workSheet.Cells[26, 5] = _temperature.Last();
                    workSheet.Cells[27, 4] = "Вязкость продукта, Па*с";
                    workSheet.Cells[27, 5] = _viscosity.Last();

                    workSheet.Cells[28, 4] = "Показатели экономичности программы:"; 
                    workSheet.Cells[28, 4].Font.Bold = true;
                    workSheet.Range[workSheet.Cells[28, 4], workSheet.Cells[28, 5]].Merge();
                    workSheet.Cells[29, 4] = "Время расчета, мс";
                    workSheet.Cells[29, 5] = _time.TotalMilliseconds;
                    workSheet.Cells[30, 4] = "Объем занимаемой оперативной памяти, КБ";
                    workSheet.Cells[30, 5] = _memory / 1024;


                    workSheet.Columns.AutoFit();
                    excelApp.Visible = true;
                });
            }

        }

    }
}
