using Senshost.ViewModels;

namespace Senshost.Views;

public partial class NotificationListPage : ContentPage
{
    private readonly NotificationListPageViewModel vm;

    public NotificationListPage(NotificationListPageViewModel notificationListPageViewModel)
    {
        InitializeComponent();
        this.vm = notificationListPageViewModel;
        BindingContext = notificationListPageViewModel;
        notificationListPageViewModel.PropertyChanged += VM_PropertyChanged;
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();

        if (!vm.IsInitialized)
            await vm.Initialize();

        vm.OnAppearing();
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
}