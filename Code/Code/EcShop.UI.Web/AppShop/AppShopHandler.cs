using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Core.Enums;
using EcShop.Entities;
using EcShop.Entities.Commodities;
using EcShop.Entities.Members;
using EcShop.Entities.Orders;
using EcShop.Entities.VShop;
using EcShop.Membership.Context;
using EcShop.Membership.Core;
using EcShop.Membership.Core.Enums;
using EcShop.Messages;
using EcShop.SaleSystem.Catalog;
using EcShop.SaleSystem.Member;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Linq;
namespace EcShop.UI.Web.AppShop
{
	public class AppShopHandler : System.Web.IHttpHandler
	{
		private SiteSettings siteSettings = SettingsManager.GetMasterSettings(true);
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
		public void ProcessRequest(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			string text = context.Request["action"];
			string key;
			switch (key = text)
			{
			case "appInit":
				this.ProcessAppInit(context);
				return;
			case "getDefaultData":
				this.GetDefaultData(context);
				return;
			case "getCategories":
				this.GetCategories(context);
				return;
			case "getAllCategories":
				this.GetAllCategories(context);
				return;
			case "getProducts":
				this.GetProducts(context);
				return;
			case "regiester":
				this.ProcessRegiester(context);
				return;
			case "login":
				this.ProcessLogin(context);
				return;
			case "logout":
				this.ProcessLogout(context);
				return;
			case "getMember":
				this.GetMember(context);
				break;

				return;
			}
		}
		private void ProcessLogout(System.Web.HttpContext context)
		{
			this.ClearLoginStatus();
		}
		private void ProcessAppInit(System.Web.HttpContext context)
		{
			string text = context.Request["VID"];
			string text2 = context.Request["device"];
			string text3 = context.Request["version"];
			string text4 = context.Request["isFirst"];
			if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(text2) || string.IsNullOrEmpty(text3) || string.IsNullOrEmpty(text4))
			{
				context.Response.Write(this.GetErrorJosn(101, "缺少必填参数"));
				return;
			}
			decimal num;
			if (!decimal.TryParse(text3, out num))
			{
				context.Response.Write(this.GetErrorJosn(102, "数字类型转换错误"));
				return;
			}
			if (text4.ToLower() == "true")
			{
				APPHelper.AddAppInstallRecord(new AppInstallRecordInfo
				{
					VID = text,
					Device = text2
				});
			}
			AppVersionRecordInfo appVersionRecordInfo = APPHelper.GetLatestAppVersionRecord(text2);
			if (appVersionRecordInfo == null)
			{
				appVersionRecordInfo = new AppVersionRecordInfo();
				appVersionRecordInfo.Version = num;
			}
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			stringBuilder.Append("{\"result\":{");
			stringBuilder.AppendFormat("\"version\":\"{0}\",", appVersionRecordInfo.Version);
			stringBuilder.AppendFormat("\"existNew\":\"{0}\",", (appVersionRecordInfo.Version > num).ToString().ToLower());
			stringBuilder.AppendFormat("\"forcible\":\"{0}\",", APPHelper.IsForcibleUpgrade(text2, num).ToString().ToLower());
			stringBuilder.AppendFormat("\"description\":\"{0}\",", appVersionRecordInfo.Description);
			stringBuilder.AppendFormat("\"upgradeUrl\":\"{0}\"", appVersionRecordInfo.UpgradeUrl);
			stringBuilder.Append("}}");
			context.Response.Write(stringBuilder.ToString());
		}
		private void GetDefaultData(System.Web.HttpContext context)
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			stringBuilder.Append("{\"result\":{\"advs\":[");
			System.Collections.Generic.IList<BannerInfo> allBanners = VShopHelper.GetAllBanners(ClientType.App);
			if (allBanners != null && allBanners.Count > 0)
			{
                allBanners = allBanners.Where(m => m.LocationType != LocationType.Register).ToList();
                if (allBanners != null && allBanners.Count > 0)
                {
                    foreach (BannerInfo current in allBanners)
                    {
                        stringBuilder.Append("{");
                        stringBuilder.AppendFormat("\"pic\":\"{0}\",", Globals.FullPath(current.ImageUrl));
                        stringBuilder.AppendFormat("\"description\":\"{0}\",", current.ShortDesc);
                        if (!string.IsNullOrEmpty(current.LoctionUrl))
                        {
                            stringBuilder.AppendFormat("\"url\":\"{0}\"", current.LoctionUrl);
                        }
                        else
                        {
                            stringBuilder.AppendFormat("\"url\":\"{0}\"", "javascript:;");
                        }
                        stringBuilder.Append("},");
                    }
                }
				stringBuilder.Remove(stringBuilder.Length - 1, 1);
			}
			stringBuilder.Append("],");
			stringBuilder.Append("\"navigates\":[");
			System.Collections.Generic.IList<NavigateInfo> allNavigate = VShopHelper.GetAllNavigate(ClientType.App);
			if (allNavigate != null && allNavigate.Count > 0)
			{
				foreach (NavigateInfo current2 in allNavigate)
				{
					stringBuilder.Append("{");
					stringBuilder.AppendFormat("\"pic\":\"{0}\",", Globals.FullPath(current2.ImageUrl));
					stringBuilder.AppendFormat("\"description\":\"{0}\",", current2.ShortDesc);
					if (!string.IsNullOrEmpty(current2.LoctionUrl))
					{
						stringBuilder.AppendFormat("\"url\":\"{0}\"", current2.LoctionUrl);
					}
					else
					{
						stringBuilder.AppendFormat("\"url\":\"{0}\"", "javascript:;");
					}
					stringBuilder.Append("},");
				}
				stringBuilder.Remove(stringBuilder.Length - 1, 1);
			}
			stringBuilder.Append("],");
			stringBuilder.Append("\"topics\":[");
			System.Data.DataTable homeTopics = VShopHelper.GetHomeTopics(ClientType.App);
			if (homeTopics != null && homeTopics.Rows.Count > 0)
			{
				foreach (System.Data.DataRow dataRow in homeTopics.Rows)
				{
					stringBuilder.Append("{");
					stringBuilder.AppendFormat("\"tid\":{0},", dataRow["TopicId"]);
					stringBuilder.AppendFormat("\"title\":\"{0}\",", dataRow["title"]);
					stringBuilder.AppendFormat("\"pic\":\"{0}\",", Globals.FullPath((string)dataRow["IconUrl"]));
					stringBuilder.AppendFormat("\"url\":\"{0}\"", Globals.FullPath(string.Format("/AppShop/Topics.aspx?TopicId={0}", dataRow["TopicId"])));
					stringBuilder.Append("},");
				}
				stringBuilder.Remove(stringBuilder.Length - 1, 1);
			}
			stringBuilder.Append("],");
			stringBuilder.Append("\"tagProducts\":[");
			System.Data.DataTable homeProducts = VShopHelper.GetHomeProducts(ClientType.App);
			if (homeProducts != null && homeProducts.Rows.Count > 0)
			{
				foreach (System.Data.DataRow dataRow2 in homeProducts.Rows)
				{
					stringBuilder.Append("{");
					stringBuilder.AppendFormat("\"pid\":{0},", dataRow2["ProductId"]);
					stringBuilder.AppendFormat("\"name\":\"{0}\",", dataRow2["ProductName"].ToString().Replace("\\", "").Replace("\\", ""));
					stringBuilder.AppendFormat("\"pic\":\"{0}\",", (dataRow2["ThumbnailUrl180"] != System.DBNull.Value) ? Globals.FullPath((string)dataRow2["ThumbnailUrl180"]) : Globals.FullPath(this.siteSettings.DefaultProductThumbnail5));
					stringBuilder.AppendFormat("\"price\":\"{0}\",", ((decimal)dataRow2["SalePrice"]).ToString("F2"));
					stringBuilder.AppendFormat("\"saleCounts\":\"{0}\",", ((int)dataRow2["ShowSaleCounts"]).ToString());
					stringBuilder.AppendFormat("\"url\":\"{0}\"", Globals.FullPath(string.Format("/AppShop/ProductDetails.aspx?productId={0}", dataRow2["ProductId"])));
					stringBuilder.Append("},");
				}
				stringBuilder.Remove(stringBuilder.Length - 1, 1);
			}
			stringBuilder.Append("]}}");
			context.Response.Write(stringBuilder.ToString());
		}
		private string GetSubCategoryNames(int parentCategoryId)
		{
			System.Collections.Generic.IList<CategoryInfo> subCategories = CategoryBrowser.GetSubCategories(parentCategoryId);
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			if (subCategories != null && subCategories.Count > 0)
			{
				foreach (CategoryInfo current in subCategories)
				{
					stringBuilder.Append(current.Name).Append(" ");
				}
				stringBuilder.Remove(stringBuilder.Length - 1, 1);
			}
			return stringBuilder.ToString();
		}
		private void GetCategories(System.Web.HttpContext context)
		{
			string text = context.Request["pid"];
			if (string.IsNullOrEmpty(text))
			{
				context.Response.Write(this.GetErrorJosn(101, "缺少必填参数"));
				return;
			}
			int num = 0;
			if (!int.TryParse(text, out num))
			{
				context.Response.Write(this.GetErrorJosn(102, "数字类型转换错误"));
				return;
			}
			System.Collections.Generic.IList<CategoryInfo> list;
			if (num == 0)
			{
				list = CategoryBrowser.GetMainCategories();
			}
			else
			{
				list = CategoryBrowser.GetSubCategories(num);
			}
			if (list == null || list.Count == 0)
			{
				context.Response.Write(this.GetErrorJosn(103, "没获取到相应的分类"));
				return;
			}
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			stringBuilder.Append("{\"result\":[");
			foreach (CategoryInfo current in list)
			{
				stringBuilder.Append("{");
				stringBuilder.AppendFormat("\"cid\":{0},", current.CategoryId);
				stringBuilder.AppendFormat("\"name\":\"{0}\",", current.Name);
				stringBuilder.AppendFormat("\"icon\":\"{0}\",", current.Icon);
				stringBuilder.AppendFormat("\"hasChildren\":\"{0}\",", current.HasChildren.ToString().ToLower());
				stringBuilder.AppendFormat("\"description\":\"{0}\"", this.GetSubCategoryNames(current.CategoryId));
				stringBuilder.Append("},");
			}
			stringBuilder.Remove(stringBuilder.Length - 1, 1);
			stringBuilder.Append("]}");
			context.Response.Write(stringBuilder.ToString());
		}
		private void GetAllCategories(System.Web.HttpContext context)
		{
			System.Collections.Generic.IList<CategoryInfo> mainCategories = CategoryBrowser.GetMainCategories();
			if (mainCategories == null || mainCategories.Count == 0)
			{
				context.Response.Write(this.GetErrorJosn(103, "没获取到相应的分类"));
				return;
			}
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			stringBuilder.Append("{\"result\":[");
			foreach (CategoryInfo current in mainCategories)
			{
				stringBuilder.Append("{");
				stringBuilder.AppendFormat("\"cid\":{0},", current.CategoryId);
				stringBuilder.AppendFormat("\"name\":\"{0}\",", current.Name);
				stringBuilder.AppendFormat("\"icon\":\"{0}\",", current.Icon);
				stringBuilder.AppendFormat("\"hasChildren\":\"{0}\",", current.HasChildren.ToString().ToLower());
				stringBuilder.AppendFormat("\"description\":\"{0}\",", this.GetSubCategoryNames(current.CategoryId));
				stringBuilder.Append("\"subs\":[");
				System.Collections.Generic.IList<CategoryInfo> subCategories = CategoryBrowser.GetSubCategories(current.CategoryId);
				if (subCategories != null && subCategories.Count > 0)
				{
					foreach (CategoryInfo current2 in subCategories)
					{
						stringBuilder.Append("{");
						stringBuilder.AppendFormat("\"cid\":{0},", current2.CategoryId);
						stringBuilder.AppendFormat("\"name\":\"{0}\",", current2.Name);
						stringBuilder.AppendFormat("\"icon\":\"{0}\",", current2.Icon);
						stringBuilder.AppendFormat("\"hasChildren\":\"{0}\",", current2.HasChildren.ToString().ToLower());
						stringBuilder.Append("\"subs\":[");
						System.Collections.Generic.IList<CategoryInfo> subCategories2 = CategoryBrowser.GetSubCategories(current2.CategoryId);
						if (subCategories2 != null && subCategories2.Count > 0)
						{
							foreach (CategoryInfo current3 in subCategories2)
							{
								stringBuilder.Append("{");
								stringBuilder.AppendFormat("\"cid\":{0},", current3.CategoryId);
								stringBuilder.AppendFormat("\"name\":\"{0}\",", current3.Name);
								stringBuilder.AppendFormat("\"icon\":\"{0}\"", current3.Icon);
								stringBuilder.Append("},");
							}
							stringBuilder.Remove(stringBuilder.Length - 1, 1);
						}
						stringBuilder.Append("]},");
					}
					stringBuilder.Remove(stringBuilder.Length - 1, 1);
				}
				stringBuilder.Append("]},");
			}
			stringBuilder.Remove(stringBuilder.Length - 1, 1);
			stringBuilder.Append("]}");
			context.Response.Write(stringBuilder.ToString());
		}
		private void GetProducts(System.Web.HttpContext context)
		{
			int pageIndex = 1;
			int pageSize = 10;
			if (!string.IsNullOrEmpty(context.Request["pageIndex"]))
			{
				int.TryParse(context.Request["pageIndex"], out pageIndex);
			}
			if (!string.IsNullOrEmpty(context.Request["pageSize"]))
			{
				int.TryParse(context.Request["pageSize"], out pageSize);
			}
			ProductBrowseQuery productBrowseQuery = new ProductBrowseQuery();
			productBrowseQuery.PageIndex = pageIndex;
			productBrowseQuery.PageSize = pageSize;
			if (!string.IsNullOrEmpty(context.Request["cId"]))
			{
				int num = 0;
				int.TryParse(context.Request["cId"], out num);
				if (num != 0)
				{
					productBrowseQuery.CategoryId = new int?(num);
				}
			}
			productBrowseQuery.Keywords = context.Request["keyword"];
			productBrowseQuery.SortBy = "DisplaySequence";
			productBrowseQuery.SortOrder = SortAction.Desc;
			if (!string.IsNullOrEmpty(context.Request["sortBy"]))
			{
				productBrowseQuery.SortBy = context.Request["sortBy"];
			}
			if (!string.IsNullOrEmpty(context.Request["sortOrder"]) && context.Request["sortOrder"] == "asc")
			{
				productBrowseQuery.SortOrder = SortAction.Asc;
			}
			DbQueryResult browseProductList = ProductBrowser.GetBrowseProductList(productBrowseQuery);
			System.Data.DataTable dataTable = (System.Data.DataTable)browseProductList.Data;
			if (dataTable == null || dataTable.Rows.Count == 0)
			{
				context.Response.Write(this.GetErrorJosn(103, "没获取到相应的商品"));
				return;
			}
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			stringBuilder.Append("{\"result\":{");
			stringBuilder.AppendFormat("\"totals\":{0},", browseProductList.TotalRecords);
			stringBuilder.Append("\"products\":[");
			foreach (System.Data.DataRow dataRow in dataTable.Rows)
			{
				stringBuilder.Append("{");
				stringBuilder.AppendFormat("\"pid\":{0},", dataRow["ProductId"]);
				stringBuilder.AppendFormat("\"name\":\"{0}\",", dataRow["ProductName"]);
				stringBuilder.AppendFormat("\"pic\":\"{0}\",", (dataRow["ThumbnailUrl60"] == System.DBNull.Value) ? Globals.FullPath(this.siteSettings.DefaultProductThumbnail4) : Globals.FullPath((string)dataRow["ThumbnailUrl60"]));
				stringBuilder.AppendFormat("\"price\":\"{0}\",", ((decimal)dataRow["SalePrice"]).ToString("F2"));
				stringBuilder.AppendFormat("\"saleCounts\":\"{0}\",", ((int)dataRow["SaleCounts"]).ToString());
				stringBuilder.AppendFormat("\"url\":\"{0}\"", Globals.FullPath(string.Format("/AppShop/ProductDetails.aspx?productId={0}", dataRow["ProductId"])));
				stringBuilder.Append("},");
			}
			stringBuilder.Remove(stringBuilder.Length - 1, 1);
			stringBuilder.Append("]}}");
			context.Response.Write(stringBuilder.ToString());
		}
		private void ProcessRegiester(System.Web.HttpContext context)
		{
			string text = context.Request["userName"];
			string text2 = context.Request["password"];
			string text3 = context.Request["email"];
			if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(text2) || string.IsNullOrEmpty(text3))
			{
				context.Response.Write(this.GetErrorJosn(101, "缺少必填参数"));
				return;
			}
			if (string.IsNullOrEmpty(text3.Trim()))
			{
				context.Response.Write(this.GetErrorJosn(203, "邮箱帐号不能为空"));
				return;
			}
			if (text3.Length > 256 || !System.Text.RegularExpressions.Regex.IsMatch(text3, "([a-zA-Z\\.0-9_-])+@([a-zA-Z0-9_-])+((\\.[a-zA-Z0-9_-]{2,4}){1,2})"))
			{
				context.Response.Write(this.GetErrorJosn(204, "错误的邮箱帐号"));
				return;
			}
			Member member = new Member(UserRole.Member);
			member.GradeId = MemberProcessor.GetDefaultMemberGrade();
			member.SessionId = Globals.GetGenerateId();
			member.Username = text;
			member.Email = text3;
			member.Password = text2;
			member.PasswordFormat = System.Web.Security.MembershipPasswordFormat.Hashed;
			member.TradePasswordFormat = System.Web.Security.MembershipPasswordFormat.Hashed;
			member.TradePassword = text2;
			member.IsApproved = true;
			member.RealName = string.Empty;
			member.Address = string.Empty;
			CreateUserStatus createUserStatus = MemberProcessor.CreateMember(member);
			if (createUserStatus == CreateUserStatus.DuplicateUsername || createUserStatus == CreateUserStatus.DisallowedUsername)
			{
				context.Response.Write(this.GetErrorJosn(201, "用户名重复"));
				return;
			}
			if (createUserStatus == CreateUserStatus.DuplicateEmailAddress)
			{
				context.Response.Write(this.GetErrorJosn(202, "邮件名重复"));
				return;
			}
			if (createUserStatus == CreateUserStatus.Created)
			{
				Messenger.UserRegister(member, text2);
				member.OnRegister(new UserEventArgs(member.Username, text2, null));
				System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
				stringBuilder.Append("{\"result\":{");
				stringBuilder.AppendFormat("\"uid\":{0},", member.UserId);
				stringBuilder.AppendFormat("\"sessionid\":\"{0}\"", member.SessionId);
				stringBuilder.Append("}}");
				context.Response.Write(stringBuilder.ToString());
				return;
			}
			context.Response.Write(this.GetErrorJosn(121, "注册用户失败"));
		}
		public void ClearLoginStatus()
		{
			try
			{
				System.Web.HttpCookie httpCookie = HiContext.Current.Context.Request.Cookies["Token_" + HiContext.Current.User.UserId.ToString()];
				if (httpCookie != null && !string.IsNullOrEmpty(httpCookie.Value))
				{
					httpCookie.Expires = System.DateTime.Now;
					System.Web.HttpContext.Current.Response.Cookies.Add(httpCookie);
				}
				System.Web.HttpCookie httpCookie2 = HiContext.Current.Context.Request.Cookies["Vshop-Member"];
				if (httpCookie2 != null && !string.IsNullOrEmpty(httpCookie2.Value))
				{
					httpCookie2.Expires = System.DateTime.Now;
					System.Web.HttpContext.Current.Response.Cookies.Add(httpCookie2);
				}
				if (System.Web.HttpContext.Current.Request.IsAuthenticated)
				{
					System.Web.Security.FormsAuthentication.SignOut();
					System.Web.HttpCookie authCookie = System.Web.Security.FormsAuthentication.GetAuthCookie(HiContext.Current.User.Username, true);
					IUserCookie userCookie = HiContext.Current.User.GetUserCookie();
					if (userCookie != null)
					{
						userCookie.DeleteCookie(authCookie);
					}
					RoleHelper.SignOut(HiContext.Current.User.Username);
					System.Web.HttpContext.Current.Response.Cookies["hishopLoginStatus"].Value = "";
				}
			}
			catch
			{
			}
		}
		private void ProcessLogin(System.Web.HttpContext context)
		{
			string text = context.Request["userName"];
			string text2 = context.Request["password"];
			if (HiContext.Current.User != null)
			{
				this.ClearLoginStatus();
			}
			if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(text2))
			{
				context.Response.Write(this.GetErrorJosn(101, "缺少必填参数"));
				return;
			}
			Member member = Users.GetUser(0, text, false, true) as Member;
			if (member == null)
			{
				context.Response.Write(this.GetErrorJosn(205, "用户名无效"));
				return;
			}
			member.Password = text2;
			LoginUserStatus loginUserStatus = MemberProcessor.ValidLogin(member);
			if (loginUserStatus != LoginUserStatus.Success)
			{
				context.Response.Write(this.GetErrorJosn(206, "密码有误"));
				return;
			}
			System.Web.HttpCookie authCookie = System.Web.Security.FormsAuthentication.GetAuthCookie(member.Username, false);
			IUserCookie userCookie = member.GetUserCookie();
			userCookie.WriteCookie(authCookie, 30, false);
			System.Web.HttpCookie httpCookie = new System.Web.HttpCookie("Vshop-Member");
			httpCookie.Value = Globals.UrlEncode(member.Username);
			System.Web.HttpContext.Current.Response.Cookies.Add(httpCookie);
			HiContext.Current.User = member;
			member.OnLogin();
			string text3 = UserHelper.UpdateSessionId(member.UserId);
			member.SessionId = text3;
			Users.UpdateUser(member);
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			stringBuilder.Append("{\"result\":{");
			stringBuilder.AppendFormat("\"uid\":{0},", member.UserId);
			stringBuilder.AppendFormat("\"sessionid\":\"{0}\"", text3);
			stringBuilder.Append("}}");
			context.Response.Write(stringBuilder.ToString());
		}
		private void GetMember(System.Web.HttpContext context)
		{
			string text = context.Request["sessionid"];
			if (string.IsNullOrEmpty(text))
			{
				context.Response.Write(this.GetErrorJosn(101, "缺少必填参数"));
				return;
			}
			Member member = Users.GetUserBySessionId(text) as Member;
			if (member == null)
			{
				context.Response.Write(this.GetErrorJosn(107, "sessionid过期或不存在，请重新登录"));
				return;
			}
            string name = "Vshop-Member";
            HttpCookie httpCookie = new HttpCookie("Vshop-Member");
            httpCookie.Value = Globals.UrlEncode(member.Username);
            httpCookie.Expires = System.DateTime.Now.AddDays(7);
            httpCookie.Domain = HttpContext.Current.Request.Url.Host;
            if (HttpContext.Current.Response.Cookies[name] != null)
            {
                HttpContext.Current.Response.Cookies.Remove(name);
            }
            HttpContext.Current.Response.Cookies.Add(httpCookie);

			HiContext.Current.User = member;
			member.OnLogin();
			MemberGradeInfo memberGrade = MemberProcessor.GetMemberGrade(member.GradeId);
			string arg = (memberGrade == null) ? "" : memberGrade.Name;
			OrderQuery orderQuery = new OrderQuery();
			orderQuery.Status = OrderStatus.WaitBuyerPay;
			int userOrderCount = MemberProcessor.GetUserOrderCount(member.UserId, orderQuery);
			orderQuery.Status = OrderStatus.SellerAlreadySent;
			int userOrderCount2 = MemberProcessor.GetUserOrderCount(member.UserId, orderQuery);
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			stringBuilder.Append("{\"result\":{");
			stringBuilder.AppendFormat("\"uid\":{0},", member.UserId);
			stringBuilder.AppendFormat("\"sessionid\":\"{0}\",", member.SessionId);
			stringBuilder.AppendFormat("\"gradeName\":\"{0}\",", arg);
			stringBuilder.AppendFormat("\"realName\":\"{0}\",", string.IsNullOrEmpty(member.RealName) ? member.Username : member.RealName);
			stringBuilder.AppendFormat("\"waitPayCount\":\"{0}\",", userOrderCount);
			stringBuilder.AppendFormat("\"waitFinishCount\":\"{0}\",", userOrderCount2);
			stringBuilder.AppendFormat("\"orderNumber\":\"{0}\",", member.OrderNumber);
			stringBuilder.AppendFormat("\"expenditure\":\"{0}\",", member.Expenditure.ToString("F2"));
			stringBuilder.AppendFormat("\"points\":\"{0}\"", member.Points);
			stringBuilder.Append("}}");
			context.Response.Write(stringBuilder.ToString());
		}
		private string GetErrorJosn(int errorCode, string errorMsg)
		{
			return "{\"error_response\":{\"errorCode\":" + string.Format("{0}, \"errorMsg\":\"{1}\"", errorCode, errorMsg) + "}}";
		}
	}
}
