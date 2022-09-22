using Android.App;
using Android.Content;
using Android.OS;
using Android.Print;
using Android.PrintServices;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using App1.Dependencies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: Dependency(typeof(PrintService))]
namespace App1.Droid
{
    public class PrintService : IPrintService
    {
        void IPrintService.Print(WebView viewToPrint)
        {
            var droidViewToPrint = Platform.CreateRenderer(viewToPrint).ViewGroup.GetChildAt(0) as Android.Webkit.WebView;

            if (droidViewToPrint != null)
            {
                // Only valid for API 19+
                var version = Android.OS.Build.VERSION.SdkInt;

                if (version >= Android.OS.BuildVersionCodes.Kitkat)
                {
                    var printMgr = (PrintManager)Forms.Context.GetSystemService(Context.PrintService);
                    var docAdapter = droidViewToPrint.CreatePrintDocumentAdapter();
                    printMgr.Print("Planet-Receipt-Print", docAdapter, null);
                }
            }
        }
    }
}