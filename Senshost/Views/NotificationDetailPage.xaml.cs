using Senshost.ViewModels;

namespace Senshost.Views;

public partial class NotificationDetailPage : ContentPage
{
	public NotificationDetailPage(NotificationDetailPageViewModel notificationsDetail)
	{
		InitializeComponent();
		BindingContext = notificationsDetail;

		notificationsDetail.InitializeComponents();
    }

    public NotificationDetailPage()
    {
        InitializeComponent();
        BindingContext = new NotificationDetailPageViewModel();

        //notificationsDetail.InitializeComponents();
    }



}