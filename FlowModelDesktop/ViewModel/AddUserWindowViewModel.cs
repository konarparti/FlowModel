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
    public class AddUserWindowViewModel : ViewModelBase
    {
        #region Variables

        private readonly IUserRepository _userRepository;
        private readonly User? _user;
        private readonly AdminWindowViewModel _viewModelBase;
        private IEnumerable<User> _allUsers;
        private User? _selectedUser;
        private string? _username;
        private string? _password;

        #endregion

        #region Constructors

        public AddUserWindowViewModel(IUserRepository userRepository, User? user, AdminWindowViewModel viewModelBase)
        {
            _userRepository = userRepository;
            _user = user;
            _viewModelBase = viewModelBase;

            _allUsers = _userRepository.GetAllUsers();

            if (user != null)
            {
                Username = user.Username;
                Password = user.Password;
                SelectedUser = _userRepository.GetById(user.Id);
            }
        }

        #endregion

        #region Properties
        public string? Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }
        public string? Password
        {
            get => _password;
            set
            {
                _password = value;
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
        public User? SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        public RelayCommand AddOrUpdateUserCommand
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

                    if (_user != null)
                    {
                        _user.Username = Username;
                        _user.Password = Password;
                        _user.Role = SelectedUser?.Role;

                        _userRepository.SaveUser(_user);

                        MessageBox.Show("Информация о пользователе успешно обновлена", "Информация", MessageBoxButton.OK,
                            MessageBoxImage.Information);
                    }
                    else
                    {
                        var newUser = new User()
                        {
                            Username = Username,
                            Password = Password,
                            Role = SelectedUser?.Role,
                        };

                        _userRepository.SaveUser(newUser);

                        MessageBox.Show("Пользователь успешно добавлен", "Информация", MessageBoxButton.OK,
                            MessageBoxImage.Information);
                    }

                    _viewModelBase.UserUpdate();
                    CloseAddUserWindow();
                });
            }
        }

        #endregion

        #region Functions
        private void CheckValues(out string errors)
        {
            errors = string.Empty;
            if (string.IsNullOrWhiteSpace(Username))
                errors += "Имя пользователя введено некорректно\n";
            if (string.IsNullOrWhiteSpace(Password))
                errors += "Пароль введен некорректно\n";
            if (SelectedUser == null)
                errors += "Вы не указали роль добавляемого пользователя\n";
        }

        #endregion

    }
}
