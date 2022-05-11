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

public class AddParameterWindowViewModel : ViewModelBase
{
    #region Variables

    private readonly IRepository<Measure> _measureRepository;
    private readonly IRepository<Parameter> _parameterRepository;
    private readonly IRepository<TypeParameter> _typeParameterRepository;
    private readonly Parameter? _parameter;
    private readonly AdminWindowViewModel _viewModelBase;

    private string? _parameterName;
    private IEnumerable<TypeParameter> _allParameterTypes;
    private IEnumerable<Measure> _allMeasures;
    private TypeParameter? _selectedTypeParameter;
    private Measure? _selectedMeasure;

    #endregion

    #region Constructors

    public AddParameterWindowViewModel(IRepository<Parameter> parameterRepository, IRepository<TypeParameter> typeParameterRepository,
        IRepository<Measure> measureRepository, Parameter? parameter, AdminWindowViewModel viewModelBase)
    {
        _measureRepository = measureRepository;
        _parameterRepository = parameterRepository;
        _typeParameterRepository = typeParameterRepository;
        _parameter = parameter;
        _viewModelBase = viewModelBase;

        _allParameterTypes = _typeParameterRepository.GetAll();
        _allMeasures = _measureRepository.GetAll();

        if (parameter != null)
        {
            ParameterName = parameter.Name;
            SelectedTypeParameter = _typeParameterRepository.GetById(parameter.IdTypeParam);

            if (parameter.IdMeasure != null)
                SelectedMeasure = _measureRepository.GetById((long)parameter.IdMeasure);
        }
    }

    #endregion

    #region Properties

    public string? ParameterName
    {
        get => _parameterName;
        set
        {
            _parameterName = value;
            OnPropertyChanged();
        }
    }
    public IEnumerable<TypeParameter> AllParameterTypes
    {
        get => _allParameterTypes.DistinctBy(x => x.Name);
        set
        {
            _allParameterTypes = value;
            OnPropertyChanged();
        }
    }

    public IEnumerable<Measure> AllMeasures
    {
        get => _allMeasures.DistinctBy(x => x.Name);
        set
        {
            _allMeasures = value;
            OnPropertyChanged();
        }
    }

    public TypeParameter? SelectedTypeParameter
    {
        get => _selectedTypeParameter;
        set
        {
            _selectedTypeParameter = value;
            OnPropertyChanged();
        }
    }
    public Measure? SelectedMeasure
    {
        get => _selectedMeasure;
        set
        {
            _selectedMeasure = value;
            OnPropertyChanged();
        }
    }
    #endregion

    #region Commands

    public RelayCommand AddOrUpdateParameterCommand
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
                
                if (_parameter != null)
                {
                    _parameter.Name = ParameterName;
                    _parameter.IdTypeParam = SelectedTypeParameter.Id;
                    _parameter.IdMeasure = SelectedMeasure?.Id;

                    _parameterRepository.Save(_parameter);

                    MessageBox.Show("Информация о параметре успешно обновлена", "Информация", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                else
                {
                    var newParameter = new Parameter()
                    {
                        Name = ParameterName,
                        IdTypeParam = SelectedTypeParameter.Id,
                        IdMeasure = SelectedMeasure?.Id,
                    };

                    _parameterRepository.Save(newParameter);

                    MessageBox.Show("Параметр успешно добавлен", "Информация", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }

                _viewModelBase.ParameterUpdate();
                CloseAddParameterWindow();
            });
        }
    }

    #endregion

    #region Functions

    private void CheckValues(out string errors)
    {
        errors = string.Empty;
        if (string.IsNullOrWhiteSpace(ParameterName))
            errors += "Наименование параметра введен некорректно\n";
        if (SelectedTypeParameter == null)
            errors += "Вы не указали тип добавляемого параметра\n";
        

    }

    #endregion
}