using System;
using System.Linq;
using Foundation;
using TeamWork.iOS.Extensions;
using UIKit;

namespace TeamWork.iOS.Controllers
{
    public partial class RatingMainViewController : UITabBarController
    {
        private RatingStreamingViewController streamingController;
        private RatingProcessingViewController processingController;

        public RatingMainViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
			this.AddDefaultNavigationTitle();

            streamingController = ChildViewControllers.OfType<RatingStreamingViewController>().FirstOrDefault();
            processingController = ChildViewControllers.OfType<RatingProcessingViewController>().FirstOrDefault();
        }

        public void ActivateCapture()
        {
            SelectedIndex = 0;
        }

        public void ActivateSubmitting()
		{
			SelectedIndex = 1;

            NSData imageData = streamingController?.CaptureData;
            processingController?.SetCaptureImage(imageData);

			if (streamingController != null)
            {
                streamingController.CaptureData = null;
            }

			processingController?.AnalyzeCapture();
        }
    }
}