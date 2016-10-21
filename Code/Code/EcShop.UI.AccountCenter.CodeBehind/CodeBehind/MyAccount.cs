using EcShop.Membership.Context;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.AccountCenter.CodeBehind
{
    [System.Web.UI.ParseChildren(true)]
    public class MyAccount : MemberTemplatedWebControl
    {
        private System.Web.UI.WebControls.HyperLink hpQuesstionlink;
        private System.Web.UI.WebControls.HyperLink hpemaillink;
        private System.Web.UI.WebControls.HyperLink hpcellphonelink;
        private System.Web.UI.WebControls.HyperLink hpidentitycardlink;
        
        private System.Web.UI.WebControls.Literal litEmailVericetionName;
        private System.Web.UI.WebControls.Literal litCellphoneVericetionName;
        private System.Web.UI.WebControls.Literal litQuesstionName;
        private System.Web.UI.WebControls.Literal litIdentityCardVericetionName;//实名认证

        protected override void OnInit(System.EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "User/Skin-MyAccount.html";
            }
            base.OnInit(e);
        }
        protected override void AttachChildControls()
        {
            this.hpQuesstionlink = (System.Web.UI.WebControls.HyperLink)this.FindControl("hpQuesstionlink");
            this.hpemaillink = (System.Web.UI.WebControls.HyperLink)this.FindControl("hpemaillink");
            this.hpcellphonelink = (System.Web.UI.WebControls.HyperLink)this.FindControl("hpcellphonelink");
            this.hpidentitycardlink = (System.Web.UI.WebControls.HyperLink)this.FindControl("hpidentitycardlink");
            
            this.litEmailVericetionName = (System.Web.UI.WebControls.Literal)this.FindControl("litEmailVericetionName");
            this.litCellphoneVericetionName = (System.Web.UI.WebControls.Literal)this.FindControl("litCellphoneVericetionName");
            this.litQuesstionName = (System.Web.UI.WebControls.Literal)this.FindControl("litQuesstionName");
            this.litIdentityCardVericetionName = (System.Web.UI.WebControls.Literal)this.FindControl("litIdentityCardVericetionName");

            this.litEmailVericetionName.Text = "<b class=\"anquan_b2\">邮箱验证</b>";
            this.litCellphoneVericetionName.Text = "<b class=\"anquan_b2\">手机验证</b>";
            this.litQuesstionName.Text = "<b class=\"anquan_b2\">密码保护</b>";

            if (this.litIdentityCardVericetionName != null)
            {
                this.litIdentityCardVericetionName.Text = "<b class=\"anquan_b2\">实名认证</b>";
            }
            

            if (!this.Page.IsPostBack)
            {
                Member member = Users.GetUser(HiContext.Current.User.UserId, false) as Member;
                if (member.EmailVerification)
                {
                    this.litEmailVericetionName.Text = "<b class=\"anquan_b1\">邮箱验证</b>";
                }
                if (member.CellPhoneVerification)
                {
                    this.litCellphoneVericetionName.Text = "<b class=\"anquan_b1\">手机验证</b>";
                }
                if (!string.IsNullOrEmpty(member.PasswordQuestion))
                {
                    this.litQuesstionName.Text = "<b class=\"anquan_b1\">密码保护</b>";
                }


                if (member.IsVerify == 1)
                {
                    if (this.litIdentityCardVericetionName != null)
                    {
                        this.litIdentityCardVericetionName.Text = "<b class=\"anquan_b1\">实名认证</b>";
                    }
                }
                

                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                if (masterSettings.SMSEnabled && !string.IsNullOrEmpty(masterSettings.SMSSettings) && !member.CellPhoneVerification)
                {
                    this.hpcellphonelink.Text = "去验证";
                    this.hpcellphonelink.NavigateUrl = "UserCellPhoneVerification.aspx";
                }
                if (member.CellPhoneVerification)
                {
                    this.hpcellphonelink.Text = "已完成";
                }
                if (masterSettings.EmailEnabled && !string.IsNullOrEmpty(masterSettings.EmailSettings) && !member.EmailVerification)
                {
                    this.hpemaillink.Text = "去验证";
                    this.hpemaillink.NavigateUrl = "UserEmailVerification.aspx";
                }
                if (member.EmailVerification)
                {
                    this.hpemaillink.Text = "已完成";
                }
                if (!string.IsNullOrEmpty(member.PasswordQuestion))
                {
                    this.hpQuesstionlink.Text = "修改>>";
                    this.hpQuesstionlink.NavigateUrl = "UpdatePasswordProtection.aspx";
                }
                if (string.IsNullOrEmpty(member.PasswordQuestion))
                {
                    this.hpQuesstionlink.Text = "去添加";
                    this.hpQuesstionlink.NavigateUrl = "UpdatePasswordProtection.aspx";
                }
                

                //新实名认证
                if (member.IsVerify==1)
                {
                    this.hpidentitycardlink.Text = "已完成";
                }
                else
                {
                    this.hpidentitycardlink.Text = "去验证";
                    this.hpidentitycardlink.NavigateUrl = "IdentityCardVericetion.aspx";
                }
            }
        }
    }
}
