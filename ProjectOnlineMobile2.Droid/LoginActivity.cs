﻿using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using Android.Webkit;
using Java.Lang;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using SpevoCore.Services;

namespace ProjectOnlineMobile2.Droid
{
    [Activity]
    public class LoginActivity : AppCompatActivity
    {
        public TokenService tokenService;
        private WebView _webView;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            AppCenter.Start("4078bace-6237-4483-aaaf-42d97bfe4564", typeof(Analytics), typeof(Crashes));

            SetContentView(Resource.Layout.activity_login);
            this.Window.AddFlags(WindowManagerFlags.Fullscreen);

            tokenService = new TokenService();

            if (tokenService.IsAlreadyLoggedIn())
            {
                GoToLandingPage();
            }

            _webView = FindViewById<WebView>(Resource.Id.wbLogin);
            _webView.Settings.JavaScriptEnabled = true;
            _webView.Settings.DomStorageEnabled = true;
            _webView.ClearCache(true);
            CookieManager.Instance.RemoveAllCookie();
            CookieManager.Instance.RemoveSessionCookie();
            _webView.LoadUrl("https://sharepointevo.sharepoint.com");
            _webView.SetWebViewClient(new CustomWebView(this, tokenService));
        }

        public void GoToLandingPage()
        {
            Intent intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
            this.Finish();
        }

        
    }
}