using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreGraphics;
using Foundation;
using ProjectOnlineMobile2.Controls;
using ProjectOnlineMobile2.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly:ExportRenderer(typeof(CustomFrame),typeof(CustomFrameRenderer))]
namespace ProjectOnlineMobile2.iOS.Renderers
{
    public class CustomFrameRenderer : FrameRenderer
    {

        protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
        {
            base.OnElementChanged(e);

            var frame = (CustomFrame)Element;

            var tappedGestureRecognizer = new UITapGestureRecognizer((tap)=> {
                tap.NumberOfTapsRequired = 1;
                if (frame.Command != null)
                {
                    if (frame.Command.CanExecute(Element.BindingContext))
                    {
                        frame.Command.Execute(frame.CommandParameter ?? Element.BindingContext);
                    }
                }
            });

            this.RemoveGestureRecognizer(tappedGestureRecognizer);
            this.AddGestureRecognizer(tappedGestureRecognizer);

            var longPressGestureRecognizer = new UILongPressGestureRecognizer((longpress)=> {
                if(frame.LongPressCommand != null)
                {
                    if (longpress.State == UIGestureRecognizerState.Began &&
                    frame.LongPressCommand.CanExecute(Element.BindingContext))
                    {
                        frame.LongPressCommand.Execute(frame.CommandParameter ?? Element.BindingContext);
                    }
                }
            });

            this.RemoveGestureRecognizer(longPressGestureRecognizer);
            this.AddGestureRecognizer(longPressGestureRecognizer);
        }

        public override void Draw(CGRect rect)
        {
            base.Draw(rect);
            Layer.ShadowRadius = 2.0f;
            Layer.ShadowColor = UIColor.Gray.CGColor;
            Layer.ShadowOffset = new CGSize(2, 2);
            Layer.ShadowOpacity = 0.8f;
            Layer.ShadowPath = UIBezierPath.FromRect(Layer.Bounds).CGPath;
            Layer.MasksToBounds = false;
        }
    }
}