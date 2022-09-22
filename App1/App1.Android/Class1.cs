using Android.App;
using Android.Content;
using Android.Graphics;

using App1.Dependencies;
using App1.Droid;
using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Firebase.ML.Vision;
using Android.Graphics;
using Firebase.ML.Vision;
using Firebase.ML.Vision.Common;
using Firebase.ML.Vision.Text;
using Xamarin.Forms;
using Android.Gms.Tasks;
using App1.Common;

[assembly: Dependency(typeof(TextRecognizerAnalyzer))]
namespace App1.Droid
{
    public class TextRecognizerAnalyzer : IPassportAnalyzer
    {
        [Obsolete]
        public void Analyzer(string base64Image)
        {

            byte[] encodedDataAsBytes = Convert.FromBase64String(base64Image);
            var bmp = BitmapFactory.DecodeByteArray(encodedDataAsBytes, 0, encodedDataAsBytes.Length);
            var image = FirebaseVisionImage.FromBitmap(bmp);
            FirebaseVisionTextRecognizer textRecognizer = FirebaseVision.Instance.OnDeviceTextRecognizer;
            textRecognizer.ProcessImage(image)
                .AddOnSuccessListener(new ProcessImageOnSuccessListener())
                .AddOnFailureListener(new ProcessImageOnFailureListener());
        }
    }

    public class ProcessImageOnSuccessListener : Java.Lang.Object, IOnSuccessListener
    {
        public void OnSuccess(Java.Lang.Object result)
        {
            var resultText = (FirebaseVisionText)result;
            if (resultText != null && resultText.Text != null)
                MessagingCenter.Send(new ReadingImageRecognationText(), "ReadingImageRecognationText", resultText.Text);
        }
    }
    public class ProcessImageOnFailureListener : Java.Lang.Object, IOnFailureListener
    {
        public void OnFailure(Java.Lang.Exception e)
        {
            MessagingCenter.Send(new ReadingImageRecognationText(), "ReadingImageRecognationTextError", new Exception(e.Message, e.InnerException));
        }
    }
}