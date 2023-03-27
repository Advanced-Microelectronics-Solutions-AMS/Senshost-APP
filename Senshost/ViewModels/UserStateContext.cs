using CommunityToolkit.Mvvm.ComponentModel;
using Senshost.Common.Interfaces;
using Senshost.Controls;
using Senshost.Models.Account;
using Senshost.Views;
using Const = Senshost.Common.Constants;
using AppConst = Senshost.Constants;
using System.Text.Json;
using Senshost.Models.Notification;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.Messaging;
using Plugin.Firebase.CloudMessaging;

namespace Senshost.ViewModels
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
                await GetNotificationCount();
            });

        }

        [ObservableProperty]
        bool isAuthorized;

        public async Task LoginAsync(string email, string password)
        {
            try
            {
                await HandleLoginAsync(email, password);
            }
            catch (Exception ex)
            {
                await AppShell.Current.DisplayAlert("Authentication Failed", ex.Message, "Close");
            }

            if (IsAuthorized)
            {
                await Shell.Current.GoToAsync($"//{nameof(DashboardPage)}", true);
                _ = Task.Run(async () =>
                {
                    await SaveUserDetailsLocally(App.UserDetails, App.ApiToken);
                    await SendDeviceTokenToSever();
                    await GetNotificationCount();
                });
            }
        }

        public async Task LogoutAsync()
        {
            IsAuthorized = false;
            _ = Task.Run(async () =>
            {
                await RemoveUserDetailsLocally();
                await RemoveDeviceTokenFromSever();
            });

            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}", true);

            //await Shell.Current.GoToAsync($"//{nameof(DashboardPage)}", true);

        }

        public async Task CheckUserLoginDetails()
        {
            var name = nameof(App.UserDetails);
            string userDetailsStr = Preferences.Get(name, default(string));
            var token = await storageService.GetAsync(Const.Constants.ApiSecureStorageToken);

            if (!string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(userDetailsStr))
            {
                var userInfo = JsonSerializer.Deserialize<LogedInUserDetails>(userDetailsStr);
                if (userInfo != null)
                {
                    try
                    {
                        await HandleLoginAsync(userInfo.Email, userInfo.Password);

                        if (IsAuthorized)
                        {
                            await Shell.Current.GoToAsync($"//{nameof(DashboardPage)}", true);

                            _ = Task.Run(async () =>
                            {
                                await SaveUserDetailsLocally(App.UserDetails, App.ApiToken);

                                if (!Preferences.ContainsKey(AppConst.AppConstants.UserDeviceTokenIdKey))
                                    await SendDeviceTokenToSever();
                                await GetNotificationCount();
                            });
                        }

                    }
                    catch (HttpRequestException)
                    {
                        var toast = Toast.Make($"Could not verify user, Loging Out!", ToastDuration.Long);
                        await toast.Show();
                        await LogoutAsync();
                    }
                    catch
                    {
                        var toast = Toast.Make($"Could not verify user", ToastDuration.Long);
                        await toast.Show();

                        App.UserDetails = userInfo;
                        App.ApiToken = token;

                        IsAuthorized = true;

                        AppShell.Current.FlyoutHeader = new FlyoutHeaderControl();

                        await Shell.Current.GoToAsync($"//{nameof(DashboardPage)}", true);
                    }
                }

            }
            else
                await LogoutAsync();
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

                AppShell.Current.FlyoutHeader = new FlyoutHeaderControl();
            }
            else
                await AppShell.Current.DisplayAlert("Authentication Failed", "Could not find user account details.", "Close");
        }

        private async Task SaveUserDetailsLocally(LogedInUserDetails user, string token)
        {
            var name = nameof(App.UserDetails);

            string userDetailStr = JsonSerializer.Serialize(user);
            if (Preferences.ContainsKey(name))
                Preferences.Remove(name);
            Preferences.Set(name, userDetailStr);

            var apiToken = await storageService.GetAsync(Const.Constants.ApiSecureStorageToken);

            if (!string.IsNullOrEmpty(apiToken))
                storageService.Remove(Const.Constants.ApiSecureStorageToken);
            await storageService.SetAsync(Const.Constants.ApiSecureStorageToken, token);
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
            catch
            {
                if (Preferences.ContainsKey(AppConst.AppConstants.UserDeviceTokenIdKey))
                    Preferences.Remove(AppConst.AppConstants.UserDeviceTokenIdKey);
            }
        }

        private async Task RemoveUserDetailsLocally()
        {
            if (Preferences.ContainsKey(nameof(App.UserDetails)))
                Preferences.Remove(nameof(App.UserDetails));

            var apiToken = await storageService.GetAsync(Const.Constants.ApiSecureStorageToken);
            if (!string.IsNullOrEmpty(apiToken))
                storageService.Remove(Const.Constants.ApiSecureStorageToken);
        }

        private async Task RemoveDeviceTokenFromSever()
        {
            if (Preferences.ContainsKey(AppConst.AppConstants.UserDeviceTokenIdKey))
            {
                var id = Preferences.Get(AppConst.AppConstants.UserDeviceTokenIdKey, default(string));
                Preferences.Remove(AppConst.AppConstants.UserDeviceTokenIdKey);

                await notificationService.DeleteDeviceToken(id);
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
            var result = await notificationService.GetNotificationCount(App.UserDetails.AccountId, App.UserDetails.UserId);
            BadgeCount = result.TotalPending.ToString();
        }

    }
}
