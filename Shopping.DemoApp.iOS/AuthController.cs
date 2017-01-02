using Foundation;
using System;
using UIKit;

namespace TeamWork.iOS
{
    public partial class AuthController : UIViewController
    {
        public AuthController (IntPtr handle) : base (handle)
        {
            var webView = new UIWebView(View.Bounds);
            View.AddSubview(webView);
            var url = AppSettings.ApiAddress; // NOTE: https secure request
            webView.LoadRequest(new NSUrlRequest(new NSUrl(url + "Account/Login")));
            webView.ScalesPageToFit = true;
        }
    }
}