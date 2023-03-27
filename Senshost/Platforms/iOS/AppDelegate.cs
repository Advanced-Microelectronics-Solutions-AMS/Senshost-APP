using Foundation;
using UIKit;

namespace Senshost;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

    [Export("application:didFinishLaunchingWithOptions:")]
    public override bool FinishedLaunching(UIKit.UIApplication application, NSDictionary launchOptions)
    {
        base.FinishedLaunching(application, launchOptions);

        Senshost.App.StatusBarHeight = (int)UIApplication.SharedApplication.StatusBarFrame.Height;


        return true;
    }
}

