using System.Collections.ObjectModel;
using System.Reflection.Metadata;
using CommunityToolkit.Maui.Converters;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Senshost.Common.Interfaces;
using Senshost.Models.Account;
using Senshost.Models.Common;
using Senshost.Models.Notification;
using Senshost.Views;

namespace Senshost.ViewModels
{
    public partial class NotificationListPageViewModel : BaseObservableViewModel
    {
        private PaginationResult paginationData = new(20);
        private readonly INotificationService notificationService;

        public NotificationListPageViewModel(INotificationService notificationService)
        {
            this.notificationService = notificationService;
        }

        [ObservableProperty]
        private ObservableCollection<NotificationDetailPageViewModel> notifications = new();
        [ObservableProperty]
        private int pendingAllNotificationCount;
        [ObservableProperty]
        private int pendingInfoNotificationCount;
        [ObservableProperty]
        private int pendingWarningNotificationCount;
        [ObservableProperty]
        private int pendingCritialNotificationCount;
        [ObservableProperty]
        LayoutState currentState = LayoutState.Loading;
        [ObservableProperty]
        bool isLoadingMore;
        [ObservableProperty]
        bool isRefreshing;
        [ObservableProperty]
        int itemThreshold = 5;

        [RelayCommand]
        public async Task Initialize()
        {
            IsBusy = true;
            CurrentState = LayoutState.Loading;
            paginationData = new(20);
            Notifications = new ObservableCollection<NotificationDetailPageViewModel>();
            ItemThreshold = 5;

            await InitializeNotifications();
            await InitializeNotificationCount();

            CurrentState = LayoutState.Success;
            IsInitialized = true;
            IsBusy = false;
        }

        public void OnAppearing()
        {
            // Register a message in some module
            WeakReferenceMessenger.Default.Register<Dictionary<string, string>>(this, async (r, m) =>
            {
                await Refresh();
            });

        }

        public void OnDisappearing()
        {
            WeakReferenceMessenger.Default.Unregister<Dictionary<string, string>>(this);
        }


        [RelayCommand]
        public async Task LoadMore()
        {
            if (IsLoadingMore)
                return;

            IsLoadingMore = true;
            paginationData.PageNumber++;
            await Task.Delay(2000);

            var notifications = await GetNotifications(paginationData);

            notifications.ToList().ForEach(x => Notifications.Add(
             new NotificationDetailPageViewModel()
             {
                 Notification = x,
                 Status = x.Status,
                 UserNotificationId= x.UserNotificationId
             }
            ));

            IsLoadingMore = false;
        }

        [RelayCommand]
        public async Task Refresh()
        {
            IsRefreshing = true;
            await Initialize();
            IsRefreshing = false;
        }

        [RelayCommand]
        public async Task Detail(NotificationDetailPageViewModel notificationsDetail)
        {
            await App.Current.MainPage.Navigation.PushAsync(new NotificationDetailPage { BindingContext = notificationsDetail.Notification });

                if (notificationsDetail.Status == Models.Constants.NotificationStatus.Pending)
            {
                notificationsDetail.Status = Models.Constants.NotificationStatus.Read;

                PendingAllNotificationCount--;
                if (notificationsDetail.Notification.Severity == Models.Constants.SeverityLevel.Info)
                    PendingInfoNotificationCount--;
                else if (notificationsDetail.Notification.Severity == Models.Constants.SeverityLevel.Warning)
                    PendingWarningNotificationCount--;
                else if (notificationsDetail.Notification.Severity == Models.Constants.SeverityLevel.Critical)
                    PendingCritialNotificationCount--;

                _ = Task.Run(async () =>
                {
                    await notificationService.AddUpdateNotificationStatus(new UserNotification()
                    {
                        UserId = new Guid(App.UserDetails.UserId ?? App.UserDetails.AccountId),
                        NotificationId = notificationsDetail.Notification.Id.Value,
                        Status = Models.Constants.NotificationStatus.Read
                    });
                });
            }
        }

        private async Task<IEnumerable<NotificationsDetail>> GetNotifications(Pagination pagination)
        {
            var result = await notificationService.GetNotifications(App.UserDetails.AccountId, App.UserDetails.UserId, pagination.PageSize, pagination.PageNumber, pagination.Sort.ToString());
            paginationData = result.Pagination;

            if (paginationData.NumberOfPage == pagination.PageNumber)
                ItemThreshold = -1;

            return result.Data;
        }

        private async Task<NotificationCount> GetNotificationCount()
        {
            return await notificationService.GetNotificationCount(App.UserDetails.AccountId, App.UserDetails.UserId);
        }

        private async Task InitializeNotificationCount()
        {
            var notificationCount = await GetNotificationCount();

            PendingAllNotificationCount = notificationCount.TotalPending;
            PendingInfoNotificationCount = notificationCount.Info;
            PendingWarningNotificationCount = notificationCount.Warning;
            PendingCritialNotificationCount = notificationCount.Critical;
        }

        private async Task InitializeNotifications()
        {
            var notifications = await GetNotifications(paginationData);

            Notifications = new ObservableCollection<NotificationDetailPageViewModel>(notifications.Select(x =>
             new NotificationDetailPageViewModel()
             {
                 Notification = x,
                 Status = x.Status,
                 UserNotificationId= x.UserNotificationId
             }
            ));
        }
    }
}
