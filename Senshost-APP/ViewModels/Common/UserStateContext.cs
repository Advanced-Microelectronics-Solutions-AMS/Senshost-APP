using CommunityToolkit.Mvvm.ComponentModel;
using Senshost_APP.Common.Interfaces;
using Senshost_APP.Models.Account;
using Senshost_APP.Views;
using AppConst = Senshost_APP.Constants;
using System.Text.Json;
using Senshost_APP.Models.Notification;
using CommunityToolkit.Mvvm.Messaging;
using Plugin.Firebase.CloudMessaging;
using Senshost_APP.Constants;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Senshost_APP.Controls;
using CommunityToolkit.Mvvm.Input;

namespace Senshost_APP.ViewModels
{
    public partial class UserStateContext : BaseObservableRecipientViewModel
    {
        private readonly IGetDeviceInfo getDeviceInfo;
        private readonly INotificationService notificationService;
        private readonly IStorageService storageService;
        private readonly IAuthService authService;

        public UserStateContext(IGetDeviceInfo getDeviceInfo, INotificationService notificationService,
            IStorageService storageService, IAuthService authService)
        {
            this.getDeviceInfo = getDeviceInfo;
            this.notificationService = notificationService;
            this.storageService = storageService;
            this.authService = authService;

            WeakReferenceMessenger.Default.Register<string>(this, async (r, m) =>
            {
                if (IsAuthorized)
                    await GetNotificationCount();
            });

        }

        [ObservableProperty]
        bool validateUser = false;

        [ObservableProperty]
        bool isAuthorized;

        [ObservableProperty]
        string badgeCount;

        public async Task LoginAsync(string email, string password)
        {
            try
            {
                await HandleLoginAsync(email, password);

                if (IsAuthorized)
                {
                    AppShell.Current.FlyoutHeader = new FlyoutHeaderControl();
                    await Shell.Current.GoToAsync($"//{nameof(DashboardPage)}", true);

                    _ = Task.Run(async () =>
                    {
                        await SaveUserDetailsLocally(App.UserDetails, App.ApiToken);
                        await SendDeviceTokenToSever();
                        await GetNotificationCount();
                    });
                }
                else
                {
                    await AppShell.Current.DisplayAlert("Authentication Error", "Could not find user account details.", "Close");
                }
            }
            catch (Exception ex)
            {
                await AppShell.Current.DisplayAlert("Authentication Failed", ex.Message, "Close");
            }
        }

        [RelayCommand]
        public async Task LogoutAsync()
        {
            IsAuthorized = false;
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}", true);
            await RemoveUserDetailsLocally();
            await RemoveDeviceTokenFromSever();
        }

        public async Task CheckUserLoginDetails()
        {
            if (IsAuthorized) return;

            var name = nameof(App.UserDetails);
            string userDetailsStr = Preferences.Get(name, default(string));
            var token = await storageService.GetAsync(APIConstants.ApiSecureStorageToken);

            if (!string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(userDetailsStr))
            {
                var userInfo = JsonSerializer.Deserialize<LogedInUserDetails>(userDetailsStr);
                if (userInfo != null)
                {
                    App.UserDetails = userInfo;
                    App.ApiToken = token;
                    IsAuthorized = true;
                    AppShell.Current.FlyoutHeader = new FlyoutHeaderControl();
                    await Shell.Current.GoToAsync($"//{nameof(DashboardPage)}", true);
                    ValidateUser = true;
                }
            }
            else
                await LogoutAsync();
        }

        partial void OnValidateUserChanged(bool oldValue, bool newValue)
        {
            if (!oldValue && newValue)
            {
                _ = Task.Run(async () =>
                {
                    try
                    {
                        await HandleLoginAsync(App.UserDetails.Email, App.UserDetails.Password);
                        BadgeCount = "0";
                        if (IsAuthorized)
                        {
                            Shell.Current.Dispatcher.Dispatch(async () =>
                            {
                                var toast = Toast.Make($"Authenticated user!", ToastDuration.Long);
                                await toast.Show();
                                AppShell.Current.FlyoutHeader = new FlyoutHeaderControl();
                            });

                            await SaveUserDetailsLocally(App.UserDetails, App.ApiToken);
                            if (!Preferences.ContainsKey(AppConst.AppConstants.UserDeviceTokenIdKey))
                                await SendDeviceTokenToSever();

                            await GetNotificationCount();
                        }
                    }
                    catch (HttpRequestException)
                    {
                        Shell.Current.Dispatcher.Dispatch(async () =>
                        {
                            var toast = Toast.Make($"Could not verify user, Signing Out!", ToastDuration.Long);
                            await toast.Show();
                        });
                        await LogoutAsync();
                    }
                    catch (Exception)
                    {
                        Shell.Current.Dispatcher.Dispatch(async () =>
                        {
                            var toast = Toast.Make($"Could not verify user", ToastDuration.Long);
                            await toast.Show();
                            AppShell.Current.FlyoutHeader = new FlyoutHeaderControl();
                        });
                    }
                });
            }
        }

        private async Task HandleLoginAsync(string email, string password)
        {
            var response = await authService.LoginAsync(email, password);

            if (response?.Account != null)
            {
                var user = GetUserDetailsToStore(response);
                App.UserDetails = user;
                App.ApiToken = response.IdentityToken;
                IsAuthorized = true;
            }
            else
            {
                IsAuthorized = false;
            }
        }

        private async Task SaveUserDetailsLocally(LogedInUserDetails user, string token)
        {
            var name = nameof(App.UserDetails);

            string userDetailStr = JsonSerializer.Serialize(user);
            if (Preferences.ContainsKey(name))
                Preferences.Remove(name);
            Preferences.Set(name, userDetailStr);

            var apiToken = await storageService.GetAsync(APIConstants.ApiSecureStorageToken);

            if (!string.IsNullOrEmpty(apiToken))
                storageService.Remove(APIConstants.ApiSecureStorageToken);
            await storageService.SetAsync(APIConstants.ApiSecureStorageToken, token);
        }

        private async Task SendDeviceTokenToSever()
        {
            try
            {
                await CrossFirebaseCloudMessaging.Current.CheckIfValidAsync();
                var devicetoken = await CrossFirebaseCloudMessaging.Current.GetTokenAsync();
                if (Preferences.ContainsKey(AppConst.AppConstants.FCMDeviceTokenKey))
                    Preferences.Remove(AppConst.AppConstants.FCMDeviceTokenKey);

                Preferences.Set(AppConst.AppConstants.FCMDeviceTokenKey, devicetoken);

                var req = new UserDeviceToken()
                {
                    AccountId = App.UserDetails.AccountId,
                    UserId = App.UserDetails.UserId,
                    DeviceType = $"{DeviceInfo.Current.Platform} {DeviceInfo.Current.Model} {DeviceInfo.Current.Idiom}",
                    UserDeviceId = getDeviceInfo.GetDeviceID(),
                    DeviceRegistrationId = devicetoken
                };

                var result = await notificationService.SaveDeviceToken(req);

                if (result.Id != null && result.Id.Value != Guid.Empty)
                {
                    if (Preferences.ContainsKey(AppConst.AppConstants.UserDeviceTokenIdKey))
                        Preferences.Remove(AppConst.AppConstants.UserDeviceTokenIdKey);

                    Preferences.Set(AppConst.AppConstants.UserDeviceTokenIdKey, result.Id.Value.ToString());
                }
            }
            catch (Exception ex)
            {
                Application.Current.Dispatcher.Dispatch(async () =>
                {
                    await AppShell.Current.DisplayAlert("Error", ex.Message, "Close");
                });

                if (Preferences.ContainsKey(AppConst.AppConstants.UserDeviceTokenIdKey))
                    Preferences.Remove(AppConst.AppConstants.UserDeviceTokenIdKey);
            }
        }

        private async Task RemoveUserDetailsLocally()
        {
            if (Preferences.ContainsKey(nameof(App.UserDetails)))
                Preferences.Remove(nameof(App.UserDetails));

            var apiToken = await storageService.GetAsync(APIConstants.ApiSecureStorageToken);
            if (!string.IsNullOrEmpty(apiToken))
                storageService.Remove(APIConstants.ApiSecureStorageToken);
        }

        private async Task RemoveDeviceTokenFromSever()
        {
            if (Preferences.ContainsKey(AppConst.AppConstants.UserDeviceTokenIdKey))
            {
                var id = Preferences.Get(AppConst.AppConstants.UserDeviceTokenIdKey, default(string));
                Preferences.Remove(AppConst.AppConstants.UserDeviceTokenIdKey);

                try
                {
                    await notificationService.DeleteDeviceToken(id);
                }
                catch { }
            }
        }

        private static LogedInUserDetails GetUserDetailsToStore(AuthenticationResponse response)
        {
            var user = new LogedInUserDetails();

            if (response.AuthResult?.User != null)
            {
                user.AccountId = response.Account.Id;
                user.UserId = response.AuthResult?.User?.Id;
                user.Email = response.AuthResult?.User?.Email;
                user.Name = response.AuthResult?.User?.Name;
                user.Password = response.AuthResult?.User?.Password;
                user.UserRole = response.AuthResult?.User?.Role;
                user.GroupId = response.AuthResult?.Group?.Id;
                user.GroupName = response.AuthResult?.Group?.Name;
                user.GroupStatus = response.AuthResult?.Group?.Status;
            }
            else
            {
                user.AccountId = response.Account.Id;
                user.Email = response.Account.Email;
                user.Name = response.Account.Name;
                user.Password = response.Account.Password;
            }

            return user;
        }

        public async Task GetNotificationCount()
        {
            try
            {
                var notificationCount = await notificationService.GetNotificationCount(App.UserDetails.AccountId, App.UserDetails.UserId);
                BadgeCount = notificationCount.TotalPending.ToString();
            }
            catch (Exception ex)
            {
                await AppShell.Current.DisplayAlert("Error", ex.Message, "Close");
                BadgeCount = "0";
            }
        }

    }
}
