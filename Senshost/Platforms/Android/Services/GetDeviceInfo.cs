using Senshost.Common.Interfaces;
using static Android.Provider.Settings;
using AndroidApp = Android.App;

namespace Senshost.Platforms.Android.Services
{
    public class GetDeviceInfo : IGetDeviceInfo
    {
        public string GetDeviceID()
        {
            var context = AndroidApp.Application.Context;

            string id = Secure.GetString(context.ContentResolver, Secure.AndroidId);

            return id;
        }
    }
}
