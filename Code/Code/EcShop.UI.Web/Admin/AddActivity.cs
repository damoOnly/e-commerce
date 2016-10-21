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
	[PrivilegeCheck(Privilege.AddActivity)]
	public class AddActivity : AdminPage
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
		protected System.Web.UI.WebControls.Button btnAddActivity;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnAddActivity.Click += new System.EventHandler(this.btnAddActivity_Click);
		}
		private void btnAddActivity_Click(object sender, System.EventArgs e)
		{
			if (ReplyHelper.HasReplyKey(this.txtKeys.Text.Trim()))
			{
				this.ShowMsg("关键字重复!", false);
				return;
			}
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
			if (VShopHelper.SaveActivity(new ActivityInfo
			{
				CloseRemark = this.txtCloseRemark.Text.Trim(),
				Description = this.txtDescription.Text.Trim(),
				EndDate = this.txtEndDate.SelectedDate.Value.AddMinutes(59.0).AddSeconds(59.0),
				Item1 = this.txtItem1.Text.Trim(),
				Item2 = this.txtItem2.Text.Trim(),
				Item3 = this.txtItem3.Text.Trim(),
				Item4 = this.txtItem4.Text.Trim(),
				Item5 = this.txtItem5.Text.Trim(),
				Keys = this.txtKeys.Text.Trim(),
				MaxValue = maxValue,
				Name = this.txtName.Text.Trim(),
				PicUrl = this.uploader1.UploadedImageUrl,
				StartDate = this.txtStartDate.SelectedDate.Value
			}))
			{
				base.Response.Redirect("ManageActivity.aspx");
				return;
			}
			this.ShowMsg("添加失败", false);
		}
	}
}
