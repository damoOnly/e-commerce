using EcShop.Entities.Comments;
using EcShop.SaleSystem.Comments;
using EcShop.UI.Common.Controls;
using System;
using System.Collections.Generic;
namespace EcShop.UI.SaleSystem.Tags
{
	public class Common_AfficheList : ThemedTemplatedRepeater
	{
		private int maxNum;
		public int MaxNum
		{
			get
			{
				return this.maxNum;
			}
			set
			{
				this.maxNum = value;
			}
		}
		protected override void OnLoad(EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				base.DataSource = this.GetDataSource();
				base.DataBind();
			}
		}
		private IList<AfficheInfo> GetDataSource()
		{
			IList<AfficheInfo> afficheList = CommentBrowser.GetAfficheList();
			if (this.MaxNum > 0 && this.MaxNum < afficheList.Count)
			{
				for (int i = afficheList.Count - 1; i >= this.MaxNum; i--)
				{
					afficheList.RemoveAt(i);
				}
			}
			return afficheList;
		}
	}
}
