using ASPNET.WebControls;
using EcShop.ControlPanel.Commodities;
using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Core.Enums;
using EcShop.Entities.Commodities;
using EcShop.Entities.Store;
using EcShop.UI.ControlPanel.Utility;
using Ionic.Zip;
using Ionic.Zlib;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin.product
{
	[PrivilegeCheck(Privilege.ProductBatchExport)]
	public class ExportToLocal : AdminPage
	{
		private class ProductDetail
		{
			public ProductInfo pi;
			public System.Collections.Generic.Dictionary<int, System.Collections.Generic.IList<int>> attrs;
			public System.Collections.Generic.IList<int> tagIds;
		}
		private string _productName;
		private string _productCode;
		private int? _categoryId;
		private System.DateTime? _startDate;
		private System.DateTime? _endDate;
		private bool _includeOnSales;
		private bool _includeUnSales;
		private bool _includeInStock;
        private readonly System.Text.Encoding _encoding = System.Text.Encoding.Unicode;
		private readonly string _zipFilename;
		private readonly System.IO.DirectoryInfo _baseDir;
		private readonly string _url;
		private readonly string _applicationPath;
		private readonly string _flag;
		private System.IO.DirectoryInfo _workDir;
		protected System.Web.UI.WebControls.TextBox txtSearchText;
		protected ProductCategoriesDropDownList dropCategories;
		protected System.Web.UI.WebControls.TextBox txtSKU;
		protected WebCalendar calendarStartDate;
		protected WebCalendar calendarEndDate;
		protected System.Web.UI.WebControls.CheckBox chkOnSales;
		protected System.Web.UI.WebControls.CheckBox chkUnSales;
		protected System.Web.UI.WebControls.CheckBox chkInStock;
		protected System.Web.UI.WebControls.Button btnSearch;
		protected System.Web.UI.WebControls.Label lblTotals;
		protected System.Web.UI.WebControls.CheckBox chkExportImages;
		protected System.Web.UI.WebControls.Button btnExport;
		protected Grid grdProducts;
		protected Pager pager;
		public ExportToLocal()
		{
			this._baseDir = new System.IO.DirectoryInfo(System.Web.HttpContext.Current.Request.MapPath("~/storage/data/local"));
			this._flag = System.DateTime.Now.ToString("yyyyMMddHHmmss");
			this._zipFilename = string.Format("local{0}.zip", this._flag);
		}
		protected override void OnInitComplete(System.EventArgs e)
		{
			base.OnInitComplete(e);
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
			this.grdProducts.RowCommand += new System.Web.UI.WebControls.GridViewCommandEventHandler(this.grdProducts_RowCommand);
			if (!this.Page.IsPostBack)
			{
				this.dropCategories.DataBind();
			}
			this.LoadParameters();
		}
		private void grdProducts_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
		{
			if (e.CommandName == "Remove")
			{
				int rowIndex = ((System.Web.UI.WebControls.GridViewRow)((System.Web.UI.Control)e.CommandSource).NamingContainer).RowIndex;
				int num = (int)this.grdProducts.DataKeys[rowIndex].Value;
				string text = (string)this.ViewState["RemoveProductIds"];
				if (string.IsNullOrEmpty(text))
				{
					text = num.ToString();
				}
				else
				{
					text = text + "," + num.ToString();
				}
				this.ViewState["RemoveProductIds"] = text;
				this.BindProducts();
			}
		}
		private void btnExport_Click(object sender, System.EventArgs e)
		{
            //修改1;
			string arg_6B_0 = Globals.ApplicationPath;
			AdvancedProductQuery query = this.GetQuery();
			System.Data.DataTable dataTable = ProductHelper.GetExportProducts(query, true, true, (string)this.ViewState["RemoveProductIds"]).Tables[3];
			System.Collections.Generic.List<ExportToLocal.ProductDetail> list = new System.Collections.Generic.List<ExportToLocal.ProductDetail>();
			foreach (System.Data.DataRow dataRow in dataTable.Rows)
			{
				int productId = System.Convert.ToInt32(dataRow["ProductId"]);
				System.Collections.Generic.IList<int> tagIds = null;
				System.Collections.Generic.Dictionary<int, System.Collections.Generic.IList<int>> attrs;
				ProductInfo productDetails = ProductHelper.GetProductDetails(productId, out attrs, out tagIds);
				list.Add(new ExportToLocal.ProductDetail
				{
					pi = productDetails,
					attrs = attrs,
					tagIds = tagIds
				});
			}
            string strflag;
            if (!(this.calendarStartDate.SelectedDate.HasValue && this.calendarEndDate.SelectedDate.HasValue))
            {
                strflag = "";
            }
            else
            {
                strflag = "_" +
                    DateTime.Parse(this.calendarStartDate.SelectedDate.ToString()).ToString("yyyyMMdd") + "-" + DateTime.Parse(this.calendarEndDate.SelectedDate.ToString()).ToString("yyyyMMdd");
            }
            //this._workDir = this._baseDir.CreateSubdirectory(this._flag);
            //string text = this._workDir.FullName + string.Format("\\local{0}.csv", this._flag);
            this._workDir = this._baseDir.CreateSubdirectory(this._flag);
            string text = this._workDir.FullName + string.Format("\\本地商品数据包{0}.csv", strflag);

			string text2 = text.Replace(".csv", "");
			if (!System.IO.Directory.Exists(text2))
			{
				System.IO.Directory.CreateDirectory(text2);
			}
			this.DoExportForHishop(text, text2, list);
		}
		private void DoExportForHishop(string csvFilename, string imagePath, System.Collections.Generic.List<ExportToLocal.ProductDetail> list)
		{
			using (System.IO.FileStream fileStream = new System.IO.FileStream(csvFilename, System.IO.FileMode.Create, System.IO.FileAccess.Write))
			{
                string productCSVForEcdev = this.GetProductCSVForHishop(imagePath, list);
				System.Text.UnicodeEncoding unicodeEncoding = new System.Text.UnicodeEncoding();
                //UTF8Encoding unicodeEncoding = new UTF8Encoding();
                int byteCount = unicodeEncoding.GetByteCount(productCSVForEcdev);
				byte[] preamble = unicodeEncoding.GetPreamble();
				byte[] array = new byte[preamble.Length + byteCount];
				System.Buffer.BlockCopy(preamble, 0, array, 0, preamble.Length);
				unicodeEncoding.GetBytes(productCSVForEcdev.ToCharArray(), 0, productCSVForEcdev.Length, array, preamble.Length);
				fileStream.Write(array, 0, array.Length);
			}
            using (ZipFile zipFile = new ZipFile(System.Text.Encoding.Default))
			{
				System.IO.DirectoryInfo directoryInfo = new System.IO.DirectoryInfo(imagePath);
				zipFile.CompressionLevel = CompressionLevel.Default;
				zipFile.AddFile(csvFilename, "");
				zipFile.AddDirectory(directoryInfo.FullName, directoryInfo.Name);
				System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
				response.ContentType = "application/x-zip-compressed";
				response.ContentEncoding = this._encoding;
				response.AddHeader("Content-Disposition", "attachment; filename=" + directoryInfo.Name + ".zip");
				response.Clear();
				zipFile.Save(response.OutputStream);
				this._workDir.Delete(true);
				response.Flush();
				response.Close();
			}
		}
		public string ConvertNull(object obj)
		{
			if (obj is System.DBNull || obj == null)
			{
				obj = "";
			}
			return obj.ToString().Replace("\t", " ").Replace("\r\n", "");
		}
		private string GetProductCSVForHishop(string imagePath, System.Collections.Generic.List<ExportToLocal.ProductDetail> list)
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			string format = "\r\n-1\t\"{0}\"\t\"{1}\"\t\"{2}\"\t\"{3}\"\t\"{4}\"\t\"{5}\"\t\"{6}\"\t\"{7}\"\t\"{8}\"\t\"{9}\"\t\"{10}\"\t\"{11}\"\t\"{12}\"\t\"{13}\"\t\"{14}\"\t\"{15}\"\t\"{16}\"\t\"{17}\"\t{18}\t\"{19}\"\t\"{20}\"\t\"{21}\"\t\"{22}\"\t\"{23}\"\t\"{24}\"\t\"{25}\"\t\"{26}\"";
			stringBuilder.Append("\"id\"\t\"所属分类\"\t\"商品类型\"\t\"商品名称\"\t\"商家编码\"\t\"简单描述\"\t\"计量单位\"\t");
			stringBuilder.Append("\"详细描述\"\t\"详细页标题\"\t\"详细页描述\"\t\"详细页搜索关键字\"\t\"销售状态\"\t");
			stringBuilder.Append("\"图片\"\t\"图片2\"\t\"图片3\"\t\"图片4\"\t\"图片5\"\t");
			stringBuilder.Append("\"市场价\"\t\"品牌\"\t\"是否有规格\"\t");
			stringBuilder.Append("\"规格属性\"\t\"货号\"\t\"重量\"\t\"库存\"\t成本价\"\t\"一口价\"\t");
			stringBuilder.Append("\"商品属性\"\t\"商品标签\"");
			foreach (ExportToLocal.ProductDetail current in list)
			{
				string text = "{" + System.Guid.NewGuid().ToString() + "}.htm";
				string path = System.IO.Path.Combine(imagePath, text);
				using (System.IO.StreamWriter streamWriter = new System.IO.StreamWriter(path, false, System.Text.Encoding.GetEncoding("gb2312")))
				{
					if (!string.IsNullOrEmpty(current.pi.Description))
					{
						string description = current.pi.Description;
						streamWriter.Write(description);
					}
				}
				string text2;
				if (!string.IsNullOrEmpty(current.pi.ImageUrl1) && !current.pi.ImageUrl1.StartsWith("http://"))
				{
					text2 = current.pi.ImageUrl1;
					if (System.IO.File.Exists(base.Server.MapPath("~" + text2)))
					{
						System.IO.FileInfo fileInfo = new System.IO.FileInfo(base.Server.MapPath("~" + text2));
						text2 = fileInfo.Name.ToLower();
						fileInfo.CopyTo(System.IO.Path.Combine(imagePath, text2), true);
					}
				}
				else
				{
					text2 = "";
				}
				string text3;
				if (!string.IsNullOrEmpty(current.pi.ImageUrl2) && !current.pi.ImageUrl2.StartsWith("http://"))
				{
					text3 = current.pi.ImageUrl2;
					if (System.IO.File.Exists(base.Server.MapPath("~" + text3)))
					{
						System.IO.FileInfo fileInfo2 = new System.IO.FileInfo(base.Server.MapPath("~" + text3));
						text3 = fileInfo2.Name.ToLower();
						fileInfo2.CopyTo(System.IO.Path.Combine(imagePath, text3), true);
					}
				}
				else
				{
					text3 = "";
				}
				string text4;
				if (!string.IsNullOrEmpty(current.pi.ImageUrl3) && !current.pi.ImageUrl3.StartsWith("http://"))
				{
					text4 = current.pi.ImageUrl3;
					if (System.IO.File.Exists(base.Server.MapPath("~" + text4)))
					{
						System.IO.FileInfo fileInfo3 = new System.IO.FileInfo(base.Server.MapPath("~" + text4));
						text4 = fileInfo3.Name.ToLower();
						fileInfo3.CopyTo(System.IO.Path.Combine(imagePath, text4), true);
					}
				}
				else
				{
					text4 = "";
				}
				string text5;
				if (!string.IsNullOrEmpty(current.pi.ImageUrl4) && !current.pi.ImageUrl4.StartsWith("http://"))
				{
					text5 = current.pi.ImageUrl4;
					if (System.IO.File.Exists(base.Server.MapPath("~" + text5)))
					{
						System.IO.FileInfo fileInfo4 = new System.IO.FileInfo(base.Server.MapPath("~" + text5));
						text5 = fileInfo4.Name.ToLower();
						fileInfo4.CopyTo(System.IO.Path.Combine(imagePath, text5), true);
					}
				}
				else
				{
					text5 = "";
				}
				string text6;
				if (!string.IsNullOrEmpty(current.pi.ImageUrl5) && !current.pi.ImageUrl5.StartsWith("http://"))
				{
					text6 = current.pi.ImageUrl5;
					if (System.IO.File.Exists(base.Server.MapPath("~" + text6)))
					{
						System.IO.FileInfo fileInfo5 = new System.IO.FileInfo(base.Server.MapPath("~" + text6));
						text6 = fileInfo5.Name.ToLower();
						fileInfo5.CopyTo(System.IO.Path.Combine(imagePath, text6), true);
					}
				}
				else
				{
					text6 = "";
				}
				string text7 = "";
				string text8 = "";
				string text9 = "";
				string text10 = "";
				string text11 = "";
				string text12 = "";
				string text13 = "";
				string text14 = "";
				string text15 = "";
				string arg_43B_0 = current.pi.SkuId;
				foreach (SKUItem current2 in current.pi.Skus.Values)
				{
					text8 = text8 + current2.SKU + ";";
					text9 = text9 + ((this.ConvertNull(current2.Weight) == "") ? "0" : this.ConvertNull(current2.Weight)) + ";";
					text10 = text10 + current2.Stock + ";";
					text11 = text11 + ((this.ConvertNull(current2.CostPrice) == "") ? "0" : this.ConvertNull(current2.CostPrice)) + ";";
					text12 = text12 + current2.SalePrice + ";";
				}
				if (text11 == "")
				{
					text11 = "0";
				}
				if (text10 == "")
				{
					text10 = "0";
				}
				if (!this.chkExportImages.Checked)
				{
					text3 = (text2 = (text4 = (text5 = (text6 = ""))));
				}
				if (current.pi.TypeId.HasValue)
				{
					int arg_5C7_0 = current.pi.TypeId.Value;
					foreach (System.Collections.Generic.KeyValuePair<int, System.Collections.Generic.IList<int>> current3 in current.attrs)
					{
						string attributeName = ProductTypeHelper.GetAttribute(current3.Key).AttributeName;
						foreach (int current4 in current3.Value)
						{
							string valueStr = ProductTypeHelper.GetAttributeValueInfo(current4).ValueStr;
							string text16 = text13;
							text13 = string.Concat(new string[]
							{
								text16,
								attributeName,
								":",
								valueStr,
								","
							});
						}
					}
					System.Data.DataTable tags = CatalogHelper.GetTags();
					foreach (int current5 in current.tagIds)
					{
						foreach (System.Data.DataRow dataRow in tags.Rows)
						{
							if (dataRow["TagId"].Equals(current5))
							{
								text14 = text14 + dataRow["TagName"] + ",";
								break;
							}
						}
					}
				}
				switch (current.pi.SaleStatus)
				{
				case ProductSaleStatus.OnSale:
					text15 = "出售中";
					break;
				case ProductSaleStatus.UnSale:
					text15 = "下架区";
					break;
				case ProductSaleStatus.OnStock:
					text15 = "仓库中";
					break;
				}
				bool hasSKU = current.pi.HasSKU;
				if (hasSKU)
				{
					foreach (SKUItem current6 in current.pi.Skus.Values)
					{
						foreach (System.Collections.Generic.KeyValuePair<int, int> current7 in current6.SkuItems)
						{
							string attributeName2 = ProductTypeHelper.GetAttribute(current7.Key).AttributeName;
							foreach (AttributeValueInfo current8 in ProductTypeHelper.GetAttribute(current7.Key).AttributeValues)
							{
								if (current8.ValueId.Equals(current7.Value))
								{
									string valueStr2 = current8.ValueStr;
									text7 = text7 + attributeName2 + ":" + valueStr2;
									break;
								}
							}
							text7 += ",";
						}
						text7 = text7.Trim(new char[]
						{
							','
						}) + ";";
					}
				}
				string obj = "";
				CategoryInfo category = CatalogHelper.GetCategory(current.pi.CategoryId);
				if (category != null)
				{
					obj = category.Name;
				}
				string obj2 = "";
				if (current.pi.TypeId.HasValue)
				{
					obj2 = ProductTypeHelper.GetProductType(current.pi.TypeId.Value).TypeName;
				}
				string obj3 = "";
				if (current.pi.BrandId.HasValue)
				{
					obj3 = CatalogHelper.GetBrandCategory(current.pi.BrandId.Value).BrandName;
				}
				stringBuilder.AppendFormat(format, new object[]
				{
					this.ConvertNull(obj),
					this.ConvertNull(obj2),
					current.pi.ProductName,
					this.ConvertNull(current.pi.ProductCode),
					this.ConvertNull(current.pi.ShortDescription),
					this.ConvertNull(current.pi.Unit),
					text,
					this.ConvertNull(current.pi.Title),
					this.ConvertNull(current.pi.MetaDescription),
					this.ConvertNull(current.pi.MetaKeywords),
					text15,
					text2,
					text3,
					text4,
					text5,
					text6,
					this.ConvertNull(current.pi.MarketPrice),
					this.ConvertNull(obj3),
					hasSKU ? "1" : "0",
					text7.Trim(new char[]
					{
						';'
					}),
					text8.Trim(new char[]
					{
						';'
					}),
					text9.Trim(new char[]
					{
						';'
					}),
					text10.Trim(new char[]
					{
						';'
					}),
					text11.Trim(new char[]
					{
						';'
					}),
					text12.Trim(new char[]
					{
						';'
					}),
					text13.Trim(new char[]
					{
						','
					}),
					text14.Trim(new char[]
					{
						','
					})
				});
			}
			return stringBuilder.ToString();
		}
		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			this.ReSearchProducts();
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				this.BindProducts();
			}
		}
		private void BindProducts()
		{
			if (!this._includeUnSales && !this._includeOnSales && !this._includeInStock)
			{
				this.ShowMsg("至少要选择包含一个商品状态", false);
				return;
			}
			AdvancedProductQuery query = this.GetQuery();
			DbQueryResult exportProducts = ProductHelper.GetExportProducts(query, (string)this.ViewState["RemoveProductIds"]);
			this.grdProducts.DataSource = exportProducts.Data;
			this.grdProducts.DataBind();
			this.pager.TotalRecords = exportProducts.TotalRecords;
			this.lblTotals.Text = exportProducts.TotalRecords.ToString(System.Globalization.CultureInfo.InvariantCulture);
		}
		private AdvancedProductQuery GetQuery()
		{
			AdvancedProductQuery advancedProductQuery = new AdvancedProductQuery
			{
				Keywords = this._productName,
				ProductCode = this._productCode,
				CategoryId = this._categoryId,
				PageSize = this.pager.PageSize,
				PageIndex = this.pager.PageIndex,
				SaleStatus = ProductSaleStatus.OnSale,
				SortOrder = SortAction.Desc,
				SortBy = "DisplaySequence",
				StartDate = this._startDate,
				EndDate = this._endDate,
				IncludeInStock = this._includeInStock,
				IncludeOnSales = this._includeOnSales,
				IncludeUnSales = this._includeUnSales
			};
			if (this._categoryId.HasValue)
			{
				advancedProductQuery.MaiCategoryPath = CatalogHelper.GetCategory(this._categoryId.Value).Path;
			}
			Globals.EntityCoding(advancedProductQuery, true);
			return advancedProductQuery;
		}
		private void ReSearchProducts()
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection
			{

				{
					"productName",
					Globals.UrlEncode(this.txtSearchText.Text.Trim())
				},

				{
					"productCode",
					Globals.UrlEncode(Globals.HtmlEncode(this.txtSKU.Text.Trim()))
				},

				{
					"pageSize",
					this.pager.PageSize.ToString()
				},

				{
					"includeOnSales",
					this.chkOnSales.Checked.ToString()
				},

				{
					"includeUnSales",
					this.chkUnSales.Checked.ToString()
				},

				{
					"includeInStock",
					this.chkInStock.Checked.ToString()
				}
			};
			if (this.dropCategories.SelectedValue.HasValue)
			{
				nameValueCollection.Add("categoryId", this.dropCategories.SelectedValue.ToString());
			}
			nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString());
			if (this.calendarStartDate.SelectedDate.HasValue)
			{
				nameValueCollection.Add("startDate", this.calendarStartDate.SelectedDate.Value.ToString());
			}
			if (this.calendarEndDate.SelectedDate.HasValue)
			{
				nameValueCollection.Add("endDate", this.calendarEndDate.SelectedDate.Value.ToString());
			}
			base.ReloadPage(nameValueCollection);
		}
		private void LoadParameters()
		{
			this._productName = this.txtSearchText.Text.Trim();
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productName"]))
			{
				this._productName = Globals.UrlDecode(this.Page.Request.QueryString["productName"]);
				this.txtSearchText.Text = this._productName;
			}
			this._productCode = this.txtSKU.Text.Trim();
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productCode"]))
			{
				this._productCode = Globals.UrlDecode(this.Page.Request.QueryString["productCode"]);
				this.txtSKU.Text = this._productCode;
			}
			this._categoryId = this.dropCategories.SelectedValue;
			int value;
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["categoryId"]) && int.TryParse(this.Page.Request.QueryString["categoryId"], out value))
			{
				this._categoryId = new int?(value);
				this.dropCategories.SelectedValue = this._categoryId;
			}
			this._startDate = this.calendarStartDate.SelectedDate;
			System.DateTime value2;
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["startDate"]) && System.DateTime.TryParse(this.Page.Request.QueryString["startDate"], out value2))
			{
				this._startDate = new System.DateTime?(value2);
				this.calendarStartDate.SelectedDate = this._startDate;
			}
			this._endDate = this.calendarEndDate.SelectedDate;
			System.DateTime value3;
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["endDate"]) && System.DateTime.TryParse(this.Page.Request.QueryString["endDate"], out value3))
			{
				this._endDate = new System.DateTime?(value3);
				this.calendarEndDate.SelectedDate = this._endDate;
			}
			this._includeOnSales = this.chkOnSales.Checked;
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["includeOnSales"]))
			{
				bool.TryParse(this.Page.Request.QueryString["includeOnSales"], out this._includeOnSales);
				this.chkOnSales.Checked = this._includeOnSales;
			}
			this._includeUnSales = this.chkUnSales.Checked;
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["includeUnSales"]))
			{
				bool.TryParse(this.Page.Request.QueryString["includeUnSales"], out this._includeUnSales);
				this.chkUnSales.Checked = this._includeUnSales;
			}
			this._includeInStock = this.chkInStock.Checked;
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["includeInStock"]))
			{
				bool.TryParse(this.Page.Request.QueryString["includeInStock"], out this._includeInStock);
				this.chkInStock.Checked = this._includeInStock;
			}
		}
	}
}
