using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Senshost_APP.ViewModels
{
    public partial class LoginPageViewModel : BaseObservableValidatorViewModel
    {
        private string password;
        private string email;

        private readonly UserStateContext userStateContext;

        public LoginPageViewModel(UserStateContext userStateContext)
        {
            this.userStateContext = userStateContext;
            this.ErrorsChanged += new EventHandler<DataErrorsChangedEventArgs>(ValidationBase_ErrorsChanged);

#if DEBUG
            email = "Demo@gmail.com";
            password = "M@ster12345";
#endif
        }


        [Required(ErrorMessage = "Please enter email address")]
        public string Email
        {
            get => email;
            set => SetProperty(ref email, value, true);
        }

        [Required(ErrorMessage = "Please enter password")]
        public string Password
        {
            get => password;
            set => SetProperty(ref password, value, true);
        }

        [ObservableProperty]
        private bool inValidEmail;

        [ObservableProperty]
        private bool inValidPassword;

        [RelayCommand]
        public async Task LoginAsync()
        {
            IsBusy = true;
            ValidateAllProperties();

            if (!HasErrors)
                await userStateContext.LoginAsync(Email.Trim(), Password.Trim());

            IsBusy = false;
        }

        [RelayCommand]
        public async Task OpenAMSLink()
        {
            BrowserLaunchOptions options = new BrowserLaunchOptions()
            {
                LaunchMode = BrowserLaunchMode.SystemPreferred,
                TitleMode = BrowserTitleMode.Show,
                PreferredToolbarColor = Color.FromArgb("#3b4655"),
                PreferredControlColor = Colors.SandyBrown
            };
            await Browser.Default.OpenAsync(new Uri("https://amsolutions.my"), options);
        }

        private void ValidationBase_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            InValidEmail = GetErrors(nameof(Email)).Count() > 0;
            InValidPassword = GetErrors(nameof(Password)).Count() > 0;
        }
    }
}
