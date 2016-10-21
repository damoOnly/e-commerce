using EcShop.ControlPanel.Promotions;
using EcShop.ControlPanel.Store;
using EcShop.Entities.Promotions;
using EcShop.Entities.Store;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using EcShop.UI.Web.Admin.promotion.Ascx;
using System;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.ProductPromotion)]
	public class EditProductPromotion : AdminPage
	{
		private int activityId;
		protected PromoteTypeRadioButtonList radPromoteType;
		protected TrimTextBox txtPromoteType;
		protected System.Web.UI.WebControls.TextBox txtCondition;
		protected System.Web.UI.WebControls.TextBox txtDiscountValue;
		protected PromotionView promotionView;
		protected System.Web.UI.WebControls.Button btnNext;
        protected System.Web.UI.WebControls.RadioButton radYes;
        protected System.Web.UI.WebControls.RadioButton radNo;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["activityId"], out this.activityId))
			{
				base.GotoResourceNotFound();
				return;
			}
			this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
			if (!this.Page.IsPostBack)
			{
				PromotionInfo promotion = PromoteHelper.GetPromotion(this.activityId);
				this.promotionView.Promotion = promotion;
				this.txtPromoteType.Text = ((int)promotion.PromoteType).ToString();
                if (promotion.IsAscend == 1)
                {
                    this.radYes.Checked = true;
                }
                else
                {
                    this.radNo.Checked = true;
                }
				if (promotion.PromoteType == PromoteType.QuantityDiscount)
				{
					this.radPromoteType.IsWholesale = true;
				}
				if (promotion.Condition != 0m)
				{

                    if (promotion.PromoteType == (PromoteType)7)
                    {
                        this.txtCondition.Text = promotion.Condition.ToString("F2");
                    }
                    else
                    {
                        this.txtCondition.Text = promotion.Condition.ToString("F0");
                    }
                   
				}
				if (promotion.DiscountValue != 0m)
				{
					if (promotion.PromoteType == PromoteType.SentProduct)
					{
						this.txtDiscountValue.Text = promotion.DiscountValue.ToString("F0");
						return;
					}
					this.txtDiscountValue.Text = promotion.DiscountValue.ToString("F2");
				}
			}
		}
		private void btnNext_Click(object sender, System.EventArgs e)
		{
			PromotionInfo promotion = this.promotionView.Promotion;
			promotion.ActivityId = this.activityId;
            if(PromoteHelper.GetActivityProductAmount(promotion.ActivityId)>0)
            {
                this.ShowMsg("已经添加促销商品的活动不可以修改，请先删除促销商品",false);
                return;

            }
			if (promotion.MemberGradeIds.Count <= 0)
			{
				this.ShowMsg("必须选择一个适合的客户", false);
				return;
			}
			if (promotion.StartDate.CompareTo(promotion.EndDate) > 0)
			{
				this.ShowMsg("开始日期应该小于结束日期", false);
				return;
			}
			promotion.PromoteType = (PromoteType)int.Parse(this.txtPromoteType.Text);
			if (promotion.PromoteType == PromoteType.QuantityDiscount)
			{
				this.radPromoteType.IsWholesale = true;
			}
			decimal condition = 0m;
			decimal discountValue = 0m;
			decimal.TryParse(this.txtCondition.Text.Trim(), out condition);
			decimal.TryParse(this.txtDiscountValue.Text.Trim(), out discountValue);
			promotion.Condition = condition;
			promotion.DiscountValue = discountValue;
            promotion.IsAscend = radYes.Checked ? 1 : 0;
			int num = PromoteHelper.EditPromotion(promotion);
			if (num == -1)
			{
				this.ShowMsg("编辑促销活动失败，可能是信填写有误，请重试", false);
				return;
			}
			if (num == -2)
			{
				this.ShowMsg("编辑促销活动失败，可能是选择的会员等级已经被删除，请重试", false);
				return;
			}
			if (num == 0)
			{
				this.ShowMsg("编辑促销活动失败，请重试", false);
				return;
			}
			this.ShowMsg("编辑促销活动成功", true);
		}
	}
}
