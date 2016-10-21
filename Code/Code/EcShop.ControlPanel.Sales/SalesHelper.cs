using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Core.Enums;
using EcShop.Entities;
using EcShop.Entities.Orders;
using EcShop.Entities.Sales;
using EcShop.SqlDal.Sales;
using System;
using System.Collections.Generic;
using System.Data;
namespace EcShop.ControlPanel.Sales
{
	public sealed class SalesHelper
	{
		private SalesHelper()
		{
		}
		public static bool AddShipper(ShippersInfo shipper)
		{
			Globals.EntityCoding(shipper, true);
			return new ShipperDao().AddShipper(shipper);
		}
		public static bool UpdateShipper(ShippersInfo shipper)
		{
			Globals.EntityCoding(shipper, true);
			return new ShipperDao().UpdateShipper(shipper);
		}
		public static bool DeleteShipper(int shipperId)
		{
			return new ShipperDao().DeleteShipper(shipperId);
		}
		public static ShippersInfo GetShipper(int shipperId)
		{
			return new ShipperDao().GetShipper(shipperId);
		}
		public static IList<ShippersInfo> GetShippers(bool includeDistributor)
		{
			return new ShipperDao().GetShippers(includeDistributor);
		}
		public static void SetDefalutShipper(int shipperId)
		{
			new ShipperDao().SetDefalutShipper(shipperId);
		}
		public static bool AddExpressTemplate(string expressName, string xmlFile)
		{
			return new ExpressTemplateDao().AddExpressTemplate(expressName, xmlFile);
		}
		public static bool UpdateExpressTemplate(int expressId, string expressName)
		{
			return new ExpressTemplateDao().UpdateExpressTemplate(expressId, expressName);
		}
		public static bool SetExpressIsUse(int expressId)
		{
			return new ExpressTemplateDao().SetExpressIsUse(expressId);
		}
		public static bool DeleteExpressTemplate(int expressId)
		{
			return new ExpressTemplateDao().DeleteExpressTemplate(expressId);
		}
		public static System.Data.DataTable GetExpressTemplates()
		{
			return new ExpressTemplateDao().GetExpressTemplates(null);
		}
		public static System.Data.DataTable GetIsUserExpressTemplates()
		{
			return new ExpressTemplateDao().GetExpressTemplates(new bool?(true));
		}
		public static void SwapPaymentModeSequence(int modeId, int replaceModeId, int displaySequence, int replaceDisplaySequence)
		{
			new PaymentModeDao().SwapPaymentModeSequence(modeId, replaceModeId, displaySequence, replaceDisplaySequence);
		}
		public static PaymentModeActionStatus CreatePaymentMode(PaymentModeInfo paymentMode)
		{
			PaymentModeActionStatus result;
			if (null == paymentMode)
			{
				result = PaymentModeActionStatus.UnknowError;
			}
			else
			{
				Globals.EntityCoding(paymentMode, true);
				result = new PaymentModeDao().CreateUpdateDeletePaymentMode(paymentMode, DataProviderAction.Create);
			}
			return result;
		}
		public static PaymentModeActionStatus UpdatePaymentMode(PaymentModeInfo paymentMode)
		{
			PaymentModeActionStatus result;
			if (null == paymentMode)
			{
				result = PaymentModeActionStatus.UnknowError;
			}
			else
			{
				Globals.EntityCoding(paymentMode, true);
				result = new PaymentModeDao().CreateUpdateDeletePaymentMode(paymentMode, DataProviderAction.Update);
			}
			return result;
		}
		public static bool DeletePaymentMode(int modeId)
		{
			PaymentModeInfo paymentModeInfo = new PaymentModeInfo();
			paymentModeInfo.ModeId = modeId;
			return new PaymentModeDao().CreateUpdateDeletePaymentMode(paymentModeInfo, DataProviderAction.Delete) == PaymentModeActionStatus.Success;
		}
		public static IList<PaymentModeInfo> GetPaymentModes(PayApplicationType payApplicationType)
		{
			return new PaymentModeDao().GetPaymentModes(payApplicationType);
		}
		public static PaymentModeInfo GetPaymentMode(int modeId)
		{
			return new PaymentModeDao().GetPaymentMode(modeId);
		}
		public static PaymentModeInfo GetPaymentMode(string gateway)
		{
			return new PaymentModeDao().GetPaymentMode(gateway);
		}
		public static IList<ShippingModeInfo> GetShippingModes()
		{
			return new ShippingModeDao().GetShippingModes();
		}
		public static void SwapShippingModeSequence(int modeId, int replaceModeId, int displaySequence, int replaceDisplaySequence)
		{
			new ShippingModeDao().SwapShippingModeSequence(modeId, replaceModeId, displaySequence, replaceDisplaySequence);
		}
		public static ShippingModeInfo GetShippingMode(int modeId, bool includeDetail)
		{
			return new ShippingModeDao().GetShippingMode(modeId, includeDetail);
		}
		public static bool CreateShippingMode(ShippingModeInfo shippingMode)
		{
			return null != shippingMode && new ShippingModeDao().CreateShippingMode(shippingMode);
		}
		public static bool DeleteShippingMode(int modeId)
		{
			return new ShippingModeDao().DeleteShippingMode(modeId);
		}
		public static bool UpdateShippMode(ShippingModeInfo shippingMode)
		{
			bool result;
			if (shippingMode == null)
			{
				result = false;
			}
			else
			{
				Globals.EntityCoding(shippingMode, true);
				result = new ShippingModeDao().UpdateShippingMode(shippingMode);
			}
			return result;
		}
		public static bool CreateShippingTemplate(ShippingModeInfo shippingMode)
		{
			return new ShippingModeDao().CreateShippingTemplate(shippingMode);
		}
		public static bool UpdateShippingTemplate(ShippingModeInfo shippingMode)
		{
			return new ShippingModeDao().UpdateShippingTemplate(shippingMode);
		}
		public static bool DeleteShippingTemplate(int templateId)
		{
			return new ShippingModeDao().DeleteShippingTemplate(templateId);
		}
		public static DbQueryResult GetShippingTemplates(Pagination pagin)
		{
			return new ShippingModeDao().GetShippingTemplates(pagin);
		}
		public static ShippingModeInfo GetShippingTemplate(int templateId, bool includeDetail)
		{
			return new ShippingModeDao().GetShippingTemplate(templateId, includeDetail);
		}
		public static System.Data.DataTable GetShippingAllTemplates()
		{
			return new ShippingModeDao().GetShippingAllTemplates();
		}
		public static IList<string> GetExpressCompanysByMode(int modeId)
		{
			return new ShippingModeDao().GetExpressCompanysByMode(modeId);
		}
		public static System.Data.DataTable GetProductSales(SaleStatisticsQuery productSale, out int totalProductSales)
		{
			System.Data.DataTable result;
			if (productSale == null)
			{
				totalProductSales = 0;
				result = null;
			}
			else
			{
				result = new SaleStatisticDao().GetProductSales(productSale, out totalProductSales);
			}
			return result;
		}
		public static System.Data.DataTable GetProductSalesNoPage(SaleStatisticsQuery productSale, out int totalProductSales)
		{
			System.Data.DataTable result;
			if (productSale == null)
			{
				totalProductSales = 0;
				result = null;
			}
			else
			{
				result = new SaleStatisticDao().GetProductSalesNoPage(productSale, out totalProductSales);
			}
			return result;
		}

        public static System.Data.DataTable GetProductSalesAsBrand(SaleStatisticsQuery productSale, out int totalProductSales)
        {
            System.Data.DataTable result;
            if (productSale == null)
            {
                totalProductSales = 0;
                result = null;
            }
            else
            {
                result = new SaleStatisticDao().GetProductSaleAsBrand(productSale, out totalProductSales);
            }
            return result;
        }

        public static System.Data.DataTable GetProductSaleAsBrandNoPage(SaleStatisticsQuery productSale, out int totalProductSales)
        {
            System.Data.DataTable result;
            if (productSale == null)
            {
                totalProductSales = 0;
                result = null;
            }
            else
            {
                result = new SaleStatisticDao().GetProductSaleAsBrandNoPage(productSale, out totalProductSales);
            }
            return result;
        }

        public static System.Data.DataTable GetProductSalesAsImportSource(SaleStatisticsQuery productSale, out int totalProductSales)
        {
            System.Data.DataTable result;
            if (productSale == null)
            {
                totalProductSales = 0;
                result = null;
            }
            else
            {
                result = new SaleStatisticDao().GetProductSaleAsImportSource(productSale, out totalProductSales);
            }
            return result;
        }

        public static System.Data.DataTable GetProductSaleAsImportSourceNoPage(SaleStatisticsQuery productSale, out int totalProductSales)
        {
            System.Data.DataTable result;
            if (productSale == null)
            {
                totalProductSales = 0;
                result = null;
            }
            else
            {
                result = new SaleStatisticDao().GetProductSaleAsImportSourceNoPage(productSale, out totalProductSales);
            }
            return result;
        }

		public static IList<UserStatisticsInfo> GetUserStatistics(Pagination page, out int totalProductSaleVisits)
		{
			IList<UserStatisticsInfo> result;
			if (page == null)
			{
				totalProductSaleVisits = 0;
				result = null;
			}
			else
			{
				result = new SaleStatisticDao().GetUserStatistics(page, out totalProductSaleVisits);
			}
			return result;
		}
		public static OrderStatisticsInfo GetUserOrders(OrderQuery userOrder)
		{
			return new SaleStatisticDao().GetUserOrders(userOrder);
		}
		public static OrderStatisticsInfo GetUserOrdersNoPage(OrderQuery userOrder)
		{
			return new SaleStatisticDao().GetUserOrdersNoPage(userOrder);
		}
		public static AdminStatisticsInfo GetStatistics()
		{
			return new SaleStatisticDao().GetStatistics();
		}
		public static System.Data.DataTable GetMemberStatistics(SaleStatisticsQuery query, out int totalProductSales)
		{
			return new SaleStatisticDao().GetMemberStatistics(query, out totalProductSales);
		}
		public static System.Data.DataTable GetMemberStatisticsNoPage(SaleStatisticsQuery query)
		{
			return new SaleStatisticDao().GetMemberStatisticsNoPage(query);
		}
		public static System.Data.DataTable GetProductVisitAndBuyStatistics(SaleStatisticsQuery query, out int totalProductSales)
		{
			return new SaleStatisticDao().GetProductVisitAndBuyStatistics(query, out totalProductSales);
		}
		public static System.Data.DataTable GetProductVisitAndBuyStatisticsNoPage(SaleStatisticsQuery query, out int totalProductSales)
		{
			return new SaleStatisticDao().GetProductVisitAndBuyStatisticsNoPage(query, out totalProductSales);
		}
		public static DbQueryResult GetSaleOrderLineItemsStatistics(SaleStatisticsQuery query)
		{
			return new SaleStatisticDao().GetSaleOrderLineItemsStatistics(query);
		}
        public static DbQueryResult GetSaleStoreOrderLineItemsStatistics(SaleStatisticsQuery query)
        {
            return new SaleStatisticDao().GetSaleStoreOrderLineItemsStatistics(query);
        }
        /// <summary>
        /// 导出Excel数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static DataTable GetProductVisitAndDt(SaleStatisticsQuery query)
        {
            return new SaleStatisticDao().GetProductVisitAndDt(query);
        }
		public static DbQueryResult GetSaleOrderLineItemsStatisticsNoPage(SaleStatisticsQuery query)
		{
			return new SaleStatisticDao().GetSaleOrderLineItemsStatisticsNoPage(query);
		}
		public static DbQueryResult GetSaleTargets()
		{
			return new SaleStatisticDao().GetSaleTargets();
		}
		public static System.Data.DataTable GetWeekSaleTota(SaleStatisticsType saleStatisticsType)
		{
			return new DateStatisticDao().GetWeekSaleTota(saleStatisticsType);
		}
        public static System.Data.DataTable GetDaySaleTotal(int year, int month, SaleStatisticsType saleStatisticsType, int? siteId, int orderSource)
		{
            return new DateStatisticDao().GetDaySaleTotal(year, month, saleStatisticsType, siteId, orderSource);
		}
		public static decimal GetDaySaleTotal(int year, int month, int day, SaleStatisticsType saleStatisticsType)
		{
			return new DateStatisticDao().GetDaySaleTotal(year, month, day, saleStatisticsType);
		}
		public static decimal GetMonthSaleTotal(int year, int month, SaleStatisticsType saleStatisticsType)
		{
			return new DateStatisticDao().GetMonthSaleTotal(year, month, saleStatisticsType);
		}
        public static System.Data.DataTable GetMonthSaleTotal(int year, SaleStatisticsType saleStatisticsType, int? siteId, int orderSource)
		{
            return new DateStatisticDao().GetMonthSaleTotal(year, saleStatisticsType, siteId, orderSource);
		}
		public static decimal GetYearSaleTotal(int year, SaleStatisticsType saleStatisticsType)
		{
			return new DateStatisticDao().GetYearSaleTotal(year, saleStatisticsType);
		}
		public static IList<UserStatisticsForDate> GetUserAdd(int? year, int? month, int? days)
		{
			return new DateStatisticDao().GetUserAdd(year, month, days);
		}
		public static decimal CalcFreight(int regionId, int totalWeight, ShippingModeInfo shippingModeInfo)
		{
			decimal result = 0m;
			int topRegionId = RegionHelper.GetTopRegionId(regionId);
			decimal d = totalWeight;
			decimal d2 = 1m;
			if (d > shippingModeInfo.Weight && shippingModeInfo.AddWeight.HasValue && shippingModeInfo.AddWeight.Value > 0m)
			{
				if ((d - shippingModeInfo.Weight) % shippingModeInfo.AddWeight == 0m)
				{
					d2 = (d - shippingModeInfo.Weight) / shippingModeInfo.AddWeight.Value;
				}
				else
				{
                    d2 = ((d - shippingModeInfo.Weight) / shippingModeInfo.AddWeight.Value) + 1;//修改1
				}
			}
			if (shippingModeInfo.ModeGroup == null || shippingModeInfo.ModeGroup.Count == 0)
			{
				if (d > shippingModeInfo.Weight && shippingModeInfo.AddPrice.HasValue)
				{
					result = d2 * shippingModeInfo.AddPrice.Value + shippingModeInfo.Price;
				}
				else
				{
					result = shippingModeInfo.Price;
				}
			}
			else
			{
				int? num = null;
				foreach (ShippingModeGroupInfo current in shippingModeInfo.ModeGroup)
				{
					foreach (ShippingRegionInfo current2 in current.ModeRegions)
					{
						if (topRegionId == current2.RegionId)
						{
							num = new int?(current2.GroupId);
							break;
						}
					}
					if (num.HasValue)
					{
						if (d > shippingModeInfo.Weight)
						{
							result = d2 * current.AddPrice + current.Price;
						}
						else
						{
							result = current.Price;
						}
						break;
					}
				}
				if (!num.HasValue)
				{
					if (d > shippingModeInfo.Weight && shippingModeInfo.AddPrice.HasValue)
					{
						result = d2 * shippingModeInfo.AddPrice.Value + shippingModeInfo.Price;
					}
					else
					{
						result = shippingModeInfo.Price;
					}
				}
			}
			return result;
		}
		public static ShippingModeInfo GetShippingModeByCompany(string companyname)
		{
			return new ShippingModeDao().GetShippingModeByCompany(companyname);
		}
        public static DataTable GetCustomServiceStatistics(DateTime startdate, DateTime enddate, int type, string workerNo,int pageSize,int pageIndex,ref int totalCount)
        {
            return new DateStatisticDao().GetCustomServiceStatistics(startdate, enddate, type, workerNo,pageSize,pageIndex,ref  totalCount);
        }
	}
}
