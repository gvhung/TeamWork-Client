using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.WindowsAzure.MobileServices;
using TeamWork.Models;
using Acr.UserDialogs;

namespace TeamWork
{
    public class AuthManager 
	{
        public bool UserIsAuthenticated { get; private set; }
        MobileServiceUser mobileUser = null;
        public async Task<Models.LoginResult> Login(LoginModel loginModel)
        {
            var result = await AzureService.Instance.Client.InvokeApiAsync<LoginModel, Models.LoginResult>("login", loginModel);
            return result;

        }

        public Task<DateTime?> Register(string username, string password)
        {
            return new Task<DateTime?>(() => {
                var qs = new Dictionary<string, string>();
                qs.Add("email", username);
                qs.Add("password", password);
                var dateTime = AzureService.Instance.Client.InvokeApiAsync("Register", null, HttpMethod.Post, qs).Result;
                return (DateTime)dateTime.Root;
            });
        }

        public async Task<bool> RequestLoginIfNecessary(string message = "För att kunna göra det här måste du vara inloggad")
        {
            if (!UserIsAuthenticated)
            {
                string result = await UserDialogs.Instance.ActionSheetAsync(message, "Avbryt", null, buttons: new[] { "Facebook" });

                try
                {

                    switch (result)
                    {
                        case "Facebook":
                            mobileUser = await LoginWithProviderAsync(MobileServiceAuthenticationProvider.Facebook);
                            break;
                        case "Twitter":
                            mobileUser = await LoginWithProviderAsync(MobileServiceAuthenticationProvider.Twitter);
                            break;
                    }

                    UserIsAuthenticated = mobileUser != null;
                }
                catch (Exception ex)
                {
                    //Debug.WriteLine(ex);
                }
            }
            
            return UserIsAuthenticated;
        }

        private Task<MobileServiceUser> LoginWithProviderAsync(MobileServiceAuthenticationProvider provider)
        {
            var window = UIKit.UIApplication.SharedApplication.KeyWindow;
            var vc = window.RootViewController;

            // take top presented view controller
            while (vc.PresentedViewController != null)
            {
                vc = vc.PresentedViewController;
            }

            return AzureService.Instance.Client.LoginAsync(vc, provider);
        }

        internal void LogOut()
        {
            AzureService.Instance.Client.LogoutAsync();
        }
    }
}