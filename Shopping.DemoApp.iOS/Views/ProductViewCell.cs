using Foundation;
using SDWebImage;
using Shopping.DemoApp.Models;
using System;
using System.Globalization;
using UIKit;
using System.Threading.Tasks;
using Shopping.DemoApp.Services;
using Shopping.DemoApp.Events;
using Shopping.DemoApp.iOS.Extensions;

namespace Shopping.DemoApp.iOS
{
    public partial class ProductViewCell : UICollectionViewCell
    {
        public static readonly NSString Key = new NSString("ProductViewCell");
        public static readonly UINib Nib;

		public string ItemId { get; private set; }

        static ProductViewCell()
        {
            Nib = UINib.FromName("ProductViewCell", NSBundle.MainBundle);
        }

        protected ProductViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public static ProductViewCell Create()
        {
            ProductViewCell cell = (ProductViewCell)Nib.Instantiate(null, null)[0];

            return cell;
        }

        public async void Bind(Product Product)
        {
            await ItemImageView.BindImageViewAsync(Product);

            ItemNameLabel.Text = !string.IsNullOrEmpty(Product.Name) ? Product.Name.ToUpperInvariant() : string.Empty;
            ItemDescriptionLabel.Text = Product.Description;

            var smallAttributes = new UIStringAttributes
            {
                Font = UIFont.FromName(ItemPriceLabel.Font.Name, 10f)
            };

            string priceStr = "$" + Math.Round(Product.Price);
            NSMutableAttributedString mutablePriceStr = new NSMutableAttributedString(priceStr);

            mutablePriceStr.SetAttributes(smallAttributes.Dictionary, new NSRange(0, 1));

            ItemPriceLabel.AttributedText = mutablePriceStr;
			ItemId = Product.Id;
        }
    }
}