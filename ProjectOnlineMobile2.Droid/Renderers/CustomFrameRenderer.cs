using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ProjectOnlineMobile2.Controls;
using ProjectOnlineMobile2.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly:ExportRenderer(typeof(CustomFrame),typeof(CustomFrameRenderer))]
namespace ProjectOnlineMobile2.Droid.Renderers
{
    public class CustomFrameRenderer : FrameRenderer
    {
        public CustomFrameRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                var frame = (CustomFrame)Element;

                this.Clickable = true;
                this.Click += delegate {
                    if(frame.Command != null)
                    {
                        if (frame.Command.CanExecute(Element.BindingContext))
                        {
                            frame.Command.Execute(frame.CommandParameter ?? Element.BindingContext);
                        }
                    }
                };

                this.LongClickable = true;
                this.LongClick += delegate {
                    if (frame.LongPressCommand != null)
                    {
                        if (frame.LongPressCommand.CanExecute(Element.BindingContext))
                        {
                            frame.LongPressCommand.Execute(frame.CommandParameter ?? Element.BindingContext);
                        }
                    }
                };

                ViewGroup.SetBackgroundResource(Resource.Drawable.frame_shadow);
            }
        }
    }
}