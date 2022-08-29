using Caliburn.Micro;
using Ninject;
using Ninject.Modules;
using Software.IP.Trace.Common.Helpers;
using Software.IP.Trace.IoC;
using Software.IP.Trace.Views.Handler;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace Software.IP.Trace;

public partial class App : Application { }

public class AppBootstrapper : BootstrapperBase {
    public readonly IKernel _kernel = new StandardKernel();

    public AppBootstrapper() => Initialize();

    protected override async void OnStartup(object sender, StartupEventArgs e) {
        await this.DisplayRootViewForAsync<HandlerViewModel>();
    }

    protected override void OnExit(object sender, EventArgs e) {
        base.OnExit(sender, e);
        _kernel.Dispose();
    }

    protected override void Configure() {
        // IoC
        _kernel.Load<AppBindingModule>();
        _kernel.LoadIpTraceIoC();
    }

    protected override object GetInstance(Type serviceType, string serviceName) {
        if (serviceType == null)
            throw new NullReferenceException(nameof(serviceType));

        try {
            return string.IsNullOrWhiteSpace(serviceName)
                ? _kernel.Get(serviceType)
                : _kernel.Get(serviceType, serviceName);
        }
        catch (Exception ex) {
            Debugger.Break();
            Debug.WriteLine(ex, "DEBUG");
            throw;
        }
    }

    protected override IEnumerable<Assembly> SelectAssemblies() {
        var assemblies = base.SelectAssemblies().ToList();
        // TODO: here paste ViewModels from other assemblies
        // assemblies.Add(typeof(ExampleViewModel).Assembly);
        return assemblies;
    }
}

public class AppBindingModule : NinjectModule {
    public override void Load() {
        Bind<IEventAggregator>().To<EventAggregator>().InSingletonScope();
        Bind<IWindowManager>().To<CustomWindowsManager>().InSingletonScope();
    }
}