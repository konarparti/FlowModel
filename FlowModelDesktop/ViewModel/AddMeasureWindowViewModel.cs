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

public class AddMeasureWindowViewModel : ViewModelBase
{

    #region Variables

    private readonly IRepository<Measure> _measureRepository;
    private readonly Measure? _measure;
    private readonly AdminWindowViewModel _viewModelBase;

    private string? _name;

    #endregion

    #region Constructors

    public AddMeasureWindowViewModel(IRepository<Measure> measureRepository, Measure? measure, AdminWindowViewModel viewModelBase)
    {
        _measureRepository = measureRepository;
        _measure = measure;
        _viewModelBase = viewModelBase;

        if(measure != null)
            Name = measure.Name;
    }

    #endregion

    #region Properties
    public string? Name
    {
        get => _name;
        set
        {
            _name = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Commands

    public RelayCommand AddOrUpdateMeasureCommand
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

                if (_measure != null)
                {
                    _measure.Name = Name;

                    _measureRepository.Save(_measure);

                    MessageBox.Show("Информация о единице измерения успешно обновлена", "Информация", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                else
                {
                    var newMeasure = new Measure
                    {
                        Name = Name,
                    };

                    _measureRepository.Save(newMeasure);

                    MessageBox.Show("Единица измерения успешно добавлена", "Информация", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }

                _viewModelBase.MeasureUpdate();
                CloseAddMeasureWindow();
            });
        }
    }

    #endregion

    #region Functions

    private void CheckValues(out string errors)
    {
        errors = string.Empty;
        if (string.IsNullOrWhiteSpace(Name))
            errors += "Сокращенное наименование единицы измерения введено некорректно\n";
    }

    #endregion
}