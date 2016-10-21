using ASPNET.WebControls;
using EcShop.ControlPanel.Commodities;
using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Entities.PO;
using EcShop.Entities.Store;
using EcShop.Membership.Context;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace EcShop.UI.Web.Admin.PO
{
    [PrivilegeCheck(Privilege.POAdd)]
    public class POItemAdd : AdminPage
    {
        protected System.Web.UI.WebControls.TextBox txtBoxBarCode;
        protected System.Web.UI.WebControls.TextBox txtExpectQuantity;
        //protected System.Web.UI.WebControls.TextBox txtPracticalQuantity;
        protected System.Web.UI.WebControls.DropDownList ddlIsSample;
        protected WebCalendar calendarManufactureDate;
        protected WebCalendar calendarEffectiveDate;
        protected System.Web.UI.WebControls.TextBox txtBatchNumber;
        protected System.Web.UI.WebControls.TextBox txtRoughWeight;
        protected System.Web.UI.WebControls.DropDownList ddlCurrency;
        protected System.Web.UI.WebControls.TextBox txtRate;
        protected System.Web.UI.WebControls.TextBox txtCostPrice;
        protected System.Web.UI.WebControls.TextBox txtSalePrice;
        protected System.Web.UI.WebControls.TextBox txtCartonSize;

        protected System.Web.UI.WebControls.TextBox txtCartonMeasure;
        protected System.Web.UI.WebControls.TextBox txtCases;

        protected System.Web.UI.WebControls.TextBox txtNetWeight;
        protected System.Web.UI.WebControls.TextBox txtOriginalCurrencyPrice;
        protected System.Web.UI.WebControls.Button btnSave;
        protected System.Web.UI.WebControls.HiddenField hidRate;
        //OriginalCurrencyPrice
	    //OriginalCurrencyTotalPrice

        /// <summary>
        /// POId
        /// </summary>
        public int POId;

        public int SupplierId;
        protected void Page_Load(object sender, System.EventArgs e)
        {
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            int tempid = 0;
            if (!int.TryParse(this.Page.Request.QueryString["POId"], out tempid))
            {
                base.GotoResourceNotFound();
                return;
            }
            int tempSupplierId = 0;
            int.TryParse(this.Page.Request.QueryString["SupplierId"], out tempSupplierId);

            this.POId = tempid;
            this.SupplierId = tempSupplierId;

            if (!this.Page.IsPostBack)
            {
                DataBindCurrency();


                //如果有Id代表是编辑
                if (Request["Id"] != null && Request["Id"].ToString() != "")
                {
                    int tmpId = 0;
                    if (int.TryParse(this.Page.Request["Id"], out tmpId))
                    {
                        DataBind(this.Page.Request["Id"]);
                    }
                    else
                    {
                        this.ShowMsg("未知参数", false);
                    }
                }
            }
        }

        /// <summary>
        /// 绑定币别
        /// </summary>
        private void DataBindCurrency()
        {
            DataSet dsBaseCurrency = PurchaseOrderHelper.GetBaseCurrency();
            ddlCurrency.DataTextField = "Name_CN";
            ddlCurrency.DataValueField = "ID";
            ddlCurrency.DataSource = dsBaseCurrency;
            ddlCurrency.DataBind();
        }

        #region 编辑时绑定数据
        /// <summary>
        /// 编辑时绑定数据
        /// </summary>
        private void DataBind(string Id)
        {
            DataSet dsPurchaseOrderItem = PurchaseOrderHelper.GetPurchaseOrderItem("Id=" + Id);
            if (dsPurchaseOrderItem != null && dsPurchaseOrderItem.Tables.Count > 0 && dsPurchaseOrderItem.Tables[0].Rows.Count > 0)
            {
                DataRow dr = dsPurchaseOrderItem.Tables[0].Rows[0];
                txtBoxBarCode.Text = dr["BoxBarCode"].ToString();
                txtExpectQuantity.Text = dr["ExpectQuantity"].ToString();
                //txtPracticalQuantity.Text = dr["PracticalQuantity"].ToString();
                ddlIsSample.SelectedValue = dr["IsSample"].ToString() == "True" ? "1" : "0";
                if (dr["ManufactureDate"] != null && dr["ManufactureDate"].ToString() != "")
                {
                    calendarManufactureDate.Text = Convert.ToDateTime(dr["ManufactureDate"]).ToString("yyyy-MM-dd");
                }
                if (dr["EffectiveDate"] != null && dr["EffectiveDate"].ToString() != "")
                {
                    calendarEffectiveDate.Text = Convert.ToDateTime(dr["EffectiveDate"]).ToString("yyyy-MM-dd"); ;
                }
                txtBatchNumber.Text = dr["BatchNumber"].ToString();
                txtRoughWeight.Text = dr["RoughWeight"].ToString();
                txtNetWeight.Text = dr["NetWeight"].ToString();
                txtOriginalCurrencyPrice.Text = dr["OriginalCurrencyPrice"].ToString() != "" ? decimal.Parse(dr["OriginalCurrencyPrice"].ToString()).ToString("F2") : "";

                ddlCurrency.SelectedValue = dr["CurrencyId"].ToString();
                txtRate.Text = dr["Rate"].ToString();
                hidRate.Value = dr["Rate"].ToString();

                txtCostPrice.Text = dr["CostPrice"].ToString() != "" ? decimal.Parse(dr["CostPrice"].ToString()).ToString("F2") : "";
                txtSalePrice.Text = dr["SalePrice"].ToString() != "" ? decimal.Parse(dr["SalePrice"].ToString()).ToString("F2") : "";
                txtCartonSize.Text = dr["CartonSize"].ToString();
                txtCartonMeasure.Text = dr["CartonMeasure"].ToString();
                txtCases.Text = dr["Cases"].ToString();
            }
        }
        #endregion

        private void btnSave_Click(object sender, System.EventArgs e)
        {
            ManagerHelper.CheckPrivilege(Privilege.POAdd);
            var member = HiContext.Current.User;
            if (member == null || member.IsLockedOut)
            {
                this.ShowMsg("获取用户信息失败，请重新登录", false);
                return;
            }

            if (Request["Id"] != null && Request["Id"].ToString() != "")
            {
                PurchaseOrderItemInfo purchaseOrderItemInfo = new PurchaseOrderItemInfo
                {
                    BoxBarCode = txtBoxBarCode.Text,
                    ExpectQuantity = txtExpectQuantity.Text.Length > 0 ? int.Parse(txtExpectQuantity.Text) : 0,
                    //PracticalQuantity = txtPracticalQuantity.Text.Length > 0 ? int.Parse(txtPracticalQuantity.Text) : 0,
                    IsSample = ddlIsSample.SelectedValue == "0" ? false : true,
                    ManufactureDate = calendarManufactureDate.SelectedDate,
                    EffectiveDate = calendarEffectiveDate.SelectedDate,
                    BatchNumber = txtBatchNumber.Text,
                    RoughWeight = txtRoughWeight.Text.Length > 0 ? decimal.Parse(txtRoughWeight.Text) : 0,
                    NetWeight = txtNetWeight.Text.Length > 0 ? decimal.Parse(txtNetWeight.Text) : 0,
                    OriginalCurrencyPrice = txtOriginalCurrencyPrice.Text.Length > 0 ? decimal.Parse(txtOriginalCurrencyPrice.Text) : 0,
                    CurrencyId = int.Parse(ddlCurrency.SelectedValue),
                    Rate = hidRate.Value.Length > 0 ? decimal.Parse(hidRate.Value) : 0,//txtRate.Text.Length > 0 ? decimal.Parse(txtRate.Text) : 0,
                    CostPrice = txtCostPrice.Text.Length > 0 ? decimal.Parse(txtCostPrice.Text) : 0,
                    SalePrice = txtSalePrice.Text.Length > 0 ? decimal.Parse(txtSalePrice.Text) : 0,
                    CartonSize = txtCartonSize.Text,
                    Cases = txtCases.Text.Length > 0 ? int.Parse(txtCases.Text) : 0,
                    CartonMeasure = txtCartonMeasure.Text,
                    UpdateUserId = member.UserId
                };
                int tmpId = 0;
                if (int.TryParse(this.Page.Request["Id"], out tmpId))
                {
                    purchaseOrderItemInfo.id = tmpId;
                    purchaseOrderItemInfo.TotalCostPrice = purchaseOrderItemInfo.CostPrice * purchaseOrderItemInfo.ExpectQuantity;
                    purchaseOrderItemInfo.TotalSalePrice = purchaseOrderItemInfo.SalePrice * purchaseOrderItemInfo.ExpectQuantity;
                    purchaseOrderItemInfo.OriginalCurrencyTotalPrice = purchaseOrderItemInfo.OriginalCurrencyPrice * purchaseOrderItemInfo.ExpectQuantity;
                    if (PurchaseOrderHelper.EditPurchaseOrderItemInfo(purchaseOrderItemInfo))
                    {
                        base.Response.Redirect(Globals.GetAdminAbsolutePath("/POManage/POItemList.aspx?Id=" + POId + "&SupplierId=" + SupplierId), true);
                        return;
                    }
                    else
                    {
                        this.ShowMsg("该单状态不允许操作或您无权操作该单", false);
                    }
                }
                else
                {
                    this.ShowMsg("未知参数", false);
                }
            }
            else
            {
                this.ShowMsg("未知参数", false);
            }
        }
    }
}
