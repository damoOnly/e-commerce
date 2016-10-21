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
	public class ReplyDao
	{
		private Database database;
		public ReplyDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public MessageInfo GetMessage(int messageId)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Vshop_Message WHERE MsgID =@MsgID");
			this.database.AddInParameter(sqlStringCommand, "MsgID", DbType.Int32, messageId);
			MessageInfo result = null;
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<MessageInfo>(dataReader);
			}
			return result;
		}
		public void DeleteNewsMsg(int id)
		{
			StringBuilder stringBuilder = new StringBuilder(" delete from vshop_Message where MsgID=@MsgID");
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "MsgID", DbType.Int32, id);
			this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public ReplyInfo GetReply(int id)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM vshop_Reply WHERE ReplyId = @ReplyId");
			this.database.AddInParameter(sqlStringCommand, "ReplyId", DbType.Int32, id);
			ReplyInfo replyInfo = null;
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					replyInfo = this.ReaderBind(dataReader);
					switch (replyInfo.MessageType)
					{
					case MessageType.Text:
					{
						TextReplyInfo textReplyInfo = replyInfo as TextReplyInfo;
						object obj = dataReader["Content"];
						if (obj != null && obj != DBNull.Value)
						{
							textReplyInfo.Text = obj.ToString();
						}
						replyInfo = textReplyInfo;
						break;
					}
					case MessageType.News:
					case MessageType.List:
					{
						NewsReplyInfo newsReplyInfo = replyInfo as NewsReplyInfo;
						newsReplyInfo.NewsMsg = this.GetNewsReplyInfo(newsReplyInfo.Id);
						replyInfo = newsReplyInfo;
						break;
					}
					}
				}
			}
			return replyInfo;
		}
		public bool DeleteReply(int id)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE vshop_Reply WHERE ReplyId = @ReplyId;DELETE vshop_Message WHERE ReplyId = @ReplyId");
			this.database.AddInParameter(sqlStringCommand, "ReplyId", DbType.Int32, id);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public bool SaveReply(ReplyInfo reply)
		{
			bool result = false;
			switch (reply.MessageType)
			{
			case MessageType.Text:
				result = this.SaveTextReply(reply as TextReplyInfo);
				break;
			case MessageType.News:
			case MessageType.List:
				result = this.SaveNewsReply(reply as NewsReplyInfo);
				break;
			}
			return result;
		}
		private bool SaveNewsReply(NewsReplyInfo model)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("insert into vshop_Reply(");
			stringBuilder.Append("Keys,MatchType,ReplyType,MessageType,IsDisable,LastEditDate,LastEditor,Content,Type)");
			stringBuilder.Append(" values (");
			stringBuilder.Append("@Keys,@MatchType,@ReplyType,@MessageType,@IsDisable,@LastEditDate,@LastEditor,@Content,@Type)");
			stringBuilder.Append(";select @@IDENTITY");
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "Keys", DbType.String, model.Keys);
			this.database.AddInParameter(sqlStringCommand, "MatchType", DbType.Int32, (int)model.MatchType);
			this.database.AddInParameter(sqlStringCommand, "ReplyType", DbType.Int32, (int)model.ReplyType);
			this.database.AddInParameter(sqlStringCommand, "MessageType", DbType.Int32, (int)model.MessageType);
			this.database.AddInParameter(sqlStringCommand, "IsDisable", DbType.Boolean, model.IsDisable);
			this.database.AddInParameter(sqlStringCommand, "LastEditDate", DbType.DateTime, model.LastEditDate);
			this.database.AddInParameter(sqlStringCommand, "LastEditor", DbType.String, model.LastEditor);
			this.database.AddInParameter(sqlStringCommand, "Content", DbType.String, "");
			this.database.AddInParameter(sqlStringCommand, "Type", DbType.Int32, 2);
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			int num;
			if (int.TryParse(obj.ToString(), out num))
			{
				foreach (NewsMsgInfo current in model.NewsMsg)
				{
					stringBuilder = new StringBuilder();
					stringBuilder.Append("insert into vshop_Message(");
					stringBuilder.Append("ReplyId,Title,ImageUrl,Url,Description,Content)");
					stringBuilder.Append(" values (");
					stringBuilder.Append("@ReplyId,@Title,@ImageUrl,@Url,@Description,@Content)");
					sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
					this.database.AddInParameter(sqlStringCommand, "ReplyId", DbType.Int32, num);
					this.database.AddInParameter(sqlStringCommand, "Title", DbType.String, current.Title);
					this.database.AddInParameter(sqlStringCommand, "ImageUrl", DbType.String, current.PicUrl);
					this.database.AddInParameter(sqlStringCommand, "Url", DbType.String, current.Url);
					this.database.AddInParameter(sqlStringCommand, "Description", DbType.String, current.Description);
					this.database.AddInParameter(sqlStringCommand, "Content", DbType.String, current.Content);
					this.database.ExecuteNonQuery(sqlStringCommand);
				}
			}
			return true;
		}
		private bool SaveTextReply(TextReplyInfo model)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("insert into vshop_Reply(");
			stringBuilder.Append("Keys,MatchType,ReplyType,MessageType,IsDisable,LastEditDate,LastEditor,Content,Type,ActivityId)");
			stringBuilder.Append(" values (");
			stringBuilder.Append("@Keys,@MatchType,@ReplyType,@MessageType,@IsDisable,@LastEditDate,@LastEditor,@Content,@Type,@ActivityId)");
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "Keys", DbType.String, model.Keys);
			this.database.AddInParameter(sqlStringCommand, "MatchType", DbType.Int32, (int)model.MatchType);
			this.database.AddInParameter(sqlStringCommand, "ReplyType", DbType.Int32, (int)model.ReplyType);
			this.database.AddInParameter(sqlStringCommand, "MessageType", DbType.Int32, (int)model.MessageType);
			this.database.AddInParameter(sqlStringCommand, "IsDisable", DbType.Boolean, model.IsDisable);
			this.database.AddInParameter(sqlStringCommand, "LastEditDate", DbType.DateTime, model.LastEditDate);
			this.database.AddInParameter(sqlStringCommand, "LastEditor", DbType.String, model.LastEditor);
			this.database.AddInParameter(sqlStringCommand, "Content", DbType.String, model.Text);
			this.database.AddInParameter(sqlStringCommand, "Type", DbType.Int32, 1);
			this.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, model.ActivityId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public bool UpdateReply(ReplyInfo reply)
		{
			bool result;
			switch (reply.MessageType)
			{
			case MessageType.Text:
				result = this.UpdateTextReply(reply as TextReplyInfo);
				return result;
			case MessageType.News:
			case MessageType.List:
				result = this.UpdateNewsReply(reply as NewsReplyInfo);
				return result;
			}
			result = this.UpdateTextReply(reply as TextReplyInfo);
			return result;
		}
		private bool UpdateNewsReply(NewsReplyInfo reply)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("update vshop_Reply set ");
			stringBuilder.Append("Keys=@Keys,");
			stringBuilder.Append("MatchType=@MatchType,");
			stringBuilder.Append("ReplyType=@ReplyType,");
			stringBuilder.Append("MessageType=@MessageType,");
			stringBuilder.Append("IsDisable=@IsDisable,");
			stringBuilder.Append("LastEditDate=@LastEditDate,");
			stringBuilder.Append("LastEditor=@LastEditor,");
			stringBuilder.Append("Content=@Content,");
			stringBuilder.Append("Type=@Type");
			stringBuilder.Append(" where ReplyId=@ReplyId;delete from vshop_Message ");
			stringBuilder.Append(" where ReplyId=@ReplyId ");
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "Keys", DbType.String, reply.Keys);
			this.database.AddInParameter(sqlStringCommand, "MatchType", DbType.Int32, (int)reply.MatchType);
			this.database.AddInParameter(sqlStringCommand, "ReplyType", DbType.Int32, (int)reply.ReplyType);
			this.database.AddInParameter(sqlStringCommand, "MessageType", DbType.Int32, (int)reply.MessageType);
			this.database.AddInParameter(sqlStringCommand, "IsDisable", DbType.Boolean, reply.IsDisable);
			this.database.AddInParameter(sqlStringCommand, "LastEditDate", DbType.DateTime, reply.LastEditDate);
			this.database.AddInParameter(sqlStringCommand, "LastEditor", DbType.String, reply.LastEditor);
			this.database.AddInParameter(sqlStringCommand, "Content", DbType.String, "");
			this.database.AddInParameter(sqlStringCommand, "Type", DbType.Int32, 2);
			this.database.AddInParameter(sqlStringCommand, "ReplyId", DbType.Int32, reply.Id);
			this.database.ExecuteNonQuery(sqlStringCommand);
			foreach (NewsMsgInfo current in reply.NewsMsg)
			{
				stringBuilder = new StringBuilder();
				stringBuilder.Append("insert into vshop_Message(");
				stringBuilder.Append("ReplyId,Title,ImageUrl,Url,Description,Content)");
				stringBuilder.Append(" values (");
				stringBuilder.Append("@ReplyId,@Title,@ImageUrl,@Url,@Description,@Content)");
				sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
				this.database.AddInParameter(sqlStringCommand, "ReplyId", DbType.Int32, reply.Id);
				this.database.AddInParameter(sqlStringCommand, "Title", DbType.String, current.Title);
				this.database.AddInParameter(sqlStringCommand, "ImageUrl", DbType.String, current.PicUrl);
				this.database.AddInParameter(sqlStringCommand, "Url", DbType.String, current.Url);
				this.database.AddInParameter(sqlStringCommand, "Description", DbType.String, current.Description);
				this.database.AddInParameter(sqlStringCommand, "Content", DbType.String, current.Content);
				this.database.ExecuteNonQuery(sqlStringCommand);
			}
			return true;
		}
		private bool UpdateTextReply(TextReplyInfo reply)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("update vshop_Reply set ");
			stringBuilder.Append("Keys=@Keys,");
			stringBuilder.Append("MatchType=@MatchType,");
			stringBuilder.Append("ReplyType=@ReplyType,");
			stringBuilder.Append("MessageType=@MessageType,");
			stringBuilder.Append("IsDisable=@IsDisable,");
			stringBuilder.Append("LastEditDate=@LastEditDate,");
			stringBuilder.Append("LastEditor=@LastEditor,");
			stringBuilder.Append("Content=@Content,");
			stringBuilder.Append("Type=@Type,");
			stringBuilder.Append("ActivityId=@ActivityId");
			stringBuilder.Append(" where ReplyId=@ReplyId");
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "Keys", DbType.String, reply.Keys);
			this.database.AddInParameter(sqlStringCommand, "MatchType", DbType.Int32, (int)reply.MatchType);
			this.database.AddInParameter(sqlStringCommand, "ReplyType", DbType.Int32, (int)reply.ReplyType);
			this.database.AddInParameter(sqlStringCommand, "MessageType", DbType.Int32, (int)reply.MessageType);
			this.database.AddInParameter(sqlStringCommand, "IsDisable", DbType.Boolean, reply.IsDisable);
			this.database.AddInParameter(sqlStringCommand, "LastEditDate", DbType.DateTime, reply.LastEditDate);
			this.database.AddInParameter(sqlStringCommand, "LastEditor", DbType.String, reply.LastEditor);
			this.database.AddInParameter(sqlStringCommand, "Content", DbType.String, reply.Text);
			this.database.AddInParameter(sqlStringCommand, "Type", DbType.Int32, 2);
			this.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, reply.ActivityId);
			this.database.AddInParameter(sqlStringCommand, "ReplyId", DbType.Int32, reply.Id);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public bool HasReplyKey(string key)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(*) FROM vshop_Reply WHERE Keys = @Keys");
			this.database.AddInParameter(sqlStringCommand, "Keys", DbType.String, key);
			return Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand)) > 0;
		}
		public bool UpdateReplyRelease(int id)
		{
			ReplyInfo reply = this.GetReply(id);
			StringBuilder stringBuilder = new StringBuilder();
			if (reply.IsDisable)
			{
				if ((reply.ReplyType & ReplyType.NoMatch) == ReplyType.NoMatch)
				{
					stringBuilder.AppendFormat("update  vshop_Reply set IsDisable = 1 where ReplyType&{0}>0;", 4);
				}
				if ((reply.ReplyType & ReplyType.Subscribe) == ReplyType.Subscribe)
				{
					stringBuilder.AppendFormat("update  vshop_Reply set IsDisable = 1 where ReplyType&{0}>0;", 1);
				}
			}
			stringBuilder.Append("update vshop_Reply set ");
			stringBuilder.Append("IsDisable=~IsDisable");
			stringBuilder.Append(" where ReplyId=@ReplyId");
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "ReplyId", DbType.Int32, id);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public IList<ReplyInfo> GetAllReply()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("select ReplyId,Keys,MatchType,ReplyType,MessageType,IsDisable,LastEditDate,LastEditor,Content,Type,ActivityId");
			stringBuilder.Append(" FROM vshop_Reply order by Replyid desc ");
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			List<ReplyInfo> list = new List<ReplyInfo>();
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					ReplyInfo replyInfo = this.ReaderBind(dataReader);
					object obj;
					switch (replyInfo.MessageType)
					{
					case MessageType.Text:
					{
						TextReplyInfo textReplyInfo = replyInfo as TextReplyInfo;
						obj = dataReader["Content"];
						if (obj != null && obj != DBNull.Value)
						{
							textReplyInfo.Text = obj.ToString();
						}
						list.Add(textReplyInfo);
						break;
					}
					case MessageType.News:
					case MessageType.List:
					{
						NewsReplyInfo newsReplyInfo = replyInfo as NewsReplyInfo;
						newsReplyInfo.NewsMsg = this.GetNewsReplyInfo(newsReplyInfo.Id);
						list.Add(newsReplyInfo);
						break;
					}
					case (MessageType)3:
						goto IL_ED;
					default:
						goto IL_ED;
					}
					continue;
					IL_ED:
					TextReplyInfo textReplyInfo2 = replyInfo as TextReplyInfo;
					obj = dataReader["Content"];
					if (obj != null && obj != DBNull.Value)
					{
						textReplyInfo2.Text = obj.ToString();
					}
					list.Add(textReplyInfo2);
				}
			}
			return list;
		}
		public IList<ReplyInfo> GetReplies(ReplyType type)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("select ReplyId,Keys,MatchType,ReplyType,MessageType,IsDisable,LastEditDate,LastEditor,Content,Type,ActivityId ");
			stringBuilder.Append(" FROM vshop_Reply ");
            stringBuilder.Append(" where ReplyType  = @ReplyType and IsDisable=0");//去掉了按位运算 ReplyType  & @ReplyType= @ReplyType
			stringBuilder.Append(" order by replyid desc");
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "ReplyType", DbType.Int32, (int)type);
			List<ReplyInfo> list = new List<ReplyInfo>();
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					ReplyInfo replyInfo = this.ReaderBind(dataReader);
					TextReplyInfo textReplyInfo;
					object obj;
					switch (replyInfo.MessageType)
					{
					case MessageType.Text:
						textReplyInfo = (replyInfo as TextReplyInfo);
						obj = dataReader["Content"];
						if (obj != null && obj != DBNull.Value)
						{
							textReplyInfo.Text = obj.ToString();
						}
						list.Add(textReplyInfo);
						break;
					case MessageType.News:
					case MessageType.List:
					{
						NewsReplyInfo newsReplyInfo = replyInfo as NewsReplyInfo;
						newsReplyInfo.NewsMsg = this.GetNewsReplyInfo(newsReplyInfo.Id);
						list.Add(newsReplyInfo);
						break;
					}
					case (MessageType)3:
						goto IL_11F;
					default:
						goto IL_11F;
					}
					continue;
					IL_11F:
					textReplyInfo = (replyInfo as TextReplyInfo);
					obj = dataReader["Content"];
					if (obj != null && obj != DBNull.Value)
					{
						textReplyInfo.Text = obj.ToString();
					}
					list.Add(textReplyInfo);
				}
			}
			return list;
		}
		public IList<NewsMsgInfo> GetNewsReplyInfo(int replyid)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("select ReplyId,MsgID,Title,ImageUrl,Url,Description,Content from vshop_Message ");
			stringBuilder.Append(" where ReplyId=@ReplyId ");
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "ReplyId", DbType.Int32, replyid);
			List<NewsMsgInfo> list = new List<NewsMsgInfo>();
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add(this.ReaderBindNewsRelpy(dataReader));
				}
			}
			return list;
		}
		private NewsMsgInfo ReaderBindNewsRelpy(IDataReader dataReader)
		{
			NewsMsgInfo newsMsgInfo = new NewsMsgInfo();
			object obj = dataReader["MsgID"];
			if (obj != null && obj != DBNull.Value)
			{
				newsMsgInfo.Id = (int)obj;
			}
			obj = dataReader["Title"];
			if (obj != null && obj != DBNull.Value)
			{
				newsMsgInfo.Title = dataReader["Title"].ToString();
			}
			obj = dataReader["ImageUrl"];
			if (obj != null && obj != DBNull.Value)
			{
				newsMsgInfo.PicUrl = dataReader["ImageUrl"].ToString();
			}
			obj = dataReader["Url"];
			if (obj != null && obj != DBNull.Value)
			{
				newsMsgInfo.Url = dataReader["Url"].ToString();
			}
			obj = dataReader["Description"];
			if (obj != null && obj != DBNull.Value)
			{
				newsMsgInfo.Description = dataReader["Description"].ToString();
			}
			obj = dataReader["Content"];
			if (obj != null && obj != DBNull.Value)
			{
				newsMsgInfo.Content = dataReader["Content"].ToString();
			}
			return newsMsgInfo;
		}
		public ReplyInfo ReaderBind(IDataReader dataReader)
		{
			ReplyInfo replyInfo = null;
			object obj = dataReader["MessageType"];
			if (obj != null && obj != DBNull.Value)
			{
				if ((MessageType)obj == MessageType.Text)
				{
					replyInfo = new TextReplyInfo();
				}
				else
				{
					replyInfo = new NewsReplyInfo();
				}
			}
			obj = dataReader["ReplyId"];
			if (obj != null && obj != DBNull.Value)
			{
				replyInfo.Id = (int)obj;
			}
			replyInfo.Keys = dataReader["Keys"].ToString();
			obj = dataReader["MatchType"];
			if (obj != null && obj != DBNull.Value)
			{
				replyInfo.MatchType = (MatchType)obj;
			}
			obj = dataReader["ReplyType"];
			if (obj != null && obj != DBNull.Value)
			{
				replyInfo.ReplyType = (ReplyType)obj;
			}
			obj = dataReader["MessageType"];
			if (obj != null && obj != DBNull.Value)
			{
				replyInfo.MessageType = (MessageType)obj;
			}
			obj = dataReader["IsDisable"];
			if (obj != null && obj != DBNull.Value)
			{
				replyInfo.IsDisable = (bool)obj;
			}
			obj = dataReader["LastEditDate"];
			if (obj != null && obj != DBNull.Value)
			{
				replyInfo.LastEditDate = (DateTime)obj;
			}
			replyInfo.LastEditor = dataReader["LastEditor"].ToString();
			obj = dataReader["ActivityId"];
			if (obj != null && obj != DBNull.Value)
			{
				replyInfo.ActivityId = (int)obj;
			}
			return replyInfo;
		}
	}
}
