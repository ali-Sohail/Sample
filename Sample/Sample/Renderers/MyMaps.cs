using Xamarin.Forms;

namespace Sample.Renderers
{
    public class MyMaps : Xamarin.Forms.Maps.Map
    {
        public bool IsTrue
        {
            get => (bool)GetValue(IsTrueProperty);
            set => SetValue(IsTrueProperty, value);
        }

        public static readonly BindableProperty IsTrueProperty =
            BindableProperty.Create(propertyName: nameof(IsTrue),
                                    returnType: typeof(bool),
                                    declaringType: typeof(MyMaps));
    }
}