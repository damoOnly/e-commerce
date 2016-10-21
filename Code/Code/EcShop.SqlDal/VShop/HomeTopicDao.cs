using EcShop.Entities;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;
using System.Text;
namespace EcShop.SqlDal.VShop
{
    public class HomeTopicDao
    {
        private Database database;
        public HomeTopicDao()
        {
            this.database = DatabaseFactory.CreateDatabase();
        }
        public bool AddHomeTopic(int TopicId, ClientType client)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Vshop_HomeTopics(TopicId,DisplaySequence,Client) VALUES (@TopicId,0,@Client)");
            this.database.AddInParameter(sqlStringCommand, "TopicId", DbType.Int32, TopicId);
            this.database.AddInParameter(sqlStringCommand, "Client", DbType.Int32, (int)client);
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

        public bool AddHomeTopic(int TopicId, ClientType client, int supplierId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Vshop_HomeTopics(TopicId,DisplaySequence,Client,supplierId) VALUES (@TopicId,0,@Client,@supplierId)");
            this.database.AddInParameter(sqlStringCommand, "TopicId", DbType.Int32, TopicId);
            this.database.AddInParameter(sqlStringCommand, "Client", DbType.Int32, (int)client);
            this.database.AddInParameter(sqlStringCommand, "supplierId", DbType.Int32, supplierId);

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

        public bool RemoveHomeTopic(int TopicId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Vshop_HomeTopics WHERE TopicId = @TopicId");
            this.database.AddInParameter(sqlStringCommand, "TopicId", DbType.Int32, TopicId);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
        public bool RemoveHomeTopic(int TopicId, ClientType client)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Vshop_HomeTopics WHERE TopicId = @TopicId and Client=@Client");
            this.database.AddInParameter(sqlStringCommand, "TopicId", DbType.Int32, TopicId);
            this.database.AddInParameter(sqlStringCommand, "Client", DbType.Int32, (int)client);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }

        public bool RemoveHomeTopic(int TopicId, ClientType client, int supplierId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Vshop_HomeTopics WHERE TopicId = @TopicId and Client=@Client and supplierId=@supplierId");
            this.database.AddInParameter(sqlStringCommand, "TopicId", DbType.Int32, TopicId);
            this.database.AddInParameter(sqlStringCommand, "Client", DbType.Int32, (int)client);
            this.database.AddInParameter(sqlStringCommand, "supplierId", DbType.Int32, supplierId);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }


        public bool RemoveAllHomeTopics(ClientType client)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("DELETE FROM Vshop_HomeTopics where Client = {0}", (int)client));
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }

        public bool RemoveAllHomeTopics(ClientType client, int supplierId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("DELETE FROM Vshop_HomeTopics where Client = {0} and supplierId={1}", (int)client, supplierId));
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }

        public DataTable GetHomeTopics(ClientType client)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("select a.TopicId,[Title],[IconUrl],t.DisplaySequence,[AddedDate],[Content],Keys=(SELECT Keys FROM vshop_Reply WHERE [ReplyType] = @ReplyType AND ActivityId = t.TopicId)  from Vshop_Topics a inner join  Vshop_HomeTopics t on a.TopicId=t.TopicId  where a.IsRelease=1 and t.Client = @Client");
            
            //若不是供应商，则supplierid=0 或 null
            stringBuilder.Append(" and (t.SupplierId=0 or t.SupplierId is null) ");
            stringBuilder.Append(" order by t.DisplaySequence asc");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            this.database.AddInParameter(sqlStringCommand, "ReplyType", DbType.Int32, 512);
            this.database.AddInParameter(sqlStringCommand, "Client", DbType.Int32, (int)client);
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        public DataTable GetHomeActivityTopics(ClientType client,string topicids)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("select a.TopicId,[Title],[IconUrl],t.DisplaySequence,[AddedDate],[Content],Keys=(SELECT Keys FROM vshop_Reply WHERE [ReplyType] = @ReplyType AND ActivityId = t.TopicId)  from Vshop_Topics a inner join  Vshop_HomeTopics t on a.TopicId=t.TopicId  where a.IsRelease=1 and t.Client = @Client");
            if (!string.IsNullOrWhiteSpace(topicids))
            {
                stringBuilder.AppendFormat(" and a.TopicId in ({0})", topicids);
            }
            //若不是供应商，则supplierid=0 或 null
            stringBuilder.Append(" and (t.SupplierId=0 or t.SupplierId is null) ");
            stringBuilder.Append(" order by t.DisplaySequence asc");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            this.database.AddInParameter(sqlStringCommand, "ReplyType", DbType.Int32, 512);
            this.database.AddInParameter(sqlStringCommand, "Client", DbType.Int32, (int)client);
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        public DataTable GetHomeTopics(ClientType client, int supplierId)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("select a.TopicId,[Title],[IconUrl],t.DisplaySequence,[AddedDate],[Content],Keys=(SELECT Keys FROM vshop_Reply WHERE [ReplyType] = @ReplyType AND ActivityId = t.TopicId)  from Vshop_Topics a inner join  Vshop_HomeTopics t on a.TopicId=t.TopicId  where a.IsRelease=1 and t.Client = @Client and t.supplierId=@supplierId");
            stringBuilder.Append(" order by t.DisplaySequence asc");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            this.database.AddInParameter(sqlStringCommand, "ReplyType", DbType.Int32, 512);
            this.database.AddInParameter(sqlStringCommand, "Client", DbType.Int32, (int)client);
            this.database.AddInParameter(sqlStringCommand, "supplierId", DbType.Int32, supplierId);

            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        public DataSet GetHomeTopicsList(ClientType client, int pageSize, int pageIndex, ref int total)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("select * from ( select ROW_NUMBER() OVER(order by t.DisplaySequence asc) as no, a.TopicId,[Title],[IconUrl],t.DisplaySequence,[AddedDate],Keys=(SELECT Keys FROM vshop_Reply WHERE [ReplyType] = @ReplyType AND ActivityId = t.TopicId)  from Vshop_Topics a inner join  Vshop_HomeTopics t on a.TopicId=t.TopicId  where a.IsRelease=1 and t.Client = @Client) as AA  WHERE AA.no >@pageIndex*@pageSize AND AA.no<=(@pageIndex+1)*@pageSize; select count(1)  from Vshop_Topics a inner join  Vshop_HomeTopics t on a.TopicId=t.TopicId  where a.IsRelease=1 and t.Client = @Client");
            //若不是供应商，则supplierid=0 或 null
            stringBuilder.Append(" and (t.SupplierId=0 or t.SupplierId is null) ");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            this.database.AddInParameter(sqlStringCommand, "ReplyType", DbType.Int32, 512);
            this.database.AddInParameter(sqlStringCommand, "Client", DbType.Int32, (int)client);
            this.database.AddInParameter(sqlStringCommand, "pageIndex", DbType.Int32, pageIndex);
            this.database.AddInParameter(sqlStringCommand, "pageSize", DbType.Int32, pageSize);
            return this.database.ExecuteDataSet(sqlStringCommand);
        }
        public bool UpdateHomeTopicSequence(int TopicId, int displaysequence, ClientType clientType)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update Vshop_HomeTopics  set DisplaySequence=@DisplaySequence where TopicId=@TopicId AND Client=@Client");
            
            this.database.AddInParameter(sqlStringCommand, "@DisplaySequence", DbType.Int32, displaysequence);
            this.database.AddInParameter(sqlStringCommand, "@TopicId", DbType.Int32, TopicId);
            this.database.AddInParameter(sqlStringCommand, "@Client", DbType.Int32, (int)clientType);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
    }
}
