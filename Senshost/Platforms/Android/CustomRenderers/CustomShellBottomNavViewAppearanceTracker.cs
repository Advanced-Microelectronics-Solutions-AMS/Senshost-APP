using ag = Android.Graphics;
using Android.Graphics.Drawables;
using Android.Views;
using Google.Android.Material.BottomNavigation;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Controls.Platform.Compatibility;
using Microsoft.Maui.Platform;

namespace Senshost.Platforms.Android.CustomRenderers
{
    class CustomShellBottomNavViewAppearanceTracker : ShellBottomNavViewAppearanceTracker
    {
        private readonly IShellContext shellContext;

        public CustomShellBottomNavViewAppearanceTracker(IShellContext shellContext, ShellItem shellItem) : base(shellContext, shellItem)
        {
            this.shellContext = shellContext;
        }

        public override void SetAppearance(BottomNavigationView bottomView, IShellAppearanceElement appearance)
        {
            base.SetAppearance(bottomView, appearance);
            var backgroundDrawable = new GradientDrawable();
            backgroundDrawable.SetShape(ShapeType.Rectangle);
            //backgroundDrawable.SetCornerRadius(70);
            //bottomView.SetElevation(25);

            //var layoutParams = bottomView.LayoutParameters;
            //if (layoutParams is ViewGroup.MarginLayoutParams marginLayoutParams)
            //{
            //    marginLayoutParams.SetMargins(80, 30, 80, 30);
            //    bottomView.LayoutParameters = layoutParams;
            //}

            backgroundDrawable.SetColors(new int[]{
                ag.Color.ParseColor("#8899af"),
                ag.Color.ParseColor("#626A76"),
                ag.Color.ParseColor("#3b4655"),
                ag.Color.ParseColor("#3b4655"),
            });
            backgroundDrawable.SetGradientType(GradientType.LinearGradient);
            bottomView.SetBackground(backgroundDrawable);
        }

        protected override void SetBackgroundColor(BottomNavigationView bottomView, Color color)
        {
            base.SetBackgroundColor(bottomView, color);
            bottomView.RootView?.SetBackgroundColor(shellContext.Shell.CurrentPage.BackgroundColor.ToPlatform());
        }
    }
}