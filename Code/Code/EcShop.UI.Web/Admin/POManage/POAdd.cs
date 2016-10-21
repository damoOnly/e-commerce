using ASPNET.WebControls;
using EcShop.ControlPanel.Commodities;
using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Core.Enums;
using EcShop.Entities.PO;
using EcShop.Entities.Store;
using EcShop.Membership.Context;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using EcShop.ControlPanel.Sales;

namespace EcShop.UI.Web.Admin.PO
{
    [PrivilegeCheck(Privilege.POAdd)]
    public class POAdd : AdminPage
    {
        protected System.Web.UI.WebControls.TextBox txtPONumber;
        protected System.Web.UI.WebControls.DropDownList ddlPOType;        
        protected SupplierDropDownList ddlSupplier;
        protected WebCalendar calendarExpectedTime;
        protected System.Web.UI.WebControls.TextBox txtRemark;
        protected System.Web.UI.WebControls.Button btnSave;
        protected WebCalendar DepartTime;     //发船日
        protected WebCalendar ArrivalTime;     //到港日期
        protected System.Web.UI.WebControls.TextBox ContainerNumber;    //柜号
        protected System.Web.UI.WebControls.TextBox BillNo;    //提单号
        protected System.Web.UI.WebControls.HiddenField hidCountryId;  //启运港ID
        protected System.Web.UI.WebControls.HiddenField hidCountry;
        protected System.Web.UI.WebControls.DropDownList ddlEndPort;    //目的港
        protected System.Web.UI.HtmlControls.HtmlInputText txtCountry;         //启运港

        protected void Page_Load(object sender, System.EventArgs e)
        {
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

            if (!this.Page.IsPostBack)
            {
                this.ddlSupplier.DataBind();
                EndPortPort();
                //如果有Id代表是编辑
                if (Request["Id"] != null && Request["Id"].ToString() != "")
                {
                    int tmpId = 0;
                    if (int.TryParse(this.Page.Request["Id"], out tmpId))
                    {
                        DataBind(this.Page.Request["Id"]);
                    }
                    else
                    {
                        this.ShowMsg("未知参数", false);
                    }
                }
                
            }
        }

        /// <summary>
        /// 编辑时绑定数据
        /// </summary>
        private void DataBind(string Id)
        {
            IList<PurchaseOrderInfo> purchaseOrderInfo = PurchaseOrderHelper.GetPurchaseOrder("Id=" + Id);
            if (purchaseOrderInfo.Count > 0)
            {
                PurchaseOrderInfo po = purchaseOrderInfo[0];
                ddlPOType.SelectedValue = po.POType.ToString();
                txtPONumber.Text = po.PONumber;
                ddlSupplier.SelectedValue = po.SupplierId;
                ddlSupplier.Enabled = false;
                ddlSupplier.BackColor = System.Drawing.Color.Gray;
                if (po.ExpectedTime != null)
                {
                    calendarExpectedTime.Text = Convert.ToDateTime(po.ExpectedTime).ToString("yyyy-MM-dd");
                }
                if (po.DepartTime != null)
                {
                    DepartTime.Text = Convert.ToDateTime(po.DepartTime).ToString("yyyy-MM-dd");
                }
                if (po.ArrivalTime != null)
                {
                    ArrivalTime.Text = Convert.ToDateTime(po.ArrivalTime).ToString("yyyy-MM-dd");
                }
                ContainerNumber.Text = po.ContainerNumber;
                BillNo.Text = po.BillNo;
                hidCountry.Value = po.StartPort;
                hidCountryId.Value = po.StartPortID;
                txtCountry.Value = po.StartPort;
                if (po.EndPortID!=null)
                {
                    ddlEndPort.SelectedValue = po.EndPortID;
                }

                txtRemark.Text = po.Remark;
            }
        }

        /// <summary>
        /// 绑定港口
        /// </summary>
        private void EndPortPort()
        {
            ddlEndPort.Items.Clear();
            ddlEndPort.Items.Add(new ListItem(string.Empty, string.Empty));
            Dictionary<string, string> portItem = BaseEnumDictHelper.GetBaseEnumDictItems("port");
            if (portItem != null && portItem.Count > 0)
            {
                foreach (KeyValuePair<string, string> kvp in portItem)
                {
                    string itemValue = kvp.Key;
                    string itemText = kvp.Value;
                    //if (string.IsNullOrEmpty(code))
                    //{
                    //    code = "0";
                    //}
                    //itemValue += "|" + code;
                    ListItem item = new ListItem(itemText, itemValue);
                    //item.Attributes.Add("Code", Globals.HtmlDecode(supplierdt.Rows[i]["SupplierCode"].ToString()));
                    ddlEndPort.Items.Add(item);
                }
            }
        }

        private void btnSave_Click(object sender, System.EventArgs e)
        {
            ManagerHelper.CheckPrivilege(Privilege.POAdd);
            if (this.ddlSupplier.SelectedValue == null || this.ddlSupplier.SelectedValue == 0)
            {
                this.ShowMsg("请选择供货商", false);
                return;
            }

            //if (calendarExpectedTime.SelectedDate < DateTime.Now)
            //{
            //    this.ShowMsg("预计到货时间必须大于当前时间", false);
            //    return;
            //}
            var member = HiContext.Current.User;
            if (member == null || member.IsLockedOut)
            {
                this.ShowMsg("获取用户信息失败，请重新登录", false);
                return;
            }

            PurchaseOrderInfo purchaseOrderInfo = new PurchaseOrderInfo
            {
                POType = int.Parse(ddlPOType.SelectedValue),
                SupplierId = this.ddlSupplier.SelectedValue,
                ExpectedTime = calendarExpectedTime.SelectedDate,
                Remark = txtRemark.Text.Trim(),
                CreateUserId = member.UserId,
                DepartTime = DepartTime.SelectedDate,
                ArrivalTime = ArrivalTime.SelectedDate,
                ContainerNumber = ContainerNumber.Text.Trim(),
                BillNo = BillNo.Text.Trim(),
                StartPort = hidCountry.Value,
                StartPortID = hidCountryId.Value,
                EndPortID = ddlEndPort.SelectedValue,
                EndPort = ddlEndPort.SelectedItem.Text
            };

            if (Request["Id"] != null && Request["Id"].ToString() != "")
            {
                int tmpId = 0;
                if (int.TryParse(this.Page.Request["Id"], out tmpId))
                {
                    purchaseOrderInfo.id = tmpId;
                    if (PurchaseOrderHelper.EditPurchaseOrderInfo(purchaseOrderInfo))
                    {
                        base.Response.Redirect(Globals.GetAdminAbsolutePath("/POManage/POList.aspx"), true);
                        return;
                    }
                    else
                    {
                        this.ShowMsg("该单状态不允许操作或您无权操作该单", false);
                    }
                }
                else
                {
                    this.ShowMsg("未知参数", false);
                }
            }
            else
            {
                if (PurchaseOrderHelper.AddPurchaseOrderInfo(purchaseOrderInfo))
                {
                    base.Response.Redirect(Globals.GetAdminAbsolutePath("/POManage/POList.aspx"), true);
                    return;
                }
                else
                {
                    this.ShowMsg("添加失败,未知错误", false);
                }
            }
        }
    }
}
