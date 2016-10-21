using ASPNET.WebControls;
using EcShop.ControlPanel.Promotions;
using EcShop.ControlPanel.Store;
using EcShop.Entities.Promotions;
using EcShop.Entities.Store;
using EcShop.Membership.Context;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.Vouchers)]
    public class SendVoucherToUsers : AdminPage
    {
        private int voucherId;
        protected System.Web.UI.HtmlControls.HtmlInputRadioButton rdoName;
        protected System.Web.UI.HtmlControls.HtmlInputRadioButton rdoRank;
        protected MemberGradeDropDownList rankList;
        protected System.Web.UI.WebControls.TextBox txtMemberNames;
        protected WebCalendar registerFromDate;
        protected WebCalendar registerToDate;
        protected RegionSelector ddlReggion;
        protected System.Web.UI.WebControls.Button btnSend;
        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!int.TryParse(this.Page.Request.QueryString["voucherId"], out this.voucherId))
            {
                base.GotoResourceNotFound();
                return;
            }
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            if (!this.Page.IsPostBack)
            {
                this.rankList.DataBind();
            }
        }
        private bool IsMembers(string name)
        {
            string pattern = "[\\u4e00-\\u9fa5a-zA-Z]+[\\u4e00-\\u9fa5_a-zA-Z0-9]*";
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(pattern);
            return regex.IsMatch(name) && name.Length >= 2 && name.Length <= 20;
        }
        private void btnSend_Click(object sender, System.EventArgs e)
        {
            VoucherInfo voucherInfo = VoucherHelper.GetVoucher(voucherId);
            DateTime deadline = DateTime.Now.AddDays(voucherInfo.Validity);
            VoucherItemInfo item = new VoucherItemInfo();
            System.Collections.Generic.IList<VoucherItemInfo> list = new System.Collections.Generic.List<VoucherItemInfo>();
            System.Collections.Generic.IList<Member> list2 = new System.Collections.Generic.List<Member>();
            if (this.rdoName.Checked)
            {
                if (!string.IsNullOrEmpty(this.txtMemberNames.Text.Trim()))
                {
                    System.Collections.Generic.IList<string> list3 = new System.Collections.Generic.List<string>();
                    string text = this.txtMemberNames.Text.Trim().Replace("\r\n", "\n");
                    string[] array = text.Replace("\n", "*").Split(new char[]
                    {
                        '*'
                    });
                    for (int i = 0; i < array.Length; i++)
                    {
                        list3.Add(array[i]);
                    }
                    list2 = PromoteHelper.GetMemdersByNames(list3);
                }
                string claimCode = string.Empty;
                string password = string.Empty;
                foreach (Member current in list2)
                {
                    claimCode = System.Guid.NewGuid().ToString().Replace("-", "").Substring(0, 15);
                    claimCode = Sign(claimCode,"UTF-8").Substring(8, 16);
                    password = System.Guid.NewGuid().ToString().Replace("-", "").Substring(0, 15);
                    item = new VoucherItemInfo(this.voucherId, claimCode, password, new int?(current.UserId), current.Username, current.Email, System.DateTime.Now,deadline);
                    list.Add(item);
                }
                if (list.Count <= 0)
                {
                    this.ShowMsg("你输入的会员名中没有一个正确的，请输入正确的会员名", false);
                    return;
                }
                VoucherHelper.SendClaimCodes(list);
                this.txtMemberNames.Text = string.Empty;
                this.ShowMsg(string.Format("此次发送操作已成功，现金券发送数量：{0}", list.Count), true);
            }
            if (this.rdoRank.Checked)
            {
                StringBuilder strcondition = new StringBuilder();
                strcondition.Append("1=1");
                if (this.rankList.SelectedValue > 0)
                {
                    strcondition.AppendFormat(" and GradeId={0} ", this.rankList.SelectedValue);
                }

                if (this.registerFromDate.SelectedDate.HasValue && this.registerToDate.SelectedDate.HasValue)
                {
                    strcondition.AppendFormat(" and CreateDate>='{0}' and CreateDate<='{1}' ", this.registerFromDate.SelectedDate.Value, this.registerToDate.SelectedDate.Value);
                }

                if (this.ddlReggion.GetSelectedRegionId().HasValue)
                {
                    strcondition.AppendFormat(" and TopRegionId={0} ", this.ddlReggion.GetSelectedRegionId().Value);
                }


                list2 = PromoteHelper.GetMembersByCondition(strcondition.ToString());
                string claimCode2 = string.Empty;
                string password2 = string.Empty;
                foreach (Member current2 in list2)
                {
                    claimCode2 = System.Guid.NewGuid().ToString().Replace("-", "").Substring(0, 15);
                    claimCode2 = Sign(claimCode2, "UTF-8").Substring(8, 16);
                    password2 = System.Guid.NewGuid().ToString().Replace("-", "").Substring(0, 15);
                    item = new VoucherItemInfo(this.voucherId, claimCode2, password2, new int?(current2.UserId), current2.Username, current2.Email, System.DateTime.Now,deadline);
                    list.Add(item);
                }
                if (list.Count <= 0)
                {
                    this.ShowMsg("您选择的条件下面没有符合条件的会员", false);
                    return;
                }
                VoucherHelper.SendClaimCodes(list);
                this.txtMemberNames.Text = string.Empty;
                this.ShowMsg(string.Format("此次发送操作已成功，现金券发送数量：{0}", list.Count), true);
            }
        }

        public string GetRandomPassword(int passwordLen)
        {
            string randomChars = "$#&*ABCDFGHJKNMPQRSTVWXY123467089";
            string password = string.Empty;
            int randomNum;
            Random random = new Random();
            for (int i = 0; i < passwordLen; i++)
            {
                randomNum = random.Next(randomChars.Length);
                password += randomChars[randomNum];
            }
            return password;
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
