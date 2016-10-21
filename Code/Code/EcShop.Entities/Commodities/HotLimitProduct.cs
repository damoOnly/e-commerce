using EcShop.Core;
using EcShop.Membership.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Commodities
{
   public class HotLimitProduct
    {
        private static string browedProductList;
        static HotLimitProduct()
		{
            HotLimitProduct.browedProductList = "BrowedHotLimitProduct-Admin";
			if (HiContext.Current.SiteSettings.UserId.HasValue)
			{
                HotLimitProduct.browedProductList = string.Format("BrowedHotLimitProduct-{0}", HiContext.Current.SiteSettings.UserId.Value);
			}
		}
		public static System.Collections.Generic.IList<int> GetBrowedProductList()
		{
			System.Collections.Generic.IList<int> result = new System.Collections.Generic.List<int>();
            HttpCookie httpCookie = HiContext.Current.Context.Request.Cookies[HotLimitProduct.browedProductList];
			if (httpCookie != null && !string.IsNullOrEmpty(httpCookie.Value))
			{
				result = (Serializer.ConvertToObject(HiContext.Current.Context.Server.UrlDecode(httpCookie.Value), typeof(System.Collections.Generic.List<int>)) as System.Collections.Generic.List<int>);
			}
			return result;
		}
		public static System.Collections.Generic.IList<int> GetBrowedProductList(int maxNum)
		{
            System.Collections.Generic.IList<int> list = HotLimitProduct.GetBrowedProductList();
			int count = list.Count;
			if (list.Count > maxNum)
			{
				for (int i = 0; i < count - maxNum; i++)
				{
					list.RemoveAt(0);
				}
			}
			return list;
		}
		public static void EnQueue(int productId)
		{
            System.Collections.Generic.IList<int> list = HotLimitProduct.GetBrowedProductList();
			int num = 0;
			foreach (int current in list)
			{
				if (productId == current)
				{
					list.RemoveAt(num);
					break;
				}
				num++;
			}
			if (list.Count <= 20)
			{
				list.Add(productId);
			}
			else
			{
				list.RemoveAt(0);
				list.Add(productId);
			}
            HotLimitProduct.SaveCookie(list);
		}
		public static void ClearQueue()
		{
            HotLimitProduct.SaveCookie(null);
		}
		private static void SaveCookie(System.Collections.Generic.IList<int> productIdList)
		{
            HttpCookie httpCookie = HiContext.Current.Context.Request.Cookies[HotLimitProduct.browedProductList];
			if (httpCookie == null)
			{
                httpCookie = new HttpCookie(HotLimitProduct.browedProductList);
			}
			httpCookie.Expires = System.DateTime.Now.AddDays(7.0);
			httpCookie.Value = Globals.UrlEncode(Serializer.ConvertToString(productIdList));
			HttpContext.Current.Response.Cookies.Add(httpCookie);
		}
    }
}
