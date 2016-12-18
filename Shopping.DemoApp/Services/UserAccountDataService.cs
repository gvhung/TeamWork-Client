namespace TeamWork.Services
{
    using Acr.UserDialogs;
    using Microsoft.WindowsAzure.MobileServices;
    using Microsoft.WindowsAzure.MobileServices.Sync;
    using TeamWork.Models;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Microsoft.WindowsAzure.MobileServices.Files;
    using System.Linq;

    public class UserAccountDataService
    {        
        private static UserAccountDataService instance = new UserAccountDataService();

        public static UserAccountDataService Instance
        {
            get
            {
                return instance;
            }
        }

        public MobileServiceClient MobileService { get; private set; }

        private UserAccountDataService()
        {
            Initialize();
        }

        private void Initialize()
        {
            //Create our client
            this.MobileService = new MobileServiceClient(AppSettings.ApiAddress);
            //We add MobileServiceFileJsonConverter to the list of available converters to avoid an internal that occurs randomly
            this.MobileService.SerializerSettings.Converters.Add(new MobileServiceFileJsonConverter(this.MobileService));
        }

        internal async Task<UserAccount> CreateAccountIfNotExists(MobileServiceUser mobileUser)
        {
            UserAccount userAccount = new UserAccount();
            userAccount.AccessToken = mobileUser.MobileServiceAuthenticationToken;
            userAccount.Id = mobileUser.UserId;

            var accountTable = await this.MobileService.GetTable<UserAccount>().ToListAsync();
            var account = accountTable.FirstOrDefault();

            if (account == null)
            {
                await this.MobileService.GetTable<UserAccount>().InsertAsync(userAccount);
                account = userAccount;
            }
            
            return account;
        }

    }
}
