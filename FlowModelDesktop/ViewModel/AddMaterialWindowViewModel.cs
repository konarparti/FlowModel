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


namespace FlowModelDesktop.ViewModel
{
    public class AddMaterialWindowViewModel : ViewModelBase
    {
        private readonly IRepository<Material> _materialRepository;
        private readonly IRepository<Parameter> _parameterRepository;
        private readonly IParameterValueRepository _parameterValueRepository;
        private readonly Material? _material;
        private readonly AdminWindowViewModel _viewModelBase;
        private DbData _materialParamValues = new DbData();
        private string _materialType;

        public AddMaterialWindowViewModel(IRepository<Material> materialRepository, IRepository<Parameter> parameterRepository, IParameterValueRepository parameterValueRepository, Material? material, AdminWindowViewModel viewModelBase)
        {
            _materialRepository = materialRepository;
            _parameterRepository = parameterRepository;
            _parameterValueRepository = parameterValueRepository;
            _material = material;
            _viewModelBase = viewModelBase;

            if (material != null)
            {
                _materialType = material.Type;

                var temp = new DbData()
                {
                    ro = (decimal)_parameterValueRepository.GetByBothId(_parameterRepository.GetByName("Плотность").Id, material.Id).Value,
                    c = (decimal)_parameterValueRepository.GetByBothId(_parameterRepository.GetByName("Удельная теплоёмкость").Id, material.Id).Value,
                    To = (decimal)_parameterValueRepository.GetByBothId(_parameterRepository.GetByName("Температура плавления").Id, material.Id).Value,
                    Mu = (decimal)_parameterValueRepository.GetByBothId(_parameterRepository.GetByName("Коэффициент консистенции при температуре приведения").Id, material.Id).Value,
                    b = (decimal)_parameterValueRepository.GetByBothId(_parameterRepository.GetByName("Температурный коэффициент вязкости").Id, material.Id).Value,
                    Tr = (decimal)_parameterValueRepository.GetByBothId(_parameterRepository.GetByName("Температура приведения").Id, material.Id).Value,
                    n = (decimal)_parameterValueRepository.GetByBothId(_parameterRepository.GetByName("Индекс течения материала").Id, material.Id).Value,
                    alpha_u = (decimal)_parameterValueRepository.GetByBothId(_parameterRepository.GetByName("Коэффициент теплоотдачи к материалу").Id, material.Id).Value,
                    
                };
                MaterialParamValues = temp;
            }
        }

        public DbData MaterialParamValues
        {
            get => _materialParamValues;
            set
            {
                _materialParamValues = value;
                OnPropertyChanged();
            }
        }
        public string MaterialType
        {
            get => _materialType;
            set
            {
                _materialType = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand AddOrUpdateMaterialCommand
        {
            get
            {
                return new RelayCommand(com =>
                {
                    //TODO: Проверка корректности ввода всех полей

                    if (_material != null)
                    {
                        _material.Type = MaterialType;
                        var ro = new ParameterValue()
                        {
                            IdMat = _material.Id,
                            IdParam = _parameterRepository.GetByName("Плотность").Id,
                            Value = (double)MaterialParamValues.ro
                        };
                        _parameterValueRepository.Save(ro);

                        var c = new ParameterValue()
                        {
                            IdMat = _material.Id,
                            IdParam = _parameterRepository.GetByName("Удельная теплоёмкость").Id,
                            Value = (double)MaterialParamValues.c
                        };
                        _parameterValueRepository.Save(c);

                        var t0 = new ParameterValue()
                        {
                            IdMat = _material.Id,
                            IdParam = _parameterRepository.GetByName("Температура плавления").Id,
                            Value = (double)MaterialParamValues.To
                        };
                        _parameterValueRepository.Save(t0);

                        var mu = new ParameterValue()
                        {
                            IdMat = _material.Id,
                            IdParam = _parameterRepository.GetByName("Коэффициент консистенции при температуре приведения").Id,
                            Value = (double)MaterialParamValues.Mu
                        };
                        _parameterValueRepository.Save(mu);

                        var b = new ParameterValue()
                        {
                            IdMat = _material.Id,
                            IdParam = _parameterRepository.GetByName("Температурный коэффициент вязкости").Id,
                            Value = (double)MaterialParamValues.b
                        };
                        _parameterValueRepository.Save(b);

                        var tr = new ParameterValue()
                        {
                            IdMat = _material.Id,
                            IdParam = _parameterRepository.GetByName("Температура приведения").Id,
                            Value = (double)MaterialParamValues.Tr
                        };
                        _parameterValueRepository.Save(tr);

                        var n = new ParameterValue()
                        {
                            IdMat = _material.Id,
                            IdParam = _parameterRepository.GetByName("Индекс течения материала").Id,
                            Value = (double)MaterialParamValues.n
                        };
                        _parameterValueRepository.Save(n);

                        var alpha_u = new ParameterValue()
                        {
                            IdMat = _material.Id,
                            IdParam = _parameterRepository.GetByName("Коэффициент теплоотдачи к материалу").Id,
                            Value = (double)MaterialParamValues.alpha_u
                        };
                        _parameterValueRepository.Save(alpha_u);

                        MessageBox.Show("Информация о материале успешно обновлена", "Информация", MessageBoxButton.OK,
                            MessageBoxImage.Information);
                    }
                    else
                    {
                        var newMaterial = new Material()
                        {
                            Type = MaterialType,
                            ParameterValues = new List<ParameterValue>
                            {
                                new ParameterValue()
                                {
                                    IdParam = _parameterRepository.GetByName("Плотность").Id,
                                    Value = (double)MaterialParamValues.ro
                                },
                                new ParameterValue()
                                {
                                    IdParam = _parameterRepository.GetByName("Удельная теплоёмкость").Id,
                                    Value = (double)MaterialParamValues.c
                                },
                                new ParameterValue()
                                {
                                    IdParam = _parameterRepository.GetByName("Температура плавления").Id,
                                    Value = (double)MaterialParamValues.To
                                },
                                new ParameterValue()
                                {
                                    IdParam = _parameterRepository.GetByName("Коэффициент консистенции при температуре приведения").Id,
                                    Value = (double)MaterialParamValues.Mu
                                },
                                new ParameterValue()
                                {
                                    IdParam = _parameterRepository.GetByName("Температурный коэффициент вязкости").Id,
                                    Value = (double)MaterialParamValues.b
                                },
                                new ParameterValue()
                                {
                                IdParam = _parameterRepository.GetByName("Температура приведения").Id,
                                Value = (double)MaterialParamValues.Tr
                                },
                                new ParameterValue()
                                {
                                    IdParam = _parameterRepository.GetByName("Индекс течения материала").Id,
                                    Value = (double)MaterialParamValues.n
                                },
                                new ParameterValue()
                                {
                                    IdParam = _parameterRepository.GetByName("Коэффициент теплоотдачи к материалу").Id,
                                    Value = (double)MaterialParamValues.alpha_u
                                }
                            }
                        };
                        _materialRepository.Save(newMaterial);

                        MessageBox.Show("Материал успешно добавлен", "Информация", MessageBoxButton.OK,
                            MessageBoxImage.Information);

                        _viewModelBase.MaterialUpdated();
                        CloseAddMaterialWindow();
                    }
                });
            }
        }
    }
}