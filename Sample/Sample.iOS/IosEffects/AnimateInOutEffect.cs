using CoreGraphics;
using Sample.Effects;
using Sample.iOS.IosEffects;
using System.Collections.Generic;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportEffect(typeof(AnimateInEffect), nameof(AnimateInEffect))]
[assembly: ExportEffect(typeof(AnimateOutEffect), nameof(AnimateOutEffect))]
[assembly: ExportEffect(typeof(StartZeroOpacityEffect), nameof(StartZeroOpacityEffect))]

namespace Sample.iOS.IosEffects
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

        private void AnimateIn(UIView view, double index, float translate = 100, string key = null)
        {
            if (_alreadyAnimated)
                return;

            if (key != null)
            {
                if (_alreadyAnimatedItems.Contains(key))
                    return;

                _alreadyAnimatedItems.Add(key);
            }

            if (view.Transform.IsIdentity)
            {
                view.Alpha = .01f;
                view.Transform = CGAffineTransform.MakeTranslation(0, translate);
            }

            UIView.AnimateNotify(
                .75f, .033f * index, .9f, 0,
                UIViewAnimationOptions.CurveEaseIn | UIViewAnimationOptions.AllowUserInteraction,
                () =>
                {
                    view.Alpha = 1f;
                    view.Transform = CGAffineTransform.MakeIdentity();
                },
                _ =>
                {
                    _alreadyAnimated = true;

                    if (key != null)
                        _alreadyAnimatedItems.Remove(key);
                });
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

        private void AnimateIn(UIView view, double index, float translate = 100, string key = null)
        {
            if (_alreadyAnimated)
                return;

            if (key != null)
            {
                if (_alreadyAnimatedItems.Contains(key))
                    return;

                _alreadyAnimatedItems.Add(key);
            }

            if (view.Transform.IsIdentity)
            {
                view.Alpha = 1;
                view.Transform = CGAffineTransform.MakeTranslation(0, 0);
            }

            UIView.AnimateNotify(
                .75f, .033f * index, .9f, 0,
                UIViewAnimationOptions.CurveEaseOut | UIViewAnimationOptions.AllowUserInteraction,
                () =>
                {
                    view.Alpha = 0f;
                    view.Transform = CGAffineTransform.MakeTranslation(0, translate);
                },
                _ =>
                {
                    _alreadyAnimated = true;

                    if (key != null)
                        _alreadyAnimatedItems.Remove(key);
                });
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