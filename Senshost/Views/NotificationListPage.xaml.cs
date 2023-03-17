using Senshost.ViewModels;

namespace Senshost.Views;


[XamlCompilation(XamlCompilationOptions.Compile)]

[QueryProperty("IsToReloadPage", "isToReloadPage")]
public partial class NotificationListPage : ContentPage
{
    private readonly NotificationListPageViewModel vm;

    bool isToReloadPage;
    public bool IsToReloadPage
    {
        set
        {
            if(value)
            {
                vm.Initialize();
            }
            isToReloadPage = value;
        }
    }

    public NotificationListPage(NotificationListPageViewModel notificationListPageViewModel)
    {
        InitializeComponent();
        this.vm = notificationListPageViewModel;
        BindingContext = notificationListPageViewModel;
        notificationListPageViewModel.PropertyChanged += VM_PropertyChanged;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        vm.OnAppearing();
        vm.BadgeCount = Senshost.App.BadgeCount;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        vm.OnDisappearing();
    }

    private void VM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(vm.IsInitialized))
        {
            if (vm.IsInitialized)
            {
                Task.Run(async () =>
                {
                    await CriticalCount.ScaleTo(1, 200, Easing.Linear);
                    await WarningCount.ScaleTo(1, 200, Easing.Linear);
                    await InfoCount.ScaleTo(1, 200, Easing.Linear);
                });
            }
        }
    }

    void CollectionView_SelectionChanged(System.Object sender, Microsoft.Maui.Controls.SelectionChangedEventArgs e)
    {
        if(listNotification.SelectedItem != null)
        {
            listNotification.SelectedItem = null;
        }
    }
}