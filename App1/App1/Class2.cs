
using App1.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using App1.Common;

[assembly: Dependency(typeof(CameraService))]
namespace App1.Services
{
    public interface ICameraService
    {
        Task TakePhotoAndSaveAsync();
        Task<string> LoadPhotoAsync(FileResult photo);
        Task<Stream> TakePhotoAndGetStreamAsync();
        Task<string> TakePhotoAndGetBase64Async();
    }

    public class CameraService : ICameraService
    {
        public async Task TakePhotoAndSaveAsync()
        {
            try
            {
                var photo = await MediaPicker.CapturePhotoAsync();
                var path = await LoadPhotoAsync(photo);
                Console.WriteLine($"CapturePhotoAsync COMPLETED: {path}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CapturePhotoAsync THREW: {ex.Message}");
            }
        }

        public async Task<Stream> TakePhotoAndGetStreamAsync()
        {
            try
            {
                var photo = await MediaPicker.CapturePhotoAsync();
                if (photo == null)
                    return null;
                return await photo.OpenReadAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CapturePhotoAsync THREW: {ex.Message}");
                return null;
            }
        }

        public async Task<string> TakePhotoAndGetBase64Async()
        {
            try
            {
                var photoStream = await TakePhotoAndGetStreamAsync();
                return photoStream?.ConvertToBase64();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CapturePhotoAsync THREW: {ex.Message}");
                return null;
            }
        }

        public async Task<string> LoadPhotoAsync(FileResult photo)
        {
            // canceled
            if (photo == null)
            {
                return null;
            }
            // save the file into local storage
            var newFile = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
            using (var stream = await photo.OpenReadAsync())
            using (var newStream = File.OpenWrite(newFile))
                await stream.CopyToAsync(newStream);
            return newFile;
        }
    }
}