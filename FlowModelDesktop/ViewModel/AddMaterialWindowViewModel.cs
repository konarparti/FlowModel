using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                    //TODO: Это изменение материала
                };
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
                return new RelayCommand(c =>
                {
                    //TODO: Проверка корректности ввода всех полей
                    if (_material != null)
                    {
                        //TODO: Это изменение материала
                    }
                    else
                    {
                        var newMaterial = new Material()
                        {
                            Type = MaterialType
                        };
                        _materialRepository.Save(newMaterial);
                        

                        var density = new ParameterValue()
                        {
                            IdMat = newMaterial.Id,
                            IdParam = _parameterRepository.GetByName("Плотность").Id,
                            Value = (double)MaterialParamValues.ro,
                            IdMatNavigation = newMaterial
                            
                        };
                        _parameterValueRepository.Save(density);

                        var heatCapacity = new ParameterValue()
                        {
                            IdMat = newMaterial.Id,
                            IdParam = _parameterRepository.GetByName("Удельная теплоёмкость").Id,
                            Value = (double)MaterialParamValues.c,
                            IdMatNavigation = newMaterial
                        };
                        _parameterValueRepository.Save(heatCapacity);

                        var tempMelting = new ParameterValue()
                        {
                            IdMat = newMaterial.Id,
                            IdParam = _parameterRepository.GetByName("Температура плавления").Id,
                            Value = (double)MaterialParamValues.To,
                            IdMatNavigation = newMaterial
                        };
                        _parameterValueRepository.Save(tempMelting);

                        var coefConsistency = new ParameterValue()
                        {
                            IdMat = newMaterial.Id,
                            IdParam = _parameterRepository.GetByName("Коэффициент консистенции при температуре приведения").Id,
                            Value = (double)MaterialParamValues.Mu,
                            IdMatNavigation = newMaterial
                        };
                        _parameterValueRepository.Save(coefConsistency);

                        var coefViscosity = new ParameterValue()
                        {
                            IdMat = newMaterial.Id,
                            IdParam = _parameterRepository.GetByName("Температурный коэффициент вязкости").Id,
                            Value = (double)MaterialParamValues.b,
                            IdMatNavigation = newMaterial
                        };
                        _parameterValueRepository.Save(coefViscosity);

                        var tempReference = new ParameterValue()
                        {
                            IdMat = newMaterial.Id,
                            IdParam = _parameterRepository.GetByName("Температура приведения").Id,
                            Value = (double)MaterialParamValues.Tr,
                            IdMatNavigation = newMaterial
                        };
                        _parameterValueRepository.Save(tempReference);

                        var flowIndex = new ParameterValue()
                        {
                            IdMat = newMaterial.Id,
                            IdParam = _parameterRepository.GetByName("Индекс течения материала").Id,
                            Value = (double)MaterialParamValues.n,
                            IdMatNavigation = newMaterial
                        };
                        _parameterValueRepository.Save(flowIndex);

                        var coefHeatTransfer = new ParameterValue()
                        {
                            IdMat = newMaterial.Id,
                            IdParam = _parameterRepository.GetByName("Коэффициент теплоотдачи к материалу").Id,
                            Value = (double)MaterialParamValues.alpha_u,
                            IdMatNavigation = newMaterial
                        };
                        _parameterValueRepository.Save(coefHeatTransfer);
                    }
                });
            }
        }
    }
}