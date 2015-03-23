using System;
using System.Drawing;
using Foundation;
using UIKit;

namespace iOSAppSingle
{
    public partial class RootViewController : UIViewController
    {
        static bool UserInterfaceIdiomIsPhone
        {
            get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
        }

        public RootViewController(IntPtr handle) : base(handle)
        {
        }

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        #region View lifecycle

        public  override void ViewDidLoad()
        {
            base.ViewDidLoad();
            /* 以下寫法會得到:
            error CS4034: The 'await' operator can only be used within an 
            async lambda expression. Consider marking this lambda expression
            with the 'async' modifier.
            
            buttonScan.TouchUpInside += (sender, e) => {

                //NOTE: On Android you MUST pass a Context into the Constructor!
                var scanner = new ZXing.Mobile.MobileBarcodeScanner();
                var result = await scanner.Scan();

                if (result != null)
                    Console.WriteLine("Scanned Barcode: " + result.Text);
            };
            */
            //改成如下是可以
            buttonScan.TouchUpInside +=async (sender, e) => {

                //NOTE: On Android you MUST pass a Context into the Constructor!
                var scanner = new ZXing.Mobile.MobileBarcodeScanner();
                var result = await scanner.Scan();

                if (result != null)
                    Console.WriteLine("Scanned Barcode: " + result.Text);
            };


            // Perform any additional setup after loading the view, typically from a nib.
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
        }

        #endregion

       
    }
}