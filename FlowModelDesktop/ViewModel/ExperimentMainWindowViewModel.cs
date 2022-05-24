using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using Accord.Statistics.Testing;
using FlowModelDesktop.Models;
using FlowModelDesktop.Models.Data.Abstract;
using LiveCharts;
using WPF_MVVM_Classes;
using ViewModelBase = FlowModelDesktop.Services.ViewModelBase;

namespace FlowModelDesktop.ViewModel;

public class ExperimentMainWindowViewModel : ViewModelBase
{
    #region Variables

    private InputData _inputData = new InputData
    {
        H = 0.01m,
        L = 8.2m,
        W = 0.21m,
        DeltaZ = 0.1m
    };
    private Material? _selectedMaterial;
    private Material? _material;
    private IEnumerable<Material> _allMaterials;
    private DbData _dbData = new DbData();
    private bool _isTemperatureChecked;
    private bool _isVelocityChecked;
    private decimal _minRangeValue;
    private decimal _maxRangeValue;
    private bool _isTemperatureCriteriaChecked;
    private bool _isViscosityCriteriaChecked;
    private decimal _step;
    private decimal _modeValue;
    private IEnumerable<ExperimentResult> _experimentalData;
    private ChartValues<decimal> _сriteriaValues = new ChartValues<decimal>();
    private List<string> _chartParams = new List<string>(){"0.1","0.2"};
    private string _lineSeriesTitle;
    private string _axisXTitle;
    private string _axisYTitle;

    private bool _isLinearChecked;
    private bool _isQuadChecked;
    private bool _isCubeChecked;
    private string _formula = string.Empty;
    private string _delta = string.Empty;
    private string _dispersia = string.Empty;
    private string _calculatedFisherValue = string.Empty;
    private string _tableFisherValue = string.Empty;
    private string _modelResults = string.Empty;

    private readonly IParameterValueRepository _parameterValueRepository;
    private readonly IRepository<Material> _materialRepository;
    private readonly IRepository<Parameter> _parameterRepository;


    #endregion

    #region Constructors

    public ExperimentMainWindowViewModel(IParameterValueRepository parameterValueRepository, IRepository<Material> materialRepository, IRepository<Parameter> parameterRepository)
    {
        _parameterValueRepository = parameterValueRepository;
        _materialRepository = materialRepository;
        _parameterRepository = parameterRepository;
        _allMaterials = _materialRepository.GetAll();
    }

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
    public Material? SelectedMaterial
    {
        get => _selectedMaterial;
        set
        {
            _selectedMaterial = value;
            MaterialChanged();
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
    public bool IsTemperatureChecked
    {
        get => _isTemperatureChecked;
        set
        {
            _isTemperatureChecked = value;
            OnPropertyChanged();
        }
    }
    public bool IsVelocityChecked
    {
        get => _isVelocityChecked;
        set
        {
            _isVelocityChecked = value;
            OnPropertyChanged();
        }
    }
    public decimal MinRangeValue
    {
        get => _minRangeValue;
        set
        {
            _minRangeValue = value;
            OnPropertyChanged();
        }
    }
    public decimal MaxRangeValue
    {
        get => _maxRangeValue;
        set
        {
            _maxRangeValue = value;
            OnPropertyChanged();
        }
    }
    public bool IsTemperatureCriteriaChecked
    {
        get => _isTemperatureCriteriaChecked;
        set
        {
            _isTemperatureCriteriaChecked = value;
            OnPropertyChanged();
        }
    }
    public bool IsViscosityCriteriaChecked
    {
        get => _isViscosityCriteriaChecked;
        set
        {
            _isViscosityCriteriaChecked = value;
            OnPropertyChanged();
        }
    }
    public decimal Step
    {
        get => _step;
        set
        {
            _step = value;
            OnPropertyChanged();
        }
    }
    public Material? Material
    {
        get => _material;
        set
        {
            _material = value;
            OnPropertyChanged();
        }
    }
    public decimal ModeValue
    {
        get => _modeValue;
        set
        {
            _modeValue = value;
            OnPropertyChanged();
        }
    }
    public IEnumerable<ExperimentResult> ExperimentalData
    {
        get => _experimentalData;
        set
        {
            _experimentalData = value;
            OnPropertyChanged();
        }
    }
    public ChartValues<decimal> CriteriaValues
    {
        get => _сriteriaValues;
        set
        {
            _сriteriaValues = value;
            OnPropertyChanged();
        }
    }
    public List<string> ChartParams
    {
        get => _chartParams;
        set
        {
            _chartParams = value;
            OnPropertyChanged();
        }
    }
    public string LineSeriesTitle
    {
        get => _lineSeriesTitle;
        set
        {
            _lineSeriesTitle = value;
            OnPropertyChanged();
        }
    }
    public string AxisXTitle
    {
        get => _axisXTitle;
        set
        {
            _axisXTitle = value;
            OnPropertyChanged();
        }
    }
    public string AxisYTitle
    {
        get => _axisYTitle;
        set
        {
            _axisYTitle = value;
            OnPropertyChanged();
        }
    }
    public bool IsLinearChecked
    {
        get => _isLinearChecked;
        set
        {
            _isLinearChecked = value;
            OnPropertyChanged();
        }
    }
    public bool IsCubeChecked
    {
        get => _isCubeChecked;
        set
        {
            _isCubeChecked = value;
            OnPropertyChanged();
        }
    }
    public bool IsQuadChecked
    {
        get => _isQuadChecked;
        set
        {
            _isQuadChecked = value;
            OnPropertyChanged();
        }
    }
    public string Formula
    {
        get => _formula;
        set
        {
            _formula = value;
            OnPropertyChanged();
        }
    }
    public string Dispersia
    {
        get => _dispersia;
        set
        {
            _dispersia = value;
            OnPropertyChanged();
        }
    }
    public string CalculatedFisherValue
    {
        get => _calculatedFisherValue;
        set
        {
            _calculatedFisherValue = value;
            OnPropertyChanged();
        }
    }
    public string TableFisherValue
    {
        get => _tableFisherValue;
        set
        {
            _tableFisherValue = value;
            OnPropertyChanged();
        }
    }
    public string ModelResults
    {
        get => _modelResults;
        set
        {
            _modelResults = value;
            OnPropertyChanged();
        }
    }
    public string Delta
    {
        get => _delta;
        set
        {
            _delta = value;
            OnPropertyChanged();
        }
    }

    public Func<double, string> YFormatter { get; set; } = value => value.ToString("N");
    #endregion

    #region Commands

    public RelayCommand CalculateCommand
    {
        get
        {
            return new RelayCommand( command =>
            {
                CriteriaValues.Clear();
                CheckValues(out var errors);
                if (errors != string.Empty)
                {
                    MessageBox.Show(errors, "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                ExperimentalData = MakeExperiment();
            });
        }
    }

    public RelayCommand SynthesisCommand
    {
        get
        {
            return new RelayCommand(command =>
            {
                if (ExperimentalData == null)
                {
                    MessageBox.Show("Для проведения синтеза регрессионной модели необходимы данные экспериментов",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var x = new List<double>();
                var y = new List<double>();
                var regressionY = new List<double>();
                foreach (var item in ExperimentalData)
                {
                    x.Add((double)item.Param);
                    y.Add((double)item.CriteriaValue);
                }

                var lsm = new LSM(x, y);
                if (IsLinearChecked)
                {
                    lsm.Polynomial(1);
                    for (int i = 0; i < x.Count; i++)
                    {
                        regressionY.Add(lsm.Coeff[1] * x[i] + lsm.Coeff[0]);
                    }
                    Formula =Math.Round(lsm.Coeff[1], 2) + "x" + SaveSign(Math.Round(lsm.Coeff[0], 2)) + "= 0";
                }
                else if (IsQuadChecked)
                {
                    lsm.Polynomial(2);
                    for (int i = 0; i < x.Count; i++)
                    {
                        regressionY.Add(lsm.Coeff[2] * x[i] * x[i] + lsm.Coeff[1] * x[i] + lsm.Coeff[0]);
                    }
                    Formula = Math.Round(lsm.Coeff[2], 2) + "x^2" + SaveSign(Math.Round(lsm.Coeff[1], 2)) + "x" + SaveSign(Math.Round(lsm.Coeff[0], 2)) + "= 0";
                }
                else if (IsCubeChecked)
                {
                    lsm.Polynomial(3);
                    for (int i = 0; i < x.Count; i++)
                    {
                        regressionY.Add(lsm.Coeff[3] * x[i] * x[i] * x[i] + lsm.Coeff[2] * x[i] * x[i] + lsm.Coeff[1] * x[i] + lsm.Coeff[0]);
                    }
                    Formula = Math.Round(lsm.Coeff[3], 2) + "x^3" + SaveSign(Math.Round(lsm.Coeff[2], 2)) + "x^2" + SaveSign(Math.Round(lsm.Coeff[1], 2)) + "x" + SaveSign(Math.Round(lsm.Coeff[0], 2)) + "= 0";
                }
                else
                {
                    MessageBox.Show("Вы не выбрали вид уравнения модели",
                        "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var sum = 0.0;
                for (int i = 0; i < x.Count; i++)
                {
                    sum += regressionY[i];
                }

                var average = sum / x.Count;

                var sumForDispersion = 0.0;

                for (int i = 0; i < x.Count; i++)
                {
                    sumForDispersion += Math.Pow(regressionY[i] - average, 2);
                }

                var residualDispersion = sumForDispersion * (1.0 / (x.Count - 1));

                Dispersia = "\\delta^2 = " + Math.Round(residualDispersion , 4);
                Delta = "\\delta = " + Math.Round(Math.Sqrt(residualDispersion ), 4);

                var sumForDispAdequacy = 0.0;

                for (int i = 0; i < x.Count; i++)
                {
                    sumForDispAdequacy += Math.Pow(y[i] - regressionY[i], 2);
                }

                var dispersionAdequacy = sumForDispAdequacy * (1.0 / (x.Count - 5));

                var fisher = residualDispersion  / dispersionAdequacy;

                CalculatedFisherValue = "F_{calc} = " + Math.Round(fisher, 2);

                var tableFisherValue = new FTest(0.95, x.Count - 5, x.Count - 1);

                TableFisherValue = "F_{crit} = " + $"{Math.Round(tableFisherValue.CriticalValue, 2)}";

                if (tableFisherValue.CriticalValue < fisher)
                    ModelResults = "\\text{Модель адекватна}";
                else
                    ModelResults = "\\text{Модель неадекватна}";
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
        if (InputData.DeltaZ <= 0)
            errors += "Величина шага по длине канала не может быть меньше или равна нулю\n";
        if (Step <= 0)
            errors += "Величина шага варьирования не может быть меньше или равна нулю\n";
        if (SelectedMaterial == null)
            errors += "Вы не выбрали тип материала\n";
        if (!IsTemperatureChecked && !IsVelocityChecked)
            errors += "Вы не выбрали варьируемый параметр\n";
        if (!IsTemperatureCriteriaChecked && !IsViscosityCriteriaChecked)
            errors += "Вы не выбрали критериальный показатель\n";
        if (MinRangeValue >= MaxRangeValue || MaxRangeValue - MinRangeValue < Step || MinRangeValue <= 0)
            errors += "Диапазон варьирования указан неверно\n";
        if (ModeValue <= 0)
            errors += "Варьируемый параметр не может быть меньше или равен нулю\n";

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
            ro = (decimal)_parameterValueRepository
                .GetByBothId((int)parameter.First(p => p.Name == "Плотность").Id, _selectedMaterial.Id).Value,
            To = (decimal)_parameterValueRepository
                .GetByBothId((int)parameter.First(p => p.Name == "Температура плавления").Id, _selectedMaterial.Id)
                .Value,
            Tr = (decimal)_parameterValueRepository
                .GetByBothId((int)parameter.First(p => p.Name == "Температура приведения").Id, _selectedMaterial.Id)
                .Value,
            Mu = (decimal)_parameterValueRepository
                .GetByBothId(
                    (int)parameter.First(p => p.Name == "Коэффициент консистенции при температуре приведения").Id,
                    _selectedMaterial.Id).Value,
            c = (decimal)_parameterValueRepository
                .GetByBothId((int)parameter.First(p => p.Name == "Удельная теплоёмкость").Id, _selectedMaterial.Id)
                .Value,
            alpha_u = (decimal)_parameterValueRepository
                .GetByBothId((int)parameter.First(p => p.Name == "Коэффициент теплоотдачи к материалу").Id,
                    _selectedMaterial.Id).Value,
            b = (decimal)_parameterValueRepository
                .GetByBothId((int)parameter.First(p => p.Name == "Температурный коэффициент вязкости").Id,
                    _selectedMaterial.Id).Value,
            n = (decimal)_parameterValueRepository
                .GetByBothId((int)parameter.First(p => p.Name == "Индекс течения материала").Id, _selectedMaterial.Id)
                .Value
        };

        DbData = temp;

        
    }

    private List<ExperimentResult> MakeExperiment()
    {
        var calc = new MathModel();
        var values = new List<ExperimentResult>();

        for (var i = MinRangeValue; i <= MaxRangeValue; i += Step)
        {
            if (IsTemperatureChecked)
            {
                InputData.Tu = i;
                InputData.Vu = ModeValue;
            }
            else
            {
                InputData.Vu = i;
                InputData.Tu = ModeValue;
            }

            calc.Calculation(InputData, DbData, out _, out var Tp, out var Eta, out _, out _);

            if (IsTemperatureCriteriaChecked)
            {
                values.Add(new ExperimentResult(i, Tp.Last()));
                CriteriaValues.Add(Tp.Last());
            }
            else
            {
                values.Add(new ExperimentResult(i, Eta.Last()));
                CriteriaValues.Add(Eta.Last());
            }

            ChartParams.Add(Math.Round(i, 2).ToString());
        }
        
        if (IsTemperatureChecked)
            AxisXTitle = "Температура крышки, °C";
        else
            AxisXTitle = "Скорость крышки, м/с";

        if (IsTemperatureCriteriaChecked)
        {
            AxisYTitle = "Температура продукта, °C";
            LineSeriesTitle = "Температура продукта, °C = ";
        }
        else
        {
            AxisYTitle = "Вязкость продукта, Па*с";
            LineSeriesTitle = "Вязкость продукта, Па*с = ";
        }
        
        YFormatter = value => value.ToString("N");
        
        return values;

    }

    private string SaveSign(double value)
    {
        if (Math.Sign(value) == -1)
        {
            return $"{value}";
        }
        if (Math.Sign(value) == 1)
        {
            return $"+ {value}";
        }
        if (Math.Sign(value) == 0)
        {
            return $"+ 0";
        }

        return string.Empty;
    }
    #endregion
}