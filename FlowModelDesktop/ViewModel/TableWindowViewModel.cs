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

                    workSheet.Cells[1, 3] = "Координата по длине канала, м";
                    workSheet.Cells[1, 4] = "Температура материала, °C";
                    workSheet.Cells[1, 5] = "Вязкость материала, Па*с";

                    

                    for (int i = 2; i <= _step.Count+1; i++)
                    {
                        workSheet.Cells[i, 3] = _step[i - 2];
                        workSheet.Cells[i, 4] = _temperature[i - 2];
                        workSheet.Cells[i, 5] = _viscosity[i - 2];
                    }

                    workSheet.Cells[1, 1] = "Входные данные"; workSheet.Cells[1, 1].Font.Bold = true; 
                    workSheet.Cells[1, 1].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    workSheet.Range[workSheet.Cells[1, 1], workSheet.Cells[1, 2]].Merge();

                    workSheet.Cells[2, 1] = "Тип материала";
                    workSheet.Cells[2, 1].Font.Bold = true;
                    workSheet.Cells[2, 2] = "Поливинилхлорид";

                    workSheet.Cells[3, 1] = "Геометрические параметры канала:"; 
                    workSheet.Cells[3, 1].Font.Bold = true;
                    workSheet.Range[workSheet.Cells[3, 1], workSheet.Cells[3, 2]].Merge();
                    workSheet.Cells[4, 1] = "Ширина канала, м";
                    workSheet.Cells[4, 2] = _input.W;
                    workSheet.Cells[5, 1] = "Глубина канала, м";
                    workSheet.Cells[5, 2] = _input.H;
                    workSheet.Cells[6, 1] = "Длина канала, м";
                    workSheet.Cells[6, 2] = _input.L;

                    workSheet.Cells[7, 1] = "Параметры свойств материала:"; 
                    workSheet.Cells[7, 1].Font.Bold = true;
                    workSheet.Range[workSheet.Cells[7, 1], workSheet.Cells[7, 2]].Merge();
                    workSheet.Cells[8, 1] = "Плотность материала, кг/м³";
                    workSheet.Cells[8, 2] = _dbData.ro;
                    workSheet.Cells[9, 1] = "Удельная теплоёмкость, Дж/(кг*°C)";
                    workSheet.Cells[9, 2] = _dbData.c;
                    workSheet.Cells[10, 1] = "Температура плавления, °C";
                    workSheet.Cells[10, 2] = _dbData.To;

                    workSheet.Cells[11, 1] = "Варьируемые режимные параметры:"; 
                    workSheet.Cells[11, 1].Font.Bold = true;
                    workSheet.Cells[11, 1].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    workSheet.Range[workSheet.Cells[11, 1], workSheet.Cells[11, 2]].Merge();
                    workSheet.Cells[12, 1] = "Скорость движения крышки, м/с";
                    workSheet.Cells[12, 2] = _input.Vu;
                    workSheet.Cells[13, 1] = "Температура крышки, °C";
                    workSheet.Cells[13, 2] = _input.Tu;

                    workSheet.Cells[14, 1] = "Параметры модели"; 
                    workSheet.Cells[14, 1].Font.Bold = true; 
                    workSheet.Cells[14, 1].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    workSheet.Range[workSheet.Cells[14, 1], workSheet.Cells[14, 2]].Merge();

                    workSheet.Cells[15, 1] = "Параметры метода решения:"; 
                    workSheet.Cells[15, 1].Font.Bold = true;
                    workSheet.Range[workSheet.Cells[15, 1], workSheet.Cells[15, 2]].Merge();
                    workSheet.Cells[16, 1] = "Шаг расчета по длине канала, м";
                    workSheet.Cells[16, 2] = _input.DeltaZ;

                    workSheet.Cells[17, 1] = "Эмпирические коэффициенты математической модели:"; 
                    workSheet.Cells[17, 1].Font.Bold = true;
                    workSheet.Range[workSheet.Cells[17, 1], workSheet.Cells[17, 2]].Merge();
                    workSheet.Cells[18, 1] = "Коэффициент консистенции при температуре приведения, Па*с^n";
                    workSheet.Cells[18, 2] = _dbData.Mu;
                    workSheet.Cells[19, 1] = "Температурный коэффициент вязкости, 1/°C";
                    workSheet.Cells[19, 2] = _dbData.b;
                    workSheet.Cells[20, 1] = "Температура приведения, °C";
                    workSheet.Cells[20, 2] = _dbData.Tr;
                    workSheet.Cells[21, 1] = "Индекс течения материала";
                    workSheet.Cells[21, 2] = _dbData.n;
                    workSheet.Cells[22, 1] = "Коэффициент теплоотдачи к материалу, Вт/(м²*°C)";
                    workSheet.Cells[22, 2] = _dbData.alpha_u;


                    workSheet.Cells[23, 1] = "Результаты расчета";
                    workSheet.Cells[23, 1].Font.Bold = true;
                    workSheet.Cells[23, 1].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    workSheet.Range[workSheet.Cells[23, 1], workSheet.Cells[23, 2]].Merge();

                    workSheet.Cells[24, 1] = "Критериальные показатели объекта:"; 
                    workSheet.Cells[24, 1].Font.Bold = true;
                    workSheet.Range[workSheet.Cells[24, 1], workSheet.Cells[24, 2]].Merge();
                    workSheet.Cells[25, 1] = "Производительность канала, кг/ч";
                    workSheet.Cells[25, 2] = Math.Round(_q * 3600, 2);
                    workSheet.Cells[26, 1] = "Температура продукта, ºС";
                    workSheet.Cells[26, 2] = _temperature.Last();
                    workSheet.Cells[27, 1] = "Вязкость продукта, Па*с";
                    workSheet.Cells[27, 2] = _viscosity.Last();

                    workSheet.Cells[28, 1] = "Показатели экономичности программы:"; 
                    workSheet.Cells[28, 1].Font.Bold = true;
                    workSheet.Range[workSheet.Cells[28, 1], workSheet.Cells[28, 2]].Merge();
                    workSheet.Cells[29, 1] = "Время расчета, мс";
                    workSheet.Cells[29, 2] = Math.Round(_time.TotalMilliseconds, 2);
                    workSheet.Cells[30, 1] = "Объем занимаемой оперативной памяти, КБ";
                    workSheet.Cells[30, 2] = _memory / 1024;


                    workSheet.Columns.AutoFit();
                    excelApp.Visible = true;
                });
            }

        }

    }
}
