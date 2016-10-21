using EcShop.ControlPanel.Store;
using EcShop.Entities.Store;
using EcShop.Entities.VShop;
using System;
namespace EcShop.UI.Web.Admin.vshop
{
	[PrivilegeCheck(Privilege.MutiArticleAdd)]
	public class ArticleList : NewsMsgInfo
	{
		public string BoxId
		{
			get;
			set;
		}
		public string Status
		{
			get;
			set;
		}
	}
}
