using EcShop.Entities.Members;
using EcShop.Membership.Context;
using EcShop.SaleSystem.Member;
using EcShop.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;
namespace EcShop.UI.AccountCenter.CodeBehind
{
    public class UserDefault : MemberTemplatedWebControl
    {
        private System.Web.UI.WebControls.Literal litUserName;
        private System.Web.UI.WebControls.Literal litUserPoint;
        private System.Web.UI.WebControls.Literal litUserRank;
        private System.Web.UI.WebControls.Literal litNoPayOrderNum;
        private System.Web.UI.WebControls.Literal litPayOrderNum;
        private System.Web.UI.WebControls.Literal litFineshOrderNum;
        private System.Web.UI.WebControls.Literal litNoReplyLeaveWordNum;
        private System.Web.UI.WebControls.Literal litemmailverfice;
        private System.Web.UI.WebControls.Literal litcellphoneverfice;
        private System.Web.UI.WebControls.Literal litIdentityCardverfice;//实名认证

        private FormatedMoneyLabel litAccountAmount;
        private FormatedMoneyLabel litUseableBalance;
        private FormatedMoneyLabel litRequestBalance;
        private System.Web.UI.WebControls.HyperLink hpMes;
        protected override void OnInit(System.EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "User/Skin-UserDefault.html";
            }
            base.OnInit(e);
        }
        protected override void AttachChildControls()
        {
            this.litUserName = (System.Web.UI.WebControls.Literal)this.FindControl("litUserName");
            this.litUserPoint = (System.Web.UI.WebControls.Literal)this.FindControl("litUserPoint");
            this.litUserRank = (System.Web.UI.WebControls.Literal)this.FindControl("litUserRank");
            this.litemmailverfice = (System.Web.UI.WebControls.Literal)this.FindControl("litemmailverfice");
            this.litIdentityCardverfice = (System.Web.UI.WebControls.Literal)this.FindControl("litIdentityCardverfice");//实名认证
            this.litcellphoneverfice = (System.Web.UI.WebControls.Literal)this.FindControl("litcellphoneverfice");
            this.litNoPayOrderNum = (System.Web.UI.WebControls.Literal)this.FindControl("litNoPayOrderNum");
            this.litPayOrderNum = (System.Web.UI.WebControls.Literal)this.FindControl("litPayOrderNum");
            this.litFineshOrderNum = (System.Web.UI.WebControls.Literal)this.FindControl("litFineshOrderNum");
            this.litNoReplyLeaveWordNum = (System.Web.UI.WebControls.Literal)this.FindControl("litNoReplyLeaveWordNum");
            this.litAccountAmount = (FormatedMoneyLabel)this.FindControl("litAccountAmount");
            this.litRequestBalance = (FormatedMoneyLabel)this.FindControl("litRequestBalance");
            this.litUseableBalance = (FormatedMoneyLabel)this.FindControl("litUseableBalance");
            this.hpMes = (System.Web.UI.WebControls.HyperLink)this.FindControl("hpMes");
            PageTitle.AddSiteNameTitle("会员中心首页");
            if (!this.Page.IsPostBack)
            {
                Member member = Users.GetUser(HiContext.Current.User.UserId, false) as Member;
                if (member == null)
                {
                    this.Page.Response.Redirect("/Login.aspx");
                }
                this.litUserPoint.Text = member.Points.ToString();
                this.litUserName.Text = member.Username;
                this.litcellphoneverfice.Text = (member.CellPhoneVerification ? "手机验证完成" : "手机未验证<a href=\"UserCellPhoneVerification.aspx\">去验证</a>");
                this.litemmailverfice.Text = (member.EmailVerification ? "邮箱验证完成" : "邮箱未验证<a href=\"UserEmailVerification.aspx\">去验证</a>");
                if (this.litIdentityCardverfice != null)
                {
                    this.litIdentityCardverfice.Text = (member.IsVerify == 1 ? "已实名验证" : "未实名验证<a href=\"IdentityCardVericetion.aspx\">去验证</a>");
                }

                MemberGradeInfo memberGrade = MemberProcessor.GetMemberGrade(member.GradeId);
                if (memberGrade != null)
                {
                    this.litUserRank.Text = memberGrade.Name;
                }
                int num = 0;
                int num2 = 0;
                int num3 = 0;
                int num4 = 0;
                int num5 = 0;
                MemberProcessor.GetStatisticsNum(out num, out num3, out num4);
                this.litNoPayOrderNum.Text = string.Concat(new object[]
				{
					"<a href=\"UserOrders.aspx?orderStatus=",
					1,
					"\">",
					num.ToString(),
					"</a>"
				});
                this.litNoReplyLeaveWordNum.Text = num4.ToString();
                this.litPayOrderNum.Text = string.Concat(new object[]
				{
					"<a href=\"UserOrders.aspx?orderStatus=",
					3,
					"\">",
					num2,
					"</a>"
				});
                this.litFineshOrderNum.Text = num5.ToString();
                this.hpMes.Text = num3.ToString();
                this.litAccountAmount.Money = member.Balance;
                this.litRequestBalance.Money = member.RequestBalance;
                this.litUseableBalance.Money = member.Balance - member.RequestBalance;
                this.hpMes.NavigateUrl = "UserReceivedMessages.aspx";
            }
        }
    }
}
