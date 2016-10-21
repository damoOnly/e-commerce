using ASPNET.WebControls;
using EcShop.ControlPanel.Commodities;
using EcShop.ControlPanel.Promotions;
using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Core.Enums;
using EcShop.Entities.Promotions;
using EcShop.Entities.Store;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.IO;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.AdOrderList)]
    public class AdOrderList : AdminPage
	{
        private string OrderId = string.Empty;
        private string KeyWord = string.Empty;
        private DateTime StartTime;
        private DateTime EndTime;
        protected System.Web.UI.WebControls.TextBox txtOrderId;
        protected System.Web.UI.WebControls.TextBox txtKeyWord;
		protected System.Web.UI.WebControls.Button btnSearch;
        protected WebCalendar calendarStartDate;
        protected WebCalendar calendarEndDate;

		protected PageSize hrefPageSize;
		protected Pager pager;
		protected Grid grdCountDownsList;
		protected Pager pager1;

        protected System.Web.UI.WebControls.LinkButton btnCreateReport;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			this.LoadParameters();
			if (!base.IsPostBack)
			{
				this.BindCountDown();
			}
            this.btnCreateReport.Click += new System.EventHandler(this.btnCreateReport_Click);

			CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
		}
        private void btnCreateReport_Click(object sender, System.EventArgs e)
        {
            this.LoadParameters();
            DbQueryResult countDownList = PromoteHelper.ExprtAdOrderInfoList(new GroupBuyQuery
            {
                OrderId   = this.OrderId,
                starttime =this.StartTime.ToString(),
                endtime = this.EndTime.ToString(),
                keyword   =this.KeyWord,
                PageIndex =this.pager.PageIndex,
                PageSize  = this.pager.PageSize,
                SortBy    = "Ordertime",
                SortOrder = SortAction.Desc
            });
            DataTable dtResult =(DataTable)countDownList.Data;
            if (dtResult == null || dtResult.Rows.Count == 0)
            {
                this.ShowMsg("没有数据！", false);
                return;
            }
            foreach (DataColumn cl in dtResult.Columns)
            {
                switch (cl.ColumnName.ToLower())
                {
                    case "orderno":
                        cl.ColumnName = "订单号";
                        break;
                    case "ordertotal":
                        cl.ColumnName = "订单总金额";
                        break;
                    case "productname":
                        cl.ColumnName = "商品名称";
                        break;
                    case "barcode":
                        cl.ColumnName = "商品条码";
                        break;
                    case "sku":
                        cl.ColumnName = "Sku编码";
                        break;
                    case "itemadjustedprice":
                        cl.ColumnName = "商品单价";
                        break;
                    case "orderstatus":
                        cl.ColumnName = "订单状态";
                        break;
                    case "paymenttype":
                        cl.ColumnName = "支付类型";
                        break;
                    case "paymentstatus":
                        cl.ColumnName = "付款状态";
                        break;
                    case "ordertime":
                        cl.ColumnName = "订单创建时间";
                        break;
                    case "updatedate":
                        cl.ColumnName = "订单修改时间";
                        break;
                }
            }
            MemoryStream ms = NPOIExcelHelper.ExportToExcel(dtResult, "广告订单");
            this.Page.Response.Clear();
            this.Page.Response.Buffer = false;
            this.Page.Response.Charset = "GB2312";

            this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=广告订单.xlsx");

            this.Page.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            this.Page.Response.ContentType = "application/octet-stream";
            this.Page.EnableViewState = false;
            this.Page.Response.BinaryWrite(ms.ToArray());
            ms.Dispose();
            ms.Close();
            EventLogs.WriteOperationLog(Privilege.ReconciOrdersDetailsExcel, string.Format(CultureInfo.InvariantCulture, "用户{0}广告订单导出明细成功", new object[]
            {
                this.User.Identity.Name
            }));

            this.Page.Response.End();

        }
		private void BindCountDown()
		{
            DbQueryResult countDownList = PromoteHelper.GetAdOrderInfoList(new GroupBuyQuery
			{
				OrderId = this.OrderId,
                starttime = this.calendarStartDate.SelectedDate.HasValue ? this.StartTime.ToString() : "",
                endtime = this.calendarEndDate.SelectedDate.HasValue ? this.EndTime.ToString() : "",
                keyword=this.KeyWord,
				PageIndex = this.pager.PageIndex,
				PageSize = this.pager.PageSize,
                SortBy = "OrderTIme",
				SortOrder = SortAction.Desc
			});
			this.grdCountDownsList.DataSource = countDownList.Data;
			this.grdCountDownsList.DataBind();
			this.pager.TotalRecords = countDownList.TotalRecords;
			this.pager1.TotalRecords = countDownList.TotalRecords;
		}
		
		private void grdGroupBuyList_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
		{
			if (PromoteHelper.DeleteCountDown((int)this.grdCountDownsList.DataKeys[e.RowIndex].Value))
			{
				this.BindCountDown();
				this.ShowMsg("成功删除了选择的限时抢购活动", true);
				return;
			}
			this.ShowMsg("删除失败", false);
		}
		private void ReloadHelpList(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
            nameValueCollection.Add("OrderId", Globals.UrlEncode(this.txtOrderId.Text.Trim()));
			if (!isSearch)
			{
				nameValueCollection.Add("PageIndex", this.pager.PageIndex.ToString());
			}
            if (this.calendarStartDate.SelectedDate.HasValue)
            {
                nameValueCollection.Add("startDate", this.calendarStartDate.SelectedDate.Value.ToString());
            }
            if (this.calendarEndDate.SelectedDate.HasValue)
            {
                nameValueCollection.Add("endDate", this.calendarEndDate.SelectedDate.Value.ToString());
            }
            nameValueCollection.Add("KeyWord", this.txtKeyWord.Text.ToString());
			nameValueCollection.Add("PageSize", this.hrefPageSize.SelectedSize.ToString());
			nameValueCollection.Add("SortBy", this.grdCountDownsList.SortOrderBy);
			nameValueCollection.Add("SortOrder", SortAction.Desc.ToString());
			base.ReloadPage(nameValueCollection);
		}
		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			this.ReloadHelpList(true);
		}
		private void lkbtnDeleteCheck_Click(object sender, System.EventArgs e)
		{
			int num = 0;
			foreach (System.Web.UI.WebControls.GridViewRow gridViewRow in this.grdCountDownsList.Rows)
			{
				System.Web.UI.WebControls.CheckBox checkBox = (System.Web.UI.WebControls.CheckBox)gridViewRow.FindControl("checkboxCol");
				if (checkBox.Checked)
				{
					num++;
					int countDownId = System.Convert.ToInt32(this.grdCountDownsList.DataKeys[gridViewRow.RowIndex].Value, System.Globalization.CultureInfo.InvariantCulture);
					PromoteHelper.DeleteCountDown(countDownId);
				}
			}
			if (num != 0)
			{
				this.BindCountDown();
				this.ShowMsg(string.Format(System.Globalization.CultureInfo.InvariantCulture, "成功删除\"{0}\"条限时抢购活动", new object[]
				{
					num
				}), true);
				return;
			}
			this.ShowMsg("请先选择需要删除的限时抢购活动", false);
		}
		private void LoadParameters()
		{
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OrderId"]))
			{
                this.OrderId = Globals.UrlDecode(this.Page.Request.QueryString["OrderId"]);
                this.txtOrderId.Text = this.OrderId;
			}
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["startDate"]))
            {
                this.StartTime =System.DateTime.Parse(this.Page.Request.QueryString["startDate"]);
                this.calendarStartDate.SelectedDate = this.StartTime;
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["endDate"]))
            {
                this.EndTime =System.DateTime.Parse(this.Page.Request.QueryString["endDate"]);
                this.calendarEndDate.SelectedDate = this.EndTime;
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["KeyWord"]))
            {
                this.KeyWord =this.Page.Request.QueryString["KeyWord"];
                this.txtKeyWord.Text = this.KeyWord;
            }
          
          
           
		}
	}
}
