using Android.Views.Animations;
using Sample.Droid.DroidEffects;
using Sample.Effects;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportEffect(typeof(AnimateInEffect), nameof(AnimateInEffect))]
[assembly: ExportEffect(typeof(AnimateOutEffect), nameof(AnimateOutEffect))]
[assembly: ExportEffect(typeof(StartZeroOpacityEffect), nameof(StartZeroOpacityEffect))]

namespace Sample.Droid.DroidEffects
{
    public class AnimateInEffect : PlatformEffect, IAnimateInOutEffect
    {
        private double _index;
        private float _translate;
        private bool _alreadyAnimated;
        private readonly HashSet<string> _alreadyAnimatedItems = new HashSet<string>();

        public void Configure(double index, float translate = 100)
        {
            _index = index;
            _translate = translate;
        }

        protected override void OnAttached()
        {
            var view = Container ?? Control;

            AnimateIn(view, _index, _translate);
        }

        private void AnimateIn(Android.Views.View view, double index, float translate = 100, string key = null)
        {
            if (_alreadyAnimated)
                return;

            if (key != null)
            {
                if (_alreadyAnimatedItems.Contains(key))
                    return;

                _alreadyAnimatedItems.Add(key);
            }

            if (_alreadyAnimated)
                return;
            view.Alpha = .01f;
            view.TranslationY = translate;

            view.Animate()
                .TranslationYBy(-translate)
                .Alpha(1)
                .SetDuration(300)
                .SetStartDelay((long)(33 * index))
                .SetInterpolator(new DecelerateInterpolator());
        }

        protected override void OnDetached()
        {
        }
    }

    public class AnimateOutEffect : PlatformEffect, IAnimateInOutEffect
    {
        private double _index;
        private float _translate;
        private bool _alreadyAnimated;
        private readonly HashSet<string> _alreadyAnimatedItems = new HashSet<string>();

        public void Configure(double index, float translate = 100)
        {
            _index = index;
            _translate = translate;
        }

        protected override void OnAttached()
        {
            var view = Container ?? Control;

            AnimateIn(view, _index, _translate);
        }

        private void AnimateIn(Android.Views.View view, double index, float translate = 100, string key = null)
        {
            if (_alreadyAnimated)
                return;

            if (key != null)
            {
                if (_alreadyAnimatedItems.Contains(key))
                    return;

                _alreadyAnimatedItems.Add(key);
            }

            if (_alreadyAnimated)
                return;

            view.Alpha = 1f;
            view.TranslationY = translate;

            view.Animate()
                .TranslationYBy(translate)
                .Alpha(0)
                .SetDuration(300)
                .SetStartDelay((long)(33 * index))
                .SetInterpolator(new DecelerateInterpolator());
        }

        protected override void OnDetached()
        {
        }
    }

    public class StartZeroOpacityEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            var view = Container ?? Control;

            view.Alpha = 0;
        }

        protected override void OnDetached()
        {
        }
    }
}