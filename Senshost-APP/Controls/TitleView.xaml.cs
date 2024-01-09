namespace Senshost_APP.Controls;

public partial class TitleView : ContentView
{
    public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(TitleView), string.Empty, propertyChanged: OnTitleChanged);

    private static void OnTitleChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var titleView = (TitleView)bindable;
        titleView.lblTitle.Text = (string)newValue;
    }

    public string Title
    {
        get => (string)GetValue(TitleView.TitleProperty);
        set => SetValue(TitleView.TitleProperty, value);
    }

    public static readonly BindableProperty IconProperty = BindableProperty.Create(nameof(Icon), typeof(string), typeof(TitleView), string.Empty, propertyChanged: OnIconChanged);

    private static void OnIconChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var titleView = (TitleView)bindable;
        if (newValue != null)
        {
            titleView.icon.IsVisible = true;
            titleView.icon.Source = (string)newValue;
        }
        else
        {
            titleView.icon.IsVisible = false;
        }
    }

    public string Icon
    {
        get => (string)GetValue(TitleView.IconProperty);
        set => SetValue(TitleView.IconProperty, value);
    }

    public static readonly BindableProperty ToolBarItemsProperty = BindableProperty.Create(nameof(ToolBarItems), typeof(Layout), typeof(TitleView), default(Layout), propertyChanged: OnToolbarItemsChanged);

    private static void OnToolbarItemsChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var titleView = (TitleView)bindable;
        if (oldValue != null)
        {
            titleView.toolbarParant.Remove(oldValue as Layout);
            return;
        }

        titleView.toolbarParant.Add(newValue as Layout);
    }

    public Layout ToolBarItems
    {
        get => (Layout)GetValue(TitleView.ToolBarItemsProperty);
        set => SetValue(TitleView.ToolBarItemsProperty, value);
    }

    public TitleView()
    {
        InitializeComponent();
    }


}