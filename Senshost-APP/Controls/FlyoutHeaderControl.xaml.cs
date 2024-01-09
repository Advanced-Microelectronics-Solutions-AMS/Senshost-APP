namespace Senshost_APP.Controls;

public partial class FlyoutHeaderControl : ContentView
{
    public FlyoutHeaderControl()
    {
        InitializeComponent();

        userName.Text = App.UserDetails.Name;
        emailAddress.Text = App.UserDetails.Email;
        firstLetter.Text = App.UserDetails.Name.Substring(0, 1);
    }
}