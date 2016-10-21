using EcShop.Core;
using EcShop.Membership.Context;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using ThoughtWorks.QRCode.Codec;
namespace EcShop.UI.AccountCenter.CodeBehind
{
	public class PopularizeGift : MemberTemplatedWebControl
	{
		private System.Web.UI.WebControls.TextBox litUserLink;
		private System.Web.UI.HtmlControls.HtmlImage imgQRCode;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-PopularizeGift.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.litUserLink = (System.Web.UI.WebControls.TextBox)this.FindControl("litUserLink");
			this.imgQRCode = (System.Web.UI.HtmlControls.HtmlImage)this.FindControl("imgQRCode");
			Member member = HiContext.Current.User as Member;
			if (member.ReferralStatus != 2)
			{
				this.Page.Response.Redirect("/User/ReferralRegisterAgreement.aspx");
			}
			Uri url = System.Web.HttpContext.Current.Request.Url;
			string text = (url.Port == 80) ? string.Empty : (":" + url.Port.ToString(System.Globalization.CultureInfo.InvariantCulture));
			this.litUserLink.Text = string.Concat(new object[]
			{
				string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}://{1}{2}/Register.aspx", new object[]
				{
					url.Scheme,
					HiContext.Current.SiteSettings.SiteUrl,
					text
				}),
				Globals.ApplicationPath,
				"?ReferralUserId=",
				HiContext.Current.User.UserId
			});
			string text2 = this.Page.Request.MapPath(Globals.ApplicationPath + "/Storage/master/QRCode/");
			if (System.IO.Directory.Exists(string.Concat(new object[]
			{
				text2,
				"referral_",
				member.UserId,
				".png"
			})))
			{
				this.imgQRCode.Src = string.Concat(new object[]
				{
					Globals.ApplicationPath,
					"/Storage/master/QRCode/referral_",
					member.UserId,
					".png"
				});
				return;
			}
			//this.imgQRCode.Src = this.CreateQRCode(this.litUserLink.Text.Replace("/Register.aspx", "/WapShop/Login.aspx") + "&action=register", member.UserId);

            this.imgQRCode.Src = this.CreateQRCode(this.litUserLink.Text, member.UserId);
		}
		private string CreateQRCode(string url, int userId)
		{
			string result = "";
			System.Drawing.Bitmap bitmap = new QRCodeEncoder
			{
				QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE,
				QRCodeScale = 10,
				QRCodeVersion = 0,
				QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M
			}.Encode(url);
			string text = this.Page.Request.MapPath(Globals.ApplicationPath + "/Storage/master/QRCode");
			if (System.IO.Directory.Exists(text))
			{
				string text2 = "referral_" + userId + ".png";
				bitmap.Save(text + "/" + text2, System.Drawing.Imaging.ImageFormat.Png);
				result = "/Storage/master/QRCode/" + text2;
			}
			else
			{
				System.IO.Directory.CreateDirectory(text);
			}
			return result;
		}
	}
}
