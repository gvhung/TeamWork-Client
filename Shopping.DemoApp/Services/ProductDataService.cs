namespace TeamWork.Services
{
    using Acr.UserDialogs;
    using Microsoft.WindowsAzure.MobileServices;
    using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
    using Microsoft.WindowsAzure.MobileServices.Sync;
    using TeamWork.Models;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Microsoft.WindowsAzure.MobileServices.Files;
    using System.IO;
    using System.Linq;

    public class ProductDataService
    {
        private IMobileServiceTable<Product> ProductTable;
        private static ProductDataService instance = new ProductDataService();

        public static ProductDataService Instance
        {
            get
            {
                return instance;
            }
        }

        public MobileServiceClient MobileService { get; private set; }

        private ProductDataService()
        {
        }

        public async Task Initialize()
        {
            const string path = "syncstore.db";

            //Create our client
            this.MobileService = new MobileServiceClient(AppSettings.ApiAddress);
            //We add MobileServiceFileJsonConverter to the list of available converters to avoid an internal that occurs randomly
            this.MobileService.SerializerSettings.Converters.Add(new MobileServiceFileJsonConverter(this.MobileService));

            //setup our local sqlite store and intialize our table
            //var store = new MobileServiceSQLiteStore(path);
            //store.DefineTable<Product>();

            //Get our sync table that will call out to azure
            this.ProductTable = this.MobileService.GetTable<Product>();

            // Add images handler
            //this.MobileService.InitializeFileSyncContext(new ImagesFileSyncHandler(this.ProductTable), store);

            //await this.MobileService.SyncContext.InitializeAsync(store, StoreTrackingOptions.NotifyLocalAndServerOperations);
            //await this.MobileService.SyncContext.InitializeAsync(store, StoreTrackingOptions.None);

        }

        public async Task<IEnumerable<Product>> GetProduct()
        {
            //await this.SyncProduct();

            return await this.ProductTable.OrderByDescending(c => c.CreatedAt).ToEnumerableAsync();
        }

        //public async Task SyncProduct()
        //{
        //    try
        //    {
        //        await MobileService.SyncContext.PushAsync();
        //        //await ProductTable.PushFileChangesAsync();

        //        await ProductTable.PullAsync("allProduct", this.ProductTable.CreateQuery());
        //    }

        //    catch (Exception e)
        //    {
        //        Debug.WriteLine(@"Sync Failed: {0}", e.Message);
        //    }
        //}


        public async Task AddItemAsync(Product item, string imagePath)
        {
            await ProductTable.InsertAsync(item);

            //string targetPath = await FileHelper.CopyProductFileAsync(item.Id, imagePath);
            //await ProductTable.AddFileAsync(item, Path.GetFileName(targetPath));

            //await SyncProduct();
        }

        public async Task<MobileServiceFile> GetItemPhotoAsync(Product item)
        {
            IEnumerable<MobileServiceFile> files = await this.ProductTable.GetFilesAsync(item);

            return files.FirstOrDefault();
        }

        
        public async Task StartSale(Product item)
        {
            try
            {
                bool succeeded = true;//await this.MobileService.InvokeApiAsync<Product, bool>("buy", item);

                if (succeeded)
                {
                    await UserDialogs.Instance.AlertAsync("Underbart! Nu drar vi igång försäljningen!");
                }
            }

            catch (Exception e)
            {
                Debug.WriteLine(@"Unexpected error {0}", e.Message);
            }
        }
    }
}
