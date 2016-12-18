namespace TeamWork
{
    using Acr.UserDialogs;
    using Microsoft.WindowsAzure.MobileServices;
    using Models;
    using Services;
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    public class AuthenticationService
    {
        private static AuthenticationService instance = new AuthenticationService();

        public bool UserIsAuthenticated { get; private set; }
        MobileServiceUser mobileUser = null;
        public static AuthenticationService Instance
        {
            get
            {
                return instance;
            }
        }

        public UserAccount User
        {
            get
            {
                UserAccount account = new UserAccount();
                return account;
            }
            
        }

        private AuthenticationService()
        {
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
                    Debug.WriteLine(ex);
                }
            }
			if(mobileUser !=null)
            	await UserAccountDataService.Instance.CreateAccountIfNotExists(mobileUser);
            return UserIsAuthenticated;
        }

#if __IOS__

        private Task<MobileServiceUser> LoginWithProviderAsync(MobileServiceAuthenticationProvider provider)
        {
            var window = UIKit.UIApplication.SharedApplication.KeyWindow;
            var vc = window.RootViewController;

            // take top presented view controller
            while (vc.PresentedViewController != null)
            {
                vc = vc.PresentedViewController;
            }

            return ProductDataService.Instance.MobileService.LoginAsync(vc, provider);
        }

#elif __ANDROID__

        private Task<MobileServiceUser> LoginWithProviderAsync(MobileServiceAuthenticationProvider provider)
        {
            return ProductDataService.Instance.MobileService.LoginAsync(Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity, provider);
        }

#elif WINDOWS_UWP

        //// In UWP we need to be logged in to subscribe to push notifications
        private async Task<MobileServiceUser> LoginWithProviderAsync(MobileServiceAuthenticationProvider provider)
        {
            var serviceUser = await ProductDataService.Instance.MobileService.LoginAsync(provider);

            return serviceUser;
        }
#endif

    }
}