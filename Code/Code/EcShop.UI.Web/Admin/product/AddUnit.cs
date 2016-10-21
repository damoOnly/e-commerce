using Ecdev.Components.Validation;
using EcShop.ControlPanel.Commodities;
using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Entities.Commodities;
using EcShop.Entities.Store;
using EcShop.Membership.Context;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using kindeditor.Net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace EcShop.UI.Web.Admin.product
{
    [PrivilegeCheck(Privilege.UnitsManager)]
    public class AddUnit : AdminPage
    {
        private string type;
        private int unitId;
        protected System.Web.UI.WebControls.TextBox txtHSJoinID;
        protected System.Web.UI.WebControls.TextBox txtUnitName;
        protected System.Web.UI.WebControls.Button btnSave;
        protected System.Web.UI.WebControls.Button btnAddUnit;
        protected void Page_Load(object sender, System.EventArgs e)
        {
            type = this.Page.Request.QueryString["type"];
            if (string.IsNullOrEmpty(type))
            {
                base.GotoResourceNotFound();
                return;
            }
            if (type == "edit" && !int.TryParse(this.Page.Request.QueryString["unitId"], out this.unitId))
            {
                base.GotoResourceNotFound();
                return;
            }
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            this.btnAddUnit.Click += new System.EventHandler(this.btnAddUnit_Click);
            if (!this.Page.IsPostBack)
            {
                if (type == "edit")
                {
                    this.btnAddUnit.Visible = false;
                    this.loadData();
                }
            }
        }

        /// <summary>
        /// 编辑时加载数据
        /// </summary>
        private void loadData()
        {
            DataTable dtUnits = CatalogHelper.GetUnitsById(this.unitId);
            if (dtUnits == null || dtUnits.Rows.Count <= 0)
            {
                base.GotoResourceNotFound();
                return;
            }

            this.txtUnitName.Text = Globals.HtmlDecode(dtUnits.Rows[0]["Name_CN"].ToString());
            this.txtHSJoinID.Text = Globals.HtmlDecode(dtUnits.Rows[0]["HSJoinID"].ToString());
        }

        private void btnSave_Click(object sender, System.EventArgs e)
        {
            if (type == "add")
            {
                AddBaseUnit(1);
            }
            else
            {
                EditBaseUnit();
            }
        }
        protected void btnAddUnit_Click(object sender, System.EventArgs e)
        {
            if (type == "add")
            {
                AddBaseUnit(2);
            }
        }

        /// <summary>
        /// 编辑计量单位
        /// </summary>
        private void EditBaseUnit()
        {
            ManagerHelper.CheckPrivilege(Privilege.UnitsEdit);
            BaseUnitsInfo baseUnitsInfo = this.GetBaseUnitsInfo();
            if (baseUnitsInfo == null || !this.ValidationUnit(baseUnitsInfo))
            {
                return;
            }
            var member = HiContext.Current.User;
            if (member == null || member.IsLockedOut)
            {
                return;
            }
            baseUnitsInfo.UpdateUser = member.Username + "_" + member.UserId;
            baseUnitsInfo.ID = this.unitId;
            if (CatalogHelper.IsExistUnit(baseUnitsInfo.HSJoinID, baseUnitsInfo.Name_CN, this.unitId))
            {
                this.ShowMsg("海关代码或计量单位名称已存在，请重新填写", true);
                return;
            }
            if (CatalogHelper.EditUnit(baseUnitsInfo))
            {
                base.Response.Redirect(Globals.GetAdminAbsolutePath("/product/BaseUnits.aspx"), true);
                return;
            }
            this.ShowMsg("编辑计量单位失败", true);
            return;
        }

        #region 添加计量单位
        /// <summary>
        /// 添加计量单位
        /// </summary>
        /// <param name="type">1：添加一个后跳转到列表页；2：连续添加</param>
        private void AddBaseUnit(int type)
        {
            ManagerHelper.CheckPrivilege(Privilege.UnitsAdd);
            BaseUnitsInfo baseUnitsInfo = this.GetBaseUnitsInfo();
            if (baseUnitsInfo == null || !this.ValidationUnit(baseUnitsInfo))
            {
                return;
            }
            var member = HiContext.Current.User;
            if (member == null || member.IsLockedOut)
            {
                return;
            }
            baseUnitsInfo.CreateUser = member.Username + "_" + member.UserId;

            if (CatalogHelper.IsExistUnit(baseUnitsInfo.HSJoinID, baseUnitsInfo.Name_CN, 0))
            {
                this.ShowMsg("海关代码或计量单位名称已存在，请重新填写", true);
                return;
            }
            if (CatalogHelper.AddUnit(baseUnitsInfo))
            {
                if (type == 1)
                {
                    base.Response.Redirect(Globals.GetAdminAbsolutePath("/product/BaseUnits.aspx"), true);
                }
                else
                {
                    this.ShowMsg("成功添加计量单位", true);
                }
                return;
            }
            this.ShowMsg("添加计量单位失败", true);
            return;
        }
        #endregion

        #region 获取页面传递计量单位数据
        /// <summary>
        /// 获取页面传递计量单位数据
        /// </summary>
        /// <returns></returns>
        private BaseUnitsInfo GetBaseUnitsInfo()
        {
            string HSJoinID = Globals.HtmlEncode(Globals.StripHtmlXmlTags(Globals.StripScriptTags(this.txtHSJoinID.Text.Trim())).Replace("\\", ""));
            if (string.IsNullOrEmpty(HSJoinID))
            {
                this.ShowMsg("请填写海关代码,海关代码中不能包含HTML字符，脚本字符，以及\\", false);
                return null;
            }

            string Name_CN = Globals.HtmlEncode(Globals.StripHtmlXmlTags(Globals.StripScriptTags(this.txtUnitName.Text.Trim())).Replace("\\", ""));
            if (string.IsNullOrEmpty(Name_CN))
            {
                this.ShowMsg("请填写计量单位名称,计量单位名称中不能包含HTML字符，脚本字符，以及\\", false);
                return null;
            }

            BaseUnitsInfo baseUnitsInfo = new BaseUnitsInfo();
            baseUnitsInfo.HSJoinID = HSJoinID;
            baseUnitsInfo.Name_CN = Name_CN;
            return baseUnitsInfo;
        }
        #endregion

        /// <summary>
        /// 验证数据格式
        /// </summary>
        /// <param name="baseUnits"></param>
        /// <returns></returns>
        private bool ValidationUnit(BaseUnitsInfo baseUnits)
        {
            ValidationResults validationResults = Validation.Validate<BaseUnitsInfo>(baseUnits, new string[]
			{
				"ValBaseUnits"
			});
            string text = string.Empty;
            if (!validationResults.IsValid)
            {
                foreach (ValidationResult current in (System.Collections.Generic.IEnumerable<ValidationResult>)validationResults)
                {
                    text += Formatter.FormatErrorMessage(current.Message);
                }
                this.ShowMsg(text, false);
            }
            return validationResults.IsValid;
        }
    }
}
