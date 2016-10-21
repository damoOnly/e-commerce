using EcShop.Core;
using EcShop.Entities.Sales;
using EcShop.Membership.Context;
using EcShop.Membership.Core;
using EcShop.SaleSystem.Member;
using EcShop.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
namespace EcShop.UI.AccountCenter.CodeBehind
{
	public class UserShippingAddresses : MemberTemplatedWebControl
	{
		private System.Web.UI.WebControls.TextBox txtShipTo;
		private System.Web.UI.WebControls.TextBox txtAddress;
		private System.Web.UI.WebControls.TextBox txtZipcode;
		private System.Web.UI.WebControls.TextBox txtTelPhone;
		private System.Web.UI.WebControls.TextBox txtCellPhone;
		private RegionSelector dropRegionsSelect;
		private IButton btnAddAddress;
		private System.Web.UI.WebControls.Literal lblAddressCount;
		private Common_Address_AddressList dtlstRegionsSelect;
		private static int shippingId;
        private System.Web.UI.WebControls.TextBox txtShippingId;

		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-UserShippingAddresses.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.txtShipTo = (System.Web.UI.WebControls.TextBox)this.FindControl("txtShipTo");
            this.txtShippingId = (System.Web.UI.WebControls.TextBox)this.FindControl("txtShippingId");
			this.txtAddress = (System.Web.UI.WebControls.TextBox)this.FindControl("txtAddress");
			this.txtZipcode = (System.Web.UI.WebControls.TextBox)this.FindControl("txtZipcode");
			this.txtTelPhone = (System.Web.UI.WebControls.TextBox)this.FindControl("txtTelPhone");
			this.txtCellPhone = (System.Web.UI.WebControls.TextBox)this.FindControl("txtCellPhone");
			this.dropRegionsSelect = (RegionSelector)this.FindControl("dropRegions");
			this.btnAddAddress = ButtonManager.Create(this.FindControl("btnAddAddress"));
			this.dtlstRegionsSelect = (Common_Address_AddressList)this.FindControl("list_Common_Consignee_ConsigneeList");
			this.lblAddressCount = (System.Web.UI.WebControls.Literal)this.FindControl("lblAddressCount");
			this.btnAddAddress.Click += new System.EventHandler(this.btnAddAddress_Click);
			this.dtlstRegionsSelect.ItemCommand += new Common_Address_AddressList.CommandEventHandler(this.dtlstRegionsSelect_ItemCommand);
			PageTitle.AddSiteNameTitle("我的收货地址");
			if (!this.Page.IsPostBack)
			{
				this.lblAddressCount.Text = HiContext.Current.Config.ShippingAddressQuantity.ToString();
				this.dropRegionsSelect.DataBind();
				this.Reset();
				this.BindList();
                try
                {
                    Member member = HiContext.Current.User as Member;
                    if (member != null)
                    {
                        this.txtShipTo.Text = member.RealName;
                        this.txtCellPhone.Text = member.CellPhone;
                    }
                }
                catch (Exception ee)
                { }
			}
           
           
		}
		protected void btnCancel_Click(object sender, System.EventArgs e)
		{
			this.Reset();
		}
		protected void btnEditAddress_Click(object sender, System.EventArgs e)
		{
			if (!this.ValShippingAddress())
			{
				return;
			}
			ShippingAddressInfo shippingAddressInfo = this.GetShippingAddressInfo();
			shippingAddressInfo.ShippingId = System.Convert.ToInt32(this.ViewState["shippingId"]);
            if (txtShippingId != null)
            {
                txtShippingId.Text = shippingAddressInfo.ShippingId.ToString();
            }
			if (MemberProcessor.UpdateShippingAddress(shippingAddressInfo))
			{
				this.ShowMessage("成功的修改了一个收货地址", true);
                txtShippingId.Text = "";
				this.Reset();
			}
			else
			{
				this.ShowMessage("地址已经在，请重新输入一次再试", false);
			}
			this.btnAddAddress.Visible = true;
			this.BindList();
		}
		private bool ValShippingAddress()
		{
            //Regex regex = new Regex("[\\u4e00-\\u9fa5a-zA-Z]+[\\u4e00-\\u9fa5_a-zA-Z0-9]*");

            Regex regex = new Regex("^[\\u4e00-\\u9fa5]{2,6}$");
            string patternIdentityCard = "^[1-9]\\d{5}[1-9]\\d{3}((0[1-9])|(1[0-2]))((0[1-9])|([1-2][0-9])|(3[0-1]))\\d{3}(\\d|x|X)";
            System.Text.RegularExpressions.Regex regexIdentityCard = new System.Text.RegularExpressions.Regex(patternIdentityCard);

			if (string.IsNullOrEmpty(this.txtShipTo.Text.Trim()) || !regex.IsMatch(this.txtShipTo.Text.Trim()))
			{
				//this.ShowMessage("收货人名字不能为空，只能是汉字或字母开头，长度在2-20个字符之间", false);
                this.ShowMessage("收货人名字只能为2-6个汉字", false);
				return false;
			}
			if (string.IsNullOrEmpty(this.txtAddress.Text.Trim()))
			{
				this.ShowMessage("详细地址不能为空", false);
				return false;
			}
			if (this.txtAddress.Text.Trim().Length < 3 || this.txtAddress.Text.Trim().Length > 60)
			{
				this.ShowMessage("详细地址长度在3-60个字符之间", false);
				return false;
			}
			if (!this.dropRegionsSelect.GetSelectedRegionId().HasValue || this.dropRegionsSelect.GetSelectedRegionId().Value == 0)
			{
				this.ShowMessage("请选择收货地址", false);
				return false;
			}
			if (string.IsNullOrEmpty(this.txtTelPhone.Text.Trim()) && string.IsNullOrEmpty(this.txtCellPhone.Text.Trim()))
			{
				this.ShowMessage("电话号码和手机二者必填其一", false);
				return false;
			}
			if (!string.IsNullOrEmpty(this.txtTelPhone.Text.Trim()) && (this.txtTelPhone.Text.Trim().Length < 3 || this.txtTelPhone.Text.Trim().Length > 20))
			{
				this.ShowMessage("电话号码长度限制在3-20个字符之间", false);
				return false;
			}
			if (!string.IsNullOrEmpty(this.txtCellPhone.Text.Trim()) && (this.txtCellPhone.Text.Trim().Length < 3 || this.txtCellPhone.Text.Trim().Length > 20))
			{
				this.ShowMessage("手机号码长度限制在3-20个字符之间", false);
				return false;
			}
          
			return true;
		}
		protected void btnAddAddress_Click(object sender, System.EventArgs e)
		{
			if (!this.ValShippingAddress())
			{
				return;
			}
            if (!string.IsNullOrEmpty(txtShippingId.Text))
			{
				ShippingAddressInfo shippingAddressInfo = this.GetShippingAddressInfo();
				shippingAddressInfo.ShippingId = System.Convert.ToInt32(this.ViewState["shippingId"]);
                if (txtShippingId != null)
                {
                    txtShippingId.Text = shippingAddressInfo.ShippingId.ToString();
                }
				if (MemberProcessor.UpdateShippingAddress(shippingAddressInfo))
				{
                   
					this.ShowMessage("成功的修改了一个收货地址", true);
                    //this.btnAddAddress.Text = "添加";
                    //txtShippingId.Text = "";
                    //this.Reset();
                    this.Page.Response.Redirect("UserShippingAddresses.aspx");
                    return;
				}
				else
				{
					this.ShowMessage("地址已经在，请重新输入一次再试", false);
                    return;
				}
			}
			else
			{
				int shippingAddressCount = MemberProcessor.GetShippingAddressCount();
				if (shippingAddressCount >= HiContext.Current.Config.ShippingAddressQuantity)
				{
					this.ShowMessage(string.Format("最多只能添加{0}个收货地址", HiContext.Current.Config.ShippingAddressQuantity), false);
					this.Reset();
					return;
				}
				ShippingAddressInfo shippingAddressInfo2 = this.GetShippingAddressInfo();
				if (shippingAddressCount == 0)
				{
					shippingAddressInfo2.IsDefault = true;
				}
				if (MemberProcessor.AddShippingAddress(shippingAddressInfo2) > 0)
				{
					this.ShowMessage("成功的添加了一个收货地址", true);
					//this.Reset();
                    this.Page.Response.Redirect("UserShippingAddresses.aspx");
                    return;
				}
				else
				{
					this.ShowMessage("地址已经在，请重新输入一次再试", false);
                    return;
				}
			}
            this.BindList();
		}
		protected void dtlstRegionsSelect_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
		{
			int num = System.Convert.ToInt32(e.CommandArgument);
            if (txtShippingId != null)
            {
                txtShippingId.Text = num.ToString();
            }
			this.ViewState["shippingId"] = num;
			if (e.CommandName == "Edit")
			{
				ShippingAddressInfo shippingAddress = MemberProcessor.GetShippingAddress(num);
				if (shippingAddress != null)
				{
					this.txtShipTo.Text = shippingAddress.ShipTo;
					this.dropRegionsSelect.SetSelectedRegionId(new int?(shippingAddress.RegionId));
					this.txtAddress.Text = shippingAddress.Address;
					this.txtZipcode.Text = shippingAddress.Zipcode;
					this.txtTelPhone.Text = shippingAddress.TelPhone;
					this.txtCellPhone.Text = shippingAddress.CellPhone;
                  
					this.btnAddAddress.Text = "保存";
                    return;
				}
			}
			if (e.CommandName == "Delete")
			{
				if (MemberProcessor.DelShippingAddress(num, HiContext.Current.User.UserId))
				{
					this.ShowMessage("成功的删除了你要删除的记录", true);
					this.txtShipTo.Text = string.Empty;
					this.txtAddress.Text = string.Empty;
					this.txtZipcode.Text = string.Empty;
					this.txtTelPhone.Text = string.Empty;
					this.txtCellPhone.Text = string.Empty;
                   
                    txtShippingId.Text = "";
                    this.Page.Response.Redirect("UserShippingAddresses.aspx");
                    return;
				}
				else
				{
					this.ShowMessage("删除失败", false);
				}
			}
            if (e.CommandName == "SetDefault")
            {
                if (MemberProcessor.SetDefaultShippingAddressPC(num, HiContext.Current.User.UserId))
                {
                    this.Page.Response.Redirect("UserShippingAddresses.aspx");
                }
                else
                {
                    this.ShowMessage("设置默认失败", false);
                }
            }
            this.BindList();
		}
		private ShippingAddressInfo GetShippingAddressInfo()
		{
			return new ShippingAddressInfo
			{
				UserId = HiContext.Current.User.UserId,
				ShipTo = this.txtShipTo.Text,
				RegionId = this.dropRegionsSelect.GetSelectedRegionId().Value,
				Address = this.txtAddress.Text,
				Zipcode = this.txtZipcode.Text,
				CellPhone = this.txtCellPhone.Text,
				TelPhone = this.txtTelPhone.Text,
                IdentityCard =""
			};
		}
		private void BindList()
		{
			System.Collections.Generic.IList<ShippingAddressInfo> shippingAddresses = MemberProcessor.GetShippingAddresses();
			this.dtlstRegionsSelect.DataSource = shippingAddresses;
			this.dtlstRegionsSelect.DataBind();
		}
		private void Reset()
		{
			this.txtShipTo.Text = string.Empty;
			this.dropRegionsSelect.SetSelectedRegionId(null);
			this.txtAddress.Text = string.Empty;
			this.txtZipcode.Text = string.Empty;
			this.txtTelPhone.Text = string.Empty;
			this.txtCellPhone.Text = string.Empty;
			UserShippingAddresses.shippingId = 0;
			this.btnAddAddress.Visible = true;
		}
	}
}
