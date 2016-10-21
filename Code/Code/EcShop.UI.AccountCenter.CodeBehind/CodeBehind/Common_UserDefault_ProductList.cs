using EcShop.Entities;
using EcShop.Entities.Commodities;
using EcShop.SaleSystem.Catalog;
using EcShop.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
namespace EcShop.UI.AccountCenter.CodeBehind
{
	public class Common_UserDefault_ProductList : AscxTemplatedWebControl
	{
		public const string TagID = "list_UserDefault_ProductList";
		private System.Web.UI.WebControls.Repeater rp_guest;
		private System.Web.UI.WebControls.Repeater rp_hot;
		private System.Web.UI.WebControls.Repeater rp_new;
		private int maxNum = 12;
		public override string ID
		{
			get
			{
				return base.ID;
			}
			set
			{
			}
		}
		public int MaxNum
		{
			get
			{
				return this.maxNum;
			}
			set
			{
				this.maxNum = value;
			}
		}
		public Common_UserDefault_ProductList()
		{
			base.ID = "list_UserDefault_ProductList";
		}
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/ascx/tags/Common_UserCenter/Skin-Common_UserDefault_ProductList.ascx";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.rp_guest = (System.Web.UI.WebControls.Repeater)this.FindControl("rp_guest");
			this.rp_hot = (System.Web.UI.WebControls.Repeater)this.FindControl("rp_hot");
			this.rp_new = (System.Web.UI.WebControls.Repeater)this.FindControl("rp_new");
			this.BindList();
		}
		private void BindList()
		{
			if (this.rp_guest != null)
			{
				System.Collections.Generic.IList<int> browedProductList = BrowsedProductQueue.GetBrowedProductList(this.MaxNum);
                this.rp_guest.DataSource = ProductBrowser.GetSuggestProductsProducts(browedProductList,5);
                //this.rp_guest.DataSource = ProductBrowser.GetVistiedProducts(browedProductList, ClientType.PC);
				this.rp_guest.DataBind();
			}
			if (this.rp_hot != null)
			{
				this.rp_hot.DataSource = ProductBrowser.GetSaleProductRanking(new int?(0), this.maxNum);
				this.rp_hot.DataBind();
			}
			if (this.rp_new != null)
			{
				SubjectListQuery subjectListQuery = new SubjectListQuery();
				subjectListQuery.MaxNum = this.maxNum;
				subjectListQuery.SortBy = "DisplaySequence";
				this.rp_new.DataSource = ProductBrowser.GetSubjectList(subjectListQuery);
				this.rp_new.DataBind();
			}
		}
	}
}
