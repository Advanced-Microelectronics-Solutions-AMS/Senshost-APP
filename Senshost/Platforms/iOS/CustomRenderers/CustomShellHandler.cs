using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform.Compatibility;

namespace Senshost.Platforms.iOS.CustomRenderers
{
    class CustomShellHandler : ShellRenderer
    {
        protected override IShellTabBarAppearanceTracker CreateTabBarAppearanceTracker()
        {
            return new CustomShellTabBarAppearanceTracker();
        }

        protected override IShellNavBarAppearanceTracker CreateNavBarAppearanceTracker()
        {
            return new CustomShellToolbarAppearanceTracker(this, base.CreateNavBarAppearanceTracker());
        }

        protected override IShellItemRenderer CreateShellItemRenderer(ShellItem item) =>
            new BadgeShellItemRenderer(this) { ShellItem = item };
    }
}
