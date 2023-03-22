using System.ComponentModel;

namespace Senshost.ViewModels
{
    public class EventListPageViewModel : BaseObservableRecipientViewModel
    {
        private readonly UserStateContext userStateContext;

        public EventListPageViewModel(UserStateContext userStateContext)
        {
            userStateContext.PropertyChanged += OnUserStatePropertyChanged;
            this.userStateContext = userStateContext;
            BadgeCount = userStateContext.BadgeCount;
        }

        private void OnUserStatePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(userStateContext.BadgeCount))
            {
                BadgeCount = userStateContext.BadgeCount;
            }
        }
    }
}
