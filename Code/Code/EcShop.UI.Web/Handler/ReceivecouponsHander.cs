using EcShop.Core;
using EcShop.Core.Configuration;
using EcShop.Entities.Sales;
using EcShop.Membership.Context;
using EcShop.Membership.Core;
using EcShop.Membership.Core.Enums;
using EcShop.Messages;
using EcShop.SaleSystem.Catalog;
using EcShop.SaleSystem.Member;
using EcShop.SaleSystem.Shopping;
using EcShop.UI.Common.Controls;
using Ecdev.Components.Validation;
using Ecdev.Plugins;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using EcShop.ControlPanel.Members;
using EcShop.ControlPanel.Promotions;
using EcShop.Entities.Promotions;
namespace EcShop.UI.Web.Handler
{

    public class ReceivecouponsHander : System.Web.IHttpHandler
    {
        private string message = "";
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public void ProcessRequest(System.Web.HttpContext context)
        {
            context.Response.ContentType = "text/json";
            string text = context.Request["action"];
            string key;
            switch (key = text)
            {
                case "ReceiveCoupons":
                    this.ReceiveCoupons(context);
                    break;

                case "ReceiveVouchers":
                    this.ReceiveVouchers(context);
                    break;
                case "GetSupplierCoupons":
                    this.GetSupplierCoupons(context);
                    break;
                case "GetRegisterCoupons":
                    this.GetRegisterCoupons(context);
                    break;

            }
            context.Response.Write(this.message);
        }

        private void ReceiveCoupons(System.Web.HttpContext context)
        {
            string text="";
            if (context.Request["couponid"] != null)
            {
                text = context.Request["couponid"].ToString();
            }
            Member member = HiContext.Current.User as Member;
            if (member == null)
            {
                this.message = "你还未登陆，请登陆";
                return;
            }
            int couponid = 0;
            if (!int.TryParse(text, out couponid))
            {
                this.message = "该链接无效";
                return;
            }

            CouponInfo couponInfo = CouponHelper.GetCoupon(couponid);
            if (couponInfo == null || couponInfo.SendType != 3 || couponInfo.StartTime.Date > DateTime.Now.Date || couponInfo.ClosingTime < DateTime.Now.Date)
            {
                this.message = "该链接无效";
                return;
            }

            int count = CouponHelper.GetCountCouponItem(couponid, member.UserId);

            if (count == 0)
            {

                string claimCode = System.Guid.NewGuid().ToString().Replace("-", "").Substring(0, 15);
                CouponItemInfo couponItemInfo = new CouponItemInfo(couponid, claimCode, new int?(member.UserId), member.Username, member.Email, System.DateTime.Now, couponInfo.StartTime, couponInfo.ClosingTime, couponInfo.Amount, couponInfo.DiscountValue);

                IList<CouponItemInfo> couponItemList = new List<CouponItemInfo>();
                couponItemList.Add(couponItemInfo);
                if (CouponHelper.SendClaimCodes(couponItemList))
                {
                    this.message = "你已成功领取该优惠券";
                }

                else
                {
                    this.message = "操作失败";
                }
                
            }
            else
            {
                this.message = "您已经领过该优惠券";
            }
        }


        private void ReceiveVouchers(System.Web.HttpContext context)
        {
            string text = "";
            if (context.Request["voucherid"] != null)
            {
               text = context.Request["voucherid"].ToString();
            }
            Member member = HiContext.Current.User as Member;
            if (member == null)
            {
                this.message = "你还未登陆，请登陆";
                return;
            }
            int voucherid = 0;
            if (!int.TryParse(text, out voucherid))
            {
                this.message = "该链接无效";
                return;
            }

            VoucherInfo voucherInfo = VoucherHelper.GetVoucher(voucherid);
            if (voucherInfo == null ||voucherInfo.SendType!=3|| voucherInfo.StartTime.Date > DateTime.Now.Date || voucherInfo.ClosingTime < DateTime.Now.Date)
            {
                this.message = "该链接无效";
                return;
            }

            int count = VoucherHelper.GetCountVoucherItem(voucherid, member.UserId);

            if (count == 0)
            {

                string claimCode = System.Guid.NewGuid().ToString().Replace("-", "").Substring(0, 15);
                claimCode = Sign(claimCode, "UTF-8").Substring(8, 16);
                string password = System.Guid.NewGuid().ToString().Replace("-", "").Substring(0, 15);
                DateTime deadline = DateTime.Now.AddDays(voucherInfo.Validity);
                VoucherItemInfo voucherItemInfo = new VoucherItemInfo(voucherid, claimCode, password, new int?(member.UserId), member.Username, member.Email, System.DateTime.Now, deadline);

                IList<VoucherItemInfo> voucherItemList = new List<VoucherItemInfo>();
                voucherItemList.Add(voucherItemInfo);
                if (VoucherHelper.SendClaimCodes(voucherItemList))
                {
                    this.message = "你已成功领取该优惠券";
                }

                else
                {
                    this.message = "操作失败";
                }

            }
            else
            {
                this.message = "您已经领过该优惠券";
            }
        }

        private void GetSupplierCoupons(System.Web.HttpContext context)
        {
            string text = "";
            if (context.Request["CouponId"] != null)
            {
                text = context.Request["CouponId"].ToString();
            }
            Member member = HiContext.Current.User as Member;
            if (member == null)
            {
                this.message = "{\"Success\":false,\"Message\":\"你还未登陆，请登陆\"}";
                return;
            }
            int couponid = 0;
            if (!int.TryParse(text, out couponid))
            {
                this.message = "{\"Success\":false,\"Message\":\"该优惠卷无效\"}";
                return;
            }

            CouponInfo couponInfo = CouponHelper.GetCoupon(couponid);
            if (couponInfo == null || couponInfo.ClosingTime < DateTime.Now.Date)
            {
                this.message = "{\"Success\":false,\"Message\":\"该优惠卷无效\"}";
                return;
            }

            int count = CouponHelper.GetCountCouponItem(couponid, member.UserId);

            if (count == 0)
            {

                string claimCode = System.Guid.NewGuid().ToString().Replace("-", "").Substring(0, 15);
                CouponItemInfo couponItemInfo = new CouponItemInfo(couponid, claimCode, new int?(member.UserId), member.Username, member.Email, System.DateTime.Now, couponInfo.StartTime, couponInfo.ClosingTime, couponInfo.Amount, couponInfo.DiscountValue);

                IList<CouponItemInfo> couponItemList = new List<CouponItemInfo>();
                couponItemList.Add(couponItemInfo);
                if (CouponHelper.SendClaimCodes(couponItemList))
                {
                    this.message = "{\"Success\":true,\"Message\":\"成功领取该优惠券\"}";
                }

                else
                {
                    this.message = "{\"Success\":false,\"Message\":\"该优惠卷无效\"}";
                }

            }
            else
            {
                this.message = "{\"Success\":false,\"Message\":\"您已经领过该优惠券\"}";
            }
        }

        /// <summary>
        /// 微信新用户注册用60元大礼包，在活动页面手动领取
        /// </summary>
        /// <param name="context"></param>
        private void GetRegisterCoupons(System.Web.HttpContext context)
        {
            Member member = HiContext.Current.User as Member;
            if (member == null)
            {
                this.message = "{\"Success\":false,\"Message\":\"你还未登陆，请登陆\"}";
                return;
            }

            int count = CouponHelper.GetCountCouponItemed(member.UserId, 3);
            if (count == 0)
            {
                IList<CouponInfo> couponList = CouponHelper.GetCouponsBySendType(3);
                IList<CouponItemInfo> couponItemList = new List<CouponItemInfo>();
                string claimCode = string.Empty;
                if (couponList != null && couponList.Count > 0)
                {
                    foreach (CouponInfo coupon in couponList)
                    {
                        CouponItemInfo item = new CouponItemInfo();
                        claimCode = System.Guid.NewGuid().ToString().Replace("-", "").Substring(0, 15);
                        item = new CouponItemInfo(coupon.CouponId, claimCode, new int?(member.UserId), member.Username, member.Email, System.DateTime.Now, coupon.StartTime, coupon.ClosingTime, coupon.Amount, coupon.DiscountValue);
                        couponItemList.Add(item);
                    }
                }
                else
                {
                    this.message = "{\"Success\":false,\"Message\":\"没有可以领取的优惠券\"}";
                    return;
                }
                if (CouponHelper.SendClaimCodes(couponItemList))
                {
                    this.message = "{\"Success\":true,\"Message\":\"成功领取该优惠券\"}";
                }

                else
                {
                    this.message = "{\"Success\":false,\"Message\":\"该优惠卷无效\"}";
                }

            }
            else
            {
                this.message = "{\"Success\":false,\"Message\":\"您已经领过该优惠券\"}";
            }
        }

        /// <summary>
        /// MD5签名
        /// </summary>
        /// <param name="prestr"></param>
        /// <param name="_input_charset"></param>
        /// <returns></returns>
        public static string Sign(string prestr, string _input_charset)
        {
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder(32);
            System.Security.Cryptography.MD5 mD = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] array = mD.ComputeHash(System.Text.Encoding.GetEncoding(_input_charset).GetBytes(prestr));
            for (int i = 0; i < array.Length; i++)
            {
                stringBuilder.Append(array[i].ToString("x").PadLeft(2, '0'));
            }

            return stringBuilder.ToString();
        }
    }
}
