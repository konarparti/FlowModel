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
using LiveCharts.Defaults;
using Pearson;
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
    private List<string> _chartParams = new List<string>() { "0.1", "0.2" };
    private string _lineSeriesTitle;
    private string _axisXTitle;
    private string _axisYTitle;

    private bool _isLinearChecked;
    private bool _isQuadChecked;
    private bool _isCubeChecked;
    private string _formula = string.Empty;
    private string _delta = string.Empty;
    private string _dispersiaModelAverage = string.Empty;
    private string _dispersia = string.Empty;
    private string _calculatedFisherValue = string.Empty;
    private string _tableFisherValue = string.Empty;
    private string _modelResults = string.Empty;

    private string _regressiveLineTitle;
    private ChartValues<decimal> _regressiveValues = new ChartValues<decimal>();

    private readonly IParameterValueRepository _parameterValueRepository;
    private readonly IRepository<Material> _materialRepository;
    private readonly IRepository<Parameter> _parameterRepository;

    private string _calculatedPearson = string.Empty;
    private string _tablePearson = string.Empty;
    private IEnumerable<PearsonResult> _pearsonDataGridValues;
    private string _normalPearsonResult = string.Empty;

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
    public string RegressiveLineTitle
    {
        get => _regressiveLineTitle;
        set
        {
            _regressiveLineTitle = value;
            OnPropertyChanged();
        }
    }
    public ChartValues<decimal> RegressiveValues
    {
        get => _regressiveValues;
        set
        {
            _regressiveValues = value;
            OnPropertyChanged();
        }
    }
    public string DispersiaModelAverage
    {
        get => _dispersiaModelAverage;
        set
        {
            _dispersiaModelAverage = value;
            OnPropertyChanged();
        }
    }
    public string CalculatedPearson
    {
        get => _calculatedPearson;
        set
        {
            _calculatedPearson = value;
            OnPropertyChanged();
        }
    }
    public string TablePearson
    {
        get => _tablePearson;
        set
        {
            _tablePearson = value;
            OnPropertyChanged();
        }
    }
    public IEnumerable<PearsonResult> PearsonDataGridValues
    {
        get => _pearsonDataGridValues;
        set
        {
            _pearsonDataGridValues = value;
            OnPropertyChanged();
        }
    }
    public string NormalPearsonResult
    {
        get => _normalPearsonResult;
        set
        {
            _normalPearsonResult = value;
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
            return new RelayCommand(command =>
            {
                try
                {
                    ChartParams.Clear();
                    CriteriaValues.Clear();
                    RegressiveValues.Clear();

                    CheckValues(out var errors);
                    if (errors != string.Empty)
                    {
                        MessageBox.Show(errors, "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    ExperimentalData = MakeExperiment();
                }
                catch
                {
                    MessageBox.Show("В процессе проведения экпериментов возникла ошибка. Уточните данные плана эксперимента.", "Ошибка расчетов", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            });
        }
    }

    public RelayCommand SynthesisCommand
    {
        get
        {
            return new RelayCommand(command =>
            {
                RegressiveValues.Clear();

                var measureDelta = string.Empty;

                var xVariable = string.Empty;
                var yVariable = string.Empty;

                yVariable = IsTemperatureCriteriaChecked ? "T" : "\\eta";
                xVariable = IsTemperatureChecked ? "T_u" : "v_u";

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
                    Formula = Math.Round(lsm.Coeff[1], 2) + $"{xVariable}" + SaveSign(Math.Round(lsm.Coeff[0], 2)) + $"= {yVariable}";
                }
                else if (IsQuadChecked)
                {
                    lsm.Polynomial(2);
                    for (int i = 0; i < x.Count; i++)
                    {
                        regressionY.Add(lsm.Coeff[2] * x[i] * x[i] + lsm.Coeff[1] * x[i] + lsm.Coeff[0]);
                    }
                    Formula = Math.Round(lsm.Coeff[2], 2) + $"{xVariable}^2" + SaveSign(Math.Round(lsm.Coeff[1], 2)) + $"{xVariable}" + SaveSign(Math.Round(lsm.Coeff[0], 2)) + $"= {yVariable}";
                }
                else if (IsCubeChecked)
                {
                    lsm.Polynomial(3);
                    for (int i = 0; i < x.Count; i++)
                    {
                        regressionY.Add(lsm.Coeff[3] * x[i] * x[i] * x[i] + lsm.Coeff[2] * x[i] * x[i] + lsm.Coeff[1] * x[i] + lsm.Coeff[0]);
                    }
                    Formula = Math.Round(lsm.Coeff[3], 2) + $"{xVariable}^3" + SaveSign(Math.Round(lsm.Coeff[2], 2)) + $"{xVariable}^2" + SaveSign(Math.Round(lsm.Coeff[1], 2)) + $"{xVariable}" + SaveSign(Math.Round(lsm.Coeff[0], 2)) + $"= {yVariable}";
                }
                else
                {
                    MessageBox.Show("Вы не выбрали вид уравнения модели",
                        "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                #region Дисперсия модели среднего

                var sum = 0.0;
                for (int i = 0; i < x.Count; i++)
                {
                    sum += y[i];
                }

                var average = sum / x.Count;

                var sumForDispersion = 0.0;

                for (int i = 0; i < x.Count; i++)
                {
                    sumForDispersion += Math.Pow(y[i] - average, 2);
                }

                var dispersiaModelAverage = sumForDispersion * (1.0 / (x.Count - 1));

                #endregion

                #region Дисперсия адекватности

                var sumForDispAdequacy = 0.0;

                for (int i = 0; i < x.Count; i++)
                {
                    sumForDispAdequacy += Math.Pow(y[i] - regressionY[i], 2);
                }

                var dispersionAdequacy = sumForDispAdequacy * (1.0 / (x.Count - 5));

                #endregion

                #region Расчет относительного значения СКО

                var sumForCKO = 0.0;
                for (int i = 0; i < x.Count; i++)
                {
                    sumForCKO += Math.Pow((y[i] - regressionY[i]) / y[i], 2);
                }

                var CKO = Math.Round(Math.Sqrt(1.0 / x.Count * sumForCKO) * 100, 4);

                #endregion

                var fisher = dispersiaModelAverage / dispersionAdequacy;

                CalculatedFisherValue = "F_{calc} = " + Math.Round(fisher, 2);

                var tableFisherValue = new FTest(0.95, x.Count - 5, x.Count - 1);

                TableFisherValue = "F_{crit} = " + $"{Math.Round(tableFisherValue.CriticalValue, 2)}";

                if (tableFisherValue.CriticalValue < fisher)
                    ModelResults = "\\text{Модель адекватна}";
                else
                    ModelResults = "\\text{Модель неадекватна}";

                if (IsTemperatureCriteriaChecked)
                {
                    measureDelta = "\\;^{\\circ}C";
                    RegressiveLineTitle = "Регрессионная модель. Температура продукта, °C";
                }
                else
                {
                    measureDelta = "\\;\\text{Па*с}";
                    RegressiveLineTitle = "Регрессионная модель. Вязкость продукта, Па*с";
                }

                DispersiaModelAverage = "S_{av}^2 = " + $"{Math.Round(dispersiaModelAverage, 4):N4}";

                if (Math.Round(dispersionAdequacy, 4) == 0)
                    Dispersia = "S_{ad}^2 < " + $"0.0001";
                else
                    Dispersia = "S_{ad}^2 = " + $"{Math.Round(dispersionAdequacy, 4):N4}";

                Delta = "\\delta = " + Math.Round(Math.Sqrt(dispersionAdequacy), 4) + measureDelta + ",\\;" + CKO + "\\%";

                foreach (var item in regressionY)
                {
                    RegressiveValues.Add((decimal)item);
                }
            });
        }
    }

    public RelayCommand PirsonCheckCommand
    {
        get
        {
            return new RelayCommand(command =>
            {
                if (ExperimentalData == null)
                {
                    MessageBox.Show("Для проверки гипотезы о нормальности распределения необходимы данные экспериментов",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                var x = new List<double>();
                var y = new List<double>();
                foreach (var item in ExperimentalData)
                {
                    x.Add((double)item.Param);
                    y.Add((double)item.CriteriaValue);
                }

                //третий столбик
                var multiXY = new List<double>();
                for (int i = 0; i < x.Count; i++)
                {
                    multiXY.Add(x[i] * y[i]);
                }

                //расчет среднего для 4 столбика
                var averageMultiXY = multiXY.Sum() / y.Sum();

                //четвертый столбик
                //tex:
                // $$(\overline{x}-x_i)^2n_i$$
                var fourth = new List<double>();
                for (int i = 0; i < x.Count; i++)
                {
                    fourth.Add(Math.Pow(averageMultiXY - x[i], 2) * y[i]);
                }

                //Выборочная исправленная дисперсия
                //tex:
                //$$S^2 = \frac{1}{n-1}\sum{(\overline{x}-x_i)^2n_i}$$

                var selectableDispersion = 1 / (y.Sum() - 1) * fourth.Sum();

                //Выборочное исправленное среднее квадратическое отклонение
                //tex:
                //$$S = \sqrt{S^2}$$

                var selectedCKO = Math.Sqrt(selectableDispersion);

                //tex:
                //$$u_i = \frac{x_i-\overline{x}}{S}$$
                var u = new List<double>();
                for (int i = 0; i < x.Count; i++)
                {
                    u.Add((x[i] - averageMultiXY) / selectedCKO);
                }

                //расчет фи
                //tex:
                //$$\phi(u) = \frac{1}{\sqrt{2\pi}}e^{\frac{-u^2}{2}}$$
                var phi = new List<double>();
                for (int i = 0; i < x.Count; i++)
                {
                    phi.Add(Math.Exp(-Math.Pow(u[i], 2) / 2) / Math.Sqrt(2 * Math.PI));
                }

                //расчет теоретических частот 
                //tex:
                //$$n_{i}^{0} = \frac{nh}{S}\phi(u_i)$$
                var n = new List<double>();
                for (int i = 0; i < x.Count; i++)
                {
                    n.Add(y.Sum() * (double)Step / selectedCKO * phi[i]);
                }

                //последний столбик
                //tex:
                //$$\frac{(n_i-n^{0}_i)^2}{n^{0}_i}$$
                var lastColumn = new List<double>();
                for (int i = 0; i < x.Count; i++)
                {
                    lastColumn.Add(Math.Pow(y[i]-n[i], 2) / n[i]);
                }

                var pearson = lastColumn.Sum();

                var criticalPearson = PearsonCriterion.GetCriticalValue(x.Count - 3);
                if (criticalPearson == -1.0)
                {
                    MessageBox.Show(
                        "Количество экспериментов превышает 10000. Пожалуйста, уменьшите количество экспериментов.",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Stop);
                    return;
                }

                CalculatedPearson = "P_{calc} = " + Math.Round(pearson, 4);
                TablePearson = "P_{crit} = " + Math.Round(criticalPearson, 4);

                var pearsonResult = new List<PearsonResult>();
                for (int i = 0; i < x.Count; i++)
                {
                    pearsonResult.Add(new PearsonResult()
                    {
                        FirstColumn = Math.Round(x[i], 2),
                        SecondColumn = Math.Round(y[i], 2),
                        ThirdColumn = Math.Round(multiXY[i], 2),
                        FourthColumn = Math.Round(fourth[i], 2),
                        FifthColumn = Math.Round(u[i], 2),
                        SixthColumn = Math.Round(phi[i], 2),
                        SeventhColumn = Math.Round(n[i], 2),
                        EighthColumn = Math.Round(lastColumn[i], 2)
                    });
                }

                PearsonDataGridValues = pearsonResult;

                if (pearson > criticalPearson)
                {
                    NormalPearsonResult = "\\text{Нулевая гипотеза о нормальном распределении отвергается}";
                }
                else
                {
                    NormalPearsonResult = "\\text{Нулевая гипотеза о нормальном распределении принимается}";
                }
            });
        }
    }
    #endregion

    #region Functions

    private void CheckValues(out string errors)
    {
        errors = string.Empty;
        if (InputData.W <= 0)
            errors += "Ширина канала не может быть меньше или равна нулю\n\n";
        if (InputData.H <= 0)
            errors += "Высота канала не может быть меньше или равна нулю\n\n";
        if (InputData.L <= 0)
            errors += "Длина канала не может быть меньше или равна нулю\n\n";
        if (InputData.DeltaZ <= 0)
            errors += "Величина шага по длине канала не может быть меньше или равна нулю\n\n";
        if (Step <= 0)
            errors += "Величина шага варьирования не может быть меньше или равна нулю\n\n";
        if (SelectedMaterial == null)
            errors += "Вы не выбрали тип материала\n\n";
        if (!IsTemperatureChecked && !IsVelocityChecked)
            errors += "Вы не выбрали варьируемый параметр\n\n";
        if (!IsTemperatureCriteriaChecked && !IsViscosityCriteriaChecked)
            errors += "Вы не выбрали критериальный показатель\n\n";
        if (MinRangeValue >= MaxRangeValue || MaxRangeValue - MinRangeValue < Step || MinRangeValue <= 0)
            errors += "Диапазон варьирования указан неверно\n\n";
        if (ModeValue <= 0)
            errors += "Варьируемый параметр не может быть меньше или равен нулю\n\n";
        if ((MaxRangeValue - MinRangeValue) / Step < 5)
            errors += "Количество экспериментов не может быть меньше количества коэффициентов математической модели. Уточние диапазон варьирования и/или шаг варьирования\n\n";

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
            ChartParams.Add(Math.Round(i, 2).ToString());
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
        }

        if (IsTemperatureChecked)
            AxisXTitle = "Температура крышки, °C";
        else
            AxisXTitle = "Скорость крышки, м/с";

        if (IsTemperatureCriteriaChecked)
        {
            AxisYTitle = "Температура продукта, °C";
            LineSeriesTitle = "Детерминированная модель. Температура продукта, °C";
        }
        else
        {
            AxisYTitle = "Вязкость продукта, Па*с";
            LineSeriesTitle = "Детерминированная модель. Вязкость продукта, Па*с";
        }

        YFormatter = value => value.ToString("N");
        RegressiveValues.Add(values[0].CriteriaValue);
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