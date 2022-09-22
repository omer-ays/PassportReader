using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using App1.Services;

namespace XamarinPassportReader
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReadPass : ContentPage
    {
        public ReadPass()
        {
            InitializeComponent();
        }

        private void elle_beni(object sender, EventArgs e)
        {
            var passportReaderService = DependencyService.Get<IPassportReaderService>();
            passportReaderService.ReadPassportOnCamera((passport, exception) =>
            {
                if (passport != null)
                {
                    var successMessage = "Pasaport No : " + passport.PassportNumber;
                    try
                    {
                        var expiryDate = new DateTime(int.Parse("20" + passport.ExpiryDate.Substring(0, 2)),
                           int.Parse(passport.ExpiryDate.Substring(2, 2)),
                           int.Parse(passport.ExpiryDate.Substring(4, 2)));
                        if (expiryDate < DateTime.Now)
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                            });
                            return;
                        }
                    }
                    catch
                    {
                        successMessage += "\n Tarih okuma hata aldı.";
                    }


                    string PassportNo = passport.PassportNumber;
                    string Name = passport.FirstName;
                    string Surname = passport.LastName;

                    //NationalityCode

                   
                    DisplayAlert("Okuma Başarılı", successMessage, "Tamam");
                }
                else if (exception != null)
                {
                    DisplayAlert("Okuma Hatası", exception.Message, "Tamam");
                }
                else
                {
                    DisplayAlert("Okuma Hatası", "Pasaport bilinmeyen bir hatadan okunamadı tekrar deneyiniz", "Tamam");
                }
            });
        }
    }
}