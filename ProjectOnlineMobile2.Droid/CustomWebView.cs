using Android.Graphics;
using Android.Webkit;
using SpevoCore.Services;

namespace ProjectOnlineMobile2.Droid
{
    public class CustomWebView : WebViewClient
    {
        private LoginActivity _loginActivity;
        private TokenService _tokenService;

        public CustomWebView(LoginActivity loginActivity, TokenService tokenService)
        {
            _loginActivity = loginActivity;
            _tokenService = tokenService;
        }

        bool doneNavigating = false;
        public override void OnPageStarted(WebView view, string url, Bitmap favicon)
        {
            base.OnPageStarted(view, url, favicon);

            bool doneSavingTokens;

            CookieManager cookieManager = CookieManager.Instance;
            cookieManager.SetAcceptCookie(true);

            doneSavingTokens = _tokenService.SaveCookies(cookieManager.GetCookie("https://sharepointevo.sharepoint.com/SitePages/home.aspx?AjaxDelta=1"));

            if(doneSavingTokens && !doneNavigating)
            {
                _loginActivity.GoToLandingPage();
                doneNavigating = true;
            }
        }
    }
}