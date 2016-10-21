using ASPNET.WebControls;
using EcShop.ControlPanel.Members;
using EcShop.ControlPanel.Store;
using EcShop.Entities.Store;
using EcShop.Entities.VShop;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.AddLotteryTicket)]
	public class AddLotteryTicket : AdminPage
	{
		protected System.Web.UI.WebControls.TextBox txtActiveName;
		protected WebCalendar calendarStartDate;
		protected WebCalendar calendarOpenDate;
		protected System.Web.UI.WebControls.DropDownList ddlHours;
		protected WebCalendar calendarEndDate;
		protected System.Web.UI.WebControls.CheckBoxList cbList;
		protected System.Web.UI.WebControls.TextBox txtMinValue;
		protected System.Web.UI.WebControls.TextBox txtCode;
		protected System.Web.UI.WebControls.TextBox txtKeyword;
		protected System.Web.UI.WebControls.TextBox txtdesc;
		protected System.Web.UI.WebControls.TextBox txtPrize1;
		protected System.Web.UI.WebControls.TextBox txtPrize1Num;
		protected System.Web.UI.WebControls.TextBox txtPrize2;
		protected System.Web.UI.WebControls.TextBox txtPrize2Num;
		protected System.Web.UI.WebControls.TextBox txtPrize3;
		protected System.Web.UI.WebControls.TextBox txtPrize3Num;
		protected System.Web.UI.WebControls.CheckBox ChkOpen;
		protected System.Web.UI.WebControls.TextBox txtPrize4;
		protected System.Web.UI.WebControls.TextBox txtPrize4Num;
		protected System.Web.UI.WebControls.TextBox txtPrize5;
		protected System.Web.UI.WebControls.TextBox txtPrize5Num;
		protected System.Web.UI.WebControls.TextBox txtPrize6;
		protected System.Web.UI.WebControls.TextBox txtPrize6Num;
		protected System.Web.UI.WebControls.FileUpload fileUpload;
		protected System.Web.UI.WebControls.Button btnAddActivity;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!base.IsPostBack)
			{
				this.cbList.DataSource = MemberHelper.GetMemberGrades();
				this.cbList.DataTextField = "Name";
				this.cbList.DataValueField = "GradeId";
				this.cbList.DataBind();
			}
		}
		protected void btnAddActivity_Click(object sender, System.EventArgs e)
		{
			if (ReplyHelper.HasReplyKey(this.txtKeyword.Text.Trim()))
			{
				this.ShowMsg("关键字重复!", false);
				return;
			}
			if (!this.calendarStartDate.SelectedDate.HasValue)
			{
				this.ShowMsg("请选择活动开始时间", false);
				return;
			}
			if (!this.calendarOpenDate.SelectedDate.HasValue)
			{
				this.ShowMsg("请选择抽奖开始时间", false);
				return;
			}
			if (!this.calendarEndDate.SelectedDate.HasValue)
			{
				this.ShowMsg("请选择活动结束时间", false);
				return;
			}
			if (this.calendarEndDate.SelectedDate.Value < this.calendarStartDate.SelectedDate.Value)
			{
				this.ShowMsg("活动开始时间不能晚于活动结束时间", false);
				return;
			}
			string activityPic = string.Empty;
			if (this.fileUpload.HasFile)
			{
				try
				{
					activityPic = VShopHelper.UploadTopicImage(this.fileUpload.PostedFile);
				}
				catch
				{
					this.ShowMsg("图片上传失败，您选择的不是图片类型的文件，或者网站的虚拟目录没有写入文件的权限", false);
					return;
				}
			}
			string text = string.Empty;
			for (int i = 0; i < this.cbList.Items.Count; i++)
			{
				if (this.cbList.Items[i].Selected)
				{
					text = text + "," + this.cbList.Items[i].Value;
				}
			}
			if (!string.IsNullOrEmpty(text))
			{
				LotteryTicketInfo lotteryTicketInfo = new LotteryTicketInfo();
				lotteryTicketInfo.GradeIds = text;
				lotteryTicketInfo.MinValue = System.Convert.ToInt32(this.txtMinValue.Text);
				lotteryTicketInfo.InvitationCode = this.txtCode.Text.Trim();
				lotteryTicketInfo.ActivityName = this.txtActiveName.Text;
				lotteryTicketInfo.ActivityKey = this.txtKeyword.Text;
				lotteryTicketInfo.ActivityDesc = this.txtdesc.Text;
				lotteryTicketInfo.ActivityPic = activityPic;
				lotteryTicketInfo.ActivityType = 4;
				lotteryTicketInfo.StartTime = this.calendarStartDate.SelectedDate.Value;
				lotteryTicketInfo.OpenTime = this.calendarOpenDate.SelectedDate.Value.AddHours((double)this.ddlHours.SelectedIndex);
				lotteryTicketInfo.EndTime = this.calendarEndDate.SelectedDate.Value;
				lotteryTicketInfo.PrizeSettingList = new System.Collections.Generic.List<PrizeSetting>();
				try
				{
					lotteryTicketInfo.PrizeSettingList.Add(new PrizeSetting
					{
						PrizeName = this.txtPrize1.Text,
						PrizeNum = System.Convert.ToInt32(this.txtPrize1Num.Text),
						PrizeLevel = "一等奖"
					});
					lotteryTicketInfo.PrizeSettingList.Add(new PrizeSetting
					{
						PrizeName = this.txtPrize2.Text,
						PrizeNum = System.Convert.ToInt32(this.txtPrize2Num.Text),
						PrizeLevel = "二等奖"
					});
					lotteryTicketInfo.PrizeSettingList.Add(new PrizeSetting
					{
						PrizeName = this.txtPrize3.Text,
						PrizeNum = System.Convert.ToInt32(this.txtPrize3Num.Text),
						PrizeLevel = "三等奖"
					});
				}
				catch (System.FormatException)
				{
					this.ShowMsg("奖品数量格式错误", false);
					return;
				}
				if (this.ChkOpen.Checked)
				{
					try
					{
						lotteryTicketInfo.PrizeSettingList.Add(new PrizeSetting
						{
							PrizeName = this.txtPrize4.Text,
							PrizeNum = System.Convert.ToInt32(this.txtPrize4Num.Text),
							PrizeLevel = "四等奖"
						});
						lotteryTicketInfo.PrizeSettingList.Add(new PrizeSetting
						{
							PrizeName = this.txtPrize5.Text,
							PrizeNum = System.Convert.ToInt32(this.txtPrize5Num.Text),
							PrizeLevel = "五等奖"
						});
						lotteryTicketInfo.PrizeSettingList.Add(new PrizeSetting
						{
							PrizeName = this.txtPrize6.Text,
							PrizeNum = System.Convert.ToInt32(this.txtPrize6Num.Text),
							PrizeLevel = "六等奖"
						});
					}
					catch (System.FormatException)
					{
						this.ShowMsg("奖品数量格式错误", false);
						return;
					}
				}
				int num = VShopHelper.SaveLotteryTicket(lotteryTicketInfo);
				if (num > 0)
				{
					ReplyHelper.SaveReply(new TextReplyInfo
					{
						Keys = lotteryTicketInfo.ActivityKey,
						MatchType = MatchType.Equal,
						MessageType = MessageType.Text,
						ReplyType = ReplyType.Ticket,
						ActivityId = num
					});
					base.Response.Redirect("ManageLotteryTicket.aspx");
				}
				return;
			}
			this.ShowMsg("请选择活动会员等级", false);
		}
	}
}
