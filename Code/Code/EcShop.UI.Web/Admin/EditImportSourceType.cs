using EcShop.ControlPanel.Commodities;
using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Entities.Commodities;
using EcShop.Entities.Store;
using EcShop.Membership.Context;
using EcShop.UI.Common.Controls;
using Ecdev.Components.Validation;
using kindeditor.Net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using EcShop.UI.ControlPanel.Utility;

namespace EcShop.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.ImportSourceTypeEdit)]
    public class EditImportSourceType : AdminPage
    {
        protected System.Web.UI.WebControls.Button btnAdd;
        protected System.Web.UI.WebControls.TextBox txtRemark;
        protected System.Web.UI.WebControls.TextBox txtCnArea;
        protected System.Web.UI.WebControls.TextBox txtEnArea;
        //protected System.Web.UI.WebControls.TextBox txtHSCode;
        protected System.Web.UI.WebControls.RadioButton radFavourableFlagY;
        protected System.Web.UI.WebControls.RadioButton radFavourableFlagN;
        protected ImageUploader uploaderIcon;
        protected DisplaySequenceDropDownList ddListDisplaySequence;
        private int importsId;
        protected System.Web.UI.WebControls.TextBox txtImportSourceId;
        protected System.Web.UI.WebControls.HiddenField hidCountryId;
        protected System.Web.UI.WebControls.HiddenField hidCountry;

        /// <summary>
        /// 国家
        /// </summary>
        //protected BaseCountryDropDownList ddlCountry;
        protected override void OnInitComplete(System.EventArgs e)
        {
            base.OnInitComplete(e);
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
        }
        protected void Page_Load(object sender, System.EventArgs e)
        {

            if (!this.Page.IsPostBack)
            {
                //this.ddlCountry.DataBind();
                this.ddListDisplaySequence.DataBind();

                if (int.TryParse(base.Request.QueryString["importSourceId"], out this.importsId))
                {
                    txtImportSourceId.Text = importsId.ToString();

                    ImportSourceTypeInfo ImInfo = ImportSourceTypeHelper.GetImportSourceTypeInfo(importsId);

                    if (ImInfo != null)
                    {
                        txtCnArea.Text = ImInfo.CnArea;
                        txtEnArea.Text = ImInfo.EnArea;
                        txtRemark.Text = ImInfo.Remark;
                        ddListDisplaySequence.SelectedValue = ImInfo.DisplaySequence;
                        uploaderIcon.UploadedImageUrl = ImInfo.Icon;
                        //txtHSCode.Text = ImInfo.HSCode;
                        if (ImInfo.FavourableFlag)
                        {
                            radFavourableFlagY.Checked = true;
                        }
                        else
                        {
                            radFavourableFlagN.Checked = true;
                        }
                        hidCountryId.Value = ImInfo.HSCode;
                        hidCountry.Value = ImInfo.StandardCName;

                    }
                }
            }
        }
        private void btnAdd_Click(object sender, System.EventArgs e)
        {
            if (hidCountryId.Value == "")
            {
                this.ShowMsg("请选择国家。", false);
                return;
            }
            //修改
            if (!string.IsNullOrEmpty(txtImportSourceId.Text))
            {
                int.TryParse(txtImportSourceId.Text, out this.importsId);

                if (importsId > 0)
                {
                    ImportSourceTypeInfo updateTypeInfo = new ImportSourceTypeInfo
                    {
                        CnArea = txtCnArea.Text,
                        EnArea = txtEnArea.Text,
                        Remark = txtRemark.Text,
                        Icon = uploaderIcon.UploadedImageUrl,
                        DisplaySequence = ddListDisplaySequence.SelectedValue,
                        ImportSourceId = importsId,
                        HSCode = hidCountryId.Value,
                        FavourableFlag = radFavourableFlagY.Checked,
                        StandardCName = hidCountry.Value
                    };

                    int importSourceId = ImportSourceTypeHelper.UpdateImportSourceType(updateTypeInfo);
                    if (importSourceId > 0)
                    {
                        HiCache.Remove("DataCache-ImportSourceTypeInfo");
                        this.ShowMsg("修改原产地成功", true);
                        base.Response.Redirect(Globals.GetAdminAbsolutePath(string.Format("/product/AddImportSourceTypeComplete.aspx?importSourceId={0}", importSourceId)), true);
                        return;
                    }
                    else
                    {
                        this.ShowMsg("修改原产地失败，未知错误", false);
                    }
                }

            }
        }
    }
}
