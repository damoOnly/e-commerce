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
    [PrivilegeCheck(Privilege.IOSUP)]
    public class IosUpgrade : AdminPage
    {
        protected System.Web.UI.WebControls.FileUpload fileUpload;
        protected System.Web.UI.WebControls.Button btnUpoad;
        protected System.Web.UI.WebControls.Literal litVersion;
        protected System.Web.UI.HtmlControls.HtmlInputHidden hidIsForcibleUpgrade;
        protected System.Web.UI.WebControls.Literal litDescription;
        protected System.Web.UI.WebControls.Literal litUpgradeUrl;

        protected System.Web.UI.WebControls.TextBox txtVersion;
        protected System.Web.UI.WebControls.TextBox txtDescription;
        protected System.Web.UI.WebControls.TextBox txtUpgradeUrl;
        protected System.Web.UI.WebControls.Button btnSaveVersion;

        protected System.Web.UI.WebControls.CheckBox ChkisForceUpgrade;
        protected void Page_Load(object sender, System.EventArgs e)
        {
            this.btnUpoad.Click += new System.EventHandler(this.btnUpoad_Click);
            this.btnSaveVersion.Click += new System.EventHandler(this.btnSaveVersion_Click);
            if (!this.Page.IsPostBack)
            {
                this.LoadVersion();

                this.LoadNewVersion();
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
            string text = this.Page.Request.MapPath("~/storage/data/app/ios");
            string fileName = System.IO.Path.GetFileName(this.fileUpload.PostedFile.FileName);
            string text2 = System.IO.Path.Combine(text, fileName);
            this.fileUpload.PostedFile.SaveAs(text2);
            System.IO.DirectoryInfo directoryInfo = new System.IO.DirectoryInfo(text);
            using (ZipFile zipFile = ZipFile.Read(System.IO.Path.Combine(directoryInfo.FullName, fileName)))
            {
                foreach (ZipEntry current in zipFile)
                {
                    current.Extract(directoryInfo.FullName, ExtractExistingFileAction.OverwriteSilently);
                }
            }
            System.IO.File.Delete(text2);
            this.LoadVersion();
            AppVersionRecordInfo appVersionRecordInfo = APPHelper.GetLatestAppVersionRecord("ios");
            if (appVersionRecordInfo == null)
            {
                appVersionRecordInfo = new AppVersionRecordInfo();
                appVersionRecordInfo.Device = "ios";
                appVersionRecordInfo.Version = 0.00m;
            }
            decimal num = 1.00m;
            decimal.TryParse(this.litVersion.Text, out num);
            if (num > appVersionRecordInfo.Version)
            {
                bool isForcibleUpgrade = false;
                bool.TryParse(this.hidIsForcibleUpgrade.Value, out isForcibleUpgrade);
                appVersionRecordInfo.Version = num;
                appVersionRecordInfo.IsForcibleUpgrade = isForcibleUpgrade;
                appVersionRecordInfo.Description = this.litDescription.Text;
                appVersionRecordInfo.UpgradeUrl = this.litUpgradeUrl.Text;
                APPHelper.AddAppVersionRecord(appVersionRecordInfo);
            }
            this.ShowMsg("上传成功！", true);
        }


        /// <summary>
        /// 保存版本信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveVersion_Click(object sender, System.EventArgs e)
        {

            AppVersionRecordInfo appVersionRecordInfo = APPHelper.GetLatestAppVersionRecordOrderById("ios");
            decimal num = 0.00m;
            if (!decimal.TryParse(this.txtVersion.Text, out num))
            {
                this.ShowMsg("版本号格式不对,必须为实数", false);
                return;
            }

            //if (appVersionRecordInfo != null)
            //{
            //    if (appVersionRecordInfo.Version >= num)
            //    {
            //        this.ShowMsg("版本号必须大于当前版本", false);
            //        return;
            //    }
            //}

            if (string.IsNullOrWhiteSpace(this.txtDescription.Text))
            {
                this.ShowMsg("版本描述不能为空", false);
                return;
            }

            if (string.IsNullOrWhiteSpace(this.txtUpgradeUrl.Text))
            {
                this.ShowMsg("链接地址不能为空", false);
                return;
            }

            if (appVersionRecordInfo == null)
            {
                AppVersionRecordInfo newappVersionRecordInfo = new AppVersionRecordInfo();


                newappVersionRecordInfo.Version = num;
                newappVersionRecordInfo.IsForcibleUpgrade = this.ChkisForceUpgrade.Checked;
                newappVersionRecordInfo.Description = this.txtDescription.Text.Trim();
                newappVersionRecordInfo.UpgradeUrl = this.txtUpgradeUrl.Text.Trim();
                newappVersionRecordInfo.Device = "ios";
                if (APPHelper.AddAppVersionRecord(newappVersionRecordInfo))
                {
                    this.ShowMsg("保存成功！", true);
                }
                else
                {
                    this.ShowMsg("保存失败！", true);
                }
            }

            else
            {
                appVersionRecordInfo.Version = num;
                appVersionRecordInfo.Description = this.txtDescription.Text.Trim();
                appVersionRecordInfo.UpgradeUrl = this.txtUpgradeUrl.Text.Trim();
                appVersionRecordInfo.IsForcibleUpgrade = this.ChkisForceUpgrade.Checked;
                appVersionRecordInfo.Device = "ios";
                if (APPHelper.UpdateAppVersionRecord(appVersionRecordInfo))
                {
                    this.ShowMsg("保存成功！", true);
                }
                else
                {
                    this.ShowMsg("保存失败！", true);
                }
            }


        }


        private void LoadVersion()
        {
            System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
            try
            {
                xmlDocument.Load(System.Web.HttpContext.Current.Request.MapPath("/Storage/data/app/ios/IosUpgrade.xml"));
                this.litVersion.Text = xmlDocument.SelectSingleNode("root/Version").InnerText;
                this.litDescription.Text = xmlDocument.SelectSingleNode("root/Description").InnerText;
                this.litUpgradeUrl.Text = xmlDocument.SelectSingleNode("root/UpgradeUrl").InnerText;
            }
            catch
            {
            }
        }


        private void LoadNewVersion()
        {
            AppVersionRecordInfo appVersionRecordInfo = APPHelper.GetLatestAppVersionRecordOrderById("ios");
            if (appVersionRecordInfo!= null)
            {
                this.txtVersion.Text = appVersionRecordInfo.Version.ToString("0.00");
                this.txtDescription.Text = appVersionRecordInfo.Description;
                this.txtUpgradeUrl.Text = appVersionRecordInfo.UpgradeUrl;
                this.ChkisForceUpgrade.Checked = appVersionRecordInfo.IsForcibleUpgrade;

            }
        }
    }
}
