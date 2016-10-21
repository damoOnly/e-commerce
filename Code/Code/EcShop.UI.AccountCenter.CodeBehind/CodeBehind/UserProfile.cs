using ASPNET.WebControls;
using EcShop.Core;
using EcShop.Entities;
using EcShop.Membership.Context;
using EcShop.UI.Common.Controls;
using Ecdev.Components.Validation;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using EcShop.Membership.Core;
using EcShop.Entities.YJF;
using System.Configuration;
using EcShop.ControlPanel.Members;
using EcShop.ControlPanel.Sales;
using EcShop.ControlPanel.Comments;
using Members;
namespace EcShop.UI.AccountCenter.CodeBehind
{
	public class UserProfile : MemberTemplatedWebControl
	{
		private SmallStatusMessage Statuses;
		private System.Web.UI.WebControls.TextBox txtRealName;
        private System.Web.UI.WebControls.TextBox txtIdentityCard;
		private System.Web.UI.WebControls.TextBox txtAddress;
		private System.Web.UI.WebControls.TextBox txtQQ;
		private System.Web.UI.WebControls.TextBox txtMSN;
		private System.Web.UI.WebControls.TextBox txtTel;
		private System.Web.UI.WebControls.TextBox txtHandSet;
		private System.Web.UI.WebControls.TextBox txtwangwang;
        private System.Web.UI.WebControls.TextBox txtRecommendCode;
		private RegionSelector dropRegionsSelect;
		private GenderRadioButtonList gender;
		private WebCalendar calendDate;
		private IButton btnOK1;
		protected virtual void ShowMsgs(SmallStatusMessage state, string msg, bool success)
		{
			if (state != null)
			{
				state.Success = success;
				state.Text = msg;
				state.Visible = true;
			}
		}
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-UserProfile.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.txtRealName = (System.Web.UI.WebControls.TextBox)this.FindControl("txtRealName");
            this.txtIdentityCard = (System.Web.UI.WebControls.TextBox)this.FindControl("txtIdentityCard");
			this.dropRegionsSelect = (RegionSelector)this.FindControl("dropRegions");
			this.gender = (GenderRadioButtonList)this.FindControl("gender");
			this.calendDate = (WebCalendar)this.FindControl("calendDate");
			this.txtAddress = (System.Web.UI.WebControls.TextBox)this.FindControl("txtAddress");
			this.txtQQ = (System.Web.UI.WebControls.TextBox)this.FindControl("txtQQ");
			this.txtMSN = (System.Web.UI.WebControls.TextBox)this.FindControl("txtMSN");
			this.txtTel = (System.Web.UI.WebControls.TextBox)this.FindControl("txtTel");
			this.txtwangwang = (System.Web.UI.WebControls.TextBox)this.FindControl("txtwangwang");
			this.txtHandSet = (System.Web.UI.WebControls.TextBox)this.FindControl("txtHandSet");
            this.txtRecommendCode = (System.Web.UI.WebControls.TextBox)this.FindControl("txtRecommendCode");
			this.btnOK1 = ButtonManager.Create(this.FindControl("btnOK1"));
			this.Statuses = (SmallStatusMessage)this.FindControl("Statuses");
			this.btnOK1.Click += new System.EventHandler(this.btnOK1_Click);
			PageTitle.AddSiteNameTitle("个人信息");
			if (!this.Page.IsPostBack)
			{
				Member member = HiContext.Current.User as Member;
				if (member != null)
				{
					this.BindData(member);
				}
			}
		}
		private void btnOK1_Click(object sender, System.EventArgs e)
		{
			Member member = Users.GetUser(HiContext.Current.User.UserId, true) as Member;

            string patternIdentityCard = "^[1-9]\\d{5}[1-9]\\d{3}((0[1-9])|(1[0-2]))((0[1-9])|([1-2][0-9])|(3[0-1]))\\d{3}(\\d|x|X)";
            System.Text.RegularExpressions.Regex regexIdentityCard = new System.Text.RegularExpressions.Regex(patternIdentityCard);
            if (!string.IsNullOrEmpty(this.txtIdentityCard.Text) && !regexIdentityCard.IsMatch(this.txtIdentityCard.Text.Trim()))
            {
                this.ShowMessage("请输入正确的身份证号码", false);
                return;
            }
			if (!this.dropRegionsSelect.GetSelectedRegionId().HasValue)
			{
				member.RegionId = 0;
			}
			else
			{
				member.RegionId = this.dropRegionsSelect.GetSelectedRegionId().Value;
				member.TopRegionId = RegionHelper.GetTopRegionId(member.RegionId);
			}
			member.Gender = this.gender.SelectedValue;
			member.BirthDate = this.calendDate.SelectedDate;
			member.Address = Globals.HtmlEncode(this.txtAddress.Text);
			member.QQ = Globals.HtmlEncode(this.txtQQ.Text);
			member.MSN = Globals.HtmlEncode(this.txtMSN.Text);
			member.TelPhone = Globals.HtmlEncode(this.txtTel.Text);
			member.CellPhone = Globals.HtmlEncode(this.txtHandSet.Text);
			member.Wangwang = Globals.HtmlEncode(this.txtwangwang.Text);
            if (string.IsNullOrWhiteSpace(member.IdentityCard))
            {
                member.IdentityCard = "";
            }

            if ((member.IdentityCard.ToUpper() != this.txtIdentityCard.Text.ToUpper() || member.RealName != this.txtRealName.Text) && UserHelper.IsExistIdentityCard(txtIdentityCard.Text.Trim(), HiContext.Current.User.UserId) > 0)
            {
                this.ShowMessage("存在已认证的身份证号码，不能保存!", false);
                return;
            }

            if (!OrderHelper.Checkisverify(HiContext.Current.User.UserId) || member.IdentityCard.ToUpper() != this.txtIdentityCard.Text.ToUpper() || member.RealName != this.txtRealName.Text)
            {
                int MaxCount = string.IsNullOrEmpty(ConfigurationManager.AppSettings["IsVerifyCount"]) ? 3 : int.Parse(ConfigurationManager.AppSettings["IsVerifyCount"]);
                int fcount = MemberHelper.CheckErrorCount(HiContext.Current.User.UserId);
                if (fcount >= MaxCount)//超过失败次数
                {
                    this.ShowMessage("您今天已提交验证" + (MaxCount) + "次，不能再验证！", false);
                    return;
                }

                CertNoValidHelper cv = new CertNoValidHelper();
                string rest = cv.CertNoValid(this.txtRealName.Text, this.txtIdentityCard.Text.ToUpper());
                MemberHelper.AddIsVerifyMsg(HiContext.Current.User.UserId);

                if (!rest.Equals("实名通过"))
                {
                    string message = "验证失败，您还有" + (MaxCount - fcount - 1) + "次验证机会,请确认身份信息是否有误！";
                    if ((MaxCount - fcount - 1) == 0)
                    {
                        message = "您今天已提交验证" + (MaxCount) + "次，不能再验证！";
                    }
                    this.ShowMessage(message, false);
                    return;
                }
            }
            member.IdentityCard = this.txtIdentityCard.Text.ToUpper();
			member.RealName = Globals.HtmlEncode(this.txtRealName.Text);
            member.IsVerify = 1;
            member.VerifyDate = DateTime.Now;
			if (!this.ValidationMember(member))
			{
				return;
			}
            if (!string.IsNullOrWhiteSpace(member.Email))
            {
                if (UserHelper.IsExistEmal(member.Email, member.Username))
                {
                    this.ShowMessage("邮箱已被使用过", false);
                    return;
                }
            }
			if (Users.UpdateUser(member))
			{
				this.ShowMessage("成功的修改了用户的个人资料", true);
				return;
			}
			this.ShowMessage("修改用户个人资料失败", false);
		}
		private void BindData(Member user)
		{
			this.txtRealName.Text = Globals.HtmlDecode(user.RealName);
			this.gender.SelectedValue = user.Gender;
			if (user.BirthDate > System.DateTime.MinValue)
			{
				this.calendDate.SelectedDate = user.BirthDate;
			}
			this.dropRegionsSelect.SetSelectedRegionId(new int?(user.RegionId));
			this.txtAddress.Text = Globals.HtmlDecode(user.Address);
			this.txtQQ.Text = Globals.HtmlDecode(user.QQ);
			this.txtMSN.Text = Globals.HtmlDecode(user.MSN);
			this.txtTel.Text = Globals.HtmlDecode(user.TelPhone);
			this.txtHandSet.Text = Globals.HtmlDecode(user.CellPhone);
			this.txtwangwang.Text = Globals.HtmlDecode(user.Wangwang);
            this.txtIdentityCard.Text = Globals.HtmlDecode(user.IdentityCard);
            // 判断是否有邀请码，没有就给与，有就用原来的
            this.txtRecommendCode.Text = MemberHelper.GetRecemmendCode(user.UserId);
		}
		private bool ValidationMember(Member member)
		{
			ValidationResults validationResults = Validation.Validate<Member>(member, new string[]
			{
				"ValMember"
			});
			string text = string.Empty;
			if (!validationResults.IsValid)
			{
				foreach (ValidationResult current in (System.Collections.Generic.IEnumerable<ValidationResult>)validationResults)
				{
					if (current.Message.IndexOf("MSN") > -1)
					{
						text += Formatter.FormatErrorMessage(current.Message.Replace("MSN", "微信"));
					}
					else
					{
						text += Formatter.FormatErrorMessage(current.Message);
					}
				}
				this.ShowMessage(text, false);
			}
			return validationResults.IsValid;
		}
	
    }
}
