using ASPNET.WebControls;
using EcShop.Entities.Promotions;
using EcShop.UI.ControlPanel.Utility;
using kindeditor.Net;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin.promotion.Ascx
{
	public class PromotionView : System.Web.UI.UserControl
	{
		private PromotionInfo promotion;
		protected System.Web.UI.WebControls.TextBox txtPromoteSalesName;
		protected WebCalendar calendarStartDate;
		protected WebCalendar calendarEndDate;
		protected MemberGradeCheckBoxList chklMemberGrade;
		protected KindeditorControl fckDescription;
		public PromotionInfo Promotion
		{
			get
			{
				PromotionInfo promotionInfo = new PromotionInfo();
				promotionInfo.Name = this.txtPromoteSalesName.Text;
				if (this.calendarStartDate.SelectedDate.HasValue)
				{
					promotionInfo.StartDate = this.calendarStartDate.SelectedDate.Value;
				}
				if (this.calendarEndDate.SelectedDate.HasValue)
				{
					promotionInfo.EndDate = this.calendarEndDate.SelectedDate.Value.AddHours(23.0).AddMinutes(59.0).AddSeconds(59.0);
				}
				promotionInfo.MemberGradeIds = this.chklMemberGrade.SelectedValue;
				promotionInfo.Description = this.fckDescription.Text;
				return promotionInfo;
			}
			set
			{
				this.promotion = value;
			}
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				this.chklMemberGrade.DataBind();
				if (this.promotion != null)
				{
					this.txtPromoteSalesName.Text = this.promotion.Name;
					this.calendarStartDate.SelectedDate = new System.DateTime?(this.promotion.StartDate);
					this.calendarEndDate.SelectedDate = new System.DateTime?(this.promotion.EndDate);
					this.chklMemberGrade.SelectedValue = this.promotion.MemberGradeIds;
					this.fckDescription.Text = this.promotion.Description;
				}
			}
		}
	}
}
