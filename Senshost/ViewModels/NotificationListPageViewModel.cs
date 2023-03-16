using System.Collections.ObjectModel;
using System.Reflection.Metadata;
using AndroidX.Startup;
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

            Initialize();
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
        int itemThreshold = 2;

        //[RelayCommand]
        public async void Initialize()
        {
            IsBusy = true;
            CurrentState = LayoutState.Loading;
            paginationData = new(20);
            Notifications = new ObservableCollection<NotificationDetailPageViewModel>();
            ItemThreshold = 2;

            await InitializeNotifications();
            await InitializeNotificationCount();

            CurrentState = LayoutState.Success;

            IsInitialized = true;
            IsBusy = false;
            IsRefreshing = false;
        }

        public void OnAppearing()
        {
            // Register a message in some module
            WeakReferenceMessenger.Default.Register<Dictionary<string, string>>(this, (r, m) =>
            {
                Refresh();
            });

        }

        public void OnDisappearing()
        {
            WeakReferenceMessenger.Default.Unregister<Dictionary<string, string>>(this);
        }


        [RelayCommand]
        public async Task LoadMore()
        {
            if (IsLoadingMore || Notifications.Count == 0)
                return;

            IsLoadingMore = true;
            paginationData.PageNumber++;
            await Task.Delay(2000);

            var notificationsTmp = await GetNotifications(paginationData);

            notificationsTmp.ToList().ForEach(x => Notifications.Add(
             new NotificationDetailPageViewModel()
             {
                 Notification = x,
                 Status = x.Status,
                 UserNotificationId = x.UserNotificationId
             }
            ));

            IsLoadingMore = false;
        }

        [RelayCommand]
        public void Refresh()
        {
            IsRefreshing = true;
            Initialize();
        }

        [RelayCommand]
        public async Task Detail(NotificationDetailPageViewModel notificationsDetail)
        {
            await App.Current.MainPage.Navigation.PushAsync(new NotificationDetailPage(notificationsDetail));

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
            var task = await Task.Run(async () =>
            {
                var result = await notificationService.GetNotifications(App.UserDetails.AccountId, App.UserDetails.UserId, pagination.PageSize, pagination.PageNumber, pagination.Sort.ToString());

                paginationData = result.Pagination;

                if (paginationData.NumberOfPage == pagination.PageNumber)
                    ItemThreshold = -1;

                //return result.Data;
                return result;
            });

            return task.Data;
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

            BadgeCount = "" + PendingAllNotificationCount;

            return;
        }

        private async Task InitializeNotifications()
        {
            var notificationsTmp = await GetNotifications(paginationData);

            Notifications = new ObservableCollection<NotificationDetailPageViewModel>();

            if (notificationsTmp != null)
            {
                foreach (var item in notificationsTmp)
                {
                    Notifications.Add(new NotificationDetailPageViewModel()
                    {
                        Notification = item,
                        Status = item.Status,
                        UserNotificationId = item.UserNotificationId
                    });
                }
            }


            //Notifications = new ObservableCollection<NotificationDetailPageViewModel>(notifications.Select(x =>
            // new NotificationDetailPageViewModel()
            // {
            //     Notification = x,
            //     Status = x.Status,
            //     UserNotificationId = x.UserNotificationId
            // }
            //));

            return;
        }
    }
}
