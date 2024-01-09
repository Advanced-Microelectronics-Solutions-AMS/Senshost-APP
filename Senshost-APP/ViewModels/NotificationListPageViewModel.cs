using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Converters;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Senshost_APP.Common.Interfaces;
using Senshost_APP.Models.Common;
using Senshost_APP.Models.Constants;
using Senshost_APP.Models.Notification;
using Senshost_APP.Views;

namespace Senshost_APP.ViewModels
{
    public partial class NotificationListPageViewModel : BaseObservableViewModel
    {
        private PaginationResult paginationData = new(20);
        private readonly INotificationService notificationService;
        private readonly UserStateContext userStateContext;

        public NotificationListPageViewModel(INotificationService notificationService, UserStateContext userStateContext)
        {
            this.notificationService = notificationService;
            this.userStateContext = userStateContext;

            WeakReferenceMessenger.Default.Register<string>(this, (r, m) =>
            {
                _ = InitializeNotificationCount();
                _ = InitializeNotifications();
            });

            Initialize();
        }

        [ObservableProperty]
        private ObservableCollection<NotificationDetailPageViewModel> notifications = new();
        [ObservableProperty]
        private NotificationDetailPageViewModel selectedNotification;
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
        [ObservableProperty]
        SeverityLevel? severityLevel;
        [ObservableProperty]
        NotificationStatus? notificationStatus = Models.Constants.NotificationStatus.Pending;
        [ObservableProperty]
        int selectedTabIndex = 0;
        [ObservableProperty]
        bool isAllChecked;
        [ObservableProperty]
        bool isOptionExpended;

        public void Initialize()
        {
            IsBusy = true;
            CurrentState = LayoutState.Loading;

            _ = Task.Run(async () =>
            {
                await Task.WhenAll(
                InitializeNotifications(),
                InitializeNotificationCount());

                CurrentState = LayoutState.Success;

                IsInitialized = true;
                IsBusy = false;
            });

            IsRefreshing = false;
        }

        partial void OnSelectedTabIndexChanged(int value)
        {
            switch (value)
            {
                case 0: SeverityLevel = null; break;
                case 1: SeverityLevel = Models.Constants.SeverityLevel.Critical; break;
                case 2: SeverityLevel = Models.Constants.SeverityLevel.Warning; break;
                case 3: SeverityLevel = Models.Constants.SeverityLevel.Info; break;
            }
            Initialize();
        }

        partial void OnIsAllCheckedChanged(bool value)
        {
            if (value)
                NotificationStatus = null;
            else
                NotificationStatus = Models.Constants.NotificationStatus.Pending;

            IsOptionExpended = false;

            Initialize();
        }

        [RelayCommand]
        public async Task LoadMore()
        {
            if (IsLoadingMore || Notifications.Count == 0)
                return;

            IsLoadingMore = true;
            paginationData.PageNumber++;

            var notificationsTmp = await GetNotifications(paginationData);

            notificationsTmp?.ToList()?.ForEach(x => Notifications.Add(
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
        public async Task MarkAllRead()
        {
            IsBusy = true;
            try
            {
                var result = await notificationService.GetNotifications(App.UserDetails.AccountId, App.UserDetails.UserId, SeverityLevel,
                            Models.Constants.NotificationStatus.Pending, null, null, DataSorting.Descending.ToString());

                if (result.Data.Count() > 0)
                {
                    foreach (var notif in result.Data)
                    {
                        try
                        {
                            await notificationService.AddUpdateNotificationStatus(new UserNotification()
                            {
                                UserId = new Guid(App.UserDetails.UserId ?? App.UserDetails.AccountId),
                                NotificationId = notif.Id.Value,
                                Status = Models.Constants.NotificationStatus.Read
                            });

                        }
                        catch { }
                    }

                    var toast = Toast.Make("Success", ToastDuration.Short);
                    await toast.Show();
                    Initialize();
                }
                else
                {
                    var toast = Toast.Make("No pending notification!", ToastDuration.Short);
                    await toast.Show();
                }
            }
            catch (Exception ex)
            {
                AppShell.Current.Dispatcher.Dispatch(async () =>
                            await AppShell.Current.DisplayAlert("Error", ex.Message, "Close"));
            }
            finally
            {
                IsBusy = false;
                IsOptionExpended = false;
            }
        }

        public void SetNotificationAsRead(NotificationDetailPageViewModel notificationsDetail)
        {
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
                userStateContext.BadgeCount = PendingAllNotificationCount.ToString();

                _ = Task.Run(async () =>
                {
                    try
                    {
                        await notificationService.AddUpdateNotificationStatus(new UserNotification()
                        {
                            UserId = new Guid(App.UserDetails.UserId ?? App.UserDetails.AccountId),
                            NotificationId = notificationsDetail.Notification.Id.Value,
                            Status = Models.Constants.NotificationStatus.Read
                        });
                    }
                    catch (Exception ex)
                    {
                        AppShell.Current.Dispatcher.Dispatch(async () =>
                        await AppShell.Current.DisplayAlert("Error", ex.Message, "Close"));
                    }
                });
            }
            IsOptionExpended = false;
        }

        async partial void OnSelectedNotificationChanged(NotificationDetailPageViewModel oldValue, NotificationDetailPageViewModel newValue)
        {
            if (newValue != null)
            {
                await Shell.Current.GoToAsync($"{nameof(NotificationDetailPage)}",
                    new Dictionary<string, object> { ["Notification"] = newValue.Notification });
                SetNotificationAsRead(newValue);
                SelectedNotification = null;
            }
            IsOptionExpended = false;
        }

        private async Task<IEnumerable<NotificationsDetail>> GetNotifications(Pagination pagination)
        {
            try
            {
                var result = await notificationService.GetNotifications(App.UserDetails.AccountId, App.UserDetails.UserId,
                    SeverityLevel, NotificationStatus, pagination.PageSize, pagination.PageNumber, pagination.Sort.ToString());

                paginationData = result.Pagination;

                if (paginationData.NumberOfPage == pagination.PageNumber)
                    ItemThreshold = -1;

                return result.Data;
            }
            catch (Exception ex)
            {
                await AppShell.Current.DisplayAlert("Error", ex.Message, "Close");
                return null;
            }
        }

        private async Task<NotificationCount> GetNotificationCount()
        {
            try
            {
                return await notificationService.GetNotificationCount(App.UserDetails.AccountId, App.UserDetails.UserId);
            }
            catch (Exception ex)
            {
                await AppShell.Current.DisplayAlert("Error", ex.Message, "Close");
                return null;
            }
        }

        public async Task InitializeNotificationCount()
        {
            var notificationCount = await GetNotificationCount();

            PendingAllNotificationCount = notificationCount?.TotalPending ?? 0;
            PendingInfoNotificationCount = notificationCount?.Info ?? 0;
            PendingWarningNotificationCount = notificationCount?.Warning ?? 0;
            PendingCritialNotificationCount = notificationCount?.Critical ?? 0;
            userStateContext.BadgeCount = PendingAllNotificationCount.ToString();
            return;
        }

        public async Task InitializeNotifications()
        {
            paginationData = new(20);
            ItemThreshold = 5;
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
            return;
        }
    }
}
