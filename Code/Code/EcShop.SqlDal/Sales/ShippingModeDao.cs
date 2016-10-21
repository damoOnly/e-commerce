using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Entities;
using EcShop.Entities.Sales;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Linq;
namespace EcShop.SqlDal.Sales
{
	public class ShippingModeDao
	{
		private Database database;
		public ShippingModeDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public IList<ShippingModeInfo> GetShippingModes()
		{
			IList<ShippingModeInfo> list = new List<ShippingModeInfo>();
			string query = "SELECT * FROM Ecshop_ShippingTypes st INNER JOIN Ecshop_ShippingTemplates temp ON st.TemplateId=temp.TemplateId Order By DisplaySequence";
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add(DataMapper.PopulateShippingMode(dataReader));
				}
			}
			return list;
		}
		public void SwapShippingModeSequence(int modeId, int replaceModeId, int displaySequence, int replaceDisplaySequence)
		{
			DataHelper.SwapSequence("Ecshop_ShippingTypes", "ModeId", "DisplaySequence", modeId, replaceModeId, displaySequence, replaceDisplaySequence);
		}
		public bool CreateShippingMode(ShippingModeInfo shippingMode)
		{
			DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_ShippingMode_Create");
			this.database.AddInParameter(storedProcCommand, "Name", DbType.String, shippingMode.Name);
			this.database.AddInParameter(storedProcCommand, "TemplateId", DbType.Int32, shippingMode.TemplateId);
			this.database.AddOutParameter(storedProcCommand, "ModeId", DbType.Int32, 4);
			this.database.AddOutParameter(storedProcCommand, "Status", DbType.Int32, 4);
			this.database.AddInParameter(storedProcCommand, "Description", DbType.String, shippingMode.Description);
			bool flag;
			using (DbConnection dbConnection = this.database.CreateConnection())
			{
				dbConnection.Open();
				DbTransaction dbTransaction = dbConnection.BeginTransaction();
				try
				{
					this.database.ExecuteNonQuery(storedProcCommand, dbTransaction);
					flag = ((int)this.database.GetParameterValue(storedProcCommand, "Status") == 0);
					if (flag)
					{
						int num = (int)this.database.GetParameterValue(storedProcCommand, "ModeId");
						DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" ");
						this.database.AddInParameter(sqlStringCommand, "ModeId", DbType.Int32, num);
						StringBuilder stringBuilder = new StringBuilder();
						int num2 = 0;
						stringBuilder.Append("DECLARE @ERR INT; Set @ERR =0;");
						foreach (string current in shippingMode.ExpressCompany)
						{
							stringBuilder.Append(" INSERT INTO Ecshop_TemplateRelatedShipping(ModeId,ExpressCompanyName) VALUES( @ModeId,").Append("@ExpressCompanyName").Append(num2).Append("); SELECT @ERR=@ERR+@@ERROR;");
							this.database.AddInParameter(sqlStringCommand, "ExpressCompanyName" + num2, DbType.String, current);
							num2++;
						}
						sqlStringCommand.CommandText = stringBuilder.Append("SELECT @ERR;").ToString();
						int num3 = (int)this.database.ExecuteScalar(sqlStringCommand, dbTransaction);
						if (num3 != 0)
						{
							dbTransaction.Rollback();
							flag = false;
						}
					}
					dbTransaction.Commit();
				}
				catch
				{
					if (dbTransaction.Connection != null)
					{
						dbTransaction.Rollback();
					}
					flag = false;
				}
				finally
				{
					dbConnection.Close();
				}
			}
			return flag;
		}
		public bool DeleteShippingMode(int modeId)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Ecshop_TemplateRelatedShipping Where ModeId=@ModeId;DELETE FROM Ecshop_ShippingTypes Where ModeId=@ModeId;");
			this.database.AddInParameter(sqlStringCommand, "ModeId", DbType.Int32, modeId);
			this.database.AddOutParameter(sqlStringCommand, "Status", DbType.Int32, 4);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public bool UpdateShippingMode(ShippingModeInfo shippingMode)
		{
			DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_ShippingMode_Update");
			this.database.AddInParameter(storedProcCommand, "Name", DbType.String, shippingMode.Name);
			this.database.AddInParameter(storedProcCommand, "TemplateId", DbType.Int32, shippingMode.TemplateId);
			this.database.AddInParameter(storedProcCommand, "ModeId", DbType.Int32, shippingMode.ModeId);
			this.database.AddOutParameter(storedProcCommand, "Status", DbType.Int32, 4);
			this.database.AddInParameter(storedProcCommand, "Description", DbType.String, shippingMode.Description);
			bool flag;
			using (DbConnection dbConnection = this.database.CreateConnection())
			{
				dbConnection.Open();
				DbTransaction dbTransaction = dbConnection.BeginTransaction();
				try
				{
					this.database.ExecuteNonQuery(storedProcCommand, dbTransaction);
					flag = ((int)this.database.GetParameterValue(storedProcCommand, "Status") == 0);
					if (flag)
					{
						DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" ");
						this.database.AddInParameter(sqlStringCommand, "ModeId", DbType.Int32, shippingMode.ModeId);
						StringBuilder stringBuilder = new StringBuilder();
						int num = 0;
						stringBuilder.Append("DECLARE @ERR INT; Set @ERR =0;");
						foreach (string current in shippingMode.ExpressCompany)
						{
							stringBuilder.Append(" INSERT INTO Ecshop_TemplateRelatedShipping(ModeId,ExpressCompanyName) VALUES( @ModeId,").Append("@ExpressCompanyName").Append(num).Append("); SELECT @ERR=@ERR+@@ERROR;");
							this.database.AddInParameter(sqlStringCommand, "ExpressCompanyName" + num, DbType.String, current);
							num++;
						}
						sqlStringCommand.CommandText = stringBuilder.Append("SELECT @ERR;").ToString();
						int num2 = (int)this.database.ExecuteScalar(sqlStringCommand, dbTransaction);
						if (num2 != 0)
						{
							dbTransaction.Rollback();
							flag = false;
						}
					}
					dbTransaction.Commit();
				}
				catch
				{
					if (dbTransaction.Connection != null)
					{
						dbTransaction.Rollback();
					}
					flag = false;
				}
				finally
				{
					dbConnection.Close();
				}
			}
			return flag;
		}
        /// <summary>
        /// 根据运费模版Id获取发货模版
        /// </summary>
        /// <param name="TemplateId">模版Id</param>
        /// <returns></returns>
        public ShippingModeInfo GetShippingMode(int TemplateId)
        {
            ShippingModeInfo shippingModeInfo = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT *,ModeId=-1,Name='',Description='',DisplaySequence=-1 from Ecshop_ShippingTemplates Where TemplateId =@TemplateId");
            DbCommand expr_37 = sqlStringCommand;
            expr_37.CommandText += "  SELECT * FROM Ecshop_ShippingTypeGroups WHERE TemplateId = @TemplateId";
            this.database.AddInParameter(sqlStringCommand, "TemplateId", DbType.Int32, TemplateId);
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (dataReader.Read())
                {
                    shippingModeInfo = DataMapper.PopulateShippingMode(dataReader);
                }
                dataReader.NextResult();
                while (dataReader.Read())
                {
                    shippingModeInfo.ModeGroup.Add(new ShippingModeGroupInfo
                    {
                        AddPrice = (decimal)dataReader["AddPrice"],
                        Price = (decimal)dataReader["Price"],
                        TemplateId = (int)dataReader["TemplateId"],
                        GroupId = (int)dataReader["GroupId"]
                    });
                    /* 2015-08-27 修改之前
                    foreach (ShippingModeGroupInfo current in shippingModeInfo.ModeGroup)
                    {
                        string commandText = "SELECT RegionId FROM Ecshop_ShippingRegions WHERE GroupId = " + current.GroupId;
                        using (IDataReader dataReader2 = this.database.ExecuteReader(CommandType.Text, commandText))
                        {
                            while (dataReader2.Read())
                            {
                                current.ModeRegions.Add(new ShippingRegionInfo
                                {
                                    GroupId = current.GroupId,
                                    TemplateId = current.TemplateId,
                                    RegionId = (int)dataReader2["RegionId"]
                                });
                            }
                        }
                    }*/
                }

                //获取发货区域信息
                List<ShippingRegionInfo> listShippingRegion = new List<ShippingRegionInfo>();

                List<ShippingModeGroupInfo> groupInfo = (shippingModeInfo != null && shippingModeInfo.ModeGroup != null && shippingModeInfo.ModeGroup.Count > 0) ? shippingModeInfo.ModeGroup.OrderByDescending(a=>a.GroupId).ToList() : null;

                if (groupInfo != null && groupInfo.Count > 0)
                {

                    var groupId = string.Join(",", groupInfo.Select(t => t.GroupId).Distinct().ToArray());

                    string commandText = "SELECT TemplateId,GroupId,RegionId FROM Ecshop_ShippingRegions WHERE GroupId in(" + groupId + ")  order by GroupId desc";
                    using (IDataReader dataReader2 = this.database.ExecuteReader(CommandType.Text, commandText))
                    {
                        while (dataReader2.Read())
                        {
                            ShippingRegionInfo shippingRegionInfo = new ShippingRegionInfo();
                            if (dataReader2["GroupId"] != System.DBNull.Value)
                            {
                                shippingRegionInfo.GroupId = (int)dataReader2["GroupId"];
                            }
                            if (dataReader2["GroupId"] != System.DBNull.Value)
                            {
                                shippingRegionInfo.TemplateId = (int)dataReader2["TemplateId"];
                            }
                            if (dataReader2["RegionId"] != System.DBNull.Value)
                            {
                                shippingRegionInfo.RegionId = (int)dataReader2["RegionId"];
                            }

                            listShippingRegion.Add(shippingRegionInfo);
                        }
                    }

                    groupInfo.ForEach(t => {
                        var list = listShippingRegion.Where(a => a.GroupId == t.GroupId);
                        var li = t.ModeRegions.ToList();
                        li.AddRange(list);
                        t.ModeRegions = li;
                    });
                }

                return shippingModeInfo;
            }
        }

        /// <summary>
        /// 获取所有发货模版
        /// </summary>
        /// <returns></returns>
        public List<ShippingModeInfo> GetAllShippingMode()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT *,ModeId=-1,Name='',Description='',DisplaySequence=-1 from Ecshop_ShippingTemplates;");
            sqlStringCommand.CommandText += "SELECT * FROM Ecshop_ShippingTypeGroups;";
            sqlStringCommand.CommandText += "SELECT * FROM Ecshop_ShippingRegions;";

            List<ShippingModeInfo> modes = new List<ShippingModeInfo>();
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (dataReader.Read())
                {
                    modes.Add(DataMapper.PopulateShippingMode(dataReader));
                }
                dataReader.NextResult();
                while (dataReader.Read())
                {
                    int templateId;
                    int.TryParse(dataReader["TemplateId"].ToString(), out templateId);
                    ShippingModeInfo shippingModeInfo = modes.FirstOrDefault(p => p.TemplateId == templateId);
                    if (shippingModeInfo == null)
                    {
                        continue;
                    }
                    shippingModeInfo.ModeGroup.Add(new ShippingModeGroupInfo
                    {
                        AddPrice = (decimal)dataReader["AddPrice"],
                        Price = (decimal)dataReader["Price"],
                        TemplateId = (int)dataReader["TemplateId"],
                        GroupId = (int)dataReader["GroupId"]
                    });
                }
                dataReader.NextResult();
                while (dataReader.Read())
                {
                    int templateId;
                    int.TryParse(dataReader["TemplateId"].ToString(), out templateId);
                    int groupId;
                    int.TryParse(dataReader["GroupId"].ToString(), out groupId);
                    ShippingModeInfo shippingModeInfo = modes.FirstOrDefault(p => p.TemplateId == templateId);
                    if (shippingModeInfo == null)
                    {
                        continue;
                    }
                    ShippingModeGroupInfo shippingModeGroupInfo = shippingModeInfo.ModeGroup.FirstOrDefault(p => p.GroupId == groupId);
                    if (shippingModeGroupInfo == null)
                    {
                        continue;
                    }
                    shippingModeGroupInfo.ModeRegions.Add(new ShippingRegionInfo
                    {
                        GroupId = shippingModeGroupInfo.GroupId,
                        TemplateId = shippingModeGroupInfo.TemplateId,
                        RegionId = (int)dataReader["RegionId"]
                    });
                }
            }
            return modes;
        }

		public ShippingModeInfo GetShippingMode(int modeId, bool includeDetail)
		{
			ShippingModeInfo shippingModeInfo = null;
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_ShippingTypes st INNER JOIN Ecshop_ShippingTemplates temp ON st.TemplateId=temp.TemplateId Where ModeId =@ModeId");
			if (includeDetail)
			{
				DbCommand expr_20 = sqlStringCommand;
				expr_20.CommandText += " SELECT * FROM Ecshop_TemplateRelatedShipping Where ModeId =@ModeId";
				DbCommand expr_37 = sqlStringCommand;
				expr_37.CommandText += " SELECT * FROM Ecshop_ShippingTypeGroups WHERE TemplateId = (SELECT TemplateId FROM Ecshop_ShippingTypes WHERE ModeId =@ModeId )";
			}
			this.database.AddInParameter(sqlStringCommand, "ModeId", DbType.Int32, modeId);
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					shippingModeInfo = DataMapper.PopulateShippingMode(dataReader);
				}
				if (includeDetail)
				{
					dataReader.NextResult();
					while (dataReader.Read())
					{
						if (dataReader["ExpressCompanyName"] != DBNull.Value)
						{
							shippingModeInfo.ExpressCompany.Add((string)dataReader["ExpressCompanyName"]);
						}
					}
					dataReader.NextResult();
					while (dataReader.Read())
					{
						shippingModeInfo.ModeGroup.Add(new ShippingModeGroupInfo
						{
							AddPrice = (decimal)dataReader["AddPrice"],
							Price = (decimal)dataReader["Price"],
							TemplateId = (int)dataReader["TemplateId"],
							GroupId = (int)dataReader["GroupId"]
						});
					}

                    if (shippingModeInfo != null)
                    {
                        foreach (ShippingModeGroupInfo current in shippingModeInfo.ModeGroup)
                        {
                            string commandText = "SELECT RegionId FROM Ecshop_ShippingRegions WHERE GroupId = " + current.GroupId;
                            using (IDataReader dataReader2 = this.database.ExecuteReader(CommandType.Text, commandText))
                            {
                                while (dataReader2.Read())
                                {
                                    current.ModeRegions.Add(new ShippingRegionInfo
                                    {
                                        GroupId = current.GroupId,
                                        TemplateId = current.TemplateId,
                                        RegionId = (int)dataReader2["RegionId"]
                                    });
                                }
                            }
                        }
                    }
				}
			}
			return shippingModeInfo;
		}
		public ShippingModeInfo GetShippingModeByCompany(string companyname)
		{
            ShippingModeInfo result = null;
			string query = "SELECT * FROM Ecshop_ShippingTypes st INNER JOIN Ecshop_ShippingTemplates temp ON st.TemplateId=temp.TemplateId AND st.ModeId IN (SELECT ModeId FROM Ecshop_TemplateRelatedShipping WHERE ExpressCompanyName='" + DataHelper.CleanSearchString(companyname) + "')";
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulateShippingMode(dataReader);
				}
			}
			return result;
		}
		public bool CreateShippingTemplate(ShippingModeInfo shippingMode)
		{
			bool flag = false;
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Ecshop_ShippingTemplates(TemplateName,Weight,AddWeight,Price,AddPrice) VALUES(@TemplateName,@Weight,@AddWeight,@Price,@AddPrice);SELECT @@Identity");
			this.database.AddInParameter(sqlStringCommand, "TemplateName", DbType.String, shippingMode.Name);
			this.database.AddInParameter(sqlStringCommand, "Weight", DbType.Int32, shippingMode.Weight);
			if (shippingMode.AddWeight.HasValue)
			{
				this.database.AddInParameter(sqlStringCommand, "AddWeight", DbType.Int32, shippingMode.AddWeight);
			}
			else
			{
				this.database.AddInParameter(sqlStringCommand, "AddWeight", DbType.Int32, 0);
			}
			this.database.AddInParameter(sqlStringCommand, "Price", DbType.Currency, shippingMode.Price);
			if (shippingMode.AddPrice.HasValue)
			{
				this.database.AddInParameter(sqlStringCommand, "AddPrice", DbType.Currency, shippingMode.AddPrice);
			}
			else
			{
				this.database.AddInParameter(sqlStringCommand, "AddPrice", DbType.Currency, 0);
			}
			using (DbConnection dbConnection = this.database.CreateConnection())
			{
				dbConnection.Open();
				DbTransaction dbTransaction = dbConnection.BeginTransaction();
				try
				{
					object obj = this.database.ExecuteScalar(sqlStringCommand, dbTransaction);
					int num = 0;
					if (obj != null && obj != DBNull.Value)
					{
						int.TryParse(obj.ToString(), out num);
						flag = (num > 0);
					}
					if (flag)
					{
						DbCommand sqlStringCommand2 = this.database.GetSqlStringCommand(" ");
						this.database.AddInParameter(sqlStringCommand2, "TemplateId", DbType.Int32, num);
						if (shippingMode.ModeGroup != null && shippingMode.ModeGroup.Count > 0)
						{
							StringBuilder stringBuilder = new StringBuilder();
							int num2 = 0;
							int num3 = 0;
							stringBuilder.Append("DECLARE @ERR INT; Set @ERR =0;");
							stringBuilder.Append(" DECLARE @GroupId Int;");
							foreach (ShippingModeGroupInfo current in shippingMode.ModeGroup)
							{
								stringBuilder.Append(" INSERT INTO Ecshop_ShippingTypeGroups(TemplateId,Price,AddPrice) VALUES( @TemplateId,").Append("@Price").Append(num2).Append(",@AddPrice").Append(num2).Append("); SELECT @ERR=@ERR+@@ERROR;");
								this.database.AddInParameter(sqlStringCommand2, "Price" + num2, DbType.Currency, current.Price);
								this.database.AddInParameter(sqlStringCommand2, "AddPrice" + num2, DbType.Currency, current.AddPrice);
								stringBuilder.Append("Set @GroupId =@@identity;");
								foreach (ShippingRegionInfo current2 in current.ModeRegions)
								{
									stringBuilder.Append(" INSERT INTO Ecshop_ShippingRegions(TemplateId,GroupId,RegionId) VALUES(@TemplateId,@GroupId").Append(",@RegionId").Append(num3).Append("); SELECT @ERR=@ERR+@@ERROR;");
									this.database.AddInParameter(sqlStringCommand2, "RegionId" + num3, DbType.Int32, current2.RegionId);
									num3++;
								}
								num2++;
							}
							sqlStringCommand2.CommandText = stringBuilder.Append("SELECT @ERR;").ToString();
							int num4 = (int)this.database.ExecuteScalar(sqlStringCommand2, dbTransaction);
							if (num4 != 0)
							{
								dbTransaction.Rollback();
								flag = false;
							}
						}
					}
					dbTransaction.Commit();
				}
				catch
				{
					if (dbTransaction.Connection != null)
					{
						dbTransaction.Rollback();
					}
					flag = false;
				}
				finally
				{
					dbConnection.Close();
				}
			}
			return flag;
		}
		public bool UpdateShippingTemplate(ShippingModeInfo shippingMode)
		{
			bool flag = false;
			StringBuilder stringBuilder = new StringBuilder("UPDATE Ecshop_ShippingTemplates SET TemplateName=@TemplateName,Weight=@Weight,AddWeight=@AddWeight,Price=@Price,AddPrice=@AddPrice WHERE TemplateId=@TemplateId;");
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "TemplateName", DbType.String, shippingMode.Name);
			this.database.AddInParameter(sqlStringCommand, "Weight", DbType.Currency, shippingMode.Weight);
			this.database.AddInParameter(sqlStringCommand, "AddWeight", DbType.Currency, shippingMode.AddWeight);
			this.database.AddInParameter(sqlStringCommand, "Price", DbType.Currency, shippingMode.Price);
			this.database.AddInParameter(sqlStringCommand, "AddPrice", DbType.Currency, shippingMode.AddPrice);
			this.database.AddInParameter(sqlStringCommand, "TemplateId", DbType.Int32, shippingMode.TemplateId);
			using (DbConnection dbConnection = this.database.CreateConnection())
			{
				dbConnection.Open();
				DbTransaction dbTransaction = dbConnection.BeginTransaction();
				try
				{
					flag = (this.database.ExecuteNonQuery(sqlStringCommand, dbTransaction) > 0);
					if (flag)
					{
						DbCommand sqlStringCommand2 = this.database.GetSqlStringCommand(" ");
						this.database.AddInParameter(sqlStringCommand2, "TemplateId", DbType.Int32, shippingMode.TemplateId);
						StringBuilder stringBuilder2 = new StringBuilder();
						int num = 0;
						int num2 = 0;
						stringBuilder2.Append("DELETE Ecshop_ShippingTypeGroups WHERE TemplateId=@TemplateId;");
						stringBuilder2.Append("DELETE Ecshop_ShippingRegions WHERE TemplateId=@TemplateId;");
						stringBuilder2.Append("DECLARE @ERR INT; Set @ERR =0;");
						stringBuilder2.Append(" DECLARE @GroupId Int;");
						if (shippingMode.ModeGroup != null && shippingMode.ModeGroup.Count > 0)
						{
							foreach (ShippingModeGroupInfo current in shippingMode.ModeGroup)
							{
								stringBuilder2.Append(" INSERT INTO Ecshop_ShippingTypeGroups(TemplateId,Price,AddPrice) VALUES( @TemplateId,").Append("@Price").Append(num).Append(",@AddPrice").Append(num).Append("); SELECT @ERR=@ERR+@@ERROR;");
								this.database.AddInParameter(sqlStringCommand2, "Price" + num, DbType.Currency, current.Price);
								this.database.AddInParameter(sqlStringCommand2, "AddPrice" + num, DbType.Currency, current.AddPrice);
								stringBuilder2.Append("Set @GroupId =@@identity;");
								foreach (ShippingRegionInfo current2 in current.ModeRegions)
								{
									stringBuilder2.Append(" INSERT INTO Ecshop_ShippingRegions(TemplateId,GroupId,RegionId) VALUES(@TemplateId,@GroupId").Append(",@RegionId").Append(num2).Append("); SELECT @ERR=@ERR+@@ERROR;");
									this.database.AddInParameter(sqlStringCommand2, "RegionId" + num2, DbType.Int32, current2.RegionId);
									num2++;
								}
								num++;
							}
						}
						sqlStringCommand2.CommandText = stringBuilder2.Append("SELECT @ERR;").ToString();
						int num3 = (int)this.database.ExecuteScalar(sqlStringCommand2, dbTransaction);
						if (num3 != 0)
						{
							dbTransaction.Rollback();
							flag = false;
						}
					}
					dbTransaction.Commit();
				}
				catch
				{
					if (dbTransaction.Connection != null)
					{
						dbTransaction.Rollback();
					}
					flag = false;
				}
				finally
				{
					dbConnection.Close();
				}
			}
			return flag;
		}
		public bool DeleteShippingTemplate(int templateId)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Ecshop_ShippingTemplates Where TemplateId=@TemplateId");
			this.database.AddInParameter(sqlStringCommand, "TemplateId", DbType.Int32, templateId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public DbQueryResult GetShippingTemplates(Pagination pagin)
		{
			return DataHelper.PagingByRownumber(pagin.PageIndex, pagin.PageSize, pagin.SortBy, pagin.SortOrder, pagin.IsCount, "Ecshop_ShippingTemplates", "TemplateId", "", "*");
		}
		public DataTable GetShippingAllTemplates()
		{
			string query = "SELECT * FROM Ecshop_ShippingTemplates ";
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			DataTable result = null;
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public ShippingModeInfo GetShippingTemplate(int templateId, bool includeDetail)
		{
			ShippingModeInfo shippingModeInfo = null;
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" SELECT * FROM Ecshop_ShippingTemplates Where TemplateId =@TemplateId");
			if (includeDetail)
			{
				DbCommand expr_20 = sqlStringCommand;
				expr_20.CommandText += " SELECT GroupId,TemplateId,Price,AddPrice FROM Ecshop_ShippingTypeGroups Where TemplateId =@TemplateId";
				DbCommand expr_37 = sqlStringCommand;
				expr_37.CommandText += " SELECT sr.TemplateId,sr.GroupId,sr.RegionId FROM Ecshop_ShippingRegions sr Where sr.TemplateId =@TemplateId";
			}
			this.database.AddInParameter(sqlStringCommand, "TemplateId", DbType.Int32, templateId);
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					shippingModeInfo = DataMapper.PopulateShippingTemplate(dataReader);
				}
				if (includeDetail)
				{
					dataReader.NextResult();
					while (dataReader.Read())
					{
						shippingModeInfo.ModeGroup.Add(DataMapper.PopulateShippingModeGroup(dataReader));
					}
					dataReader.NextResult();
					while (dataReader.Read())
					{
						foreach (ShippingModeGroupInfo current in shippingModeInfo.ModeGroup)
						{
							if (current.GroupId == (int)dataReader["GroupId"])
							{
								current.ModeRegions.Add(DataMapper.PopulateShippingRegion(dataReader));
							}
						}
					}
				}
			}
			return shippingModeInfo;
		}
		public IList<string> GetExpressCompanysByMode(int modeId)
		{
			IList<string> list = new List<string>();
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_TemplateRelatedShipping Where ModeId =@ModeId");
			this.database.AddInParameter(sqlStringCommand, "ModeId", DbType.Int32, modeId);
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					if (dataReader["ExpressCompanyName"] != DBNull.Value)
					{
						list.Add((string)dataReader["ExpressCompanyName"]);
					}
				}
			}
			return list;
		}
	}
}
