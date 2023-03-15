using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Platform;
using Senshost.Common.Interfaces;
using Senshost.Handlers;
using Senshost.Models.Account;
using Senshost.ViewModels;

namespace Senshost;

public partial class App : Application
{
    public static LogedInUserDetails UserDetails;
    public static string ApiToken;
    public static bool IsNotificationReceived;

    //public App()
    //{
    //    MainPage = new ContentPage();

    //}

    public App(IServiceProvider serviceProvider)
    {
        InitializeComponent();

        Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(nameof(BorderlessEntry), (handler, view) =>
        {
            if (view is BorderlessEntry)
            {
#if __ANDROID__
                handler.PlatformView.SetBackgroundColor(Colors.Transparent.ToPlatform());
                handler.PlatformView.TextCursorDrawable.SetTint(Colors.White.ToPlatform());
                handler.PlatformView.SetPadding(40, 0, 40, 0);
#elif __IOS__
                handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#endif
            }
        });

        //MainPage = new ContentPage();
        MainPage = new AppShell(serviceProvider.GetService<AppShellViewModel>());
    }
}
