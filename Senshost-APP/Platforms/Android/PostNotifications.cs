using Android;
using Android.OS;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace Senshost_APP.Platforms.Android
{
    public partial class PostNotifications : BasePlatformPermission
    {
        public override (string androidPermission, bool isRuntime)[] RequiredPermissions
        {
            get
            {
                var permissions = new List<(string, bool)>();

#if __ANDROID_33__
                if (OperatingSystem.IsAndroidVersionAtLeast(33) && Platform.AppContext.ApplicationInfo.TargetSdkVersion >= BuildVersionCodes.Tiramisu)
                {
                    // new runtime permissions on Android 12
                    if (IsDeclaredInManifest(Manifest.Permission.PostNotifications))
                        permissions.Add((Manifest.Permission.PostNotifications, true));
                }
#endif

                return permissions.ToArray();
            }
        }
    }
}
