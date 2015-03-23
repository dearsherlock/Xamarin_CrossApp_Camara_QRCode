using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Provider;
using Android.Widget;
using Cirrious.MvvmCross.Droid.Views;
using System.Collections.Generic;
using Core;
using Java.IO;
using Android.Graphics;
using System;

using Environment = Android.OS.Environment;
using Uri = Android.Net.Uri;
namespace AndroidApp.Views
{
    public static class BitmapHelpers
    {
        public static Bitmap LoadAndResizeBitmap(this string fileName, int width, int height)
        {
            // First we get the the dimensions of the file on disk
            BitmapFactory.Options options = new BitmapFactory.Options { InJustDecodeBounds = true };
            BitmapFactory.DecodeFile(fileName, options);

            // Next we calculate the ratio that we need to resize the image by
            // in order to fit the requested dimensions.
            int outHeight = options.OutHeight;
            int outWidth = options.OutWidth;
            int inSampleSize = 1;

            if (outHeight > height || outWidth > width)
            {
                inSampleSize = outWidth > outHeight
                                   ? outHeight / height
                                   : outWidth / width;
            }

            // Now we will load the image and have BitmapFactory resize it for us.
            options.InSampleSize = inSampleSize;
            options.InJustDecodeBounds = false;
            Bitmap resizedBitmap = BitmapFactory.DecodeFile(fileName, options);

            return resizedBitmap;
        }
    }
    public static class App
    {
        public static File _file;
        public static File _dir;
        public static Bitmap bitmap;
    }
    [Activity(Label = "View for FirstViewModel")]
    public class FirstView : MvxActivity
    {
        private ImageView _imageView;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.FirstView);
            Button buttonQR = this.FindViewById<Button>(Resource.Id.buttonQR);

            buttonQR.Click += async delegate {
                //NOTE: On Android you MUST pass a Context into the Constructor!
            var scanner = new ZXing.Mobile.MobileBarcodeScanner(this);
                var result = await scanner.Scan();
                if (result != null)
                 System.Console.WriteLine("Scanned Barcode: " + result.Text);
            };
            /*
            if (IsThereAnAppToTakePictures())
            {
                CreateDirectoryForPictures();
                Button button = this.FindViewById<Button>(Resource.Id.button1);

                //Button button = FindViewById<Button>(Resource.Id.myButton);
                _imageView = FindViewById<ImageView>(Resource.Id.imageView1);
                if (App.bitmap != null)
                {
                    _imageView.SetImageBitmap(App.bitmap);
                    //App.bitmap = null;
                }
                button.Click += TakeAPicture;
            }
            */
        }
        void DisplayResult(ZXing.Result result)
        {
            string message = "";

            if (result != null && !string.IsNullOrEmpty(result.Text))
                message = "Barcode: " + result.Text;
            else
                message = "Could not scan.";

            this.RunOnUiThread(() => Toast.MakeText(this, message, ToastLength.Short).Show());
        }

        /*
        async private void ButtonQR_Click(object sender, EventArgs e)
        {

            //NOTE: On Android you MUST pass a Context into the Constructor!
            var scanner = new ZXing.Mobile.MobileBarcodeScanner(this.ApplicationContext);
            var result = await scanner.Scan();

            if (result != null)
                System.Console.WriteLine("Scanned Barcode: " + result.Text);
        }
        */
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            // make it available in the gallery
            Intent mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
            Uri contentUri = Uri.FromFile(App._file);
            mediaScanIntent.SetData(contentUri);
            SendBroadcast(mediaScanIntent);

            // display in ImageView. We will resize the bitmap to fit the display
            // Loading the full sized image will consume to much memory 
            // and cause the application to crash.
            int height = Resources.DisplayMetrics.HeightPixels;
            int width = _imageView.Width;
            App.bitmap = App._file.Path.LoadAndResizeBitmap(width, height);
        }
        private void TakeAPicture(object sender, EventArgs eventArgs)
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);

            App._file = new File(App._dir, String.Format("myPhoto_{0}.jpg", Guid.NewGuid()));

            intent.PutExtra(MediaStore.ExtraOutput, Uri.FromFile(App._file));

            StartActivityForResult(intent, 0);
        }
        private bool IsThereAnAppToTakePictures()
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            IList<ResolveInfo> availableActivities = PackageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
            return availableActivities != null && availableActivities.Count > 0;
        }

        private void CreateDirectoryForPictures()
        {
            App._dir = new File(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures), "CameraAppDemo");
            if (!App._dir.Exists())
            {
                App._dir.Mkdirs();
            }
        }
    }

}