namespace Senshost.Controls.BadgeView
{
    public abstract class BaseTemplatedView<TControl> : TemplatedView where TControl : View, new()
    {
        protected TControl? Control { get; private set; }

        public BaseTemplatedView()
            => ControlTemplate = new ControlTemplate(typeof(TControl));

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (Control != null)
                Control.BindingContext = BindingContext;
        }

        protected override void OnChildAdded(Element child)
        {
            if (Control == null && child is TControl control)
            {
                Control = control;
                OnControlInitialized(Control);
            }

            base.OnChildAdded(child);
        }

        protected abstract void OnControlInitialized(TControl control);
    }
}