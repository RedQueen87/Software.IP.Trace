﻿using Ninject.Modules;
using Software.IP.Trace.Services.Ips;
using Software.IP.Trace.Services.IpStack;

namespace Software.IP.Trace.IoC;

public class ServicesBindingModule : NinjectModule {
    public override void Load() {
        Bind<HttpClient>().ToSelf().InSingletonScope();
        Bind<IIpStackService>().To<IpStackService>();
        Bind<IIpService>().To<IpService>();
    }
}
