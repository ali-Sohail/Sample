using CoreGraphics;
using Foundation;
using Sample.Effects;
using Sample.iOS.IosEffects;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

//[assembly: ResolutionGroupName("XamarinDocs")]
[assembly: ExportEffect(typeof(LabelShadowEffect), "LabelShadowEffect")]

namespace Sample.iOS.IosEffects
{
    [Preserve(AllMembers = true)]
    public class LabelShadowEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            try
            {
                UpdateRadius();
                UpdateColor();
                UpdateOffset();
                Control.Layer.ShadowOpacity = 1.0f;
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
            }
            else if (args.PropertyName == ShadowEffect.ColorProperty.PropertyName)
            {
                UpdateColor();
            }
            else if (args.PropertyName == ShadowEffect.DistanceXProperty.PropertyName ||
                     args.PropertyName == ShadowEffect.DistanceYProperty.PropertyName)
            {
                UpdateOffset();
            }
        }

        private void UpdateRadius()
        {
            Control.Layer.CornerRadius = (nfloat)ShadowEffect.GetRadius(Element);
        }

        private void UpdateColor()
        {
            Control.Layer.ShadowColor = ShadowEffect.GetColor(Element).ToCGColor();
        }

        private void UpdateOffset()
        {
            Control.Layer.ShadowOffset = new CGSize((double)ShadowEffect.GetDistanceX(Element), (double)ShadowEffect.GetDistanceY(Element));
        }
    }
}