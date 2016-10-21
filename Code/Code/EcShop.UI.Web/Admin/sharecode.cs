using EcShop.ControlPanel.Members;
using EcShop.Membership.Context;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin
{
    public class sharecode : Page
    {
        private System.Web.UI.WebControls.Literal litRecemmendCode;
        private System.Web.UI.WebControls.Literal litusername;
        protected void Page_Load(object sender, System.EventArgs e)
        {
            int vuserid;
            this.litRecemmendCode = (System.Web.UI.WebControls.Literal)this.FindControl("litRecemmendCode");
            this.litusername = (System.Web.UI.WebControls.Literal)this.FindControl("litusername");
            if (!this.IsPostBack)
            {
                //app分享
                if (this.Page.Request["UserId"] != null)
                {
                    string userid = this.Page.Request["UserId"].ToString();
                    string sessionId = this.ToSeesionId(userid);

                    Member member = Users.GetUserIdByUserSessionId(sessionId, false) as Member;

                    if(member!=null)
                    {

                        if(string.IsNullOrEmpty(member.RealName))
                        {
                            litusername.Text = "您的好友";
                        }

                        else
                        {
                            litusername.Text = "*" + member.RealName.Substring(1);
                        }

                        litRecemmendCode.Text = MemberHelper.GetRecemmendCode(member.UserId);


                    }

                }              
                else if(this.Page.Request["VUserId"] != null && int.TryParse(this.Page.Request["VUserId"].ToString(),out vuserid))
                {
                    Member member = Users.GetUser(vuserid,null, false, false) as Member;

                    if (member != null)
                    {

                        if (string.IsNullOrEmpty(member.RealName))
                        {
                            litusername.Text = "您的好友";
                        }

                        else
                        {
                            litusername.Text = "*" + member.RealName.Substring(1);
                        }

                        litRecemmendCode.Text = MemberHelper.GetRecemmendCode(member.UserId);


                    }
                }

                //微信分享
                else
                {
                    if (this.Page.Request["recemmendCode"] != null)
                    {
                        litRecemmendCode.Text = this.Page.Request["recemmendCode"];
                    }
                    //if (this.Page.Request["username"] != null)
                    //{
                    //    litusername.Text = this.Page.Request["username"];
                    //}

                    litusername.Text = "您的好友";
                }
            }
        }

        private  string ToSeesionId(string source)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                return Guid.Empty.ToString();
            }

            string str = "";

            try
            {
                str = new Guid(source).ToString().ToLower();
            }

            catch (Exception ex)
            {
               
                str = Guid.Empty.ToString();
            }

            return str;
        }
    }
}