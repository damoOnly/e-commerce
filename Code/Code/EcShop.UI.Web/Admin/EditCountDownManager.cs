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
    public class EditCountDownManager : AdminPage
	{
        protected System.Web.UI.WebControls.TextBox txtTitle;
        protected WebCalendar calendarStartDate;
        protected HourDropDownList drophours;
        protected WebCalendar calendarEndDate;
        protected HourDropDownList HourDropDownList1;
        protected System.Web.UI.WebControls.Button btnUpdateCountDown;
        protected System.Web.UI.WebControls.FileUpload fileUpload;
        protected System.Web.UI.WebControls.TextBox txtActiveImgUrl;
        protected System.Web.UI.WebControls.HiddenField hidd_ImgSrc;
        protected int CountDownCategoryId;

		protected void Page_Load(object sender, System.EventArgs e)
		{
            if (!int.TryParse(base.Request.QueryString["CountDownCategoryId"], out this.CountDownCategoryId))
			{
				base.GotoResourceNotFound();
				return;
			}
			this.btnUpdateCountDown.Click += new System.EventHandler(this.btnUpdateGroupBuy_Click);
			if (!base.IsPostBack)
			{
	
				this.HourDropDownList1.DataBind();
				this.drophours.DataBind();
                CountDownCategoriesInfo countDownInfo = PromoteHelper.GetCountDownnCategoriesInfo(this.CountDownCategoryId);
				if (countDownInfo == null)
				{
					base.GotoResourceNotFound();
					return;
				}
				this.LoadCountDown(countDownInfo);
			}
		}
        private void LoadCountDown(CountDownCategoriesInfo countDownInfo)
		{
			this.txtTitle.Text = countDownInfo.Title;
		
			this.calendarEndDate.SelectedDate = new System.DateTime?(countDownInfo.EndTime.Date);
            this.drophours.SelectedValue = new int?(countDownInfo.StartTime.Hour);

			this.calendarStartDate.SelectedDate = new System.DateTime?(countDownInfo.StartTime.Date);
            this.HourDropDownList1.SelectedValue = new int?(countDownInfo.EndTime.Hour);
            this.txtActiveImgUrl.Text = countDownInfo.AdImageLinkUrl;
            this.hidd_ImgSrc.Value = countDownInfo.AdImageUrl;

		}
		private void btnUpdateGroupBuy_Click(object sender, System.EventArgs e)
		{
            CountDownCategoriesInfo countDownInfo = new CountDownCategoriesInfo();
            string text = string.Empty;
            countDownInfo.StartTime = Convert.ToDateTime("2015-1-1 " + this.drophours.SelectedValue.Value + ":00:00.0000000");
            countDownInfo.EndTime   = Convert.ToDateTime("2015-1-1 " + this.HourDropDownList1.SelectedValue.Value + ":00:00.0000000");


            if (System.DateTime.Compare(countDownInfo.StartTime, countDownInfo.EndTime) >= 0 && this.HourDropDownList1.SelectedValue.Value!=0)
            {
                this.ShowMsg("开始日期必须要早于结束日期", false);
                return;
            }
            countDownInfo.Title = this.txtTitle.Text.Trim();
            countDownInfo.AdImageUrl =CatalogHelper.UploadActiveCategorieImage(this.fileUpload.PostedFile);
            countDownInfo.AdImageUrl = string.IsNullOrEmpty(countDownInfo.AdImageUrl) ? this.hidd_ImgSrc.Value.Trim() : countDownInfo.AdImageUrl;
            countDownInfo.AdImageLinkUrl = this.txtActiveImgUrl.Text.Trim();

            countDownInfo.CountDownCategoryId = CountDownCategoryId;

            if (PromoteHelper.UpdateCountDownDownCategories(countDownInfo))
            {
                this.ShowMsg("修改限时抢购活动成功", true);
                return;
            }
			this.ShowMsg("编辑限时抢购活动失败", true);
		}
	}
}
