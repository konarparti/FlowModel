using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlowModelDesktop.Models;
using FlowModelDesktop.Models.Data.Abstract;
using FlowModelDesktop.Services;

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

        #endregion


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
        }
    }
}
