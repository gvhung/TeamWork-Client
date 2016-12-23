using Foundation;
using SDWebImage;
using TeamWork.Models;
using System;
using System.Globalization;
using UIKit;
using System.Threading.Tasks;
using TeamWork.Services;
using TeamWork.Events;
using TeamWork.iOS.Extensions;

namespace TeamWork.iOS
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

            ItemSupplierLabel.Text = !string.IsNullOrEmpty(Product.Supplier.Name) ? Product.Supplier.Name.ToUpperInvariant() : string.Empty;
            ItemNameLabel.Text = !string.IsNullOrEmpty(Product.Name) ? Product.Name.ToUpperInvariant() : string.Empty;
            ItemDescriptionLabel.Text = !string.IsNullOrEmpty(Product.Description) ? Product.Description : "Beskrivning saknas";

            //var smallAttributes = new UIStringAttributes
            //{
            //    Font = UIFont.FromName(ItemPriceLabel.Font.Name, 10f)
            //};

            //string priceStr = "$" + Math.Round(Product.Price);
            //NSMutableAttributedString mutablePriceStr = new NSMutableAttributedString(priceStr);

            //mutablePriceStr.SetAttributes(smallAttributes.Dictionary, new NSRange(0, 1));

            //ItemPriceLabel.AttributedText = mutablePriceStr;
			ItemId = Product.Id;
        }
    }
}