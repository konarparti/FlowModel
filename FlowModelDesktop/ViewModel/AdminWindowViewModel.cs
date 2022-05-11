using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using FlowModelDesktop.Models;
using FlowModelDesktop.Models.Data.Abstract;
using Microsoft.Win32;
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
        private Parameter? _selectedParameter;
        private ParameterValue? _selectedParameterValue;
        private User? _selectedUser;
        private Measure? _selectedMeasure;


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
        public Parameter? SelectedParameter
        {
            get => _selectedParameter;
            set
            {
                _selectedParameter = value;
                OnPropertyChanged();
            }
        }
        public ParameterValue? SelectedParameterValue
        {
            get => _selectedParameterValue;
            set
            {
                _selectedParameterValue = value;
                OnPropertyChanged();
            }
        }
        public User? SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
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
                    if (_selectedMaterial == null)
                    {
                        MessageBox.Show("Выберите материал, информацию о котором необходимо изменить", "Информация",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        var material = new AddMaterialWindowViewModel(_materialRepository,
                            _parameterRepository, _parameterValueRepository, SelectedMaterial, this);
                        ShowAddMaterialWindow(material, "Изменение материала");
                    }
                });
            }
        }

        public RelayCommand DeleteMaterialCommand
        {
            get
            {
                return new RelayCommand(command =>
                {
                    if (_selectedMaterial == null)
                    {
                        MessageBox.Show("Выберите материал, информацию о котором необходимо удалить", "Информация",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        if (MessageBox.Show(
                                $"Вы уверены что хотите удалить материал {_selectedMaterial.Type}?",
                                "Информация",
                                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            var paramValues = _parameterValueRepository.GetByMaterialId(_selectedMaterial.Id);
                            foreach (var item in paramValues)
                            {
                                _parameterValueRepository.Delete(_selectedMaterial.Id);
                            }
                            _materialRepository.Delete(_selectedMaterial.Id);
                            MaterialUpdated();
                        }
                    }
                });
            }
        }

        internal void MaterialUpdated()
        {
            AllMaterials = _materialRepository.GetAll();
            AllParameterValues = _parameterValueRepository.GetAll();
            _viewModelBase.UpdateMaterials();
        }

        #endregion

        #region ParametersTable

        public RelayCommand AddParameterCommand
        {
            get
            {
                return new RelayCommand(command =>
                {
                    var newParameter = new AddParameterWindowViewModel(_parameterRepository, _typeParameterRepository, _measureRepository,
                        null, this);
                    ShowAddParameterWindow(newParameter, "Добавление нового параметра");
                });
            }
        }
        public RelayCommand EditParameterCommand
        {
            get
            {
                return new RelayCommand(command =>
                {
                    if (_selectedParameter == null)
                    {
                        MessageBox.Show("Выберите параметр, информацию о котором необходимо изменить", "Информация",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        var parameter = new AddParameterWindowViewModel(_parameterRepository, _typeParameterRepository, _measureRepository,
                            _selectedParameter, this);
                        ShowAddParameterWindow(parameter, "Изменение параметра");
                    }
                });
            }
        }

        public RelayCommand DeleteParameterCommand
        {
            get
            {
                return new RelayCommand(command =>
                {
                    if (_selectedParameter == null)
                    {
                        MessageBox.Show("Выберите параметр, информацию о котором необходимо удалить", "Информация",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        if (MessageBox.Show(
                                $"Вы уверены что хотите удалить параметр {_selectedParameter.Name}?",
                                "Информация",
                                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            if (_parameterValueRepository.GetByParameterId(_selectedParameter.Id).Count() != 0)
                            {
                                MessageBox.Show("Выбранный параметр удалить невозможно, так как в таблице значений параметров он имеет неудаленные значения", "Ошибка",
                                    MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                            _parameterRepository.Delete(_selectedParameter.Id);
                            ParameterUpdate();
                        }
                    }
                });
            }
        }

        internal void ParameterUpdate()
        {
            AllParameters = _parameterRepository.GetAll();
        }
        #endregion

        #region ParameterValuesTable

        public RelayCommand AddParameterValueCommand
        {
            get
            {
                return new RelayCommand(command =>
                {
                    var newParamValue = new AddParameterValueWindowViewModel(_materialRepository, _parameterRepository,
                        _parameterValueRepository, _measureRepository, null, this);
                    ShowAddParameterValueWindow(newParamValue, "Добавление значения параметра");
                });
            }
        }

        public RelayCommand EditParameterValueCommand
        {
            get
            {
                return new RelayCommand(command =>
                {
                    if (_selectedParameterValue == null)
                    {
                        MessageBox.Show("Выберите значение параметра, информацию о котором необходимо изменить", "Информация",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        var paramValue = new AddParameterValueWindowViewModel(_materialRepository, _parameterRepository,
                            _parameterValueRepository, _measureRepository, _selectedParameterValue, this);
                        ShowAddParameterValueWindow(paramValue, "Изменение значения параметра");
                    }
                });
            }
        }

        public RelayCommand DeleteParameterValueCommand
        {
            get
            {
                return new RelayCommand(command =>
                {
                    if (_selectedParameterValue == null)
                    {
                        MessageBox.Show("Выберите значение параметра, информацию о котором необходимо удалить", "Информация",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        var delParam = _parameterRepository.GetById(_selectedParameterValue.IdParam).Name;
                        var delMat = _materialRepository.GetById(_selectedParameterValue.IdMat).Type;
                        if (MessageBox.Show(
                                $"Вы уверены что хотите удалить значения параметра {delParam} для материала {delMat}?",
                                "Информация",
                                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            _parameterValueRepository.Delete(_selectedParameterValue.IdMat, _selectedParameterValue.IdParam);
                            ParameterValueUpdate();
                        }
                    }
                });
            }
        }

        public void ParameterValueUpdate()
        {
            AllParameterValues = _parameterValueRepository.GetAll();
        }

        #endregion

        #region MeasureTable

        public RelayCommand AddMeasureCommand
        {
            get
            {
                return new RelayCommand(command =>
                {
                    var newMeasure = new AddMeasureWindowViewModel(_measureRepository, null, this);
                    ShowAddMeasureWindow(newMeasure, "Добавление новой единицы измерения");
                });
            }
        }

        public RelayCommand EditMeasureCommand
        {
            get
            {
                return new RelayCommand(command =>
                {
                    if (_selectedMeasure == null)
                    {
                        MessageBox.Show("Выберите единицу измерения, информацию о которой необходимо изменить", "Информация",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {

                        var measure = new AddMeasureWindowViewModel(_measureRepository, _selectedMeasure, this);
                        ShowAddMeasureWindow(measure, "Изменение единицы измерения");

                    }
                });
            }
        }

        public void MeasureUpdate()
        {
            AllMeasures = _measureRepository.GetAll();
        }

        #endregion

        #region UserTable

        public RelayCommand AddUserCommand
        {
            get
            {
                return new RelayCommand(command =>
                {
                    var newUser = new AddUserWindowViewModel(_userRepository, null, this);
                    ShowAddUserWindow(newUser, "Добавление нового пользователя");
                });
            }
        }
        public RelayCommand EditUserCommand
        {
            get
            {
                return new RelayCommand(command =>
                {
                    if (_selectedUser == null)
                    {
                        MessageBox.Show("Выберите пользователя, информацию о котором необходимо изменить", "Информация",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        var user = new AddUserWindowViewModel(_userRepository, _selectedUser, this);
                        ShowAddUserWindow(user, "Изменение данных пользователя");
                    }
                });
            }
        }

        public RelayCommand DeleteUserCommand
        {
            get
            {
                return new RelayCommand(command =>
                {
                    if (_selectedUser == null)
                    {
                        MessageBox.Show("Выберите пользователя, информацию о котором необходимо удалить", "Информация",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        if (MessageBox.Show(
                                $"Вы уверены что хотите удалить данные о пользователе {_selectedUser.Username}?",
                                "Информация",
                                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            if (_userRepository.GetAllUsers().Count() == 1)
                            {
                                MessageBox.Show("Вы не можете удалить единственную запись в таблице пользователей", "Ошибка",
                                    MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                            _userRepository.DeleteUser(_selectedUser.Id);
                            UserUpdate();
                        }
                    }
                });
            }
        }
        internal void UserUpdate()
        {
            AllUsers = _userRepository.GetAllUsers();
        }

        #endregion

        #region Backups

        public RelayCommand BackupBaseCommand
        {
            get
            {
                return new RelayCommand(command =>
                {
                    
                });
            }
        }

        public RelayCommand RestoreBaseCommand
        {
            get
            {
                return new RelayCommand(command =>
                {
                    
                });
            }
        }

        #endregion

        #endregion


    }
}
