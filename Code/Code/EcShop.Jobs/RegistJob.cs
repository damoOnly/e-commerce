using EcShop.ControlPanel.Promotions;
using EcShop.Core.Jobs;
using EcShop.Entities.Promotions;
using EcShop.Membership.Context;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Xml;

namespace EcShop.Jobs
{
    public class RegistJob : IJob
    {
        public void Execute(XmlNode node)
        {
            //XmlAttribute xmlAttribute = node.Attributes["expires"];
            //if (xmlAttribute != null)
            //{
            //    int.TryParse(xmlAttribute.Value, out num);
            //}
            string setdate = GetRegistJobtime();

            IList<Member> members = PromoteHelper.GetMembersByCreateDate(setdate, 10);

            if (members.Count > 0)
            {

                //RegistSendCoupon(members);

                //RegistSendVoucher(members);
                string enddate = members[members.Count - 1].AccurateCreateDate;
                //UpdateRegistJobtime(enddate);
            }

        }

        protected string GetRegistJobtime()
        {
            string setdate = string.Empty;
            Database database = DatabaseFactory.CreateDatabase();
            System.Data.Common.DbCommand sqlStringCommand = database.GetSqlStringCommand("select Value FROM Ecshop_SiteSetting WHERE [Key]= 'RegistJobSignDaste';");
            object a = database.ExecuteScalar(sqlStringCommand);
            if (a == null)
            {
                setdate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                sqlStringCommand = database.GetSqlStringCommand("insert into Ecshop_SiteSetting([Key],[Value]) values('RegistJobSignDaste',@Value)");
                database.AddInParameter(sqlStringCommand, "Value", DbType.String, setdate);
                database.ExecuteNonQuery(sqlStringCommand);
            }
            else
            {
                setdate = a.ToString();
            }

            return setdate;
        }

        protected void UpdateRegistJobtime(string value)
        {
            Database database = DatabaseFactory.CreateDatabase();
            System.Data.Common.DbCommand sqlStringCommand = database.GetSqlStringCommand("update  Ecshop_SiteSetting set Value=@Value  WHERE [Key]= 'RegistJobSignDaste';");
            database.AddInParameter(sqlStringCommand, "Value", DbType.String, value);
            database.ExecuteNonQuery(sqlStringCommand);

        }


        /// <summary>
        /// 注册赠送优惠券
        /// </summary>
        /// <param name="members"></param>
        protected void RegistSendCoupon(IList<Member> members)
        {
            IList<CouponInfo> couponList = CouponHelper.GetCouponsBySendType(2);

            IList<CouponItemInfo> list = new System.Collections.Generic.List<CouponItemInfo>();

            string claimCode = string.Empty;
            if (couponList.Count > 0)
            {
                foreach (Member member in members)
                {
                    foreach (CouponInfo coupon in couponList)
                    {
                        CouponItemInfo item = new CouponItemInfo();
                        if (CouponHelper.GetCountCouponItem(coupon.CouponId, member.UserId) > 0)
                        {
                            continue;
                        }
                        else
                        {
                            claimCode = System.Guid.NewGuid().ToString().Replace("-", "").Substring(0, 15);
                            item = new CouponItemInfo(coupon.CouponId, claimCode, new int?(member.UserId), member.Username, member.Email, System.DateTime.Now, coupon.StartTime, coupon.ClosingTime, coupon.Amount, coupon.DiscountValue);
                            list.Add(item);
                        }
                    }


                }

            }

            if (list.Count > 0)
            {
                try
                {
                    CouponHelper.SendClaimCodes(list);


                }

                catch
                {

                }
            }

        }


        /// <summary>
        /// 注册赠送现金券
        /// </summary>
        /// <param name="members"></param>
        protected void RegistSendVoucher(IList<Member> members)
        {
            IList<VoucherInfo> voucherList = VoucherHelper.GetVoucherBySendType(2);

            IList<VoucherItemInfo> list = new System.Collections.Generic.List<VoucherItemInfo>();

            string claimCode = string.Empty;
            string password = string.Empty;
            if (voucherList.Count > 0)
            {
                foreach (Member member in members)
                {
                    foreach (VoucherInfo voucher in voucherList)
                    {
                        VoucherItemInfo item = new VoucherItemInfo();
                        if (VoucherHelper.GetCountVoucherItem(voucher.VoucherId, member.UserId) > 0)
                        {
                            continue;
                        }
                        else
                        {
                            claimCode = System.Guid.NewGuid().ToString().Replace("-", "").Substring(0, 15);
                            claimCode = Sign(claimCode, "UTF-8").Substring(8, 16);
                            password = System.Guid.NewGuid().ToString().Replace("-", "").Substring(0, 15);
                            item = new VoucherItemInfo(voucher.VoucherId, claimCode, password, new int?(member.UserId), member.Username, member.Email, System.DateTime.Now, DateTime.Now.AddDays(voucher.Validity));
                            list.Add(item);
                        }
                    }


                }

            }

            if (list.Count > 0)
            {
                try
                {
                    VoucherHelper.SendClaimCodes(list);


                }

                catch
                {

                }
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
