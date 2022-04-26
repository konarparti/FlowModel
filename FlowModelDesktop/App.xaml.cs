using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Autofac;
using FlowModelDesktop.Models;
using FlowModelDesktop.Models.Data.Abstract;
using FlowModelDesktop.Models.Data.EntityFramework;
using FlowModelDesktop.ViewModel;

namespace FlowModelDesktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.DefaultThreadCurrentUICulture;
            var builder = new ContainerBuilder();
            builder.RegisterType<MainWindowViewModel>().AsSelf();
            builder.RegisterType<FlowModelDbContext>().AsSelf();
            builder.RegisterType<IdentityFlowModelContext>().AsSelf();
            builder.RegisterType<EFMaterialRepository>().As<IRepository<Material>>();
            builder.RegisterType<EFMeasureRepository>().As<IRepository<Measure>>();
            builder.RegisterType<EFParameterRepository>().As<IRepository<Parameter>>();
            builder.RegisterType<EFParameterValueRepository>().As<IRepository<ParameterValue>>();
            builder.RegisterType<EFTypeParameterRepository>().As<IRepository<TypeParameter>>();
            builder.RegisterType<EFUserRepository>().As<IUserRepository>();
            var container = builder.Build();
            var mainWindowViewModel = container.Resolve<MainWindowViewModel>();
            var mainWindow = new MainWindow { DataContext = mainWindowViewModel };
            mainWindow.Show();
        }
         
    }
}
