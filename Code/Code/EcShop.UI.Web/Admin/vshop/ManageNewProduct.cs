using ASPNET.WebControls;
using EcShop.ControlPanel.Store;
using EcShop.Entities;
using EcShop.Entities.Store;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin.vshop
{
     [PrivilegeCheck(Privilege.vBannerManage)]
    public class ManageNewProduct : AdminPage
    {
        protected Grid grdRecommend;
        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.BindData();
            }
        }
        private void BindData()
        {
            this.grdRecommend.DataSource = VShopHelper.GetAllRecommend(ClientType.VShop);
            this.grdRecommend.DataBind();
        }
        protected void grdRecommend_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            int rowIndex = ((System.Web.UI.WebControls.GridViewRow)((System.Web.UI.Control)e.CommandSource).NamingContainer).RowIndex;
            int num = (int)this.grdRecommend.DataKeys[rowIndex].Value;
            int num2 = 0;
            if (e.CommandName == "Fall")
            {
                if (rowIndex < this.grdRecommend.Rows.Count - 1)
                {
                    num2 = (int)this.grdRecommend.DataKeys[rowIndex + 1].Value;
                }
            }
            else
            {
                if (e.CommandName == "Rise" && rowIndex > 0)
                {
                    num2 = (int)this.grdRecommend.DataKeys[rowIndex - 1].Value;
                }
            }
            if (num2 > 0)
            {
                VShopHelper.SwapTplCfgSequence(num, num2);
                base.ReloadPage(null);
            }
            if (e.CommandName == "Delete")
            {
                if (VShopHelper.DelTplCfg(num))
                {
                    this.ShowMsg("删除成功！", true);
                    base.ReloadPage(null);
                    return;
                }
                this.ShowMsg("删除失败！", false);
            }
        }

        protected string CovertType(string type)
        {
            string retType;
            switch (type)
            {
                case "Activity":
                    retType = "活动";
                    break;
                case "Home":
                    retType = "首页";
                    break;
                case "Category":
                    retType = "商品分类";
                    break;
                case "ShoppingCart":
                    retType = "购物车";
                    break;
                case "OrderCenter":
                    retType = "订单中心";
                    break;
                case "VipCard":
                    retType = "会员卡";
                    break;
                case "Link":
                    retType = "链接";
                    break;
                case "Phone":
                    retType = "电话";
                    break;
                case "Address":
                    retType = "地址";
                    break;
                case "GroupBuy":
                    retType = "团购";
                    break;
                case "Brand":
                    retType = "品牌";
                    break;
                case "Article":
                    retType = "文章";
                    break;
                case "ImportSourceType":
                    retType = "原产地";
                    break;
                case "Product":
                    retType = "商品";
                    break;
                default:
                    retType = type;
                    break;


            }
            return retType;
        }
    }
}
