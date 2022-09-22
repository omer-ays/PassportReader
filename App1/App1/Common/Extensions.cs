
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace App1.Common
{
    public static class Extentions
    {
        public static string ConvertToBase64(this Stream stream)
        {
            byte[] bytes;
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                bytes = memoryStream.ToArray();
            }
            return Convert.ToBase64String(bytes);
        }

        public static string ConvertBase64ToHtml(this string base64String)
        {
            var gzipData = Convert.FromBase64String(base64String);
            using (var zipStream = new GZipStream(new MemoryStream(gzipData), CompressionMode.Decompress))
            {
                using (var resultStream = new MemoryStream())
                {
                    var buffer = new byte[4096];
                    int read;

                    while ((read = zipStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        resultStream.Write(buffer, 0, read);
                    }
                    var htmlString = System.Text.Encoding.UTF8.GetString(resultStream.ToArray());
                    htmlString.Insert(0, "<meta name='viewport' content='width=device-width,initial-scale=1,maximum-scale=1' />");
                    return htmlString;
                }
            }
        }

    }
}
