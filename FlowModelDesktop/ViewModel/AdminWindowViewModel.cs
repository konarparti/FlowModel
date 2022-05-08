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
    public class AdminWindowViewModel : ViewModelBase
    {
        #region Variables

        private readonly IRepository<Material> _materialRepository;
        private readonly IRepository<Measure> _measureRepository;
        private readonly IRepository<Parameter> _parameterRepository;
        private readonly IParameterValueRepository _parameterValueRepository;
        private readonly IRepository<TypeParameter> _typeParameterRepository;
        private readonly IUserRepository _userRepository;
        private readonly MainWindowViewModel _viewModelBase;
        
        private IEnumerable<Material> _allMaterials;
        private IEnumerable<Parameter> _allParameters;
        private IEnumerable<ParameterValue> _allParameterValues;
        private IEnumerable<TypeParameter> _allTypeParameters;
        private IEnumerable<Measure> _allMeasures;
        private IEnumerable<User> _allUsers;
        private Material? _selectedMaterial;

        #endregion

        #region Constructors

        public AdminWindowViewModel(IRepository<Material> materialRepository, IRepository<Measure> measureRepository,
            IRepository<Parameter> parameterRepository, IParameterValueRepository parameterValueRepository, IRepository<TypeParameter> typeParameterRepository,
            IUserRepository userRepository, MainWindowViewModel viewModelBase)
        {
            _materialRepository = materialRepository;
            _measureRepository = measureRepository;
            _parameterRepository = parameterRepository;
            _parameterValueRepository = parameterValueRepository;
            _typeParameterRepository = typeParameterRepository;
            _userRepository = userRepository;
            _viewModelBase = viewModelBase;
            _allMaterials = _materialRepository.GetAll();
            _allParameters = _parameterRepository.GetAll();
            _allParameterValues = _parameterValueRepository.GetAll();
            _allTypeParameters = _typeParameterRepository.GetAll();
            _allMeasures = _measureRepository.GetAll();
            _allUsers = _userRepository.GetAllUsers();
        }

        #endregion


        #region Properties

        public IEnumerable<Material> AllMaterials
        {
            get => _allMaterials;
            set
            {
                _allMaterials = value;
                OnPropertyChanged();
            }
        }
        public IEnumerable<Parameter> AllParameters
        {
            get => _allParameters;
            set
            {
                _allParameters = value;
                OnPropertyChanged();
            }
        }
        public IEnumerable<ParameterValue> AllParameterValues
        {
            get => _allParameterValues;
            set
            {
                _allParameterValues = value;
                OnPropertyChanged();
            }
        }
        public IEnumerable<TypeParameter> AllTypeParameters
        {
            get => _allTypeParameters;
            set
            {
                _allTypeParameters = value;
                OnPropertyChanged();
            }
        }
        public IEnumerable<Measure> AllMeasures
        {
            get => _allMeasures;
            set
            {
                _allMeasures = value;
                OnPropertyChanged();
            }
        }
        public IEnumerable<User> AllUsers
        {
            get => _allUsers;
            set
            {
                _allUsers = value;
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

        #endregion

        #region Commands

        #region MaterialsTable

        public RelayCommand AddMaterialCommand
        {
            get
            {
                return new RelayCommand(c =>
                {
                    var newMaterial = new AddMaterialWindowViewModel(_materialRepository,
                        _parameterRepository, _parameterValueRepository, null, this);
                    ShowAddMaterialWindow(newMaterial, "Добавление нового материала");
                });
            }
        }
        public RelayCommand EditMaterialCommand
        {
            get
            {
                return new RelayCommand(c =>
                {
                    var material = new AddMaterialWindowViewModel(_materialRepository,
                        _parameterRepository, _parameterValueRepository, SelectedMaterial, this);
                    ShowAddMaterialWindow(material, "Изменение материала");
                });
            }
        }

        #endregion


        #endregion
    }
}
