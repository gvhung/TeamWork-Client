using Acr.UserDialogs;
using Foundation;
using MonoTouch.Dialog;
using System;
using TeamWork.Models;
using UIKit;

namespace TeamWork.iOS
{
    public partial class LoginViewController : DialogViewController
    {        
        public LoginViewController (IntPtr handle) : base (handle)
        {
           
        }

        public override void ViewDidLoad()
        {
            EntryElement login, password;
            var emailLabel = new NSString("E-post", NSStringEncoding.UTF8);
            var passwordLabel = new NSString("Lösenord", NSStringEncoding.UTF8);
            var userInfoLabel = new NSString("Användaruppgifter", NSStringEncoding.UTF8);

            Root = new RootElement("Logga in")
            {
               new Section (userInfoLabel){
                  (login = new EntryElement (emailLabel, "andreas.wahlgren@consid.se", "andreas.wahlgren@consid.se")),
                  (password = new EntryElement (passwordLabel, "Zaq1xsw2", "Zaq1xsw2", true))
                 },
                  new Section () {
                    new StringElement ("Login", async delegate{
                        var loginModel = new LoginModel();
                        loginModel.Email = login.Value;
                        loginModel.Password = password.Value;
                        var result = await AzureService.Instance.AuthManager.Login(loginModel);
                        if (result.Success)
                        {
                            NavigationController.PopToRootViewController(true);
                        }
                        else
                        {
                            UserDialogs.Instance.Alert(result.ErrorMessage, okText: "Ok");
                        }
                    })
                 }
            };
        }
    }
}