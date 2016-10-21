using EcShop.ControlPanel.Commodities;
using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Entities.Commodities;
using EcShop.Entities.HOP;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
namespace EcShop.UI.Web.API
{
	public class TaobaoProductHandler : System.Web.IHttpHandler
	{
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
		public void ProcessRequest(System.Web.HttpContext context)
		{
			context.Response.ContentType = "text/plain";
			GzipExtention.Gzip(context);
			string text = context.Request.Form["action"];
			string key;
			switch (key = text)
			{
			case "ProductSearch":
				this.ProcessProductSearch(context);
				return;
			case "ProductDetails":
				this.ProcessProductDetails(context);
				return;
			case "TaobaoProductMake":
				this.ProcessTaobaoProductMake(context);
				return;
			case "TaobaoProductDetails":
				this.ProcessTaobaoProductDetails(context);
				return;
			case "TaobaoProductIdAdd":
				this.ProcessTaobaoProductIdAdd(context);
				return;
			case "TaobaoProductIsExit":
				this.ProcessTaobaoProductIsExit(context);
				return;
			case "TaobaoProductDown":
				this.ProcessTaobaoProductDown(context);
				return;
			}
			context.Response.Write("error");
		}
		private void ProcessProductSearch(System.Web.HttpContext context)
		{
			string str = "http://" + System.Web.HttpContext.Current.Request.Url.Host + ((System.Web.HttpContext.Current.Request.Url.Port == 80) ? "" : (":" + System.Web.HttpContext.Current.Request.Url.Port));
			ProductQuery productQuery = new ProductQuery();
			productQuery.PageIndex = int.Parse(context.Request.Form["pageIndex"]);
			productQuery.PageSize = int.Parse(context.Request.Form["pageSize"]);
			productQuery.Keywords = context.Request.Form["productName"];
			productQuery.ProductCode = context.Request.Form["productCode"];
			if (!string.IsNullOrEmpty(context.Request.Form["publishStatus"]))
			{
				productQuery.PublishStatus = (PublishStatus)int.Parse(context.Request.Form["publishStatus"]);
			}
			if (!string.IsNullOrEmpty(context.Request.Form["IsMakeTaobao"]))
			{
				productQuery.IsMakeTaobao = new int?(int.Parse(context.Request.Form["IsMakeTaobao"]));
			}
			if (!string.IsNullOrEmpty(context.Request.Form["startDate"]))
			{
				productQuery.StartDate = new System.DateTime?(System.DateTime.Parse(context.Request.Form["startDate"]));
			}
			if (!string.IsNullOrEmpty(context.Request.Form["endDate"]))
			{
				productQuery.EndDate = new System.DateTime?(System.DateTime.Parse(context.Request.Form["endDate"]));
			}
			Globals.EntityCoding(productQuery, true);
			DbQueryResult toTaobaoProducts = ProductHelper.GetToTaobaoProducts(productQuery);
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			stringBuilder.Append("{");
			stringBuilder.Append("\"Products\":[");
			System.Data.DataTable dataTable = (System.Data.DataTable)toTaobaoProducts.Data;
			if (dataTable.Rows.Count > 0)
			{
				foreach (System.Data.DataRow dataRow in dataTable.Rows)
				{
					stringBuilder.Append("{");
					stringBuilder.AppendFormat("\"ProductId\":{0},", dataRow["ProductId"]);
					stringBuilder.AppendFormat("\"ProductDetailLink\":\"{0}\",", str + Globals.GetSiteUrls().UrlData.FormatUrl("productDetails", new object[]
					{
						dataRow["ProductId"]
					}));
					stringBuilder.AppendFormat("\"DisplaySequence\":{0},", dataRow["DisplaySequence"]);
					stringBuilder.AppendFormat("\"ThumbnailUrl40\":\"{0}\",", (dataRow["ThumbnailUrl40"] != System.DBNull.Value) ? (str + Globals.ApplicationPath + (string)dataRow["ThumbnailUrl40"]) : "");
					stringBuilder.AppendFormat("\"ProductName\":\"{0}\",", System.Web.HttpUtility.UrlEncode(dataRow["ProductName"].ToString()));
					stringBuilder.AppendFormat("\"ProductCode\":\"{0}\",", dataRow["ProductCode"]);
					stringBuilder.AppendFormat("\"Stock\":{0},", dataRow["Stock"]);
					stringBuilder.AppendFormat("\"MarketPrice\":{0},", (dataRow["MarketPrice"] != System.DBNull.Value) ? ((decimal)dataRow["MarketPrice"]).ToString("F2") : "0");
					stringBuilder.AppendFormat("\"SalePrice\":{0},", (dataRow["SalePrice"] != System.DBNull.Value) ? ((decimal)dataRow["SalePrice"]).ToString("F2") : "0");
					stringBuilder.AppendFormat("\"IsMakeTaobao\":{0},", (dataRow["IsMakeTaobao"] != System.DBNull.Value) ? ((int)dataRow["IsMakeTaobao"]) : 0);
					stringBuilder.AppendFormat("\"TaobaoProductId\":{0}", (dataRow["TaobaoProductId"] != System.DBNull.Value) ? ((long)dataRow["TaobaoProductId"]) : 0L);
					stringBuilder.Append("},");
				}
				stringBuilder.Remove(stringBuilder.Length - 1, 1);
			}
			stringBuilder.Append("],");
			stringBuilder.AppendFormat("\"TotalResults\":{0}", toTaobaoProducts.TotalRecords);
			stringBuilder.Append("}");
			context.Response.Write(stringBuilder.ToString());
		}
		private void ProcessProductDetails(System.Web.HttpContext context)
		{
			int productId = int.Parse(context.Request.Form["productId"]);
			System.Data.DataSet taobaoProductDetails = ProductHelper.GetTaobaoProductDetails(productId);
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			System.Data.DataTable dataTable = taobaoProductDetails.Tables[0];
			stringBuilder.Append("{");
			stringBuilder.AppendFormat("\"HasSKU\":\"{0}\",", dataTable.Rows[0]["HasSKU"]);
			stringBuilder.AppendFormat("\"ProductName\":\"{0}\",", System.Web.HttpUtility.UrlEncode(dataTable.Rows[0]["ProductName"].ToString()));
			stringBuilder.AppendFormat("\"ProductCode\":\"{0}\",", dataTable.Rows[0]["ProductCode"]);
			stringBuilder.AppendFormat("\"CategoryName\":\"{0}\",", dataTable.Rows[0]["CategoryName"]);
			stringBuilder.AppendFormat("\"BrandName\":\"{0}\",", dataTable.Rows[0]["BrandName"]);
			stringBuilder.AppendFormat("\"SalePrice\":\"{0}\",", dataTable.Rows[0]["SalePrice"]);
			stringBuilder.AppendFormat("\"MarketPrice\":\"{0}\",", (dataTable.Rows[0]["MarketPrice"] == System.DBNull.Value) ? "0.00" : dataTable.Rows[0]["MarketPrice"]);
			stringBuilder.AppendFormat("\"CostPrice\":\"{0}\",", (dataTable.Rows[0]["CostPrice"] == System.DBNull.Value) ? "0.00" : dataTable.Rows[0]["CostPrice"]);
			stringBuilder.AppendFormat("\"Stock\":\"{0}\",", dataTable.Rows[0]["Stock"]);
			stringBuilder.AppendFormat("\"Attributes\":\"{0}\",", this.GetProductAttribute(taobaoProductDetails.Tables[1]));
			stringBuilder.AppendFormat("\"Skus\":\"{0}\"", this.GetProductSKU(taobaoProductDetails.Tables[2]));
			System.Data.DataTable dataTable2 = taobaoProductDetails.Tables[3];
			if (dataTable2.Rows.Count > 0)
			{
				stringBuilder.AppendFormat(",\"Cid\":\"{0}\",", dataTable2.Rows[0]["Cid"]);
				stringBuilder.AppendFormat("\"StuffStatus\":\"{0}\",", dataTable2.Rows[0]["StuffStatus"]);
				stringBuilder.AppendFormat("\"ProTitle\":\"{0}\",", dataTable2.Rows[0]["ProTitle"]);
				stringBuilder.AppendFormat("\"Num\":\"{0}\",", dataTable2.Rows[0]["Num"]);
				stringBuilder.AppendFormat("\"LocationState\":\"{0}\",", dataTable2.Rows[0]["LocationState"]);
				stringBuilder.AppendFormat("\"LocationCity\":\"{0}\",", dataTable2.Rows[0]["LocationCity"]);
				stringBuilder.AppendFormat("\"FreightPayer\":\"{0}\",", dataTable2.Rows[0]["FreightPayer"]);
				stringBuilder.AppendFormat("\"PostFee\":\"{0}\",", dataTable2.Rows[0]["PostFee"]);
				stringBuilder.AppendFormat("\"ExpressFee\":\"{0}\",", dataTable2.Rows[0]["ExpressFee"]);
				stringBuilder.AppendFormat("\"EMSFee\":\"{0}\",", dataTable2.Rows[0]["EMSFee"]);
				stringBuilder.AppendFormat("\"HasInvoice\":\"{0}\",", dataTable2.Rows[0]["HasInvoice"]);
				stringBuilder.AppendFormat("\"HasWarranty\":\"{0}\",", dataTable2.Rows[0]["HasWarranty"]);
				stringBuilder.AppendFormat("\"HasDiscount\":\"{0}\",", dataTable2.Rows[0]["HasDiscount"]);
				stringBuilder.AppendFormat("\"FoodAttributes\":\"{0}\",", System.Web.HttpUtility.UrlEncode(dataTable2.Rows[0]["FoodAttributes"].ToString()));
				stringBuilder.AppendFormat("\"PropertyAlias\":\"{0}\",", dataTable2.Rows[0]["PropertyAlias"]);
				stringBuilder.AppendFormat("\"SkuProperties\":\"{0}\",", dataTable2.Rows[0]["SkuProperties"]);
				stringBuilder.AppendFormat("\"SkuQuantities\":\"{0}\",", dataTable2.Rows[0]["SkuQuantities"]);
				stringBuilder.AppendFormat("\"SkuPrices\":\"{0}\",", dataTable2.Rows[0]["SkuPrices"]);
				stringBuilder.AppendFormat("\"SkuOuterIds\":\"{0}\",", dataTable2.Rows[0]["SkuOuterIds"]);
				stringBuilder.AppendFormat("\"Inputpids\":\"{0}\",", dataTable2.Rows[0]["Inputpids"]);
				stringBuilder.AppendFormat("\"Inputstr\":\"{0}\"", dataTable2.Rows[0]["Inputstr"]);
			}
			stringBuilder.Append("}");
			context.Response.Write(stringBuilder.ToString());
		}
		private string GetProductAttribute(System.Data.DataTable productAttribute)
		{
			string text = string.Empty;
			if (productAttribute.Rows.Count > 0)
			{
				foreach (System.Data.DataRow dataRow in productAttribute.Rows)
				{
					object obj = text;
					text = string.Concat(new object[]
					{
						obj,
						dataRow["AttributeName"],
						":",
						dataRow["ValueStr"],
						";"
					});
				}
				text = text.Remove(text.Length - 1);
			}
			return text;
		}
		private string GetProductSKU(System.Data.DataTable productSKU)
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			System.Text.StringBuilder stringBuilder2 = new System.Text.StringBuilder();
			if (productSKU.Rows.Count > 0)
			{
				for (int i = productSKU.Columns.Count - 1; i >= 0; i--)
				{
					if (productSKU.Columns[i].ColumnName != "SkuId")
					{
						stringBuilder2.Append(productSKU.Columns[i].ColumnName).Append(";");
					}
				}
				foreach (System.Data.DataRow dataRow in productSKU.Rows)
				{
					for (int j = productSKU.Columns.Count - 1; j >= 0; j--)
					{
						if (productSKU.Columns[j].ColumnName != "SkuId")
						{
							stringBuilder.Append(dataRow[productSKU.Columns[j].ColumnName]).Append(";");
						}
					}
					stringBuilder.Remove(stringBuilder.Length - 1, 1).Append(",");
				}
				stringBuilder2.Remove(stringBuilder2.Length - 1, 1).Append(",").Append(stringBuilder.Remove(stringBuilder.Length - 1, 1));
			}
			return stringBuilder2.ToString();
		}
		private void ProcessTaobaoProductMake(System.Web.HttpContext context)
		{
			TaobaoProductInfo taobaoProduct = this.GetTaobaoProduct(context);
			taobaoProduct.ProductId = int.Parse(context.Request.Form["ProductId"]);
			taobaoProduct.ProTitle = context.Request.Form["ProTitle"];
			taobaoProduct.Num = long.Parse(context.Request.Form["Num"]);
			taobaoProduct.FoodAttributes = context.Request.Form["FoodAttributes"];
			bool flag = ProductHelper.UpdateToaobProduct(taobaoProduct);
			context.Response.Write(flag.ToString());
		}
		private void ProcessTaobaoProductDetails(System.Web.HttpContext context)
		{
			string str = "http://" + System.Web.HttpContext.Current.Request.Url.Host + ((System.Web.HttpContext.Current.Request.Url.Port == 80) ? "" : (":" + System.Web.HttpContext.Current.Request.Url.Port));
			int productId = int.Parse(context.Request.Form["productId"]);
			PublishToTaobaoProductInfo taobaoProduct = ProductHelper.GetTaobaoProduct(productId);
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			stringBuilder.Append("{");
			stringBuilder.AppendFormat("\"Cid\":\"{0}\",", taobaoProduct.Cid);
			stringBuilder.AppendFormat("\"StuffStatus\":\"{0}\",", taobaoProduct.StuffStatus);
			stringBuilder.AppendFormat("\"ProductId\":\"{0}\",", taobaoProduct.ProductId);
			stringBuilder.AppendFormat("\"ProTitle\":\"{0}\",", System.Web.HttpUtility.UrlEncode(taobaoProduct.ProTitle));
			stringBuilder.AppendFormat("\"Num\":\"{0}\",", taobaoProduct.Num);
			stringBuilder.AppendFormat("\"LocationState\":\"{0}\",", taobaoProduct.LocationState);
			stringBuilder.AppendFormat("\"LocationCity\":\"{0}\",", taobaoProduct.LocationCity);
			stringBuilder.AppendFormat("\"FreightPayer\":\"{0}\",", taobaoProduct.FreightPayer);
			stringBuilder.AppendFormat("\"PostFee\":\"{0}\",", taobaoProduct.PostFee.ToString("F2"));
			stringBuilder.AppendFormat("\"ExpressFee\":\"{0}\",", taobaoProduct.ExpressFee.ToString("F2"));
			stringBuilder.AppendFormat("\"EMSFee\":\"{0}\",", taobaoProduct.EMSFee.ToString("F2"));
			stringBuilder.AppendFormat("\"HasInvoice\":\"{0}\",", taobaoProduct.HasInvoice);
			stringBuilder.AppendFormat("\"HasWarranty\":\"{0}\",", taobaoProduct.HasWarranty);
			stringBuilder.AppendFormat("\"HasDiscount\":\"{0}\",", taobaoProduct.HasDiscount);
			stringBuilder.AppendFormat("\"ValidThru\":\"{0}\",", taobaoProduct.ValidThru);
			stringBuilder.AppendFormat("\"ListTime\":\"{0}\",", taobaoProduct.ListTime);
			stringBuilder.AppendFormat("\"PropertyAlias\":\"{0}\",", taobaoProduct.PropertyAlias);
			stringBuilder.AppendFormat("\"InputPids\":\"{0}\",", taobaoProduct.InputPids);
			stringBuilder.AppendFormat("\"InputStr\":\"{0}\",", taobaoProduct.InputStr);
			stringBuilder.AppendFormat("\"SkuProperties\":\"{0}\",", taobaoProduct.SkuProperties);
			stringBuilder.AppendFormat("\"SkuQuantities\":\"{0}\",", taobaoProduct.SkuQuantities);
			stringBuilder.AppendFormat("\"SkuPrices\":\"{0}\",", taobaoProduct.SkuPrices);
			stringBuilder.AppendFormat("\"SkuOuterIds\":\"{0}\",", taobaoProduct.SkuOuterIds);
			stringBuilder.AppendFormat("\"TaobaoProductId\":\"{0}\",", taobaoProduct.TaobaoProductId);
			stringBuilder.AppendFormat("\"ProductCode\":\"{0}\",", taobaoProduct.ProductCode);
			if (string.IsNullOrEmpty(taobaoProduct.Description))
			{
				taobaoProduct.Description = "暂无描述信息";
			}
			stringBuilder.AppendFormat("\"Description\":\"{0}\",", System.Web.HttpUtility.UrlEncode(taobaoProduct.Description.Replace(string.Format("src=\"{0}/Storage/master/gallery", Globals.ApplicationPath), string.Format("src=\"{0}/Storage/master/gallery", str + Globals.ApplicationPath)).Replace('"', '“')));
			string text = str + Globals.ApplicationPath + taobaoProduct.ImageUrl1;
			if (System.IO.File.Exists(Globals.MapPath(Globals.ApplicationPath + taobaoProduct.ImageUrl1)) && (text.EndsWith(".jpg", System.StringComparison.CurrentCultureIgnoreCase) || text.EndsWith(".gif", System.StringComparison.CurrentCultureIgnoreCase) || text.EndsWith(".png", System.StringComparison.CurrentCultureIgnoreCase) || text.EndsWith(".jpeg", System.StringComparison.CurrentCultureIgnoreCase)))
			{
				stringBuilder.AppendFormat("\"ImageName1\":\"{0}\",", text);
			}
			string text2 = str + Globals.ApplicationPath + taobaoProduct.ImageUrl2;
			if (System.IO.File.Exists(Globals.MapPath(Globals.ApplicationPath + taobaoProduct.ImageUrl2)) && (text2.EndsWith(".jpg", System.StringComparison.CurrentCultureIgnoreCase) || text2.EndsWith(".gif", System.StringComparison.CurrentCultureIgnoreCase) || text2.EndsWith(".png", System.StringComparison.CurrentCultureIgnoreCase) || text2.EndsWith(".jpeg", System.StringComparison.CurrentCultureIgnoreCase)))
			{
				stringBuilder.AppendFormat("\"ImageName2\":\"{0}\",", text2);
			}
			string text3 = str + Globals.ApplicationPath + taobaoProduct.ImageUrl3;
			if (System.IO.File.Exists(Globals.MapPath(Globals.ApplicationPath + taobaoProduct.ImageUrl3)) && (text3.EndsWith(".jpg", System.StringComparison.CurrentCultureIgnoreCase) || text3.EndsWith(".gif", System.StringComparison.CurrentCultureIgnoreCase) || text3.EndsWith(".png", System.StringComparison.CurrentCultureIgnoreCase) || text3.EndsWith(".jpeg", System.StringComparison.CurrentCultureIgnoreCase)))
			{
				stringBuilder.AppendFormat("\"ImageName3\":\"{0}\",", text3);
			}
			string text4 = str + Globals.ApplicationPath + taobaoProduct.ImageUrl4;
			if (System.IO.File.Exists(Globals.MapPath(Globals.ApplicationPath + taobaoProduct.ImageUrl4)) && (text4.EndsWith(".jpg", System.StringComparison.CurrentCultureIgnoreCase) || text4.EndsWith(".gif", System.StringComparison.CurrentCultureIgnoreCase) || text4.EndsWith(".png", System.StringComparison.CurrentCultureIgnoreCase) || text4.EndsWith(".jpeg", System.StringComparison.CurrentCultureIgnoreCase)))
			{
				stringBuilder.AppendFormat("\"ImageName4\":\"{0}\",", text4);
			}
			string text5 = str + Globals.ApplicationPath + taobaoProduct.ImageUrl5;
			if (System.IO.File.Exists(Globals.MapPath(Globals.ApplicationPath + taobaoProduct.ImageUrl5)) && (text5.EndsWith(".jpg", System.StringComparison.CurrentCultureIgnoreCase) || text5.EndsWith(".gif", System.StringComparison.CurrentCultureIgnoreCase) || text5.EndsWith(".png", System.StringComparison.CurrentCultureIgnoreCase) || text5.EndsWith(".jpeg", System.StringComparison.CurrentCultureIgnoreCase)))
			{
				stringBuilder.AppendFormat("\"ImageName5\":\"{0}\",", text5);
			}
			stringBuilder.AppendFormat("\"SalePrice\":\"{0}\",", taobaoProduct.SalePrice.ToString("F2"));
			stringBuilder.AppendFormat("\"FoodAttributes\":\"{0}\"", System.Web.HttpUtility.UrlEncode(taobaoProduct.FoodAttributes));
			stringBuilder.Append("}");
			context.Response.Write(stringBuilder.ToString());
		}
		private void ProcessTaobaoProductIdAdd(System.Web.HttpContext context)
		{
			int productId = int.Parse(context.Request.Form["productId"]);
			long taobaoProductId = long.Parse(context.Request.Form["taobaoProductId"]);
			bool flag = ProductHelper.UpdateTaobaoProductId(productId, taobaoProductId);
			context.Response.Write(flag.ToString());
		}
		private void ProcessTaobaoProductIsExit(System.Web.HttpContext context)
		{
			long taobaoProductId = long.Parse(context.Request.Form["taobaoProductId"]);
			bool flag = ProductHelper.IsExitTaobaoProduct(taobaoProductId);
			context.Response.Write(flag.ToString());
		}
		private void ProcessTaobaoProductDown(System.Web.HttpContext context)
		{
			ProductInfo productInfo = new ProductInfo();
			productInfo.CategoryId = 0;
			productInfo.BrandId = new int?(0);
			productInfo.ProductName = System.Web.HttpUtility.UrlDecode(context.Request.Form["ProductName"]);
			productInfo.ProductCode = context.Request.Form["ProductCode"];
			productInfo.Description = System.Web.HttpUtility.UrlDecode(context.Request.Form["Description"]);
			if (context.Request.Form["SaleStatus"] == "onsale")
			{
				productInfo.SaleStatus = ProductSaleStatus.OnSale;
			}
			else
			{
				productInfo.SaleStatus = ProductSaleStatus.OnStock;
			}
			productInfo.AddedDate = System.DateTime.Parse(context.Request.Form["AddedDate"]);
			productInfo.TaobaoProductId = long.Parse(context.Request.Form["TaobaoProductId"]);
			string text = context.Request.Form["ImageUrls"];
			if (!string.IsNullOrEmpty(text))
			{
				this.DownloadImage(productInfo, text, context);
			}
			productInfo.TypeId = new int?(ProductTypeHelper.GetTypeId(context.Request.Form["TypeName"]));
			decimal weight = decimal.Parse(context.Request.Form["Weight"]);
			System.Collections.Generic.Dictionary<string, SKUItem> skus = this.GetSkus(productInfo, weight, context);
			ProductActionStatus productActionStatus = ProductHelper.AddProduct(productInfo, skus, null, null);
			if (productActionStatus == ProductActionStatus.Success)
			{
				TaobaoProductInfo taobaoProduct = this.GetTaobaoProduct(context);
				taobaoProduct.ProductId = productInfo.ProductId;
				taobaoProduct.ProTitle = productInfo.ProductName;
				taobaoProduct.Num = (long)productInfo.Stock;
				ProductHelper.UpdateToaobProduct(taobaoProduct);
			}
			context.Response.ContentType = "text/string";
			context.Response.Write(productActionStatus.ToString());
		}
		private void DownloadImage(ProductInfo product, string imageUrls, System.Web.HttpContext context)
		{
			imageUrls = System.Web.HttpUtility.UrlDecode(imageUrls);
			string[] array = imageUrls.Split(new char[]
			{
				';'
			});
			int num = 1;
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string text = array2[i];
				string text2 = string.Format("/Storage/master/product/images/{0}", System.Guid.NewGuid().ToString("N", System.Globalization.CultureInfo.InvariantCulture) + text.Substring(text.LastIndexOf('.')));
				string text3 = text2.Replace("/images/", "/thumbs40/40_");
				string text4 = text2.Replace("/images/", "/thumbs60/60_");
				string text5 = text2.Replace("/images/", "/thumbs100/100_");
				string text6 = text2.Replace("/images/", "/thumbs160/160_");
				string text7 = text2.Replace("/images/", "/thumbs180/180_");
				string text8 = text2.Replace("/images/", "/thumbs220/220_");
				string text9 = text2.Replace("/images/", "/thumbs310/310_");
				string text10 = text2.Replace("/images/", "/thumbs410/410_");
				string text11 = System.Web.HttpContext.Current.Request.MapPath(Globals.ApplicationPath + text2);
				System.Net.WebClient webClient = new System.Net.WebClient();
				try
				{
					webClient.DownloadFile(text, text11);
					ResourcesHelper.CreateThumbnail(text11, context.Request.MapPath(Globals.ApplicationPath + text3), 40, 40);
					ResourcesHelper.CreateThumbnail(text11, context.Request.MapPath(Globals.ApplicationPath + text4), 60, 60);
					ResourcesHelper.CreateThumbnail(text11, context.Request.MapPath(Globals.ApplicationPath + text5), 100, 100);
					ResourcesHelper.CreateThumbnail(text11, context.Request.MapPath(Globals.ApplicationPath + text6), 160, 160);
					ResourcesHelper.CreateThumbnail(text11, context.Request.MapPath(Globals.ApplicationPath + text7), 180, 180);
					ResourcesHelper.CreateThumbnail(text11, context.Request.MapPath(Globals.ApplicationPath + text8), 220, 220);
					ResourcesHelper.CreateThumbnail(text11, context.Request.MapPath(Globals.ApplicationPath + text9), 310, 310);
					ResourcesHelper.CreateThumbnail(text11, context.Request.MapPath(Globals.ApplicationPath + text10), 410, 410);
					switch (num)
					{
					case 1:
						product.ImageUrl1 = text2;
						product.ThumbnailUrl40 = text3;
						product.ThumbnailUrl60 = text4;
						product.ThumbnailUrl100 = text5;
						product.ThumbnailUrl160 = text6;
						product.ThumbnailUrl180 = text7;
						product.ThumbnailUrl220 = text8;
						product.ThumbnailUrl310 = text9;
						product.ThumbnailUrl410 = text10;
						break;
					case 2:
						product.ImageUrl2 = text2;
						break;
					case 3:
						product.ImageUrl3 = text2;
						break;
					case 4:
						product.ImageUrl4 = text2;
						break;
					case 5:
						product.ImageUrl5 = text2;
						break;
					}
					num++;
				}
				catch
				{
				}
			}
		}
		private System.Collections.Generic.Dictionary<string, SKUItem> GetSkus(ProductInfo product, decimal weight, System.Web.HttpContext context)
		{
			string text = context.Request.Form["SkuString"];
			System.Collections.Generic.Dictionary<string, SKUItem> dictionary;
			if (string.IsNullOrEmpty(text))
			{
				product.HasSKU = false;
				dictionary = new System.Collections.Generic.Dictionary<string, SKUItem>
				{

					{
						"0",
						new SKUItem
						{
							SkuId = "0",
							SKU = product.ProductCode,
							SalePrice = decimal.Parse(context.Request.Form["SalePrice"]),
							CostPrice = 0m,
							Stock = int.Parse(context.Request.Form["Stock"]),
							Weight = weight
						}
					}
				};
			}
			else
			{
				product.HasSKU = true;
				dictionary = new System.Collections.Generic.Dictionary<string, SKUItem>();
				text = System.Web.HttpUtility.UrlDecode(text);
				string[] array = text.Split(new char[]
				{
					'|'
				});
				for (int i = 0; i < array.Length; i++)
				{
					string text2 = array[i];
					string[] array2 = text2.Split(new char[]
					{
						','
					});
					SKUItem sKUItem = new SKUItem();
					sKUItem.SKU = array2[0].Replace("_", "-");
					sKUItem.Weight = weight;
					sKUItem.Stock = int.Parse(array2[1]);
					sKUItem.SalePrice = decimal.Parse(array2[2]);
					string text3 = array2[3];
					string text4 = "";
					string[] array3 = text3.Split(new char[]
					{
						';'
					});
					for (int j = 0; j < array3.Length; j++)
					{
						string text5 = array3[j];
						string[] array4 = text5.Split(new char[]
						{
							':'
						});
						int specificationId = ProductTypeHelper.GetSpecificationId(product.TypeId.Value, array4[0]);
						int specificationValueId = ProductTypeHelper.GetSpecificationValueId(specificationId, array4[1].Replace("\\", "/"));
						text4 = text4 + specificationValueId + "_";
						sKUItem.SkuItems.Add(specificationId, specificationValueId);
					}
					sKUItem.SkuId = text4.Substring(0, text4.Length - 1);
					dictionary.Add(sKUItem.SkuId, sKUItem);
				}
			}
			return dictionary;
		}
		private TaobaoProductInfo GetTaobaoProduct(System.Web.HttpContext context)
		{
			TaobaoProductInfo taobaoProductInfo = new TaobaoProductInfo();
			taobaoProductInfo.Cid = long.Parse(context.Request.Form["Cid"]);
			taobaoProductInfo.StuffStatus = context.Request.Form["StuffStatus"];
			taobaoProductInfo.LocationState = context.Request.Form["LocationState"];
			taobaoProductInfo.LocationCity = context.Request.Form["LocationCity"];
			taobaoProductInfo.FreightPayer = context.Request.Form["FreightPayer"];
			if (!string.IsNullOrEmpty(context.Request.Form["PostFee"]))
			{
				taobaoProductInfo.PostFee = decimal.Parse(context.Request.Form["PostFee"]);
			}
			if (!string.IsNullOrEmpty(context.Request.Form["ExpressFee"]))
			{
				taobaoProductInfo.ExpressFee = decimal.Parse(context.Request.Form["ExpressFee"]);
			}
			if (!string.IsNullOrEmpty(context.Request.Form["EMSFee"]))
			{
				taobaoProductInfo.EMSFee = decimal.Parse(context.Request.Form["EMSFee"]);
			}
			taobaoProductInfo.HasInvoice = bool.Parse(context.Request.Form["HasInvoice"]);
			taobaoProductInfo.HasWarranty = bool.Parse(context.Request.Form["HasWarranty"]);
			taobaoProductInfo.HasDiscount = false;
			taobaoProductInfo.ListTime = System.DateTime.Now;
			taobaoProductInfo.PropertyAlias = context.Request.Form["PropertyAlias"];
			taobaoProductInfo.InputPids = context.Request.Form["InputPids"];
			taobaoProductInfo.InputStr = context.Request.Form["InputStr"];
			taobaoProductInfo.SkuProperties = context.Request.Form["SkuProperties"];
			taobaoProductInfo.SkuQuantities = context.Request.Form["SkuQuantities"];
			taobaoProductInfo.SkuPrices = context.Request.Form["SkuPrices"];
			taobaoProductInfo.SkuOuterIds = context.Request.Form["SkuOuterIds"];
			return taobaoProductInfo;
		}
	}
}
