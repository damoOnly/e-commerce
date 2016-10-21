using ASPNET.WebControls;
using EcShop.ControlPanel.Commodities;
using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Core.Enums;
using EcShop.Entities.PO;
using EcShop.Entities.Store;
using EcShop.Membership.Context;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.Reflection;
using NPOI.HSSF.UserModel;
using System.IO;
using NPOI.SS.UserModel;
using System.Web;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Data.OleDb;

namespace EcShop.UI.Web.Admin.PO
{
    [PrivilegeCheck(Privilege.POManage)]
    public class POList : AdminPage
    {
        private string _dataPath;
        private string _PdfPath;
        private string poNumber;
        private int? supplierId;
        private string hsInOut;
        private int Status = -1;
        private int QPStatus = -1;
        private int CIStatus = -1;
        private System.DateTime? startDate;
        private System.DateTime? endDate;


        protected System.Web.UI.WebControls.DropDownList ddlStatus;
        protected System.Web.UI.WebControls.DropDownList ddlCIStatus;
        protected System.Web.UI.WebControls.DropDownList ddlQPStatus;
        protected WebCalendar calendarStartDate;
        protected WebCalendar calendarEndDate;
        protected System.Web.UI.WebControls.TextBox txtPONumber;
        protected System.Web.UI.WebControls.TextBox txtHSInOut;

        protected System.Web.UI.WebControls.Button btnSearch;
        protected System.Web.UI.WebControls.Button btnExportData;
        protected System.Web.UI.WebControls.Button btnExportPdf;
        protected System.Web.UI.WebControls.Button btnExportItem;
        protected System.Web.UI.WebControls.Button btnImportItem;

        protected System.Web.UI.WebControls.FileUpload fileUploader;

        protected ASPNET.WebControls.PageSize hrefPageSize;
        protected Pager pager1;
        protected System.Web.UI.WebControls.Repeater rpPO;
        protected Pager pager;

        protected System.Web.UI.WebControls.HiddenField hidUserid;
        protected System.Web.UI.WebControls.HiddenField hidUserName;
        protected SupplierDropDownList ddlSupplier;
        protected void Page_Load(object sender, System.EventArgs e)
        {
            this._dataPath = this.Page.Request.MapPath("~/storage/data/poImport/" + DateTime.Now.ToString("yyyyMM"));
            this._PdfPath = "/storage/data/poToPdf/" + DateTime.Now.ToString("yyyyMM") + "/";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            this.btnExportData.Click += new System.EventHandler(this.btnExportData_Click);
            this.btnExportPdf.Click += new System.EventHandler(this.btnExportPdf_Click);
            this.btnExportItem.Click += new System.EventHandler(this.btnExportItem_Click);
            this.btnImportItem.Click += new System.EventHandler(this.btnImportItem_Click);


            var member = HiContext.Current.User;
            if (member != null && !member.IsLockedOut)
            {
                hidUserid.Value = member.UserId.ToString();
                hidUserName.Value = member.Username;
            }
            else
            {
                this.ShowMsg("获取用户信息失败，请重新登录", false);
                return;
            }
            if (!this.Page.IsPostBack)
            {
                this.ddlSupplier.DataBind();
                this.BindProducts("CreateTime", SortAction.Desc);

            }
            CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
        }
        private void btnSearch_Click(object sender, System.EventArgs e)
        {
            this.ReloadProductOnSales(true);
        }

        #region 绑定数据时更改显示状态
        /// <summary>
        /// 绑定数据时更改显示状态 订单状态（0：创建；1：招商确认；2：关务认领；3：关务申报；4：申报成功；5：申报失败；6：已入库）
        /// </summary>
        protected void rpt_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Label lblPOType = (Label)e.Item.FindControl("lblPOType");
            if (lblPOType != null)
            {
                lblPOType.Text = lblPOType.Text == "1" ? "采购单" : "其他";
            }
            Label lblPOStatus = (Label)e.Item.FindControl("lblPOStatus");
            if (lblPOStatus != null)
            {
                switch (lblPOStatus.Text)
                {
                    case "0":
                        lblPOStatus.Text = "创建订单";
                        break;
                    case "1":
                        lblPOStatus.Text = "招商确认";
                        break;
                    case "2":
                        lblPOStatus.Text = "关务认领";
                        break;
                    case "3":
                        lblPOStatus.Text = "报关信息录入完成";
                        break;
                    case "4":
                        lblPOStatus.Text = "关务申报";
                        break;
                    case "5":
                        lblPOStatus.Text = "申报成功";
                        break;
                    case "6":
                        lblPOStatus.Text = "申报失败";
                        break;
                    case "7":
                        lblPOStatus.Text = "报关全部成功，发送WMS";
                        break;
                    case "8":
                        lblPOStatus.Text = "已入库";
                        break;
                    default:
                        lblPOStatus.Text = "未知";
                        break;
                }
            }
            Label lblQPStatus = (Label)e.Item.FindControl("lblQPStatus");
            if (lblQPStatus != null)
            {
                switch (lblQPStatus.Text)
                {
                    case "0":
                        lblQPStatus.Text = "未入库申报";
                        break;
                    case "1":
                        lblQPStatus.Text = "已入库申报";
                        break;
                    case "2":
                        lblQPStatus.Text = "入库申报失败";
                        break;
                    default:
                        lblQPStatus.Text = "未知";
                        break;
                }
            }
            Label lblCIStatus = (Label)e.Item.FindControl("lblCIStatus");
            if (lblCIStatus != null)
            {
                switch (lblCIStatus.Text)
                {
                    case "0":
                        lblCIStatus.Text = "未商检入库";
                        break;
                    case "1":
                        lblCIStatus.Text = "已商检入库";
                        break;
                    case "2":
                        lblCIStatus.Text = "商检入库失败";
                        break;
                    default:
                        lblCIStatus.Text = "未知";
                        break;
                }
            }
            Label lblSendWMSByPO = (Label)e.Item.FindControl("lblSendWMSByPO");
            if (lblSendWMSByPO != null)
            {
                lblSendWMSByPO.Text = lblSendWMSByPO.Text == "1" ? "是" : "否";
            }
        }
        #endregion

        #region 初始化绑定商品数据
        /// <summary>
        /// 初始化绑定商品数据
        /// </summary>
        /// <param name="sortBy"></param>
        /// <param name="sortOrder"></param>
        private void BindProducts(string sortBy, SortAction sortOrder)
        {
            DbQueryResult purchaseOrder = this.GetData(sortBy, sortOrder);
            this.rpPO.DataSource = purchaseOrder.Data;
            this.rpPO.DataBind();

            this.pager1.TotalRecords = (this.pager.TotalRecords = purchaseOrder.TotalRecords);
        }

        private DbQueryResult GetData(string sortBy, SortAction sortOrder)
        {
            this.LoadParameters();
            PurchaseOrderQuery purchaseOrderQuery = new PurchaseOrderQuery
            {
                PageSize = this.pager.PageSize,
                PageIndex = this.pager.PageIndex,
                SortOrder = sortOrder,
                SortBy = sortBy,
                StartDate = this.startDate,
                EndDate = this.endDate,
                PONumber = this.poNumber,
                SupplierId = this.supplierId,
                POStatus = this.Status,
                HSInOut = this.hsInOut,
                QPStatus=this.QPStatus,
                CIStatus=this.CIStatus
            };
            Globals.EntityCoding(purchaseOrderQuery, true);
            return PurchaseOrderHelper.GetPurchaseOrderList(purchaseOrderQuery);
        }
        #endregion

        #region 查询商品数据
        /// <summary>
        /// 查询商品数据
        /// </summary>
        /// <param name="isSearch"></param>
        private void ReloadProductOnSales(bool isSearch)
        {
            System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
            nameValueCollection.Add("poNumber", Globals.UrlEncode(this.txtPONumber.Text.Trim()));
            nameValueCollection.Add("hsInOut", Globals.UrlEncode(this.txtHSInOut.Text.Trim()));
            nameValueCollection.Add("supplierId", ddlSupplier.SelectedValue.ToString());

            nameValueCollection.Add("pageSize", this.pager.PageSize.ToString());
            if (!isSearch)
            {
                nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString());
            }
            if (this.calendarStartDate.SelectedDate.HasValue)
            {
                nameValueCollection.Add("startDate", this.calendarStartDate.SelectedDate.Value.ToString());
            }
            if (this.calendarEndDate.SelectedDate.HasValue)
            {
                nameValueCollection.Add("endDate", this.calendarEndDate.SelectedDate.Value.ToString());
            }

            nameValueCollection.Add("Status", this.ddlStatus.SelectedValue.ToString());
            nameValueCollection.Add("QPStatus", this.ddlQPStatus.SelectedValue.ToString());
            nameValueCollection.Add("CIStatus", this.ddlCIStatus.SelectedValue.ToString());
            base.ReloadPage(nameValueCollection, false);
        }
        #endregion

        #region 加载查询参数
        /// <summary>
        /// 加载查询参数
        /// </summary>
        private void LoadParameters()
        {
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["poNumber"]))
            {
                this.poNumber = Globals.UrlDecode(this.Page.Request.QueryString["poNumber"]);
            }

            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["hsInOut"]))
            {
                this.hsInOut = Globals.UrlDecode(this.Page.Request.QueryString["hsInOut"]);
            }

            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["supplierId"]))
            {
                int tmpsupplierId = -1;
                if (int.TryParse(this.Page.Request.QueryString["supplierId"], out tmpsupplierId))
                {
                    this.supplierId = tmpsupplierId;
                }
            }

            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["startDate"]))
            {
                this.startDate = new System.DateTime?(System.DateTime.Parse(this.Page.Request.QueryString["startDate"]));
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["endDate"]))
            {
                this.endDate = new System.DateTime?(System.DateTime.Parse(this.Page.Request.QueryString["endDate"]));
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["Status"]))//
            {
                int tmpStatus = -1;
                if (int.TryParse(this.Page.Request.QueryString["Status"], out tmpStatus))
                {
                    this.Status = tmpStatus;
                }
            }

            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["QPStatus"]))//
            {
                int tmpStatus = -1;
                if (int.TryParse(this.Page.Request.QueryString["QPStatus"], out tmpStatus))
                {
                    this.QPStatus = tmpStatus;
                }
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["CIStatus"]))//
            {
                int tmpStatus = -1;
                if (int.TryParse(this.Page.Request.QueryString["CIStatus"], out tmpStatus))
                {
                    this.CIStatus = tmpStatus;
                }
            }


            this.txtPONumber.Text = this.poNumber;
            this.txtHSInOut.Text = this.hsInOut;
            this.ddlSupplier.SelectedValue = this.supplierId;

            this.calendarStartDate.SelectedDate = this.startDate;
            this.calendarEndDate.SelectedDate = this.endDate;
            this.ddlStatus.SelectedValue = this.Status.ToString();
            this.ddlQPStatus.SelectedValue = this.QPStatus.ToString();
            this.ddlCIStatus.SelectedValue = this.CIStatus.ToString();

        }
        #endregion


        #region 导出采购明细供招商修改后导入
        private void btnImportItem_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (Request["RadioGroup"] == null || Request["RadioGroup"].Length < 0)
                {
                    this.ShowMsg("请选择要导入数据", false);
                    return;
                }
                var member = HiContext.Current.User;
                if (member == null || member.IsLockedOut)
                {
                    this.ShowMsg("获取用户信息失败，请重新登录", false);
                    return;
                }

                //是否有权限,是否自己的，是否在可变更状态
                ManagerHelper.CheckPrivilege(Privilege.POAdd);
                DataSet ds = PurchaseOrderHelper.GetPurchaseOrder(1, string.Format("IsDel=0 and id={0} AND POStatus=0 and CreateUserId={1}", Request["RadioGroup"].ToString(), member.UserId));
                if (ds == null || ds.Tables.Count <= 0 || ds.Tables[0].Rows.Count <= 0)
                {
                    this.ShowMsg("导入失败，请检查您是否有权限，或订单状态是否允许修改", false);
                    return;
                }


                if (!this.fileUploader.HasFile)
                {
                    this.ShowMsg("请先选择一个数据文件", false);
                    return;
                }
                if (this.fileUploader.PostedFile.ContentLength == 0 || this.fileUploader.PostedFile.ContentType != "application/vnd.ms-excel")
                {
                    this.ShowMsg("请上传正确的数据文件", false);
                    return;
                }
                if (!Directory.Exists(this._dataPath)) Directory.CreateDirectory(this._dataPath);
                string fileName = System.IO.Path.GetFileName(this.fileUploader.PostedFile.FileName);
                this.fileUploader.PostedFile.SaveAs(System.IO.Path.Combine(this._dataPath, fileName));
                DataTable dt = ExcelDataSource(this._dataPath + "/" + fileName);
                if (dt != null && dt.Rows.Count > 0)
                {
                    SavePOItem(int.Parse(Request["RadioGroup"].ToString()), dt,member.UserId);
                }
                else
                {
                    this.ShowMsg("导入文档无数据", false);
                }
            }
            catch (Exception ex)
            {
                this.ShowMsg("导入失败" + ex.Message, false);
            }
        }

        private bool SavePOItem(int POId, DataTable dt, int UserId)
        {
            //导入字段检查
            string strField = "商品代码,商品名称,产品条码,外箱条形码,订单数量,是否样品,生产日期,有效日期,生产批号,商品总净重(kg),商品总毛重(kg),币别,原币单价,成本价,销售价,装箱规格,箱子尺寸,箱数";
            foreach (string item in strField.Split(','))
            {
                if (!dt.Columns.Contains(item))
                {
                    this.ShowMsg("导入模板缺少字段:" + item, false);
                    return false;
                }
            }

            //获取所有币别 Name_CN
            DataSet dsBaseCurrency = PurchaseOrderHelper.GetBaseCurrency();
            if (dsBaseCurrency == null || dsBaseCurrency.Tables.Count <= 0 || dsBaseCurrency.Tables[0].Rows.Count <= 0)
            {
                this.ShowMsg("获取币别信息错误，请重试！", false);
                return false;
            }

            List<PurchaseOrderItemInfo> listInfo = new List<PurchaseOrderItemInfo>();
            dt.Columns.Add("数据检查结果");
            #region 检查并构造数据
            foreach (DataRow dr in dt.Rows)
            {
                PurchaseOrderItemInfo purchaseOrderItemInfo = new PurchaseOrderItemInfo();
                if (dr["商品代码"].ToString() == "")
                {
                    dr["数据检查结果"] = "商品代码不能为空";
                    continue;
                }
                purchaseOrderItemInfo.SkuId = dr["商品代码"].ToString();
                purchaseOrderItemInfo.POId = POId;
                purchaseOrderItemInfo.BoxBarCode = dr["外箱条形码"].ToString();
                int tempExpectQuantity = 0;
                if (!int.TryParse(dr["订单数量"].ToString(), out tempExpectQuantity))
                {
                    dr["数据检查结果"] = "订单数量转换错误，请输入正确的数字";
                    continue;
                }
                purchaseOrderItemInfo.ExpectQuantity = tempExpectQuantity;
                if (dr["是否样品"].ToString() != "" && dr["是否样品"].ToString() != "是" && dr["是否样品"].ToString() != "否")
                {
                    dr["数据检查结果"] = "是否样品，请输入‘是’或‘否’";
                    continue;
                }

                if (dr["是否样品"].ToString() != "")
                {
                    purchaseOrderItemInfo.IsSample = dr["是否样品"].ToString() == "否" ? false : true;
                }

                DateTime tempManufactureDate = DateTime.MinValue;
                if (dr["生产日期"].ToString() != "" && !DateTime.TryParse(dr["生产日期"].ToString(), out tempManufactureDate))
                {
                    dr["数据检查结果"] = "生产日期格式错误";
                    continue;
                }
                if (dr["生产日期"].ToString() != "")
                {
                    purchaseOrderItemInfo.ManufactureDate = tempManufactureDate;
                }

                DateTime tempEffectiveDate = DateTime.MinValue;
                if (dr["有效日期"].ToString() != "" && !DateTime.TryParse(dr["有效日期"].ToString(), out tempEffectiveDate))
                {
                    dr["数据检查结果"] = "有效日期格式错误";
                    continue;
                }
                if (dr["有效日期"].ToString() != "")
                {
                    purchaseOrderItemInfo.EffectiveDate = tempEffectiveDate;
                }
                purchaseOrderItemInfo.BatchNumber = dr["生产批号"].ToString();

                decimal tempNetWeight = 0;
                if (dr["商品总净重(kg)"].ToString() != "" && !decimal.TryParse(dr["商品总净重(kg)"].ToString(), out tempNetWeight))
                {
                    dr["数据检查结果"] = "商品总净重(kg)格式错误";
                    continue;
                }
                if (dr["商品总净重(kg)"].ToString() != "")
                {
                    purchaseOrderItemInfo.NetWeight = tempNetWeight;
                }

                decimal tempRoughWeight = 0;
                if (dr["商品总毛重(kg)"].ToString() != "" && !decimal.TryParse(dr["商品总毛重(kg)"].ToString(), out tempRoughWeight))
                {
                    dr["数据检查结果"] = "商品总毛重(kg)格式错误";
                    continue;
                }
                if (dr["商品总毛重(kg)"].ToString() != "")
                {
                    purchaseOrderItemInfo.RoughWeight = tempRoughWeight;
                }

                //需转换成Id
                if (dr["币别"].ToString() != "")
                {
                    DataRow[] drs = dsBaseCurrency.Tables[0].Select("Name_CN='" + dr["币别"].ToString() + "'");
                    if (drs.Length <= 0)
                    {
                        dr["数据检查结果"] = "未匹配到币别，请输入系统中存在币别";
                        continue;
                    }
                    purchaseOrderItemInfo.CurrencyId = int.Parse(drs[0]["ID"].ToString());
                }

                decimal tempOriginalCurrencyPrice = 0;
                if (dr["原币单价"].ToString() != "" && !decimal.TryParse(dr["原币单价"].ToString(), out tempOriginalCurrencyPrice))
                {
                    dr["数据检查结果"] = "原币单价格式错误";
                    continue;
                }
                if (dr["原币单价"].ToString() != "")
                {
                    purchaseOrderItemInfo.OriginalCurrencyPrice = tempOriginalCurrencyPrice;
                }

                decimal tempCostPrice = 0;
                if (dr["成本价"].ToString() != "" && !decimal.TryParse(dr["成本价"].ToString(), out tempCostPrice))
                {
                    dr["数据检查结果"] = "成本价格式错误";
                    continue;
                }
                if (dr["成本价"].ToString() != "")
                {
                    purchaseOrderItemInfo.CostPrice = tempCostPrice;
                }


                decimal tempSalePrice = 0;
                if (dr["销售价"].ToString() != "" && !decimal.TryParse(dr["销售价"].ToString(), out tempSalePrice))
                {
                    dr["数据检查结果"] = "销售价格式错误";
                    continue;
                }
                if (dr["销售价"].ToString() != "")
                {
                    purchaseOrderItemInfo.SalePrice = tempSalePrice;
                }

                purchaseOrderItemInfo.CartonSize = dr["装箱规格"].ToString();
                purchaseOrderItemInfo.CartonMeasure = dr["箱子尺寸"].ToString();

                int tempCases = 0;
                if (dr["箱数"].ToString() != "" && !int.TryParse(dr["箱数"].ToString(), out tempCases))
                {
                    dr["数据检查结果"] = "箱数转换错误，请输入正确的数字";
                    continue;
                }
                if (dr["箱数"].ToString() != "")
                {
                    purchaseOrderItemInfo.Cases = tempCases;
                }

                purchaseOrderItemInfo.UpdateUserId = UserId;

                //汇率
                if (tempOriginalCurrencyPrice != 0 && tempSalePrice != 0)
                {
                    purchaseOrderItemInfo.Rate = tempOriginalCurrencyPrice / tempSalePrice;
                }
                //合计价格
                if (tempCostPrice != 0)
                {
                    purchaseOrderItemInfo.TotalCostPrice = tempCostPrice * purchaseOrderItemInfo.ExpectQuantity;
                }
                if (tempSalePrice != 0)
                {
                    purchaseOrderItemInfo.TotalSalePrice = tempSalePrice * purchaseOrderItemInfo.ExpectQuantity;
                }
                if (tempOriginalCurrencyPrice != 0)
                {
                    purchaseOrderItemInfo.OriginalCurrencyTotalPrice = tempOriginalCurrencyPrice * purchaseOrderItemInfo.ExpectQuantity;
                }
                dr["数据检查结果"] = "数据无误";
                listInfo.Add(purchaseOrderItemInfo);
            }
            #endregion
            if (listInfo != null && listInfo.Count > 0 && listInfo.Count == dt.Rows.Count)
            {
                this.ShowMsg(PurchaseOrderHelper.ImportPoItem(listInfo), true);
            }
            else
            {
                //全部提交失败
                this.ShowMsg("存在数据错误,请查看错误Excel后导入", false);
                GetExcelReportItem(dt, "POImportError");
                return false;
            }
            return true;
        }
        #endregion

        #region 导入功能
        DataTable ExcelDataSource(string FileName)
        {
            System.Data.DataTable dt = null;
            string mystring = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + FileName + "';Extended Properties='Excel 8.0;HDR=Yes;IMEX=1'";
            OleDbConnection connxls = new OleDbConnection(mystring);
            try
            {
                connxls.Open();
                System.Data.DataTable schemaTable = connxls.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, null);
                for (int i = 0; i < 1; i++)// schemaTable.Rows.Count; i++)
                {
                    DataSet ds = new DataSet();
                    string tableName = schemaTable.Rows[i][2].ToString().Trim();
                    string TempSQL = "select * from [" + tableName + "]";
                    OleDbDataAdapter mydb = new OleDbDataAdapter(TempSQL, connxls);
                    mydb.Fill(ds);
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count == 0)
                        {
                            continue;
                        }
                        dt = ds.Tables[0];
                    }
                }
            }
            finally
            {
                connxls.Close();
            }
            return dt;
        }

        #endregion

        #region 导出采购明细供招商修改后导入
        private void btnExportItem_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (Request["RadioGroup"] == null || Request["RadioGroup"].Length < 0)
                {
                    this.ShowMsg("请选择要导出数据", false);
                    return;
                }

                DataSet ds = PurchaseOrderHelper.GetPurchaseOrderExportItem(int.Parse(Request["RadioGroup"].ToString()));
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    GetExcelReportItem(ds.Tables[0], "POImportAndExport");
                }
                else
                {
                    this.ShowMsg("未查询到数据", false);
                }
            }
            catch (Exception ex)
            {
                this.ShowMsg("导出失败" + ex.Message, false);
            }
        }
        #endregion

        #region 导出Excel GetExcelReportItem
        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="monthFlag"></param>
        public static void GetExcelReportItem(DataTable dt, string Name)
        {
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet(Name);

            //开始行，小标从0开始
            int StartRow = 1;
            //设置单元格样式
            HSSFCellStyle style = (HSSFCellStyle)book.CreateCellStyle();
            style.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            style.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
            style.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;//DASH_DOT_DOT
            style.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
            style.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;//HAIR
            style.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
            style.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;//MEDIUM_DASHED
            style.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;

            #region 给excel数据源
            //创建表头
            IRow rowColumns = sheet1.CreateRow(0);
            rowColumns.HeightInPoints = 30;
            for (int c = 0; c < dt.Columns.Count; c++)
            {
                HSSFCell cell = (HSSFCell)rowColumns.CreateCell(c);
                cell.SetCellValue(dt.Columns[c].ColumnName);
                cell.CellStyle = style;
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                //创建行
                IRow row = sheet1.CreateRow(i + StartRow);
                row.HeightInPoints = 30;
                //填充数据
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    HSSFCell cell = (HSSFCell)row.CreateCell(j);
                    cell.SetCellValue(dr[j].ToString());
                    cell.CellStyle = style;
                }
            }
            #endregion

            ImportExcel(book, string.Format("{0}" + Name, DateTime.Now.ToString("yyyy-MM-dd")));
        }

        #endregion

        #region 导出Pdf
        private void btnExportPdf_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (Request["RadioGroup"] == null || Request["RadioGroup"].Length < 0)
                {
                    this.ShowMsg("请选择要导出数据", false);
                    return;
                }

                DataSet ds = PurchaseOrderHelper.exportPurchaseOrder(int.Parse(Request["RadioGroup"].ToString()));
                if (ds != null && ds.Tables.Count > 1 && ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0)
                {
                    //判断目录是否存在，不存在则创建
                    string path = Request.MapPath(_PdfPath);
                    if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                    string strHeaderID = "";

                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        DataRow[] drs = ds.Tables[0].Select("HeaderID=" + dr["HeaderID"]);
                        if (drs.Length <= 0)
                        {
                            this.ShowMsg("并非所有入库单都有正确返回数据", false);
                            return;
                        }
                        strHeaderID += dr["HeaderID"].ToString() + ",";
                        DataSet dsData = new DataSet();
                        DataTable dt = ds.Tables[0].Clone();
                        dt.TableName = "dt";
                        foreach (DataRow item in drs)
                            dt.Rows.Add(item.ItemArray);
                        dsData.Tables.Add(dt);

                        RptClass rc = new RptClass();
                        if (rc.ExportRpt(Request.MapPath("/Admin/rptTemplates/POInvoice.rpt"), dsData, path + dsData.Tables[0].Rows[0]["ContractNo"] + "-POInvoice.pdf") != "导出成功")
                        {
                            this.ShowMsg("操作失败", false);
                            return;
                        }

                        if (rc.ExportRpt(Request.MapPath("/Admin/rptTemplates/POPacking.rpt"), dsData, path + dsData.Tables[0].Rows[0]["ContractNo"] + "-POPacking.pdf") != "导出成功")
                        {
                            this.ShowMsg("操作失败", false);
                            return;
                        }

                        if (rc.ExportRpt(Request.MapPath("/Admin/rptTemplates/POContract.rpt"), dsData, path + dsData.Tables[0].Rows[0]["ContractNo"] + "-POContract.pdf") != "导出成功")
                        {
                            this.ShowMsg("操作失败", false);
                            return;
                        }
                    }
                    //插入数据库文件地址
                    if (PurchaseOrderHelper.SaveFilePath(_PdfPath, strHeaderID.TrimEnd(',')))
                    {
                        this.ShowMsg("操作成功", true);
                    }
                    else
                    {
                        this.ShowMsg("操作失败", false);
                    }
                }
                else
                {
                    this.ShowMsg("未查询到数据，请检查报关单数据是否成功返回。", false);
                }
            }
            catch (Exception ex)
            {
                this.ShowMsg("失败：" + ex.Message, false);
            }
        }
        #endregion

        #region 导出采购单给供应商确认
        /// <summary>
        /// 导出
        /// </summary>
        private void btnExportData_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (Request["RadioGroup"] == null || Request["RadioGroup"].Length < 0)
                {
                    this.ShowMsg("请选择要导出数据", false);
                    return;
                }

                PurchaseOrderItemQuery purchaseOrderItemQuery = new PurchaseOrderItemQuery
                {
                    PageSize = 9999999,
                    PageIndex = 1,
                    SortOrder = SortAction.Desc,
                    SortBy = "CreateTime",
                    POId = int.Parse(Request["RadioGroup"].ToString())
                };

                DbQueryResult purchaseOrder = PurchaseOrderHelper.GetPurchaseOrderItemList(purchaseOrderItemQuery);
                System.Data.DataTable dtPO = purchaseOrder.Data as DataTable;
                if (dtPO != null && dtPO.Rows.Count > 0)
                {
                    GetExcelReport(dtPO, Globals.MapPath("/config/ProductListTemplate.xls"));
                }
                else
                {
                    this.ShowMsg("未查询到数据", false);
                }
            }
            catch (Exception ex)
            {
                this.ShowMsg("导出失败" + ex.Message, false);
            }
        }
        #endregion

        #region 导出Excel
        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="monthFlag"></param>
        public static void GetExcelReport(DataTable dt, string path)
        {
            HSSFWorkbook hssfworkbookDown;
            //读入刚复制的要导出的excel文件
            using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                hssfworkbookDown = new HSSFWorkbook(file);
                file.Close();
            }
            HSSFSheet sheet1 = (HSSFSheet)hssfworkbookDown.GetSheetAt(0);

            #region 表头信息
            //List No.(清单号码) #:
            HSSFCell cellPONumber = (HSSFCell)sheet1.GetRow(5).GetCell(12);
            cellPONumber.SetCellValue("List No.(清单号码) #:" + dt.Rows[0]["PONumber"].ToString());
            //Date(时间):
            HSSFCell cellDate = (HSSFCell)sheet1.GetRow(6).GetCell(12);
            cellDate.SetCellValue("Date(时间):" + DateTime.Now.ToString("yyyy-MM-dd"));

            //Company Name (公司名称）:
            HSSFCell cellCompanyName = (HSSFCell)sheet1.GetRow(8).GetCell(0);
            cellCompanyName.SetCellValue("Company Name (公司名称）:" + dt.Rows[0]["SupplierName"].ToString());
            //Address（地址）: 
            HSSFCell cellAddress = (HSSFCell)sheet1.GetRow(9).GetCell(0);
            cellAddress.SetCellValue("Address（地址）: " + dt.Rows[0]["Address"].ToString());
            //Contact(联系人）:
            HSSFCell cellContact = (HSSFCell)sheet1.GetRow(10).GetCell(0);
            cellContact.SetCellValue("Contact(联系人）:" + dt.Rows[0]["Contact"].ToString());
            //Mobil(手机）:
            HSSFCell cellMobil = (HSSFCell)sheet1.GetRow(11).GetCell(0);
            cellMobil.SetCellValue("Mobil(手机）:" + dt.Rows[0]["Mobile"].ToString());
            //Email（邮箱）:
            HSSFCell cellEmail = (HSSFCell)sheet1.GetRow(12).GetCell(0);
            cellEmail.SetCellValue("Email（邮箱）:" + dt.Rows[0]["Email"].ToString());
            //Tel（电话）:
            HSSFCell cellTel = (HSSFCell)sheet1.GetRow(13).GetCell(0);
            cellTel.SetCellValue("Tel（电话）:" + dt.Rows[0]["Phone"].ToString());
            //Fax（传真）:
            HSSFCell cellFax = (HSSFCell)sheet1.GetRow(13).GetCell(9);
            cellFax.SetCellValue("Fax（传真）:" + dt.Rows[0]["Fax"].ToString());
            //Category（商品品类）:
            HSSFCell cellCategory = (HSSFCell)sheet1.GetRow(14).GetCell(0);
            cellCategory.SetCellValue("Category（商品品类）:" + dt.Rows[0]["Category"].ToString());

            //Beneficiary Name: 
            HSSFCell cellBeneficiaryName = (HSSFCell)sheet1.GetRow(9).GetCell(12);
            cellBeneficiaryName.SetCellValue("Beneficiary Name: " + dt.Rows[0]["BeneficiaryName"].ToString());
            //Swift Code: 
            HSSFCell cellSwiftCode = (HSSFCell)sheet1.GetRow(10).GetCell(12);
            cellSwiftCode.SetCellValue("Swift Code: " + dt.Rows[0]["SwiftCode"].ToString());
            //Bank  Account: 
            HSSFCell cellBankAccount = (HSSFCell)sheet1.GetRow(11).GetCell(12);
            cellBankAccount.SetCellValue("Bank Account: " + dt.Rows[0]["BankAccount"].ToString());
            //Bank Name:  
            HSSFCell cellBankName = (HSSFCell)sheet1.GetRow(12).GetCell(12);
            cellBankName.SetCellValue("Bank Name:  " + dt.Rows[0]["BankName"].ToString());
            //Bank Address: 
            HSSFCell cellBankAddress = (HSSFCell)sheet1.GetRow(13).GetCell(12);
            cellBankAddress.SetCellValue("Bank Address: " + dt.Rows[0]["BankAddress"].ToString());
            //IBAN: 
            HSSFCell cellIBAN = (HSSFCell)sheet1.GetRow(14).GetCell(12);
            cellIBAN.SetCellValue("IBAN: " + dt.Rows[0]["IBAN"].ToString());
            #endregion


            //开始行，小标从0开始
            int StartRow = 17;
            double TotalAmout = 0;
            int TotalQuantity = 0;
            int TotalCases = 0;
            int RowCount = 0;
            //设置单元格样式
            HSSFCellStyle style = (HSSFCellStyle)hssfworkbookDown.CreateCellStyle();
            style.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            style.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
            style.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;//DASH_DOT_DOT
            style.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
            style.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;//HAIR
            style.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
            style.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;//MEDIUM_DASHED
            style.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
            //批量移动行
            sheet1.ShiftRows(StartRow, sheet1.LastRowNum, dt.Rows.Count, true, false, true);

            #region 给excel数据源
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                RowCount++;
                DataRow dr = dt.Rows[i];

                //创建行
                IRow row = sheet1.CreateRow(i + StartRow);
                //row.Height = 50;
                row.HeightInPoints = 60;
                //填充数据
                int cls = 0;
                //序号
                HSSFCell cell = (HSSFCell)row.CreateCell(cls++);
                cell.SetCellValue(i + 1);
                cell.CellStyle = style;
                //图片
                if (dr["ImageUrl1"].ToString().Length > 0 && File.Exists(Globals.MapPath(dr["ImageUrl1"].ToString())))
                {
                    AddPieChart(sheet1, hssfworkbookDown, Globals.MapPath(dr["ImageUrl1"].ToString()), i + StartRow, cls++);
                }
                else
                {
                    HSSFCell cellImg = (HSSFCell)row.CreateCell(cls++);
                    cellImg.SetCellValue("");
                    cellImg.CellStyle = style;
                }
                //商品代码
                HSSFCell cellSkuId = (HSSFCell)row.CreateCell(cls++);
                cellSkuId.SetCellValue(dr["SkuId"].ToString());
                cellSkuId.CellStyle = style;

                //商品中文名称
                HSSFCell cellProName = (HSSFCell)row.CreateCell(cls++);
                cellProName.SetCellValue(dr["ProductName"].ToString());
                cellProName.CellStyle = style;

                //商品英文名称
                HSSFCell cellEnglishName = (HSSFCell)row.CreateCell(cls++);
                cellEnglishName.SetCellValue(dr["EnglishName"].ToString());
                cellEnglishName.CellStyle = style;

                //品牌
                HSSFCell cellBrand = (HSSFCell)row.CreateCell(cls++);
                cellBrand.SetCellValue(dr["BrandName"].ToString());
                cellBrand.CellStyle = style;

                //商品规格
                HSSFCell cellProductStandard = (HSSFCell)row.CreateCell(cls++);
                cellProductStandard.SetCellValue(dr["ProductStandard"].ToString());
                cellProductStandard.CellStyle = style;

                //计量单位
                HSSFCell cellUnit = (HSSFCell)row.CreateCell(cls++);
                cellUnit.SetCellValue(dr["Unit"].ToString());
                cellUnit.CellStyle = style;

                //原产国
                HSSFCell cellCountryOfOrigin = (HSSFCell)row.CreateCell(cls++);
                cellCountryOfOrigin.SetCellValue(dr["CnArea"].ToString());
                cellCountryOfOrigin.CellStyle = style;
                //描述或者成分
                HSSFCell cellDescription = (HSSFCell)row.CreateCell(cls++);
                cellDescription.SetCellValue(dr["Ingredient"].ToString());
                cellDescription.CellStyle = style;
                //生产日期
                HSSFCell cellProductionDate = (HSSFCell)row.CreateCell(cls++);
                cellProductionDate.SetCellValue(dr["ManufactureDate"].ToString());
                cellProductionDate.CellStyle = style;
                //有效日期
                HSSFCell cellExpirationDate = (HSSFCell)row.CreateCell(cls++);
                cellExpirationDate.SetCellValue(dr["EffectiveDate"].ToString());
                cellExpirationDate.CellStyle = style;
                //生产批号
                HSSFCell cellProductionBatch = (HSSFCell)row.CreateCell(cls++);
                cellProductionBatch.SetCellValue(dr["BatchNumber"].ToString());
                cellProductionBatch.CellStyle = style;
                //数量 
                HSSFCell cellQuantity = (HSSFCell)row.CreateCell(cls++);
                cellQuantity.SetCellValue(dr["ExpectQuantity"].ToString());
                cellQuantity.CellStyle = style;
                TotalQuantity += dr["ExpectQuantity"].ToString().Length > 0 ? int.Parse(dr["ExpectQuantity"].ToString()) : 0;
                //箱数
                HSSFCell cellCases = (HSSFCell)row.CreateCell(cls++);
                cellCases.SetCellValue(dr["Cases"].ToString());
                cellCases.CellStyle = style;
                TotalCases += dr["Cases"].ToString().Length > 0 ? int.Parse(dr["Cases"].ToString()) : 0;
                //产品包装规格
                HSSFCell cellPackageSize = (HSSFCell)row.CreateCell(cls++);
                cellPackageSize.SetCellValue(dr["CartonSize"].ToString());
                cellPackageSize.CellStyle = style;
                //箱子尺寸
                HSSFCell cellCartonMeasure = (HSSFCell)row.CreateCell(cls++);
                cellCartonMeasure.SetCellValue(dr["CartonMeasure"].ToString());
                cellCartonMeasure.CellStyle = style;
                
                //产品条形码
                HSSFCell cellEANUnit = (HSSFCell)row.CreateCell(cls++);
                cellEANUnit.SetCellValue(dr["BarCode"].ToString());
                cellEANUnit.CellStyle = style;
                //外箱条形码
                HSSFCell cellEANCarton = (HSSFCell)row.CreateCell(cls++);
                cellEANCarton.SetCellValue(dr["BoxBarCode"].ToString());
                cellEANCarton.CellStyle = style;
                //零售价
                HSSFCell cellRetailPrice = (HSSFCell)row.CreateCell(cls++);
                cellRetailPrice.SetCellValue(dr["SalePrice"].ToString().Length > 0 ? decimal.Parse(dr["SalePrice"].ToString()).ToString("F2") : "");
                cellRetailPrice.CellStyle = style;
                //总价
                HSSFCell cellTotalAmout = (HSSFCell)row.CreateCell(cls++);
                cellTotalAmout.SetCellValue(dr["TotalSalePrice"].ToString().Length > 0 ? decimal.Parse(dr["TotalSalePrice"].ToString()).ToString("F2") : "");
                cellTotalAmout.CellStyle = style;

                TotalAmout += dr["TotalSalePrice"].ToString().Length > 0 ? double.Parse(dr["TotalSalePrice"].ToString()) : 0;
            }
            #endregion
            //Total:
            //
            HSSFCell cellTotalQuantity1 = (HSSFCell)sheet1.GetRow(StartRow + RowCount).GetCell(13);
            cellTotalQuantity1.SetCellValue(TotalQuantity);

            HSSFCell cellTotalCases = (HSSFCell)sheet1.GetRow(StartRow + RowCount).GetCell(14);
            cellTotalCases.SetCellValue(TotalCases);

            HSSFCell cellTotal = (HSSFCell)sheet1.GetRow(StartRow + RowCount).GetCell(20);
            cellTotal.SetCellValue(TotalAmout.ToString("F2"));

            //Say Total amout:   
            HSSFCell cellSayTotalAmout = (HSSFCell)sheet1.GetRow(StartRow + RowCount + 1).GetCell(0);
            cellSayTotalAmout.SetCellValue("Say Total amout:   " + TotalAmout.ToString("F2"));
            
            //Total Quantity: 
            HSSFCell cellTotalQuantity = (HSSFCell)sheet1.GetRow(StartRow + RowCount + 2).GetCell(0);
            cellTotalQuantity.SetCellValue("Total Quantity: " + TotalQuantity);


            ImportExcel(hssfworkbookDown, string.Format("{0}ProductList", DateTime.Now.ToString("yyyy-MM-dd")));
        }

        private static void ImportExcel(HSSFWorkbook workbook, string exportname)
        {
            MemoryStream ms = new MemoryStream();
            try
            {
                workbook.Write(ms);
                HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachment; filename=" + exportname + ".xls"));
                HttpContext.Current.Response.BinaryWrite(ms.ToArray());
                workbook = null;

            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                ms.Close();
                ms.Dispose();
            }
        }

        /// <summary>
        /// 向sheet插入图片
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="workbook"></param>
        /// <param name="fileurl"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        private static void AddPieChart(HSSFSheet sheet, HSSFWorkbook workbook, string fileurl, int row, int col)
        {
            try
            {
                byte[] bytes = System.IO.File.ReadAllBytes(fileurl);
                if (!string.IsNullOrEmpty(fileurl))
                {
                    int pictureIdx = workbook.AddPicture(bytes, NPOI.SS.UserModel.PictureType.JPEG);
                    HSSFPatriarch patriarch = (HSSFPatriarch)sheet.CreateDrawingPatriarch();
                    HSSFClientAnchor anchor = new HSSFClientAnchor(0, 0, 50, 25, col, row, col + 1, row + 1);
                    //##处理照片位置，【图片左上角为（col, row）第row+1行col+1列，右下角为（ col +1, row +1）第 col +1+1行row +1+1列，宽为100，高为50
                    HSSFPicture pict = (HSSFPicture)patriarch.CreatePicture(anchor, pictureIdx);
                    // pict.Resize();这句话一定不要，这是用图片原始大小来显示
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        /// <summary>
        /// 把双引号替换为&#34;，单引号替换为&#39;。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string ReplaceStr(string str)
        {
            return str.Replace("\"", "&#34;").Replace("'", "&#39;");
        }

        protected void rpPO_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
        {
            var member = HiContext.Current.User;
            if (member == null || member.IsLockedOut)
            {
                this.ShowMsg("获取用户信息失败，请重新登录", false);
                return;
            }
            if (e.CommandName == "del")
            {
                int num = System.Convert.ToInt32(e.CommandArgument);
                if (!PurchaseOrderHelper.Deletet(num, member.UserId))
                {
                    this.ShowMsg("该单状态不允许操作或您无权操作该单", false);
                    return;
                }
                this.ReloadProductOnSales(true);
            }
        }
    }
}
