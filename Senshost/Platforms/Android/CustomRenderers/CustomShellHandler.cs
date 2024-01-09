using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform.Compatibility;

namespace Senshost.Platforms.Android.CustomRenderers
{
    class CustomShellHandler : ShellRenderer
    {
        //protected override IShellBottomNavViewAppearanceTracker CreateBottomNavViewAppearanceTracker(ShellItem shellItem)
        //{
        //    return new CustomShellBottomNavViewAppearanceTracker(this, shellItem.CurrentItem);
        //}

        protected override IShellToolbarAppearanceTracker CreateToolbarAppearanceTracker()
        {
            return new CustomShellToolbarAppearanceTracker(this);
        }

        protected override IShellItemRenderer CreateShellItemRenderer(ShellItem shellItem)
        {
            return new BadgeShellItemRenderer(this);
        }
    }
}
