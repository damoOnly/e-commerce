using EcShop.Core;
using EcShop.Membership.Context;
using EcShop.UI.Common.Controls;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.UI.HtmlControls;
using ThoughtWorks.QRCode.Codec;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	public class WAPDevelopReferrals : WAPMemberTemplatedWebControl
	{
		private System.Web.UI.HtmlControls.HtmlImage imgQRCode;
		private System.Web.UI.HtmlControls.HtmlAnchor linDevelopReferrals;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-DevelopReferrals.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.linDevelopReferrals = (System.Web.UI.HtmlControls.HtmlAnchor)this.FindControl("linDevelopReferrals");
			this.imgQRCode = (System.Web.UI.HtmlControls.HtmlImage)this.FindControl("imgQRCode");
			Member member = HiContext.Current.User as Member;
			if (member.ReferralStatus != 2)
			{
				this.Page.Response.Redirect("ReferralRegisterAgreement.aspx");
			}
			System.Uri url = System.Web.HttpContext.Current.Request.Url;
			string text = (url.Port == 80) ? string.Empty : (":" + url.Port.ToString(System.Globalization.CultureInfo.InvariantCulture));
			this.linDevelopReferrals.HRef = string.Concat(new object[]
			{
				string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}://{1}{2}/WapShop/Login.aspx", new object[]
				{
					url.Scheme,
					HiContext.Current.SiteSettings.SiteUrl,
					text
				}),
				Globals.ApplicationPath,
				"?ReferralUserId=",
				HiContext.Current.User.UserId,
				"&action=register"
			});
			string text2 = this.Page.Request.MapPath(Globals.ApplicationPath + "/Storage/master/QRCode/");
			if (System.IO.File.Exists(string.Concat(new object[]
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
			if (this.linDevelopReferrals.HRef.IndexOf("/WapShop/Login.aspx") == -1)
			{
				this.imgQRCode.Src = this.CreateQRCode(this.linDevelopReferrals.HRef.Replace("/Login.aspx", "/WapShop/Login.aspx"), member.UserId);
				return;
			}
			this.imgQRCode.Src = this.CreateQRCode(this.linDevelopReferrals.HRef, member.UserId);
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
