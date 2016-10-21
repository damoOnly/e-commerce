using EcShop.ControlPanel.Promotions;
using EcShop.ControlPanel.Store;
using EcShop.Entities.Promotions;
using EcShop.Entities.Store;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin.promotion
{
	[PrivilegeCheck(Privilege.BindProduct)]
	public class EditBundlingProduct : AdminPage
	{
		private int bundlingid;
		protected System.Web.UI.WebControls.TextBox txtBindName;
		protected YesNoRadioButtonList radstock;
		protected System.Web.UI.WebControls.TextBox txtNum;
		protected System.Web.UI.WebControls.TextBox txtSalePrice;
		protected TrimTextBox txtShortDescription;
		protected System.Web.UI.WebControls.Repeater Rpbinditems;
		protected System.Web.UI.WebControls.Button btnEditBindProduct;
		protected ProductCategoriesDropDownList dropCategories;
		protected BrandCategoriesDropDownList dropBrandList;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!int.TryParse(base.Request.QueryString["bundlingid"], out this.bundlingid))
			{
				this.ShowMsg("参数提交错误", false);
				return;
			}
			if (!this.Page.IsPostBack)
			{
				this.BindEditBindProduct();
				this.dropCategories.DataBind();
				this.dropBrandList.DataBind();
			}
		}
		protected void BindEditBindProduct()
		{
			BundlingInfo bundlingInfo = PromoteHelper.GetBundlingInfo(this.bundlingid);
			this.txtBindName.Text = bundlingInfo.Name;
			this.txtNum.Text = bundlingInfo.Num.ToString();
			this.txtSalePrice.Text = bundlingInfo.Price.ToString("F");
			this.radstock.SelectedValue = System.Convert.ToBoolean(bundlingInfo.SaleStatus);
			this.txtShortDescription.Text = bundlingInfo.ShortDescription;
			this.Rpbinditems.DataSource = bundlingInfo.BundlingItemInfos;
			this.Rpbinditems.DataBind();
		}
		protected void btnEditBindProduct_Click(object sender, System.EventArgs e)
		{
			if (string.IsNullOrEmpty(base.Request.Form["selectProductsinfo"]))
			{
				this.ShowMsg("获取绑定商品信息错误", false);
				return;
			}
			if (string.IsNullOrEmpty(this.txtBindName.Text) || string.IsNullOrEmpty(this.txtNum.Text) || string.IsNullOrEmpty(this.txtSalePrice.Text))
			{
				this.ShowMsg("你的资料填写不完整", false);
				return;
			}
			BundlingInfo bundlingInfo = new BundlingInfo();
			bundlingInfo.BundlingID = this.bundlingid;
			bundlingInfo.AddTime = System.DateTime.Now;
			bundlingInfo.Name = this.txtBindName.Text;
			bundlingInfo.Price = System.Convert.ToDecimal(this.txtSalePrice.Text);
			bundlingInfo.SaleStatus = System.Convert.ToInt32(this.radstock.SelectedValue);
			int num = 0;
			int.TryParse(this.txtNum.Text, out num);
			bundlingInfo.Num = num;
			if (!string.IsNullOrEmpty(this.txtShortDescription.Text))
			{
				bundlingInfo.ShortDescription = this.txtShortDescription.Text;
			}
			string text = base.Request.Form["selectProductsinfo"];
			string[] array = text.Split(new char[]
			{
				','
			});
			System.Collections.Generic.List<BundlingItemInfo> list = new System.Collections.Generic.List<BundlingItemInfo>();
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string text2 = array2[i];
				BundlingItemInfo bundlingItemInfo = new BundlingItemInfo();
				string[] array3 = text2.Split(new char[]
				{
					'|'
				});
				bundlingItemInfo.ProductID = System.Convert.ToInt32(array3[0]);
				bundlingItemInfo.SkuId = array3[1];
				int num2 = 1;
				int.TryParse(array3[2], out num2);
				if (num2 <= 0)
				{
					num2 = 1;
				}
				bundlingItemInfo.ProductNum = num2;
				list.Add(bundlingItemInfo);
			}
			bundlingInfo.BundlingItemInfos = list;
			if (PromoteHelper.UpdateBundlingProduct(bundlingInfo))
			{
				this.ShowMsg("修改绑定商品成功", true);
				this.BindEditBindProduct();
				return;
			}
			this.ShowMsg("修改绑定商品错误", false);
		}
	}
}
