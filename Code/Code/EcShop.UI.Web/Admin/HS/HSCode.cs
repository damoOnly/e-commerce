using ASPNET.WebControls;
using EcShop.ControlPanel.Commodities;
using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Entities.HS;
using EcShop.Entities.Store;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;


namespace EcShop.UI.Web.Admin
{
    public class HSCode : AdminPage
    {
        private string searchkey;
        private string HSName;
        protected System.Web.UI.WebControls.TextBox codeSearchText;
        protected System.Web.UI.WebControls.TextBox nameSearchText;
        protected System.Web.UI.WebControls.Button btnSearchButton;
        protected Grid grdHSCode;
        protected Pager pager;

        protected override void OnInitComplete(System.EventArgs e)
        {
            base.OnInitComplete(e);
            this.btnSearchButton.Click += new System.EventHandler(this.btnSearchButton_Click);
            this.grdHSCode.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdHSCode_RowDeleting);
        }

        private void grdHSCode_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            //ManagerHelper.CheckPrivilege(Privilege.DeleteMember);
            int HS_CODE_ID = (int)this.grdHSCode.DataKeys[e.RowIndex].Value;
            if (!HSCodeHelper.Delete(HS_CODE_ID))
            {
                this.ShowMsg("删除失败, 可能该海关编码正在使用", false);
                return;
            }
            this.BindData();
            this.ShowMsg("成功删除了选择的海关编码", true);
        }

        protected void Page_Load()
        {
            this.LoadParameters();
            if (!this.Page.IsPostBack)
            {
                this.BindData();
            }
        }

        private void BindData()
        {
            DbQueryResult HSCode = HSCodeHelper.GetHSCode(new HSCodeQuery
            {
                HS_CODE = this.searchkey,
                HS_NAME = this.HSName,
                PageIndex = this.pager.PageIndex,
                PageSize = this.pager.PageSize
            });
            this.grdHSCode.DataSource = HSCode.Data;
            this.grdHSCode.DataBind();
            this.pager.TotalRecords = HSCode.TotalRecords;
        }

        private void LoadParameters()
        {
            if (!this.Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["searchKey"]))
                {
                    this.searchkey = Globals.UrlDecode(this.Page.Request.QueryString["searchKey"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["HS_NAME"]))
                {
                    this.HSName = base.Server.UrlDecode(this.Page.Request.QueryString["HS_NAME"]);
                }
                this.codeSearchText.Text = this.searchkey;
                this.nameSearchText.Text = this.HSName;
                return;
            }
            this.searchkey = this.codeSearchText.Text.Trim();
            this.HSName = this.nameSearchText.Text.Trim();
        }

        private void btnSearchButton_Click(object sender, System.EventArgs e)
        {
            this.ReBind(true);
        }

        private void ReBind(bool isSearch)
        {
            System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
            nameValueCollection.Add("searchKey", this.codeSearchText.Text);
            //nameValueCollection.Add("pageSize", "10");
            nameValueCollection.Add("HS_NAME", this.nameSearchText.Text);
            nameValueCollection.Add("pageSize", this.pager.PageSize.ToString(System.Globalization.CultureInfo.InvariantCulture));
            if (!isSearch)
            {
                nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString(System.Globalization.CultureInfo.InvariantCulture));
            }
            base.ReloadPage(nameValueCollection);
        }

        /// <summary>
        /// 发送POST请求
        /// </summary>
        public string SendData(string url, string postData)
        {
            string str = string.Empty;
            try
            {
                Uri requestUri = new Uri(url);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUri);
                byte[] bytes = Encoding.UTF8.GetBytes(postData);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = bytes.Length;
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(bytes, 0, bytes.Length);
                }
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    Stream myResponseStream = response.GetResponseStream();

                    StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));

                    str = myStreamReader.ReadToEnd();
                }
            }
            catch (Exception exception)
            {
                str = string.Format("获取信息错误：{0}", exception.Message);
            }
            return str;
        }
    }
}
