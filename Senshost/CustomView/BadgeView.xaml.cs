using Senshost.Views;

namespace Senshost.CustomView;

public partial class BadgeView : ContentView
{
    public string Title
    {
        set
        {
            lblTitle.Text = value;

        }
    }

    public BadgeView()
    {
        InitializeComponent();
    }

    async void OnTapGestureRecognizerTapped(object sender, TappedEventArgs args)
    {
        await Shell.Current.GoToAsync($"//{nameof(NotificationListPage)}", true);
    }
}
