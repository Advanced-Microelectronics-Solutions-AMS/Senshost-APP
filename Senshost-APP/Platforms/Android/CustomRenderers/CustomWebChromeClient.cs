using Android.App;
using Android.Webkit;

namespace Senshost_APP.Platforms.Android.CustomRenderers
{
    internal class CustomWebChromeClient : WebChromeClient
    {
        public override void OnPermissionRequest(PermissionRequest request)
        {

            foreach (var resource in request.GetResources())
            {
                if (resource.Equals(PermissionRequest.ResourceVideoCapture, StringComparison.OrdinalIgnoreCase))
                {
                    AppShell.Current.Dispatcher.Dispatch(() => { request.Grant(request.GetResources()); }); ;
                    return;
                }
            }

            base.OnPermissionRequest(request);
        }
    }
}
