namespace Senshost.Controls;

public partial class FlyoutHeaderControl : Grid
{
    public FlyoutHeaderControl()
    {
        InitializeComponent();

        userName.Text = App.UserDetails?.Name.Length > 20 ? $"{App.UserDetails?.Name.Substring(0, 20)}..."  : App.UserDetails?.Name;
        emailAddress.Text = App.UserDetails?.Email;
        firstLetter.Text = App.UserDetails?.Name.Substring(0, 1);
    }
}