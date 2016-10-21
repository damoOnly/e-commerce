using EcShop.ControlPanel.Members;
using EcShop.ControlPanel.Sales;
using EcShop.Core;
using EcShop.Entities.YJF;
using EcShop.Membership.Context;
using EcShop.Membership.Core;
using EcShop.UI.Common.Controls;
using Members;
using System;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace EcShop.UI.AccountCenter.CodeBehind
{
    public class IdentityCardVericetion : MemberTemplatedWebControl
    {
        private System.Web.UI.WebControls.TextBox txtRealName;
        private System.Web.UI.WebControls.TextBox txtIdentityCard;
        private System.Web.UI.WebControls.Button btnSubmit;
        protected override void OnInit(System.EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "User/Skin-IdentityCardVericetion.html";
            }
            base.OnInit(e);
        }
        protected override void AttachChildControls()
        {
            this.txtRealName = (System.Web.UI.WebControls.TextBox)this.FindControl("txtRealName");
            this.txtIdentityCard = (System.Web.UI.WebControls.TextBox)this.FindControl("txtIdentityCard");
            this.btnSubmit = (System.Web.UI.WebControls.Button)this.FindControl("btnSubmit");
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
        }
        private void btnSubmit_Click(object sender, System.EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtRealName.Text.Trim()))
            {
                this.ShowMessage("请输入姓名", false);
                return;
            }
            if (string.IsNullOrEmpty(this.txtIdentityCard.Text.Trim()))
            {
                this.ShowMessage("请输入身份证号码", false);
                return;
            }
            string patternIdentityCard = "^[1-9]\\d{5}[1-9]\\d{3}((0[1-9])|(1[0-2]))((0[1-9])|([1-2][0-9])|(3[0-1]))\\d{3}(\\d|x|X)";
            System.Text.RegularExpressions.Regex regexIdentityCard = new System.Text.RegularExpressions.Regex(patternIdentityCard);
            if (!string.IsNullOrEmpty(this.txtIdentityCard.Text) && !regexIdentityCard.IsMatch(this.txtIdentityCard.Text.Trim()))
            {
                this.ShowMessage("请输入正确的身份证号码", false);
                return;
            }
            Member member = Users.GetUser(HiContext.Current.User.UserId, true) as Member;

            if(string.IsNullOrEmpty(member.IdentityCard))
            {
                member.IdentityCard = "";
            }

            if ((member.RealName != this.txtRealName.Text.Trim() || member.IdentityCard.ToUpper() != this.txtIdentityCard.Text.Trim().ToUpper()) && UserHelper.IsExistIdentityCard(txtIdentityCard.Text.Trim(), HiContext.Current.User.UserId) > 0)
            {
                this.ShowMessage("存在已认证的身份证号码，不能保存!", false);
                return;
            }

            if (!OrderHelper.Checkisverify(HiContext.Current.User.UserId) || member.RealName != this.txtRealName.Text.Trim() || member.IdentityCard.ToUpper() != this.txtIdentityCard.Text.Trim().ToUpper())
            {
                int MaxCount = string.IsNullOrEmpty(ConfigurationManager.AppSettings["IsVerifyCount"]) ? 3 : int.Parse(ConfigurationManager.AppSettings["IsVerifyCount"]);
                int fcount = MemberHelper.CheckErrorCount(HiContext.Current.User.UserId);
                //if (fcount >= MaxCount || (MaxCount - fcount - 1) == 0)//超过失败次数
                if (fcount >= MaxCount)//超过失败次数 20160315 修改之前
                {
                    this.ShowMessage("您今天已提交验证" + (MaxCount) + "次，不能再验证！", false);
                    return;
                }

                CertNoValidHelper cv = new CertNoValidHelper();
                string rest = cv.CertNoValid(this.txtRealName.Text.Trim(), this.txtIdentityCard.Text.Trim().ToUpper());
                MemberHelper.AddIsVerifyMsg(HiContext.Current.User.UserId);
                if (!rest.Equals("实名通过"))
                {
                    string message ="验证失败，您还有" + (MaxCount - fcount - 1) + "次验证机会,请确认身份信息是否有误！";
                    if ((MaxCount - fcount - 1) == 0)
                    {
                        message="您今天已提交验证" + (MaxCount) + "次，不能再验证！";
                    }
                    this.ShowMessage(message, false);
                    return;
                }
            }

            member.IsVerify = 1;
            member.VerifyDate = DateTime.Now;
            member.IdentityCard = this.txtIdentityCard.Text.Trim().ToUpper();
            member.RealName = Globals.HtmlEncode(this.txtRealName.Text.Trim());
            if (Users.UpdateUser(member))
            {
                string type = string.IsNullOrEmpty(this.Page.Request.QueryString["type"]) ? "" : this.Page.Request.QueryString["type"];
                if (type == "submit")
                {
                    this.Page.Response.Redirect("../SubmmitOrder.aspx?buyAmount=" + this.Page.Request.QueryString["buyAmount"] + "&productSku=" + this.Page.Request.QueryString["productSku"] + "&from=" + this.Page.Request.QueryString["from"] + "&storeId=");
                    return;
                }
                this.ShowMessage("认证成功", true);
                return;
            }
            this.ShowMessage("认证失败", false);
        }
    }
}
