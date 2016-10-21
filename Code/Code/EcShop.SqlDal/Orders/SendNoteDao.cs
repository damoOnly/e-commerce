using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Entities.Orders;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;
using System.Text;
namespace EcShop.SqlDal.Orders
{
	public class SendNoteDao
	{
		private Database database;
		public SendNoteDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public bool SaveSendNote(SendNoteInfo note)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" insert into Ecshop_OrderSendNote(NoteId,OrderId,Operator,Remark) values(@NoteId,@OrderId,@Operator,@Remark)");
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "NoteId", DbType.String, note.NoteId);
			this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, note.OrderId);
			this.database.AddInParameter(sqlStringCommand, "Operator", DbType.String, note.Operator);
			this.database.AddInParameter(sqlStringCommand, "Remark", DbType.String, note.Remark);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public DbQueryResult GetAllSendNote(RefundApplyQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" 1=1");
			if (!string.IsNullOrEmpty(query.OrderId))
			{
				stringBuilder.AppendFormat(" and OrderId = '{0}'", query.OrderId);
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Ecshop_OrderSendNote", "OrderId", stringBuilder.ToString(), "*");
		}
		public bool DelSendNote(string noteId)
		{
			string query = string.Format("DELETE FROM Ecshop_OrderSendNote WHERE NoteId='{0}'", noteId);
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
	}
}
