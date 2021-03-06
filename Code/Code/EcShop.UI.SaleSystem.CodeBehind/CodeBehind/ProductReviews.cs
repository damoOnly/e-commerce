using EcShop.Core;
using EcShop.Entities.Comments;
using EcShop.Entities.Commodities;
using EcShop.Membership.Context;
using EcShop.Membership.Core;
using EcShop.Membership.Core.Enums;
using EcShop.SaleSystem.Catalog;
using EcShop.SaleSystem.Member;
using EcShop.UI.Common.Controls;
using Ecdev.Components.Validation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	public class ProductReviews : HtmlTemplatedWebControl
	{
		private int productId;
		private System.Web.UI.WebControls.TextBox txtUserName;
		private System.Web.UI.WebControls.TextBox txtEmail;
		private System.Web.UI.WebControls.TextBox txtContent;
		private IButton btnRefer;
		private System.Web.UI.HtmlControls.HtmlControl spReviewUserName;
		private System.Web.UI.HtmlControls.HtmlControl spReviewPsw;
		private System.Web.UI.HtmlControls.HtmlControl spReviewReg;
		private System.Web.UI.HtmlControls.HtmlInputText txtReviewUserName;
		private System.Web.UI.HtmlControls.HtmlInputText txtReviewPsw;
		private System.Web.UI.HtmlControls.HtmlInputText txtReviewCode;
		private ProductDetailsLink productdetailLink;
		private string verifyCodeKey = "VerifyCode";
		public ProductReviews()
		{
			if (HiContext.Current.User.UserRole != UserRole.Member)
			{
				this.Page.Response.Redirect(Globals.GetSiteUrls().UrlData.FormatUrl("login", new object[]
				{
					this.Page.Request.RawUrl
				}), true);
			}
		}
		private bool CheckVerifyCode(string verifyCode)
		{
			return System.Web.HttpContext.Current.Request.Cookies[this.verifyCodeKey] != null && string.Compare(HiCryptographer.Decrypt(System.Web.HttpContext.Current.Request.Cookies[this.verifyCodeKey].Value), verifyCode, true, System.Globalization.CultureInfo.InvariantCulture) == 0;
		}
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-ProductReviews.html";
			}
			bool flag = !string.IsNullOrEmpty(System.Web.HttpContext.Current.Request["isCallback"]) && System.Web.HttpContext.Current.Request["isCallback"] == "true";
			if (flag)
			{
				string verifyCode = System.Web.HttpContext.Current.Request["code"];
				string arg;
				if (!this.CheckVerifyCode(verifyCode))
				{
					arg = "0";
				}
				else
				{
					arg = "1";
				}
				System.Web.HttpContext.Current.Response.Clear();
				System.Web.HttpContext.Current.Response.ContentType = "application/json";
				System.Web.HttpContext.Current.Response.Write("{ ");
				System.Web.HttpContext.Current.Response.Write(string.Format("\"flag\":\"{0}\"", arg));
				System.Web.HttpContext.Current.Response.Write("}");
				System.Web.HttpContext.Current.Response.End();
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			if (!int.TryParse(this.Page.Request.QueryString["productId"], out this.productId))
			{
				base.GotoResourceNotFound();
			}
			this.txtEmail = (System.Web.UI.WebControls.TextBox)this.FindControl("txtEmail");
			this.txtUserName = (System.Web.UI.WebControls.TextBox)this.FindControl("txtUserName");
			this.txtContent = (System.Web.UI.WebControls.TextBox)this.FindControl("txtContent");
			this.btnRefer = ButtonManager.Create(this.FindControl("btnRefer"));
			this.spReviewUserName = (System.Web.UI.HtmlControls.HtmlControl)this.FindControl("spReviewUserName");
			this.spReviewPsw = (System.Web.UI.HtmlControls.HtmlControl)this.FindControl("spReviewPsw");
			this.spReviewReg = (System.Web.UI.HtmlControls.HtmlControl)this.FindControl("spReviewReg");
			this.txtReviewUserName = (System.Web.UI.HtmlControls.HtmlInputText)this.FindControl("txtReviewUserName");
			this.txtReviewPsw = (System.Web.UI.HtmlControls.HtmlInputText)this.FindControl("txtReviewPsw");
			this.txtReviewCode = (System.Web.UI.HtmlControls.HtmlInputText)this.FindControl("txtReviewCode");
			this.productdetailLink = (ProductDetailsLink)this.FindControl("ProductDetailsLink1");
			this.btnRefer.Click += new System.EventHandler(this.btnRefer_Click);
			if (!this.Page.IsPostBack)
			{
				PageTitle.AddSiteNameTitle("商品评论");
				if (HiContext.Current.User.UserRole == UserRole.Member)
				{
					this.txtUserName.Text = HiContext.Current.User.Username;
					this.txtEmail.Text = HiContext.Current.User.Email;
					this.txtReviewUserName.Value = string.Empty;
					this.txtReviewPsw.Value = string.Empty;
					this.spReviewUserName.Visible = false;
					this.spReviewPsw.Visible = false;
					this.spReviewReg.Visible = false;
					this.btnRefer.Text = "评论";
				}
				else
				{
					this.spReviewUserName.Visible = true;
					this.spReviewPsw.Visible = true;
					this.spReviewReg.Visible = true;
					this.btnRefer.Text = "登录并评论";
				}
				this.txtReviewCode.Value = string.Empty;
				ProductInfo productSimpleInfo = ProductBrowser.GetProductSimpleInfo(this.productId);
				if (productSimpleInfo != null)
				{
					this.productdetailLink.ProductId = this.productId;
					this.productdetailLink.ProductName = productSimpleInfo.ProductName;
				}
			}
		}
		public void btnRefer_Click(object sender, System.EventArgs e)
		{
			if (!this.ValidateConvert())
			{
				return;
			}
			ProductReviewInfo productReviewInfo = new ProductReviewInfo();
			productReviewInfo.ReviewDate = System.DateTime.Now;
			productReviewInfo.ProductId = this.productId;
			productReviewInfo.UserId = HiContext.Current.User.UserId;
			productReviewInfo.UserName = this.txtUserName.Text;
			productReviewInfo.UserEmail = this.txtEmail.Text;
			productReviewInfo.ReviewText = this.txtContent.Text;
			ValidationResults validationResults = Validation.Validate<ProductReviewInfo>(productReviewInfo, new string[]
			{
				"Refer"
			});
			string text = string.Empty;
			if (!validationResults.IsValid)
			{
				foreach (ValidationResult current in (System.Collections.Generic.IEnumerable<ValidationResult>)validationResults)
				{
					text += Formatter.FormatErrorMessage(current.Message);
				}
				this.ShowMessage(text, false);
				return;
			}
			if (HiContext.Current.User.UserRole != UserRole.Member && !this.userRegion(this.txtReviewUserName.Value, this.txtReviewPsw.Value))
			{
				return;
			}
			if (string.IsNullOrEmpty(this.txtReviewCode.Value))
			{
				this.ShowMessage("请输入验证码", false);
				return;
			}
			if (!HiContext.Current.CheckVerifyCode(this.txtReviewCode.Value.Trim()))
			{
				this.ShowMessage("验证码不正确", false);
				return;
			}
			int num = 0;
			int num2 = 0;
			ProductBrowser.LoadProductReview(this.productId, out num, out num2, "");
			if (num == 0)
			{
				this.ShowMessage("您没有购买此商品，因此不能进行评论", false);
				return;
			}
			if (num2 >= num)
			{
				this.ShowMessage("您已经对此商品进行了评论，请再次购买后方能再进行评论", false);
				return;
			}
			if (ProductBrowser.InsertProductReview(productReviewInfo))
			{
				this.Page.ClientScript.RegisterClientScriptBlock(base.GetType(), "success", string.Format("<script>alert(\"{0}\");window.location.href=\"{1}\"</script>", "评论成功", Globals.GetSiteUrls().UrlData.FormatUrl("productReviews", new object[]
				{
					this.productId
				})));
				return;
			}
			this.ShowMessage("评论失败，请重试", false);
		}
		private bool userRegion(string username, string password)
		{
			HiContext current = HiContext.Current;
			Member member = Users.GetUser(0, username, false, true) as Member;
			if (member == null || member.IsAnonymous)
			{
				this.ShowMessage("用户名或密码错误", false);
				return false;
			}
			member.Password = password;
			LoginUserStatus loginUserStatus = MemberProcessor.ValidLogin(member);
			if (loginUserStatus == LoginUserStatus.Success)
			{
				System.Web.HttpCookie authCookie = System.Web.Security.FormsAuthentication.GetAuthCookie(member.Username, false);
				IUserCookie userCookie = member.GetUserCookie();
				userCookie.WriteCookie(authCookie, 30, false);
				current.User = member;
				return true;
			}
			if (loginUserStatus == LoginUserStatus.AccountPending)
			{
				this.ShowMessage("用户账号还没有通过审核", false);
				return false;
			}
			if (loginUserStatus == LoginUserStatus.InvalidCredentials)
			{
				this.ShowMessage("用户名或密码错误", false);
				return false;
			}
			this.ShowMessage("未知错误", false);
			return false;
		}
		private bool ValidateConvert()
		{
			string text = string.Empty;
			if (HiContext.Current.User.UserRole != UserRole.Member && (string.IsNullOrEmpty(this.txtReviewUserName.Value) || string.IsNullOrEmpty(this.txtReviewPsw.Value)))
			{
				text += Formatter.FormatErrorMessage("请填写用户名和密码");
			}
			if (!string.IsNullOrEmpty(text))
			{
				this.ShowMessage(text, false);
				return false;
			}
			return true;
		}
	}
}
