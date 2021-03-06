using ASPNET.WebControls;
using EcShop.ControlPanel.Promotions;
using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Entities.Promotions;
using EcShop.Entities.Store;
using EcShop.Membership.Context;
using EcShop.Membership.Core;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
namespace EcShop.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.SupplierCountDowns)]
    public class SupplierAddCountDown : AdminPage
	{
		protected System.Web.UI.WebControls.TextBox txtSearchText;
		protected ProductCategoriesDropDownList dropCategories;
		protected System.Web.UI.WebControls.TextBox txtSKU;
		protected GroupBuyProductDropDownList dropGroupBuyProduct;
		protected System.Web.UI.WebControls.Label lblPrice;
		protected WebCalendar calendarStartDate;
		protected HourDropDownList drophours;
		protected WebCalendar calendarEndDate;
		protected HourDropDownList HourDropDownList1;
		protected System.Web.UI.WebControls.TextBox txtPrice;
		protected System.Web.UI.WebControls.TextBox txtMaxCount;
        protected System.Web.UI.WebControls.TextBox txtPlanCount;
		protected System.Web.UI.WebControls.TextBox txtContent;
		protected System.Web.UI.WebControls.Button btnAddCountDown;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnAddCountDown.Click += new System.EventHandler(this.btnbtnAddCountDown_Click);
			if (!this.Page.IsPostBack)
			{
				this.dropCategories.DataBind();
                this.dropGroupBuyProduct.SupplierDataBind(UserHelper.GetAssociatedSupplierId(HiContext.Current.User.UserId));
				this.HourDropDownList1.DataBind();
				this.drophours.DataBind();
			}
		}
		private void btnbtnAddCountDown_Click(object sender, System.EventArgs e)
		{
			CountDownInfo countDownInfo = new CountDownInfo();
			string text = string.Empty;
			if (this.dropGroupBuyProduct.SelectedValue > 0)
			{
				if (PromoteHelper.ProductCountDownExist(this.dropGroupBuyProduct.SelectedValue.Value))
				{
					this.ShowMsg("已经存在此商品的限时抢购活动", false);
					return;
				}
				countDownInfo.ProductId = this.dropGroupBuyProduct.SelectedValue.Value;
			}
			else
			{
				text += Formatter.FormatErrorMessage("请选择限时抢购商品");
			}

            bool timeValidated = false;

			if (!this.calendarEndDate.SelectedDate.HasValue)
			{
				text += Formatter.FormatErrorMessage("请选择结束日期");
			}
			else
			{
				countDownInfo.EndDate = this.calendarEndDate.SelectedDate.Value.AddHours((double)this.HourDropDownList1.SelectedValue.Value);
				if (System.DateTime.Compare(this.calendarStartDate.SelectedDate.Value.AddHours((double)this.drophours.SelectedValue.Value), countDownInfo.EndDate) >= 0)
				{
					text += Formatter.FormatErrorMessage("开始日期必须要早于结束日期");
				}
				else
				{
					countDownInfo.StartDate = this.calendarStartDate.SelectedDate.Value.AddHours((double)this.drophours.SelectedValue.Value);

                    timeValidated = true;
				}
			}

            if (timeValidated && PromoteHelper.ProductCountDownExist(this.dropGroupBuyProduct.SelectedValue.Value, countDownInfo.StartDate, countDownInfo.EndDate, 0))
            {
                this.ShowMsg("已经存在此商品该时间段的限时抢购活动", false);
                return;
            }

			int maxCount;
			if (int.TryParse(this.txtMaxCount.Text.Trim(), out maxCount))
			{
				countDownInfo.MaxCount = maxCount;
			}
			else
			{
				text += Formatter.FormatErrorMessage("限购数量不能为空，只能为整数");
			}

            int planCount;
            if (int.TryParse(this.txtPlanCount.Text.Trim(), out planCount))
            {
                countDownInfo.PlanCount = planCount;
            }
            else
            {
                text += Formatter.FormatErrorMessage("活动数量不能为空，只能为整数");
            }

			if (!string.IsNullOrEmpty(this.txtPrice.Text))
			{
				decimal countDownPrice;
				if (decimal.TryParse(this.txtPrice.Text.Trim(), out countDownPrice))
				{
					countDownInfo.CountDownPrice = countDownPrice;
				}
				else
				{
					text += Formatter.FormatErrorMessage("价格填写格式不正确");
				}
			}
			if (!string.IsNullOrEmpty(text))
			{
				this.ShowMsg(text, false);
				return;
			}
			countDownInfo.Content = Globals.HtmlEncode(this.txtContent.Text);
            countDownInfo.PlanCount = maxCount * 10;   //TODO
            countDownInfo.SupplierId = UserHelper.GetAssociatedSupplierId(HiContext.Current.User.UserId);
			if (PromoteHelper.AddCountDown(countDownInfo))
			{
				this.ShowMsg("添加限时抢购活动成功", true);
				return;
			}
			this.ShowMsg("添加限时抢购活动失败", true);
		}
	}
}
