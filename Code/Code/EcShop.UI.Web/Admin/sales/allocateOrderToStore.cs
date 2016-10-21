using ASPNET.WebControls;
using EcShop.ControlPanel.Commodities;
using EcShop.ControlPanel.Sales;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcShop.UI.Web.Admin.sales
{
    //[PrivilegeCheck(Privilege.Orders)]
    public class AllocateOrderToStore : AdminPage
    {
        protected System.Web.UI.WebControls.Button btnDo;
        protected System.Web.UI.WebControls.DropDownList dropStores;
        protected void Page_Load(object sender, System.EventArgs e)

        {
            if (!this.Page.IsPostBack)
            {
                this.dropStores.DataBind();
                this.dropStores.DataSource = StoreManagementHelper.GetStore();
                this.dropStores.DataTextField = "StoreName";
                this.dropStores.DataValueField = "StoreId";
                this.dropStores.DataBind();
                this.dropStores.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", ""));
            }
            this.btnDo.Click += new System.EventHandler(this.btnDo_Click);
        }

        protected void btnDo_Click(object sender, System.EventArgs e)
        {
           int storeId=0;
           int.TryParse(this.dropStores.SelectedValue, out storeId);
           string orderIds = Request.QueryString["OrderIds"];
           if (string.IsNullOrEmpty(orderIds))
           {
               ShowMsg("请选择要分配的订单", false);
               return;
           }
           string orderIds2 = "'" + orderIds.Replace(",", "','") + "'";
           if (OrderHelper.AllocateOrderToStore(orderIds2, storeId))
           {
               ShowMsg("成功分配订单到门店", true);
           }
           else
           {
               ShowMsg("分配订单到门店失败", false);
           }
        }
    }
}
