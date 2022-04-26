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
    public class AuthorizationWindowViewModel : ViewModelBase
    {
        #region Variables

        private readonly IRepository<Material> _materialRepository;
        private readonly IRepository<Measure> _measureRepository;
        private readonly IRepository<Parameter> _parameterRepository;
        private readonly IRepository<ParameterValue> _parameterValueRepository;
        private readonly IRepository<TypeParameter> _typeParameterRepository;
        private readonly IUserRepository _userRepository;
        private readonly MainWindowViewModel _viewModelBase;
        private RelayCommand? _authorizeCommand;
        private string _username;
        private string _password;

        #endregion

        #region Constructors
        public AuthorizationWindowViewModel(IRepository<Material> materialRepository, IRepository<Measure> measureRepository, 
            IRepository<Parameter> parameterRepository, IRepository<ParameterValue> parameterValueRepository, IRepository<TypeParameter> typeParameterRepository,
            IUserRepository userRepository, MainWindowViewModel viewModelBase)
        {
            _materialRepository = materialRepository;
            _measureRepository = measureRepository;
            _parameterRepository = parameterRepository;
            _parameterValueRepository = parameterValueRepository;
            _typeParameterRepository = typeParameterRepository;
            _userRepository = userRepository;
            _viewModelBase = viewModelBase;
        }

        #endregion

        #region Properties

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        public RelayCommand AuthorizeCommand
        {
            get
            {
                return _authorizeCommand ??= new RelayCommand(x =>
                {
                    if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
                    {
                        MessageBox.Show("Вы не указали логин или пароль", "Ошибка", MessageBoxButton.OK,
                            MessageBoxImage.Warning);
                        return;
                    }
                    if (_userRepository.VerifyUser(Username, Password))
                    {
                        var adminPanel = new AdminWindowViewModel(_materialRepository, _measureRepository, _parameterRepository, 
                            _parameterValueRepository, _typeParameterRepository, _userRepository, _viewModelBase);
                        ShowAdmin(adminPanel);
                        CloseAuthorizationWindow();
                    }
                    else
                    {
                        MessageBox.Show("Неверный логин или пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                });
            }
        }

        #endregion
    }
}
