using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CrystalDecisions.CrystalReports.Engine;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.IO;

namespace EcShop.UI.Web.Admin.PO
{
    public class RptClass
    {

        #region public ReportDocument ReturnReportDoc(String ReportName,DataSet DS) 生成报表 tanxb 2011-11-09
        /// <summary>
        /// 返回DOC Tan.x.b  2011-11-09
        /// </summary>
        /// <param name="ReportName"></param>
        /// <param name="DS"></param>
        /// <returns></returns>
        public ReportDocument ReturnReportDoc(String ReportName, DataSet DS)
        {
            ReportDocument doc = new ReportDocument();
            string reportPath = ReportName;
            //reportPath = @"D:\TFSWork\APP.Win\BuildPublish\Reports\Import\SCMInvoice.rpt";
                
            doc.Load(reportPath);
            //判断存在
            if (doc.Subreports.Count > 0)
            {
                for (int i = 0; i < doc.Subreports.Count; i++)
                {
                    doc.Subreports[i].SetDataSource(DS.Tables[i + 1].DefaultView);
                }
            } 
            doc.SetDataSource(DS.Tables[0].DefaultView);
            return doc;
        } 
        #endregion
     

        #region ExportRpt(ReportDocument rDoc, string DocumentName) 把报表生成各种文件格式的文件（pdf,doc,xls,tif...）Author: Tan x.b ( 2011-11-09 )

        /// <summary>
        /// 把报表生成各种文件格式的文件（pdf,doc,xls）
        /// </summary>
        /// <param name="rDoc">生成的报表</param>
        /// <param name="DocumentName">导出的文件名</param>
        /// <returns></returns>
     
        public string ExportRpt(ReportDocument rDoc, string DocumentName)
        {
            try
            {
                CrystalDecisions.Shared.DiskFileDestinationOptions file = new CrystalDecisions.Shared.DiskFileDestinationOptions();


                file.DiskFileName =   DocumentName;

                string FileName = "";
                FileName = DocumentName.Substring(DocumentName.Length - 3, 3).ToString();

                rDoc.ExportOptions.ExportDestinationType = CrystalDecisions.Shared.ExportDestinationType.DiskFile;
                rDoc.ExportOptions.DestinationOptions = file;

                if (FileName.ToLower() == "doc")
                {
                    rDoc.ExportOptions.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.WordForWindows;
                }
                else if (FileName.ToLower() == "xls")
                {
                    rDoc.ExportOptions.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.Excel;
                }
                else
                {
                    rDoc.ExportOptions.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.PortableDocFormat;
                }
                rDoc.Export();

                return "导出成功";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        #endregion

        #region ExportRpt(String ReportName, DataSet DS, string DocumentName) 把报表生成各种文件格式的文件（pdf,doc,xls,tif...）Author: Tan x.b ( 2011-11-09 )

        /// <summary>
        /// 把报表生成各种文件格式的文件（pdf,doc,xls）
        /// </summary>
        /// <param name="ReportName">报表名称</param>
        /// <param name="DS">数据源</param>
        /// <param name="DocumentName">导出的文件名</param>
        /// <returns></returns>

        public string ExportRpt(String ReportName, DataSet DS, string DocumentName)
        {
            try
            {
                ReportDocument rDoc = new ReportDocument();
                rDoc = ReturnReportDoc(ReportName, DS);
                CrystalDecisions.Shared.DiskFileDestinationOptions file = new CrystalDecisions.Shared.DiskFileDestinationOptions();


                file.DiskFileName = DocumentName;

                string FileName = "";
                FileName = DocumentName.Substring(DocumentName.Length - 3, 3).ToString();

                rDoc.ExportOptions.ExportDestinationType = CrystalDecisions.Shared.ExportDestinationType.DiskFile;
                rDoc.ExportOptions.DestinationOptions = file;

                if (FileName.ToLower() == "doc")
                {
                    rDoc.ExportOptions.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.WordForWindows;
                }
                else if (FileName.ToLower() == "xls")
                {
                    rDoc.ExportOptions.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.Excel;
                }
                else
                {
                    rDoc.ExportOptions.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.PortableDocFormat;
                }
                rDoc.Export();
                return "导出成功";
            }
            catch (Exception ex)
            {
                ErrorLogs(ex.ToString());
                return ex.ToString();
            }
        }
        #endregion
        /// <summary>
        /// 文本日志
        /// </summary>
        /// <param name="list"></param>
        public static void ErrorLogs(string remark)
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + @"/LogPdf";
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                string FileNames = DateTime.Now.ToString("yyyyMMdd");
                StreamWriter sw = new StreamWriter(path + @"/" + FileNames + ".log", true);

                sw.Write(remark);
                sw.Flush();
                sw.Close();
            }
            catch
            {
            }
        }
    }
}
