using ASPNET.WebControls;
using EcShop.ControlPanel.Commodities;
using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Core.Enums;
using EcShop.Entities.Commodities;
using EcShop.Entities.Store;
using EcShop.Membership.Context;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin
{
    //[PrivilegeCheck(Privilege.SupplierOrderView)]
    public class SupplierProductOnSales : AdminPage
    {
        private string productName;
        private string productCode;
        private string BarCode;
        private int? categoryId;
        private int? tagId;
        private int? typeId;
        private int? importSourceId;
        private int? supplierId;
        private int? IsApproved;
        private int? IsAllClassify;

        private int? IsApprovedPrice;
        private ProductSaleStatus saleStatus = ProductSaleStatus.All;
        private System.DateTime? startDate;
        private System.DateTime? endDate;
        protected System.Web.UI.WebControls.TextBox txtSearchText;
        protected ProductCategoriesDropDownList dropCategories;
        protected BrandCategoriesDropDownList dropBrandList;
        protected ProductTagsDropDownList dropTagList;
        protected ProductTypeDownList dropType;
        protected System.Web.UI.WebControls.TextBox txtSKU;
        protected System.Web.UI.WebControls.TextBox txt_BarCode;
        protected WebCalendar calendarStartDate;
        protected WebCalendar calendarEndDate;
        protected System.Web.UI.WebControls.Button btnSearch;
        protected PageSize hrefPageSize;
        protected Pager pager1;
        protected ImageLinkButton btnDelete;

        protected ImageLinkButton btnReSubmitPriceApprove;
        protected ImageLinkButton btnGenerationQCode;
        protected ImportSourceTypeDropDownList ddlImportSourceType;
        protected SupplierDropDownList ddlSupplier;

        protected SaleStatusDropDownList dropSaleStatus;
        protected Grid grdProducts;
        protected Pager pager;
        protected ProductTagsLiteral litralProductTag;
        protected System.Web.UI.WebControls.TextBox txtReferralDeduct;
        protected System.Web.UI.WebControls.Literal litReferralDeduct;
        protected System.Web.UI.WebControls.TextBox txtSubMemberDeduct;
        protected System.Web.UI.WebControls.Literal litSubMemberDeduct;
        protected System.Web.UI.WebControls.TextBox txtSubReferralDeduct;
        protected System.Web.UI.WebControls.Literal litSubReferralDeduct;
        protected System.Web.UI.WebControls.Button btnUpdateProductTags;
        protected System.Web.UI.WebControls.Button btnUpdateProductDeducts;
        protected TrimTextBox txtProductTag;
        protected System.Web.UI.WebControls.Button btnInStock;
        protected System.Web.UI.WebControls.Button btnUnSale;
        protected System.Web.UI.WebControls.Button btnUpSale;
        protected System.Web.UI.WebControls.Button btnSetFreeShip;
        protected System.Web.UI.WebControls.Button btnCancelFreeShip;
        protected System.Web.UI.WebControls.DropDownList dropIsApproved;
        protected System.Web.UI.WebControls.DropDownList dropIsApprovedPrice;
        protected System.Web.UI.WebControls.DropDownList dropIsAllClassify;

        protected System.Web.UI.WebControls.LinkButton btnCreateReport;
        protected System.Web.UI.WebControls.LinkButton linkUpload;
        protected FileUpload fileCheck;


        protected void Page_Load(object sender, System.EventArgs e)
        {
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            this.btnReSubmitPriceApprove.Click += new System.EventHandler(this.btnReSubmitPriceApprove_Click);

            this.btnGenerationQCode.Click += new System.EventHandler(this.btnGenerationQCode_Click);

            this.linkUpload.Click += linkUpload_Click;
            this.btnCreateReport.Click += new System.EventHandler(this.btnCreateReport_Click);


            this.btnUpSale.Click += new System.EventHandler(this.btnUpSale_Click);
            this.btnUnSale.Click += new System.EventHandler(this.btnUnSale_Click);
            this.btnInStock.Click += new System.EventHandler(this.btnInStock_Click);
            this.btnCancelFreeShip.Click += new System.EventHandler(this.btnSetFreeShip_Click);
            this.btnSetFreeShip.Click += new System.EventHandler(this.btnSetFreeShip_Click);
            this.btnUpdateProductTags.Click += new System.EventHandler(this.btnUpdateProductTags_Click);
            this.btnUpdateProductDeducts.Click += new System.EventHandler(this.btnUpdateProductDeducts_Click);
            this.grdProducts.RowDataBound += new System.Web.UI.WebControls.GridViewRowEventHandler(this.grdProducts_RowDataBound);
            this.grdProducts.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdProducts_RowDeleting);
            this.dropSaleStatus.SelectedIndexChanged += new System.EventHandler(this.dropSaleStatus_SelectedIndexChanged);
            if (!this.Page.IsPostBack)
            {
                this.dropCategories.DataBind();
                this.dropBrandList.DataBind();
                this.dropTagList.DataBind();
                this.dropType.DataBind();
                this.dropSaleStatus.DataBind();
                this.ddlImportSourceType.DataBind();
                this.ddlSupplier.BindDataBind();
                supplierId = CheckSupplierRole();
                if (supplierId.HasValue && supplierId.Value != 0)
                {
                    ListItem item = this.ddlSupplier.Items.FindByValue(supplierId.ToString());
                    if (item != null)
                    {
                        item.Selected = true;
                        this.ddlSupplier.Enabled = false;
                        //this.ddlSupplier.Visible = false;
                    }
                }
                this.BindProducts();

                SiteSettings siteSettings = HiContext.Current.SiteSettings;
                this.litReferralDeduct.Text = "（全站统一比例：" + siteSettings.ReferralDeduct.ToString("F2") + " %）";
                this.litSubMemberDeduct.Text = "（全站统一比例：" + siteSettings.SubMemberDeduct.ToString("F2") + " %）";
                this.litSubReferralDeduct.Text = "（全站统一比例：" + siteSettings.SubReferralDeduct.ToString("F2") + " %）";
            }
            CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
        }
        /// <summary>
        /// 导入商品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            StringBuilder SbrError = new StringBuilder();
            if (expandName.ToLower() == "csv")
            {
                Stream stream = fileCheck.PostedFile.InputStream;
                StreamReader sr = new StreamReader(stream, Encoding.GetEncoding("GB2312"));
                string str = "";
                string s = sr.ReadLine();
                System.Collections.Generic.List<string[]> list = new System.Collections.Generic.List<string[]>();
                int i =2;
                #region  获取数据
                while (str != null)
                {
                    str = sr.ReadLine();
                    if (str != null)
                    {
                        string[] xu = str.Replace("\t", "").Replace("\"", "").Split(',');
                        if (xu != null)
                        {
                            if (IsNumber(xu[7]) && IsNumber(xu[8])) //数字类型验证
                            {
                                list.Add(xu);
                            }
                            else
                            {
                                SbrError.AppendFormat("第{0}行，一口价或成本必须为数字！", i);
                            }
                        }
                    }
                }
                sr.Close();
                stream.Close();
                dt = CVSToDataTable(list);
                #endregion

                if (!string.IsNullOrEmpty(SbrError.ToString()))
                {
                    this.ShowMsg("导入失败：" + SbrError.ToString(), false);
                    return;
                }

                #region 组装XML数据
                if(dt!=null&&dt.Rows.Count>0)
                {
                    string xmlProduct = "<Product ProductName=\"{0}\"  SysProductName=\"{1}\" EnglishName=\"{2}\"   HSBrand=\"{3}\" CnArea=\"{4}\" BarCode=\"{5}\" ProductStandard=\"{6}\" SalePrice=\"{7}\" CostPrice=\"{8}\" SupplierName=\"{9}\"/>";
                    StringBuilder sbProduct = new StringBuilder();
                    foreach(DataRow row in dt.Rows)
                    {
                        sbProduct.AppendFormat(xmlProduct,row[0],
                            row[1],
                            row[2],
                            row[3],
                            row[4],
                            row[5],
                            row[6],
                            row[7],
                            row[8],
                            row[9]);
                    }
                    DataTable dtRest = ProductHelper.ImportProductsList(sbProduct.ToString());
                    if(dtRest!=null&&dtRest.Rows.Count>0)
                    {
                        string checkMessage = string.Empty;
                        foreach(DataRow row in dtRest.Rows)
                        {
                            checkMessage += row[0] + ":" + row[1] + "<br/>";
                        }
                        this.ShowMsg(string.Format("批量导入失败：{0} 基础数据中不存在！", checkMessage), false);
                        return;
                    }
                    this.ShowMsg("批量导入成功", true);
                    return;
                 }

                #endregion 



            }
            else
            {
                this.ShowMsg("导入文件只支持CSV格式，请下载最新的模版！", false);
                return;
            }
        }
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
        /// 将CSV文件转换为DataTable
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private DataTable CVSToDataTable(List<string[]> list)
        {
            DataTable dt = CreateSupllyProductTable();
            List<DataRow> listRow = new List<DataRow>();
            foreach (string[] s in list)
            {
                DataRow row = dt.NewRow();
                row["ProductName"]     = s[0].Trim();//商品名称
                row["SysProductName"]  = s[1].Trim();//商品实际名称
                row["EnglishName"]     = s[2].Trim();//商品英文名称
                row["HSBrand"]         = s[3].Trim();//品牌名称
                row["CnArea"]          = s[4].Trim();//原产地
                row["BarCode"]         = s[5].Trim();//商品条码
                row["ProductStandard"] = s[6].Trim();//商品规格
                row["SalePrice"]       = s[7].Trim();//一口价
                row["CostPrice"]       = s[8].Trim();//成本价格
                row["SupplierName"]    = s[9].Trim(); //供应商名称
                listRow.Add(row);
            }
            foreach (DataRow r in listRow)
            {
                dt.Rows.Add(r);
            }
            return dt;
        }
        /// <summary>
        /// 创建供应商信息表
        /// </summary>
        /// <returns></returns>
        private DataTable CreateSupllyProductTable()
        {
            DataTable dt = new DataTable();
            DataColumn cl0  = new DataColumn("ProductName");
            DataColumn cl1 = new DataColumn("SysProductName");
            DataColumn cl2 = new DataColumn("EnglishName");
            DataColumn cl3 = new DataColumn("HSBrand");
            DataColumn cl4 = new DataColumn("CnArea");
            DataColumn cl5 = new DataColumn("BarCode");
            DataColumn cl6 = new DataColumn("ProductStandard");//商品规格
            DataColumn cl7 = new DataColumn("SalePrice");//一口价
            DataColumn cl8 = new DataColumn("CostPrice");//一口价
            DataColumn cl9 = new DataColumn("SupplierName");//供应商名称
            dt.Columns.AddRange(new DataColumn[] { cl0, cl1, cl2, cl3, cl4, cl5, cl6, cl7, cl8, cl9 });
            return dt;
        }
        private void btnCreateReport_Click(object sender, System.EventArgs e)
        {

            if (this.calendarStartDate.SelectedDate.HasValue)
            {
                this.startDate=Convert.ToDateTime(this.calendarStartDate.SelectedDate.Value.ToString());
            }
            if (this.calendarEndDate.SelectedDate.HasValue)
            {
                this.endDate=Convert.ToDateTime( this.calendarEndDate.SelectedDate.Value.ToString());
            }
            if (this.dropType.SelectedValue.HasValue)
            {
                this.typeId=int.Parse( this.dropType.SelectedValue.ToString());
            }
            if (this.ddlSupplier.SelectedValue.HasValue)
            {
                this.supplierId =int.Parse(this.ddlSupplier.SelectedValue.ToString());
            }
            if (this.dropIsApproved.SelectedValue != "-1")
            {
               this.IsApproved=int.Parse(this.dropIsApproved.SelectedValue);
            }

            if (this.dropIsApprovedPrice.SelectedValue != "-1")
            {
                this.IsApprovedPrice=int.Parse( this.dropIsApprovedPrice.SelectedValue);
            }
            if (this.dropIsAllClassify.SelectedValue != "-1")
            {
               this.IsAllClassify=int.Parse( this.dropIsAllClassify.SelectedValue);
            }
            if (this.dropCategories.SelectedValue.HasValue)
            {
                this.categoryId=int.Parse(this.dropCategories.SelectedValue.ToString());
            }
            if (this.ddlSupplier.SelectedValue.HasValue)
            {
                this.supplierId = int.Parse(this.ddlSupplier.SelectedValue.ToString());
            }
            ProductQuery productQuery = new ProductQuery
            {
                Keywords =this.txtSearchText.Text,
                ProductCode =this.txtSKU.Text,
                BarCode=this.txt_BarCode.Text,
                CategoryId = categoryId,
                PageSize =1000000,
                PageIndex =1,
                SortOrder = SortAction.Desc,
                SortBy = "DisplaySequence",
                StartDate =this.startDate,
                BrandId = this.dropBrandList.SelectedValue.HasValue ? this.dropBrandList.SelectedValue : null,
                TagId = this.dropTagList.SelectedValue.HasValue ? this.dropTagList.SelectedValue : null,
                TypeId = this.typeId,
                SaleStatus = this.saleStatus,
                EndDate = this.endDate,
                ImportSourceId = this.ddlImportSourceType.SelectedValue.HasValue ? this.ddlImportSourceType.SelectedValue : null,
                IsApproved =this.IsApproved,
                IsApprovedPrice = this.IsApprovedPrice,
                IsAllClassify = this.IsAllClassify,
                SupplierId = this.supplierId
            };
            if (this.categoryId.HasValue)
            {
                productQuery.MaiCategoryPath = CatalogHelper.GetCategory(this.categoryId.Value).Path;
            }

            Globals.EntityCoding(productQuery, true);
            DbQueryResult products = ProductHelper.GetProducts(productQuery,false);

            DataTable dtResult = (DataTable)products.Data;
            if (dtResult == null || dtResult.Rows.Count == 0)
            {
                this.ShowMsg("没有数据！", false);
                return;
            }
            foreach (DataColumn cl in dtResult.Columns)
            {
                switch (cl.ColumnName.ToLower())
                {
                    case "productname":
                        cl.ColumnName = "商品名称";
                        break;
                    case "barcode":
                        cl.ColumnName = "商品条码";
                        break;
                    case "productcode":
                        cl.ColumnName = "商品编码";
                        break;
                    case "stock":
                        cl.ColumnName = "库存";
                        break;
                    case "costprice":
                        cl.ColumnName = "成本";
                        break;
                    case "saleprice":
                        cl.ColumnName = "商品价格";
                        break;
                    case "salestatus":
                        cl.ColumnName = "商品状态";
                        break;
                    case "isapproved":
                        cl.ColumnName = "审核状态";
                        break;
                    case "isapprovedprice":
                        cl.ColumnName = "审核价状态";
                        break;
                    case "isallclassify":
                        cl.ColumnName = "备案状态";
                        break;
                    case "cnarea":
                        cl.ColumnName = "原产地";
                        break;
                    case "suppliername":
                        cl.ColumnName = "供应商名称";
                        break;
                   
                    case "productid":
                        cl.ColumnName = "商品ID";
                        break;
                }
            }
            
            //导出带图片
            //Export(dtResult);

            MemoryStream ms = NPOIExcelHelper.ExportToExcel(dtResult, "供应商商品明细");
            this.Page.Response.Clear();
            this.Page.Response.Buffer = false;
            this.Page.Response.Charset = "GB2312";

            this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=供应商商品明细.xlsx");

            this.Page.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            this.Page.Response.ContentType = "application/octet-stream";
            this.Page.EnableViewState = false;
            this.Page.Response.BinaryWrite(ms.ToArray());
            ms.Dispose();
            ms.Close();
            EventLogs.WriteOperationLog(Privilege.ReconciOrdersDetailsExcel, string.Format(CultureInfo.InvariantCulture, "用户{0}生成供应商商品明细成功", new object[]
            {
                this.User.Identity.Name
            }));

            this.Page.Response.End();

        }
        #region 导出
       // protected bool Export(DataTable dt)
       //{

       //    //try
       //    //{
       //        if (dt != null)
       //        {
       //            #region 操作excel
       //            Microsoft.Office.Interop.Excel.Workbook xlWorkBook=  new Microsoft.Office.Interop.Excel.Application().Workbooks.Add(Missing.Value);
       //            xlWorkBook.Application.Visible = false;
       //            Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet = xlWorkBook.Sheets[1] as Microsoft.Office.Interop.Excel.Worksheet;
            
       //            //设置标题
       //            xlWorkSheet.Cells[1, 1] = "商品名称";
       //            xlWorkSheet.Cells[1, 2] = "商品编码";
       //            xlWorkSheet.Cells[1, 3] = "库存";
       //            xlWorkSheet.Cells[1, 4] = "成本";
       //            xlWorkSheet.Cells[1, 5] = "商品价格";
       //            xlWorkSheet.Cells[1, 6] = "商品状态";
       //            xlWorkSheet.Cells[1, 7] = "审核状态";
       //            xlWorkSheet.Cells[1, 8] = "审核价状态";
       //            xlWorkSheet.Cells[1, 9] = "备案状态";
       //            xlWorkSheet.Cells[1, 10] = "原产地";
       //            xlWorkSheet.Cells[1, 11] = "供应商名称";
          
       //            //设置宽度            
       //           (xlWorkSheet.Cells[1, 2] as Microsoft.Office.Interop.Excel.Range).ColumnWidth = 15;
       //            //设置字体
       //            xlWorkSheet.Cells.Font.Size = 12;
                  
       //            for (int i = 0; i < dt.Rows.Count; i++)
       //            {
       //                #region 为excel赋值
       //                //为单元格赋值。
       //                xlWorkSheet.Cells[i + 2]  = dt.Rows[i][0].ToString();
       //                xlWorkSheet.Cells[i + 2, 2]  = dt.Rows[i][1].ToString();
       //                xlWorkSheet.Cells[i + 2, 3]  = dt.Rows[i][2].ToString();
       //                xlWorkSheet.Cells[i + 2, 4]  = dt.Rows[i][3].ToString();
       //                xlWorkSheet.Cells[i + 2, 5]  = dt.Rows[i][4].ToString();
       //                xlWorkSheet.Cells[i + 2, 6]  = dt.Rows[i][5].ToString();
       //                xlWorkSheet.Cells[i + 2, 7]  = dt.Rows[i][6].ToString();
       //                xlWorkSheet.Cells[i + 2, 8]  = dt.Rows[i][7].ToString();
       //                xlWorkSheet.Cells[i + 2, 9]  = dt.Rows[i][8].ToString();
       //                xlWorkSheet.Cells[i + 2, 10] = dt.Rows[i][9].ToString();
       //                xlWorkSheet.Cells[i + 2, 11] = dt.Rows[i][10].ToString();
       //                #endregion

       //                string filename = "E:\\ZHOUMing\\haimylife\\trunk\\Code\\EcShop.Website\\Storage\\master\\product\\images\\2b5ab98c05554e57838576225208f6ec.png";
                       
       //                int rangeindex = i + 2;
       //                string rangename = "B" + rangeindex;

       //                Microsoft.Office.Interop.Excel.Range range = xlWorkSheet.get_Range(rangename, Type.Missing);
       //                range.Select();
                    
       //                float PicLeft, PicTop, PicWidth, PicHeight;　　　　//距离左边距离，顶部距离，图片宽度、高度
       //                PicTop     = 10;// Convert.ToSingle(range.Top);
       //                PicHeight  = 10;// Convert.ToSingle(range.Height);
       //                PicWidth   = 10;// Convert.ToSingle(range.Width);
       //                PicLeft    = 10;// Convert.ToSingle(range.Left);
                     
       //                Microsoft.Office.Interop.Excel.Pictures pict = xlWorkSheet.Pictures(Type.Missing) as Microsoft.Office.Interop.Excel.Pictures;
       //                if (filename.IndexOf(".") > 0)
       //                {
       //                    if (System.IO.File.Exists(filename))
       //                    {
       //                        //pict.Insert(filename, Type.Missing);//显示原图   重叠在一起
       //                        xlWorkSheet.Shapes.AddPicture(filename, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, PicLeft, PicTop, PicWidth, PicHeight);//指定位置显示小图
       //                    }
       //                }
       //            }
       //            #endregion

       //            #region 保存excel文件
       //            string filePath = Server.MapPath("ReadExcel") + "" + System.DateTime.Now.ToString().Replace(":", "") + "导出.xls";
       //            xlWorkBook.SaveAs(filePath, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
       //            xlWorkBook.Application.Quit();
       //            xlWorkSheet = null;
       //            xlWorkBook = null;
       //            GC.Collect();
       //            System.GC.WaitForPendingFinalizers();
       //            #endregion
              
       //            #region 导出到客户端
       //            Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
       //            Response.AppendHeader("content-disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode("导出", System.Text.Encoding.UTF8) + ".xls");
       //            Response.ContentType = "Application/excel";
       //            Response.WriteFile(filePath);
       //            Response.End();
       //            #endregion
       //            KillProcessexcel("EXCEL");
       //        }
       //        return true;
       //    //}
       //    //catch (Exception ee)
       //    //{
       //    //}
       //    return false;

       // }
        #endregion
        #region 杀死进程
        //private void KillProcessexcel(string processName)
        //{ //获得进程对象，以用来操作
        //    System.Diagnostics.Process myproc = new System.Diagnostics.Process();
        //    //得到所有打开的进程
        //    try
        //    {
        //        //获得需要杀死的进程名
        //        foreach (Process thisproc in Process.GetProcessesByName(processName))
        //        { //立即杀死进程
        //            thisproc.Kill();
        //        }
        //    }
        //    catch (Exception Exc)
        //    {
        //        throw new Exception("", Exc);
        //    }
        //}
        #endregion


        
        private void btnUpdateProductDeducts_Click(object sender, System.EventArgs e)
        {
            string text = base.Request.Form["CheckBoxGroup"];
            if (string.IsNullOrEmpty(text))
            {
                this.ShowMsg("请先选择要更新推广分佣的商品", false);
                return;
            }
            decimal referralDeduct = 0m;
            decimal subMemberDeduct = 0m;
            decimal subReferralDeduct = 0m;
            if (string.IsNullOrEmpty(this.txtReferralDeduct.Text.Trim()))
            {
                this.ShowMsg("请输入直接推广佣金！", false);
                return;
            }
            if (!decimal.TryParse(this.txtReferralDeduct.Text.Trim(), out referralDeduct))
            {
                this.ShowMsg("您输入的直接推广佣金格式不对！", false);
                return;
            }
            if (string.IsNullOrEmpty(this.txtSubMemberDeduct.Text.Trim()))
            {
                this.ShowMsg("请输入下级会员佣金！", false);
                return;
            }
            if (!decimal.TryParse(this.txtSubMemberDeduct.Text, out subMemberDeduct))
            {
                this.ShowMsg("您输入的下级会员佣金格式不正确！", false);
                return;
            }
            if (string.IsNullOrEmpty(this.txtSubReferralDeduct.Text.Trim()))
            {
                this.ShowMsg("请输入下级推广员佣金！", false);
                return;
            }
            if (!decimal.TryParse(this.txtSubReferralDeduct.Text, out subReferralDeduct))
            {
                this.ShowMsg("您输入的下级推广员佣金格式不正确！", false);
                return;
            }
            if (ProductHelper.UpdateProductReferralDeduct(text, referralDeduct, subMemberDeduct, subReferralDeduct))
            {
                this.ShowMsg("成功更新了商品的推广分佣！", true);
                this.BindProducts();
                return;
            }
            this.ShowMsg("更新商品的推广分佣失败！", false);
        }
        private void btnSetFreeShip_Click(object sender, System.EventArgs e)
        {
            bool flag = ((System.Web.UI.WebControls.Button)sender).ID == "btnSetFreeShip";
            string text = base.Request.Form["CheckBoxGroup"];
            if (string.IsNullOrEmpty(text))
            {
                this.ShowMsg("请先选择要设置为包邮的商品", false);
                return;
            }
            int num = ProductHelper.SetFreeShip(text, flag);
            if (num > 0)
            {
                this.ShowMsg("成功" + (flag ? "设置" : "取消") + "了商品包邮状态", true);
                this.BindProducts();
                return;
            }
            this.ShowMsg((flag ? "设置" : "取消") + "商品包邮状态失败，未知错误", false);
        }
        private void dropSaleStatus_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            this.ReloadProductOnSales(true);
        }
        private void btnSearch_Click(object sender, System.EventArgs e)
        {
            this.ReloadProductOnSales(true);
        }
        private void grdProducts_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
            {
                System.Web.UI.WebControls.Literal literal = (System.Web.UI.WebControls.Literal)e.Row.FindControl("litSaleStatus");
                System.Web.UI.WebControls.Literal literal2 = (System.Web.UI.WebControls.Literal)e.Row.FindControl("litMarketPrice");
                if (literal.Text == "1")
                {
                    literal.Text = "出售中";
                }
                else
                {
                    if (literal.Text == "2")
                    {
                        literal.Text = "下架区";
                    }
                    else
                    {
                        literal.Text = "仓库中";
                    }
                }
                if (string.IsNullOrEmpty(literal2.Text))
                {
                    literal2.Text = "-";
                }
            }
        }
        private void grdProducts_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            ManagerHelper.CheckPrivilege(Privilege.SupplierProductDelete);
            System.Collections.Generic.List<int> list = new System.Collections.Generic.List<int>();
            string text = this.grdProducts.DataKeys[e.RowIndex].Value.ToString();
            if (text != "")
            {
                list.Add(System.Convert.ToInt32(text));
            }
            if (ProductHelper.RemoveProduct(text) > 0)
            {
                this.ShowMsg("删除商品成功", true);
                this.ReloadProductOnSales(false);
            }
        }
        private void btnUpSale_Click(object sender, System.EventArgs e)
        {
            ManagerHelper.CheckPrivilege(Privilege.SupplierProductUp);
            string text = base.Request.Form["CheckBoxGroup"];
            if (string.IsNullOrEmpty(text))
            {
                this.ShowMsg("请先选择要上架的商品", false);
                return;
            }

            //商品是否未归类
            if (ProductHelper.IsExitNoClassifyProduct(text))
            {
                this.ShowMsg("商品还未完成归类操作", false);
                return;
            }

            int num = ProductHelper.UpShelf(text);
            if (num > 0)
            {
                this.ShowMsg("成功上架了选择的商品，您可以在出售中的商品里面找到上架以后的商品", true);
                this.BindProducts();
                return;
            }
            this.ShowMsg("上架商品失败，未知错误", false);
        }
        private void btnUnSale_Click(object sender, System.EventArgs e)
        {
            ManagerHelper.CheckPrivilege(Privilege.SupplierProductDown);
            string text = base.Request.Form["CheckBoxGroup"];
            if (string.IsNullOrEmpty(text))
            {
                this.ShowMsg("请先选择要下架的商品", false);
                return;
            }
            int num = ProductHelper.OffShelf(text);
            if (num > 0)
            {
                this.ShowMsg("成功下架了选择的商品，您可以在下架区的商品里面找到下架以后的商品", true);
                this.BindProducts();
                return;
            }
            this.ShowMsg("下架商品失败，未知错误", false);
        }
        private void btnInStock_Click(object sender, System.EventArgs e)
        {
            ManagerHelper.CheckPrivilege(Privilege.SupplierProductInStock);
            string text = base.Request.Form["CheckBoxGroup"];
            if (string.IsNullOrEmpty(text))
            {
                this.ShowMsg("请先选择要入库的商品", false);
                return;
            }
            int num = ProductHelper.InStock(text);
            if (num > 0)
            {
                this.ShowMsg("成功入库选择的商品，您可以在仓库区的商品里面找到入库以后的商品", true);
                this.BindProducts();
                return;
            }
            this.ShowMsg("入库商品失败，未知错误", false);
        }
        private void btnDelete_Click(object sender, System.EventArgs e)
        {
            ManagerHelper.CheckPrivilege(Privilege.SupplierProductDelete);
            string text = base.Request.Form["CheckBoxGroup"];
            if (string.IsNullOrEmpty(text))
            {
                this.ShowMsg("请先选择要删除的商品", false);
                return;
            }
            int num = ProductHelper.RemoveProduct(text);
            if (num > 0)
            {
                this.ShowMsg("成功删除了选择的商品", true);
                this.BindProducts();
                return;
            }
            this.ShowMsg("删除商品失败，未知错误", false);
        }

        /// <summary>
        /// 重新提交审价
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReSubmitPriceApprove_Click(object sender, System.EventArgs e)
        {
            string text = base.Request.Form["CheckBoxGroup"];
            if (string.IsNullOrEmpty(text))
            {
                this.ShowMsg("请先选择要重新审价的商品", false);
                return;
            }
            int num = ProductHelper.ReSubmitPriceApprove(text);
            if (num > 0)
            {
                this.ShowMsg("成功提交审价申请", true);
                this.BindProducts();
                return;
            }
            this.ShowMsg("提交失败，请检查商品是否处于审价不通过状态", false);
        }
        /// <summary>
        /// 生成打印任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGenerationQCode_Click(object sender, System.EventArgs e)
        {
            string text = base.Request.Form["CheckBoxGroup"];
            if (string.IsNullOrEmpty(text))
            {
                this.ShowMsg("请先选择要生成二维码的商品", false);
                return;
            }
            string[] ArrProductId = text.Split(',');
            string QRcode = string.Empty;
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (string proid in ArrProductId)
            {
                QRcode = CreateQrcode("ProductId=" + proid, proid);
                dic.Add(proid, QRcode);
            }
            bool num = ProductHelper.UpdateQRcode(dic);
            if (num)
            {
                this.ShowMsg("成功生成二维码", true);
                this.ReloadProductOnSales(false);
            }
            else
            {
                this.ShowMsg("生成失败，未知错误", false);
            }
        }


        private void btnUpdateProductTags_Click(object sender, System.EventArgs e)
        {
            string text = base.Request.Form["CheckBoxGroup"];
            if (string.IsNullOrEmpty(text))
            {
                this.ShowMsg("请先选择要关联的商品", false);
                return;
            }
            System.Collections.Generic.IList<int> list = new System.Collections.Generic.List<int>();
            if (!string.IsNullOrEmpty(this.txtProductTag.Text.Trim()))
            {
                string text2 = this.txtProductTag.Text.Trim();
                string[] array;
                if (text2.Contains(","))
                {
                    array = text2.Split(new char[]
					{
						','
					});
                }
                else
                {
                    array = new string[]
					{
						text2
					};
                }
                string[] array2 = array;
                for (int i = 0; i < array2.Length; i++)
                {
                    string value = array2[i];
                    list.Add(System.Convert.ToInt32(value));
                }
            }
            string[] array3;
            if (text.Contains(","))
            {
                array3 = text.Split(new char[]
				{
					','
				});
            }
            else
            {
                array3 = new string[]
				{
					text
				};
            }
            int num = 0;
            string[] array4 = array3;
            for (int j = 0; j < array4.Length; j++)
            {
                string value2 = array4[j];
                ProductHelper.DeleteProductTags(System.Convert.ToInt32(value2), null);
                if (list.Count > 0 && ProductHelper.AddProductTags(System.Convert.ToInt32(value2), list, null))
                {
                    num++;
                }
            }
            if (num > 0)
            {
                EventLogs.WriteOperationLog(Privilege.SubjectProducts, string.Format(CultureInfo.InvariantCulture, "已成功修改了{0}件商品的商品标签", new object[]
			    {
				     num,
				}));
                this.ShowMsg(string.Format("已成功修改了{0}件商品的商品标签", num), true);
            }
            else
            {
                this.ShowMsg("已成功取消了商品的关联商品标签", true);
            }
            this.txtProductTag.Text = "";
        }
        private void BindProducts()
        {
            this.LoadParameters();
            ProductQuery productQuery = new ProductQuery
            {
                Keywords = this.productName,
                ProductCode = this.productCode,
                BarCode=this.BarCode,
                CategoryId = this.categoryId,
                PageSize = this.pager.PageSize,
                PageIndex = this.pager.PageIndex,
                SortOrder = SortAction.Desc,
                SortBy = "DisplaySequence",
                StartDate = this.startDate,
                BrandId = this.dropBrandList.SelectedValue.HasValue ? this.dropBrandList.SelectedValue : null,
                TagId = this.dropTagList.SelectedValue.HasValue ? this.dropTagList.SelectedValue : null,
                TypeId = this.typeId,
                SaleStatus = this.saleStatus,
                EndDate = this.endDate,
                ImportSourceId = this.ddlImportSourceType.SelectedValue.HasValue ? this.ddlImportSourceType.SelectedValue : null,
                IsApproved = this.IsApproved,
                IsApprovedPrice = this.IsApprovedPrice,
                IsAllClassify = this.IsAllClassify
            };
            if (this.ddlSupplier.SelectedValue.HasValue)
            {
                productQuery.SupplierId = this.ddlSupplier.SelectedValue;
            }
            if (this.categoryId.HasValue)
            {
                productQuery.MaiCategoryPath = CatalogHelper.GetCategory(this.categoryId.Value).Path;
            }

            if (productQuery.IsApproved.HasValue)
            {
                this.dropIsApproved.SelectedValue = productQuery.IsApproved.Value.ToString();
            }

            if (productQuery.IsApprovedPrice.HasValue)
            {
                this.dropIsApprovedPrice.SelectedValue = productQuery.IsApprovedPrice.Value.ToString();
            }

            if (productQuery.IsAllClassify.HasValue)
            {
                this.dropIsAllClassify.SelectedValue = productQuery.IsAllClassify.Value.ToString();
            }


            Globals.EntityCoding(productQuery, true);
            DbQueryResult products = ProductHelper.GetProducts(productQuery);
            this.grdProducts.DataSource = products.Data;
            this.grdProducts.DataBind();
            this.txtSearchText.Text = productQuery.Keywords;
            this.txtSKU.Text = productQuery.ProductCode;
            this.txt_BarCode.Text = productQuery.BarCode;
            this.dropCategories.SelectedValue = productQuery.CategoryId;
            this.dropType.SelectedValue = productQuery.TypeId;
            this.ddlImportSourceType.SelectedValue = productQuery.ImportSourceId;
            this.ddlSupplier.SelectedValue = productQuery.SupplierId;
            this.pager1.TotalRecords = (this.pager.TotalRecords = products.TotalRecords);
        }
        private void ReloadProductOnSales(bool isSearch)
        {
            System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
            nameValueCollection.Add("productName", Globals.UrlEncode(this.txtSearchText.Text.Trim()));
            if (this.dropCategories.SelectedValue.HasValue)
            {
                nameValueCollection.Add("categoryId", this.dropCategories.SelectedValue.ToString());
            }
            nameValueCollection.Add("productCode", Globals.UrlEncode(Globals.HtmlEncode(this.txtSKU.Text.Trim())));
            nameValueCollection.Add("BarCode", Globals.UrlEncode(Globals.HtmlEncode(this.txt_BarCode.Text.Trim())));
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
            if (this.dropBrandList.SelectedValue.HasValue)
            {
                nameValueCollection.Add("brandId", this.dropBrandList.SelectedValue.ToString());
            }
            if (this.dropTagList.SelectedValue.HasValue)
            {
                nameValueCollection.Add("tagId", this.dropTagList.SelectedValue.ToString());
            }
            if (this.dropType.SelectedValue.HasValue)
            {
                nameValueCollection.Add("typeId", this.dropType.SelectedValue.ToString());
            }
            if (this.ddlImportSourceType.SelectedValue.HasValue)
            {
                nameValueCollection.Add("importSourceId", this.ddlImportSourceType.SelectedValue.ToString());
            }
            if (this.ddlSupplier.SelectedValue.HasValue)
            {
                nameValueCollection.Add("supplierId", this.ddlSupplier.SelectedValue.ToString());
            }

            if (this.dropIsApproved.SelectedValue != "-1")
            {
                nameValueCollection.Add("IsApproved", this.dropIsApproved.SelectedValue);
            }

            if (this.dropIsApprovedPrice.SelectedValue != "-1")
            {
                nameValueCollection.Add("IsApprovedPrice", this.dropIsApprovedPrice.SelectedValue);
            }


            if (this.dropIsAllClassify.SelectedValue != "-1")
            {
                nameValueCollection.Add("IsAllClassify", this.dropIsAllClassify.SelectedValue);
            }


            nameValueCollection.Add("SaleStatus", this.dropSaleStatus.SelectedValue.ToString());
            base.ReloadPage(nameValueCollection);
        }
        private void LoadParameters()
        {
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productName"]))
            {
                this.productName = Globals.UrlDecode(this.Page.Request.QueryString["productName"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productCode"]))
            {
                this.productCode = Globals.UrlDecode(this.Page.Request.QueryString["productCode"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["BarCode"]))
            {
                this.BarCode = Globals.UrlDecode(this.Page.Request.QueryString["BarCode"]);
            }
            int value = 0;
            if (int.TryParse(this.Page.Request.QueryString["categoryId"], out value))
            {
                this.categoryId = new int?(value);
            }
            int value2 = 0;
            if (int.TryParse(this.Page.Request.QueryString["brandId"], out value2))
            {
                this.dropBrandList.SelectedValue = new int?(value2);
            }
            int value3 = 0;
            if (int.TryParse(this.Page.Request.QueryString["tagId"], out value3))
            {
                this.tagId = new int?(value3);
            }
            int value4 = 0;
            if (int.TryParse(this.Page.Request.QueryString["typeId"], out value4))
            {
                this.typeId = new int?(value4);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["SaleStatus"]))
            {
                this.saleStatus = (ProductSaleStatus)System.Enum.Parse(typeof(ProductSaleStatus), this.Page.Request.QueryString["SaleStatus"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["startDate"]))
            {
                this.startDate = new System.DateTime?(System.DateTime.Parse(this.Page.Request.QueryString["startDate"]));
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["endDate"]))
            {
                this.endDate = new System.DateTime?(System.DateTime.Parse(this.Page.Request.QueryString["endDate"]));
            }
            int iSourceId = 0;
            if (int.TryParse(this.Page.Request.QueryString["importSourceId"], out iSourceId))
            {
                this.importSourceId = new int?(iSourceId);
            }

            int iSupplier = 0;
            if (int.TryParse(this.Page.Request.QueryString["supplierId"], out iSupplier))
            {
                this.supplierId = new int?(iSupplier);
            }


            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["IsApproved"]))//是否审核过滤
            {
                int tmpIsApproved = 0;
                if (int.TryParse(this.Page.Request.QueryString["IsApproved"], out tmpIsApproved))
                {
                    this.IsApproved = new Int32?(tmpIsApproved);
                }
            }

            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["IsApprovedPrice"]))//是否审价过滤
            {
                int tmpIsApprovedPrice = 0;
                if (int.TryParse(this.Page.Request.QueryString["IsApprovedPrice"], out tmpIsApprovedPrice))
                {
                    this.IsApprovedPrice = new Int32?(tmpIsApprovedPrice);
                }
            }


            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["IsAllClassify"]))//是否审价过滤
            {
                int tmpIsAllClassify = 0;
                if (int.TryParse(this.Page.Request.QueryString["IsAllClassify"], out tmpIsAllClassify))
                {
                    this.IsAllClassify = new Int32?(tmpIsAllClassify);
                }
            }

            this.txtSearchText.Text = this.productName;
            this.txtSKU.Text = this.productCode;
            this.txt_BarCode.Text = this.BarCode;
            this.dropCategories.DataBind();
            this.dropCategories.SelectedValue = this.categoryId;

            this.ddlImportSourceType.DataBind();
            this.ddlImportSourceType.SelectedValue = this.importSourceId;

            this.ddlSupplier.DataBind();
            this.ddlSupplier.SelectedValue = this.supplierId;

            this.dropTagList.DataBind();
            this.dropTagList.SelectedValue = this.tagId;
            this.calendarStartDate.SelectedDate = this.startDate;
            this.calendarEndDate.SelectedDate = this.endDate;
            this.dropType.SelectedValue = this.typeId;
            this.dropSaleStatus.SelectedValue = this.saleStatus;
        }

        public string ProductImg(string isprove, string productid, string imgpath)
        {
            string resultstr = string.Empty;
            if (isprove == "True")
            {
                resultstr = "<a href='../../ProductDetails.aspx?productId=" + productid + "' target=\"_blank\"><img src='" + imgpath + "' style=\"border-width:0px;\"></a>";
            }
            else
            {
                resultstr = "<img src='" + imgpath + "' style=\"border-width:0px;\">";
            }
            return resultstr;
        }

        public string ProductDetails(string isprove, string productid, string productname)
        {
            string resultstr = string.Empty;
            if (isprove == "True")
            {
                resultstr = "<a href='../../ProductDetails.aspx?productId=" + productid + "' target=\"_blank\">" + productname + "</a>";
            }
            else
            {
                resultstr = "" + productname + "";
            }
            return resultstr;
        }
    }
}
