using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data;

namespace pbxdata.dalEcShop.UI.Web.Admin
{
    public class ExcelOperation
    {
        public DataTable GetTablesName(string filename, out Exception exx)
        {
            string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Extended Properties=Excel 8.0;" + "data source=" + filename;
            OleDbConnection myConn = new OleDbConnection(strCon);
            exx = null;
            try
            {
                Open(myConn);
                DataTable dt = myConn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, null);
                List<DataRow> list = new List<DataRow>();//删除重复表名
                foreach (DataRow r in dt.Rows)
                {
                    if (r["Table_Name"].ToString().Contains("_"))
                    {
                        list.Add(r);
                    }
                }
                foreach (DataRow r in list)
                {
                    dt.Rows.Remove(r);
                }
                return dt;
            }
            catch (Exception ex)
            {
                exx = ex;
                return null;
            }
            finally
            {
                Close(myConn);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename">excel文件完全限定路径</param>
        /// <param name="workTableName">excel文件工作表名称</param>
        /// <returns></returns>
        public DataSet GetData(string filename, string workTableName, out Exception exx)
        {
            DataSet ds;
            string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Extended Properties=Excel 8.0;" + "data source=" + filename;
            OleDbConnection myConn = new OleDbConnection(strCon);
            string strCom = " SELECT * FROM [" + workTableName + "]";
            try
            {
                Open(myConn);//打开数据库
                OleDbDataAdapter myCommand = new OleDbDataAdapter(strCom, myConn);
                ds = new DataSet();
                myCommand.Fill(ds);
                myCommand.Dispose();
                exx = null;
                return ds;
            }
            catch (Exception ex)
            { 
                exx = ex;
                return null;
            }
            finally
            {
                Close(myConn);
            }
        }
        private void Close(OleDbConnection con)
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
        /// <summary>
        /// 打开OledbOleDbConnection
        /// </summary>
        /// <param name="con"></param>
        private void Open(OleDbConnection con)
        {
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
        }
    }
}
