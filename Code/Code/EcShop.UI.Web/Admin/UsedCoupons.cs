using ASPNET.WebControls;
using EcShop.ControlPanel.Promotions;
using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Core.Enums;
using EcShop.Entities.Promotions;
using EcShop.Entities.Store;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.UsedCoupons)]
    public class UsedCoupons : AdminPage
    {
        private string couponName = string.Empty;
        private string couponOrder = string.Empty;
        private string userName = string.Empty;
        private string addUserName = string.Empty;
        private int? couponstatus;
        private int? CouponId;
        protected System.Web.UI.WebControls.TextBox txtCouponName;
        protected System.Web.UI.WebControls.TextBox txtOrderID;
        protected System.Web.UI.WebControls.DropDownList Dpstatus;
        protected System.Web.UI.WebControls.Button btnSearch;
        protected Grid grdCoupons;
        protected Pager pager;
        protected System.Web.UI.WebControls.TextBox txtUserName;
        protected System.Web.UI.WebControls.TextBox txtAddUserName;
        protected System.Web.UI.WebControls.HiddenField nowCouponId;
        protected System.Web.UI.WebControls.Button btnExport;
        protected WebCalendar calendarStart;
        protected WebCalendar calendarEnd;
        private System.DateTime? dateStart;
        private System.DateTime? dateEnd;
        private string FlagExport;
        protected void Page_Load(object sender, System.EventArgs e)
        {
            this.LoadParameters();
            if (!Page.IsPostBack)
            {
                if (FlagExport == "E")
                {
                    this.ExportExcelUserCoupons();
                }
                else
                {
                    this.BindCouponList();
                }
            }
        }
        private void ReloadHelpList(bool isSearch)
        {
            string couponName = Globals.UrlEncode(this.txtCouponName.Text.Trim());
            string OrderID = Globals.UrlEncode(this.txtOrderID.Text.Trim());
            string name = Globals.UrlEncode(this.txtUserName.Text.Trim());
            string addname = Globals.UrlEncode(this.txtAddUserName.Text.Trim());
            string nowid = Globals.UrlDecode(this.nowCouponId.Value);
            string begindate = Globals.UrlDecode(this.calendarStart.SelectedDate.ToString());
            string enddate = Globals.UrlDecode(this.calendarEnd.SelectedDate.ToString());
            System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
            nameValueCollection.Add("couponName", couponName);
            nameValueCollection.Add("OrderID", OrderID);
            nameValueCollection.Add("userName", name);
            nameValueCollection.Add("addUserName", addname);
            nameValueCollection.Add("CouponId", nowid);
            nameValueCollection.Add("begindate", begindate);
            nameValueCollection.Add("enddate", enddate);
            nameValueCollection.Add("CouponStatus", this.Dpstatus.SelectedValue);
            nameValueCollection.Add("flagExport", this.FlagExport);
            if (!isSearch)
            {
                nameValueCollection.Add("PageIndex", this.pager.PageIndex.ToString());
            }
            nameValueCollection.Add("SortOrder", SortAction.Desc.ToString());
            //if (this.Dpstatus.SelectedValue != "1")
            //{
            //    if (string.IsNullOrEmpty(OrderID) && string.IsNullOrEmpty(couponName) && string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(addUserName))
            //    {
            //        ShowMsg("使用状态没有选择 已使用，则需要填写优惠券名称或者订单号", false);
            //        return;
            //    }
            //}
            base.ReloadPage(nameValueCollection);
        }
        protected string IsCouponEnd(object endtime)
        {
            if (string.IsNullOrEmpty(endtime.ToString()))
            {
                return "";
            }
            System.DateTime dateTime = System.Convert.ToDateTime(endtime);
            if (dateTime.CompareTo(System.DateTime.Now) > 0)
            {
                return dateTime.ToString();
            }
            return "已过期";
        }
        protected void BindCouponList()
        {
            //if (string.IsNullOrEmpty(this.couponName)&& 
            //    string.IsNullOrEmpty(this.couponOrder)&&
            //    !this.couponstatus.HasValue&&
            //    !this.CouponId.HasValue)
            //{
            //    return;
            //}
            DbQueryResult couponsList = CouponHelper.GetCouponsList(new CouponItemInfoQuery
            {
                CounponName = this.couponName,
                OrderId = this.couponOrder,
                CouponId = this.CouponId,
                UserName = this.userName,
                AddUserName = this.addUserName,
                BeginTime = !this.dateStart.HasValue ? "" : this.dateStart.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                EndTime = !this.dateEnd.HasValue ? "" : (this.dateEnd.Value.ToString("yyyy-MM-dd") + " 23:59:59"),
                CouponStatus = this.couponstatus,
                PageIndex = this.pager.PageIndex,
                PageSize = this.pager.PageSize,
                SortBy = "GenerateTime",
                SortOrder = SortAction.Desc
            });
            this.pager.TotalRecords = couponsList.TotalRecords;
            this.grdCoupons.DataSource = couponsList.Data;
            this.grdCoupons.DataBind();
        }
        private void LoadParameters()
        {
            if (!base.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["couponName"]))
                {
                    this.couponName = Globals.UrlDecode(this.Page.Request.QueryString["couponName"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OrderID"]))
                {
                    this.couponOrder = Globals.UrlDecode(this.Page.Request.QueryString["OrderID"]);
                }
                if (!string.IsNullOrEmpty(base.Request.QueryString["CouponStatus"]))
                {
                    this.couponstatus = new int?(System.Convert.ToInt32(base.Request.QueryString["CouponStatus"]));
                }
                if (!string.IsNullOrEmpty(base.Request.QueryString["CouponId"]))
                {
                    this.CouponId = new int?(System.Convert.ToInt32(base.Request.QueryString["CouponId"]));
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["userName"]))
                {
                    this.userName = Globals.UrlDecode(this.Page.Request.QueryString["userName"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["addUserName"]))
                {
                    this.addUserName = Globals.UrlDecode(this.Page.Request.QueryString["addUserName"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["flagExport"]))
                {
                    this.FlagExport = Globals.UrlDecode(this.Page.Request.QueryString["flagExport"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["begindate"]))
                {
                    this.dateStart = new System.DateTime?(System.Convert.ToDateTime(base.Server.UrlDecode(this.Page.Request.QueryString["begindate"])));
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["enddate"]))
                {
                    this.dateEnd = new System.DateTime?(System.Convert.ToDateTime(base.Server.UrlDecode(this.Page.Request.QueryString["enddate"])));
                }
                if (this.dateStart > this.dateEnd)
                {
                    this.ShowMsg("发劵查询开始时间不能大于结束时间！", false);
                    return;
                }
                this.txtOrderID.Text = this.couponOrder;
                this.txtCouponName.Text = this.couponName;
                this.txtUserName.Text = this.userName;
                this.txtAddUserName.Text = this.addUserName;
                this.nowCouponId.Value = this.CouponId.ToString();
                this.calendarStart.SelectedDate = this.dateStart;
                this.calendarEnd.SelectedDate = this.dateEnd;
                this.Dpstatus.SelectedValue = System.Convert.ToString(this.couponstatus);
                return;
            }
            this.couponName = this.txtCouponName.Text;
            this.couponOrder = this.txtOrderID.Text;
        }
        protected void btnSearch_Click(object sender, System.EventArgs e)
        {
            FlagExport = "";
            this.ReloadHelpList(true);
        }

        private void ExportExcelUserCoupons()
        {
            DbQueryResult couponsList = CouponHelper.GetCouponsListToExport(new CouponItemInfoQuery
            {
                CounponName = this.couponName,
                OrderId = this.couponOrder,
                CouponId = this.CouponId,
                UserName = this.userName,
                AddUserName = this.addUserName,
                BeginTime = !this.dateStart.HasValue ? "" : this.dateStart.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                EndTime = !this.dateEnd.HasValue ? "" : (this.dateEnd.Value.ToString("yyyy-MM-dd") + " 23:59:59"),
                CouponStatus = this.couponstatus
            });
            DataTable dtResult = (DataTable)couponsList.Data;
            if (dtResult == null || dtResult.Rows.Count == 0)
            {
                this.ShowMsg("没有数据需要导出！", false);
                return;
            }
            DataTable dtcoupons = new DataTable();
            dtcoupons.Columns.Add("用户名", typeof(string));
            dtcoupons.Columns.Add("劵名称", typeof(string));
            dtcoupons.Columns.Add("发放时间", typeof(string));
            dtcoupons.Columns.Add("使用时间", typeof(string));
            dtcoupons.Columns.Add("使用金额", typeof(string));
            dtcoupons.Columns.Add("订单号", typeof(string));
            for (var i = 0; i < dtResult.Rows.Count; i++)
            {
                DataRow dr = dtcoupons.NewRow();
                dr["用户名"] = dtResult.Rows[i]["UserName"].ToString();
                dr["劵名称"] = dtResult.Rows[i]["Name"].ToString();
                dr["发放时间"] = dtResult.Rows[i]["GenerateTime"].ToString();
                dr["使用时间"] = dtResult.Rows[i]["UsedTime"].ToString();
                dr["使用金额"] = dtResult.Rows[i]["DiscountValue"].ToString();
                dr["订单号"] = dtResult.Rows[i]["Orderid"].ToString();
                dtcoupons.Rows.Add(dr);
            }
            MemoryStream ms = NPOIExcelHelper.ExportToExcel(dtcoupons, "优惠券使用明细");
            this.Page.Response.Clear();
            this.Page.Response.Buffer = false;
            this.Page.Response.Charset = "GB2312";
            if (!(this.calendarStart.SelectedDate.HasValue && this.calendarEnd.SelectedDate.HasValue))
            {
                this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=优惠券使用明细.xlsx");
            }
            else
            {
                this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=优惠券使用明细_" + this.dateStart.Value.ToString("yyyyMMdd") + "-" + this.dateEnd.Value.ToString("yyyyMMdd") + ".xlsx");
            }
            this.Page.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            this.Page.Response.ContentType = "application/octet-stream";
            this.Page.EnableViewState = false;
            this.Page.Response.BinaryWrite(ms.ToArray());
            ms.Dispose();
            ms.Close();
            this.Page.Response.End();
        }

        protected void btnExport_Click(object sender, System.EventArgs e)
        {
            ManagerHelper.CheckPrivilege(Privilege.UserCouponExportXls);
            FlagExport = "E";
            this.ReloadHelpList(true);
        }
    }
}
