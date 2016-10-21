using EcShop.Core;
using EcShop.Entities;
using EcShop.Entities.Commodities;
using EcShop.Entities.Sales;
using EcShop.Membership.Context;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
using System.Web;
using System.Xml;
namespace EcShop.SqlDal.Sales
{
	public class CookieShoppingDao
	{
		private const string CartDataCookieName = "Hid_Ecshop_ShoppingCart_Data_New";
		private Database database;
		public CookieShoppingDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public decimal GetCostPrice(string skuId)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT CostPrice FROM Ecshop_SKUs WHERE SkuId=@SkuId");
			this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			decimal result;
			if (obj != null && obj != DBNull.Value)
			{
				result = (decimal)obj;
			}
			else
			{
				result = 0m;
			}
			return result;
		}
		public int GetSkuStock(string skuId)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT Stock FROM Ecshop_SKUs WHERE SkuId=@SkuId;");
			this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			int result;
			if (obj != null && obj != DBNull.Value)
			{
				result = (int)obj;
			}
			else
			{
				result = 0;
			}
			return result;
		}
		public bool IsExistSkuId(string skuId)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(SkuId) FROM Ecshop_SKUs WHERE SkuId=@SkuId;");
			this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			return obj != null && obj != DBNull.Value && (int)obj != 0;
		}
        public AddCartItemStatus AddLineItem(string skuId, int quantity, int storeId)
		{
			AddCartItemStatus result;
			if (this.IsExistSkuId(skuId))
			{
				XmlDocument shoppingCartData = this.GetShoppingCartData();
				XmlNode xmlNode = shoppingCartData.SelectSingleNode("//sc/lis");
				XmlNode xmlNode2 = xmlNode.SelectSingleNode("l[@s='" + skuId + "']");
				if (xmlNode2 == null)
				{
                    xmlNode2 = CookieShoppingDao.CreateLineItemNode(shoppingCartData, skuId, quantity, storeId);
					xmlNode.InsertBefore(xmlNode2, xmlNode.FirstChild);
				}
				else
				{
                    if (storeId != 0)
                    {                
                        xmlNode2.Attributes["q"].Value = (int.Parse(xmlNode2.Attributes["q"].Value) + quantity).ToString(CultureInfo.InvariantCulture);
                        xmlNode2.Attributes["sto"].Value = storeId.ToString(CultureInfo.InvariantCulture); //门店Id使用最新的覆盖
                    }
                    else
                    {
                        xmlNode2.Attributes["q"].Value = (int.Parse(xmlNode2.Attributes["q"].Value) + quantity).ToString(CultureInfo.InvariantCulture);
                    }
				}
				this.SaveShoppingCartData(shoppingCartData);
				result = AddCartItemStatus.Successed;
			}
			else
			{
				result = AddCartItemStatus.ProductNotExists;
			}
			return result;
		}
		public void ClearShoppingCart()
		{
			HiContext.Current.Context.Response.Cookies["Hid_Ecshop_ShoppingCart_Data_New"].Value = null;
			HiContext.Current.Context.Response.Cookies["Hid_Ecshop_ShoppingCart_Data_New"].Expires = new DateTime(1999, 10, 12);
			HiContext.Current.Context.Response.Cookies["Hid_Ecshop_ShoppingCart_Data_New"].Path = "/";
		}
		public ShoppingCartInfo GetShoppingCart()
		{
			XmlDocument shoppingCartData = this.GetShoppingCartData();
			ShoppingCartInfo shoppingCartInfo = null;
			XmlNodeList xmlNodeList = shoppingCartData.SelectNodes("//sc/lis/l");
			XmlNodeList xmlNodeList2 = shoppingCartData.SelectNodes("//sc/gf/l");
			if ((xmlNodeList != null && xmlNodeList.Count > 0) || (xmlNodeList2 != null && xmlNodeList2.Count > 0))
			{
				shoppingCartInfo = new ShoppingCartInfo();
			}
			if (xmlNodeList != null && xmlNodeList.Count > 0)
			{
				IList<string> list = new List<string>();
				Dictionary<string, int> dictionary = new Dictionary<string, int>();
                Dictionary<string, int> dictionary2 = new Dictionary<string, int>();
				foreach (XmlNode xmlNode in xmlNodeList)
				{
					list.Add(xmlNode.Attributes["s"].Value);
					dictionary.Add(xmlNode.Attributes["s"].Value, int.Parse(xmlNode.Attributes["q"].Value));
                    dictionary2.Add(xmlNode.Attributes["s"].Value, int.Parse(xmlNode.Attributes["sto"].Value));//门店id
				}
                this.LoadCartProduct(shoppingCartInfo, dictionary, list, dictionary2);
			}
			if (xmlNodeList2 != null && xmlNodeList2.Count > 0)
			{
				StringBuilder stringBuilder = new StringBuilder();
				Dictionary<int, int> dictionary2 = new Dictionary<int, int>();
				Dictionary<int, int> dictionary3 = new Dictionary<int, int>();
				foreach (XmlNode xmlNode2 in xmlNodeList2)
				{
					stringBuilder.AppendFormat("{0},", int.Parse(xmlNode2.Attributes["g"].Value));
					dictionary2.Add(int.Parse(xmlNode2.Attributes["g"].Value), int.Parse(xmlNode2.Attributes["g"].Value));
					dictionary3.Add(int.Parse(xmlNode2.Attributes["g"].Value), int.Parse(xmlNode2.Attributes["q"].Value));
				}
				this.LoadCartGift(shoppingCartInfo, dictionary2, dictionary3, stringBuilder.ToString());
			}
			return shoppingCartInfo;
		}
		public void RemoveLineItem(string skuId)
		{
			XmlDocument shoppingCartData = this.GetShoppingCartData();
			XmlNode xmlNode = shoppingCartData.SelectSingleNode("//sc/lis");
			XmlNode xmlNode2 = xmlNode.SelectSingleNode("l[@s='" + skuId + "']");
			if (xmlNode2 != null)
			{
				xmlNode.RemoveChild(xmlNode2);
				this.SaveShoppingCartData(shoppingCartData);
			}
		}
		public void UpdateLineItemQuantity(string skuId, int quantity)
		{
			if (quantity <= 0)
			{
				this.RemoveLineItem(skuId);
			}
			else
			{
				XmlDocument shoppingCartData = this.GetShoppingCartData();
				XmlNode xmlNode = shoppingCartData.SelectSingleNode("//lis");
				XmlNode xmlNode2 = xmlNode.SelectSingleNode("l[@s='" + skuId + "']");
				if (xmlNode2 != null)
				{
					xmlNode2.Attributes["q"].Value = quantity.ToString(CultureInfo.InvariantCulture);
					this.SaveShoppingCartData(shoppingCartData);
				}
			}
		}
		public bool AddGiftItem(int giftId, int quantity)
		{
			XmlDocument shoppingCartData = this.GetShoppingCartData();
			XmlNode xmlNode = shoppingCartData.SelectSingleNode("//sc/gf");
			XmlNode xmlNode2 = xmlNode.SelectSingleNode("l[@g=" + giftId + "]");
			if (xmlNode2 == null)
			{
				xmlNode2 = CookieShoppingDao.CreateGiftLineItemNode(shoppingCartData, giftId, quantity);
				xmlNode.InsertBefore(xmlNode2, xmlNode.FirstChild);
			}
			else
			{
				xmlNode2.Attributes["q"].Value = (int.Parse(xmlNode2.Attributes["q"].Value) + quantity).ToString(CultureInfo.InvariantCulture);
			}
			this.SaveShoppingCartData(shoppingCartData);
			return true;
		}
		public void UpdateGiftItemQuantity(int giftId, int quantity)
		{
			if (quantity <= 0)
			{
				this.RemoveGiftItem(giftId);
			}
			else
			{
				XmlDocument shoppingCartData = this.GetShoppingCartData();
				XmlNode xmlNode = shoppingCartData.SelectSingleNode("//sc/gf");
				XmlNode xmlNode2 = xmlNode.SelectSingleNode("l[@g='" + giftId + "']");
				if (xmlNode2 != null)
				{
					xmlNode2.Attributes["q"].Value = quantity.ToString(CultureInfo.InvariantCulture);
					this.SaveShoppingCartData(shoppingCartData);
				}
			}
		}
		public void RemoveGiftItem(int giftId)
		{
			XmlDocument shoppingCartData = this.GetShoppingCartData();
			XmlNode xmlNode = shoppingCartData.SelectSingleNode("//sc/gf");
			XmlNode xmlNode2 = xmlNode.SelectSingleNode("l[@g='" + giftId + "']");
			if (xmlNode2 != null)
			{
				xmlNode.RemoveChild(xmlNode2);
				this.SaveShoppingCartData(shoppingCartData);
			}
		}
		private ShoppingCartItemInfo GetCartItemInfo(string skuId, int quantity,int storeId)
		{
			ShoppingCartItemInfo shoppingCartItemInfo = null;
			DbCommand storedProcCommand = this.database.GetStoredProcCommand("ss_ShoppingCart_GetItemInfo");
			this.database.AddInParameter(storedProcCommand, "Quantity", DbType.Int32, quantity);
			this.database.AddInParameter(storedProcCommand, "UserId", DbType.Int32, 0);
			this.database.AddInParameter(storedProcCommand, "SkuId", DbType.String, skuId);
			this.database.AddInParameter(storedProcCommand, "GradeId", DbType.Int32, 0);
			using (IDataReader dataReader = this.database.ExecuteReader(storedProcCommand))
			{
				if (dataReader.Read())
				{
					shoppingCartItemInfo = new ShoppingCartItemInfo();
					shoppingCartItemInfo.SkuId = skuId;
					shoppingCartItemInfo.ProductId = (int)dataReader["ProductId"];
					shoppingCartItemInfo.Name = dataReader["ProductName"].ToString();
                    if (DBNull.Value != dataReader["TaxRate"])
                    {
                        shoppingCartItemInfo.TaxRate = decimal.Parse(dataReader["TaxRate"].ToString());
                    }
                    if (DBNull.Value != dataReader["TaxRateId"])
                    {
                        shoppingCartItemInfo.TaxRateId = (int)dataReader["TaxRateId"];
                    }
					if (DBNull.Value != dataReader["Weight"])
					{
						shoppingCartItemInfo.Weight = (decimal)dataReader["Weight"];
					}
					shoppingCartItemInfo.MemberPrice = (shoppingCartItemInfo.AdjustedPrice = (decimal)dataReader["SalePrice"]);
					if (DBNull.Value != dataReader["ThumbnailUrl40"])
					{
						shoppingCartItemInfo.ThumbnailUrl40 = dataReader["ThumbnailUrl40"].ToString();
					}
					if (DBNull.Value != dataReader["ThumbnailUrl60"])
					{
						shoppingCartItemInfo.ThumbnailUrl60 = dataReader["ThumbnailUrl60"].ToString();
					}
					if (DBNull.Value != dataReader["ThumbnailUrl100"])
					{
						shoppingCartItemInfo.ThumbnailUrl100 = dataReader["ThumbnailUrl100"].ToString();
					}
                    if (DBNull.Value != dataReader["ThumbnailUrl160"])
                    {
                        shoppingCartItemInfo.ThumbnailUrl160 = dataReader["ThumbnailUrl160"].ToString();
                    }
                    if (DBNull.Value != dataReader["ThumbnailUrl180"])
                    {
                        shoppingCartItemInfo.ThumbnailUrl180 = dataReader["ThumbnailUrl180"].ToString();
                    }
                    if (DBNull.Value != dataReader["ThumbnailUrl220"])
                    {
                        shoppingCartItemInfo.ThumbnailUrl220 = dataReader["ThumbnailUrl220"].ToString();
                    }
					if (dataReader["SKU"] != DBNull.Value)
					{
						shoppingCartItemInfo.SKU = (string)dataReader["SKU"];
					}
					shoppingCartItemInfo.ShippQuantity = quantity;
                    shoppingCartItemInfo.StoreId = storeId;//门店id
                    shoppingCartItemInfo.Quantity = quantity;
					if (DBNull.Value != dataReader["IsfreeShipping"])
					{
						shoppingCartItemInfo.IsfreeShipping = Convert.ToBoolean(dataReader["IsfreeShipping"]);
					}
                    shoppingCartItemInfo.IsCustomsClearance = false;
                    if (DBNull.Value != dataReader["IsCustomsClearance"])
                    {
                        shoppingCartItemInfo.IsCustomsClearance = Convert.ToBoolean(dataReader["IsCustomsClearance"]);
                    }
					string text = string.Empty;
					if (dataReader.NextResult())
					{
						while (dataReader.Read())
						{
							if (dataReader["AttributeName"] != DBNull.Value && !string.IsNullOrEmpty((string)dataReader["AttributeName"]) && dataReader["ValueStr"] != DBNull.Value && !string.IsNullOrEmpty((string)dataReader["ValueStr"]))
							{
								object obj = text;
								text = string.Concat(new object[]
								{
									obj,
									dataReader["AttributeName"],
									"：",
									dataReader["ValueStr"],
									"; "
								});
							}
						}
					}
					shoppingCartItemInfo.SkuContent = text;
				}
			}
			return shoppingCartItemInfo;
		}
		public bool GetShoppingProductInfo(int productId, string skuId, out ProductSaleStatus saleStatus, out int stock, out int totalQuantity)
		{
			saleStatus = ProductSaleStatus.Delete;
			stock = 0;
			totalQuantity = 0;
			bool result = false;
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT Stock，SaleStatus,AlertStock FROM Ecshop_Skus s INNER JOIN Ecshop_Products p ON s.ProductId=p.ProductId WHERE s.ProductId=@ProductId AND s.SkuId=@SkuId");
			this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					saleStatus = (ProductSaleStatus)((int)dataReader["SaleStatus"]);
					stock = (int)dataReader["Stock"];
					int num = (int)dataReader["AlertStock"];
					if (stock <= num)
					{
						saleStatus = ProductSaleStatus.UnSale;
					}
					result = true;
				}
				totalQuantity = this.GetShoppingProductQuantity(skuId, productId);
			}
			return result;
		}
		public Dictionary<string, decimal> GetCostPriceForItems(int userId)
		{
			Dictionary<string, decimal> dictionary = new Dictionary<string, decimal>();
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT sc.SkuId, s.CostPrice FROM Ecshop_ShoppingCarts sc INNER JOIN Ecshop_SKUs s ON sc.SkuId = s.SkuId WHERE UserId = @UserId");
			this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					decimal value = (dataReader["CostPrice"] == DBNull.Value) ? 0m : ((decimal)dataReader["CostPrice"]);
					dictionary.Add((string)dataReader["SkuId"], value);
				}
			}
			return dictionary;
		}
		private void LoadCartGift(ShoppingCartInfo cartInfo, Dictionary<int, int> giftIdList, Dictionary<int, int> giftQuantityList, string giftIds)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT * FROM Ecshop_Gifts WHERE GiftId in {0}", giftIds.TrimEnd(new char[]
			{
				','
			})));
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					ShoppingCartGiftInfo shoppingCartGiftInfo = DataMapper.PopulateGiftCartItem(dataReader);
					shoppingCartGiftInfo.Quantity = giftQuantityList[shoppingCartGiftInfo.GiftId];
					cartInfo.LineGifts.Add(shoppingCartGiftInfo);
				}
			}
		}
		private void LoadCartProduct(ShoppingCartInfo cartInfo, Dictionary<string, int> productQuantityList, IList<string> skuIds,Dictionary<string, int> storeIdList)
		{
			foreach (string current in skuIds)
			{
                ShoppingCartItemInfo cartItemInfo = this.GetCartItemInfo(current, productQuantityList[current], storeIdList[current]);
				if (cartItemInfo != null)
				{
					cartInfo.LineItems.Add(cartItemInfo);
				}
			}
		}
		private int GetShoppingProductQuantity(string skuId, int ProductId)
		{
			int result = 0;
			XmlDocument shoppingCartData = this.GetShoppingCartData();
			XmlNode xmlNode = shoppingCartData.SelectSingleNode(string.Concat(new object[]
			{
				"//sc/lis/l[SkuId='",
				skuId,
				"' AND p=",
				ProductId,
				"]"
			}));
			if (xmlNode != null)
			{
				int.TryParse(xmlNode.Attributes["q"].Value, out result);
			}
			return result;
		}
		private XmlDocument GetShoppingCartData()
		{
			XmlDocument xmlDocument = new XmlDocument();
			HttpCookie httpCookie = HiContext.Current.Context.Request.Cookies["Hid_Ecshop_ShoppingCart_Data_New"];
			if (httpCookie == null || string.IsNullOrEmpty(httpCookie.Value))
			{
				xmlDocument = CookieShoppingDao.CreateEmptySchema();
			}
			else
			{
				try
				{
					xmlDocument.LoadXml(Globals.UrlDecode(httpCookie.Value));
				}
				catch
				{
					this.ClearShoppingCart();
					xmlDocument = CookieShoppingDao.CreateEmptySchema();
				}
			}
			return xmlDocument;
		}
		private void SaveShoppingCartData(XmlDocument doc)
		{
			if (doc == null)
			{
				this.ClearShoppingCart();
			}
			else
			{
				HttpCookie httpCookie = HiContext.Current.Context.Request.Cookies["Hid_Ecshop_ShoppingCart_Data_New"];
				if (httpCookie == null)
				{
					httpCookie = new HttpCookie("Hid_Ecshop_ShoppingCart_Data_New");
				}
				httpCookie.Value = Globals.UrlEncode(doc.OuterXml);
				httpCookie.Expires = DateTime.Now.AddDays(3.0);
				HiContext.Current.Context.Response.Cookies.Add(httpCookie);
			}
		}
		private static XmlDocument CreateEmptySchema()
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml("<sc><lis></lis><gf></gf></sc>");
			return xmlDocument;
		}
        private static XmlNode CreateLineItemNode(XmlDocument doc, string skuId, int quantity, int storeId)
		{
			XmlNode xmlNode = doc.CreateElement("l");
			XmlNode xmlNode2 = doc.SelectSingleNode("//lis");
			XmlAttribute xmlAttribute = doc.CreateAttribute("s");
			xmlAttribute.Value = skuId;
			XmlAttribute xmlAttribute2 = doc.CreateAttribute("q");
			xmlAttribute2.Value = quantity.ToString(CultureInfo.InvariantCulture);
            XmlAttribute xmlAttribute3 = doc.CreateAttribute("sto");
            xmlAttribute3.Value = storeId.ToString(CultureInfo.InvariantCulture);
			xmlNode.Attributes.Append(xmlAttribute);
			xmlNode.Attributes.Append(xmlAttribute2);
            xmlNode.Attributes.Append(xmlAttribute3);
			return xmlNode;
		}
		private static XmlNode CreateGiftLineItemNode(XmlDocument doc, int giftId, int quantity)
		{
			XmlNode xmlNode = doc.CreateElement("l");
			XmlNode xmlNode2 = doc.SelectSingleNode("//gf");
			XmlAttribute xmlAttribute = doc.CreateAttribute("q");
			xmlAttribute.Value = quantity.ToString(CultureInfo.InvariantCulture);
			XmlAttribute xmlAttribute2 = doc.CreateAttribute("g");
			xmlAttribute2.Value = giftId.ToString();
			xmlNode.Attributes.Append(xmlAttribute);
			xmlNode.Attributes.Append(xmlAttribute2);
			return xmlNode;
		}
		private static int GenerateLastItemId(XmlDocument doc)
		{
			XmlNode xmlNode = doc.SelectSingleNode("/sc");
			XmlAttribute xmlAttribute = xmlNode.Attributes["lid"];
			int result;
			if (xmlAttribute == null)
			{
				xmlAttribute = doc.CreateAttribute("lid");
				xmlNode.Attributes.Append(xmlAttribute);
				result = 1;
			}
			else
			{
				result = int.Parse(xmlAttribute.Value) + 1;
			}
			xmlAttribute.Value = result.ToString(CultureInfo.InvariantCulture);
			return result;
		}
	}
}
