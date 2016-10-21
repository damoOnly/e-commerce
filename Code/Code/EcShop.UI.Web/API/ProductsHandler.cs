using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Xml;

using Newtonsoft.Json;

using EcShop.ControlPanel.Commodities;
using EcShop.Core;
using EcShop.Entities.Commodities;
using EcShop.Membership.Context;

namespace EcShop.UI.Web.API
{
	[WebService(Namespace = "http://tempuri.org/"), WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class ProductsHandler : System.Web.IHttpHandler
	{
		private SiteSettings siteSettings;
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		public void ProcessRequest(System.Web.HttpContext context)
		{
			string content = "";
			string message = "";
			string action = context.Request.Form["action"].Trim();
			string sign = context.Request.Form["sign"].Trim();
			this.siteSettings = SettingsManager.GetMasterSettings(false);
			string checkCode = this.siteSettings.CheckCode;
			SortedDictionary<string, string> sortedDictionary = new SortedDictionary<string, string>();
			
			string skuContentFormat = "{{\"lid\":\"{0}\",\"OuterId\":\"{1}\",\"SkuId\":\"{2}\",\"SKUContent\":\"{3}\",\"Nums\":\"{4}\",\"Price\":{5},\"CostPrice\":{6}}}";
			string productFormat = "{{\"lid\":\"{0}\",\"OuterId\":\"{1}\",\"Title\":\"{2}\",\"PicUrl\":\"{3}\",\"Url\":\"{4}\",\"MarketPrice\":{5},\"Price\":{6},\"CostPrice\":{7},\"Weight\":{8},\"Status\":{9},\"SaleCounts\":{10},\"ShortDescription:\":\"{11}\",\"Stock\":{12},\"AddedDate\":\"{13}\",\"Description\":\"{14}\",\"RankPrice\":{15},\"List\":\"{16}\",\"BrandId\":\"{17}\",\"SkuItems\":[{18}],\"Unit\":\"{19}\"}}";
			
			string arg_A3_0 = context.Request.Form["format"];

			System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();

			try
			{
				if (!string.IsNullOrEmpty(action))
				{
					string hostPath = HiContext.Current.HostPath;
					string a;
					if ((a = action) != null)
					{
						if (!(a == "productview"))
						{
							if (!(a == "stockview"))
							{
								if (!(a == "quantity"))
								{
									if (a == "statue")
									{
										string state = context.Request.Form["state"].Trim();
										string productId = context.Request.Form["productId"].Trim();
										if (string.IsNullOrEmpty(state) || string.IsNullOrEmpty(productId))
										{
											message = MessageInfo.ShowMessageInfo(ApiErrorCode.Empty_Error, "state or productId");
											goto IL_78C;
										}
										ProductSaleStatus productSaleStatus = (ProductSaleStatus)System.Enum.Parse(typeof(ProductSaleStatus), state, true);
										int num = System.Convert.ToInt32(productId);
										if (num <= 0)
										{
											message = MessageInfo.ShowMessageInfo(ApiErrorCode.Format_Eroor, "productId");
											goto IL_78C;
										}
										sortedDictionary.Add("productid", productId);
										sortedDictionary.Add("state", state);
										sortedDictionary.Add("format", "json");
										if (!APIHelper.CheckSign(sortedDictionary, checkCode, sign))
										{
											message = MessageInfo.ShowMessageInfo(ApiErrorCode.Signature_Error, "sign");
											goto IL_78C;
										}
										bool flag = false;
										if (productSaleStatus == ProductSaleStatus.OnSale)
										{
											if (ProductHelper.UpShelfAPI(num.ToString()) > 0)
											{
												flag = true;
											}
											else
											{
												message = MessageInfo.ShowMessageInfo(ApiErrorCode.Format_Eroor, "productId");
											}
										}
										else
										{
											if (productSaleStatus == ProductSaleStatus.UnSale)
											{
												if (ProductHelper.OffShelfAPI(num.ToString()) > 0)
												{
													flag = true;
												}
												else
												{
													message = MessageInfo.ShowMessageInfo(ApiErrorCode.Format_Eroor, "productId");
												}
											}
										}
										if (flag)
										{
											content = string.Concat(new string[]
											{
												"{\"item_update_statue_response\":\"item\":{\"num_iid\":\"",
												productId,
												"\",\"modified\":\"",
												DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),
												"\"}}"
											});
											goto IL_78C;
										}
										message = MessageInfo.ShowMessageInfo(ApiErrorCode.Unknown_Error, "update");
										goto IL_78C;
									}
								}
								else
								{
									if (string.IsNullOrEmpty(context.Request.Form["productId"].Trim()) || string.IsNullOrEmpty(context.Request.Form["quantity"].Trim()))
									{
										message = MessageInfo.ShowMessageInfo(ApiErrorCode.Empty_Error, "paramters");
										goto IL_78C;
									}
									string text6 = context.Request.Form["productId"];
									string text7 = "";
									string text8 = "";
									int type = 1;
									int stock = System.Convert.ToInt32(context.Request.Form["quantity"].Trim());
									if (!string.IsNullOrEmpty(context.Request.Form["sku_id"].Trim()))
									{
										text7 = context.Request.Form["sku_id"];
									}
									if (!string.IsNullOrEmpty(context.Request.Form["outer_id"].Trim()))
									{
										text8 = context.Request.Form["outer_id"];
									}
									if (!string.IsNullOrEmpty(context.Request.Form["type"]))
									{
										type = System.Convert.ToInt32(context.Request.Form["type"]);
									}
									sortedDictionary.Add("productId", text6.ToString());
									sortedDictionary.Add("quantity", stock.ToString());
									sortedDictionary.Add("sku_id", text7);
									sortedDictionary.Add("outer_id", text8);
									sortedDictionary.Add("type", type.ToString());
									sortedDictionary.Add("format", "json");
									sortedDictionary.Add("action", action);
									if (!APIHelper.CheckSign(sortedDictionary, checkCode, sign))
									{
										message = MessageInfo.ShowMessageInfo(ApiErrorCode.Signature_Error, "sign");
										goto IL_78C;
									}
									ApiErrorCode apiErrorCode = ProductHelper.UpdateProductStock(System.Convert.ToInt32(text6), text7, text8, type, stock);
									if (ApiErrorCode.Success == apiErrorCode)
									{
										string format = "{{\"trade_get_response\":{{\"product\":{{{0}}}}}}}";
										content = string.Format(format, this.GetProductDetailsView(System.Convert.ToInt32(text6), 0, hostPath, productFormat, skuContentFormat).ToString());
										goto IL_78C;
									}
									message = MessageInfo.ShowMessageInfo(apiErrorCode, "paramters");
									goto IL_78C;
								}
							}
							else
							{
								if (string.IsNullOrEmpty(context.Request.Form["productId"].Trim()) || System.Convert.ToInt32(context.Request.Form["productId"].Trim()) <= 0)
								{
									message = MessageInfo.ShowMessageInfo(ApiErrorCode.Empty_Error, "productId");
									goto IL_78C;
								}
								int num = System.Convert.ToInt32(context.Request.Form["productId"].Trim());
								string text9 = "0";
								if (!string.IsNullOrEmpty(context.Request.Form["gradeId"].Trim()) && System.Convert.ToInt16(context.Request.Form["gradeId"].Trim()) > 0)
								{
									text9 = context.Request.Form["gradeId"].Trim();
								}
								sortedDictionary.Add("productid", num.ToString());
								sortedDictionary.Add("action", "stockview");
								sortedDictionary.Add("format", "json");
								sortedDictionary.Add("gradeId", text9);
								if (APIHelper.CheckSign(sortedDictionary, checkCode, sign))
								{
									string format2 = "{{\"trade_get_response\":{{\"product\":{{{0}}}}}}}";
									content = string.Format(format2, this.GetProductDetailsView(num, int.Parse(text9), hostPath, productFormat, skuContentFormat).ToString());
									goto IL_78C;
								}
								message = MessageInfo.ShowMessageInfo(ApiErrorCode.Signature_Error, "sign");
								goto IL_78C;
							}
						}
						else
						{
							string value2 = context.Request.Form["parma"].Trim();
							string text9 = context.Request.Form["gradeId"].Trim();
							ProductQuery productQuery = new ProductQuery();
							if (!string.IsNullOrEmpty(value2))
							{
								productQuery = JsonConvert.DeserializeObject<ProductQuery>(value2);
							}
							message = MessageInfo.ShowMessageInfo(ApiErrorCode.Empty_Error, "sign");
							if (string.IsNullOrEmpty(sign))
							{
								goto IL_78C;
							}
							sortedDictionary.Add("parma", context.Request.Form["parma"]);
							sortedDictionary.Add("format", "json");
							sortedDictionary.Add("action", "productview");
							sortedDictionary.Add("gradeId", text9.ToString());
							message = MessageInfo.ShowMessageInfo(ApiErrorCode.Signature_Error, "sign");
							if (APIHelper.CheckSign(sortedDictionary, checkCode, sign))
							{
								Globals.EntityCoding(productQuery, true);
								int num2 = 0;
								string format3 = "{{\"trade_get_response\":{{\"product\":[{0}],\"totalrecord\":\"{1}\"}}}}";
								content = string.Format(format3, this.GetProductView(productQuery, int.Parse(text9), hostPath, productFormat, skuContentFormat, out num2).ToString(), num2.ToString());
								goto IL_78C;
							}
							goto IL_78C;
						}
					}
					message = MessageInfo.ShowMessageInfo(ApiErrorCode.Paramter_Error, "paramters");
				}
				else
				{
					message = MessageInfo.ShowMessageInfo(ApiErrorCode.Empty_Error, "modeId");
				}
				IL_78C:;
			}
			catch (System.Exception ex)
			{
				message = MessageInfo.ShowMessageInfo(ApiErrorCode.Unknown_Error, ex.Message);
			}

			if (string.IsNullOrEmpty(content))
			{
				content = message;
				xmlDocument.Load(new System.IO.MemoryStream(Encoding.GetEncoding("UTF-8").GetBytes(content)));
				content = JsonConvert.SerializeXmlNode(xmlDocument);
			}

			context.Response.ContentType = "text/json";
			context.Response.Write(content);
		}

		public StringBuilder GetProductView(ProductQuery query, int gradeId, string host, string strformat, string skuContentFormat, out int recordes)
		{
			if (query.CategoryId.HasValue)
			{
				query.MaiCategoryPath = CatalogHelper.GetCategory(query.CategoryId.Value).Path;
			}

			StringBuilder sbProducts = new StringBuilder();
			int recordCount = 0;
			DataSet productsByQuery = ProductHelper.GetProductsByQuery(query, gradeId, out recordCount);
			foreach (DataRow rowProduct in productsByQuery.Tables[0].Rows)
			{
				StringBuilder sbProductRel = new StringBuilder();
				string existRelationProduct = "false";
				DataRow[] childRows = rowProduct.GetChildRows("ProductRealation");
				DataRow[] array = childRows;
				for (int i = 0; i < array.Length; i++)
				{
					DataRow rowProductRel = array[i];
					existRelationProduct = "true";
					string skuContent = MessageInfo.GetSkuContent(rowProductRel["SkuId"].ToString());
					string salePriceRel = "";
					string costPriceRel = "";
					if (rowProductRel["SalePrice"] != null && rowProductRel["SalePrice"].ToString() != "")
					{
						salePriceRel = decimal.Parse(rowProductRel["SalePrice"].ToString()).ToString("F2");
					}

					if (rowProductRel["CostPrice"] != null && rowProductRel["CostPrice"].ToString() != "")
					{
						costPriceRel = decimal.Parse(rowProductRel["CostPrice"].ToString()).ToString("F2");
					}

					sbProductRel.AppendFormat(skuContentFormat, new object[]
					{
						rowProductRel["ProductId"].ToString(),
						rowProductRel["SKU"].ToString(),
						rowProductRel["SKuId"].ToString(),
						skuContent,
						rowProductRel["Stock"].ToString(),
						salePriceRel,
						costPriceRel
					});
					sbProductRel.Append(",");
				}
				if (sbProductRel.Length > 0)
				{
					sbProductRel = sbProductRel.Remove(sbProductRel.Length - 1, 1);
				}
				string detailsUrl = host + "/AppShop/ProductDetails.aspx?productId=" + rowProduct["ProductId"];
				string imageUrl60 = host + rowProduct["ThumbnailUrl60"];
				if (string.IsNullOrEmpty(rowProduct["ThumbnailUrl60"].ToString()))
				{
					imageUrl60 = host + this.siteSettings.DefaultProductThumbnail2;
				}
				if (rowProduct["ThumbnailUrl60"].ToString().IndexOf("http://image58.kuaidiantong.cn/Storage/master/product/") >= 0)
				{
					imageUrl60 = rowProduct["ThumbnailUrl60"].ToString();
				}
				string marketPrice = "";
				string salePrice = "";
				string rankPrice = "";
				string costPrice = "";
				string shotDescription = System.Convert.ToBase64String(Encoding.UTF8.GetBytes(rowProduct["ShortDescription"].ToString()));

				if (rowProduct["MarketPrice"] != null && rowProduct["MarketPrice"].ToString() != "")
				{
					marketPrice = decimal.Parse(rowProduct["MarketPrice"].ToString()).ToString("F2");
				}
				if (rowProduct["SalePrice"] != null && rowProduct["SalePrice"].ToString() != "")
				{
					salePrice = decimal.Parse(rowProduct["SalePrice"].ToString()).ToString("F2");
				}
				if (rowProduct["CostPrice"] != null && rowProduct["CostPrice"].ToString() != "")
				{
					costPrice = decimal.Parse(rowProduct["CostPrice"].ToString()).ToString("F2");
				}
				if (rowProduct["RankPrice"] != null && rowProduct["RankPrice"].ToString() != "")
				{
					rankPrice = decimal.Parse(rowProduct["RankPrice"].ToString()).ToString("F2");
				}

				sbProducts.AppendFormat(strformat, new object[]
				{
					rowProduct["ProductId"].ToString(),
					rowProduct["ProductCode"].ToString(),
					rowProduct["ProductName"].ToString(),
					imageUrl60,
					detailsUrl,
					marketPrice,
					salePrice,
					costPrice,
					rowProduct["Weight"].ToString(),
					rowProduct["SaleStatus"].ToString(),
					rowProduct["SaleCounts"].ToString(),
					shotDescription,
					rowProduct["Stock"].ToString(),
					rowProduct["AddedDate"].ToString(),
					MessageInfo.GetDesciption(rowProduct["Description"].ToString(), host),
					rankPrice,
					existRelationProduct,
					rowProduct["BrandId"].ToString(),
					sbProductRel,
					rowProduct["Unit"].ToString()
				});

				sbProducts.Append(",");
			}

			recordes = recordCount;

			if (sbProducts.Length > 0)
			{
				sbProducts = sbProducts.Remove(sbProducts.Length - 1, 1);
			}
			return sbProducts;
		}

		public string ImageUrl(string host, string[] images)
		{
			string url = "";
			for (int i = 0; i < images.Length; i++)
			{
				if (images[i].ToString().IndexOf("http://images.net.92hidc.com/Storage/master/product/") >= 0)
				{
					host = "";
				}
				if (images[i].ToString() != "")
				{
					url = url + host + images[i].ToString() + "|";
				}
			}
			if (url != "")
			{
				url = url.Substring(0, url.Length - 1);
			}
			return url;
		}

		public StringBuilder GetProductDetailsView(int pid, int gradeId, string host, string format, string skuContentFormat)
		{
			StringBuilder sbProduct = new StringBuilder();
			
            string existSkuDetials = "false";
			string skuList = "";

			DataSet productSkuDetials = ProductHelper.GetProductSkuDetials(pid, gradeId);
			foreach (DataRow skuRow in productSkuDetials.Tables[1].Rows)
			{
				existSkuDetials = "true";
				string skuContent = MessageInfo.GetSkuContent(skuRow["SkuId"].ToString());
				string skuSalePrice = "";
				string skuCostPrice = "";
				if (skuRow["SalePrice"] != null && skuRow["SalePrice"].ToString() != "")
				{
					skuSalePrice = decimal.Parse(skuRow["SalePrice"].ToString()).ToString("F2");
				}
				if (skuRow["CostPrice"] != null && skuRow["CostPrice"].ToString() != "")
				{
					skuCostPrice = decimal.Parse(skuRow["CostPrice"].ToString()).ToString("F2");
				}
				skuList = skuList + string.Format(skuContentFormat, new object[]
				{
					skuRow["ProductId"].ToString(),
					skuRow["SKU"].ToString(),
					skuRow["SkuId"],
					skuContent,
					skuRow["Stock"],
					skuSalePrice,
					skuCostPrice
				}) + ",";
			}

			if (skuList.Length > 0)
			{
				skuList = skuList.Substring(0, skuList.Length - 1);
			}

			JsonConvert.SerializeObject(productSkuDetials.Tables[0]);

			foreach (DataRow productRow in productSkuDetials.Tables[0].Rows)
			{
				string productDetailsUrl = host + Globals.GetSiteUrls().UrlData.FormatUrl("productDetails", new object[]
				{
					productRow["ProductId"].ToString()
				});
				string[] strimg = new string[]
				{
					productRow["ImageUrl1"].ToString(),
					productRow["ImageUrl2"].ToString(),
					productRow["ImageUrl3"].ToString(),
					productRow["ImageUrl4"].ToString(),
					productRow["ImageUrl5"].ToString()
				};
				string desciption = MessageInfo.GetDesciption(productRow["Description"].ToString(), host);
				string shortDescription = System.Convert.ToBase64String(Encoding.UTF8.GetBytes(productRow["ShortDescription"].ToString()));
				string imageUrl = this.ImageUrl(host, strimg);
				string salePrice = "";
				string marketPrice = "";
				string costPrice = "";
				if (productRow["SalePrice"] != null && productRow["SalePrice"].ToString() != "")
				{
					salePrice = decimal.Parse(productRow["SalePrice"].ToString()).ToString("F2");
				}
				if (productRow["CostPrice"] != null && productRow["CostPrice"].ToString() != "")
				{
					costPrice = decimal.Parse(productRow["SalePrice"].ToString()).ToString("F2");
				}
				if (productRow["MarketPrice"] != null && productRow["MarketPrice"].ToString() != "")
				{
					marketPrice = decimal.Parse(productRow["MarketPrice"].ToString()).ToString("F2");
				}
				format = format.Replace("{{", "").Replace("}}", "");
				sbProduct.AppendFormat(format, new object[]
				{
					productRow["ProductId"].ToString(),
					productRow["ProductCode"].ToString(),
					productRow["ProductName"].ToString(),
					imageUrl,
					productDetailsUrl,
					marketPrice,
					salePrice,
					costPrice,
					productRow["Weight"].ToString(),
					productRow["SaleStatus"].ToString(),
					productRow["SaleCounts"].ToString(),
					shortDescription,
					productRow["Stock"].ToString(),
					productRow["AddedDate"].ToString(),
					desciption,
					salePrice,
					existSkuDetials,
					productRow["BrandId"].ToString(),
					skuList,
					productRow["Unit"].ToString()
				});
			}
			return sbProduct;
		}
	}
}
