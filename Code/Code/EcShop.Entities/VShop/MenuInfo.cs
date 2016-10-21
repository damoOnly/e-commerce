using System;
using System.Collections.Generic;
using System.Web;
namespace EcShop.Entities.VShop
{
	public class MenuInfo
	{
		public int MenuId
		{
			get;
			set;
		}
		public int ParentMenuId
		{
			get;
			set;
		}
		public string Name
		{
			get;
			set;
		}
		public string Type
		{
			get;
			set;
		}
		public int ReplyId
		{
			get;
			set;
		}
		public int DisplaySequence
		{
			get;
			set;
		}
		public System.Collections.Generic.IList<MenuInfo> Chilren
		{
			get;
			set;
		}
		public ClientType Client
		{
			get;
			set;
		}
		public int Bind
		{
			get;
			set;
		}
		public BindType BindType
		{
			get
			{
				BindType result;
				switch (this.Bind)
				{
				case 0:
					result = BindType.None;
					break;
				case 1:
					result = BindType.Key;
					break;
				case 2:
					result = BindType.Topic;
					break;
				case 3:
					result = BindType.HomePage;
					break;
				case 4:
					result = BindType.ProductCategory;
					break;
				case 5:
					result = BindType.ShoppingCar;
					break;
				case 6:
					result = BindType.OrderCenter;
					break;
				case 7:
					result = BindType.MemberCard;
					break;
				case 8:
					result = BindType.Url;
					break;
				default:
					result = BindType.None;
					break;
				}
				return result;
			}
		}
		public string Content
		{
			get;
			set;
		}
		public virtual string BindTypeName
		{
			get
			{
				string result;
				switch (this.BindType)
				{
				case BindType.Key:
					result = "关键字";
					return result;
				case BindType.Topic:
					result = "专题";
					return result;
				case BindType.HomePage:
					result = "首页";
					return result;
				case BindType.ProductCategory:
					result = "分类页";
					return result;
				case BindType.ShoppingCar:
					result = "购物车";
					return result;
				case BindType.OrderCenter:
					result = "会员中心";
					return result;
				case BindType.MemberCard:
					result = "会员卡";
					return result;
				case BindType.Url:
					result = "自定义链接";
					return result;
				}
				result = string.Empty;
				return result;
			}
		}
		public virtual string Url
		{
			get
			{
				string host = HttpContext.Current.Request.Url.Host;
				string text = (this.Client == ClientType.VShop) ? "Vshop" : "AliOH";
				string result;
				switch (this.BindType)
				{
				case BindType.Key:
					result = this.ReplyId.ToString();
					return result;
				case BindType.Topic:
					result = string.Format("http://{0}/{2}/Topics.aspx?TopicId={1}", host, this.Content, text);
					return result;
				case BindType.HomePage:
					result = string.Format("http://{0}/{1}/Default.aspx", host, text);
					return result;
				case BindType.ProductCategory:
					result = string.Format("http://{0}/{1}/ProductSearch.aspx", host, text);
					return result;
				case BindType.ShoppingCar:
					result = string.Format("http://{0}/{1}/ShoppingCart.aspx", host, text);
					return result;
				case BindType.OrderCenter:
					result = string.Format("http://{0}/{1}/MemberCenter.aspx", host, text);
					return result;
				case BindType.MemberCard:
					result = string.Format("http://{0}/{1}/MemberCard.aspx", host, text);
					return result;
				case BindType.Url:
					result = this.Content;
					return result;
				}
				result = string.Empty;
				return result;
			}
		}
	}
}
