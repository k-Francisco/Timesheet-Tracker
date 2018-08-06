using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using ProjectOnlineMobile2.Controls;
using ProjectOnlineMobile2.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly:ExportRenderer(typeof(CustomListView),typeof(CustomListViewRenderer))]
namespace ProjectOnlineMobile2.iOS.Renderers
{
    public class CustomListViewRenderer : ListViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<ListView> e)
        {
            base.OnElementChanged(e);

            var vc = ((UITableViewController)ViewController);

            if (vc?.RefreshControl != null)
            {
                vc.RefreshControl.TintColor = UIColor.FromRGB(49, 117, 47);
            }
        }
    }
}