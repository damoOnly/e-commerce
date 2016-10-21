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
using System.Collections.Specialized;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using ThoughtWorks.QRCode.Codec;
namespace EcShop.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.Products)]
	public class SelectShareProduct : AdminPage
	{
		private string productName;
		private string productCode;
		private int? categoryId;
		private int? lineId;
		private int? tagId;
		private int? typeId;
		private int? distributorId;
		private bool isAlert;
		private int? shareId;
		private ProductSaleStatus saleStatus = ProductSaleStatus.OnSale;
		private PenetrationStatus penetrationStatus;
		private System.DateTime? startDate;
		private System.DateTime? endDate;
		protected System.Web.UI.WebControls.Literal litnumber;
		protected System.Web.UI.WebControls.Panel panlproducts;
		protected System.Web.UI.WebControls.TextBox txtSearchText;
		protected ProductCategoriesDropDownList dropCategories;
		protected BrandCategoriesDropDownList dropBrandList;
		protected ProductTagsDropDownList dropTagList;
		protected ProductTypeDownList dropType;
		protected WebCalendar calendarStartDate;
		protected WebCalendar calendarEndDate;
		protected System.Web.UI.WebControls.Button btnSearch;
		protected PageSize hrefPageSize;
		protected Pager pager1;
		protected System.Web.UI.WebControls.LinkButton btnBatchDelete;
		protected System.Web.UI.WebControls.Repeater rp_shareproduct;
		protected Pager pager;
		protected System.Web.UI.WebControls.Panel SharePanel;
		protected System.Web.UI.HtmlControls.HtmlInputText txtlink;
		protected System.Web.UI.WebControls.Image imgurl;
		protected System.Web.UI.HtmlControls.HtmlInputText txttitle;
		protected System.Web.UI.WebControls.Button btnSharePage;
		private string TemporaryProductId
		{
			get
			{
				System.Web.HttpCookie httpCookie = HiContext.Current.Context.Request.Cookies["tempproductId"];
				if (httpCookie == null || string.IsNullOrEmpty(httpCookie.Value))
				{
					return "";
				}
				return httpCookie.Value;
			}
			set
			{
				System.Web.HttpCookie httpCookie = HiContext.Current.Context.Request.Cookies["tempproductId"];
				if (value == null)
				{
					httpCookie.Value = "";
					HiContext.Current.Context.Response.Cookies.Set(httpCookie);
					return;
				}
				if (httpCookie != null && !string.IsNullOrEmpty(httpCookie.Value))
				{
					string text = "";
					string[] array;
					if (value.Contains(","))
					{
						array = value.Split(new char[]
						{
							','
						});
					}
					else
					{
						array = new string[]
						{
							value
						};
					}
					string[] array2;
					if (httpCookie.Value.Contains(","))
					{
						array2 = httpCookie.Value.Split(new char[]
						{
							','
						});
					}
					else
					{
						array2 = new string[]
						{
							httpCookie.Value
						};
					}
					for (int i = 0; i < array2.Length; i++)
					{
						bool flag = true;
						for (int j = 0; j < array.Length; j++)
						{
							if (array2[i] == array[j])
							{
								flag = false;
								break;
							}
						}
						if (flag)
						{
							text = text + array2[i] + ",";
						}
					}
					if (text != "")
					{
						text = text.Substring(0, text.Length - 1);
					}
					httpCookie.Value = text;
					HiContext.Current.Context.Response.Cookies.Set(httpCookie);
					return;
				}
				httpCookie = new System.Web.HttpCookie("tempproductId", value);
				httpCookie.Expires = System.DateTime.Now.AddDays(3.0);
				HiContext.Current.Context.Response.Cookies.Add(httpCookie);
			}
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			this.btnSharePage.Click += new System.EventHandler(this.btnSharePage_Click);
			this.btnBatchDelete.Click += new System.EventHandler(this.btnBatchDelete_Click);
			this.rp_shareproduct.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(this.rp_shareproduct_ItemDataBound);
			this.rp_shareproduct.ItemCommand += new System.Web.UI.WebControls.RepeaterCommandEventHandler(this.rp_shareproduct_ItemCommand);
			if (!this.Page.IsPostBack)
			{
				this.dropCategories.DataBind();
				this.dropBrandList.DataBind();
				this.dropTagList.DataBind();
				this.dropType.DataBind();
				this.BindProducts();
				this.GetSelectProductNumber();
			}
		}
		private void btnBatchDelete_Click(object sender, System.EventArgs e)
		{
			string text = base.Request.Form["CheckBoxGroup"];
			if (string.IsNullOrEmpty(text))
			{
				this.ShowMsg("请先选择要删除的商品", false);
				return;
			}
			this.TemporaryProductId = text;
			this.ReloadProductOnSales(false);
			this.ShowMsg("批量删除成功", true);
		}
		private void btnSharePage_Click(object sender, System.EventArgs e)
		{
			if (string.IsNullOrEmpty(this.TemporaryProductId))
			{
				this.ShowMsg("请先添加要分享的商品", false);
				return;
			}
			if (string.IsNullOrEmpty(this.txttitle.Value))
			{
				this.ShowMsg("请输入分享页标题！", false);
				return;
			}
			this.TemporaryProductId = null;
			this.shareId = new int?(ProductHelper.AddShareLine(new ShareProductInfo
			{
				ShareTitle = this.txttitle.Value
			}, this.TemporaryProductId));
			this.ReloadProductOnSales(false);
		}
		private void rp_shareproduct_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Item || e.Item.ItemType == System.Web.UI.WebControls.ListItemType.AlternatingItem)
			{
				System.Web.UI.WebControls.Literal literal = (System.Web.UI.WebControls.Literal)e.Item.FindControl("litSaleStatus");
				System.Web.UI.WebControls.Literal literal2 = (System.Web.UI.WebControls.Literal)e.Item.FindControl("litMarketPrice");
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
		private void rp_shareproduct_ItemCommand(object sender, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
		{
			if (e.CommandName == "delete")
			{
				this.TemporaryProductId = e.CommandArgument.ToString();
				this.ReloadProductOnSales(false);
			}
		}
		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			this.ReloadProductOnSales(true);
		}
		private void BindProducts()
		{
			this.LoadParameters();
			ProductQuery productQuery = new ProductQuery
			{
				Keywords = this.productName,
				ProductCode = this.productCode,
				CategoryId = this.categoryId,
				ProductLineId = this.lineId,
				PageSize = this.pager.PageSize,
				PageIndex = this.pager.PageIndex,
				SortOrder = SortAction.Desc,
				SortBy = "DisplaySequence",
				StartDate = this.startDate,
				BrandId = this.dropBrandList.SelectedValue.HasValue ? this.dropBrandList.SelectedValue : null,
				TagId = this.dropTagList.SelectedValue.HasValue ? this.dropTagList.SelectedValue : null,
				TypeId = this.typeId,
				UserId = this.distributorId,
				IsAlert = this.isAlert,
				SaleStatus = this.saleStatus,
				PenetrationStatus = this.penetrationStatus,
				EndDate = this.endDate
			};
			if (this.categoryId.HasValue)
			{
				productQuery.MaiCategoryPath = CatalogHelper.GetCategory(this.categoryId.Value).Path;
			}
			Globals.EntityCoding(productQuery, true);
			string temporaryProductId = this.TemporaryProductId;
			if (!string.IsNullOrEmpty(temporaryProductId))
			{
				DbQueryResult shareProducts = ProductHelper.GetShareProducts(productQuery, true, this.TemporaryProductId);
				this.rp_shareproduct.DataSource = shareProducts.Data;
				this.rp_shareproduct.DataBind();
				this.pager1.TotalRecords = (this.pager.TotalRecords = shareProducts.TotalRecords);
			}
			this.txtSearchText.Text = productQuery.Keywords;
			this.dropCategories.SelectedValue = productQuery.CategoryId;
			this.dropType.SelectedValue = productQuery.TypeId;
		}
		public void GetSelectProductNumber()
		{
			string text = "";
			System.Web.HttpCookie httpCookie = HiContext.Current.Context.Request.Cookies["tempproductId"];
			if (httpCookie != null && !string.IsNullOrEmpty(httpCookie.Value))
			{
				text = "(" + httpCookie.Value.Split(new char[]
				{
					','
				}).Length.ToString() + ")";
			}
			this.litnumber.Text = text;
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
			if (int.TryParse(this.Page.Request.QueryString["lineId"], out value3))
			{
				this.lineId = new int?(value3);
			}
			int value4 = 0;
			if (int.TryParse(this.Page.Request.QueryString["tagId"], out value4))
			{
				this.tagId = new int?(value4);
			}
			int value5 = 0;
			if (int.TryParse(this.Page.Request.QueryString["typeId"], out value5))
			{
				this.typeId = new int?(value5);
			}
			int value6 = 0;
			if (int.TryParse(this.Page.Request.QueryString["distributorId"], out value6))
			{
				this.distributorId = new int?(value6);
			}
			bool.TryParse(this.Page.Request.QueryString["isAlert"], out this.isAlert);
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["SaleStatus"]))
			{
				this.saleStatus = (ProductSaleStatus)System.Enum.Parse(typeof(ProductSaleStatus), this.Page.Request.QueryString["SaleStatus"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["PenetrationStatus"]))
			{
				this.penetrationStatus = (PenetrationStatus)System.Enum.Parse(typeof(PenetrationStatus), this.Page.Request.QueryString["PenetrationStatus"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["startDate"]))
			{
				this.startDate = new System.DateTime?(System.DateTime.Parse(this.Page.Request.QueryString["startDate"]));
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["endDate"]))
			{
				this.endDate = new System.DateTime?(System.DateTime.Parse(this.Page.Request.QueryString["endDate"]));
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["shareId"]))
			{
				this.shareId = new int?(System.Convert.ToInt32(this.Page.Request.QueryString["shareId"]));
			}
			if (this.shareId.HasValue && this.shareId.Value > 0)
			{
				this.TemporaryProductId = null;
				string value7 = Globals.HostPath(System.Web.HttpContext.Current.Request.Url) + "/WapShop/ShareProducts.aspx?Id=" + this.shareId.Value;
				ShareProductInfo shareProductInfoById = ProductHelper.GetShareProductInfoById(this.shareId.Value);
				string text = this.Create_ImgCode(value7, 10);
				shareProductInfoById.ShareUrl = text;
				if (ProductHelper.UpdateShareLine(shareProductInfoById))
				{
					this.panlproducts.Visible = false;
					this.SharePanel.Visible = true;
					this.imgurl.ImageUrl = text;
					this.txtlink.Value = value7;
				}
			}
			this.txtSearchText.Text = this.productName;
			this.dropCategories.DataBind();
			this.dropCategories.SelectedValue = this.categoryId;
			this.dropTagList.DataBind();
			this.dropTagList.SelectedValue = this.tagId;
			this.calendarStartDate.SelectedDate = this.startDate;
			this.calendarEndDate.SelectedDate = this.endDate;
			this.dropType.SelectedValue = this.typeId;
		}
		private string Create_ImgCode(string imgurl, int size)
		{
			string result = "";
			System.Drawing.Bitmap bitmap = new QRCodeEncoder
			{
				QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE,
				QRCodeScale = size,
				QRCodeVersion = 0,
				QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M
			}.Encode(imgurl);
			string text = this.Page.Request.MapPath(Globals.ApplicationPath + "/Storage/master/shareproduct");
			if (System.IO.Directory.Exists(text))
			{
				string text2 = System.Guid.NewGuid().ToString().Replace("-", "") + ".png";
				bitmap.Save(text + "/" + text2, System.Drawing.Imaging.ImageFormat.Png);
				result = "/Storage/master/shareproduct/" + text2;
			}
			else
			{
				System.IO.Directory.CreateDirectory(text);
			}
			return result;
		}
		private void ReloadProductOnSales(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			nameValueCollection.Add("productName", Globals.UrlEncode(this.txtSearchText.Text.Trim()));
			if (this.dropCategories.SelectedValue.HasValue)
			{
				nameValueCollection.Add("categoryId", this.dropCategories.SelectedValue.ToString());
			}
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
			if (this.shareId.HasValue && this.shareId.Value > 0)
			{
				nameValueCollection.Add("shareId", this.shareId.Value.ToString());
			}
			base.ReloadPage(nameValueCollection);
		}
	}
}
