using ASPNET.WebControls;
using EcShop.ControlPanel.Commodities;
using EcShop.ControlPanel.Promotions;
using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Entities.Promotions;
using EcShop.Entities.Store;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.CountDownsManager)]
    public class AddCountDownManager : AdminPage
	{
        protected System.Web.UI.WebControls.TextBox txtTitle;
		protected WebCalendar calendarStartDate;
		protected HourDropDownList drophours;
		protected WebCalendar calendarEndDate;
		protected HourDropDownList HourDropDownList1;
		protected System.Web.UI.WebControls.Button btnAddCountDown;
        protected System.Web.UI.WebControls.FileUpload fileUpload;
        protected System.Web.UI.WebControls.TextBox txtActiveImgUrl;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnAddCountDown.Click += new System.EventHandler(this.btnbtnAddCountDown_Click);
			if (!this.Page.IsPostBack)
			{

				this.HourDropDownList1.DataBind();
				this.drophours.DataBind();
			}
		}
		private void btnbtnAddCountDown_Click(object sender, System.EventArgs e)
		{
            CountDownCategoriesInfo countDownInfo = new CountDownCategoriesInfo();

            countDownInfo.StartTime = Convert.ToDateTime("2015-1-1 " + this.drophours.SelectedValue.Value + ":00:00.0000000");
            countDownInfo.EndTime   = Convert.ToDateTime("2015-1-1 " + this.HourDropDownList1.SelectedValue.Value + ":00:00.0000000");

            if (System.DateTime.Compare(countDownInfo.StartTime, countDownInfo.EndTime) >= 0 && this.HourDropDownList1.SelectedValue.Value!=0)
			{
                this.ShowMsg("开始日期必须要早于结束日期", false);
                return;
			}
            countDownInfo.Title = this.txtTitle.Text.Trim();
            countDownInfo.AdImageUrl = CatalogHelper.UploadActiveCategorieImage(this.fileUpload.PostedFile);
            countDownInfo.AdImageLinkUrl = this.txtActiveImgUrl.Text.Trim();

            if (PromoteHelper.AddCountDownCategories(countDownInfo))
            {
                this.ShowMsg("添加限时管理成功", true);
                return;
            }
            this.ShowMsg("添加限时管理失败", true);
		}
	}
}
