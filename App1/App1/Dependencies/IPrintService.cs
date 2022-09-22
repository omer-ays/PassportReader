using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace App1.Dependencies
{
    public interface IPrintService
    {
        void Print(WebView viewToPrint);
    }
}
