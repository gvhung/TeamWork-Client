using System;
using System.Collections.Generic;
using System.Linq;

using UIKit;
using MonoTouch.Dialog;
using Acr.UserDialogs;
using TeamWork.Models;
using Foundation;

namespace TeamWork.iOS.Controllers
{
    public partial class LoginDialogViewController : DialogViewController
    {
        public LoginDialogViewController() : base(UITableViewStyle.Grouped, null)
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