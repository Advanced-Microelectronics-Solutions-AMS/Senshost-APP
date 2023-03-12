using Senshost.Common.Interfaces;
using UIKit;

namespace Senshost.Platforms.iOS.Services
{
    public class GetDeviceInfo : IGetDeviceInfo
    {
        public string GetDeviceID()
        {
            return UIDevice.CurrentDevice.IdentifierForVendor.ToString(); ;
        }
    }
}
