using EcShop.Entities;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace EcShop.SqlDal
{
	public class VshopDesignTemplateDao
	{
		private Database database;
        public VshopDesignTemplateDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
        public bool AddVshopDesignTemplate(VshopDesignTemplate template)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Ecshop_VshopDesignTemplate VALUES (@TemplateName,@AddTime,@InUse,@TemplateType,@Content,@SupplierId)");
            this.database.AddInParameter(sqlStringCommand, "TemplateName", DbType.String, template.TemplateName);
            this.database.AddInParameter(sqlStringCommand, "AddTime", DbType.DateTime, template.AddTime);
            this.database.AddInParameter(sqlStringCommand, "InUse", DbType.Boolean, template.InUse);
            this.database.AddInParameter(sqlStringCommand, "TemplateType", DbType.Int32, template.TemplateType);
            this.database.AddInParameter(sqlStringCommand, "Content", DbType.String, template.Content);
            this.database.AddInParameter(sqlStringCommand, "SupplierId", DbType.Int32, template.SupplierId);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
        public VshopDesignTemplate GetVshopDesignTemplate(int TemplateType, bool IsInUse,int supplierId)
        {
            VshopDesignTemplate template = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT TOP 1 * FROM Ecshop_VshopDesignTemplate where TemplateType=@TemplateType and supplierId=@supplierId  ");
            this.database.AddInParameter(sqlStringCommand, "TemplateType", DbType.Int32, TemplateType);
            this.database.AddInParameter(sqlStringCommand, "supplierId", DbType.Int32, supplierId);
           // this.database.AddInParameter(sqlStringCommand, "IsInUse", DbType.Boolean, IsInUse);
            using (IDataReader dataReader= this.database.ExecuteReader(sqlStringCommand))
            {
                template = ReaderConvert.ReaderToModel<VshopDesignTemplate>(dataReader);
            }
            return template;
        }

        public VshopDesignTemplate GetVshopDesignTemplate(int TemplateType, bool IsInUse)
        {
            VshopDesignTemplate template = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT TOP 1 * FROM Ecshop_VshopDesignTemplate where TemplateType=@TemplateType  ");
            this.database.AddInParameter(sqlStringCommand, "TemplateType", DbType.Int32, TemplateType);
            // this.database.AddInParameter(sqlStringCommand, "IsInUse", DbType.Boolean, IsInUse);
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                template = ReaderConvert.ReaderToModel<VshopDesignTemplate>(dataReader);
            }
            return template;
        }

        public bool UpdateVshopDesignTemplate(VshopDesignTemplate template)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update Ecshop_VshopDesignTemplate SET TemplateName=@TemplateName,InUse=@InUse,TemplateType=@TemplateType,Content=@Content,SupplierId=@SupplierId WHERE Id=@Id");
            this.database.AddInParameter(sqlStringCommand, "TemplateName", DbType.String, template.TemplateName);
            this.database.AddInParameter(sqlStringCommand, "InUse", DbType.Boolean, template.InUse);
            this.database.AddInParameter(sqlStringCommand, "TemplateType", DbType.Int32, template.TemplateType);
            this.database.AddInParameter(sqlStringCommand, "Content", DbType.String, template.Content);
            this.database.AddInParameter(sqlStringCommand, "SupplierId", DbType.Int32, template.SupplierId);
            this.database.AddInParameter(sqlStringCommand, "Id", DbType.Int32, template.Id);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
        public bool ActivateDefaultTemplate(bool IsInUse)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update Ecshop_VshopDesignTemplate SET InUse=@InUse WHERE TemplateType=1");//目前只有一个首页模版,暂不考虑多模版
            this.database.AddInParameter(sqlStringCommand, "InUse", DbType.Boolean, IsInUse);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
    }
}
