using App1.Common;
using App1.Dependencies;
using App1.iOS;
using Firebase.MLKit.Vision;
using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;


[assembly: Dependency(typeof(PassportAnalyzer))]

namespace App1.iOS
{
    public class PassportAnalyzer : IPassportAnalyzer
    {
        void IPassportAnalyzer.Analyzer(string base64Image)
        {

            var vision = VisionApi.Create();
            var textRecognizer = vision.GetOnDeviceTextRecognizer();
            var image = Utils.Utility.Base64ToUIImage(base64Image);
            image = Utils.Utility.ScaleAndRotateImage(image, UIKit.UIImageOrientation.Right);
            var im = new VisionImage(image);
            textRecognizer.ProcessImage(im, new VisionTextRecognitionCallbackHandler((text, err) =>
            {
                if (text != null && text.Text != null)
                {
                    MessagingCenter.Send(new ReadingImageRecognationText(), "ReadingImageRecognationText", text.Text);
                }
                else if (err != null)
                {
                    MessagingCenter.Send(new ReadingImageRecognationText(), "ReadingImageRecognationTextError", text.Text);
                }
                else
                {
                    MessagingCenter.Send(new ReadingImageRecognationText(), "ReadingImageRecognationTextError", new Exception("Okuma Hatası Tekrar Deneyiniz", null));
                }
            }));
        }
    }
}