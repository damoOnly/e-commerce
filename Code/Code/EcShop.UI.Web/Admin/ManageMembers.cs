using ASPNET.WebControls;
using EcShop.ControlPanel.Comments;
using EcShop.ControlPanel.Members;
using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Core.Configuration;
using EcShop.Core.Entities;
using EcShop.Core.Enums;
using EcShop.Entities.Comments;
using EcShop.Entities.Members;
using EcShop.Entities.Store;
using EcShop.Membership.Context;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using Ecdev.Plugins;
using Ionic.Zlib;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
namespace EcShop.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.Members)]
	public class ManageMembers : AdminPage
	{
		private string searchKey;
		private string realName;
		private int? rankId;
		private bool? approved;
		protected System.Web.UI.WebControls.TextBox txtSearchText;
		protected System.Web.UI.WebControls.TextBox txtRealName;
		protected MemberGradeDropDownList rankList;
		protected System.Web.UI.WebControls.Button btnSearchButton;
		protected ExportFieldsCheckBoxList exportFieldsCheckBoxList;
		protected ExportFormatRadioButtonList exportFormatRadioButtonList;
		protected System.Web.UI.WebControls.Button btnExport;
		protected PageSize hrefPageSize;
		protected Pager pager;
		protected ImageLinkButton lkbDelectCheck1;
		protected System.Web.UI.HtmlControls.HtmlGenericControl Span1;
		protected System.Web.UI.HtmlControls.HtmlGenericControl Span2;
		protected System.Web.UI.HtmlControls.HtmlGenericControl Span3;
		protected ApprovedDropDownList ddlApproved;
		protected Grid grdMemberList;
		protected ImageLinkButton lkbDelectCheck;
		protected Pager pager1;
		protected System.Web.UI.WebControls.Literal litsmscount;
		protected System.Web.UI.HtmlControls.HtmlTextArea txtmsgcontent;
		protected System.Web.UI.HtmlControls.HtmlTextArea txtemailcontent;
		protected System.Web.UI.HtmlControls.HtmlTextArea txtsitecontent;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hdenablemsg;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hdenableemail;
		protected System.Web.UI.WebControls.Button btnsitecontent;
		protected System.Web.UI.WebControls.Button btnSendEmail;
		protected System.Web.UI.WebControls.Button btnSendMessage;
        protected WebCalendar calendarStartDate;
        protected WebCalendar calendarEndDate;
        private System.DateTime? startDate;
        private System.DateTime? endDate;
        protected HourDropDownList Starthours;
        protected HourDropDownList Endhours;

        protected UserTypeDropDownList userTypeDropDownList;
        private int? userType;
		protected override void OnInitComplete(System.EventArgs e)
		{
			base.OnInitComplete(e);
			this.grdMemberList.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdMemberList_RowDeleting);
			this.grdMemberList.ReBindData += new Grid.ReBindDataEventHandler(this.grdMemberList_ReBindData);
			this.lkbDelectCheck.Click += new System.EventHandler(this.lkbDelectCheck_Click);
			this.lkbDelectCheck1.Click += new System.EventHandler(this.lkbDelectCheck_Click);
			this.btnSearchButton.Click += new System.EventHandler(this.btnSearchButton_Click);
			this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
			this.ddlApproved.AutoPostBack = true;
			this.ddlApproved.SelectedIndexChanged += new System.EventHandler(this.ddlApproved_SelectedIndexChanged);
			this.btnSendMessage.Click += new System.EventHandler(this.btnSendMessage_Click);
			this.btnSendEmail.Click += new System.EventHandler(this.btnSendEmail_Click);
			this.btnsitecontent.Click += new System.EventHandler(this.btnsitecontent_Click);
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.LoadParameters();
			if (!this.Page.IsPostBack)
			{
				this.rankList.DataBind();
				this.rankList.SelectedValue = this.rankId;

                this.userTypeDropDownList.DataBind();
                this.userTypeDropDownList.SelectedValue = this.userType;

				this.ddlApproved.DataBind();
				if (this.approved.HasValue)
				{
					this.ddlApproved.SelectedValue = this.approved;
				}
                this.BindData();
				SiteSettings siteSetting = this.GetSiteSetting();
                //if (siteSetting.SMSEnabled)
                //{
                //    this.litsmscount.Text = this.GetAmount(siteSetting).ToString();
                //    this.hdenablemsg.Value = "1";
                //}
				if (siteSetting.EmailEnabled)
				{
					this.hdenableemail.Value = "1";
				}
			}
			CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
		}
		private SiteSettings GetSiteSetting()
		{
			return SettingsManager.GetMasterSettings(false);
		}
		private void LoadParameters()
		{
			if (!this.Page.IsPostBack)
			{
                this.Starthours.DataBind();
                this.Endhours.DataBind();
				int value = 0;
				if (int.TryParse(this.Page.Request.QueryString["rankId"], out value))
				{
					this.rankId = new int?(value);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["searchKey"]))
				{
					this.searchKey = base.Server.UrlDecode(this.Page.Request.QueryString["searchKey"]);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["realName"]))
				{
					this.realName = base.Server.UrlDecode(this.Page.Request.QueryString["realName"]);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["Approved"]))
				{
					this.approved = new bool?(System.Convert.ToBoolean(this.Page.Request.QueryString["Approved"]));
				}
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["startDate"]))
                {
                    this.startDate = new System.DateTime?(System.DateTime.Parse(this.Page.Request.QueryString["startDate"]));
                    this.calendarStartDate.SelectedDate=this.startDate;
                    this.Starthours.SelectedIndex = startDate.Value.Hour;
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["endDate"]))
                {
                    this.endDate = new System.DateTime?(System.DateTime.Parse(this.Page.Request.QueryString["endDate"]));
                     this.calendarEndDate.SelectedDate=this.endDate;
                     this.Endhours.SelectedIndex = endDate.Value.Hour;
                }
                int userTypeValue = 0;
                if (int.TryParse(this.Page.Request.QueryString["userType"], out userTypeValue))
                {
                    this.userType = new int?(userTypeValue);
                }
				this.rankList.SelectedValue = this.rankId;
				this.txtSearchText.Text = this.searchKey;
				this.txtRealName.Text = this.realName;
                this.calendarEndDate.SelectedDate = this.endDate;
                this.calendarStartDate.SelectedDate = this.startDate;
                if (this.startDate.HasValue)
                {
                    this.Starthours.SelectedIndex = startDate.Value.Hour;
                }
                if (this.endDate.HasValue)
                {
                    this.Endhours.SelectedIndex = endDate.Value.Hour;
                }

                this.userTypeDropDownList.SelectedValue = this.userType;
				return;
			}
			this.rankId = this.rankList.SelectedValue;

            this.userType = this.userTypeDropDownList.SelectedValue;
			this.searchKey = this.txtSearchText.Text;
			this.realName = this.txtRealName.Text.Trim();
          
            
		}
		private void ReBind(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			if (this.rankList.SelectedValue.HasValue)
			{
				nameValueCollection.Add("rankId", this.rankList.SelectedValue.Value.ToString(System.Globalization.CultureInfo.InvariantCulture));
			}

            if (this.userTypeDropDownList.SelectedValue.HasValue)
            {
                nameValueCollection.Add("userType", this.userTypeDropDownList.SelectedValue.Value.ToString(System.Globalization.CultureInfo.InvariantCulture));
            }
			nameValueCollection.Add("searchKey", this.txtSearchText.Text);
			nameValueCollection.Add("realName", this.txtRealName.Text);
			nameValueCollection.Add("pageSize", this.pager.PageSize.ToString(System.Globalization.CultureInfo.InvariantCulture));
			nameValueCollection.Add("Approved", this.ddlApproved.SelectedValue.ToString());
            
            //日期区间判断
            DateTime startTime;
            DateTime endTime;
            if(this.calendarEndDate.SelectedDate.HasValue)
            {
                endTime = this.calendarEndDate.SelectedDate.Value.AddHours((double)this.Endhours.SelectedValue.Value);
            }
            else
            {
                endTime = DateTime.Now;
            }
            if (this.calendarStartDate.SelectedDate.HasValue)
            {
                if (System.DateTime.Compare(this.calendarStartDate.SelectedDate.Value.AddHours((double)this.Starthours.SelectedValue.Value), endTime) >= 0)
                {
                    string text = Formatter.FormatErrorMessage("开始日期必须要早于结束日期");
                    this.ShowMsg(text, false);
                    return;
                }
                else
                {
                    startTime = this.calendarStartDate.SelectedDate.Value.AddHours((double)this.Starthours.SelectedValue.Value);
                }
                if (startTime != null)
                {
                    nameValueCollection.Add("startDate", startTime.ToString());
                    nameValueCollection.Add("endDate", endTime.ToString());
                }
            }
            if (!isSearch)
			{
				nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString(System.Globalization.CultureInfo.InvariantCulture));
			}
			base.ReloadPage(nameValueCollection);
		}
		protected void BindData()
		{
			MemberQuery memberQuery = new MemberQuery();
			memberQuery.Username = this.searchKey;
			memberQuery.Realname = this.realName;
			memberQuery.GradeId = this.rankId;
			memberQuery.PageIndex = this.pager.PageIndex;
			memberQuery.IsApproved = this.approved;
			memberQuery.SortBy = this.grdMemberList.SortOrderBy;
			memberQuery.PageSize = this.pager.PageSize;
            memberQuery.UserType = this.userType;
            memberQuery.RstStartTime = this.startDate;
            memberQuery.RstEndTime = this.endDate;
			if (this.grdMemberList.SortOrder.ToLower() == "desc")
			{
				memberQuery.SortOrder = SortAction.Desc;
			}
			DbQueryResult members = MemberHelper.GetMembers(memberQuery);
			this.grdMemberList.DataSource = members.Data;
			this.grdMemberList.DataBind();
			this.pager1.TotalRecords = (this.pager.TotalRecords = members.TotalRecords);
		}
		private void grdMemberList_ReBindData(object sender)
		{
			this.ReBind(false);
		}
		private void grdMemberList_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
		{
			ManagerHelper.CheckPrivilege(Privilege.DeleteMember);
			int userId = (int)this.grdMemberList.DataKeys[e.RowIndex].Value;
			if (!MemberHelper.Delete(userId))
			{
				this.ShowMsg("未知错误", false);
				return;
			}
			this.BindData();
			this.ShowMsg("成功删除了选择的会员", true);
		}
		private void lkbDelectCheck_Click(object sender, System.EventArgs e)
		{
			ManagerHelper.CheckPrivilege(Privilege.DeleteMember);
			int num = 0;
			foreach (System.Web.UI.WebControls.GridViewRow gridViewRow in this.grdMemberList.Rows)
			{
				System.Web.UI.WebControls.CheckBox checkBox = (System.Web.UI.WebControls.CheckBox)gridViewRow.FindControl("checkboxCol");
				if (checkBox.Checked)
				{
					int userId = System.Convert.ToInt32(this.grdMemberList.DataKeys[gridViewRow.RowIndex].Value);
					if (MemberHelper.Delete(userId))
					{
						num++;
					}
				}
			}
			if (num == 0)
			{
				this.ShowMsg("请先选择要删除的会员账号", false);
				return;
			}
			this.BindData();
			this.ShowMsg("成功删除了选择的会员", true);
		}
		private void btnExport_Click(object sender, System.EventArgs e)
		{
			if (this.exportFieldsCheckBoxList.SelectedItem == null)
			{
				this.ShowMsg("请选择需要导出的会员信息", false);
				return;
			}
			System.Collections.Generic.IList<string> list = new System.Collections.Generic.List<string>();
			System.Collections.Generic.IList<string> list2 = new System.Collections.Generic.List<string>();
			foreach (System.Web.UI.WebControls.ListItem listItem in this.exportFieldsCheckBoxList.Items)
			{
				if (listItem.Selected)
				{
					list.Add(listItem.Value);
					list2.Add(listItem.Text);
				}
			}
			System.Data.DataTable membersNopage = MemberHelper.GetMembersNopage(new MemberQuery
			{
				Username = this.searchKey,
				Realname = this.realName,
				GradeId = this.rankId,
                RstEndTime=this.endDate,
                RstStartTime= this.startDate
			}, list);
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			foreach (string current in list2)
			{
				stringBuilder.Append(current + ",");
				if (current == list2[list2.Count - 1])
				{
					stringBuilder = stringBuilder.Remove(stringBuilder.Length - 1, 1);
					stringBuilder.Append("\r\n");
				}
			}
			foreach (System.Data.DataRow dataRow in membersNopage.Rows)
			{
				foreach (string current2 in list)
				{
					stringBuilder.Append(dataRow[current2]).Append(",");
					if (current2 == list[list2.Count - 1])
					{
						stringBuilder = stringBuilder.Remove(stringBuilder.Length - 1, 1);
						stringBuilder.Append("\r\n");
					}
				}
			}
			this.Page.Response.Clear();
			this.Page.Response.Buffer = false;
			this.Page.Response.Charset = "GB2312";
			if (this.exportFormatRadioButtonList.SelectedValue == "csv")
			{
				//this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=MemberInfo.csv");
                this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=会员列表.csv");
				this.Page.Response.ContentType = "application/octet-stream";
			}
			else
			{
				//this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=MemberInfo.txt");
                this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=会员列表.txt");
				this.Page.Response.ContentType = "application/vnd.ms-word";
			}
			this.Page.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
			this.Page.EnableViewState = false;
			this.Page.Response.Write(stringBuilder.ToString());
			this.Page.Response.End();
		}
		private void btnSearchButton_Click(object sender, System.EventArgs e)
		{
			this.ReBind(true);
		}
		private void ddlApproved_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			this.ReBind(false);
		}
		private void btnSendMessage_Click(object sender, System.EventArgs e)
		{
			SiteSettings siteSetting = this.GetSiteSetting();
			string sMSSender = siteSetting.SMSSender;
			if (string.IsNullOrEmpty(sMSSender))
			{
				this.ShowMsg("请先选择发送方式", false);
				return;
			}
			ConfigData configData = null;
			if (siteSetting.SMSEnabled)
			{
				configData = new ConfigData(HiCryptographer.Decrypt(siteSetting.SMSSettings));
			}
			if (configData == null)
			{
				this.ShowMsg("请先选择发送方式并填写配置信息", false);
				return;
			}
			if (!configData.IsValid)
			{
				string text = "";
				foreach (string current in configData.ErrorMsgs)
				{
					text += Formatter.FormatErrorMessage(current);
				}
				this.ShowMsg(text, false);
				return;
			}
			string text2 = this.txtmsgcontent.Value.Trim();
			if (string.IsNullOrEmpty(text2))
			{
				this.ShowMsg("请先填写发送的内容信息", false);
				return;
			}
			int num = System.Convert.ToInt32(this.litsmscount.Text);
			string text3 = null;
			foreach (System.Web.UI.WebControls.GridViewRow gridViewRow in this.grdMemberList.Rows)
			{
				System.Web.UI.WebControls.CheckBox checkBox = (System.Web.UI.WebControls.CheckBox)gridViewRow.FindControl("checkboxCol");
				if (checkBox.Checked)
				{
					string text4 = ((System.Web.UI.DataBoundLiteralControl)gridViewRow.Controls[2].Controls[0]).Text.Trim().Replace("<div></div>", "");
					if (!string.IsNullOrEmpty(text4) && System.Text.RegularExpressions.Regex.IsMatch(text4, "^(13|14|15|17|18)\\d{9}$"))
					{
						text3 = text3 + text4 + ",";
					}
				}
			}
			if (text3 == null)
			{
				this.ShowMsg("请先选择要发送的会员或检测所选手机号格式是否正确", false);
				return;
			}
			text3 = text3.Substring(0, text3.Length - 1);
			string[] array;
			if (text3.Contains(","))
			{
				array = text3.Split(new char[]
				{
					','
				});
			}
			else
			{
				array = new string[]
				{
					text3
				};
			}
			if (num < array.Length)
			{
				this.ShowMsg("发送失败，您的剩余短信条数不足", false);
				return;
			}
			SMSSender sMSSender2 = SMSSender.CreateInstance(sMSSender, configData.SettingsXml);
			string msg;
			bool success = sMSSender2.Send(array, text2, out msg);
			this.ShowMsg(msg, success);
			this.txtmsgcontent.Value = "输入发送内容……";
			this.litsmscount.Text = (num - array.Length).ToString();
		}
		private void btnSendEmail_Click(object sender, System.EventArgs e)
		{
			SiteSettings siteSetting = this.GetSiteSetting();
			string text = siteSetting.EmailSender.ToLower();
			if (string.IsNullOrEmpty(text))
			{
				this.ShowMsg("请先选择发送方式", false);
				return;
			}
			ConfigData configData = null;
			if (siteSetting.EmailEnabled)
			{
				configData = new ConfigData(HiCryptographer.Decrypt(siteSetting.EmailSettings));
			}
			if (configData == null)
			{
				this.ShowMsg("请先选择发送方式并填写配置信息", false);
				return;
			}
			if (!configData.IsValid)
			{
				string text2 = "";
				foreach (string current in configData.ErrorMsgs)
				{
					text2 += Formatter.FormatErrorMessage(current);
				}
				this.ShowMsg(text2, false);
				return;
			}
			string text3 = this.txtemailcontent.Value.Trim();
			if (string.IsNullOrEmpty(text3))
			{
				this.ShowMsg("请先填写发送的内容信息", false);
				return;
			}
			string text4 = null;
			foreach (System.Web.UI.WebControls.GridViewRow gridViewRow in this.grdMemberList.Rows)
			{
				System.Web.UI.WebControls.CheckBox checkBox = (System.Web.UI.WebControls.CheckBox)gridViewRow.FindControl("checkboxCol");
				if (checkBox.Checked)
				{
					string text5 = ((System.Web.UI.DataBoundLiteralControl)gridViewRow.Controls[3].Controls[0]).Text.Trim().Replace("<div></div>", "");
					if (!string.IsNullOrEmpty(text5) && System.Text.RegularExpressions.Regex.IsMatch(text5, "([a-zA-Z\\.0-9_-])+@([a-zA-Z0-9_-])+((\\.[a-zA-Z0-9_-]{2,4}){1,2})"))
					{
						text4 = text4 + text5 + ",";
					}
				}
			}
			if (text4 == null)
			{
				this.ShowMsg("请先选择要发送的会员或检测邮箱格式是否正确", false);
				return;
			}
			text4 = text4.Substring(0, text4.Length - 1);
			string[] array;
			if (text4.Contains(","))
			{
				array = text4.Split(new char[]
				{
					','
				});
			}
			else
			{
				array = new string[]
				{
					text4
				};
			}
			System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage
			{
				IsBodyHtml = true,
				Priority = System.Net.Mail.MailPriority.High,
				SubjectEncoding = System.Text.Encoding.UTF8,
				BodyEncoding = System.Text.Encoding.UTF8,
				Body = text3,
				Subject = "来自" + siteSetting.SiteName
			};
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string addresses = array2[i];
				mailMessage.To.Add(addresses);
			}
			EmailSender emailSender = EmailSender.CreateInstance(text, configData.SettingsXml);
			try
			{
				if (emailSender.Send(mailMessage, System.Text.Encoding.GetEncoding(HiConfiguration.GetConfig().EmailEncoding)))
				{
					this.ShowMsg("发送邮件成功", true);
				}
				else
				{
					this.ShowMsg("发送邮件失败", false);
				}
			}
			catch (System.Exception)
			{
				this.ShowMsg("发送邮件成功,但存在无效的邮箱账号", true);
			}
			this.txtemailcontent.Value = "输入发送内容……";
		}
		private void btnsitecontent_Click(object sender, System.EventArgs e)
		{
			System.Collections.Generic.IList<MessageBoxInfo> list = new System.Collections.Generic.List<MessageBoxInfo>();
			string text = this.txtsitecontent.Value.Trim();
			if (string.IsNullOrEmpty(text) || text.Equals("输入发送内容……"))
			{
				this.ShowMsg("请输入要发送的内容信息", false);
				return;
			}
			string title = text;
			if (text.Length > 10)
			{
				title = text.Substring(0, 10) + "……";
			}
			foreach (System.Web.UI.WebControls.GridViewRow gridViewRow in this.grdMemberList.Rows)
			{
				System.Web.UI.WebControls.CheckBox checkBox = (System.Web.UI.WebControls.CheckBox)gridViewRow.FindControl("checkboxCol");
				if (checkBox.Checked)
				{
					string text2 = ((System.Web.UI.WebControls.Literal)gridViewRow.Controls[1].Controls[1]).Text.Trim();
					if (this.IsMembers(text2))
					{
						list.Add(new MessageBoxInfo
						{
							Sernder = "Admin",
							Accepter = text2,
							Title = title,
							Content = text
						});
					}
				}
			}
			if (list.Count > 0)
			{
				NoticeHelper.SendMessageToMember(list);
				this.txtsitecontent.Value = "输入发送内容……";
				this.ShowMsg(string.Format("成功给{0}个用户发送了消息.", list.Count), true);
				return;
			}
			this.ShowMsg("没有要发送的对象", false);
		}
		private bool IsMembers(string name)
		{
			string pattern = "[\\u4e00-\\u9fa5a-zA-Z]+[\\u4e00-\\u9fa5_a-zA-Z0-9]*";
			new System.Text.RegularExpressions.Regex(pattern);
			return name.Length >= 2 && name.Length <= 20;
		}
		protected int GetAmount(SiteSettings settings)
		{
			int result = 0;
			if (!string.IsNullOrEmpty(settings.SMSSettings))
			{
				string xml = HiCryptographer.Decrypt(settings.SMSSettings);
				System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
				xmlDocument.LoadXml(xml);
				string innerText = xmlDocument.SelectSingleNode("xml/Appkey").InnerText;
				string postData = "method=getAmount&Appkey=" + innerText;
				string text = this.PostData("http://sms.ecdev.cn/getAmount.aspx", postData);
				int num;
				if (int.TryParse(text, out num))
				{
					result = System.Convert.ToInt32(text);
				}
			}
			return result;
		}
		public new string PostData(string url, string postData)
		{
			string result = string.Empty;
			try
			{
				System.Uri requestUri = new System.Uri(url);
				System.Net.HttpWebRequest httpWebRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(requestUri);
				System.Text.Encoding uTF = System.Text.Encoding.UTF8;
				byte[] bytes = uTF.GetBytes(postData);
				httpWebRequest.Method = "POST";
				httpWebRequest.ContentType = "application/x-www-form-urlencoded";
				httpWebRequest.ContentLength = (long)bytes.Length;
				using (System.IO.Stream requestStream = httpWebRequest.GetRequestStream())
				{
					requestStream.Write(bytes, 0, bytes.Length);
				}
				using (System.Net.HttpWebResponse httpWebResponse = (System.Net.HttpWebResponse)httpWebRequest.GetResponse())
				{
					using (System.IO.Stream responseStream = httpWebResponse.GetResponseStream())
					{
						System.Text.Encoding uTF2 = System.Text.Encoding.UTF8;
						System.IO.Stream stream = responseStream;
						if (httpWebResponse.ContentEncoding.ToLower() == "gzip")
						{
							stream = new GZipStream(responseStream, CompressionMode.Decompress);
						}
						else
						{
							if (httpWebResponse.ContentEncoding.ToLower() == "deflate")
							{
								stream = new DeflateStream(responseStream, CompressionMode.Decompress);
							}
						}
						using (System.IO.StreamReader streamReader = new System.IO.StreamReader(stream, uTF2))
						{
							result = streamReader.ReadToEnd();
						}
					}
				}
			}
			catch (System.Exception ex)
			{
				result = string.Format("获取信息错误：{0}", ex.Message);
			}
			return result;
		}
	}
}
