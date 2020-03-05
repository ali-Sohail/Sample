using Sample.Droid.DroidEffects;
using Sample.Effects;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

//[assembly: ResolutionGroupName("XamarinDocs")]
[assembly: ExportEffect(typeof(LabelShadowEffect), "LabelShadowEffect")]

namespace Sample.Droid.DroidEffects
{
    public class LabelShadowEffect : PlatformEffect
    {
        private Android.Widget.TextView control;
        private Android.Graphics.Color color;
        private float radius, distanceX, distanceY;

        protected override void OnAttached()
        {
            try
            {
                control = Control as Android.Widget.TextView;
                UpdateRadius();
                UpdateColor();
                UpdateOffset();
                UpdateControl();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Cannot set property on attached control. Error: ", ex.Message);
            }
        }

        protected override void OnDetached()
        {
        }

        protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
        {
            if (args.PropertyName == ShadowEffect.RadiusProperty.PropertyName)
            {
                UpdateRadius();
                UpdateControl();
            }
            else if (args.PropertyName == ShadowEffect.ColorProperty.PropertyName)
            {
                UpdateColor();
                UpdateControl();
            }
            else if (args.PropertyName == ShadowEffect.DistanceXProperty.PropertyName ||
                     args.PropertyName == ShadowEffect.DistanceYProperty.PropertyName)
            {
                UpdateOffset();
                UpdateControl();
            }
        }

        private void UpdateControl()
        {
            if (control != null)
            {
                control.SetShadowLayer(radius, distanceX, distanceY, color);
            }
        }

        private void UpdateRadius()
        {
            radius = (float)ShadowEffect.GetRadius(Element);
        }

        private void UpdateColor()
        {
            color = ShadowEffect.GetColor(Element).ToAndroid();
        }

        private void UpdateOffset()
        {
            distanceX = (float)ShadowEffect.GetDistanceX(Element);
            distanceY = (float)ShadowEffect.GetDistanceY(Element);
        }
    }
}