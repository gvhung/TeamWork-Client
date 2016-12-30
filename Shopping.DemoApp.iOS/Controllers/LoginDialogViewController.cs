using System;
using System.Collections.Generic;
using System.Linq;

using UIKit;
using MonoTouch.Dialog;
using Acr.UserDialogs;
using TeamWork.Models;

namespace TeamWork.iOS.Controllers
{
    public partial class LoginDialogViewController : DialogViewController
    {
        public LoginDialogViewController() : base(UITableViewStyle.Grouped, null)
        {
            EntryElement login, password;

            Root = new RootElement("Logga in")
            {
               new Section ("Användaruppgifter"){
                  (login = new EntryElement ("E-post", "andreas.wahlgren@consid.se", "andreas.wahlgren@consid.se")),
                  (password = new EntryElement ("Lösenord", "Zaq1xsw2", "Zaq1xsw2", true))
                 },
                  new Section () {
                    new StringElement ("Login", async delegate{
                        var loginModel = new LoginModel();
                        loginModel.Email = login.Value;
                        loginModel.Password = password.Value;
                        var result = await AzureService.Instance.AuthManager.Login(loginModel);
                        if (result.Success)
                        {

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