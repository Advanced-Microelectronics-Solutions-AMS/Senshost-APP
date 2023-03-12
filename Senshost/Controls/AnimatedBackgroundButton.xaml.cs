namespace Senshost.Controls;

public partial class AnimatedBackgroundButton : Grid
{
    public AnimatedBackgroundButton()
    {
        InitializeComponent();

        Task.Run(AnimateBackground);
    }

    private async void AnimateBackground()
    {
        var color1 = Color.FromArgb("#23D5AB");
        var color2 = Color.FromArgb("#E73C7E");
        var color3 = Color.FromArgb("#EE7752");

        Action<double> forward = v => bdGradient.Background = new LinearGradientBrush()
        {
            GradientStops = new GradientStopCollection()
                {
                    new GradientStop(GetColor(color1, color2, v), (float)0.3),
                    new GradientStop(GetColor(color2, color3, v), (float)0.7),
                    new GradientStop(GetColor(color3, color1, v), (float)1.0),
                }
        };

        Action<double> backward = v => bdGradient.Background = new LinearGradientBrush()
        {
            GradientStops = new GradientStopCollection()
                {
                    new GradientStop(GetColor(color1, color2, v), (float)0.3),
                    new GradientStop(GetColor(color2, color3, v), (float)0.7),
                    new GradientStop(GetColor(color3, color1, v), (float)1.0),
                }
        };

        while (true)
        {
            bdGradient.Animate(name: "forward", callback: forward, start: 0, end: 1, length: 5000, easing: Easing.SinIn);
            await Task.Delay(5000);
            bdGradient.Animate(name: "backward", callback: backward, start: 1, end: 0, length: 5000, easing: Easing.SinIn);
            await Task.Delay(5000);
        }
    }

    private Color GetColor(Color fromColor, Color toColor, double t)
    {
        return Color.FromRgba(fromColor.Red + t * (toColor.Red - fromColor.Red),
                 fromColor.Green + t * (toColor.Green - fromColor.Green),
                 fromColor.Blue + t * (toColor.Blue - fromColor.Blue),
                 fromColor.Alpha + t * (toColor.Alpha - fromColor.Alpha));
    }

}