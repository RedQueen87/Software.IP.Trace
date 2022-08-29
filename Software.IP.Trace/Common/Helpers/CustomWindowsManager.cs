using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Software.IP.Trace.Common.Helpers;

public class CustomWindowsManager : WindowManager {
    public const string ClientName = "IP Trace";

    static CustomWindowsManager() {
        // properties
        Title = ClientName;
    }

    public static string Title { get; }

    protected override Task<Window> CreateWindowAsync(object rootModel, bool isDialog, object context, IDictionary<string, object> settings) =>
        base.CreateWindowAsync(rootModel, isDialog, context, settings)
            .ToObservable()
            .Do(window => {
                window.Title = CustomWindowsManager.Title;
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            })
            .ToTask();

    protected override Window EnsureWindow(object model, object view, bool isDialog) {
        var window = base.EnsureWindow(model, view, isDialog);

        // settings for all windows
        var assemblyName = typeof(CustomWindowsManager).Assembly.GetName().Name;
        window.Icon = new BitmapImage(new Uri($"pack://application:,,,/{assemblyName};component/Images/favicon.ico"));
        window.MinWidth = 640;
        window.MinHeight = 480;
        window.SizeToContent = SizeToContent.Manual;
        return window;
    }
}