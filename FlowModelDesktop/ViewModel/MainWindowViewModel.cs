using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using FlowModelDesktop.Models;
using FlowModelDesktop.Models.Data.Abstract;
using FlowModelDesktop.Models.Data.EntityFramework;
using FlowModelDesktop.Services;
using WPF_MVVM_Classes;
using Math = FlowModelDesktop.Models.MathModel;
using ViewModelBase = FlowModelDesktop.Services.ViewModelBase;

namespace FlowModelDesktop.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {

        #region Variables

        private readonly IRepository<Material> _materialRepository;
        private readonly IRepository<Measure> _measureRepository;
        private readonly IRepository<Parameter> _parameterRepository;
        private readonly IParameterValueRepository _parameterValueRepository;
        private readonly IRepository<TypeParameter> _typeParameterRepository;
        private readonly IUserRepository _userRepository;

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

        private DbData _dbData = new DbData();
        private IEnumerable<decimal> _temperatureP;
        private IEnumerable<decimal> _viscosity;
        private IEnumerable<Material> _allMaterials;
        private RelayCommand? _calculateCommand;
        private RelayCommand? _openChartsCommand;
        private RelayCommand? _openTableCommand;
        private RelayCommand? _openAuthorizationCommand;
        private decimal _q;
        private TimeSpan _time;
        private decimal _memory;
        private Material _selectedMaterial;
        private Material _material;

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

        public IEnumerable<Material> AllMaterials
        {
            get => _allMaterials;
            set
            {
                _allMaterials = value;
                OnPropertyChanged();
            }
        }

        public Material SelectedMaterial
        {
            get => _selectedMaterial;
            set
            {
                _selectedMaterial = value;
                MaterialChanged();
                OnPropertyChanged();
            }
        }

        public Material Material
        {
            get => _material;
            set
            {
                _material = value;
                OnPropertyChanged();
            }
        }

        #endregion

        public MainWindowViewModel(IRepository<Material> materialRepository, IRepository<Measure> measureRepository,
            IRepository<Parameter> parameterRepository, IParameterValueRepository parameterValueRepository, IRepository<TypeParameter> typeParameterRepository,
            IUserRepository userRepository)
        {
            _materialRepository = materialRepository;
            _measureRepository = measureRepository;
            _parameterRepository = parameterRepository;
            _parameterValueRepository = parameterValueRepository;
            _typeParameterRepository = typeParameterRepository;
            _userRepository = userRepository;
            _allMaterials = _materialRepository.GetAll();
        }

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
                                        "Показатели экономичности программы:\n" +
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
                    if (TemperatureP == null || Viscosity == null)
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
                    var child = new AuthorizationWindowViewModel(_materialRepository, _measureRepository, _parameterRepository,
                        _parameterValueRepository, _typeParameterRepository, _userRepository, this);
                    ShowAuthorization(child);
                });
            }
        }
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


        public RelayCommand MakeExperimentCommand
        {
            get
            {
                return new RelayCommand(command =>
                {
                    if (SelectedMaterial == null)
                    {
                        MessageBox.Show("Вы не выбрали тип материала", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    var experiment = new ExperimentMainWindowViewModel();
                    ShowExperimentMainWindow(experiment, SelectedMaterial.Type);
                });
            }
        }
        #endregion


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
            if (SelectedMaterial == null)
                errors += "Вы не выбрали тип материала\n";
        }

        private void MaterialChanged()
        {
            Material = AllMaterials.First(x => x.Type == _selectedMaterial.Type);
            var parameterValues = _parameterValueRepository.GetByMaterialId((int)_selectedMaterial.Id);
            List<Parameter> parameter = new List<Parameter>();
            foreach (var item in parameterValues)
            {
                parameter.Add(_parameterRepository.GetById(item.IdParam));
            }

            var temp = new DbData()
            {
                ro = (decimal)_parameterValueRepository.GetByBothId((int)parameter.First(p => p.Name == "Плотность").Id, _selectedMaterial.Id).Value,
                To = (decimal)_parameterValueRepository.GetByBothId((int)parameter.First(p => p.Name == "Температура плавления").Id, _selectedMaterial.Id).Value,
                Tr = (decimal)_parameterValueRepository.GetByBothId((int)parameter.First(p => p.Name == "Температура приведения").Id, _selectedMaterial.Id).Value,
                Mu = (decimal)_parameterValueRepository.GetByBothId((int)parameter.First(p => p.Name == "Коэффициент консистенции при температуре приведения").Id, _selectedMaterial.Id).Value,
                c = (decimal)_parameterValueRepository.GetByBothId((int)parameter.First(p => p.Name == "Удельная теплоёмкость").Id, _selectedMaterial.Id).Value,
                alpha_u = (decimal)_parameterValueRepository.GetByBothId((int)parameter.First(p => p.Name == "Коэффициент теплоотдачи к материалу").Id, _selectedMaterial.Id).Value,
                b = (decimal)_parameterValueRepository.GetByBothId((int)parameter.First(p => p.Name == "Температурный коэффициент вязкости").Id, _selectedMaterial.Id).Value,
                n = (decimal)_parameterValueRepository.GetByBothId((int)parameter.First(p => p.Name == "Индекс течения материала").Id, _selectedMaterial.Id).Value
            };

            DbData = temp;

            //очень сложная и странная система,
            //сначала берется материал, который выбран, там известен id материала
            // затем берутся все значения параметров, которые относятся к выбранному материалу (уже отсекаются ненужные значения и как следствие параметры)
            // затем формируется список параметров, значение которых уже нашли выше
            // затем каждому полю объекта DbData даем значение, которое берется из списка значений параметров, учитывая что его название должно быть таким какое надо

        }
        internal void UpdateMaterials()
        {
            AllMaterials = _materialRepository.GetAll();
        }
        #endregion
    }
}
