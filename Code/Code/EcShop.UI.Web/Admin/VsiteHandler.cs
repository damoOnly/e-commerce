using EcShop.ControlPanel.Commodities;
using EcShop.ControlPanel.Store;
using EcShop.Entities.Comments;
using EcShop.Entities.Commodities;
using EcShop.Entities.VShop;
using EcShop.SaleSystem.Comments;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
namespace EcShop.UI.Web.Admin
{
	public class VsiteHandler : System.Web.IHttpHandler
	{
		private class EnumJson
		{
			public string Name
			{
				get;
				set;
			}
			public string Value
			{
				get;
				set;
			}
		}
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
		public void ProcessRequest(System.Web.HttpContext context)
		{
			string text = context.Request.Form["actionName"];
			string s = string.Empty;
			string key;
			switch (key = text)
			{
			case "Topic":
			{
				System.Collections.Generic.IList<TopicInfo> value = VShopHelper.Gettopics();
				s = JsonConvert.SerializeObject(value);
				break;
			}
			case "Vote":
			{
				System.Data.DataSet votes = StoreHelper.GetVotes();
				s = JsonConvert.SerializeObject(votes);
				break;
			}
			case "Category":
			{
				var value2 = 
					from item in CatalogHelper.GetMainCategories()
					select new
					{
						CateId = item.CategoryId,
						CateName = item.Name
					};
				s = JsonConvert.SerializeObject(value2);
				break;
			}
			case "Activity":
			{
				System.Array values = System.Enum.GetValues(typeof(LotteryActivityType));
				System.Collections.Generic.List<VsiteHandler.EnumJson> list = new System.Collections.Generic.List<VsiteHandler.EnumJson>();
				foreach (System.Enum @enum in values)
				{
					list.Add(new VsiteHandler.EnumJson
					{
						Name = @enum.ToShowText(),
						Value = @enum.ToString()
					});
				}
				s = JsonConvert.SerializeObject(list);
				break;
			}
			case "ActivityList":
			{
				string value3 = context.Request.Form["acttype"];
				LotteryActivityType lotteryActivityType = (LotteryActivityType)System.Enum.Parse(typeof(LotteryActivityType), value3);
				if (lotteryActivityType == LotteryActivityType.SignUp)
				{
					var value4 = 
						from item in VShopHelper.GetAllActivity()
						select new
						{
							ActivityId = item.ActivityId,
							ActivityName = item.Name
						};
					s = JsonConvert.SerializeObject(value4);
				}
				else
				{
					System.Collections.Generic.IList<LotteryActivityInfo> lotteryActivityByType = VShopHelper.GetLotteryActivityByType(lotteryActivityType);
					s = JsonConvert.SerializeObject(lotteryActivityByType);
				}
				break;
			}
			case "ArticleCategory":
			{
				System.Collections.Generic.IList<ArticleCategoryInfo> articleMainCategories = CommentBrowser.GetArticleMainCategories();
				if (articleMainCategories != null && articleMainCategories.Count > 0)
				{
					System.Collections.Generic.List<VsiteHandler.EnumJson> list2 = new System.Collections.Generic.List<VsiteHandler.EnumJson>();
					foreach (ArticleCategoryInfo current in articleMainCategories)
					{
						list2.Add(new VsiteHandler.EnumJson
						{
							Name = current.Name,
							Value = current.CategoryId.ToString()
						});
					}
					s = JsonConvert.SerializeObject(list2);
				}
				break;
			}
			case "ArticleList":
			{
				int categoryId = 0;
				if (context.Request.Form["categoryId"] != null)
				{
					int.TryParse(context.Request.Form["categoryId"].ToString(), out categoryId);
				}
				System.Collections.Generic.IList<ArticleInfo> articleList = CommentBrowser.GetArticleList(categoryId, 1000);
				System.Collections.Generic.List<VsiteHandler.EnumJson> list3 = new System.Collections.Generic.List<VsiteHandler.EnumJson>();
				foreach (ArticleInfo current2 in articleList)
				{
					list3.Add(new VsiteHandler.EnumJson
					{
						Name = current2.Title,
						Value = current2.ArticleId.ToString()
					});
				}
				s = JsonConvert.SerializeObject(list3);
				break;
			}
			}
			context.Response.Write(s);
		}
	}
}
