using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace EcShop.UI.Web
{
	public class PIC
	{
		private Bitmap _outBmp;

		public Bitmap OutBMP
		{
			get
			{
				return this._outBmp;
			}
		}

		private static Size NewSize(int maxWidth, int maxHeight, int width, int height)
		{
			double newWidth = System.Convert.ToDouble(width);
			double newHeight = System.Convert.ToDouble(height);
			double newMaxWidth = System.Convert.ToDouble(maxWidth);
			double newMaxHeight = System.Convert.ToDouble(maxHeight);
			double retWidth;
			double retHeight;

			if (newWidth < newMaxWidth && newHeight < newMaxHeight)
			{
				retWidth = newWidth;
				retHeight = newHeight;
			}
			else
			{
                /*
				if (newWidth / newHeight > newMaxWidth / newMaxHeight)
				{
					retWidth = (double)maxWidth;
					retHeight = retWidth * newHeight / newWidth;
				}
				else
				{
					retHeight = (double)maxHeight;
					retWidth = retHeight * newWidth / newHeight;
				}
                */

                double widthRate = newWidth / newMaxWidth;
                double heightRate = newHeight / newMaxHeight;

                if (widthRate < heightRate)
                {
                    retWidth = maxWidth;
                    retHeight = newHeight * widthRate;
                }
                else
                {
                    retHeight = maxHeight;
                    retWidth = newWidth * heightRate;
                }
			}

			return new Size(System.Convert.ToInt32(retWidth), System.Convert.ToInt32(retHeight));
		}

		public static void SendSmallImage(string fileName, string newFile, int maxHeight, int maxWidth)
		{
			Image image = null;
			Bitmap bitmap = null;
			Graphics graphics = null;

			try
			{
				image = Image.FromFile(fileName);
				ImageFormat rawFormat = image.RawFormat;
				Size size = PIC.NewSize(maxWidth, maxHeight, image.Width, image.Height);
				bitmap = new Bitmap(size.Width, size.Height);

				graphics = Graphics.FromImage(bitmap);
				graphics.CompositingQuality = CompositingQuality.HighQuality;
				graphics.SmoothingMode = SmoothingMode.HighQuality;
				graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
				graphics.DrawImage(image, new Rectangle(0, 0, size.Width, size.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel);

				if (graphics != null)
				{
					graphics.Dispose();
				}

				EncoderParameters encoderParameters = new EncoderParameters();
				long[] value = new long[]
				{
					100L
				};

				EncoderParameter encoderParameter = new EncoderParameter(Encoder.Quality, value);
				encoderParameters.Param[0] = encoderParameter;
				ImageCodecInfo[] imageEncoders = ImageCodecInfo.GetImageEncoders();
				ImageCodecInfo imageCodecInfo = null;

				for (int i = 0; i < imageEncoders.Length; i++)
				{
					if (imageEncoders[i].FormatDescription.Equals("JPEG"))
					{
						imageCodecInfo = imageEncoders[i];
						break;
					}
				}

				if (imageCodecInfo != null)
				{
					bitmap.Save(newFile, imageCodecInfo, encoderParameters);
				}
				else
				{
					bitmap.Save(newFile, rawFormat);
				}
			}
			catch
			{
			}
			finally
			{
				if (graphics != null)
				{
					graphics.Dispose();
				}
				if (image != null)
				{
					image.Dispose();
				}
				if (bitmap != null)
				{
					bitmap.Dispose();
				}
			}
		}

		public void Dispose()
		{
			if (this._outBmp != null)
			{
				this._outBmp.Dispose();
				this._outBmp = null;
			}
		}

		public void SendSmallImage(string fileName, int maxHeight, int maxWidth)
		{
			Image image = null;
			this._outBmp = null;
			Graphics graphics = null;

			try
			{
				image = Image.FromFile(fileName);
				Size size = PIC.NewSize(maxWidth, maxHeight, image.Width, image.Height);
				this._outBmp = new Bitmap(size.Width, size.Height);
				graphics = Graphics.FromImage(this._outBmp);
				graphics.CompositingQuality = CompositingQuality.HighQuality;
				graphics.SmoothingMode = SmoothingMode.HighQuality;
				graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
				graphics.DrawImage(image, new Rectangle(0, 0, size.Width, size.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel);

				if (graphics != null)
				{
					graphics.Dispose();
				}
			}
			catch
			{
			}
			finally
			{
				if (graphics != null)
				{
					graphics.Dispose();
				}
				if (image != null)
				{
					image.Dispose();
				}
			}
		}

		public MemoryStream AddImageSignPic(Image img, string watermarkFilename, int watermarkStatus, int quality, int watermarkTransparency)
		{
			Graphics graphics = null;
			Image image = null;
			ImageAttributes imageAttributes = null;
			MemoryStream result;

			try
			{
				graphics = Graphics.FromImage(img);
				image = new Bitmap(watermarkFilename);
				imageAttributes = new ImageAttributes();

				ColorMap[] map = new ColorMap[]
				{
					new ColorMap
					{
						OldColor = Color.FromArgb(255, 0, 255, 0),
						NewColor = Color.FromArgb(0, 0, 0, 0)
					}
				};

				imageAttributes.SetRemapTable(map, ColorAdjustType.Bitmap);

                float watermarkTransparencyRate = 0.5f;

				if (watermarkTransparency >= 1 && watermarkTransparency <= 10)
				{
                    watermarkTransparencyRate = (float)watermarkTransparency / 10f;
				}

				float[][] array = new float[5][];
                array[0] = new float[] { 1f, 0f, 0f, 0f, 0f };
                array[1] = new float[] { 0f, 1f, 0f, 0f, 0f };
                array[2] = new float[] { 0f, 0f, 1f, 0f, 0f };
                array[3] = new float[] { 0f, 0f, 0f, watermarkTransparencyRate, 0f };
				array[4] = new float[] {0f, 0f, 0f, 0f, 1f};

                ColorMatrix newColorMatrix = new ColorMatrix(array);
				imageAttributes.SetColorMatrix(newColorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

				int x = 0;
				int y = 0;

                switch (watermarkStatus)
                {
                    case 1:
                        x = (int)((float)img.Width * 0.01f);
                        y = (int)((float)img.Height * 0.01f);
                        break;
                    case 2:
                        x = (int)((float)img.Width * 0.5f - (float)(image.Width / 2));
                        y = (int)((float)img.Height * 0.01f);
                        break;
                    case 3:
                        x = (int)((float)img.Width * 0.99f - (float)image.Width);
                        y = (int)((float)img.Height * 0.01f);
                        break;
                    case 4:
                        x = (int)((float)img.Width * 0.01f);
                        y = (int)((float)img.Height * 0.5f - (float)(image.Height / 2));
                        break;
                    case 5:
                        x = (int)((float)img.Width * 0.5f - (float)(image.Width / 2));
                        y = (int)((float)img.Height * 0.5f - (float)(image.Height / 2));
                        break;
                    case 6:
                        x = (int)((float)img.Width * 0.99f - (float)image.Width);
                        y = (int)((float)img.Height * 0.5f - (float)(image.Height / 2));
                        break;
                    case 7:
                        x = (int)((float)img.Width * 0.01f);
                        y = (int)((float)img.Height * 0.99f - (float)image.Height);
                        break;
                    case 8:
                        x = (int)((float)img.Width * 0.5f - (float)(image.Width / 2));
                        y = (int)((float)img.Height * 0.99f - (float)image.Height);
                        break;
                    case 9:
                        x = (int)((float)img.Width * 0.99f - (float)image.Width);
                        y = (int)((float)img.Height * 0.99f - (float)image.Height);
                        break;
                }

				graphics.DrawImage(image, new Rectangle(x, y, image.Width, image.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, imageAttributes);
				ImageCodecInfo[] imageEncoders = ImageCodecInfo.GetImageEncoders();
				ImageCodecInfo imageCodecInfo = null;

                for (int i = 0; i < imageEncoders.Length; i++)
				{
                    ImageCodecInfo imageCodecInfoCurrent = imageEncoders[i];

					if (imageCodecInfoCurrent.MimeType.IndexOf("jpeg") > -1)
					{
						imageCodecInfo = imageCodecInfoCurrent;
					}
				}

				EncoderParameters encoderParameters = new EncoderParameters();

				long[] values = new long[1];
				if (quality < 0 || quality > 100)
				{
					quality = 80;
				}
				values[0] = (long)quality;

				EncoderParameter encoderParameter = new EncoderParameter(Encoder.Quality, values);
				encoderParameters.Param[0] = encoderParameter;
				MemoryStream memoryStream = new MemoryStream();

				if (imageCodecInfo != null)
				{
					img.Save(memoryStream, imageCodecInfo, encoderParameters);
				}

				result = memoryStream;
			}
			catch
			{
				MemoryStream memoryStream = null;
				result = memoryStream;
			}
			finally
			{
				if (graphics != null)
				{
					graphics.Dispose();
				}
				if (img != null)
				{
					img.Dispose();
				}
				if (image != null)
				{
					image.Dispose();
				}
				if (imageAttributes != null)
				{
					imageAttributes.Dispose();
				}
			}
			return result;
		}
	}
}
