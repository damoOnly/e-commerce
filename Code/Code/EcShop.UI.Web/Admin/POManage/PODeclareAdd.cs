using EcShop.ControlPanel.Commodities;
using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Entities.PO;
using EcShop.Entities.Store;
using EcShop.Membership.Context;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcShop.UI.Web.Admin.PO
{
    [PrivilegeCheck(Privilege.PODeclareAdd)]
    public class PODeclareAdd : AdminPage
    {
        protected TransportTypeDropDownList TransportType;
        protected BusinessTypeDropDownList BusinessType;
        protected WrapTypeDropDownList WrapType;
        protected HSTradeTypeDropDownList TradeType;
        protected GetTaxDropDownList GetTax;
        protected HSUserTypeDropDownList UserType;
        protected ApplyortDrowpDownList Applyort;
        protected System.Web.UI.WebControls.Button btnSave;
        protected System.Web.UI.WebControls.TextBox transname;
        protected System.Web.UI.WebControls.TextBox cabinno;
        protected System.Web.UI.WebControls.TextBox FriNum;
        protected System.Web.UI.WebControls.TextBox Qty;
        protected System.Web.UI.WebControls.TextBox Gwt;
        protected System.Web.UI.WebControls.TextBox applyNum;
        protected System.Web.UI.WebControls.TextBox txtPONumber;
        protected System.Web.UI.WebControls.TextBox voyage;
        protected System.Web.UI.WebControls.DropDownList ContainerNumberType;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

            if (!this.Page.IsPostBack)
            {
                this.TransportType.DataBind();
                this.BusinessType.DataBind();
                this.WrapType.DataBind();
                this.TradeType.DataBind();
                this.GetTax.DataBind();
                this.UserType.DataBind();
                this.Applyort.DataBind();
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

        private void btnSave_Click(object sender, EventArgs e)
        {
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
            if (string.IsNullOrEmpty(txtPONumber.Text))
            {
                this.ShowMsg("获取PO单号失败", false);
                return;
            }
            if (string.IsNullOrEmpty(transname.Text))
            {
                this.ShowMsg("运输工具名称为空，请重新输入", false);
                return;
            }
            if (string.IsNullOrEmpty(voyage.Text))
            {
                this.ShowMsg("航班号为空，请重新输入", false);
                return;
            }
            if (string.IsNullOrEmpty(cabinno.Text))
            {
                this.ShowMsg("提运单号为空，请重新输入", false);
                return;
            }
            if (string.IsNullOrEmpty(FriNum.Text))
            {
                this.ShowMsg("法定数量为空，请重新输入", false);
                return;
            }
            if (ContainerNumberType.SelectedItem.Text == "--请选择规格--")
            {
                this.ShowMsg("请选择规格", false);
                return;
            }
            if (Applyort.SelectedItem.Text == "--请选择进口口岸--")
            {
                this.ShowMsg("请选择进口口岸", false);
                return;
            }
            if (GetTax.SelectedItem.Text == "--请选择征免方式--")
            {
                this.ShowMsg("请选择征免方式", false);
                return;
            }

            if (UserType.SelectedItem.Text == "--请选择用途--")
            {
                this.ShowMsg("请选择用途", false);
                return;
            }

            if (TransportType.SelectedItem.Text == "--请选择运输方式--")
            {
                this.ShowMsg("请选择运输方式", false);
                return;
            }
            if (BusinessType.SelectedItem.Text == "--请选择业务类型--")
            {
                this.ShowMsg("请选择业务类型", false);
                return;
            }
            if (WrapType.SelectedItem.Text == "--请选择包装种类--")
            {
                this.ShowMsg("请选择包装种类", false);
                return;
            }
            if (TradeType.SelectedItem.Text == "--请选择成交方式--")
            {
                this.ShowMsg("请选择成交方式", false);
                return;
            }
            if (Applyort.SelectedItem.Text == "--请选择进口口岸--")
            {
                this.ShowMsg("请选择进口口岸", false);
                return;
            }


            PODeclareInfo PODeclareInfo = new PODeclareInfo
            {
                TransportType = TransportType.SelectedItem.Text,
                TransportTypeCode = Convert.ToInt32(TransportType.SelectedValue),
                BusinessType = BusinessType.SelectedItem.Text,
                BusinessTypeCode = BusinessType.SelectedValue,
                WrapType = WrapType.SelectedItem.Text,
                WrapTypeCode = Convert.ToInt32(WrapType.SelectedValue),
                TradeType = TradeType.SelectedItem.Text,
                TradeTypeCode = Convert.ToInt32(TradeType.SelectedValue),
                //Qty = Convert.ToDouble(Qty.Text),
                //Gwt=Convert.ToDouble(Gwt.Text),
                transname=transname.Text,
                cabinno = cabinno.Text,
                //applyNum = Convert.ToInt32(applyNum.Text),
                FriNum=FriNum.Text,
                getTax = GetTax.SelectedItem.Text,
                getTaxCode = Convert.ToInt32(GetTax.SelectedValue),
                useType = UserType.SelectedItem.Text,
                useTypeCode = Convert.ToInt32(UserType.SelectedValue),
                applyort = Applyort.SelectedItem.Text,
                applyortCode = Convert.ToInt32(Applyort.SelectedValue),
                ContainerNumberType = ContainerNumberType.SelectedItem.Text,
                voyage=voyage.Text
            };

            if (Request["Id"] != null && Request["Id"].ToString() != "")
            {
                int tmpId = 0;
                if (int.TryParse(this.Page.Request["Id"], out tmpId))
                {
                    PODeclareInfo.id = tmpId;
                    if (PurchaseOrderHelper.SavePODeclareInfo(PODeclareInfo))
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
            
        }

        private void DataBind(string Id)
        {
            IList<PODeclareInfo> PODeclareInfo = PurchaseOrderHelper.GetPODeclareInfo("Id=" + Id);
            if (PODeclareInfo.Count > 0)
            {
                PODeclareInfo po = PODeclareInfo[0];
                txtPONumber.Text = po.PONumber;
                if (!string.IsNullOrEmpty(po.transname))
                {
                    transname.Text = po.transname.ToString();
                }
                if (!string.IsNullOrEmpty(po.cabinno))
                {
                    cabinno.Text = po.cabinno.ToString();
                }
                if (!string.IsNullOrEmpty(po.FriNum))
                {
                    FriNum.Text = po.FriNum.ToString();
                }
                if (!string.IsNullOrEmpty(po.useType))
                {
                    UserType.SelectedItem.Text = po.useType.ToString();
                    UserType.SelectedValue = po.useTypeCode.ToString();
                }
                if (!string.IsNullOrEmpty(po.TransportType))
                {
                    TransportType.SelectedItem.Text = po.TransportType.ToString();
                    TransportType.SelectedValue = po.TransportTypeCode.ToString();
                }
                if (!string.IsNullOrEmpty(po.BusinessType))
                {
                    BusinessType.SelectedItem.Text = po.BusinessType.ToString();
                    BusinessType.SelectedValue = po.BusinessTypeCode.ToString();
                }
                if (!string.IsNullOrEmpty(po.WrapType))
                {
                    WrapType.SelectedItem.Text = po.WrapType.ToString();
                    WrapType.SelectedValue = po.WrapTypeCode.ToString();
                }
                if (!string.IsNullOrEmpty(po.TradeType))
                {
                    TradeType.SelectedItem.Text = po.TradeType.ToString();
                    TradeType.SelectedValue = po.TradeTypeCode.ToString();
                }
                if (!string.IsNullOrEmpty(po.getTax))
                {
                    GetTax.SelectedItem.Text = po.getTax.ToString();
                    GetTax.SelectedValue = po.getTaxCode.ToString();
                }
                if (!string.IsNullOrEmpty(po.applyort))
                {
                    Applyort.SelectedItem.Text = po.applyort.ToString();
                    Applyort.SelectedValue = po.applyortCode.ToString();
                }
                if (!string.IsNullOrEmpty(po.ContainerNumberType))
                {
                    ContainerNumberType.SelectedItem.Text = po.ContainerNumberType.ToString();
                }
                if (!string.IsNullOrEmpty(po.voyage))
                {
                    voyage.Text = po.voyage.ToString();
                }
            }
        }
    }
}
