﻿using CoreGraphics;
using Foundation;
using Sample.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ResolutionGroupName("XamarinDocs")]
[assembly: ExportEffect(typeof(Sample.iOS.IosEffects.TouchEffect), "TouchEffect")]

namespace Sample.iOS.IosEffects
{
    public class TouchEffect : PlatformEffect
    {
        private UIView view;
        private TouchRecognizer touchRecognizer;

        protected override void OnAttached()
        {
            // Get the iOS UIView corresponding to the Element that the effect is attached to
            view = Control == null ? Container : Control;

            // Uncomment this line if the UIView does not have touch enabled by default
            //view.UserInteractionEnabled = true;

            // Get access to the TouchEffect class in the .NET Standard library
            Effects.TouchEffect effect = (Effects.TouchEffect)Element.Effects.FirstOrDefault(e => e is Effects.TouchEffect);

            if (effect != null && view != null)
            {
                // Create a TouchRecognizer for this UIView
                touchRecognizer = new TouchRecognizer(Element, view, effect);
                view.AddGestureRecognizer(touchRecognizer);
            }
        }

        protected override void OnDetached()
        {
            if (touchRecognizer != null)
            {
                // Clean up the TouchRecognizer object
                touchRecognizer.Detach();

                // Remove the TouchRecognizer from the UIView
                view.RemoveGestureRecognizer(touchRecognizer);
            }
        }
    }

    internal class TouchRecognizer : UIGestureRecognizer
    {
        private readonly Element element;        // Forms element for firing events
        private readonly UIView view;            // iOS UIView
        private readonly Effects.TouchEffect touchEffect;
        private bool capture;

        private static readonly Dictionary<UIView, TouchRecognizer> viewDictionary =
            new Dictionary<UIView, TouchRecognizer>();

        private static readonly Dictionary<long, TouchRecognizer> idToTouchDictionary =
            new Dictionary<long, TouchRecognizer>();

        public TouchRecognizer(Element element, UIView view, Effects.TouchEffect touchEffect)
        {
            this.element = element;
            this.view = view;
            this.touchEffect = touchEffect;

            viewDictionary.Add(view, this);
        }

        public void Detach()
        {
            viewDictionary.Remove(view);
        }

        // touches = touches of interest; evt = all touches of type UITouch
        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);

            foreach (UITouch touch in touches.Cast<UITouch>())
            {
                long id = touch.Handle.ToInt64();
                FireEvent(this, id, TouchActionType.Pressed, touch, true);

                if (!idToTouchDictionary.ContainsKey(id))
                {
                    idToTouchDictionary.Add(id, this);
                }
            }

            // Save the setting of the Capture property
            capture = touchEffect.Capture;
        }

        public override void TouchesMoved(NSSet touches, UIEvent evt)
        {
            base.TouchesMoved(touches, evt);

            foreach (UITouch touch in touches.Cast<UITouch>())
            {
                long id = touch.Handle.ToInt64();

                if (capture)
                {
                    FireEvent(this, id, TouchActionType.Moved, touch, true);
                }
                else
                {
                    CheckForBoundaryHop(touch);

                    if (idToTouchDictionary[id] != null)
                    {
                        FireEvent(idToTouchDictionary[id], id, TouchActionType.Moved, touch, true);
                    }
                }
            }
        }

        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            base.TouchesEnded(touches, evt);

            foreach (UITouch touch in touches.Cast<UITouch>())
            {
                long id = touch.Handle.ToInt64();

                if (capture)
                {
                    FireEvent(this, id, TouchActionType.Released, touch, false);
                }
                else
                {
                    CheckForBoundaryHop(touch);

                    if (idToTouchDictionary[id] != null)
                    {
                        FireEvent(idToTouchDictionary[id], id, TouchActionType.Released, touch, false);
                    }
                }
                idToTouchDictionary.Remove(id);
            }
        }

        public override void TouchesCancelled(NSSet touches, UIEvent evt)
        {
            base.TouchesCancelled(touches, evt);

            foreach (UITouch touch in touches.Cast<UITouch>())
            {
                long id = touch.Handle.ToInt64();

                if (capture)
                {
                    FireEvent(this, id, TouchActionType.Cancelled, touch, false);
                }
                else if (idToTouchDictionary[id] != null)
                {
                    FireEvent(idToTouchDictionary[id], id, TouchActionType.Cancelled, touch, false);
                }
                idToTouchDictionary.Remove(id);
            }
        }

        private void CheckForBoundaryHop(UITouch touch)
        {
            long id = touch.Handle.ToInt64();

            // TODO: Might require converting to a List for multiple hits
            TouchRecognizer recognizerHit = null;

            foreach (UIView view in viewDictionary.Keys)
            {
                CGPoint location = touch.LocationInView(view);

                if (new CGRect(new CGPoint(), view.Frame.Size).Contains(location))
                {
                    recognizerHit = viewDictionary[view];
                }
            }
            if (recognizerHit != idToTouchDictionary[id])
            {
                if (idToTouchDictionary[id] != null)
                {
                    FireEvent(idToTouchDictionary[id], id, TouchActionType.Exited, touch, true);
                }
                if (recognizerHit != null)
                {
                    FireEvent(recognizerHit, id, TouchActionType.Entered, touch, true);
                }
                idToTouchDictionary[id] = recognizerHit;
            }
        }

        private void FireEvent(TouchRecognizer recognizer, long id, TouchActionType actionType, UITouch touch, bool isInContact)
        {
            // Convert touch location to Xamarin.Forms Point value
            CGPoint cgPoint = touch.LocationInView(recognizer.View);
            Point xfPoint = new Point(cgPoint.X, cgPoint.Y);

            // Get the method to call for firing events
            Action<Element, TouchActionEventArgs> onTouchAction = recognizer.touchEffect.OnTouchAction;

            // Call that method
            onTouchAction(recognizer.element,
                new TouchActionEventArgs(id, actionType, xfPoint, isInContact));
        }
    }
}