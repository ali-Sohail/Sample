using Xamarin.Forms;

namespace Sample.Effects
{
    public static class AnimateInOutEffect
    {
        public static Effect ZeroOpacity => Effect.Resolve("XamarinDocs.StartZeroOpacityEffect");
        public static Effect NoScrollListView => Effect.Resolve("XamarinDocs.NoScrollListViewEffect");
        public static Effect Shadow => Effect.Resolve("XamarinDocs.ShadowEffect");

        public static Effect AnimateIn(double index = 0, float translate = 100)
        {
            IAnimateInOutEffect effect = (IAnimateInOutEffect)Effect.Resolve("XamarinDocs.AnimateInEffect");
            effect.Configure(index, translate);

            return (Effect)effect;
        }

        public static Effect AnimateOut(double index = 0, float translate = 100)
        {
            IAnimateInOutEffect effect = (IAnimateInOutEffect)Effect.Resolve("XamarinDocs.AnimateOutEffect");
            effect.Configure(index, translate);

            return (Effect)effect;
        }

        public static T StartAtZeroOpacity<T>(this T v)
        where T : View
        {
            v.Effects.Add(ZeroOpacity);
            return v;
        }

    }

    public interface IAnimateInOutEffect
    {
        void Configure(double index = 0, float translate = 100);
    }
}