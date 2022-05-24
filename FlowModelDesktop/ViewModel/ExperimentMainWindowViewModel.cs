using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
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

        #endregion
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
}