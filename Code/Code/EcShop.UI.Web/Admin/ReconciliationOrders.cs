using ASPNET.WebControls;
using EcShop.ControlPanel.Members;
using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Core.Enums;
using EcShop.Entities.Members;
using EcShop.Entities.Store;
using EcShop.UI.ControlPanel.Utility;
using Members;
using pbxdata.dalEcShop.UI.Web.Admin;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using EcShop.SaleSystem.Member;
using System.Text.RegularExpressions;
namespace EcShop.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.ReconciOrdersView)]
    public class ReconciliationOrders : AdminPage
    {
        private System.DateTime? dateStart;
        private System.DateTime? dateEnd;

        protected WebCalendar calendarStart;
        protected WebCalendar calendarEnd;
        protected System.Web.UI.WebControls.Button btnQueryBalanceDetails;
        protected System.Web.UI.WebControls.LinkButton btnCreateReport;
        protected PageSize hrefPageSize;
        protected Pager pager;
        protected Grid grdBalanceDetails;
        protected Pager pager1;
        protected System.Web.UI.WebControls.LinkButton linkUpload;
        protected DropDownList ddlUploadingCheck;
        protected FileUpload fileCheck;
        protected override void OnInitComplete(System.EventArgs e)
        {
            base.OnInitComplete(e);
            this.btnQueryBalanceDetails.Click += new System.EventHandler(this.btnQueryBalanceDetails_Click);
            this.btnCreateReport.Click += new System.EventHandler(this.btnCreateReport_Click);
            this.linkUpload.Click += linkUpload_Click;
        }

        void linkUpload_Click(object sender, EventArgs e)
        {
            if (!fileCheck.HasFile)
            {
                this.ShowMsg("请选择文件！", false);
                return;
            }
            DataTable dt;

            string name = fileCheck.PostedFile.FileName;
            string path = Server.MapPath("/userloadfile/") + name;
            string expandName = fileCheck.FileName.Substring(name.LastIndexOf('.') + 1);
            //上传支付宝对账单
            if (ddlUploadingCheck.SelectedValue == "1")
            {
                //this.fileCheck.PostedFile.SaveAs(path);
                if (expandName.ToLower() == "csv")
                {
                    Stream stream = fileCheck.PostedFile.InputStream;
                    StreamReader sr = new StreamReader(stream, Encoding.GetEncoding("GB2312"));
                    string str = "";
                    string s = sr.ReadLine();
                    s = sr.ReadLine();
                    string accountId = "";
                    if (s != null)
                    {
                        if (s.Contains("["))
                        {
                            accountId = s.Substring(s.IndexOf('[') + 1);
                            accountId = accountId.Replace("]", "").Replace(",", "").Replace("\"", "").Replace("\t", "");
                        }
                    }
                    //获取起始日期 终止日期
                    s = sr.ReadLine();
                    DateTime? beginTime =null;
                    DateTime? endTime = null;
                    
                    System.Collections.Generic.List<string[]> list = new System.Collections.Generic.List<string[]>();
                    while (str != null)
                    {
                       str = sr.ReadLine();
                        if (str != null)
                        {
                            string[] xu = str.Replace("\t", "").Replace("\"", "").Split(',');
                            if (xu != null && xu.Length >= 12)
                            {
                                if (IsNumber(xu[0]))
                                {
                                    list.Add(xu);
                                }
                            }
                        }
                    }
                    sr.Close();
                    stream.Close();
                    dt = CVSToDataTable(list, accountId);
                    if (dt.Rows.Count > 0)
                    {
                        dt.DefaultView.Sort = "CreateTime asc";//重新设置排序
                        DataTable dtNew = dt.DefaultView.ToTable(); //保存在一张新表中
                        dt.Dispose();
                        beginTime = (DateTime)dtNew.Rows[0]["CreateTime"];
                        endTime = (DateTime)dtNew.Rows[dtNew.Rows.Count - 1]["CreateTime"];
                        MemberProcessor.DeleteEcshop_TradeDetails(beginTime, endTime, "AliPay");
                        Exception ex;
                        if (MemberProcessor.DataTableAddToTradeDetails(dtNew, out ex))
                        {
                            this.ShowMsg("文件上传成功！", true);
                        }
                        else
                        {
                            this.ShowMsg("文件上传失败！错误：" + ex.Message, false);
                        }
                    }
                    return;
                }
                else
                {
                    this.ShowMsg("上传支付对账单时请上传CVS格式文件！", false);
                    return;
                }
            }

            this.ShowMsg("文件上传成功！", true);
        }
      
        #region 上传支账单单击事件使用方法
        private bool IsNumber(String strNumber)
        {

            Regex objNotNumberPattern = new Regex("[^0-9.-]");

            Regex objTwoDotPattern = new Regex("[0-9]*[.][0-9]*[.][0-9]*");

            Regex objTwoMinusPattern = new Regex("[0-9]*[-][0-9]*[-][0-9]*");

            String strValidRealPattern = "^([-]|[.]|[-.]|[0-9])[0-9]*[.]*[0-9]+$";

            String strValidIntegerPattern = "^([-]|[0-9])[0-9]*$";

            Regex objNumberPattern = new Regex("(" + strValidRealPattern + ")|(" + strValidIntegerPattern + ")");

            return !objNotNumberPattern.IsMatch(strNumber) &&

                   !objTwoDotPattern.IsMatch(strNumber) &&

                   !objTwoMinusPattern.IsMatch(strNumber) &&

                   objNumberPattern.IsMatch(strNumber);

        }
        /// <summary>
        /// 创建支付宝对账单DataTable架构
        /// </summary>
        /// <returns></returns>
        private DataTable CreateTable()
        {
            DataTable dt = new DataTable();
            DataColumn cl = new DataColumn("TradingTime");
            DataColumn cl1 = new DataColumn("AccountId");
            DataColumn cl2 = new DataColumn("MerchantNumber");
            DataColumn cl3 = new DataColumn("SubMerchantNumber");
            DataColumn cl4 = new DataColumn("EquipmentNumber");
            DataColumn cl5 = new DataColumn("MicroOrderId");
            DataColumn cl6 = new DataColumn("MerchantOrderId");
            DataColumn cl7 = new DataColumn("UserIdentity");
            DataColumn cl8 = new DataColumn("TradeTypes");
            DataColumn cl9 = new DataColumn("TradingStatus");
            DataColumn cl10 = new DataColumn("PayingBank");
            DataColumn cl11 = new DataColumn("CurrencyType");
            DataColumn cl12 = new DataColumn("TotalAmount");
            DataColumn cl13 = new DataColumn("RedAmount");
            DataColumn cl14 = new DataColumn("WeChatRefundNumber");
            DataColumn cl15 = new DataColumn("MerchantsRefundNumber");
            DataColumn cl16 = new DataColumn("RefundAmount");
            DataColumn cl17 = new DataColumn("EnterpriseRefundAmount");
            DataColumn cl18 = new DataColumn("RefundType");
            DataColumn cl19 = new DataColumn("RefundStatus");
            DataColumn cl20 = new DataColumn("SkuName");
            DataColumn cl21 = new DataColumn("DataPacket");
            DataColumn cl22 = new DataColumn("CounterFee");
            DataColumn cl23 = new DataColumn("Rate");
            DataColumn cl24 = new DataColumn("DetailsType");
            DataColumn cl25 = new DataColumn("CreateTime", typeof(DateTime));
            dt.Columns.AddRange(new DataColumn[] { cl, cl1, cl2, cl3, cl4, cl5, cl6, cl7, cl8, cl9, cl10, cl11, cl12, cl13, cl14, cl15, cl16, cl17, cl18, cl19, cl20, cl21, cl22, cl23, cl24,cl25 });
            return dt;
        }
        /// <summary>
        /// 将CSV文件转换为DataTable
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private DataTable CVSToDataTable(List<string[]> list, string accountId)
        {
            DataTable dt = CreateTable();
            List<DataRow> listRow = new List<DataRow>();
            foreach (string[] s in list)
            {
                if (s[10] == "在线支付")
                {
                    DataRow row = dt.NewRow();
                    row["MicroOrderId"] = s[1].Trim();//业务流水号
                    row["MerchantOrderId"] = s[2].Trim();//商户订单号
                    DateTime datetime;
                    if (DateTime.TryParse(s[4].Trim(), out datetime))
                    {
                        row["CreateTime"] = datetime.ToString("yyyy-MM-dd HH:mm:ss");
                        row["TradingTime"] = datetime.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    else
                    {
                        row["CreateTime"] = row["TradingTime"] = s[4].Trim();//订单创建时间
                    }
                    row["UserIdentity"] = s[5].Trim();//对方账号
                    row["TotalAmount"] = s[6].Trim();//收入金额
                    row["TradeTypes"] = s[9].Trim();//交易渠道
                    row["TradingStatus"] = "SUCCESS";//交易状态
                    row["AccountId"] = accountId;//账号
                    row["DetailsType"] = "AliPay";
                    listRow.Add(row);
                }
                else if (s[10] == "收费")
                {
                    DataRow dr = listRow.Find(r => r["MerchantOrderId"].ToString() == s[2].Trim());
                    if (dr != null)
                    {
                        decimal TatalAmount = Math.Abs(decimal.Parse(dr["TotalAmount"].ToString().Trim()));
                        decimal CounterFee = Math.Abs(decimal.Parse(s[7].Trim()));
                        dr["CounterFee"] = CounterFee.ToString("F2");//服务费
                        dr["Rate"] = ((CounterFee / TatalAmount) * 100).ToString("F2") + "%";//服务费率 (CounterFee/TotalAmount)
                    }
                }
                else if (s[10] == "交易退款")
                {
                    DataRow dr = listRow.Find(r => r["MerchantOrderId"].ToString() == s[2].Trim());
                    DataRow row = dt.NewRow();
                     decimal CounterFee = Math.Abs(decimal.Parse(s[7].Trim()));
                    if (dr != null)
                    {
                        foreach (DataColumn c in dt.Columns)
                        {
                            row[c.ColumnName] = dr[c.ColumnName];
                        }
                       
                    }
                    else
                    {
                        row["MicroOrderId"] = s[1].Trim();//业务流水号
                        row["MerchantOrderId"] = s[2].Trim();//商户订单号
                        DateTime datetime;
                        if (DateTime.TryParse(s[4].Trim(), out datetime))
                        {
                            row["CreateTime"] = datetime.ToString("yyyy-MM-dd HH:mm:ss");
                            row["TradingTime"] = datetime.ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        else
                        {
                            row["CreateTime"] = row["TradingTime"] = s[4].Trim();//订单创建时间
                        }
                        row["UserIdentity"] = s[5].Trim();//对方账号
                        row["TradeTypes"] = s[9].Trim();//交易渠道
                        row["AccountId"] = accountId;//账号
                        row["DetailsType"] = "AliPay";
                        
                    }
                    row["TotalAmount"] = "0";
                    row["RefundAmount"] = CounterFee;
                    row["TradingStatus"] = "REFUND";//交易状态
                    row["RefundType"] = "ORIGINAL";
                    row["RefundStatus"] = "SUCCESS";//退款状态
                    listRow.Add(row);
                }

            }
            foreach (DataRow r in listRow)
            {
                dt.Rows.Add(r);
            }
            return dt;
        }
      
        
        #endregion
        protected void Page_Load(object sender, System.EventArgs e)
        {
            this.LoadParameters();
            if (!this.Page.IsPostBack)
            {
                this.GetBalanceDetails();
            }
        }
        private void LoadParameters()
        {
            if (!this.Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["dateStart"]))
                {
                    this.dateStart = new System.DateTime?(System.Convert.ToDateTime(base.Server.UrlDecode(this.Page.Request.QueryString["dateStart"])));
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["dateEnd"]))
                {
                    this.dateEnd = new System.DateTime?(System.Convert.ToDateTime(base.Server.UrlDecode(this.Page.Request.QueryString["dateEnd"])));
                }
                this.calendarStart.SelectedDate = this.dateStart;
                this.calendarEnd.SelectedDate = this.dateEnd;
                return;
            }
            this.dateStart = this.calendarStart.SelectedDate;
            this.dateEnd = this.calendarEnd.SelectedDate;

        }
        private void ReBind(bool isSearch)
        {
            System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();

            nameValueCollection.Add("dateStart", this.calendarStart.SelectedDate.ToString());
            nameValueCollection.Add("dateEnd", this.calendarEnd.SelectedDate.ToString());
            nameValueCollection.Add("pageSize", this.pager.PageSize.ToString(System.Globalization.CultureInfo.InvariantCulture));
            if (!isSearch)
            {
                nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString(System.Globalization.CultureInfo.InvariantCulture));
            }
            base.ReloadPage(nameValueCollection);
        }
        private void GetBalanceDetails()
        {

            DbQueryResult balanceDetails = MemberHelper.GetReconciliationOrders(new ReconciliationOrdersQuery
            {
                StartDate = this.dateStart,
                EndDate = this.dateEnd,
                PageIndex = this.pager.PageIndex,
                PageSize = this.pager.PageSize,
                SortBy = "TradeDate",
                SortOrder = SortAction.Desc,

            });
            this.grdBalanceDetails.DataSource = balanceDetails.Data;
            this.grdBalanceDetails.DataBind();
            this.pager1.TotalRecords = (this.pager.TotalRecords = balanceDetails.TotalRecords);
        }
        private void btnCreateReport_Click(object sender, System.EventArgs e)
        {
            ManagerHelper.CheckPrivilege(Privilege.ReconciOrdersExcel);
            string startDate = this.calendarStart.SelectedDate.ToString();
            string EndSdate = this.calendarEnd.SelectedDate.ToString();
            if (string.IsNullOrEmpty(startDate) || string.IsNullOrEmpty(EndSdate))
            {
                this.ShowMsg("起止时间不能为空！", false);
                return;
            }

            System.TimeSpan ND = Convert.ToDateTime(EndSdate) - Convert.ToDateTime(startDate);
            double mday = ND.Days;
            if (mday > 31)
            {
                this.ShowMsg("查询起止时间不能大于31天！", false);
                return;
            }
            DbQueryResult balanceDetailsNoPage = MemberHelper.ExportReconciliationOrders(new ReconciliationOrdersQuery
            {
                StartDate = this.dateStart,
                EndDate = this.dateEnd,
                SortBy = "TradingTime",
                SortOrder = SortAction.Desc,

            });
            DataTable dtResult = (DataTable)balanceDetailsNoPage.Data;
            if (dtResult == null || dtResult.Rows.Count == 0)
            {
                this.ShowMsg("没有数据！", false);
                return;
            }
            MemoryStream ms = NPOIExcelHelper.ExportToExcel(dtResult, "对账单");


            this.Page.Response.Clear();
            this.Page.Response.Buffer = false;
            this.Page.Response.Charset = "GB2312";
            //this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=ReconciliationOrders.xlsx");
            if (!(this.calendarStart.SelectedDate.HasValue && this.calendarEnd.SelectedDate.HasValue))
            {
                this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=对账订单报表.xlsx");
            }
            else
            {
                this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=对账订单报表_" + this.dateStart.Value.ToString("yyyyMMdd") + "-" + this.dateEnd.Value.ToString("yyyyMMdd") + ".xlsx");
            }
            this.Page.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            this.Page.Response.ContentType = "application/octet-stream";
            this.Page.EnableViewState = false;
            this.Page.Response.BinaryWrite(ms.ToArray());
            ms.Dispose();
            ms.Close();
            EventLogs.WriteOperationLog(Privilege.ReconciOrdersExcel, string.Format(CultureInfo.InvariantCulture, "用户{0}生成的对账订单报告成功", new object[]
			{
                this.User.Identity.Name
			}));
            this.Page.Response.End();

            StringBuilder sbstr = new StringBuilder();

            string orderid = string.Empty;

            string text = string.Empty;
            sbstr.Append("付款日期");
            sbstr.Append(",订单号");
            sbstr.Append(",实际金额");
            sbstr.Append(",收款金额");
            sbstr.Append(",退款金额");
            sbstr.Append(",买家昵称");
            sbstr.Append(",拍单时间");
            sbstr.Append(",付款时间");
            sbstr.Append(",商品总金额");
            sbstr.Append(",订单金额");
            sbstr.Append(",优惠金额");
            sbstr.Append(",运费");
            sbstr.Append(",税额");
            sbstr.Append(",优惠券");
            sbstr.Append(",现金劵");
            sbstr.Append(",货到付款服务费");
            sbstr.Append(",订单备注");
            sbstr.Append(",买家留言");
            sbstr.Append(",收货地址");
            sbstr.Append(",收货人名称");
            sbstr.Append(",收货国家");
            sbstr.Append(",州/省");
            sbstr.Append(",城市");
            sbstr.Append(",区");
            sbstr.Append(",邮编");
            sbstr.Append(",联系电话");
            sbstr.Append(",手机");
            sbstr.Append(",买家选择物流");
            sbstr.Append(",最晚发货时间");
            sbstr.Append(",海外订单");
            sbstr.Append(",是否货到付款");
            sbstr.Append(",订单状态");
            sbstr.Append(",发货快递单号");
            sbstr.Append(",快递公司");
            sbstr.Append(",货主Id");
            sbstr.Append(",货主名称\r\n");
            foreach (System.Data.DataRow dataRow in ((System.Data.DataTable)balanceDetailsNoPage.Data).Rows)
            {
                sbstr.Append(dataRow["TradingTime"]);
                sbstr.AppendFormat(",\"\t{0}\"", dataRow["OrderId"]);
                sbstr.AppendFormat(",{0}", dataRow["ActualAmount"]);
                sbstr.AppendFormat(",{0}", dataRow["TotalAmount"]);
                sbstr.AppendFormat(",{0}", dataRow["RefundAmount"]);
                sbstr.AppendFormat(",{0}", dataRow["RealName"]);
                sbstr.AppendFormat(",{0}", dataRow["OrderDate"]);
                sbstr.AppendFormat(",{0}", dataRow["PayDate"]);
                sbstr.AppendFormat(",{0}", dataRow["Amount"]);
                sbstr.AppendFormat(",{0}", dataRow["OrderTotal"]);
                sbstr.AppendFormat(",{0}", dataRow["DiscountAmount"]);
                sbstr.AppendFormat(",{0}", dataRow["AdjustedFreight"]);
                sbstr.AppendFormat(",{0}", dataRow["Tax"]);
                sbstr.AppendFormat(",{0}", dataRow["CouponValue"]);
                sbstr.AppendFormat(",{0}", dataRow["VoucherValue"]);
                sbstr.AppendFormat(",{0}", dataRow["PayCharge"]);

                sbstr.AppendFormat(",\"{0}\"", dataRow["ManagerRemark"]);
                sbstr.AppendFormat(",\"{0}\"", dataRow["Remark"]);
                sbstr.AppendFormat(",\"{0}\"", dataRow["Address"]);
                sbstr.AppendFormat(",\"{0}\"", dataRow["ShipTo"]);
                sbstr.AppendFormat(",\"{0}\"", dataRow["Country"]);
                sbstr.AppendFormat(",\"{0}\"", dataRow["Province"]);
                sbstr.AppendFormat(",\"{0}\"", dataRow["City"]);
                sbstr.AppendFormat(",\"{0}\"", dataRow["Area"]);
                sbstr.AppendFormat(",\"\t{0}\"", dataRow["ZipCode"]);
                sbstr.AppendFormat(",\"\t{0}\"", dataRow["TelPhone"]);
                sbstr.AppendFormat(",\"\t{0}\"", dataRow["CellPhone"]);



                sbstr.AppendFormat(",\"{0}\"", dataRow["ModeName"]);
                sbstr.AppendFormat(",\"{0}\"", dataRow["LatestDelivery"]);
                sbstr.AppendFormat(",\"{0}\"", dataRow["OverseasOrders"]);
                sbstr.AppendFormat(",\"{0}\"", dataRow["CashDelivery"]);
                sbstr.AppendFormat(",\"{0}\"", dataRow["OrderStatus"]);
                sbstr.AppendFormat(",\"\t{0}\"", dataRow["ShipOrderNumber"]);
                sbstr.AppendFormat(",\"{0}\"", dataRow["ExpressCompanyName"]);
                sbstr.AppendFormat(",{0}", dataRow["ownerId"]);
                sbstr.AppendFormat(",\"{0}\"", dataRow["ownername"]);
                sbstr.Append("\r\n");
            }
            this.Page.Response.Clear();
            this.Page.Response.Buffer = false;
            this.Page.Response.Charset = "GB2312";
            //this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=ReconciliationOrders.csv");
            if (!(this.calendarStart.SelectedDate.HasValue && this.calendarEnd.SelectedDate.HasValue))
            {
                this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=对账订单报表.csv");
            }
            else
            {
                this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=对账订单报表_" + this.dateStart.Value.ToString("yyyyMMdd") + "-" + this.dateEnd.Value.ToString("yyyyMMdd") + ".csv");
            }
            this.Page.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            this.Page.Response.ContentType = "application/octet-stream";
            this.Page.EnableViewState = false;
            this.Page.Response.Write(sbstr.ToString());
            this.Page.Response.End();


        }
        private void btnQueryBalanceDetails_Click(object sender, System.EventArgs e)
        {
            try
            {
                string startDate = this.calendarStart.SelectedDate.ToString();
                string EndSdate = this.calendarEnd.SelectedDate.ToString();
                if (string.IsNullOrEmpty(startDate) || string.IsNullOrEmpty(EndSdate))
                {
                    this.ShowMsg("起止时间不能为空！", false);
                    return;
                }

                System.TimeSpan ND = Convert.ToDateTime(EndSdate) - Convert.ToDateTime(startDate);
                double mday = ND.Days;
                if (mday > 31)
                {
                    this.ShowMsg("查询起止时间不能大于31天！", false);
                    return;
                }
                this.ReBind(true);
            }
            catch (Exception ee)
            {
                this.ShowMsg("执行异常！", false);
            }
        }
    }
}
