using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;

namespace Senshost_APP.ViewModels
{
    public abstract partial class BaseObservableViewModel : ObservableObject, IQueryAttributable
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))]
        private bool isBusy;

        [ObservableProperty]
        private string title;

        [ObservableProperty]
        private bool isInitialized;

        public bool IsNotBusy => !IsBusy;


        public virtual void ApplyQueryAttributes(IDictionary<string, object> query)
        {
        }
    }

    public partial class BaseObservableRecipientViewModel : ObservableRecipient
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))]
        private bool isBusy;

        [ObservableProperty]
        private string title;

        [ObservableProperty]
        private bool isInitialized;

        public bool IsNotBusy => !IsBusy;
    }

    public partial class BaseObservableValidatorViewModel : ObservableValidator, IQueryAttributable
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))]
        private bool isBusy;

        [ObservableProperty]
        private string title;

        [ObservableProperty]
        private bool isInitialized;

        public bool IsNotBusy => !IsBusy;

        public virtual void ApplyQueryAttributes(IDictionary<string, object> query)
        {
        }
    }

    public abstract class BaseObservableRecipientWithValidation : ObservableValidator
    {
        protected BaseObservableRecipientWithValidation()
            : this(WeakReferenceMessenger.Default)
        {
        }

        protected BaseObservableRecipientWithValidation(IMessenger messenger)
        {
            Messenger = messenger;
        }

        protected IMessenger Messenger { get; }
    }
}
