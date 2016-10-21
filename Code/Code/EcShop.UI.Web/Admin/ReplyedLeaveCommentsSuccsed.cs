using EcShop.ControlPanel.Comments;
using EcShop.Core;
using EcShop.Entities.Comments;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin
{
    public class ReplyedLeaveCommentsSuccsed : AdminPage
    {
        private long leaveId;
        protected System.Web.UI.HtmlControls.HtmlAnchor a_user;
        protected System.Web.UI.WebControls.Label lblUserName;
        protected FormatedTimeLabel litLeaveDate;
        protected System.Web.UI.WebControls.Literal litTitle;
        protected System.Web.UI.WebControls.Literal litPublishContent;
        protected System.Web.UI.WebControls.DataList dtlistLeaveCommentsReply;
        protected System.Web.UI.WebControls.HyperLink hlReply;
        protected System.Web.UI.WebControls.Literal litType;
        protected System.Web.UI.WebControls.Literal litContact;
        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!long.TryParse(this.Page.Request.QueryString["leaveId"], out this.leaveId))
            {
                base.GotoResourceNotFound();
                return;
            }
            this.dtlistLeaveCommentsReply.DeleteCommand += new System.Web.UI.WebControls.DataListCommandEventHandler(this.dtlistLeaveCommentsReply_DeleteCommand);
            if (!base.IsPostBack)
            {
                this.SetControl(this.leaveId);
                this.hlReply.NavigateUrl = Globals.ApplicationPath + "/admin/comment/ReplyLeaveComments.aspx?LeaveId=" + this.leaveId;
                this.BindList();
            }
        }
        private void SetControl(long leaveId)
        {
            LeaveCommentInfo leaveComment = NoticeHelper.GetLeaveComment(leaveId);
            Globals.EntityCoding(leaveComment, false);
            this.litTitle.Text = leaveComment.Title;
            this.lblUserName.Text = leaveComment.UserName;
            this.litLeaveDate.Time = leaveComment.PublishDate;
            this.litPublishContent.Text = leaveComment.PublishContent;
            this.litContact.Text = leaveComment.ContactWay;
            this.litType.Text = ConverFeedBackType(leaveComment.FeedbackType);


            this.a_user.HRef = string.Concat(new object[]
			{
				"javascript:DialogFrame('",
				Globals.ApplicationPath,
				"/admin/member/MemberDetails.aspx?userId=",
				leaveComment.UserId,
				"','查看会员详细信息',null,null)"
			});
        }
        private void dtlistLeaveCommentsReply_DeleteCommand(object sender, System.Web.UI.WebControls.DataListCommandEventArgs e)
        {
            long leaveReplyId = System.Convert.ToInt64(this.dtlistLeaveCommentsReply.DataKeys[e.Item.ItemIndex]);
            if (NoticeHelper.DeleteLeaveCommentReply(leaveReplyId))
            {
                this.ShowMsg("删除成功", true);
                this.BindList();
                return;
            }
            this.ShowMsg("删除失败，请重试", false);
        }
        private void BindList()
        {
            this.dtlistLeaveCommentsReply.DataSource = NoticeHelper.GetReplyLeaveComments(this.leaveId);
            this.dtlistLeaveCommentsReply.DataBind();
        }

        public string ConverFeedBackType(int? type)
        {
            int? feebacktype = type;

            string str = "";

            if (feebacktype.HasValue)
            {
                switch (feebacktype)
                {
                    case 1: str += "配送服务"; break;
                    case 2: str += "意见与反馈"; break;
                    case 3: str += "软件报错"; break;
                    case 4: str += "其他"; break;
                }
            }
            return str;
        }
    }
}
