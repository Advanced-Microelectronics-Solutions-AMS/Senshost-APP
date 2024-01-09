using Android.OS;
using Android.Views;
using Google.Android.Material.Badge;
using Google.Android.Material.BottomNavigation;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using Microsoft.Maui.Controls.Platform.Compatibility;
using Microsoft.Maui.Platform;
using Senshost_APP.Controls;
using System.ComponentModel;
using AView = Android.Views.View;


namespace Senshost_APP.Platforms.Android.CustomRenderers
{
    public class BadgeShellItemRenderer : ShellItemRenderer
    {
        private readonly IShellContext _shellContext;

        private readonly string[] _applyPropertyNames =
            new string[]
            {
                Badge.TextProperty.PropertyName,
                Badge.TextColorProperty.PropertyName,
                Badge.BackgroundColorProperty.PropertyName
            };

        private BottomNavigationView _bottomNavigationView;

        private readonly Dictionary<Guid, int> _tabRealIndexByItemId =
            new Dictionary<Guid, int>();

        public BadgeShellItemRenderer(IShellContext shellContext) : base(shellContext)
        {
            _shellContext = shellContext;
        }

        public override AView OnCreateView(
            LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var outerlayout = base.OnCreateView(inflater, container, savedInstanceState);
            if (outerlayout is ViewGroup vg)
                _bottomNavigationView = vg.GetFirstChildOfType<BottomNavigationView>();

            return outerlayout;
        }

        public override void OnDestroyView()
        {
            base.OnDestroyView();

            _bottomNavigationView = default;
        }

        public override void OnResume()
        {
            base.OnResume();

            Application.Current.Dispatcher.Dispatch(() => _shellContext.CurrentDrawerLayout.Post(InitBadges));
               
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override void OnShellSectionPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnShellSectionPropertyChanged(sender, e);

            if (_applyPropertyNames.All(x => x != e.PropertyName))
                return;

            Application.Current.Dispatcher.Dispatch(() =>
                {
                    var item = (ShellSection)sender;
                    if (item.IsVisible)
                    {
                        var index = _tabRealIndexByItemId.GetValueOrDefault(item.Id, -1);
                        UpdateBadge(item, index);
                    }
                });
        }

        private void InitBadges()
        {
            _tabRealIndexByItemId.Clear();
            for (int index = 0, filteredIndex = 0; index < ShellItem.Items.Count; index++)
            {
                var item = ShellItem.Items.ElementAtOrDefault(index);
                if (!item.IsVisible)
                    continue;
                _tabRealIndexByItemId[item.Id] = filteredIndex;
                UpdateBadge(item, filteredIndex);
                filteredIndex++;
            }
        }

        private void UpdateBadge(ShellSection item, int index)
        {
            if (index < 0)
                return;

            var text = Badge.GetText(item);
            var textColor = Badge.GetTextColor(item);
            var bg = Badge.GetBackgroundColor(item);
            ApplyBadge(index, text, bg, textColor);
        }

        private void ApplyBadge(int itemId, string badgeText, Color badgeBg, Color textColor)
        {
            if (default == _bottomNavigationView)
                return;

            using var bottomNavigationMenuView = (BottomNavigationMenuView)_bottomNavigationView.GetChildAt(0);
            using var itemView = bottomNavigationMenuView.FindViewById<BottomNavigationItemView>(itemId);
            if (default == itemView)
                return;

            using var iconView = itemView.GetChildAt(0);

            var badgeBackgroundColor = badgeBg.ToAndroid();
            var badgeTextColor = textColor.ToAndroid();
            _ = int.TryParse(badgeText, out var badgeNumber);

            var badge = BadgeDrawable.Create(new ContextThemeWrapper(
                _shellContext.AndroidContext, Resource.Style.Base_Theme_MaterialComponents_Bridge));
            badge.BackgroundColor = badgeBackgroundColor;
            badge.BadgeTextColor = badgeTextColor;
            badge.VerticalOffset = (int)(iconView.Top / 1.5);

            iconView.Overlay?.Clear();

            if (string.IsNullOrEmpty(badgeText))
            {
                badge.SetVisible(false);
            }
            else
            {
                badge.SetVisible(true);

                if (badgeNumber == 0)
                {
                    badge.ClearNumber();
                    BadgeUtils.AttachBadgeDrawable(badge, iconView);
                }
                else
                {
                    badge.Number = badgeNumber;
                    BadgeUtils.AttachBadgeDrawable(badge, iconView);
                }
            }
        }
    }
}
