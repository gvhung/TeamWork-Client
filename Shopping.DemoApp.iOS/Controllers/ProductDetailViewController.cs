using System;
using Foundation;
using TeamWork.iOS.Extensions;
using TeamWork.Models;
using TeamWork.Services;
using UIKit;

namespace TeamWork.iOS.Controllers
{
    public partial class ProductDetailViewController : UIViewController
	{
		public Product Product { get; set; }

		public ProductDetailViewController(IntPtr handle) : base(handle)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			this.AddDefaultNavigationTitle();

			LoadData();
            BuyButton.TouchUpInside += OnStartSaleRequested;
		}

        private async void LoadData()
		{
            await ItemImageView.BindImageViewAsync(Product);

            ItemTitleLabel.Text = (Product.Name ?? string.Empty).ToUpper();
			ItemDescriptionLabel.Text = Product.Description;

			var smallAttributes = new UIStringAttributes
			{
				Font = UIFont.FromName(ItemPriceLabel.Font.Name, 20f)
			};

			string priceStr = "$" + Product.Price.ToString("0.00");
			NSMutableAttributedString mutablePriceStr = new NSMutableAttributedString(priceStr);

			mutablePriceStr.SetAttributes(smallAttributes.Dictionary, new NSRange(0, 1));
			mutablePriceStr.SetAttributes(smallAttributes.Dictionary, new NSRange(priceStr.Length - 3, 3));

			ItemPriceLabel.AttributedText = mutablePriceStr;
        }

        private async void OnStartSaleRequested(object sender, EventArgs e)
        {
			await ProductDataService.Instance.StartSale(Product);
        }
    }
}