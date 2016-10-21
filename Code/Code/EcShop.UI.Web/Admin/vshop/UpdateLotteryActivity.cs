using ASPNET.WebControls;
using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Entities.Store;
using EcShop.Entities.VShop;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin.vshop
{
	[PrivilegeCheck(Privilege.EditLotteryActivity)]
	public class UpdateLotteryActivity : AdminPage
	{
		private int type;
		private int activityid;
		protected System.Web.UI.WebControls.Literal LitTitle;
		protected System.Web.UI.WebControls.Literal Litdesc;
		protected System.Web.UI.WebControls.TextBox txtActiveName;
		protected WebCalendar calendarStartDate;
		protected WebCalendar calendarEndDate;
		protected System.Web.UI.WebControls.TextBox txtKeyword;
		protected System.Web.UI.WebControls.TextBox txtMaxNum;
		protected System.Web.UI.WebControls.TextBox txtdesc;
		protected System.Web.UI.WebControls.TextBox txtPrize1;
		protected System.Web.UI.WebControls.TextBox txtPrize1Num;
		protected System.Web.UI.WebControls.TextBox txtProbability1;
		protected System.Web.UI.WebControls.TextBox txtPrize2;
		protected System.Web.UI.WebControls.TextBox txtPrize2Num;
		protected System.Web.UI.WebControls.TextBox txtProbability2;
		protected System.Web.UI.WebControls.TextBox txtPrize3;
		protected System.Web.UI.WebControls.TextBox txtPrize3Num;
		protected System.Web.UI.WebControls.TextBox txtProbability3;
		protected System.Web.UI.WebControls.CheckBox ChkOpen;
		protected System.Web.UI.WebControls.TextBox txtPrize4;
		protected System.Web.UI.WebControls.TextBox txtPrize4Num;
		protected System.Web.UI.WebControls.TextBox txtProbability4;
		protected System.Web.UI.WebControls.TextBox txtPrize5;
		protected System.Web.UI.WebControls.TextBox txtPrize5Num;
		protected System.Web.UI.WebControls.TextBox txtProbability5;
		protected System.Web.UI.WebControls.TextBox txtPrize6;
		protected System.Web.UI.WebControls.TextBox txtPrize6Num;
		protected System.Web.UI.WebControls.TextBox txtProbability6;
		protected System.Web.UI.WebControls.FileUpload fileUpload;
		protected HiImage imgPic;
		protected ImageLinkButton btnPicDelete;
		protected System.Web.UI.WebControls.Button btnUpdateActivity;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (int.TryParse(base.Request.QueryString["typeid"], out this.type) && int.TryParse(base.Request.QueryString["id"], out this.activityid))
			{
				switch (this.type)
				{
				case 1:
					this.LitTitle.Text = "大转盘编辑";
					this.Litdesc.Text = "大转盘编辑";
					break;
				case 2:
					this.LitTitle.Text = "刮刮卡编辑";
					this.Litdesc.Text = "刮刮卡编辑";
					break;
				case 3:
					this.LitTitle.Text = "砸金蛋编辑";
					this.Litdesc.Text = "砸金蛋编辑";
					break;
				}
				if (!this.Page.IsPostBack)
				{
					this.RestoreLottery();
					return;
				}
			}
			else
			{
				this.ShowMsg("参数错误", false);
			}
		}
		public void RestoreLottery()
		{
			LotteryActivityInfo lotteryActivityInfo = VShopHelper.GetLotteryActivityInfo(this.activityid);
			this.txtActiveName.Text = lotteryActivityInfo.ActivityName;
			this.txtKeyword.Text = lotteryActivityInfo.ActivityKey;
			this.txtdesc.Text = lotteryActivityInfo.ActivityDesc;
			this.imgPic.ImageUrl = lotteryActivityInfo.ActivityPic;
			this.btnPicDelete.Visible = !string.IsNullOrEmpty(this.imgPic.ImageUrl);
			this.calendarStartDate.SelectedDate = new System.DateTime?(lotteryActivityInfo.StartTime);
			this.calendarEndDate.SelectedDate = new System.DateTime?(lotteryActivityInfo.EndTime);
			this.txtMaxNum.Text = lotteryActivityInfo.MaxNum.ToString();
			this.txtPrize1.Text = lotteryActivityInfo.PrizeSettingList[0].PrizeName;
			this.txtPrize1Num.Text = lotteryActivityInfo.PrizeSettingList[0].PrizeNum.ToString();
			this.txtProbability1.Text = lotteryActivityInfo.PrizeSettingList[0].Probability.ToString();
			this.txtPrize2.Text = lotteryActivityInfo.PrizeSettingList[1].PrizeName;
			this.txtPrize2Num.Text = lotteryActivityInfo.PrizeSettingList[1].PrizeNum.ToString();
			this.txtProbability2.Text = lotteryActivityInfo.PrizeSettingList[1].Probability.ToString();
			this.txtPrize3.Text = lotteryActivityInfo.PrizeSettingList[2].PrizeName;
			this.txtPrize3Num.Text = lotteryActivityInfo.PrizeSettingList[2].PrizeNum.ToString();
			this.txtProbability3.Text = lotteryActivityInfo.PrizeSettingList[2].Probability.ToString();
			if (lotteryActivityInfo.PrizeSettingList.Count > 3)
			{
				this.ChkOpen.Checked = true;
				this.txtPrize4.Text = lotteryActivityInfo.PrizeSettingList[3].PrizeName;
				this.txtPrize4Num.Text = lotteryActivityInfo.PrizeSettingList[3].PrizeNum.ToString();
				this.txtProbability4.Text = lotteryActivityInfo.PrizeSettingList[3].Probability.ToString();
				this.txtPrize5.Text = lotteryActivityInfo.PrizeSettingList[4].PrizeName;
				this.txtPrize5Num.Text = lotteryActivityInfo.PrizeSettingList[4].PrizeNum.ToString();
				this.txtProbability5.Text = lotteryActivityInfo.PrizeSettingList[4].Probability.ToString();
				this.txtPrize6.Text = lotteryActivityInfo.PrizeSettingList[5].PrizeName;
				this.txtPrize6Num.Text = lotteryActivityInfo.PrizeSettingList[5].PrizeNum.ToString();
				this.txtProbability6.Text = lotteryActivityInfo.PrizeSettingList[5].Probability.ToString();
			}
		}
		protected void btnUpdateActivity_Click(object sender, System.EventArgs e)
		{
			if (VShopHelper.GetLotteryActivityInfo(this.activityid).ActivityKey != this.txtKeyword.Text.Trim() && ReplyHelper.HasReplyKey(this.txtKeyword.Text.Trim()))
			{
				this.ShowMsg("关键字重复!", false);
				return;
			}
			string text = string.Empty;
			if (this.fileUpload.HasFile)
			{
				try
				{
					text = VShopHelper.UploadTopicImage(this.fileUpload.PostedFile);
					goto IL_AB;
				}
				catch
				{
					this.ShowMsg("图片上传失败，您选择的不是图片类型的文件，或者网站的虚拟目录没有写入文件的权限", false);
					return;
				}
			}
			if (!string.IsNullOrEmpty(this.imgPic.ImageUrl))
			{
				text = this.imgPic.ImageUrl;
				goto IL_AB;
			}
			this.ShowMsg("您没有选择上传的图片文件！", false);
			return;
			IL_AB:
			if (!this.calendarStartDate.SelectedDate.HasValue)
			{
				this.ShowMsg("请选择活动开始时间", false);
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
			int num;
			if (!int.TryParse(this.txtMaxNum.Text, out num) || num.ToString() != this.txtMaxNum.Text)
			{
				this.ShowMsg("可抽奖次数必须是整数", false);
				return;
			}
			LotteryActivityInfo lotteryActivityInfo = new LotteryActivityInfo();
			lotteryActivityInfo.ActivityName = this.txtActiveName.Text;
			lotteryActivityInfo.ActivityId = this.activityid;
			lotteryActivityInfo.ActivityType = this.type;
			lotteryActivityInfo.ActivityKey = this.txtKeyword.Text;
			lotteryActivityInfo.ActivityDesc = this.txtdesc.Text;
			lotteryActivityInfo.ActivityPic = text;
			lotteryActivityInfo.StartTime = this.calendarStartDate.SelectedDate.Value;
			lotteryActivityInfo.EndTime = this.calendarEndDate.SelectedDate.Value;
			lotteryActivityInfo.MaxNum = System.Convert.ToInt32(this.txtMaxNum.Text);
			System.Collections.Generic.List<PrizeSetting> list = new System.Collections.Generic.List<PrizeSetting>();
			int num2;
			if (int.TryParse(this.txtPrize1Num.Text, out num2) && int.TryParse(this.txtPrize2Num.Text, out num2) && int.TryParse(this.txtPrize3Num.Text, out num2))
			{
				decimal probability = System.Convert.ToDecimal(this.txtProbability1.Text);
				decimal probability2 = System.Convert.ToDecimal(this.txtProbability2.Text);
				decimal probability3 = System.Convert.ToDecimal(this.txtProbability3.Text);
				list.AddRange(new System.Collections.Generic.List<PrizeSetting>
				{
					new PrizeSetting
					{
						PrizeName = this.txtPrize1.Text,
						PrizeNum = System.Convert.ToInt32(this.txtPrize1Num.Text),
						PrizeLevel = "一等奖",
						Probability = probability
					},
					new PrizeSetting
					{
						PrizeName = this.txtPrize2.Text,
						PrizeNum = System.Convert.ToInt32(this.txtPrize2Num.Text),
						PrizeLevel = "二等奖",
						Probability = probability2
					},
					new PrizeSetting
					{
						PrizeName = this.txtPrize3.Text,
						PrizeNum = System.Convert.ToInt32(this.txtPrize3Num.Text),
						PrizeLevel = "三等奖",
						Probability = probability3
					}
				});
				if (this.ChkOpen.Checked)
				{
					if (string.IsNullOrEmpty(this.txtPrize4.Text) || string.IsNullOrEmpty(this.txtPrize5.Text) || string.IsNullOrEmpty(this.txtPrize6.Text))
					{
						this.ShowMsg("开启四五六名必须填写", false);
						return;
					}
					if (!int.TryParse(this.txtPrize4Num.Text, out num2) || !int.TryParse(this.txtPrize5Num.Text, out num2) || !int.TryParse(this.txtPrize6Num.Text, out num2))
					{
						this.ShowMsg("奖品数量必须为数字！", false);
						return;
					}
					decimal probability4 = System.Convert.ToDecimal(this.txtProbability4.Text);
					decimal probability5 = System.Convert.ToDecimal(this.txtProbability5.Text);
					decimal probability6 = System.Convert.ToDecimal(this.txtProbability6.Text);
					list.AddRange(new System.Collections.Generic.List<PrizeSetting>
					{
						new PrizeSetting
						{
							PrizeName = this.txtPrize4.Text,
							PrizeNum = System.Convert.ToInt32(this.txtPrize4Num.Text),
							PrizeLevel = "四等奖",
							Probability = probability4
						},
						new PrizeSetting
						{
							PrizeName = this.txtPrize5.Text,
							PrizeNum = System.Convert.ToInt32(this.txtPrize5Num.Text),
							PrizeLevel = "五等奖",
							Probability = probability5
						},
						new PrizeSetting
						{
							PrizeName = this.txtPrize6.Text,
							PrizeNum = System.Convert.ToInt32(this.txtPrize6Num.Text),
							PrizeLevel = "六等奖",
							Probability = probability6
						}
					});
				}
				lotteryActivityInfo.PrizeSettingList = list;
				if (VShopHelper.UpdateLotteryActivity(lotteryActivityInfo))
				{
					this.imgPic.ImageUrl = text;
					base.Response.Redirect("ManageLotteryActivity.aspx?type=" + this.type, true);
				}
				return;
			}
			this.ShowMsg("奖品数量必须为数字！", false);
		}
		protected void btnPicDelete_Click(object sender, System.EventArgs e)
		{
			try
			{
				ResourcesHelper.DeleteImage(this.imgPic.ImageUrl);
				this.imgPic.ImageUrl = "";
				this.btnPicDelete.Visible = !string.IsNullOrEmpty(this.imgPic.ImageUrl);
				this.imgPic.Visible = !string.IsNullOrEmpty(this.imgPic.ImageUrl);
			}
			catch
			{
			}
		}
	}
}
