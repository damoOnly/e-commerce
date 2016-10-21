using ASPNET.WebControls;
using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Entities;
using EcShop.Entities.Store;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.AppRunImg)]
    public class AppRunImg : AdminPage
    {
        protected System.Web.UI.WebControls.Image Android720_1280;
        protected System.Web.UI.WebControls.Image iOS750_1334;
        protected System.Web.UI.WebControls.Image iOS640_960;
        protected System.Web.UI.WebControls.Image iOS1242_2208;
        protected System.Web.UI.WebControls.Button btnSave;
        protected System.Web.UI.WebControls.Button btnCancel;
        protected System.Web.UI.WebControls.HiddenField Android720_1280hid;
        protected System.Web.UI.WebControls.HiddenField iOS750_1334hid;
        protected System.Web.UI.WebControls.HiddenField iOS640_960hid;
        protected System.Web.UI.WebControls.HiddenField iOS1242_2208hid;

        protected FileUpload fileUpload1;
        protected FileUpload fileUpload2;
        protected FileUpload fileUpload3;
        protected FileUpload fileUpload4;
        Dictionary<string, string> dic;
        protected void Page_Load(object sender, System.EventArgs e)
        {
            btnCancel.Click += btnCancel_Click;
            btnSave.Click += btnSave_Click;

            dic = new Dictionary<string, string>();
            List<AppRunImgInfo> list = APPHelper.GetAppRunImg();
            if (!(list == null || list.Count == 0))
            {
                foreach (AppRunImgInfo a in list)
                {
                    dic.Add(a.phoneType, a.imgSrc);
                }
            }
            if (!this.Page.IsPostBack)
            {
                if (dic.ContainsKey("Android720_1280"))
                {
                    Android720_1280hid.Value = Android720_1280.ImageUrl = dic["Android720_1280"];

                }
                if (dic.ContainsKey("iOS750_1334"))
                {
                    iOS750_1334hid.Value = iOS750_1334.ImageUrl = dic["iOS750_1334"];
                }
                if (dic.ContainsKey("iOS640_960"))
                {
                    iOS640_960hid.Value = iOS640_960.ImageUrl = dic["iOS640_960"];
                }
                if (dic.ContainsKey("iOS1242_2208"))
                {
                    iOS1242_2208hid.Value = iOS1242_2208.ImageUrl = dic["iOS1242_2208"];
                }
            }
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            string mapPath = Globals.MapPath("/Storage/master/app/");

            string pa = "/Storage/master/app/";

            string fileUuid = Guid.NewGuid().ToString().ToLower() + "_";

            if (!string.IsNullOrWhiteSpace(fileUpload1.FileName))
            {
                string filename = fileUuid + "android_720_1289.";
                string ext = fileUpload1.FileName.Substring(fileUpload1.FileName.LastIndexOf('.') + 1);

                fileUpload1.SaveAs(mapPath + filename + ext);
                Android720_1280.ImageUrl = pa + filename + ext;
            }
            if (!string.IsNullOrWhiteSpace(fileUpload2.FileName))
            {
                string filename = fileUuid + "ios750_1334.";
                string ext = fileUpload2.FileName.Substring(fileUpload2.FileName.LastIndexOf('.') + 1);

                fileUpload2.SaveAs(mapPath + filename + ext);
                iOS750_1334.ImageUrl = pa + filename + ext;
            }
            if (!string.IsNullOrWhiteSpace(fileUpload3.FileName))
            {
                string filename = fileUuid + "ios640_960.";
                string ext = fileUpload3.FileName.Substring(fileUpload3.FileName.LastIndexOf('.') + 1);

                fileUpload3.SaveAs(mapPath + filename + ext);
                iOS640_960.ImageUrl = pa + filename + ext;
            }
            if (!string.IsNullOrWhiteSpace(fileUpload4.FileName))
            {
                string filename = fileUuid + "ios1242_2208.";
                string ext = fileUpload4.FileName.Substring(fileUpload4.FileName.LastIndexOf('.') + 1);

                fileUpload4.SaveAs(mapPath + filename + ext);
                iOS1242_2208.ImageUrl = pa + filename + ext;
            }
            //return;
            //ShowMsg(Android720_1280hid.Value,false);
            if (dic.ContainsKey("Android720_1280"))
            {
                dic["Android720_1280"] = Android720_1280.ImageUrl;
            }
            else
            {
                dic.Add("Android720_1280", Android720_1280.ImageUrl);
            }
            if (dic.ContainsKey("iOS750_1334"))
            {
                dic["iOS750_1334"] = iOS750_1334.ImageUrl;
            }
            else
            {
                dic.Add("iOS750_1334", iOS750_1334.ImageUrl);
            }
            if (dic.ContainsKey("iOS640_960"))
            {
                dic["iOS640_960"] = iOS640_960.ImageUrl;
            }
            else
            {
                dic.Add("iOS640_960", iOS640_960.ImageUrl);
            }
            if (dic.ContainsKey("iOS1242_2208"))
            {
                dic["iOS1242_2208"] = iOS1242_2208.ImageUrl;
            }
            else
            {
                dic.Add("iOS1242_2208", iOS1242_2208.ImageUrl);
            }
            if (APPHelper.ChangeAppRunImg(dic))
            {
                this.ShowMsg("保存成功！", true);
                string[] name= System.IO.Directory.GetFiles(mapPath);
                foreach (string n in name)
                {
                     
                }
                //this.Response.Redirect("AppRunImg.aspx", true);
            }
            else
            {
                this.ShowMsg("保存失败！", false);
            }
        }

        void btnCancel_Click(object sender, EventArgs e)
        {

        }
    }
}
