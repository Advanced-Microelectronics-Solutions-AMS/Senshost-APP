using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Senshost.Platforms;
using Sharpnado.Tabs;
using SkiaSharp.Views.Maui.Controls.Hosting;

namespace Senshost;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseSkiaSharp()
            .UseMauiCommunityToolkit()
            .UseSharpnadoTabs(loggerEnable: false)
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });


        builder.RegisterPlatformDependencies();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}

