using ASPNET.WebControls;
using EcShop.ControlPanel.Store;
using EcShop.Entities.Store;
using EcShop.Entities.VShop;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.EditActivity)]
	public class EditActivity : AdminPage
	{
		protected System.Web.UI.WebControls.TextBox txtName;
		protected WebCalendar txtStartDate;
		protected WebCalendar txtEndDate;
		protected System.Web.UI.WebControls.TextBox txtMaxValue;
		protected System.Web.UI.WebControls.TextBox txtDescription;
		protected System.Web.UI.WebControls.TextBox txtItem1;
		protected System.Web.UI.WebControls.TextBox txtItem2;
		protected System.Web.UI.WebControls.TextBox txtItem3;
		protected System.Web.UI.WebControls.TextBox txtItem4;
		protected System.Web.UI.WebControls.TextBox txtItem5;
		protected System.Web.UI.WebControls.TextBox txtCloseRemark;
		protected System.Web.UI.WebControls.TextBox txtKeys;
		protected ImageUploader uploader1;
		protected System.Web.UI.WebControls.Button btnEditActivity;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnEditActivity.Click += new System.EventHandler(this.btnEditActivity_Click);
			if (!this.Page.IsPostBack)
			{
				int urlIntParam = base.GetUrlIntParam("id");
				ActivityInfo activity = VShopHelper.GetActivity(urlIntParam);
				this.txtCloseRemark.Text = activity.CloseRemark;
				this.txtDescription.Text = activity.Description;
				this.txtEndDate.SelectedDate = new System.DateTime?(activity.EndDate);
				this.txtKeys.Text = activity.Keys;
				if (activity.MaxValue != 0)
				{
					this.txtMaxValue.Text = activity.MaxValue.ToString();
				}
				this.txtName.Text = activity.Name;
				this.txtStartDate.SelectedDate = new System.DateTime?(activity.StartDate);
				this.uploader1.UploadedImageUrl = activity.PicUrl;
				this.txtItem1.Text = activity.Item1;
				this.txtItem2.Text = activity.Item2;
				this.txtItem3.Text = activity.Item3;
				this.txtItem4.Text = activity.Item4;
				this.txtItem5.Text = activity.Item5;
			}
		}
		private void btnEditActivity_Click(object sender, System.EventArgs e)
		{
			int maxValue = 0;
			if (!this.txtStartDate.SelectedDate.HasValue)
			{
				this.ShowMsg("请选择开始日期！", false);
				return;
			}
			if (!this.txtEndDate.SelectedDate.HasValue)
			{
				this.ShowMsg("请选择结束日期！", false);
				return;
			}
			if (this.txtStartDate.SelectedDate.Value.CompareTo(this.txtEndDate.SelectedDate.Value) >= 0)
			{
				this.ShowMsg("开始日期不能晚于结束日期！", false);
				return;
			}
			if (this.txtMaxValue.Text != "" && !int.TryParse(this.txtMaxValue.Text, out maxValue))
			{
				this.ShowMsg("人数上限格式错误！", false);
				return;
			}
			int urlIntParam = base.GetUrlIntParam("id");
			ActivityInfo activity = VShopHelper.GetActivity(urlIntParam);
			if (activity.Keys != this.txtKeys.Text.Trim() && ReplyHelper.HasReplyKey(this.txtKeys.Text.Trim()))
			{
				this.ShowMsg("关键字重复!", false);
				return;
			}
			activity.CloseRemark = this.txtCloseRemark.Text.Trim();
			activity.Description = this.txtDescription.Text.Trim();
			activity.EndDate = this.txtEndDate.SelectedDate.Value.AddMinutes(59.0).AddSeconds(59.0);
			activity.Item1 = this.txtItem1.Text.Trim();
			activity.Item2 = this.txtItem2.Text.Trim();
			activity.Item3 = this.txtItem3.Text.Trim();
			activity.Item4 = this.txtItem4.Text.Trim();
			activity.Item5 = this.txtItem5.Text.Trim();
			activity.Keys = this.txtKeys.Text.Trim();
			activity.MaxValue = maxValue;
			activity.Name = this.txtName.Text.Trim();
			activity.PicUrl = this.uploader1.UploadedImageUrl;
			activity.StartDate = this.txtStartDate.SelectedDate.Value;
			if (VShopHelper.UpdateActivity(activity))
			{
				base.Response.Redirect("ManageActivity.aspx");
				return;
			}
			this.ShowMsg("更新失败", false);
		}
	}
}
