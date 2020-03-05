using System.ComponentModel;
using Xamarin.Forms;

namespace Sample.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class AboutPage : ContentPage
    {
        private readonly Xamarin.Essentials.DisplayInfo display;
        private double x, y;

        private bool check = false;

        public AboutPage()
        {
            InitializeComponent();
            display = Xamarin.Essentials.DeviceDisplay.MainDisplayInfo;
        }

        private async void PanUpdated(object sender, PanUpdatedEventArgs e)
        {
            check = !check;
            if (check)
            {
                return;
            }
            VisualElement element = sender as VisualElement;

            System.Diagnostics.Debug.WriteLine($"Gesture Id{ e.GestureId}\tStatusType {e.StatusType}\t Total X{ e.TotalX}\tTotal Y {e.TotalY}");

            switch (e.StatusType)
            {
                case GestureStatus.Running:
                    // Translate and ensure we don't pan beyond the wrapped user interface element bounds.
                    //element.TranslationX = Math.Max(Math.Min(0, x + e.TotalX), -Math.Abs(element.Width - display.Width));
                    //element.TranslationY = Math.Max(Math.Min(0, y + e.TotalY), -Math.Abs(element.Height - display.Height));
                    element.TranslationX = e.TotalX;
                    element.TranslationY = e.TotalY;
                    System.Diagnostics.Debug.WriteLine($"element TranslationX{element.TranslationX}\telement TranslationY {element.TranslationY}");
                    break;

                case GestureStatus.Completed:
                    // Store the translation applied during the pan
                    x = element.TranslationX;
                    y = element.TranslationY;
                    await element.TranslateTo(0, 0, 1000, Easing.BounceOut);
                    System.Diagnostics.Debug.WriteLine($"Completed TranslationX{element.TranslationX}\tCompleted TranslationY {element.TranslationY}");
                    break;
            }
        }

        private double Interpolation(double interpole)
        {
            double tension = 2.0 * 1.5;
            double result = 0;

            if (interpole < 0.5)
                result = 0.5 * a(interpole * 2.0, tension);
            else
            {
                result = 0.5 * (o(interpole * 2.0 - 2.0, tension) + 2.0);
            }
            return result;
        }

        private double a(double t, double s)
        {
            return t * t * ((s + 1) * t - s);
        }

        private double o(double t, double s)
        {
            return t * t * ((s + 1) * t + s);
        }

        private async void TouchEffectAction(object sender, Effects.TouchActionEventArgs args)
        {
            System.Diagnostics.Debug.WriteLine($"Location X :{args.Location.X}\t Location Y :{args.Location.Y}");

            switch (args.Type)
            {
                case Sample.Effects.TouchActionType.Entered:
                    await label.TranslateTo(-(label.X + label.Width / 2 - args.Location.X), -(label.Y + label.Height / 2 - args.Location.Y), 16);
                    break;
                case Sample.Effects.TouchActionType.Pressed:
                    await label.TranslateTo(-(label.X + label.Width / 2 - args.Location.X), -(label.Y + label.Height / 2 - args.Location.Y), 800);
                    break;
                case Sample.Effects.TouchActionType.Moved:
                    await label.TranslateTo(-(label.X + label.Width / 2 - args.Location.X), -(label.Y + label.Height / 2 - args.Location.Y), 16);
                    break;
                case Sample.Effects.TouchActionType.Released:
                    await label.TranslateTo(-(label.X + label.Width / 2 - args.Location.X), -(label.Y + label.Height / 2 - args.Location.Y), 16);
                    break;
                case Sample.Effects.TouchActionType.Exited:
                    await label.TranslateTo(-(label.X + label.Width / 2 - args.Location.X), -(label.Y + label.Height / 2 - args.Location.Y), 16);
                    break;
                case Sample.Effects.TouchActionType.Cancelled:
                    await label.TranslateTo(-(label.X + label.Width / 2 - args.Location.X), -(label.Y + label.Height / 2 - args.Location.Y), 1);
                    break;
                default:
                    break;
            }
            //await label.TranslateTo((label.X - args.Location.X), (label.Y - args.Location.Y), 16);
            //await label.TranslateTo(-(label.X - args.Location.X), -(label.Y - args.Location.Y), 16);
            //await label.TranslateTo(-(label.X + label.Width / 2 - args.Location.X), -(label.Y + label.Height / 2 - args.Location.Y), 16);
        }


    }
}