using EcShop.Core;
using EcShop.Membership.Context;
using System;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
namespace EcShop.UI.Common.Controls
{
	public class UploadHandler : IHttpHandler
	{
		private string uploaderId;
		private string uploadType;
		private string action;
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
		public void ProcessRequest(HttpContext context)
		{
			this.uploaderId = context.Request.QueryString["uploaderId"];
			this.uploadType = context.Request.QueryString["uploadType"];
			this.action = context.Request.QueryString["action"];
			context.Response.Clear();
			context.Response.ClearHeaders();
			context.Response.ClearContent();
			context.Response.Expires = -1;
			try
			{
				if (this.action.Equals("upload"))
				{
					this.DoUpload(context);
				}
				else
				{
					if (this.action.Equals("delete"))
					{
						this.DoDelete(context);
					}
				}
			}
			catch (Exception ex)
			{
				this.WriteBackError(context, ex.Message);
			}
		}
		private void DoUpload(HttpContext context)
		{
			if (context.Request.Files.Count == 0)
			{
				this.WriteBackError(context, "没有检测到任何文件");
				return;
			}
			HttpPostedFile httpPostedFile = context.Request.Files[0];
			int num = 1;
			while (httpPostedFile.ContentLength == 0 && num < context.Request.Files.Count)
			{
				httpPostedFile = context.Request.Files[num];
				num++;
			}
			if (httpPostedFile.ContentLength == 0)
			{
				this.WriteBackError(context, "当前文件没有任何内容");
				return;
			}
			if (!httpPostedFile.ContentType.ToLower().StartsWith("image/") || !Regex.IsMatch(Path.GetExtension(httpPostedFile.FileName.ToLower()), "\\.(jpg|gif|png|bmp|jpeg)$", RegexOptions.Compiled))
			{
				this.WriteBackError(context, "文件类型错误，请选择有效的图片文件");
				return;
			}
			this.UploadImage(context, httpPostedFile);
		}
		private void DoDelete(HttpContext context)
		{
			string path = context.Request.MapPath(Globals.ApplicationPath + context.Request.Form[this.uploaderId + "_uploadedImageUrl"]);

			string path40 = path.Replace("\\images\\", "\\thumbs40\\40_");
			string path60 = path.Replace("\\images\\", "\\thumbs60\\60_");
			string path100 = path.Replace("\\images\\", "\\thumbs100\\100_");
			string path160 = path.Replace("\\images\\", "\\thumbs160\\160_");
			string path180 = path.Replace("\\images\\", "\\thumbs180\\180_");
			string path220 = path.Replace("\\images\\", "\\thumbs220\\220_");
			string path310 = path.Replace("\\images\\", "\\thumbs310\\310_");
			string path410 = path.Replace("\\images\\", "\\thumbs410\\410_");

			if (File.Exists(path))
			{
				File.Delete(path);
			}
			if (File.Exists(path40))
			{
				File.Delete(path40);
			}
			if (File.Exists(path60))
			{
				File.Delete(path60);
			}
			if (File.Exists(path100))
			{
				File.Delete(path100);
			}
			if (File.Exists(path160))
			{
				File.Delete(path160);
			}
			if (File.Exists(path180))
			{
				File.Delete(path180);
			}
			if (File.Exists(path220))
			{
				File.Delete(path220);
			}
			if (File.Exists(path310))
			{
				File.Delete(path310);
			}
			if (File.Exists(path410))
			{
				File.Delete(path410);
			}

			context.Response.Write("<script type=\"text/javascript\">window.parent.DeleteCallback('" + this.uploaderId + "');</script>");
		}
		private void UploadImage(HttpContext context, HttpPostedFile file)
		{
			string storageRoot = HiContext.Current.GetStoragePath() + "/" + this.uploadType;
			string filename = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + Path.GetExtension(file.FileName);

            string imageFilename = storageRoot + "/images/" + filename;
			string thumbs40 = storageRoot + "/thumbs40/40_" + filename;
			string thumbs60 = storageRoot + "/thumbs60/60_" + filename;
			string thumbs100 = storageRoot + "/thumbs100/100_" + filename;
			string thumbs160 = storageRoot + "/thumbs160/160_" + filename;
			string thumbs180 = storageRoot + "/thumbs180/180_" + filename;
			string thumbs220 = storageRoot + "/thumbs220/220_" + filename;
			string thumbs310 = storageRoot + "/thumbs310/310_" + filename;
			string thumbs410 = storageRoot + "/thumbs410/410_" + filename;
			
            file.SaveAs(context.Request.MapPath(Globals.ApplicationPath + imageFilename));
			string sourceFilename = context.Request.MapPath(Globals.ApplicationPath + imageFilename);
			
            ResourcesHelper.CreateThumbnail(sourceFilename, context.Request.MapPath(Globals.ApplicationPath + thumbs40), 40, 40);
			ResourcesHelper.CreateThumbnail(sourceFilename, context.Request.MapPath(Globals.ApplicationPath + thumbs60), 60, 60);
			ResourcesHelper.CreateThumbnail(sourceFilename, context.Request.MapPath(Globals.ApplicationPath + thumbs100), 100, 100);
			ResourcesHelper.CreateThumbnail(sourceFilename, context.Request.MapPath(Globals.ApplicationPath + thumbs160), 160, 160);
			ResourcesHelper.CreateThumbnail(sourceFilename, context.Request.MapPath(Globals.ApplicationPath + thumbs180), 180, 180);
			ResourcesHelper.CreateThumbnail(sourceFilename, context.Request.MapPath(Globals.ApplicationPath + thumbs220), 220, 220);
			ResourcesHelper.CreateThumbnail(sourceFilename, context.Request.MapPath(Globals.ApplicationPath + thumbs310), 310, 310);
			ResourcesHelper.CreateThumbnail(sourceFilename, context.Request.MapPath(Globals.ApplicationPath + thumbs410), 410, 410);

			string[] value = new string[]
			{
				"'" + this.uploadType + "'",
				"'" + this.uploaderId + "'",
				"'" + imageFilename + "'",
				"'" + thumbs40 + "'",
				"'" + thumbs60 + "'",
				"'" + thumbs100 + "'",
				"'" + thumbs160 + "'",
				"'" + thumbs180 + "'",
				"'" + thumbs220 + "'",
				"'" + thumbs310 + "'",
				"'" + thumbs410 + "'"
			};
			context.Response.Write("<script type=\"text/javascript\">window.parent.UploadCallback(" + string.Join(",", value) + ");</script>");
		}
		private void WriteBackError(HttpContext context, string error)
		{
			string[] value = new string[]
			{
				"'" + this.uploadType + "'",
				"'" + this.uploaderId + "'",
				"'" + error + "'"
			};
			context.Response.Write("<script type=\"text/javascript\">window.parent.ErrorCallback(" + string.Join(",", value) + ");</script>");
		}
	}
}
