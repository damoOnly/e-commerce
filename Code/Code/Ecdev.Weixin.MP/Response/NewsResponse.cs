using Ecdev.Weixin.MP.Domain;
using System;
using System.Collections.Generic;
namespace Ecdev.Weixin.MP.Response
{
	public class NewsResponse : AbstractResponse
	{
		public int ArticleCount
		{
			get
			{
				return (this.Articles == null) ? 0 : this.Articles.Count;
			}
		}
		public IList<Article> Articles
		{
			get;
			set;
		}
		public override ResponseMsgType MsgType
		{
			get
			{
				return ResponseMsgType.News;
			}
		}
	}
}
