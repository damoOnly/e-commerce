using ASPNET.WebControls;
using EcShop.Core.Entities;
using EcShop.Core.Enums;
using EcShop.Entities.Promotions;
using EcShop.Membership.Context;
using EcShop.SaleSystem.Catalog;
using EcShop.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class BundlingProducts : HtmlTemplatedWebControl
	{
		private ThemedTemplatedRepeater rptProduct;
		private Pager pager;
        private int? SupplierId;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-BundlingProducts.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
            int value = 0;
            if (int.TryParse(this.Page.Request.QueryString["supplierid"], out value))
            {
                this.SupplierId = new int?(value);
            }
			this.rptProduct = (ThemedTemplatedRepeater)this.FindControl("rptProduct");
			this.pager = (Pager)this.FindControl("pager");
			this.rptProduct.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(this.rptProduct_ItemDataBound);
			if (!this.Page.IsPostBack)
			{
				this.BindBundlingProducts();
			}
		}
		protected void rptProduct_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == System.Web.UI.WebControls.ListItemType.AlternatingItem || e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Item)
			{
				DataRowView dataRowView = (DataRowView)e.Item.DataItem;
				System.Web.UI.WebControls.Label label = (System.Web.UI.WebControls.Label)e.Item.Controls[0].FindControl("lbBundlingID");
				FormatedMoneyLabel formatedMoneyLabel = (FormatedMoneyLabel)e.Item.Controls[0].FindControl("lbltotalPrice");
				FormatedMoneyLabel formatedMoneyLabel2 = (FormatedMoneyLabel)e.Item.Controls[0].FindControl("lblbundlingPrice");
				FormatedMoneyLabel formatedMoneyLabel3 = (FormatedMoneyLabel)e.Item.Controls[0].FindControl("lblsaving");
				System.Web.UI.WebControls.HyperLink hyperLink = (System.Web.UI.WebControls.HyperLink)e.Item.Controls[0].FindControl("hlBuy");
				System.Web.UI.WebControls.Repeater repeater = (System.Web.UI.WebControls.Repeater)e.Item.Controls[0].FindControl("rpbundlingitem");
				System.Collections.Generic.List<BundlingItemInfo> bundlingItemsByID = ProductBrowser.GetBundlingItemsByID(System.Convert.ToInt32(label.Text));
				decimal num = 0m;
				bool flag = false;
				if ((int)dataRowView["SaleStatus"] == 0)
				{
					flag = true;
				}
				foreach (BundlingItemInfo current in bundlingItemsByID)
				{
					num += current.ProductNum * current.ProductPrice;
				}
				formatedMoneyLabel.Money = num;
				formatedMoneyLabel3.Money = System.Convert.ToDecimal(formatedMoneyLabel.Money) - System.Convert.ToDecimal(formatedMoneyLabel2.Money);
				if (!HiContext.Current.SiteSettings.IsOpenSiteSale || flag)
				{
					hyperLink.Visible = false;
				}
				repeater.DataSource = bundlingItemsByID;
				repeater.DataBind();
			}
		}
		private void ReloadHelpList(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			if (!isSearch)
			{
				nameValueCollection.Add("PageIndex", this.pager.PageIndex.ToString());
			}
			nameValueCollection.Add("PageSize", this.pager.PageSize.ToString());
			nameValueCollection.Add("SortOrder", SortAction.Desc.ToString());
			base.ReloadPage(nameValueCollection);
		}
		private void BindBundlingProducts()
		{
			DbQueryResult bundlingProductList = ProductBrowser.GetBundlingProductList(new BundlingInfoQuery
			{
				PageIndex = this.pager.PageIndex,
				PageSize = this.pager.PageSize,
				SortBy = "DisplaySequence",
				SortOrder = SortAction.Desc,
                SupplierId = this.SupplierId
			});
			this.rptProduct.DataSource = bundlingProductList.Data;
			this.rptProduct.DataBind();
			this.pager.TotalRecords = bundlingProductList.TotalRecords;
		}
		public System.Collections.Generic.List<BundlingItemInfo> BindBundlingItems(int id)
		{
			return ProductBrowser.GetBundlingItemsByID(id);
		}
	}
}
