using App1.Common;
using App1.Dependencies;
using App1.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

[assembly: Dependency(typeof(PassportReaderService))]
namespace App1.Services
{
    public interface IPassportReaderService
    {
        void ReadPassport(string base64Image, Action<PassportCodeModel, Exception> Callback);
        void ReadPassportOnCamera(Action<PassportCodeModel, Exception> Callback);
    }

    public class PassportReaderService : IPassportReaderService
    {
        public void ReadPassport(string base64Image, Action<PassportCodeModel, Exception> Callback)
        {
            MessagingCenter.Unsubscribe<ReadingImageRecognationText, string>(this, "ReadingImageRecognationText");
            MessagingCenter.Unsubscribe<ReadingImageRecognationText, string>(this, "ReadingImageRecognationTextError");
            MessagingCenter.Subscribe<ReadingImageRecognationText, string>(this, "ReadingImageRecognationText", (sender, text) =>
            {
                try
                {
                    if (text.Contains("P<"))
                    {
                        var passportText = text.Substring(text.IndexOf("P<")).Replace("\n", "").Replace(" ", "");
                        /*For Regex
                        //var matchGroups = Regex.Match(passportText, "(^[P])<*([A-Z,a-z,0-9])*<*([A-Z])*<*([A-Z])<*([a-z,A-Z,0-9]{9})([a-z,A-Z,0-9])*<*([0-9])*");
                        if (matchGroups.Success)
                        {
                            var model = new PassportCodeModel();
                            foreach (var match in matchGroups.Groups)
                            {
                                model.CountryCode =
                               //TODO : ..
                            }
                        }
                        */

                        //Split Char
                        char splitCharacter = passportText[1];
                        passportText = passportText.Replace("«", "<");
                        //Divided 2 section
                        string line1 = passportText.Substring(0, 44);
                        string line2 = passportText.Substring(44, 44);

                        //Passaport Number old version 9 character, new 7
                        var passportNumber = line2.Substring(0, 9);
                        if (passportNumber.Contains(splitCharacter.ToString()))
                        {
                            passportNumber = line2.Substring(0, line2.IndexOf(splitCharacter));
                        }

                        //Getting User Info
                        string fullName = line1.Substring(5, 39);
                        while (fullName.Contains("<<"))
                        {
                            fullName = fullName.Replace("<<", "<");
                        }

                        var sFullName = fullName.Split(splitCharacter);

                        string lastName = null;
                        string firstName = null;
                        if (sFullName.Length > 0)
                        {
                            lastName = sFullName[0];
                            firstName = sFullName[1];
                            for (int i = 2; i < 10; i++)
                            {
                                if (sFullName.Length > i)
                                {
                                    var name = sFullName[i].Replace(splitCharacter.ToString(), "");
                                    if (name.Length > 0)
                                        firstName += " " + name;
                                    i++;
                                }
                                else
                                    break;
                            }
                        }

                        //Setting For Model and Fire
                        Callback?.Invoke(new PassportCodeModel()
                        {
                            Type = passportText[0].ToString(),
                            IssuingCountry = passportText.Substring(2, 3),
                            LastName = lastName,
                            FirstName = firstName,
                            PassportNumber = passportNumber,
                            Nationality = line2.Substring(10, 3),
                            BirthDate = line2.Substring(13, 6),
                            Gender = line2.Substring(20, 1),
                            ExpiryDate = line2.Substring(21, 6),
                            ID = line2.Substring(28, 11)
                        }, null);
                    }
                    else
                    {
                        Callback?.Invoke(null, new Exception("Pasaport okunamadı tekrar deneyiniz.", null));
                    }
                }
                catch
                {
                    Callback?.Invoke(null, new Exception("Pasaport okunamadı tekrar deneyiniz.", null));
                }

            });
            MessagingCenter.Subscribe<ReadingImageRecognationText, string>(this, "ReadingImageRecognationTextError", (sender, text) =>
            {
                Callback?.Invoke(null, null);
            });

            DependencyService.Get<IPassportAnalyzer>().Analyzer(base64Image);
        }

        public void ReadPassportOnCamera(Action<PassportCodeModel, Exception> Callback)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var photoBase64 = await DependencyService.Get<ICameraService>().TakePhotoAndGetBase64Async();
                if (photoBase64 != null)
                    ReadPassport(photoBase64, Callback);
                else
                {
                    Callback?.Invoke(null, null);
                }
            });
        }
    }
}
