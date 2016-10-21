using ASPNET.WebControls;
using EcShop.ControlPanel.Commodities;
using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Core.Enums;
using EcShop.Entities.Commodities;
using EcShop.Entities.Store;
using EcShop.Membership.Context;
using EcShop.UI.ControlPanel.Utility;
using Ionic.Zip;
using Ionic.Zlib;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin.product
{
    [PrivilegeCheck(Privilege.UnitsView)]
    public partial class BaseUnits : AdminPage
    {
        protected System.Web.UI.WebControls.TextBox txtSearchText;
        protected System.Web.UI.WebControls.Button btnSearchButton;
        protected System.Web.UI.WebControls.LinkButton btnorder;
        protected Grid grdUnitList;
        protected override void OnInitComplete(System.EventArgs e)
        {
            base.OnInitComplete(e);
            this.grdUnitList.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdUnitList_RowDeleting);
            this.btnSearchButton.Click += new System.EventHandler(this.btnSearchButton_Click);
            this.btnorder.Click += new System.EventHandler(this.btnorder_Click);
        }
        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.BindUnits();
            }
        }

        /// <summary>
        /// 数据绑定
        /// </summary>
        private void BindUnits()
        {
            this.grdUnitList.DataSource = CatalogHelper.GetUnits();
            this.grdUnitList.DataBind();
        }

        protected void grdUnitList_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            ManagerHelper.CheckPrivilege(Privilege.UnitsDelete);
            int UnitId = (int)this.grdUnitList.DataKeys[e.RowIndex].Value;
            var member = HiContext.Current.User;
            if (member == null || member.IsLockedOut)
            {
                return;
            }

            if (CatalogHelper.DeleteUnit(UnitId, member.Username + "_" + member.UserId))
            {
                this.ShowMsg("成功删除计量单位", true);
            }
            else
            {
                this.ShowMsg("删除计量单位失败", false);
            }
            this.BindUnits();
        }
        

        protected void btnSearchButton_Click(object sender, System.EventArgs e)
        {
            string Key = this.txtSearchText.Text.Trim();
            this.grdUnitList.DataSource = CatalogHelper.GetUnits(Key);
            this.grdUnitList.DataBind();
        }
        protected void btnorder_Click(object sender, System.EventArgs e)
        {
            try
            {
                ManagerHelper.CheckPrivilege(Privilege.UnitsEdit);
                bool flag = true;
                for (int i = 0; i < this.grdUnitList.Rows.Count; i++)
                {
                    int UnitId = (int)this.grdUnitList.DataKeys[i].Value;
                    int displaysequence = int.Parse((this.grdUnitList.Rows[i].Cells[2].Controls[1] as System.Web.UI.HtmlControls.HtmlInputText).Value);
                    if (!CatalogHelper.UpdateUnitDisplaySequence(UnitId, displaysequence))
                    {
                        flag = false;
                    }
                }
                if (flag)
                {
                    this.ShowMsg("批量更新排序成功！", true);
                    this.BindUnits();
                }
                else
                {
                    this.ShowMsg("批量更新排序失败！", false);
                }
            }
            catch (System.Exception ex)
            {
                this.ShowMsg("批量更新排序失败！" + ex.Message, false);
            }
        }
    }
}
