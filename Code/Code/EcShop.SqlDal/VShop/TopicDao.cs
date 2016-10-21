using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Entities;
using EcShop.Entities.VShop;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
namespace EcShop.SqlDal.VShop
{
	public class TopicDao
	{
		private Database database;
		public TopicDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public int AddTopic(TopicInfo topicinfo)
		{
			StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("insert into Vshop_Topics(Title,IconUrl,Content,AddedDate,IsRelease,DisplaySequence,SupplierId,MobileListImageUrl,MobileBannerImageUrl,PCListImageUrl,PCBannerImageUrl)");
            stringBuilder.Append(" values (@Title,@IconUrl,@Content,@AddedDate,@IsRelease,@DisplaySequence,@SupplierId,@MobileListImageUrl,@MobileBannerImageUrl,@PCListImageUrl,@PCBannerImageUrl);");
			stringBuilder.Append(" select @@IDENTITY");
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "Title", DbType.String, topicinfo.Title);
			this.database.AddInParameter(sqlStringCommand, "IconUrl", DbType.String, topicinfo.IconUrl);
			this.database.AddInParameter(sqlStringCommand, "Content", DbType.String, topicinfo.Content);
			this.database.AddInParameter(sqlStringCommand, "AddedDate", DbType.DateTime, topicinfo.AddedDate);
			this.database.AddInParameter(sqlStringCommand, "IsRelease", DbType.Boolean, topicinfo.IsRelease);
			this.database.AddInParameter(sqlStringCommand, "DisplaySequence", DbType.Int32, topicinfo.DisplaySequence);
			this.database.AddInParameter(sqlStringCommand, "Keys", DbType.String, topicinfo.Keys);
            this.database.AddInParameter(sqlStringCommand, "SupplierId", DbType.Int32, topicinfo.SupplierId);
            this.database.AddInParameter(sqlStringCommand, "MobileListImageUrl", DbType.String, topicinfo.MobileListImageUrl);
            this.database.AddInParameter(sqlStringCommand, "MobileBannerImageUrl", DbType.String, topicinfo.MobileBannerImageUrl);
            this.database.AddInParameter(sqlStringCommand, "PCListImageUrl", DbType.String, topicinfo.PCListImageUrl);
            this.database.AddInParameter(sqlStringCommand, "PCBannerImageUrl", DbType.String, topicinfo.PCBannerImageUrl);
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			int result;
			int.TryParse(obj.ToString(), out result);
			return result;
		}
		public bool UpdateTopic(TopicInfo topic)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("update Vshop_Topics set ");
			stringBuilder.Append("Title=@Title,");
			stringBuilder.Append("IconUrl=@IconUrl,");
            stringBuilder.Append("MobileListImageUrl=@MobileListImageUrl,");
            stringBuilder.Append("MobileBannerImageUrl=@MobileBannerImageUrl,");
            stringBuilder.Append("PCListImageUrl=@PCListImageUrl,");
            stringBuilder.Append("PCBannerImageUrl=@PCBannerImageUrl,");
			stringBuilder.Append("Content=@Content,");
			stringBuilder.Append("AddedDate=@AddedDate,");
			stringBuilder.Append("DisplaySequence=@DisplaySequence,");
			stringBuilder.Append("IsRelease=@IsRelease");
			stringBuilder.Append(" where TopicId=@TopicId ");
			stringBuilder.Append(" UPDATE vshop_Reply SET Keys = @Keys WHERE  ActivityId = @TopicId  AND [ReplyType] = @ReplyType");
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "TopicId", DbType.Int32, topic.TopicId);
			this.database.AddInParameter(sqlStringCommand, "Title", DbType.String, topic.Title);
			this.database.AddInParameter(sqlStringCommand, "IconUrl", DbType.String, topic.IconUrl);
            this.database.AddInParameter(sqlStringCommand, "MobileListImageUrl", DbType.String, topic.MobileListImageUrl);
            this.database.AddInParameter(sqlStringCommand, "MobileBannerImageUrl", DbType.String, topic.MobileBannerImageUrl);
            this.database.AddInParameter(sqlStringCommand, "PCListImageUrl", DbType.String, topic.PCListImageUrl);
            this.database.AddInParameter(sqlStringCommand, "PCBannerImageUrl", DbType.String, topic.PCBannerImageUrl);
			this.database.AddInParameter(sqlStringCommand, "Content", DbType.String, topic.Content);
			this.database.AddInParameter(sqlStringCommand, "AddedDate", DbType.DateTime, topic.AddedDate);
			this.database.AddInParameter(sqlStringCommand, "DisplaySequence", DbType.Int32, topic.DisplaySequence);
			this.database.AddInParameter(sqlStringCommand, "IsRelease", DbType.Boolean, topic.IsRelease);
			this.database.AddInParameter(sqlStringCommand, "Keys", DbType.String, topic.Keys);
			this.database.AddInParameter(sqlStringCommand, "ReplyType", DbType.Int32, 512);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public bool DeleteTopic(int TopicId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("delete from Vshop_Topics where TopicId=@TopicId ");
			stringBuilder.Append(" DELETE FROM vshop_Reply WHERE ActivityId = @TopicId  AND [ReplyType] = @ReplyType");
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "TopicId", DbType.Int32, TopicId);
			this.database.AddInParameter(sqlStringCommand, "ReplyType", DbType.Int32, 512);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public TopicInfo GetTopic(int TopicId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("select *, (SELECT Keys FROM vshop_Reply WHERE [ReplyType] = @ReplyType AND ActivityId = t.TopicId) AS Keys from Vshop_Topics t where TopicId=@TopicId");
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "TopicId", DbType.Int32, TopicId);
			this.database.AddInParameter(sqlStringCommand, "ReplyType", DbType.Int32, 512);
			TopicInfo result = null;
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<TopicInfo>(dataReader);
			}
			return result;
		}
		public DbQueryResult GetTopicList(TopicQuery page)
		{
			StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(" 1=1 ");
			if (page.IsRelease.HasValue && page.IsRelease.Value)
			{
				stringBuilder.Append(" and IsRelease = 1");
			}
			else
			{
				if (page.IsRelease.HasValue && !page.IsRelease.Value)
				{
					stringBuilder.Append(" and IsRelease = 0");
				}
			}

            if (page.SupplierId.HasValue)
            {
                stringBuilder.AppendFormat(" and SupplierId = {0} ", page.SupplierId.Value);
            }

            else
            {
                stringBuilder.Append(" and SupplierId=0 ");
            }


			if (page.IsincludeHomeProduct.HasValue)
			{
				if (!page.IsincludeHomeProduct.Value)
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.Append(" and ");
					}
					if (!page.Client.HasValue)
					{
						stringBuilder.Append(" topicid  not in (select topicid from Vshop_HomeTopics) ");
					}
					else
					{
						stringBuilder.Append(" topicid  not in (select topicid from Vshop_HomeTopics where Client=" + page.Client.Value + " ) ");
					}
				}
			}

            
			int num = 512;
			return DataHelper.PagingByRownumber(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "Vshop_Topics t", "TopicId", stringBuilder.ToString(), "*, (SELECT Keys FROM vshop_Reply WHERE [ReplyType] =" + num + " AND Activityid = t.TopicId) AS Keys");
		}
		public IList<TopicInfo> GetTopics()
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select * from Vshop_Topics order by DisplaySequence asc,topicid desc");
			IList<TopicInfo> result = null;
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToList<TopicInfo>(dataReader);
			}
			return result;
		}
		public int DeleteTopics(IList<int> Topics)
		{
			int num = 0;
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Vshop_Topics WHERE TopicId=@TopicId");
			this.database.AddInParameter(sqlStringCommand, "TopicId", DbType.Int32);
			foreach (int current in Topics)
			{
				this.database.SetParameterValue(sqlStringCommand, "TopicId", current);
				this.database.ExecuteNonQuery(sqlStringCommand);
				num++;
			}
			return num;
		}
		public DataTable GetRelatedTopicProducts(int topicid,bool verfiy=true)
		{
			StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("select ProductId, ProductCode, ProductName, ThumbnailUrl40, MarketPrice, SalePrice, Stock,t.DisplaySequence from vw_Ecshop_CDisableBrowseProductList p inner join  Vshop_RelatedTopicProducts t on p.productid=t.RelatedProductId where t.Topicid=@Topicid");
            if (verfiy)
            {
               stringBuilder.AppendFormat(" and SaleStatus = {0}", 1);
            }
			stringBuilder.Append(" order by t.DisplaySequence asc");
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "Topicid", DbType.Int32, topicid);
			return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
		}
		public bool AddReleatesProdcutBytopicid(int topicid, int prodcutId)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Vshop_RelatedTopicProducts(Topicid, RelatedProductId,DisplaySequence) VALUES (@Topicid, @RelatedProductId,0)");
			this.database.AddInParameter(sqlStringCommand, "Topicid", DbType.Int32, topicid);
			this.database.AddInParameter(sqlStringCommand, "RelatedProductId", DbType.Int32, prodcutId);
			bool result;
			try
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
			}
			catch
			{
				result = false;
			}
			return result;
		}
		public bool RemoveReleatesProductBytopicid(int topicid, int productId)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Vshop_RelatedTopicProducts WHERE Topicid = @Topicid AND RelatedProductId = @RelatedProductId");
			this.database.AddInParameter(sqlStringCommand, "Topicid", DbType.Int32, topicid);
			this.database.AddInParameter(sqlStringCommand, "RelatedProductId", DbType.Int32, productId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public bool RemoveReleatesProductBytopicid(int topicid)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Vshop_RelatedTopicProducts WHERE Topicid = @Topicid");
			this.database.AddInParameter(sqlStringCommand, "Topicid", DbType.Int32, topicid);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public bool SwapTopicSequence(int TopicId, int displaysequence)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update Vshop_Topics  set DisplaySequence=@DisplaySequence where TopicId=@TopicId");
			this.database.AddInParameter(sqlStringCommand, "@DisplaySequence", DbType.Int32, displaysequence);
			this.database.AddInParameter(sqlStringCommand, "@TopicId", DbType.Int32, TopicId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public bool UpdateRelateProductSequence(int TopicId, int RelatedProductId, int displaysequence)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update Vshop_RelatedTopicProducts  set DisplaySequence=@DisplaySequence where TopicId=@TopicId and RelatedProductId=@RelatedProductId");
			this.database.AddInParameter(sqlStringCommand, "@DisplaySequence", DbType.Int32, displaysequence);
			this.database.AddInParameter(sqlStringCommand, "@TopicId", DbType.Int32, TopicId);
			this.database.AddInParameter(sqlStringCommand, "@RelatedProductId", DbType.Int32, RelatedProductId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
	}
}
