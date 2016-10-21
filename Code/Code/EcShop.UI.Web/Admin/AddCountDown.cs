using ASPNET.WebControls;
using EcShop.ControlPanel.Promotions;
using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Entities.Promotions;
using EcShop.Entities.Store;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Linq;
namespace EcShop.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.CountDown)]
	public class AddCountDown : AdminPage
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
        protected SupplierDropDownList ddlSupplier;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnAddCountDown.Click += new System.EventHandler(this.btnbtnAddCountDown_Click);
			if (!this.Page.IsPostBack)
			{
				this.dropCategories.DataBind();
				this.dropGroupBuyProduct.DataBind();
                //this.HourDropDownList1.DataBind();
                //this.drophours.DataBind();
                this.ddlSupplier.BindDataBind();
			}
		}
		private void btnbtnAddCountDown_Click(object sender, System.EventArgs e)
		{
            try
            {
                string[] Start_hour = Request.Form["sl_hoursByStart"].Split(',').Where(t=>!string.IsNullOrEmpty(t)).ToArray() ;
                string[] Start_time = Request.Form["StartTimes"].Split(',');
                string[] End_hour = Request.Form["sl_hoursByEnd"].Split(',') ;
                string[] End_time = Request.Form["EndTimes"].Split(',').Where(t =>!string.IsNullOrEmpty(t)).ToArray(); ;
                if (Start_hour.Length != Start_time.Length || Start_hour.Length != End_time.Length || Start_hour.Length != End_time.Length)
                {
                    this.ShowMsg("开始和结束时间参数不能为空！", false);
                    return;
                }
                List<CountDownInfo> cdlist = new List<CountDownInfo>();

                CountDownInfo countDownInfo = null;
                string text = string.Empty;

                for (int i = 0; i < Start_time.Length; i++)
                {
                    countDownInfo = new CountDownInfo();

                    #region 请选择限时抢购商品
                    if (this.dropGroupBuyProduct.SelectedValue > 0)
                    {
                        // 移到下面与时间一起判断
                        //if (PromoteHelper.ProductCountDownExist(this.dropGroupBuyProduct.SelectedValue.Value))
                        //{
                        //    this.ShowMsg("已经存在此商品的限时抢购活动", false);
                        //    return;
                        //}
                        countDownInfo.ProductId = this.dropGroupBuyProduct.SelectedValue.Value;
                    }
                    else
                    {
                        text += Formatter.FormatErrorMessage("请选择限时抢购商品");
                    }
                    #endregion

                    bool timeValidated = false;
                    #region 组装日期数据
                    countDownInfo.EndDate = Convert.ToDateTime(End_time[i]).AddHours(double.Parse(End_hour[i]));//this.calendarEndDate.SelectedDate.Value.AddHours((double)this.HourDropDownList1.SelectedValue.Value);
                    countDownInfo.StartDate = Convert.ToDateTime(Start_time[i]).AddHours(double.Parse(Start_hour[i]));// this.calendarStartDate.SelectedDate.Value.AddHours((double)this.drophours.SelectedValue.Value);
                    if (System.DateTime.Compare(countDownInfo.StartDate, countDownInfo.EndDate) >= 0)
                    {
                        text += Formatter.FormatErrorMessage("开始日期必须要早于结束日期");
                    }
                    else
                    {
                        //countDownInfo.StartDate = this.calendarStartDate.SelectedDate.Value.AddHours((double)this.drophours.SelectedValue.Value);
                        timeValidated = true;
                    }
                    if (timeValidated && PromoteHelper.ProductCountDownExist(this.dropGroupBuyProduct.SelectedValue.Value, countDownInfo.StartDate, countDownInfo.EndDate, 0))
                    {
                        this.ShowMsg("已经存在此商品该时间段的限时抢购活动", false);
                        return;
                    }
                    #endregion

                    #region 数字验证
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

                    if (maxCount > planCount)
                    {
                        text += Formatter.FormatErrorMessage("抢购数量不能大于活动数量");
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
                    #endregion

                    countDownInfo.Content = Globals.HtmlEncode(this.txtContent.Text);

                    cdlist.Add(countDownInfo);
                }

                if (!string.IsNullOrEmpty(text))
                {
                    this.ShowMsg(text, false);
                    return;
                }

                string RsStr = string.Empty;
                if (cdlist != null)
                {
                    cdlist.ForEach(t =>
                    {
                        bool rest = PromoteHelper.AddCountDown(t);
                        if (!rest)
                        {
                            RsStr += t.StartDate + "~" + t.EndDate + ";";
                        }
                    });

                }
                if (string.IsNullOrEmpty(RsStr))
                {
                    this.ShowMsg("添加限时抢购活动成功", true);
                    return;
                }
                this.ShowMsg("添加限时抢购活动部分失败:" + RsStr, true);
            }
            catch (Exception ee)
            {
                this.ShowMsg("必填参数为空，或格式错误！",false);
                return;
            }
		}
	}
}
