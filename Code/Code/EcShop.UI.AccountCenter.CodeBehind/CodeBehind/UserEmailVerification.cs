using EcShop.Core;
using EcShop.Membership.Context;
using EcShop.Membership.Core;
using System;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace EcShop.UI.AccountCenter.CodeBehind
{
    [System.Web.UI.ParseChildren(true)]
    public class UserEmailVerification : MemberTemplatedWebControl
    {
        private System.Web.UI.HtmlControls.HtmlInputText txtcode;
        private System.Web.UI.HtmlControls.HtmlInputText txtemail;
        private System.Web.UI.WebControls.Button btnSubmit;
        protected override void OnInit(System.EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "User/Skin-UserEmailVerification.html";
            }
            base.OnInit(e);
        }
        protected override void AttachChildControls()
        {
            this.txtcode = (System.Web.UI.HtmlControls.HtmlInputText)this.FindControl("txtcode");
            this.txtemail = (System.Web.UI.HtmlControls.HtmlInputText)this.FindControl("txtemail");
            this.btnSubmit = (System.Web.UI.WebControls.Button)this.FindControl("btnSubmit");
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            string userName = HiContext.Current.User.Username;
            //Regex reg = new Regex("^[0-9]*$");
            ////判断是否为邮箱注册用户 是绑定邮箱到txtemail
            //if (!reg.IsMatch(userName))
            //{
            //    this.txtemail.Disabled = true;
            //    this.txtemail.Value = userName;
            //}
        }
        private void btnSubmit_Click(object sender, System.EventArgs e)
        {
            string value = this.txtemail.Value;
            if (string.IsNullOrEmpty(value))
            {
                this.ShowMessage("邮箱不允许为空！", false);
                return;
            }
            if (value.Length > 256 || !Regex.IsMatch(value, "([a-zA-Z\\.0-9_-])+@([a-zA-Z0-9_-])+((\\.[a-zA-Z0-9_-]{2,4}){1,2})"))
            {
                this.ShowMessage("请输入正确的邮箱账号", false);
                return;
            }
            if (string.IsNullOrEmpty(this.txtcode.Value))
            {
                this.ShowMessage("验证码不允许为空！", false);
                return;
            }
            object obj = HiCache.Get(HiContext.Current.User.UserId + "email");
            if (obj == null)
            {
                this.ShowMessage("验证码错误！", false);
                return;
            }
            if (this.txtcode.Value.ToLower() != obj.ToString().ToLower())
            {
                this.ShowMessage("验证码输入错误！", false);
                return;
            }
            int userIdByEmail = UserHelper.GetUserIdByEmail(value);
            if (userIdByEmail != HiContext.Current.User.UserId && userIdByEmail != 0)
            {
                this.ShowMessage("该邮箱已被其它用户使用了,请更换其它邮箱！", false);
                return;
            }
            Member member = Users.GetUser(HiContext.Current.User.UserId, true) as Member;
            member.EmailVerification = true;
            member.Email = value;
            if (Users.UpdateUser(member))
            {
                HiCache.Remove(HiContext.Current.User.UserId + "email");
                this.Page.Response.Redirect("VerificationSuccess.aspx?type=1");
                return;
            }
            this.ShowMessage("发送验证码失败", false);
        }
    }
}
