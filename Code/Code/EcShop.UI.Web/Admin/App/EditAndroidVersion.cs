﻿using EcShop.ControlPanel.Store;
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
namespace EcShop.UI.Web.Admin.App
{
    [PrivilegeCheck(Privilege.EditAndroidVersion)]
    public class EditAndroidVersion : AdminPage
    {
        private int Id = 0;
        protected System.Web.UI.WebControls.TextBox txtVersion;
        protected System.Web.UI.WebControls.TextBox txtDescription;
        protected System.Web.UI.WebControls.TextBox txtUpgradeUrl;
        protected System.Web.UI.WebControls.Button btnSaveVersion;
        protected System.Web.UI.WebControls.CheckBox ChkisForceUpgrade;
        protected System.Web.UI.WebControls.FileUpload fileUpload;
        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!int.TryParse(this.Page.Request.QueryString["Id"], out this.Id))
            {
                base.GotoResourceNotFound();
                return;
            }
            this.btnSaveVersion.Click += new System.EventHandler(this.btnSaveVersion_Click);
            if (!this.Page.IsPostBack)
            {
                if (this.Id > 0)
                {

                    this.LoadVersion();
                    this.txtVersion.Enabled = false;
                }

                else
                {

                }
            }
        }
        /// <summary>
        /// 保存版本信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveVersion_Click(object sender, System.EventArgs e)
        {
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

            decimal num = 0.00m;
            if (!decimal.TryParse(this.txtVersion.Text, out num))
            {
                this.ShowMsg("版本号格式不对,必须为实数", false);
                return;
            }


            if (this.Id > 0)
            {

                //修改版本有上传文件才进行数据包存储操作
                if (this.fileUpload.HasFile)
                {


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
                }


                AppVersionRecordInfo appVersionRecordInfo = APPHelper.GetAppVersionById(this.Id);

                appVersionRecordInfo.Version = num;
                appVersionRecordInfo.Description = this.txtDescription.Text;
                appVersionRecordInfo.UpgradeUrl = this.txtUpgradeUrl.Text.Trim();
                appVersionRecordInfo.IsForcibleUpgrade = this.ChkisForceUpgrade.Checked;
                appVersionRecordInfo.Device = "android";
                if (APPHelper.UpdateAppVersionRecord(appVersionRecordInfo))
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



                AppVersionRecordInfo appVersionRecordInfo = APPHelper.GetLatestAppVersionRecordOrderById("android");
                if (appVersionRecordInfo != null)
                {
                    if (appVersionRecordInfo.Version >= num)
                    {
                        this.ShowMsg("版本号必须大于之前的版本", false);
                        return;
                    }
                }


                AppVersionRecordInfo newappVersionRecordInfo = new AppVersionRecordInfo();
                newappVersionRecordInfo.Version = num;
                newappVersionRecordInfo.IsForcibleUpgrade = this.ChkisForceUpgrade.Checked;
                newappVersionRecordInfo.Description = this.txtDescription.Text.Trim();
                newappVersionRecordInfo.UpgradeUrl = this.txtUpgradeUrl.Text.Trim();
                newappVersionRecordInfo.Device = "android";
                if (APPHelper.AddAppVersionRecord(newappVersionRecordInfo))
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
            AppVersionRecordInfo appVersionRecordInfo = APPHelper.GetAppVersionById(this.Id);
            if (appVersionRecordInfo != null)
            {
                this.txtVersion.Text = appVersionRecordInfo.Version.ToString("0.00");
                this.txtDescription.Text = appVersionRecordInfo.Description;
                this.txtUpgradeUrl.Text = appVersionRecordInfo.UpgradeUrl;
                this.ChkisForceUpgrade.Checked = appVersionRecordInfo.IsForcibleUpgrade;

            }
        }
    }
}

