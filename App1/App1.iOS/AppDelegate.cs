using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Firebase.RemoteConfig;
using Foundation;
using UIKit;
using Xamarin.Forms;

namespace App1.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Forms.SetFlags("CollectionView_Experimental");
            Forms.Init();
            var type = typeof(RemoteConfig);
            Firebase.Core.App.Configure();

            LoadApplication(new App());
            InitializeLocalization();
            return base.FinishedLaunching(app, options);
        }

        public void InitializeLocalization()
        {
        }
    }
}
