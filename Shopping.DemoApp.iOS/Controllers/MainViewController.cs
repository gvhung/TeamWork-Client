using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Foundation;
using TeamWork.Events;
using TeamWork.iOS.Extensions;
using TeamWork.Models;
using TeamWork.Services;
using UIKit;

namespace TeamWork.iOS.Controllers
{
	public partial class MainViewController : UIViewController
    {
        private const int RatingAlertDelaySeconds = 2;
        private ProductDataSource ProductSource;

        public MainViewController (IntPtr handle) : base (handle)
		{
		}

        public async override void ViewDidLoad ()
        {
            base.ViewDidLoad();

			this.AddDefaultNavigationTitle();
            CustomizeNavigationBar();

            await InitializeCollectionView();
            await LoadProduct();

            //NSTimer.CreateScheduledTimer(TimeSpan.FromSeconds(RatingAlertDelaySeconds), OnRatingAlertScheduled);
        }

        public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
            // Release any cached data, images, etc that aren't in use.
        }

        private void CustomizeNavigationBar()
        {
            NavigationController.NavigationBar.Translucent = false;
            NavigationController.NavigationBar.BarTintColor = new UIColor(1 / 255f, 31 / 255f, 79 / 255f, 1);
            NavigationController.NavigationBar.TintColor = UIColor.White;
            NavigationController.NavigationBar.ShadowImage = new UIImage();
			NavigationController.NavigationBar.SetBackgroundImage(new UIImage(), UIBarMetrics.Default);

			NavigationController.NavigationBar.TitleTextAttributes = new UIStringAttributes()
			{
				ForegroundColor = UIColor.White
			};          

            var refreshButton = new UIBarButtonItem(UIBarButtonSystemItem.Refresh, async delegate
            {
                await LoadProduct();
            });

            var spacer = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace) { Width = 50 };
            if (AzureService.Instance.AuthManager.UserIsAuthenticated)
            {
                var pauseButton = new UIBarButtonItem(UIBarButtonSystemItem.Camera, async delegate
                {
                    bool confirmed = await UserDialogs.Instance.ConfirmAsync("Time to rate the app! We'll calculate the score based on your smile.", okText: "Let's do it!");

                    if (confirmed)
                    {
                        UIViewController controller = Storyboard.InstantiateViewController(nameof(RatingMainViewController));
                        NavigationController.PushViewController(controller, true);
                    }

                });
                this.SetToolbarItems(new UIBarButtonItem[] {spacer, pauseButton}, false);
            }
            else
            {
                var loginButton = new UIBarButtonItem(UIBarButtonSystemItem.Action, delegate
                {
                    UIViewController controller = new LoginDialogViewController();
                    NavigationController.PushViewController(controller, true);

                });
                this.SetToolbarItems(new UIBarButtonItem[] { spacer, loginButton }, false);
            }

            NavigationItem.RightBarButtonItem = refreshButton;

            this.NavigationController.ToolbarHidden = false;

        }

        private async Task InitializeCollectionView()
        {
            UINib nib = UINib.FromName(ProductViewCell.Key, null);
            ProductCollectionView.RegisterNibForCell(nib, ProductViewCell.Key);

            //nib = UINib.FromName(SellArticleButtonViewCell.Key, null);
            //ProductCollectionView.RegisterNibForCell(nib, SellArticleButtonViewCell.Key);

            ProductSource = new ProductDataSource();
            ProductSource.SellRequestedCallback = OnSellRequested;
			ProductSource.ProductelectedCallback = OnProductelected;
            ProductCollectionView.Source = ProductSource;

			//await ProductDataService.Instance.Initialize();
            

            // As each item image download may occur after its cell is loaded,
            // we should reload it when that download is completed
    //        ProductDataService.Instance.MobileService.EventManager
				//				   .Subscribe<FileDownloadedEvent>(file =>
				//{
				//	ProductCollectionView.BeginInvokeOnMainThread(() =>
				//	{
				//		var targetCell = ProductCollectionView.VisibleCells
				//												.OfType<ProductViewCell>()
				//												.FirstOrDefault(c => c.ItemId == file.Name);

				//		if (targetCell != null)
				//		{
				//			NSIndexPath path = ProductCollectionView.IndexPathForCell(targetCell);
				//			ProductCollectionView.ReloadItems(new[] { path });
				//		}
				//	});
				//});
        }

		private async Task LoadProduct()
        {
            UserDialogs.Instance.ShowLoading("Laddar...");
            var data = await AzureService.Instance.ProductManager.GetItemsAsync(true);
            ProductSource.Items = data;
            ProductCollectionView.ReloadData();
            UserDialogs.Instance.HideLoading();
        }

        private async void OnRatingAlertScheduled(NSTimer timer)
        {
            timer.Invalidate();
            timer.Dispose();
            timer = null;

            bool confirmed = await UserDialogs.Instance.ConfirmAsync("Time to rate the app! We'll calculate the score based on your smile.", okText: "Let's do it!");

            if (confirmed)
            {
                UIViewController controller = Storyboard.InstantiateViewController(nameof(RatingMainViewController));
                NavigationController.PushViewController(controller, true);
            }
        }

        private async void OnSellRequested()
        {
			await AzureService.Instance.AuthManager.RequestLoginIfNecessary();

            if (AzureService.Instance.AuthManager.UserIsAuthenticated)
            {                
                UIViewController controller = Storyboard.InstantiateViewController(nameof(AddProductViewController));
                NavigationController.PushViewController(controller, true);
            }
        }

		private void OnProductelected(Product item)
		{
			ProductDetailViewController controller = Storyboard.InstantiateViewController(nameof(ProductDetailViewController)) as ProductDetailViewController;
			controller.Product = item;
			NavigationController.PushViewController(controller, true);
		}
    }
}

