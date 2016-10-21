using ASPNET.WebControls;
using EcShop.ControlPanel.Commodities;
using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Core.Enums;
using EcShop.Entities.Commodities;
using EcShop.Entities.Store;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.Web.UI.WebControls;

namespace EcShop.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.BrandType)]
    public class BrandType : AdminPage
    {
        protected System.Web.UI.WebControls.TextBox txtSearchText;
        protected System.Web.UI.WebControls.Button btnSearchButton;
        protected System.Web.UI.WebControls.LinkButton btnorder;
        protected Grid grdBrandCategriesList;
        protected override void OnInitComplete(System.EventArgs e)
        {
            base.OnInitComplete(e);
            this.grdBrandCategriesList.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdBrandCategriesList_RowDeleting);
            this.grdBrandCategriesList.RowCommand += new System.Web.UI.WebControls.GridViewCommandEventHandler(this.grdBrandCategriesList_RowCommand);
            this.btnSearchButton.Click += new System.EventHandler(this.btnSearchButton_Click);
            this.btnorder.Click += new System.EventHandler(this.btnorder_Click);
        
        }
        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.BindBrandTags();
            }
        }
        protected void grdBrandCategriesList_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            int brandId = (int)this.grdBrandCategriesList.DataKeys[e.RowIndex].Value;
            if (CatalogHelper.BrandTagHaveBrand(brandId))
            {
                this.ShowMsg("选择的品牌标签下还有品牌关联，删除失败", false);
                return;
            }
            if (CatalogHelper.DeleteBrandTags(brandId))
            {
                this.ShowMsg("成功删除品牌标签", true);
            }
            else
            {
                this.ShowMsg("删除品牌标签失败", false);
            }
            this.BindBrandTags();
        }
        protected void grdBrandCategriesList_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            int rowIndex = ((System.Web.UI.WebControls.GridViewRow)((System.Web.UI.Control)e.CommandSource).NamingContainer).RowIndex;
            int brandId = (int)this.grdBrandCategriesList.DataKeys[rowIndex].Value;
            if (e.CommandName == "Rise")
            {
                if (rowIndex != this.grdBrandCategriesList.Rows.Count)
                {
                    CatalogHelper.UpdateBrandCategorieDisplaySequence(brandId, SortAction.Asc);
                    this.BindBrandTags();
                    return;
                }
            }
            else
            {
                if (e.CommandName == "Fall")
                {
                    CatalogHelper.UpdateBrandCategorieDisplaySequence(brandId, SortAction.Desc);
                    this.BindBrandTags();
                }
            }
        }
        private void BindBrandTags()
        {
            this.grdBrandCategriesList.DataSource = CatalogHelper.GetBrandTags();
            this.grdBrandCategriesList.DataBind();
        }
      
        protected void btnSearchButton_Click(object sender, System.EventArgs e)
        {
            string brandName = this.txtSearchText.Text.Trim();
            this.grdBrandCategriesList.DataSource = CatalogHelper.GetBrandTags(brandName);
            this.grdBrandCategriesList.DataBind();
        }
        protected void btnorder_Click(object sender, System.EventArgs e)
        {
            try
            {
                bool flag = true;
                for (int i = 0; i < this.grdBrandCategriesList.Rows.Count; i++)
                {
                    int brandTagId = (int)this.grdBrandCategriesList.DataKeys[i].Value;
                    int displaysequence = int.Parse((this.grdBrandCategriesList.Rows[i].Cells[1].Controls[1] as System.Web.UI.HtmlControls.HtmlInputText).Value);
                    if (!CatalogHelper.UpdateBrandTagDisplaySequence(brandTagId, displaysequence))
                    {
                        flag = false;
                    }
                }
                if (flag)
                {
                    this.ShowMsg("批量更新排序成功！", true);
                    this.BindBrandTags();
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
