using ASPNET.WebControls;
using EcShop.ControlPanel.Commodities;
using EcShop.Core;
using EcShop.Entities.HS;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace EcShop.UI.Web.Admin 
{
    public class EditHSCode : AdminPage
    {
        private int HS_CODE_ID;
        protected System.Web.UI.WebControls.TextBox txtHSCode;
        protected System.Web.UI.WebControls.TextBox txtHSCodeName;
        protected System.Web.UI.WebControls.TextBox txtLowRate;
        protected System.Web.UI.WebControls.TextBox txtHighRate;
        protected System.Web.UI.WebControls.TextBox txtOutRate;
        protected System.Web.UI.WebControls.TextBox txtTaxRate;
        protected System.Web.UI.WebControls.TextBox txtTslRate;
        protected System.Web.UI.WebControls.TextBox txtControlMa;
        protected System.Web.UI.WebControls.TextBox txtTempInRate;
        protected System.Web.UI.WebControls.TextBox txtTempOutRate;
        protected System.Web.UI.WebControls.TextBox txtNote;
        protected System.Web.UI.WebControls.TextBox txtControlInspection;
        protected System.Web.UI.WebControls.TextBox txtConsumptionRate;
        protected System.Web.UI.WebControls.TextBox elementsSearchText;
        //protected System.Web.UI.HtmlControls.HtmlTable elmentsList;
        protected System.Web.UI.WebControls.HiddenField elmentsJson;

        protected ElmentsListBox elmentsListBox;
        protected ProductCategoriesListBox listbox;

        protected UnitDropDownList dropUnit1;
        protected UnitDropDownList dropUnit2;

        protected System.Web.UI.WebControls.Button btnElementsSearchButton;
        protected System.Web.UI.WebControls.Button btnSave;
        protected System.Web.UI.WebControls.Button btnAddHSCode;

        //protected Pager pager;
        protected string searchkey;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            this.LoadParameters();
            if (!int.TryParse(this.Page.Request.QueryString["HS_CODE_ID"], out this.HS_CODE_ID))
            {
                
                base.GotoResourceNotFound();
                return;
            }

            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            this.btnElementsSearchButton.Click += new System.EventHandler(this.btnElementsSearchButton_Click);

            if (!this.Page.IsPostBack)
            {
                DataTable hscodeinfo = HSCodeHelper.GetHSCodeInfo(HS_CODE_ID).Tables[0];
                DataTable elmentsinfo = HSCodeHelper.GetHSCodeInfo(HS_CODE_ID).Tables[1];

                if (hscodeinfo == null)
                {
                    base.GotoResourceNotFound();
                    return;
                }

                this.dropUnit1.DataBind();
                this.dropUnit2.DataBind();
                this.elmentsListBox.DataBind();

                Globals.EntityCoding(hscodeinfo, false);
                this.BindHSCodeInfo(hscodeinfo, elmentsinfo);

            }
        }

        private void BindHSCodeInfo(DataTable hscodeinfo, DataTable elmentsinfo)
        {
            if (hscodeinfo.Rows.Count>0)
            {

                foreach (DataRow dr in hscodeinfo.Rows)
                {
                    this.txtHSCode.Text = dr["HS_CODE"].ToString().Trim();
                    this.txtHSCodeName.Text = dr["HS_NAME"].ToString().Trim();
                    this.dropUnit1.Text = dr["UNIT_1"].ToString();
                    this.dropUnit2.Text = dr["UNIT_2"].ToString();
                    this.txtLowRate.Text = dr["LOW_RATE"].ToString().Trim();
                    this.txtHighRate.Text = dr["HIGH_RATE"].ToString().Trim();
                    this.txtOutRate.Text = dr["OUT_RATE"].ToString().Trim();
                    this.txtTaxRate.Text = dr["TAX_RATE"].ToString().Trim();
                    this.txtTslRate.Text = dr["TSL_RATE"].ToString().Trim();
                    this.txtControlMa.Text = dr["CONTROL_MA"].ToString().Trim();
                    this.txtNote.Text = dr["NOTE_S"].ToString().Trim();
                    this.txtTempInRate.Text = dr["TEMP_IN_RATE"].ToString().Trim();
                    this.txtTempOutRate.Text = dr["TEMP_OUT_RATE"].ToString().Trim();
                    this.txtControlInspection.Text = dr["CONTROL_INSPECTION"].ToString().Trim();
                    this.txtConsumptionRate.Text = dr["CONSUMPTION_RATE"].ToString().Trim();

                    //HS_CODE ,HS_NAME ,LOW_RATE ,HIGH_RATE ,OUT_RATE ,TAX_RATE ,TSL_RATE ,UNIT_1 ,UNIT_2 ,CONTROL_MA ,NOTE_S ,TEMP_IN_RATE ,TEMP_OUT_RATE, CONTROL_INSPECTION ,CONSUMPTION_RAT
                }

            }
            
            if (elmentsinfo.Rows.Count > 0)
            {
                elmentsJson.Value = Newtonsoft.Json.JsonConvert.SerializeObject(elmentsinfo);
            }
            //else
            //{
            //    elmentsJson.Value = "{\"success\":\"NO\",\"MSG\":\"参数获取失败\"}";
            //}
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            HSCodeInfo hscodeinfo = this.GetHSCode();
            if (!HSCodeHelper.UpdateHSCodeInfo(hscodeinfo))
            {
                this.ShowMsg("修改海关编码失败,未知错误", false);
            }
            else
            {
                base.Response.Redirect(Globals.GetAdminAbsolutePath("/HS/HSCode.aspx"), true);
                return;
            }
        }

        private HSCodeInfo GetHSCode()
        {
            Hashtable hs = new Hashtable();

            HSCodeInfo hscodeinfo = new HSCodeInfo();

            hscodeinfo.HS_CODE_ID = int.Parse(this.Page.Request.QueryString["HS_CODE_ID"].ToString());
            hscodeinfo.HS_CODE = this.txtHSCode.Text.Trim();
            hscodeinfo.HS_NAME = this.txtHSCodeName.Text.Trim();

            hscodeinfo.UNIT_1 = this.dropUnit1.SelectedValue;
            hscodeinfo.UNIT_2 = this.dropUnit2.SelectedValue;
            hscodeinfo.CONTROL_MA = this.txtControlMa.Text.Trim();

            hscodeinfo.NOTE_S = this.txtNote.Text.Trim();
            hscodeinfo.CONTROL_INSPECTION = this.txtControlInspection.Text.Trim();

            hscodeinfo.LOW_RATE = decimal.Parse(Globals.HtmlEncode(this.txtLowRate.Text.Trim()));
            hscodeinfo.HIGH_RATE = decimal.Parse(Globals.HtmlEncode(this.txtHighRate.Text.Trim()));
            hscodeinfo.OUT_RATE = decimal.Parse(Globals.HtmlEncode(this.txtOutRate.Text.Trim()));
            hscodeinfo.TAX_RATE = decimal.Parse(Globals.HtmlEncode(this.txtTaxRate.Text.Trim()));
            hscodeinfo.TSL_RATE = decimal.Parse(Globals.HtmlEncode(this.txtTslRate.Text.Trim()));
            hscodeinfo.TEMP_IN_RATE = decimal.Parse(Globals.HtmlEncode(this.txtTempInRate.Text.Trim()));
            hscodeinfo.TEMP_OUT_RATE = decimal.Parse(Globals.HtmlEncode(this.txtTempOutRate.Text.Trim()));
            hscodeinfo.CONSUMPTION_RATE = decimal.Parse(Globals.HtmlEncode(this.txtConsumptionRate.Text.Trim()));

            DataSet ds =this.CreateTable();
            if (ds != null)
            {
                hscodeinfo.FND_HS_ELMENTS = ds;
            }

            return hscodeinfo;
        }

        private void btnElementsSearchButton_Click(object sender, EventArgs e)
        {
            //NetworkConnection.CreateNetWorkConnection("\\\\192.168.1.118\\trans2", "administrator", "xwj@123", "Z:");
            //File.WriteAllText("\\\\192.168.1.118\\trans2\\123.txt", "123");
            //NetworkConnection.Disconnect("Z:");

            this.elmentsListBox.HSElmentsName = this.searchkey;
            this.elmentsListBox.DataBind();
            //[{"HS_ELMENTS_ID":391,"HS_ELMENTS_NAME":"成分含量(钽的含量)","HS_ELMENTS_DESC":"","HS_CODE_ID":25,"VOIDED":false},{"HS_ELMENTS_ID":199,"HS_ELMENTS_NAME":"规格","HS_ELMENTS_DESC":"","HS_CODE_ID":25,"VOIDED":false},{"HS_ELMENTS_ID":4,"HS_ELMENTS_NAME":"品牌","HS_ELMENTS_DESC":"","HS_CODE_ID":25,"VOIDED":false}]
            DataSet ds = this.CreateTable();
            if (ds != null)
            {
                elmentsJson.Value = Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[0]);
            }
        }

        private DataSet CreateTable()
        {
            string str = this.Page.Request["elmentsID"];
            DataSet ds = null;
            DataTable dt = null;

            if (!string.IsNullOrEmpty(str))
            {
                ds = new DataSet();
                dt = new DataTable();

                DataColumn dcID = new DataColumn("HS_ELMENTS_ID");
                DataColumn dcName = new DataColumn("HS_ELMENTS_NAME");
                DataColumn dcDesc = new DataColumn("HS_ELMENTS_DESC");

                dt.Columns.Add(dcID);
                dt.Columns.Add(dcName);
                dt.Columns.Add(dcDesc);

                string[] arrelmentsID = str.Split(',');

                foreach (string elmentsID in arrelmentsID)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = elmentsID;
                    dr[1] = this.Page.Request["elmentsName_" + elmentsID];
                    dr[2] = this.Page.Request["elmentsDesc_" + elmentsID];
                    dt.Rows.Add(dr);
                }

                ds.Tables.Add(dt);
            }


            return ds;
        }

        private void LoadParameters()
        {
            if (!this.Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["searchKey"]))
                {
                    this.searchkey = Globals.UrlDecode(this.Page.Request.QueryString["searchKey"]);
                }

                this.elementsSearchText.Text = this.searchkey;
                return;
            }
            this.searchkey = this.elementsSearchText.Text.Trim();
        }

    }


   #region 共享访问
		 //public enum ERROR_ID
    //{
    //    ERROR_SUCCESS = 0,  // Success 
    //    ERROR_BUSY = 170,
    //    ERROR_MORE_DATA = 234,
    //    ERROR_NO_BROWSER_SERVERS_FOUND = 6118,
    //    ERROR_INVALID_LEVEL = 124,
    //    ERROR_ACCESS_DENIED = 5,
    //    ERROR_INVALID_PASSWORD = 86,
    //    ERROR_INVALID_PARAMETER = 87,
    //    ERROR_BAD_DEV_TYPE = 66,
    //    ERROR_NOT_ENOUGH_MEMORY = 8,
    //    ERROR_NETWORK_BUSY = 54,
    //    ERROR_BAD_NETPATH = 53,
    //    ERROR_NO_NETWORK = 1222,
    //    ERROR_INVALID_HANDLE_STATE = 1609,
    //    ERROR_EXTENDED_ERROR = 1208,
    //    ERROR_DEVICE_ALREADY_REMEMBERED = 1202,
    //    ERROR_NO_NET_OR_BAD_PATH = 1203
    //}

    //public enum RESOURCE_SCOPE
    //{
    //    RESOURCE_CONNECTED = 1,
    //    RESOURCE_GLOBALNET = 2,
    //    RESOURCE_REMEMBERED = 3,
    //    RESOURCE_RECENT = 4,
    //    RESOURCE_CONTEXT = 5
    //}

    //public enum RESOURCE_TYPE
    //{
    //    RESOURCETYPE_ANY = 0,
    //    RESOURCETYPE_DISK = 1,
    //    RESOURCETYPE_PRINT = 2,
    //    RESOURCETYPE_RESERVED = 8,
    //}

    //public enum RESOURCE_USAGE
    //{
    //    RESOURCEUSAGE_CONNECTABLE = 1,
    //    RESOURCEUSAGE_CONTAINER = 2,
    //    RESOURCEUSAGE_NOLOCALDEVICE = 4,
    //    RESOURCEUSAGE_SIBLING = 8,
    //    RESOURCEUSAGE_ATTACHED = 16,
    //    RESOURCEUSAGE_ALL = (RESOURCEUSAGE_CONNECTABLE | RESOURCEUSAGE_CONTAINER | RESOURCEUSAGE_ATTACHED),
    //}

    //public enum RESOURCE_DISPLAYTYPE
    //{
    //    RESOURCEDISPLAYTYPE_GENERIC = 0,
    //    RESOURCEDISPLAYTYPE_DOMAIN = 1,
    //    RESOURCEDISPLAYTYPE_SERVER = 2,
    //    RESOURCEDISPLAYTYPE_SHARE = 3,
    //    RESOURCEDISPLAYTYPE_FILE = 4,
    //    RESOURCEDISPLAYTYPE_GROUP = 5,
    //    RESOURCEDISPLAYTYPE_NETWORK = 6,
    //    RESOURCEDISPLAYTYPE_ROOT = 7,
    //    RESOURCEDISPLAYTYPE_SHAREADMIN = 8,
    //    RESOURCEDISPLAYTYPE_DIRECTORY = 9,
    //    RESOURCEDISPLAYTYPE_TREE = 10,
    //    RESOURCEDISPLAYTYPE_NDSCONTAINER = 11
    //}

    //[StructLayout(LayoutKind.Sequential)]
    //public struct NETRESOURCE
    //{
    //    public RESOURCE_SCOPE dwScope;
    //    public RESOURCE_TYPE dwType;
    //    public RESOURCE_DISPLAYTYPE dwDisplayType;
    //    public RESOURCE_USAGE dwUsage;

    //    [MarshalAs(UnmanagedType.LPStr)]
    //    public string lpLocalName;

    //    [MarshalAs(UnmanagedType.LPStr)]
    //    public string lpRemoteName;

    //    [MarshalAs(UnmanagedType.LPStr)]
    //    public string lpComment;

    //    [MarshalAs(UnmanagedType.LPStr)]
    //    public string lpProvider;
    //}

    //public class NetworkConnection
    //{

    //    [DllImport("mpr.dll")]
    //    public static extern int WNetAddConnection2A(NETRESOURCE[] lpNetResource, string lpPassword, string lpUserName, int dwFlags);

    //    [DllImport("mpr.dll")]
    //    public static extern int WNetCancelConnection2A(string sharename, int dwFlags, int fForce);


    //    public static int Connect(string remotePath, string localPath, string username, string password)
    //    {
    //        NETRESOURCE[] share_driver = new NETRESOURCE[1];
    //        share_driver[0].dwScope = RESOURCE_SCOPE.RESOURCE_GLOBALNET;
    //        share_driver[0].dwType = RESOURCE_TYPE.RESOURCETYPE_DISK;
    //        share_driver[0].dwDisplayType = RESOURCE_DISPLAYTYPE.RESOURCEDISPLAYTYPE_SHARE;
    //        share_driver[0].dwUsage = RESOURCE_USAGE.RESOURCEUSAGE_CONNECTABLE;
    //        share_driver[0].lpLocalName = localPath;
    //        share_driver[0].lpRemoteName = remotePath;

    //        Disconnect(localPath);
    //        int ret = WNetAddConnection2A(share_driver, password, username, 1);

    //        return ret;
    //    }

    //    public static int Disconnect(string localpath)
    //    {
    //        return WNetCancelConnection2A(localpath, 1, 1);
    //    }

    //    /// <summary>
    //    /// 创建共享连接 NetworkConnection.Disconnect(localpath);
    //    /// </summary>
    //    /// <param name="remotePath">远程目录"\\\\192.168.27.8\\trans2"</param>
    //    /// <param name="User">登录名</param>
    //    /// <param name="Pwd">密码</param>
    //    public static bool CreateNetWorkConnection(string remotePath, string User, string Pwd, string localpath = "X:")
    //    {
    //        int status = NetworkConnection.Connect(remotePath.TrimEnd('\\'), localpath, User, Pwd);
    //        if (status == (int)ERROR_ID.ERROR_SUCCESS)
    //        {
    //            return true;
    //        }
    //        else
    //        {
    //            return false;
    //        }
    //    } 
    //}
   #endregion
}
