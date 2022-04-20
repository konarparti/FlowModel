using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        #region Variables

        //TODO: Удалить инициализацию свойств здесь после того, как данные будут вводится/приходить из базы,
        //это для того, чтобы не вводить каждый раз вручную данные нашего варианта
        private InputData _inputData = new InputData()
        {
            W = 0.21M,
            H = 0.01M,
            L = 8.2M,
            Vu = 1.2M,
            Tu = 150M,
            DeltaZ = 0.1M
        };
        private DbData _dbData = new DbData()
        {
            Mu = 10000M,
            To = 140M,
            Tr = 170M,
            alpha_u = 450,
            b = 0.04M,
            c = 2100M,
            n = 0.3M,
            ro = 1200M

        };
        private IEnumerable<decimal> _temperatureP;
        private IEnumerable<decimal> _viscosity;
        private RelayCommand? _calculateCommand;
        private RelayCommand? _openChartsCommand;
        private RelayCommand? _openTableCommand;
        private RelayCommand? _openAuthorizationCommand;
        private decimal _q;
        private TimeSpan _time;
        private decimal _memory;

        #endregion

        #region Properties

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

        #endregion

        #region Commands

        public RelayCommand CalculateCommand
        {
            get
            {
                return _calculateCommand ??= new RelayCommand(x =>
                {
                    var math = new Math();
                    try
                    {
                        CheckValues(out var errors);
                        if (errors != string.Empty)
                        {
                            MessageBox.Show(errors, "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        math.Calculation(InputData, DbData, out decimal Q, out List<decimal> Tp, out List<decimal> Etap,
                            out TimeSpan time, out long memory);
                        TemperatureP = Tp;
                        Viscosity = Etap;
                        _q = Q;
                        _time = time;
                        _memory = memory;
                        MessageBox.Show("Критериальные показатели объекта: \n" +
                                        $"Производительность канала, кг/ч: {System.Math.Round(Q * 3600, 2) }\n" +
                                        $"Температура продукта, ºС: {Tp.Last()}\n" +
                                        $"Вязкость продукта, Па*с: {Etap.Last()}\n\n" +
                                        "Показатели экономичности программы:\n"+
                                        $"Время расчета, мс: {System.Math.Round(time.TotalMilliseconds, 2)}\n" +
                                        $"Объем занимаемой оперативной памяти, КБ: {memory / 1024}",
                            "Результаты расчета", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Что-то пошло не так, и мы это не обработали\n" +
                                        "Информация по исключению:\n" +
                                        $"Источник: {ex.Source}\n" +
                                        $"Метод: {ex.TargetSite}\n" +
                                        $"Собщение: {ex.Message}\n" +
                                        $"Трассировка стека: {ex.StackTrace}\n" +
                                        $"Внутренние исключения:{ex.InnerException}", "Исключение",
                            MessageBoxButton.OK, MessageBoxImage.Warning);
                    }

                });
            }
        }

        public RelayCommand OpenChartsCommand
        {
            get
            {
                return _openChartsCommand ??= new RelayCommand(x =>
                {
                    // ReSharper disable twice ConditionIsAlwaysTrueOrFalse
                    if(TemperatureP == null || Viscosity == null)
                        MessageBox.Show("Для построения графиков необходимо произвести расчеты", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    else
                    {
                        var child = new ChartsWindowViewModel(TemperatureP, Viscosity, InputData.DeltaZ);
                        ShowChart(child);
                    }
                });
            }
        }

        public RelayCommand OpenAuthorizationCommand
        {
            get
            {
                return _openAuthorizationCommand ??= new RelayCommand(x =>
                {
                    var child = new AuthorizationWindowViewModel();
                    ShowAuthorization(child);
                });
            }
        }

        #endregion

        public RelayCommand OpenTableCommand
        {
            get
            {
                return _openTableCommand ??= new RelayCommand(x =>
                {
                    if (TemperatureP == null || Viscosity == null)
                        MessageBox.Show("Для построения таблицы необходимо произвести расчеты", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    else
                    {
                        var child = new TableWindowViewModel((List<decimal>)TemperatureP, (List<decimal>)Viscosity, InputData, DbData, _q, _time, _memory);
                        ShowTable(child);
                    }
                });
            }
        }

        #region Functions

        private void CheckValues(out string errors)
        {
            errors = string.Empty;
            if (InputData.W <= 0)
                errors += "Ширина канала не может быть меньше или равна нулю\n";
            if (InputData.H <= 0)
                errors += "Высота канала не может быть меньше или равна нулю\n";
            if (InputData.L <= 0)
                errors += "Длина канала не может быть меньше или равна нулю\n";
            if (InputData.Vu <= 0)
                errors += "Скорость крышки не может быть меньше или равна нулю\n";
            if (InputData.Tu <= 0)
                errors += "Температура крышки не может быть меньше или равна нулю\n";
            if (InputData.DeltaZ <= 0)
                errors += "Величина шага не может быть меньше или равна нулю\n";
        }

        #endregion
    }
}
