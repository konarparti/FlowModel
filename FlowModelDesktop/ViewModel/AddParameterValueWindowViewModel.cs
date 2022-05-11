using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using FlowModelDesktop.Models;
using FlowModelDesktop.Models.Data.Abstract;
using WPF_MVVM_Classes;
using ViewModelBase = FlowModelDesktop.Services.ViewModelBase;

namespace FlowModelDesktop.ViewModel;

public class AddParameterValueWindowViewModel : ViewModelBase
{
    #region Variables

    private readonly IRepository<Material> _materialRepository;
    private readonly IRepository<Parameter> _parameterRepository;
    private readonly IParameterValueRepository _parameterValueRepository;
    private readonly IRepository<Measure> _measureRepository;
    private readonly ParameterValue? _parameterValue;
    private readonly AdminWindowViewModel _viewModelBase;

    private IEnumerable<Material> _allMaterials;
    private IEnumerable<Parameter> _allParameters;
    private Material? _selectedMaterial;
    private Parameter? _selectedParameter;
    private decimal _value;
    private Measure? _measureUnit;

    #endregion

    #region Constructors

    public AddParameterValueWindowViewModel(IRepository<Material> materialRepository, IRepository<Parameter> parameterRepository,
        IParameterValueRepository parameterValueRepository, IRepository<Measure> measureRepository, ParameterValue? parameterValue, AdminWindowViewModel viewModelBase)
    {
        _materialRepository = materialRepository;
        _parameterRepository = parameterRepository;
        _parameterValueRepository = parameterValueRepository;
        _measureRepository = measureRepository;
        _parameterValue = parameterValue;
        _viewModelBase = viewModelBase;

        AllMaterials = _materialRepository.GetAll();
        AllParameters = _parameterRepository.GetAll();

        if (parameterValue != null)
        {
            SelectedMaterial = _materialRepository.GetById(parameterValue.IdMat);
            SelectedParameter = _parameterRepository.GetById(parameterValue.IdParam);
            Value = (decimal)parameterValue.Value;
        }
    }

    #endregion

    #region Properties


    public IEnumerable<Material> AllMaterials
    {
        get => _allMaterials.DistinctBy(x => x.Type);
        set
        {
            _allMaterials = value;
            OnPropertyChanged();
        }
    }
    public IEnumerable<Parameter> AllParameters
    {
        get => _allParameters.DistinctBy(x => x.Name);
        set
        {
            _allParameters = value;
            OnPropertyChanged();
        }
    }
    public Material? SelectedMaterial
    {
        get => _selectedMaterial;
        set
        {
            _selectedMaterial = value;
            OnPropertyChanged();
        }
    }
    public Parameter? SelectedParameter
    {
        get => _selectedParameter;
        set
        {
            _selectedParameter = value;
            ParameterChanged();
            OnPropertyChanged();
        }
    }
    public decimal Value
    {
        get => _value;
        set
        {
            _value = value;
            OnPropertyChanged();
        }
    }
    public Measure? MeasureUnit
    {
        get => _measureUnit;
        set
        {
            _measureUnit = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Commands

    public RelayCommand AddOrUpdateParameterValueCommand
    {
        get
        {
            return new RelayCommand(command =>
            {
                CheckValues(out var errors);
                if (errors != string.Empty)
                {
                    MessageBox.Show(errors, "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (_parameterValue != null)
                {
                    _parameterValue.IdMat = SelectedMaterial.Id;
                    _parameterValue.IdParam = SelectedParameter.Id;
                    _parameterValue.Value = (double)Value;
                    
                    _parameterValueRepository.Save(_parameterValue);
                    MessageBox.Show("Значение параметра успешно обновлено", "Информация", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                else
                {
                    var newParamValue = new ParameterValue()
                    {
                        IdMat = SelectedMaterial.Id,
                        IdParam = SelectedParameter.Id,
                        Value = (double)Value
                    };

                    _parameterValueRepository.Save(newParamValue);

                    MessageBox.Show("Значение параметра успешно добавлено", "Информация", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }

                _viewModelBase.ParameterValueUpdate();
                CloseAddParameterValueWindow();
            });
        }
    }

   

    #endregion

    #region Functions

    private void ParameterChanged()
    {
        MeasureUnit = SelectedParameter.IdMeasure != null ? _measureRepository.GetById((long)SelectedParameter.IdMeasure) : null;
    }

    private void CheckValues(out string errors)
    {
        errors = string.Empty;
        if (SelectedMaterial == null)
            errors += "Вы не указали тип материала\n";
        if (SelectedParameter == null)
            errors += "Вы не указали наименование параметра\n";
        if (Value <= 0)
            errors += "Значение параметра не может быть меньше нуля или равно нулю\n";
    }
    #endregion
}