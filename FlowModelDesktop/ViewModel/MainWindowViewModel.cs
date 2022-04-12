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
                        math.Calculation(InputData, DbData, out decimal Q, out List<decimal> Tp, out List<decimal> Etap,
                            out TimeSpan time, out long memory);
                        TemperatureP = Tp;
                        Viscosity = Etap;
                        MessageBox.Show($"Производительность канала, кг/с: {System.Math.Round(Q, 2)}\n" +
                                        $"Температура продукта, ºС: {System.Math.Round(Tp.Last(), 2)}\n" +
                                        $"Вязкость продукта, Па*с: {System.Math.Round(Etap.Last(), 2)}\n" +
                                        $"Время расчета, мс: {time.TotalMilliseconds}\n" +
                                        $"Объем занимаемой опреативной памяти, Кб: {memory / 1024}",
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
                        var child = new ChartsWindowViewModel(TemperatureP, Viscosity);
                        Show(child);
                    }
                });
            }
        }

        #endregion


    }
}
