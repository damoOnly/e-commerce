using EcShop.ControlPanel.Store;
using EcShop.Entities;
using EcShop.Entities.Store;
using EcShop.UI.ControlPanel.Utility;
using Ionic.Zip;
using System;
using System.IO;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
namespace EcShop.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.AndroidUP)]
	public class AndroidUpgrade : AdminPage
	{
		protected System.Web.UI.WebControls.FileUpload fileUpload;
		protected System.Web.UI.WebControls.Button btnUpoad;
        protected System.Web.UI.WebControls.Literal litAppName;
        protected System.Web.UI.WebControls.Literal litApkName;
		protected System.Web.UI.WebControls.Literal litVersion;
        protected System.Web.UI.WebControls.Literal litVersionName;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hidIsForcibleUpgrade;
		protected System.Web.UI.WebControls.Literal litDescription;
		protected System.Web.UI.WebControls.Literal litUpgradeUrl;
        protected System.Web.UI.WebControls.Literal litUpgradeInfoUrl;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnUpoad.Click += new System.EventHandler(this.btnUpoad_Click);
			if (!this.Page.IsPostBack)
			{
				this.LoadVersion();
			}
		}
		private void btnUpoad_Click(object sender, System.EventArgs e)
		{
			if (!this.fileUpload.HasFile)
			{
				this.ShowMsg("请先选择一个数据包文件", false);
				return;
			}
			if (this.fileUpload.PostedFile.ContentLength == 0 || (this.fileUpload.PostedFile.ContentType != "application/x-zip-compressed" && this.fileUpload.PostedFile.ContentType != "application/zip" && this.fileUpload.PostedFile.ContentType != "application/octet-stream"))
			{
				this.ShowMsg("请上传正确的数据包文件", false);
				return;
			}
			string savedPath = this.Page.Request.MapPath("~/storage/data/app/android");
			string fileName = System.IO.Path.GetFileName(this.fileUpload.PostedFile.FileName);
			string fullFileName = System.IO.Path.Combine(savedPath, fileName);
			this.fileUpload.PostedFile.SaveAs(fullFileName);
			System.IO.DirectoryInfo directoryInfo = new System.IO.DirectoryInfo(savedPath);
			using (ZipFile zipFile = ZipFile.Read(System.IO.Path.Combine(directoryInfo.FullName, fileName)))
			{
				foreach (ZipEntry current in zipFile)
				{
					current.Extract(directoryInfo.FullName, ExtractExistingFileAction.OverwriteSilently);
				}
			}
			System.IO.File.Delete(fullFileName);

			this.LoadVersion();

			AppVersionRecordInfo appVersionRecordInfo = APPHelper.GetLatestAppVersionRecord("android");
			if (appVersionRecordInfo == null)
			{
				appVersionRecordInfo = new AppVersionRecordInfo();
				appVersionRecordInfo.Device = "android";
				appVersionRecordInfo.Version = 0.00m;
			}

			decimal num = 1.00m;
			decimal.TryParse(this.litVersion.Text, out num);
			if (num > appVersionRecordInfo.Version)
			{
				bool isForcibleUpgrade = false;
				bool.TryParse(this.hidIsForcibleUpgrade.Value, out isForcibleUpgrade);

                appVersionRecordInfo.AppName = this.litAppName.Text;
                appVersionRecordInfo.AppPackageName = this.litApkName.Text;
                appVersionRecordInfo.VersionName = this.litVersionName.Text;
				appVersionRecordInfo.Version = num;
				appVersionRecordInfo.IsForcibleUpgrade = isForcibleUpgrade;
				appVersionRecordInfo.Description = this.litDescription.Text;
				appVersionRecordInfo.UpgradeUrl = this.litUpgradeUrl.Text;
                appVersionRecordInfo.UpgradeInfoUrl = this.litUpgradeInfoUrl.Text;

				APPHelper.AddAppVersionRecord(appVersionRecordInfo);
			}

			this.ShowMsg("上传成功！", true);
		}
		private void LoadVersion()
		{
			System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
			try
			{
				doc.Load(System.Web.HttpContext.Current.Request.MapPath("/Storage/data/app/android/AndroidUpgrade.xml"));
                this.litAppName.Text = doc.SelectSingleNode("root/AppName").InnerText;
                this.litApkName.Text = doc.SelectSingleNode("root/ApkName").InnerText;
                this.litVersionName.Text = doc.SelectSingleNode("root/Version").InnerText;
				this.litVersion.Text = doc.SelectSingleNode("root/VersionCode").InnerText;
				this.litDescription.Text = doc.SelectSingleNode("root/Description").InnerText;
                this.litUpgradeUrl.Text = "/Storage/data/app/android/" + doc.SelectSingleNode("root/ApkName").InnerText;
                this.litUpgradeInfoUrl.Text = "/Storage/data/app/android/" + doc.SelectSingleNode("root/UpgradeInfo").InnerText;

                this.hidIsForcibleUpgrade.Value = doc.SelectSingleNode("root/IsForcibleUpgrade").InnerText;
			}
			catch(Exception ex)
			{
			}
		}
	}
}
