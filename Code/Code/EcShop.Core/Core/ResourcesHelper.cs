using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Web;
namespace EcShop.Core
{
	public static class ResourcesHelper
	{
		public static void CreateThumbnail(string sourceFilename, string destFilename, int width, int height)
		{
			Image image = Image.FromFile(sourceFilename);

			if (image.Width <= width && image.Height <= height)
			{
				File.Copy(sourceFilename, destFilename, true);
				image.Dispose();
			}
			else
			{
				int imgWidth = image.Width;
				int imgHeight = image.Height;

				float scale = (float)height / (float)imgHeight;
				if ((float)width / (float)imgWidth < scale)
				{
					scale = (float)width / (float)imgWidth;
				}
				width = (int)((float)imgWidth * scale);
				height = (int)((float)imgHeight * scale);

				Image thumbnial = new Bitmap(width, height);
				Graphics graphics = Graphics.FromImage(thumbnial);
				graphics.Clear(Color.White);
				graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
				graphics.SmoothingMode = SmoothingMode.HighQuality;
				graphics.DrawImage(image, new Rectangle(0, 0, width, height), new Rectangle(0, 0, imgWidth, imgHeight), GraphicsUnit.Pixel);
				EncoderParameters encoderParameters = new EncoderParameters();
				EncoderParameter encoderParameter = new EncoderParameter(Encoder.Quality, 100L);
				encoderParameters.Param[0] = encoderParameter;
				ImageCodecInfo[] imageEncoders = ImageCodecInfo.GetImageEncoders();
				ImageCodecInfo encoder = null;

				for (int i = 0; i < imageEncoders.Length; i++)
				{
					if (imageEncoders[i].FormatDescription.Equals("JPEG"))
					{
						encoder = imageEncoders[i];
						break;
					}
				}
				thumbnial.Save(destFilename, encoder, encoderParameters);
				encoderParameters.Dispose();
				encoderParameter.Dispose();
				image.Dispose();
				thumbnial.Dispose();
				graphics.Dispose();
			}
		}
		public static bool CheckPostedFile(HttpPostedFile postedFile)
		{
			bool result;
			if (postedFile == null || postedFile.ContentLength == 0)
			{
				result = false;
			}
			else
			{
				string ext = Path.GetExtension(postedFile.FileName).ToLower();
				if (ext != ".jpg" && ext != ".gif" && ext != ".jpeg" && ext != ".png" && ext != ".bmp")
				{
					result = false;
				}
				else
				{
					string contentType = postedFile.ContentType.ToLower();
					result = (!(contentType != "image/pjpeg") || !(contentType != "image/jpeg") || !(contentType != "image/gif") || !(contentType != "image/bmp") || !(contentType != "image/png") || !(contentType != "image/x-png"));
				}
			}
			return result;
		}
		public static string GenerateFilename(string extension)
		{
			return Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + extension;
		}
		public static void DeleteImage(string imageUrl)
		{
			if (!string.IsNullOrEmpty(imageUrl))
			{
				try
				{
					string path = HttpContext.Current.Request.MapPath(Globals.ApplicationPath + imageUrl);
					if (File.Exists(path))
					{
						File.Delete(path);
					}
				}
				catch
				{
				}
			}
		}
	}
}
