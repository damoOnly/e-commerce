using EcShop.ControlPanel.Promotions;
using EcShop.Entities;
using EcShop.Entities.Promotions;
using EcShop.Entities.VShop;
using EcShop.Membership.Context;
using EcShop.Membership.Core.Enums;
using EcShop.SaleSystem.Catalog;
using EcShop.SaleSystem.Vshop;
using EcShop.UI.Common.Controls;
using EcShop.UI.SaleSystem.Tags;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace EcShop.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
    public class VSendVoucher : VMemberTemplatedWebControl
	{
        private VVoucherImage vVoucherImage;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-VSendVoucher.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{

            this.vVoucherImage = (VVoucherImage)this.FindControl("vVoucherImage");

            string nowKeyCode = "";
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["k"]))
            {
                nowKeyCode = this.Page.Request.QueryString["k"];
            }
            else
            {
                if (this.vVoucherImage != null)
                {
                    vVoucherImage.Visible = false;
                }
            }
            string k = "8c0f9f7b58864063bd83cffeb9624915";
            int voucherItemsCount = 0;

            if (nowKeyCode != k)
            {
                if (this.vVoucherImage != null)
                {
                    vVoucherImage.Visible = false;
                }
            }
            else
            {
                Member member = HiContext.Current.User as Member;

                voucherItemsCount = VoucherHelper.GetVoucherItemsCount(member.UserId, nowKeyCode);

                if (voucherItemsCount == 0)
                {
                    //通过发送方式获取优惠券列表(3为自助领劵)
                    IList<VoucherInfo> voucherList = VoucherHelper.GetVoucherBySendType(3);

                    //1.看是否登录，就是判断有没有userId,获取openId;2.往Ecshop_VoucherItems插入记录
                    VoucherItemInfo item = new VoucherItemInfo();
                    IList<VoucherItemInfo> list = new List<VoucherItemInfo>();
                    
                    string claimCode = string.Empty;
                    string password = string.Empty;
                    DateTime deadline;

                    foreach (VoucherInfo current in voucherList)
                    {
                        deadline = DateTime.Today.AddDays(current.Validity);
                        claimCode = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 15);
                        claimCode = Sign(claimCode, "UTF-8").Substring(8, 16);
                        password = System.Guid.NewGuid().ToString().Replace("-", "").Substring(0, 15);
                        item = new VoucherItemInfo(current.VoucherId, claimCode, password, new int?(member.UserId), member.Username, member.Email, System.DateTime.Now, deadline, nowKeyCode);
                        list.Add(item);
                    }

                    VoucherHelper.BulkAddVoucherItems(list);
                }

                voucherItemsCount = VoucherHelper.GetVoucherItemsCount(member.UserId, nowKeyCode);
            }

            if (voucherItemsCount > 0)
            {
                //显示页面
                if (this.vVoucherImage != null)
                {
                    //可见
                    vVoucherImage.Visible = true;
                }
            }
            else
            {
                //否则隐藏
                if (this.vVoucherImage != null)
                {
                    vVoucherImage.Visible = false;
                }
            }

            //string.Format("此次发送操作已成功，现金券发送数量：{0}", list.Count);
            PageTitle.AddSiteNameTitle("发送优惠券");
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
