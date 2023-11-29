using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using System.Text;

namespace ScrSht
{
	class ReceiveScreenShot
	{
		public ReceiveScreenShot()
		{
		}

		public string ConsumeScreenshot(MultipartFormDataContent content)
        {
            var folder = "ScreenShotR" + DateTime.Now.ToString("MMMM");
            string pathString = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folder);
            if (!Directory.Exists(pathString))
            {
                Directory.CreateDirectory(pathString);
            }
            var screenshotFile = pathString + "\\" + Guid.NewGuid().ToString() + ".jpg";

			

			var byteArray = content.ReadAsByteArrayAsync().Result;
			var stream = new MemoryStream(byteArray);
			Image image = Image.FromStream(stream, true); ;
			image.Save(screenshotFile, ImageFormat.Jpeg);
			stream.Dispose();

			return "saved in server";
        }

        //[HttpPost]
        //public void ConsumeScreenshot(HttpContent content)
        //{
        //    var folder = "ScreenShotR" + DateTime.Now.ToString("MMMM");
        //    string pathString = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folder);
        //    if (!Directory.Exists(pathString))
        //    {
        //        Directory.CreateDirectory(pathString);
        //    }
        //    var screenshotFile = pathString + "\\" + Guid.NewGuid().ToString() + ".jpg";

        //    var byteArray = content.ReadAsByteArrayAsync().Result;
        //    var stream = new MemoryStream(byteArray);
        //    Image image = Image.FromStream(stream, true); ;
        //    image.Save(screenshotFile, ImageFormat.Jpeg);
        //    stream.Dispose();
        //}

    }
}
