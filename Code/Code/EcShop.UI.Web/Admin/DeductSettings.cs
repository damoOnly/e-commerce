using EcShop.ControlPanel.Store;
using EcShop.Membership.Context;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin
{
	[AdministerCheck(true)]
	public class DeductSettings : AdminPage
	{
		protected System.Web.UI.WebControls.TextBox txtReferralDeduct;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtReferralDeductTip;
		protected System.Web.UI.WebControls.TextBox txtSubMemberDeduct;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtSubMemberDeductTip;
		protected System.Web.UI.WebControls.TextBox txtSubReferralDeduct;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtSubReferralDeductTip;
		protected System.Web.UI.WebControls.Button btnOK;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			if (!this.Page.IsPostBack)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
				this.txtReferralDeduct.Text = masterSettings.ReferralDeduct.ToString("F2");
				this.txtSubMemberDeduct.Text = masterSettings.SubMemberDeduct.ToString("F2");
				this.txtSubReferralDeduct.Text = masterSettings.SubReferralDeduct.ToString("F2");
			}
		}
		private void btnOK_Click(object sender, System.EventArgs e)
		{
			decimal referralDeduct = 0m;
			decimal subMemberDeduct = 0m;
			decimal subReferralDeduct = 0m;
			if (!decimal.TryParse(this.txtReferralDeduct.Text.Trim(), out referralDeduct))
			{
				this.ShowMsg("您输入的直接推广佣金格式不对！", false);
				return;
			}
			if (!decimal.TryParse(this.txtSubMemberDeduct.Text, out subMemberDeduct))
			{
				this.ShowMsg("您输入的下级会员佣金格式不正确！", false);
				return;
			}
			if (!decimal.TryParse(this.txtSubReferralDeduct.Text, out subReferralDeduct))
			{
				this.ShowMsg("您输入的下级推广员佣金格式不正确！", false);
				return;
			}
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
			masterSettings.ReferralDeduct = referralDeduct;
			masterSettings.SubMemberDeduct = subMemberDeduct;
			masterSettings.SubReferralDeduct = subReferralDeduct;
			SettingsManager.Save(masterSettings);
			this.ShowMsg("成功修改了佣金比例", true);
		}
	}
}
