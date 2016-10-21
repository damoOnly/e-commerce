using EcShop.Core;
using EcShop.Membership.Context;
using EcShop.UI.Common.Controls;
using System;
using System.Web;
using System.Web.UI;
namespace EcShop.UI.SaleSystem.CodeBehind
{
    [System.Web.UI.ParseChildren(true)]
    public class VBindAccount : VshopTemplatedWebControl
    {
        private string openId;
        protected override void OnInit(System.EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "skin-VBindAccount.html";
            }
            base.OnInit(e);
        }
        protected override void AttachChildControls()
        {
            /*this.openId = this.Page.Request.QueryString["sessionId"];
            Member member = null;
            if (!string.IsNullOrEmpty(this.openId))
            {
                member = (Users.GetUserByOpenId(this.openId) as Member);
            }
            Member member2 = HiContext.Current.User as Member;
            if (member2 != null)
            {
                if (string.IsNullOrEmpty(member2.OpenId) && member == null)
                {
                    member2.OpenId = this.openId;
                    Users.UpdateUser(member2);
                }
                member = member2;
            }
            if (member != null)
            {
                System.Web.HttpCookie httpCookie = new System.Web.HttpCookie("Vshop-Member");
                httpCookie.Value = Globals.UrlEncode(member.Username);
                httpCookie.Expires = System.DateTime.Now.AddYears(1);
                System.Web.HttpContext.Current.Response.Cookies.Add(httpCookie);
                this.Page.Response.Redirect("/Vshop/MemberCenter.aspx");
            }*/
            PageTitle.AddSiteNameTitle("绑定PC端账号");
        }
    }
}
