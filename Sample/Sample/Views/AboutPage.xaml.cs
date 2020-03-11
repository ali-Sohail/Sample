using Sample.Effects;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
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
        private bool isLabelTeal = false;
        private bool check = false;
        private double move = 0;
        private double stackHeight = 0;
        private bool IsNavBarVisible = true;


        public AboutPage()
        {
            InitializeComponent();
            display = Xamarin.Essentials.DeviceDisplay.MainDisplayInfo;

        }

        protected override void OnAppearing()
        {
            AnimationIn(label);
            AnimationIn(button);
            stackHeight = -(navStack.Height + navStack.Margin.VerticalThickness);
            base.OnAppearing();
        }

        protected override async void OnDisappearing()
        {
            AnimationOut(label);
            AnimationOut(button);
            await Task.Delay(500);
            base.OnDisappearing();
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
            {
                result = 0.5 * a(interpole * 2.0, tension);
            }
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

            await HideShowNavBar(args);

            //switch (args.Type)
            //{
            //    case Sample.Effects.TouchActionType.Entered:
            //        System.Diagnostics.Debug.WriteLine("Entered");
            //        await label.TranslateTo(-(label.X + label.Width / 2 - args.Location.X), -(label.Y + label.Height / 2 - args.Location.Y), 10);
            //        break;

            //    case Sample.Effects.TouchActionType.Pressed:
            //        System.Diagnostics.Debug.WriteLine("Pressed");
            //        await label.TranslateTo(-(label.X + label.Width / 2 - args.Location.X), -(label.Y + label.Height / 2 - args.Location.Y), 800);
            //        break;

            //    case Sample.Effects.TouchActionType.Moved:
            //        System.Diagnostics.Debug.WriteLine("Moved");
            //        await label.TranslateTo(-(label.X + label.Width / 2 - args.Location.X), -(label.Y + label.Height / 2 - args.Location.Y), 16);
            //        break;

            //    case Sample.Effects.TouchActionType.Released:
            //        System.Diagnostics.Debug.WriteLine("Released");
            //        await label.TranslateTo(-(label.X + label.Width / 2 - args.Location.X), -(label.Y + label.Height / 2 - args.Location.Y), 200);
            //        break;

            //    case Sample.Effects.TouchActionType.Exited:
            //        System.Diagnostics.Debug.WriteLine("Exited");
            //        await label.TranslateTo(-(label.X + label.Width / 2 - args.Location.X), -(label.Y + label.Height / 2 - args.Location.Y), 1);
            //        break;

            //    case Sample.Effects.TouchActionType.Cancelled:
            //        System.Diagnostics.Debug.WriteLine("Cancelled");
            //        await label.TranslateTo(-(label.X + label.Width / 2 - args.Location.X), -(label.Y + label.Height / 2 - args.Location.Y), 1);
            //        break;

            //    default:
            //        break;
            //}
            //await label.TranslateTo((label.X - args.Location.X), (label.Y - args.Location.Y), 16);
            //await label.TranslateTo(-(label.X - args.Location.X), -(label.Y - args.Location.Y), 16);
            //await label.TranslateTo(-(label.X + label.Width / 2 - args.Location.X), -(label.Y + label.Height / 2 - args.Location.Y), 16);
        }

        private void OnButtonClicked(object sender, EventArgs args)
        {
            if (isLabelTeal)
            {
                Color color = Color.Default;
                switch (Device.RuntimePlatform)
                {
                    case Device.iOS:
                        color = Color.Black;
                        break;

                    case Device.Android:
                        color = Color.White;
                        break;

                    case Device.UWP:
                        color = Color.Red;
                        break;
                }

                ShadowEffect.SetColor(label, color);
                isLabelTeal = false;
            }
            else
            {
                ShadowEffect.SetColor(label, Color.Teal);
                isLabelTeal = true;
            }
        }

        private async Task HideShowNavBar(Effects.TouchActionEventArgs args)
        {
            stackHeight = -(navStack.Height + navStack.Margin.VerticalThickness);
            switch (args.Type)
            {
                case Sample.Effects.TouchActionType.Entered:
                    break;

                case Sample.Effects.TouchActionType.Pressed:
                    move = args.Location.Y;
                    System.Diagnostics.Debug.WriteLine($"Pressed : { args.Location.Y}\t Stack Y : {navStack.TranslationY}");
                    break;

                case Sample.Effects.TouchActionType.Moved:

                    System.Diagnostics.Debug.WriteLine($"Moved :{Math.Min(Math.Max(stackHeight, args.Location.Y) - move, 0)}");
                    System.Diagnostics.Debug.WriteLine($"Moved Args : { args.Location.Y}\t Stack Y : {navStack.TranslationY}");
                    await navStack.TranslateTo((navStack.X - navStack.Margin.HorizontalThickness / 2), Math.Min(args.Location.Y - move, 0), 16);
                    break;

                case Sample.Effects.TouchActionType.Released:
                    System.Diagnostics.Debug.WriteLine($"Released : { args.Location.Y}\t Stack Y : {navStack.TranslationY}");

                    if (Math.Min(args.Location.Y - move, 0) < 0)
                    {
                        await navStack.TranslateTo((navStack.X - navStack.Margin.HorizontalThickness / 2), stackHeight, 200);
                        IsNavBarVisible = false;
                    }
                    else
                    {
                        await navStack.TranslateTo((navStack.X - navStack.Margin.HorizontalThickness / 2), 0, 200);
                        IsNavBarVisible = true;
                    }
                    break;

                case Sample.Effects.TouchActionType.Exited:
                    break;

                case Sample.Effects.TouchActionType.Cancelled:
                    break;

                default:
                    break;
            }
        }

        public static void AnimationIn(VisualElement view, int index = 0)
        {
            try
            {
                if (view != null)
                {
                    view.Effects.Add(AnimateInOutEffect.AnimateIn(index));
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        public static void AnimationOut(VisualElement view, int index = 0)
        {
            try
            {
                if (view != null)
                {
                    view.Effects.Add(AnimateInOutEffect.AnimateOut(index));
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }
    }
}