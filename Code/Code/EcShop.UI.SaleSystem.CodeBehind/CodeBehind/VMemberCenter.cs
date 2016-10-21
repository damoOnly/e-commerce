using EcShop.Core;
using EcShop.Core.ErrorLog;
using EcShop.Entities;
using EcShop.Entities.Members;
using EcShop.Entities.Orders;
using EcShop.Entities.Sales;
using EcShop.Membership.Context;
using EcShop.Membership.Core;
using EcShop.SaleSystem.Catalog;
using EcShop.SaleSystem.Member;
using EcShop.SaleSystem.Shopping;
using EcShop.UI.Common.Controls;
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.CodeBehind
{
    [System.Web.UI.ParseChildren(true)]
    public class VMemberCenter : VMemberTemplatedWebControl
    {
        private System.Web.UI.WebControls.Literal litUserLink;
        private System.Web.UI.WebControls.Literal litUserName;
        private System.Web.UI.WebControls.Literal litExpenditure;
        private System.Web.UI.WebControls.Literal litPoints;
        private System.Web.UI.WebControls.Literal litMemberGrade;
        private System.Web.UI.WebControls.Literal litWaitForRecieveCount;
        private System.Web.UI.WebControls.Literal litWaitForPayCount;
        private System.Web.UI.WebControls.Literal litAllOrderCount;
        private System.Web.UI.WebControls.Literal litRefundCount;
        private System.Web.UI.WebControls.Literal litReturnCount;
        private System.Web.UI.WebControls.Literal litReplaceCount;
        private System.Web.UI.WebControls.Literal litPaymentBalance;
        private System.Web.UI.WebControls.HyperLink referralLink;
        private System.Web.UI.WebControls.HyperLink bindAccountLink;
        private System.Web.UI.WebControls.HyperLink switchAccountLink;
        private System.Web.UI.WebControls.Literal litCoupons;
        private System.Web.UI.WebControls.Literal litVoucher;
        private System.Web.UI.HtmlControls.HtmlAnchor litBindPhone;

        protected override void OnInit(System.EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VMemberCenter.html";
            }
            base.OnInit(e);
        }
        protected override void AttachChildControls()
        {
            PageTitle.AddSiteNameTitle("会员中心");
            Member member = HiContext.Current.User as Member;
            if (member == null)
            {
                this.Page.Response.Redirect("/Vshop/Login.aspx");
            }
            this.litUserLink = (System.Web.UI.WebControls.Literal)this.FindControl("litUserLink");
            this.litUserName = (System.Web.UI.WebControls.Literal)this.FindControl("litUserName");
            this.litPaymentBalance = (System.Web.UI.WebControls.Literal)this.FindControl("litPaymentBalance");
            this.litExpenditure = (System.Web.UI.WebControls.Literal)this.FindControl("litExpenditure");
            this.litExpenditure.SetWhenIsNotNull(member.Expenditure.ToString("F2"));
            this.litPaymentBalance.SetWhenIsNotNull(member.Balance.ToString("F2"));
            this.litPoints = (System.Web.UI.WebControls.Literal)this.FindControl("litPoints");
            this.referralLink = (System.Web.UI.WebControls.HyperLink)this.FindControl("referralLink");
            this.bindAccountLink = (System.Web.UI.WebControls.HyperLink)this.FindControl("bindAccountLink");
            this.litAllOrderCount = (System.Web.UI.WebControls.Literal)this.FindControl("litAllOrderCount");
            this.litRefundCount = (System.Web.UI.WebControls.Literal)this.FindControl("litRefundCount");
            this.litReturnCount = (System.Web.UI.WebControls.Literal)this.FindControl("litReturnCount");
            this.litReplaceCount = (System.Web.UI.WebControls.Literal)this.FindControl("litReplaceCount");
            this.switchAccountLink = (System.Web.UI.WebControls.HyperLink)this.FindControl("switchAccountLink");
            this.litCoupons = (System.Web.UI.WebControls.Literal)this.FindControl("litCoupons");
            this.litVoucher = (System.Web.UI.WebControls.Literal)this.FindControl("litVoucher");
            this.litBindPhone = (System.Web.UI.HtmlControls.HtmlAnchor)this.FindControl("litBindPhone");
            Regex regMobile = new Regex("^(13|14|15|17|18)\\d{9}$");
            if (!regMobile.IsMatch(member.Username))
            {
                this.litBindPhone.Visible = true;
            }
            else
            {
                this.litBindPhone.Visible = false;
            }
            if (this.litPoints != null)
            {
                this.litPoints.SetWhenIsNotNull(member.Points.ToString("F2"));
            }
            this.litMemberGrade = (System.Web.UI.WebControls.Literal)this.FindControl("litMemberGrade");
            MemberGradeInfo memberGrade = MemberProcessor.GetMemberGrade(member.GradeId);
            if (memberGrade != null)
            {
                this.litMemberGrade.SetWhenIsNotNull(memberGrade.Name);
            }
            this.litUserName.Text = (string.IsNullOrEmpty(member.RealName) ? member.Username : member.RealName);
            this.litWaitForRecieveCount = (System.Web.UI.WebControls.Literal)this.FindControl("litWaitForRecieveCount");
            this.litWaitForPayCount = (System.Web.UI.WebControls.Literal)this.FindControl("litWaitForPayCount");
            OrderQuery orderQuery = new OrderQuery();
            orderQuery.Status = OrderStatus.WaitBuyerPay;
            int userOrderCount = MemberProcessor.GetUserOrderCount(HiContext.Current.User.UserId, orderQuery);
            //
            HttpCookie httpCookieMember = new HttpCookie("wait");
            httpCookieMember.Value = userOrderCount.ToString();
            httpCookieMember.Expires = System.DateTime.Now.AddYears(1);
            HttpContext.Current.Response.Cookies.Add(httpCookieMember);

            #region 处理cookie中的购物车和收藏信息
            Member curMember = HiContext.Current.User as Member;
            if (curMember != null && !curMember.IsAnonymous)
            {
                //修改TopRegionId
                if (curMember.TopRegionId == 0)
                {
                    long ip = 0;
                    try
                    {
                        ip = Globals.IpToInt(Globals.IPAddress);                 
                    }
                    catch
                    {
                    }
                    if (ip != 0)
                    {
                        string provinceName = TradeHelper.GetProvinceName(ip);
                        int ProvinceId = 0;
                        if (!string.IsNullOrEmpty(provinceName))
                        {
                            provinceName = provinceName.Replace("市", "");
                            ProvinceId = RegionHelper.GetProvinceId(provinceName);
                            ErrorLog.Write(string.Format("更新会员的ProvinceId：{0}", ProvinceId));
                            if (ProvinceId != 0)
                            {
                                UserHelper.UpdateUserTopRegionId(curMember.UserId, ProvinceId);
                            }
                        }
                    }
                }
                //
                ShoppingCartInfo cookieShoppingCart = ShoppingCartProcessor.GetCookieShoppingCart();
                if (cookieShoppingCart != null)
                {
                    ShoppingCartProcessor.ConvertShoppingCartToDataBase(cookieShoppingCart);
                    ShoppingCartProcessor.ClearCookieShoppingCart();
                }

                System.Web.HttpCookie cookieFavorite = HiContext.Current.Context.Request.Cookies["Hid_Ecshop_Favorite_Data_New"];
                if (cookieFavorite != null && !string.IsNullOrEmpty(cookieFavorite.Value))
                {
                    string[] favoriteProductIds = cookieFavorite.Value.Split('|');
                    int productId = 0;
                    foreach (string fav in favoriteProductIds)
                    {
                        try
                        {
                            productId = int.Parse(fav);
                            int favoriteId;
                            ProductBrowser.AddProductToFavorite(productId, curMember.UserId, out favoriteId);
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    cookieFavorite.Expires = DateTime.Now.AddDays(-1);
                    HttpContext.Current.Response.Cookies.Add(cookieFavorite);
                }
            }
            #endregion

            //购物车商品数量
            ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetShoppingCart();
            string quantity = "0";
            if (shoppingCart != null)
            {
                quantity = shoppingCart.GetQuantity().ToString();
            }
            HttpCookie httpCookieShoppingCart = new HttpCookie("cn");
            httpCookieShoppingCart.Value = quantity;
            httpCookieShoppingCart.Expires = System.DateTime.Now.AddYears(1);
            HttpContext.Current.Response.Cookies.Add(httpCookieShoppingCart);

            this.litWaitForPayCount.SetWhenIsNotNull(userOrderCount.ToString());
            orderQuery.Status = OrderStatus.SellerAlreadySent;
            userOrderCount = MemberProcessor.GetUserOrderCount(HiContext.Current.User.UserId, orderQuery);
            this.litWaitForRecieveCount.SetWhenIsNotNull(userOrderCount.ToString());
            //
            orderQuery.Status = OrderStatus.All;
            userOrderCount = MemberProcessor.GetUserOrderCount(HiContext.Current.User.UserId, orderQuery);
            this.litAllOrderCount.SetWhenIsNotNull(userOrderCount.ToString());

            userOrderCount = MemberProcessor.GetRefundCount(HiContext.Current.User.UserId);
            this.litRefundCount.SetWhenIsNotNull(userOrderCount.ToString());
            userOrderCount = MemberProcessor.GetReturnCount(HiContext.Current.User.UserId);
            this.litReturnCount.SetWhenIsNotNull(userOrderCount.ToString());
            userOrderCount = MemberProcessor.GetReplaceCount(HiContext.Current.User.UserId);
            this.litReplaceCount.SetWhenIsNotNull(userOrderCount.ToString());

            int UserNotReadCoupons = MemberProcessor.GetUserNotReadCoupons(HiContext.Current.User.UserId);
            this.litCoupons.SetWhenIsNotNull(UserNotReadCoupons.ToString());

            int UserNotReadlitVoucher = MemberProcessor.GetUserNotReadVoucher(HiContext.Current.User.UserId);
            if (this.litVoucher != null)
            {
                this.litVoucher.SetWhenIsNotNull(UserNotReadlitVoucher.ToString());
            }

            if (this.litUserLink != null)
            {
                System.Uri url = System.Web.HttpContext.Current.Request.Url;
                string text = (url.Port == 80) ? string.Empty : (":" + url.Port.ToString(System.Globalization.CultureInfo.InvariantCulture));
                this.litUserLink.Text = string.Concat(new object[]
				{
					string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}://{1}{2}", new object[]
					{
						url.Scheme,
						url.Host,
						text
					}),
					Globals.ApplicationPath,
					"/VShop/?ReferralUserId=",
					HiContext.Current.User.UserId
				});
            }
            if (this.referralLink != null)
            {
                this.referralLink.CssClass = "list-group-item";
                if (member.ReferralStatus == 0 || member.ReferralStatus == 1 || member.ReferralStatus == 3)
                {
                    this.referralLink.Text = "申请成为推广员";
                    if (member.ReferralStatus == 1 || member.ReferralStatus == 3)
                    {
                        this.referralLink.NavigateUrl = "/VShop/ReferralRegisterresults.aspx";
                    }
                    else
                    {
                        this.referralLink.NavigateUrl = "/VShop/ReferralRegisterAgreement.aspx";
                    }
                }
                if (member.ReferralStatus == 2)
                {
                    this.referralLink.Text = "推广员";
                    this.referralLink.NavigateUrl = "/VShop/Referral.aspx";
                }
            }

            if (bindAccountLink != null)
            {
                this.bindAccountLink.CssClass = "list-group-item";
                this.bindAccountLink.Text = "绑定PC端会员帐号";
                this.bindAccountLink.NavigateUrl = "/VShop/BindPCAccount.aspx";
            }

            int totalCount = 0;
            string opernId = member.OpenId;
            if (!string.IsNullOrEmpty(opernId))
            {
                totalCount = UserHelper.GetToalCountByOpenId(opernId);
            }

            if (switchAccountLink != null && totalCount >= 2)
            {
                this.switchAccountLink.CssClass = "list-group-item";
                this.switchAccountLink.Text = "切换帐号";
                this.switchAccountLink.NavigateUrl = "/VShop/SwitchAccount.aspx";
            }
        }
    }
}
