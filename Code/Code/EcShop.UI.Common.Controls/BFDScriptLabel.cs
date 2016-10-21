using EcShop.Core;
using EcShop.Membership.Context;
using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.Common.Controls
{
	public class BFDScriptLabel : Literal
	{
		private string client;
		protected override void Render(HtmlTextWriter writer)
		{
			SiteSettings siteSettings = HiContext.Current.SiteSettings;
			if (siteSettings != null && siteSettings.EnabledBFD)
			{
				this.client = "C" + siteSettings.BFDUserName;
				string text = HttpContext.Current.Request.Url.ToString();
				base.Text = this.ScriptByUrl(text.ToLower());
			}
			base.Render(writer);
		}
		protected string ScriptByUrl(string url)
		{
			string text = "<script  type='text/javascript' src='/Utility/BFD.js'></script>";
			string text2 = this.client;
			string text3 = HiContext.Current.User.UserId.ToString();
			if (url.IndexOf("userdefault") >= 0)
			{
				string str = string.Concat(new string[]
				{
					"{user_id :\"",
					text3,
					"\" , client :\"",
					text2,
					"\"  }"
				});
				text = text + "<script type='text/javascript'>BFD(" + str + ",'usercenter')</script>";
			}
			else
			{
				if (url.IndexOf("default") >= 0)
				{
					string str2 = string.Concat(new string[]
					{
						"{user_id :\"",
						text3,
						"\" , client :\"",
						text2,
						"\"  }"
					});
					text = text + "<script type='text/javascript'>BFD(" + str2 + ",'default')</script>";
				}
				else
				{
					if (url.IndexOf("unproductdetails") >= 0)
					{
						string arg = HttpContext.Current.Request.QueryString["productId"];
						StringBuilder stringBuilder = new StringBuilder();
						stringBuilder.Append("{");
						stringBuilder.AppendFormat("id:\"{0}\",", arg);
						stringBuilder.AppendFormat("sku_id:{0},", "$(\"#bfdProductCode\").text()");
						stringBuilder.AppendFormat("name:{0},", "$(\"#bfdProductTitle\").text()");
						stringBuilder.AppendFormat("item_link:{0},", "location.href");
						stringBuilder.AppendFormat("image_link:{0},", "location.origin+$(\"#ProductDetails_common_ProductImages___iptPicUrl1\").val()");
						stringBuilder.AppendFormat("price:{0},", "changeTwoDecimal($(\"#UnProductDetails_lblBuyPrice\").text())");
						stringBuilder.AppendFormat("promotion_price:{0},", "''");
						stringBuilder.AppendFormat("categroyDetail:{0},", "GetcategroyDetail(false)");
						stringBuilder.AppendFormat("brand:{0},", "$(\"#bfdBrand\").text()");
						stringBuilder.AppendFormat("store:{0},", "parseInt($(\"#productDetails_Stock\").text())");
						stringBuilder.AppendFormat("del :{0},", "false");
						stringBuilder.Append(string.Concat(new string[]
						{
							"user_id :\"",
							text3,
							"\" , client :\"",
							text2,
							"\"}"
						}));
						text = text + "<script type='text/javascript'>BFD(" + stringBuilder.ToString() + ",'productdetail') </script>";
					}
					else
					{
						if (url.IndexOf("productdetails") >= 0)
						{
							string arg2 = HttpContext.Current.Request.QueryString["productId"];
							StringBuilder stringBuilder2 = new StringBuilder();
							stringBuilder2.Append("{");
							stringBuilder2.AppendFormat("id:\"{0}\",", arg2);
							stringBuilder2.AppendFormat("sku_id:{0},", "$(\"#bfdProductCode\").text()");
							stringBuilder2.AppendFormat("name:{0},", "$(\"#bfdProductTitle\").text()");
							stringBuilder2.AppendFormat("item_link:{0},", "location.href");
							stringBuilder2.AppendFormat("image_link:{0},", "location.origin+$(\"#ProductDetails_common_ProductImages___iptPicUrl1\").val()");
							stringBuilder2.AppendFormat("price:{0},", "changeTwoDecimal($(\"#ProductDetails_lblBuyPrice\").text())");
							stringBuilder2.AppendFormat("promotion_price:{0},", "''");
							stringBuilder2.AppendFormat("categroyDetail:{0},", "GetcategroyDetail(false)");
							stringBuilder2.AppendFormat("brand:{0},", "$(\"#bfdBrand\").text()");
							stringBuilder2.AppendFormat("store:{0},", "parseInt($(\"#productDetails_Stock\").text())");
							stringBuilder2.AppendFormat("del :{0},", "true");
							stringBuilder2.Append(string.Concat(new string[]
							{
								"user_id :\"",
								text3,
								"\" , client :\"",
								text2,
								"\"}"
							}));
							text = text + "<script type='text/javascript'>BFD(" + stringBuilder2.ToString() + ",'productdetail') </script>";
						}
						else
						{
							if (url.IndexOf("shoppingcart") >= 0)
							{
								StringBuilder stringBuilder3 = new StringBuilder();
								stringBuilder3.Append("{");
								stringBuilder3.AppendFormat("items:{0},", "GetBFDCartitems()");
								stringBuilder3.Append(string.Concat(new string[]
								{
									"user_id :\"",
									text3,
									"\" , client :\"",
									text2,
									"\"}"
								}));
								text = text + "<script type='text/javascript'>BFD(" + stringBuilder3.ToString() + ",'cart') </script>";
							}
							else
							{
								if (url.IndexOf("listproduct") >= 0)
								{
									string textToFormat = HttpContext.Current.Request.QueryString["keywords"];
									StringBuilder stringBuilder4 = new StringBuilder();
									stringBuilder4.Append("{");
									stringBuilder4.AppendFormat("search_word:\"{0}\",", DataHelper.CleanSearchString(Globals.UrlDecode(Globals.HtmlEncode(textToFormat))));
									stringBuilder4.AppendFormat("result:{0},", "$(\".category_pro_tab\").find(\"li\").length>0?true:false");
									stringBuilder4.Append(string.Concat(new string[]
									{
										"user_id :\"",
										text3,
										"\" , client :\"",
										text2,
										"\"}"
									}));
									text = text + "<script type='text/javascript'>BFD(" + stringBuilder4.ToString() + ",'search') </script>";
								}
								else
								{
									if (url.IndexOf("category") >= 0)
									{
										StringBuilder stringBuilder5 = new StringBuilder();
										stringBuilder5.Append("{");
										stringBuilder5.AppendFormat("category_tree:{0},", "GetcategroyDetail(true)");
										stringBuilder5.Append(string.Concat(new string[]
										{
											"user_id :\"",
											text3,
											"\" , client :\"",
											text2,
											"\"  }"
										}));
										text = text + "<script type='text/javascript'>BFD(" + stringBuilder5.ToString() + ",'categroy')</script>";
									}
									else
									{
										if (url.IndexOf("paymentreturn_url") >= 0)
										{
											Literal literal = this.Page.Controls[0].FindControl("litMessage") as Literal;
											if (literal.Text.IndexOf("订单已成功完成支付") >= 0)
											{
												Regex regex = new Regex("\\d+", RegexOptions.IgnoreCase);
												Match match = regex.Match(literal.Text);
												string value = match.Value;
												if (!string.IsNullOrEmpty(value))
												{
													BFDOrder bFDOrder = ControlProvider.Instance().GetBFDOrder(value);
													StringBuilder stringBuilder6 = new StringBuilder();
													stringBuilder6.Append("{");
													stringBuilder6.AppendFormat("order_id:\"{0}\",", value);
													stringBuilder6.AppendFormat("order_list:{0},", bFDOrder.orderlist);
													stringBuilder6.AppendFormat("order_sumprice:{0},", bFDOrder.ordertotal);
													stringBuilder6.Append(string.Concat(new string[]
													{
														"user_id :\"",
														text3,
														"\" , client :\"",
														text2,
														"\"  }"
													}));
													text = text + "<script type='text/javascript'>BFD(" + stringBuilder6.ToString() + ",'buy')</script>";
												}
											}
										}
										else
										{
											if (url.IndexOf("paysucceed") >= 0)
											{
												string text4 = HttpContext.Current.Request.QueryString["orderId"];
												if (!string.IsNullOrEmpty(text4))
												{
													BFDOrder bFDOrder2 = ControlProvider.Instance().GetBFDOrder(text4);
													StringBuilder stringBuilder7 = new StringBuilder();
													stringBuilder7.Append("{");
													stringBuilder7.AppendFormat("order_id:\"{0}\",", text4);
													stringBuilder7.AppendFormat("order_list:{0},", bFDOrder2.orderlist);
													stringBuilder7.AppendFormat("order_sumprice:{0},", bFDOrder2.ordertotal);
													stringBuilder7.Append(string.Concat(new string[]
													{
														"user_id :\"",
														text3,
														"\" , client :\"",
														text2,
														"\"  }"
													}));
													text = text + "<script type='text/javascript'>BFD(" + stringBuilder7.ToString() + ",'buy')</script>";
												}
											}
											else
											{
												if (url.IndexOf("finishorder.aspx") >= 0)
												{
													string text5 = HttpContext.Current.Request.QueryString["orderId"];
													if (!string.IsNullOrEmpty(text5))
													{
														BFDOrder bFDOrder3 = ControlProvider.Instance().GetBFDOrder(text5);
														StringBuilder stringBuilder8 = new StringBuilder();
														stringBuilder8.Append("{");
														stringBuilder8.AppendFormat("order_id:\"{0}\",", text5);
														stringBuilder8.AppendFormat("order_list:{0},", bFDOrder3.orderlist);
														stringBuilder8.AppendFormat("order_sumprice:{0},", bFDOrder3.ordertotal);
														stringBuilder8.AppendFormat("order_shippingName:\"{0}\",", bFDOrder3.ModeName);
														stringBuilder8.AppendFormat("order_payName:\"{0}\",", bFDOrder3.paymenttype);
														stringBuilder8.Append(string.Concat(new string[]
														{
															"user_id :\"",
															text3,
															"\" , client :\"",
															text2,
															"\"  }"
														}));
														text = text + "<script type='text/javascript'>BFD(" + stringBuilder8.ToString() + ",'order')</script>";
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
			return text;
		}
	}
}
