using Foundation;
using Newtonsoft.Json;
using ProjectOnlineMobile2.Pages;
using ProjectOnlineMobile2.Services;
using SpevoCore.Services.Token_Service;
using System;
using System.Diagnostics;
using System.Linq;
using UIKit;
using WebKit;
using Xamarin.Forms;

namespace ProjectOnlineMobile2.iOS
{
    public partial class LoginController : UIViewController, IWKNavigationDelegate, IWKUIDelegate
    {

        string rtFa = string.Empty;
        string FedAuth = string.Empty;

        [Export("webView:didStartProvisionalNavigation:")]
        public void DidStartProvisionalNavigation(WKWebView webView, WKNavigation navigation)
        {
            activityIndicator.StartAnimating();
        }

        [Export("webView:didFinishNavigation:")]
        public void DidFinishNavigation(WKWebView webView, WKNavigation navigation)
        {
            activityIndicator.StopAnimating();
        }

        [Export("webView:didCommitNavigation:")]
        public async void DidCommitNavigation(WKWebView webView, WKNavigation navigation)
        {
            var cookies = await webView.Configuration.WebsiteDataStore.HttpCookieStore.GetAllCookiesAsync();

            if (string.IsNullOrEmpty(rtFa) && string.IsNullOrEmpty(FedAuth))
            {
                foreach (var cookie in cookies)
                {
                    if (cookie.Name.Equals("rtFa"))
                    {
                        rtFa = cookie.Name + "=" + cookie.Value;
                    }

                    if (cookie.Name.Equals("FedAuth"))
                    {
                        FedAuth = cookie.Name + "=" + cookie.Value;
                    }
                }

                if (!string.IsNullOrEmpty(rtFa) && !string.IsNullOrEmpty(FedAuth))
                {
                    try
                    {
                        activityIndicator.StopAnimating();

                        var authCookie = rtFa + "; " + FedAuth;
                        Settings.CookieString = JsonConvert.SerializeObject(authCookie);

                        var controller = Storyboard.InstantiateViewController("TabBarController") as TabBarController;
                        AppDelegate.appDelegate.Window.RootViewController = controller;
                    }
                    catch (Exception ez)
                    {
                        Debug.WriteLine("webViewLoading", ez.Message);
                    }
                }

            }
        }

        public LoginController (IntPtr handle) : base (handle)
        {
            
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            NSUrlCache.SharedCache.RemoveAllCachedResponses();
            NSHttpCookieStorage CookieStorage = NSHttpCookieStorage.SharedStorage;
            foreach (var cookie in CookieStorage.Cookies)
                CookieStorage.DeleteCookie(cookie);

            activityIndicator = new UIActivityIndicatorView();

            var url = "https://sharepointevo.sharepoint.com";
            var request = new NSMutableUrlRequest(new NSUrl(url), NSUrlRequestCachePolicy.ReloadIgnoringCacheData, 0);

            loginWebView = new WKWebView(View.Frame, new WKWebViewConfiguration());
            loginWebView.Frame = this.View.Frame;
            loginWebView.Bounds = this.View.Bounds;
            loginWebView.NavigationDelegate = this;
            loginWebView.UIDelegate = this;

            loginWebView.LoadRequest(request);

            View.AddSubview(loginWebView);

        }

        private void clearCache()
        {
            var websiteDataTypes = new NSSet<NSString>(new[]
            {
                WKWebsiteDataType.Cookies,
                WKWebsiteDataType.DiskCache,
                WKWebsiteDataType.IndexedDBDatabases,
                WKWebsiteDataType.LocalStorage,
                WKWebsiteDataType.MemoryCache,
                WKWebsiteDataType.OfflineWebApplicationCache,
                WKWebsiteDataType.SessionStorage,
                WKWebsiteDataType.WebSQLDatabases
            });

            WKWebsiteDataStore.DefaultDataStore.FetchDataRecordsOfTypes(websiteDataTypes, (NSArray records) =>
            {
                for (nuint i = 0; i < records.Count; i++)
                {
                    var record = records.GetItem<WKWebsiteDataRecord>(i);

                    WKWebsiteDataStore.DefaultDataStore.RemoveDataOfTypes(record.DataTypes,
                        new[] { record }, null );
                }
            });
        }
    }
}