using Senshost.ViewModels;

namespace Senshost.Views.Templates;

public partial class NotificationTemplate : ContentView
{
	public NotificationTemplate()
	{
		InitializeComponent();
	}

    async void TapGestureRecognizer_Tapped(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        var colorStr = "#7ba651";

        if (e.Parameter is NotificationDetailPageViewModel vm)
        {
            if(vm.Notification?.Severity == Models.Constants.SeverityLevel.Critical)
            {
                colorStr = "#b86935";
            } else if (vm.Notification?.Severity == Models.Constants.SeverityLevel.Info)
            {               
                colorStr = "#5c3b5c";
            }
        }

        mainGridBG.BackgroundColor = Color.FromArgb(colorStr);

        await Task.Delay(250);

        mainGridBG.BackgroundColor = Color.FromArgb("#00000000");

       

    }
}