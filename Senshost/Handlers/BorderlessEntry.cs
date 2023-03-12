namespace Senshost.Handlers
{
    public class BorderlessEntry : Entry
    {
        public static BindableProperty CursorColorProperty = BindableProperty.Create(
            nameof(CursorColor), typeof(Color), typeof(BorderlessEntry), Colors.White);

        public Color CursorColor
        {
            get => (Color)GetValue(CursorColorProperty);
            set => SetValue(CursorColorProperty, value);
        }
    }
}
