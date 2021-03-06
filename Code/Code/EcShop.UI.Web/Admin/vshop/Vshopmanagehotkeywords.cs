﻿using ASPNET.WebControls;
using EcShop.ControlPanel.Store;
using EcShop.Entities;
using EcShop.Entities.Store;
using EcShop.Membership.Context;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin.vshop
{
	[PrivilegeCheck(Privilege.HotKeywords)]
	public class Vshopmanagehotkeywords : AdminPage
	{
		protected System.Web.UI.HtmlControls.HtmlInputHidden txtHid;
		protected Grid grdHotKeywords;
		protected ProductCategoriesDropDownList dropCategory;
		protected System.Web.UI.WebControls.TextBox txtHotKeywords;
		protected System.Web.UI.WebControls.Button btnSubmitHotkeyword;
		protected System.Web.UI.WebControls.Button btnEditHotkeyword;
		protected ProductCategoriesDropDownList dropEditCategory;
		protected System.Web.UI.WebControls.TextBox txtEditHotKeyword;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hiHotKeyword;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hicategory;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnSubmitHotkeyword.Click += new System.EventHandler(this.btnSubmitHotkeyword_Click);
			this.grdHotKeywords.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdHotKeywords_RowDeleting);
			this.grdHotKeywords.RowCommand += new System.Web.UI.WebControls.GridViewCommandEventHandler(this.grdHotKeywords_RowCommand);
			this.btnEditHotkeyword.Click += new System.EventHandler(this.btnEditHotkeyword_Click);
			if (!this.Page.IsPostBack)
			{
				this.dropCategory.DataBind();
				this.dropEditCategory.DataBind();
				this.BindData();
			}
		}
		private void btnEditHotkeyword_Click(object sender, System.EventArgs e)
		{
			int hid = System.Convert.ToInt32(this.txtHid.Value);
			if (string.IsNullOrEmpty(this.txtEditHotKeyword.Text.Trim()) || this.txtEditHotKeyword.Text.Trim().Length > 60)
			{
				this.ShowMsg("热门关键字不能为空,长度限制在60个字符以内", false);
				return;
			}
            //if (!this.dropEditCategory.SelectedValue.HasValue)
            //{
            //    this.ShowMsg("请选择商品主分类", false);
            //    return;
            //}
			System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex("^(?!_)(?!.*?_$)(?!-)(?!.*?-$)[a-zA-Z0-9_一-龥-]+$");
			if (!regex.IsMatch(this.txtEditHotKeyword.Text.Trim()))
			{
				this.ShowMsg("热门关键字只能输入汉字,数字,英文,下划线,减号,不能以下划线、减号开头或结尾", false);
				return;
			}
            //if (string.Compare(this.txtEditHotKeyword.Text.Trim(), this.hiHotKeyword.Value) != 0 && this.IsSame(this.txtEditHotKeyword.Text.Trim(), System.Convert.ToInt32(this.dropEditCategory.SelectedValue.Value)))
            //{
            //    this.ShowMsg("存在相同的的关键字，编辑失败", false);
            //    return;
            //}
            //if (((string.Compare(this.dropEditCategory.SelectedValue.Value.ToString(), this.hicategory.Value) == 0 & string.Compare(this.txtEditHotKeyword.Text, this.hiHotKeyword.Value) != 0) && this.IsSame(this.txtEditHotKeyword.Text.Trim(), System.Convert.ToInt32(this.dropEditCategory.SelectedValue.Value))) || (string.Compare(this.txtEditHotKeyword.Text.Trim(), this.hiHotKeyword.Value) == 0 && string.Compare(this.dropEditCategory.SelectedValue.Value.ToString(), this.hicategory.Value) != 0 && this.IsSame(this.txtEditHotKeyword.Text.Trim(), System.Convert.ToInt32(this.dropEditCategory.SelectedValue.Value))))
            //{
            //    this.ShowMsg("同一分类型不允许存在相同的关键字,编辑失败", false);
            //    return;
            //}

            if (this.dropEditCategory.SelectedValue.HasValue)
            {
                if(string.Compare(this.txtEditHotKeyword.Text.Trim(), this.hiHotKeyword.Value)==0 && string.Compare(this.dropEditCategory.SelectedValue.Value.ToString(), this.hicategory.Value) == 0)
                {
                    return;
                }
                if (this.IsSame(this.txtEditHotKeyword.Text.Trim(), System.Convert.ToInt32(this.dropEditCategory.SelectedValue.Value)))
                {
                      this.ShowMsg("同一分类型不允许存在相同的关键字,编辑失败", false);
                       return;
                }
                StoreHelper.UpdateHotWords(hid, this.dropEditCategory.SelectedValue.Value, this.txtEditHotKeyword.Text.Trim(), HiContext.Current.User.UserId);
                this.ShowMsg("编辑热门关键字成功！", true);
                this.hicategory.Value = "";
                this.hiHotKeyword.Value = "";
                this.BindData();

            }
            else
            {
                if (string.Compare(this.txtEditHotKeyword.Text.Trim(), this.hiHotKeyword.Value) == 0 && string.Compare("", this.hicategory.Value) == 0)
                {
                    return;
                }

                if (this.IsSame(this.txtEditHotKeyword.Text.Trim(),null))
                {
                    this.ShowMsg("同一分类型不允许存在相同的关键字,编辑失败", false);
                    return;
                }

                StoreHelper.UpdateHotWords(hid, null, this.txtEditHotKeyword.Text.Trim(), HiContext.Current.User.UserId);
                this.ShowMsg("编辑热门关键字成功！", true);
                this.hicategory.Value = "";
                this.hiHotKeyword.Value = "";
                this.BindData();
            }
			
		}
		private void grdHotKeywords_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
		{
			if (e.CommandName == "Fall" || e.CommandName == "Rise")
			{
				int rowIndex = ((System.Web.UI.WebControls.GridViewRow)((System.Web.UI.Control)e.CommandSource).NamingContainer).RowIndex;
				int hid = (int)this.grdHotKeywords.DataKeys[rowIndex].Value;
				int displaySequence = int.Parse((this.grdHotKeywords.Rows[rowIndex].FindControl("lblDisplaySequence") as System.Web.UI.WebControls.Literal).Text);
				int num = 0;
				int replaceDisplaySequence = 0;
				if (e.CommandName == "Fall")
				{
					if (rowIndex + 1 != this.grdHotKeywords.Rows.Count)
					{
						num = (int)this.grdHotKeywords.DataKeys[rowIndex + 1].Value;
						replaceDisplaySequence = int.Parse((this.grdHotKeywords.Rows[rowIndex + 1].FindControl("lblDisplaySequence") as System.Web.UI.WebControls.Literal).Text);
					}
				}
				else
				{
					if (e.CommandName == "Rise" && rowIndex != 0)
					{
						num = (int)this.grdHotKeywords.DataKeys[rowIndex - 1].Value;
						replaceDisplaySequence = int.Parse((this.grdHotKeywords.Rows[rowIndex - 1].FindControl("lblDisplaySequence") as System.Web.UI.WebControls.Literal).Text);
					}
				}
				if (num != 0)
				{
					StoreHelper.SwapHotWordsSequence(hid, num, displaySequence, replaceDisplaySequence);
					this.BindData();
				}
			}
		}
		private void grdHotKeywords_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
		{
			int hid = (int)this.grdHotKeywords.DataKeys[e.RowIndex].Value;
			StoreHelper.DeleteHotKeywords(hid);
			this.BindData();
		}
		private void btnSubmitHotkeyword_Click(object sender, System.EventArgs e)
		{
			if (string.IsNullOrEmpty(this.txtHotKeywords.Text.Trim()))
			{
				this.ShowMsg("热门关键字不能为空", false);
				return;
			}
            //if (!this.dropCategory.SelectedValue.HasValue)
            //{
            //    this.ShowMsg("请选择商品主分类", false);
            //    return;
            //}
			string text = this.txtHotKeywords.Text.Trim().Replace("\r\n", "\n");
			string[] array = text.Replace("\n", "*").Split(new char[]
			{
				'*'
			});
			int num = 0;
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string text2 = array2[i];
				System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex("^(?!_)(?!.*?_$)(?!-)(?!.*?-$)[a-zA-Z0-9_一-龥-]+$");
                if (this.dropCategory.SelectedValue.HasValue)
                {
                    if (regex.IsMatch(text2) && !this.IsSame(text2, System.Convert.ToInt32(this.dropCategory.SelectedValue.Value)))
                    {
                        StoreHelper.AddHotkeywords(this.dropCategory.SelectedValue.Value, text2, ClientType.VShop, HiContext.Current.User.UserId);
                        num++;
                    }
                }
                else
                {
                    if (regex.IsMatch(text2) && !this.IsSame(text2, null))
                    {
                        StoreHelper.AddHotkeywords(null, text2, ClientType.VShop, HiContext.Current.User.UserId);
                        num++;
                    }
                }
			}
			if (num > 0)
			{
				this.ShowMsg(string.Format("成功添加了{0}个热门关键字", num), true);
				this.txtHotKeywords.Text = "";
				this.BindData();
				return;
			}
			this.ShowMsg("添加失败，请检查是否存在同类型的同名关键字", false);
		}
		private void BindData()
		{
            this.grdHotKeywords.DataSource = StoreHelper.GetHotKeywords(ClientType.VShop);
			this.grdHotKeywords.DataBind();
		}
		private bool IsSame(string word, int? categoryId)
		{
            System.Data.DataTable hotKeywords = StoreHelper.GetHotKeywords(ClientType.VShop);
			foreach (System.Data.DataRow dataRow in hotKeywords.Rows)
			{
				string b = dataRow["Keywords"].ToString();
                if (dataRow["CategoryId"]==DBNull.Value)
                {
                    if (word == b && categoryId == null)
                    {
                        return true;
                    }
                }
                else
                {
                    if (word == b && categoryId == System.Convert.ToInt32(dataRow["CategoryId"].ToString()))
                    {
                        return true;
                    }
                }
			}
			return false;
		}
	}
}

