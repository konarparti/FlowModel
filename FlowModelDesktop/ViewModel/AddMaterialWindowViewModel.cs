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
    public class AddMaterialWindowViewModel : ViewModelBase 
    {
        private readonly IRepository<Material> _materialRepository;
        private readonly IRepository<Parameter> _parameterRepository;
        private readonly IParameterValueRepository _parameterValueRepository;
        private readonly Material? _material;
        private readonly AdminWindowViewModel _viewModelBase;

        public AddMaterialWindowViewModel(IRepository<Material> materialRepository, IRepository<Parameter> parameterRepository, IParameterValueRepository parameterValueRepository, Material? material, AdminWindowViewModel viewModelBase)
        {
            _materialRepository = materialRepository;
            _parameterRepository = parameterRepository;
            _parameterValueRepository = parameterValueRepository;
            _material = material;
            _viewModelBase = viewModelBase;
        }
    }
}
