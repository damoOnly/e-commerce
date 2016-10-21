using EcShop.Entities;
using EcShop.Entities.Members;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
namespace EcShop.SqlDal.Members
{
     public  class TelVerifyDao
    {
         private Database database;

         public TelVerifyDao()
         {
             this.database = DatabaseFactory.CreateDatabase();
         }

         public  int  CreateVerify(Verify verify)
         {
             int result = 0;
             DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Ecshop_Verify([VerifyCode],[CellPhone],[ExpiredTime],[CreateTeime],CType) VALUES(@VerifyCode,@CellPhone,@ExpiredTime,@CreateTeime,@CType);SELECT @@IDENTITY");
             this.database.AddInParameter(sqlStringCommand, "VerifyCode", DbType.String, verify.VerifyCode);
             this.database.AddInParameter(sqlStringCommand, "CellPhone", DbType.String, verify.CellPhone);
             this.database.AddInParameter(sqlStringCommand, "ExpiredTime", DbType.DateTime, DateTime.Now.AddMinutes(5));
             this.database.AddInParameter(sqlStringCommand, "CreateTeime", DbType.DateTime, DateTime.Now);
             this.database.AddInParameter(sqlStringCommand, "CType", DbType.Int32, verify.CType);
             object obj = this.database.ExecuteScalar(sqlStringCommand);
             if (obj != null)
             {
                 result = Convert.ToInt32(obj.ToString());
             }
             return result;
         }

         public Verify GetVerify(string telphone)
         {
             Verify result = null;
             DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT top 1 * FROM Ecshop_Verify WHERE CellPhone=@CellPhone and ExpiredTime>=getdate() ORDER BY VerifyId DESC");
             this.database.AddInParameter(sqlStringCommand, "CellPhone", DbType.String, telphone);
             using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
             {
                 if (dataReader.Read())
                 {
                     result = new Verify();
                     result.CellPhone = dataReader["CellPhone"].ToString();
                     result.VerifyCode = dataReader["VerifyCode"].ToString();
                     result.CType = int.Parse(dataReader["CType"].ToString());
                 }
             }

             return result;
         }

         public Verify GetVerify(string telphone, int cType)
         {
             Verify result = null;
             DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT top 1 * FROM Ecshop_Verify WHERE CellPhone=@CellPhone and CType = @CType and ExpiredTime>=getdate() ORDER BY VerifyId DESC");
             this.database.AddInParameter(sqlStringCommand, "CellPhone", DbType.String, telphone);
             this.database.AddInParameter(sqlStringCommand, "CType", DbType.Int32, cType);

             using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
             {
                 if (dataReader.Read())
                 {
                     result = new Verify();
                     result.CellPhone = dataReader["CellPhone"].ToString();
                     result.VerifyCode = dataReader["VerifyCode"].ToString();
                     result.CType = int.Parse(dataReader["CType"].ToString());
                 }
             }

             return result;
         }
    }
}
