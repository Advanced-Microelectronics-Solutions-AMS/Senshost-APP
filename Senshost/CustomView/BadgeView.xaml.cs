using Microsoft.Maui.Controls.PlatformConfiguration;
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

    //xmlns: ios = "clr-namespace:Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;assembly=Microsoft.Maui.Controls"


        mainGrid.Padding = new Thickness(0, Senshost.App.StatusBarHeight, 0, 0);

        //var safeInsetstmp = Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific.Page.SafeAreaInsets;
       
        //Console.WriteLine(tmp);

        //var safeInsets = Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific.Page.SafeAreaInsets;
        //safeInsets.Left = 20;
        //Padding = safeInsets;

        //Console.WriteLine(safeInsetstmp);


    }

    async void OnTapGestureRecognizerTapped(object sender, TappedEventArgs args)
    {
        await Shell.Current.GoToAsync($"//{nameof(NotificationListPage)}", true);
    }

    void TapGestureRecognizer_Tapped(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        Shell.Current.FlyoutIsPresented = true;
    }

    void Button_Clicked(System.Object sender, System.EventArgs e)
    {
        Shell.Current.FlyoutIsPresented = true;
    }
}
