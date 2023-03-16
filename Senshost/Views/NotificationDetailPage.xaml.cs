using Senshost.Models.Notification;
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

        var title = Preferences.Get("pushTitle", "no");

        var bindCntxt = new NotificationDetailPageViewModel();
        if (Senshost.App.PushData is Dictionary<string, string> pushDict)
        {
            var notification = new Notification()
            {
                Title = pushDict["title"],
                Body = pushDict["body"],
                CreationDate = DateTime.Parse(pushDict["creationDate"])
            };
            bindCntxt.Notification = notification;

            Senshost.App.PushData = null;
        }
        BindingContext = bindCntxt;

        


        //notificationsDetail.InitializeComponents();
    }



}