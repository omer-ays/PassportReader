using App1.Dependencies;
using App1.iOS;
using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: Dependency(typeof(PrintService))]
namespace App1.iOS
{
    public class PrintService : IPrintService
    {
        void IPrintService.Print(WebView viewToPrint)
        {
            //var appleViewToPrint = Platform.CreateRenderer(viewToPrint).NativeView as WKWebView;

            var renderer = Platform.GetRenderer(viewToPrint);
            if (renderer == null)
            {
                renderer = Platform.CreateRenderer(viewToPrint);
                Platform.SetRenderer(viewToPrint, renderer);
            }

            var printInfo = UIPrintInfo.PrintInfo;

            printInfo.OutputType = UIPrintInfoOutputType.General;
            printInfo.JobName = "Forms EZ-Print";
            printInfo.Orientation = UIPrintInfoOrientation.Portrait;
            printInfo.Duplex = UIPrintInfoDuplex.None;

            var printController = UIPrintInteractionController.SharedPrintController;

            printController.PrintInfo = printInfo;
            printController.ShowsPageRange = true;
            printController.PrintFormatter = renderer.NativeView.ViewPrintFormatter;

            printController.Present(true, (printInteractionController, completed, error) =>
            {

            });
        }
    }
}