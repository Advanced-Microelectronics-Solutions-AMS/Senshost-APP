using Senshost.Common.Interfaces;
using Senshost.Services;

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

		GetNotificationCount();

    }

	private void GetNotificationCount()
	{


    }
}
