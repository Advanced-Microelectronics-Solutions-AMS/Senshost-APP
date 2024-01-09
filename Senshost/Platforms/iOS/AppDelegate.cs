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

        try
        {

            Senshost.App.StatusBarHeight = (int)UIApplication.SharedApplication.StatusBarFrame.Height;

        }
        catch (Exception ex)
        {

        }

        return true;
    }

    //[Export("application:didRegisterUserNotificationSettings:")]
    //public void DidRegisterUserNotificationSettings(UIApplication application, UIUserNotificationSettings notificationSettings)
    //{

    //}

    //[Foundation.Export("application:didRegisterForRemoteNotificationsWithDeviceToken:")]
    //public virtual void RegisteredForRemoteNotifications(UIKit.UIApplication application, NSData deviceToken)
    //{
    //}

    //[Export("application:didFailToRegisterForRemoteNotificationsWithError:")]
    //public void FailedToRegisterForRemoteNotifications(UIKit.UIApplication application, NSError error)
    //{
    //    int x = 0;
    //}


}

