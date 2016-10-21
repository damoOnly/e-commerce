using ASPNET.WebControls;
using EcShop.ControlPanel.Promotions;
using EcShop.ControlPanel.Store;
using EcShop.Entities.Promotions;
using EcShop.Entities.Store;
using EcShop.Core;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using Ecdev.Components.Validation;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

namespace EcShop.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.Vouchers)]
    public class AddVoucher: AdminPage
    {
        private int voucherId=0;
        protected System.Web.UI.WebControls.TextBox txtVoucherName;
        protected System.Web.UI.WebControls.TextBox txtAmount;
        protected System.Web.UI.WebControls.TextBox txtDiscountValue;
        protected WebCalendar calendarStartDate;
        protected WebCalendar calendarEndDate;
        protected System.Web.UI.WebControls.Button btnAddVouchers;
        protected System.Web.UI.WebControls.TextBox txtValidity;
        protected System.Web.UI.HtmlControls.HtmlInputRadioButton rdoManually;
        protected System.Web.UI.HtmlControls.HtmlInputRadioButton rdoOverMoney;
        protected System.Web.UI.HtmlControls.HtmlInputRadioButton rdoRegist;
        protected System.Web.UI.HtmlControls.HtmlInputRadioButton rdoLq;
        protected System.Web.UI.WebControls.TextBox txtOverMoney;
        protected void Page_Load(object sender, System.EventArgs e)
        {
            
            if (!int.TryParse(this.Page.Request.QueryString["voucherId"], out this.voucherId))
            {
                base.GotoResourceNotFound();
                return;
            }
            this.btnAddVouchers.Click += new System.EventHandler(this.btnAddVouchers_Click);

            //如果是编辑现金券
            if ((!this.Page.IsPostBack))
            {
                if(this.voucherId>0)
                { 
                VoucherInfo voucher = VoucherHelper.GetVoucher(this.voucherId);
                if (voucher == null)
                {
                    base.GotoResourceNotFound();
                    return;
                }
                if (voucher.ClosingTime.CompareTo(System.DateTime.Now) < 0)
                {
                    this.ShowMsg("该现金券已经结束！", false);
                    return;
                }
                Globals.EntityCoding(voucher, false);
                //this.lblEditCouponId.Text = coupon.CouponId.ToString();
                this.btnAddVouchers.Text = "修改";
                this.txtVoucherName.Text = voucher.Name;
                this.txtValidity.Text = voucher.Validity.ToString();
                if (voucher.Amount.HasValue)
                {
                    this.txtAmount.Text = string.Format("{0:F2}", voucher.Amount);
                }
                this.txtDiscountValue.Text = voucher.DiscountValue.ToString("F2");
                this.calendarEndDate.SelectedDate = new System.DateTime?(voucher.ClosingTime);
                this.calendarStartDate.SelectedDate = new System.DateTime?(voucher.StartTime);

                switch (voucher.SendType)
                {
                    case 0: rdoManually.Checked = true; break;

                    case 1: rdoOverMoney.Checked = true; this.txtOverMoney.Text = voucher.SendTypeItem; break;

                    case 2: rdoRegist.Checked = true; break;

                    case 3: rdoLq.Checked = true; break;
                }
                }

                else 
                {
                    rdoManually.Checked = true;
                }
               
            }

            
        }

        /// <summary>
        /// 添加按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddVouchers_Click(object sender, System.EventArgs e)
        {
            string text = string.Empty;
            string arg_0B_0 = string.Empty;
            decimal? amount;
            decimal discountValue;
            if (!this.ValidateValues(out amount, out discountValue))
            {
                return;
            }
            if (!this.calendarStartDate.SelectedDate.HasValue)
            {
                this.ShowMsg("请选择开始日期！", false);
                return;
            }
            if (!this.calendarEndDate.SelectedDate.HasValue)
            {
                this.ShowMsg("请选择结束日期！", false);
                return;
            }
            if (this.calendarStartDate.SelectedDate.Value.CompareTo(this.calendarEndDate.SelectedDate.Value) >= 0)
            {
                this.ShowMsg("开始日期不能晚于结束日期！", false);
                return;
            }

            string strValidity = this.txtValidity.Text;
            if(!Regex.IsMatch(strValidity, @"^[1-9][0-9]*$"))
            {
                this.ShowMsg(" 有效期只能是数字，必须大于等于O！", false);
                return;
            }

            string strOverMoney = txtOverMoney.Text;
            if (this.rdoOverMoney.Checked&&!Regex.IsMatch(strOverMoney, @"^(?!0+(?:\.0+)?$)(?:[1-9]\d*|0)(?:\.\d{1,2})?$"))
            {
                this.ShowMsg(" 满足金额必须为正数，且最多只能有两位小数", false);
                return;
            }
        
           
            VoucherInfo voucherInfo = new VoucherInfo();
            voucherInfo.Name = this.txtVoucherName.Text;
            voucherInfo.ClosingTime = this.calendarEndDate.SelectedDate.Value.AddDays(1).AddSeconds(-1);
            voucherInfo.StartTime = this.calendarStartDate.SelectedDate.Value;
            voucherInfo.Amount = amount;
            voucherInfo.DiscountValue = discountValue;

            #region 发送方式
            int SendType = 0;
            string stroverMoney = string.Empty;
            if (this.rdoManually.Checked)
            {
                SendType = int.Parse(this.rdoManually.Value);
            }

            else if (this.rdoOverMoney.Checked)
            {
                SendType = int.Parse(this.rdoOverMoney.Value);
                stroverMoney = txtOverMoney.Text.ToString();
            }

            else if (this.rdoRegist.Checked)
            {
                SendType = int.Parse(this.rdoRegist.Value);
            }

            else if (this.rdoLq.Checked)
            {
                SendType = int.Parse(this.rdoLq.Value);
            }


            #endregion

            voucherInfo.SendType = SendType;
            voucherInfo.SendTypeItem = stroverMoney;
            voucherInfo.Validity = int.Parse(this.txtValidity.Text);

            #region 字段限制验证，通过数据注解验证的方式
            ValidationResults validationResults = Validation.Validate<VoucherInfo>(voucherInfo, new string[]
			{
				"Voucher"
			});
            if (!validationResults.IsValid)
            {
                using (System.Collections.Generic.IEnumerator<ValidationResult> enumerator = ((System.Collections.Generic.IEnumerable<ValidationResult>)validationResults).GetEnumerator())
                {
                    if (enumerator.MoveNext())
                    {
                        ValidationResult current = enumerator.Current;
                        text += Formatter.FormatErrorMessage(current.Message);
                        this.ShowMsg(text, false);
                        return;
                    }
                }
            }
            #endregion

            string empty = string.Empty;
            if (this.voucherId == 0)  //创建现金券
            {
                VoucherActionStatus voucherActionStatus = VoucherHelper.CreateVoucher(voucherInfo, 0, out empty,1);
                if (voucherActionStatus == VoucherActionStatus.UnknowError)
                {
                    this.ShowMsg("未知错误", false);
                }
                else
                {
                    if (voucherActionStatus == VoucherActionStatus.DuplicateName)
                    {
                        this.ShowMsg("已经存在相同的现金券名称", false);
                        return;
                    }
                    if (voucherActionStatus == VoucherActionStatus.CreateClaimCodeError)
                    {
                        this.ShowMsg("生成现金券号码错误", false);
                        return;
                    }
                    this.ShowMsg("添加现金券成功", true);
                    this.RestCoupon();
                    return;
                }
            }

            else  //修改现金券
            {
                voucherInfo.VoucherId = this.voucherId;
                VoucherActionStatus voucherActionStatus = VoucherHelper.UpdateVoucher(voucherInfo);
                if (voucherActionStatus == VoucherActionStatus.Success)
                {
                    this.RestCoupon();
                    this.ShowMsg("成功修改了现金券信息", true);
                }
                else
                {
                    if (voucherActionStatus == VoucherActionStatus.DuplicateName)
                    {
                        this.ShowMsg("修改现金券信息错误,已经具有此现金券名称", false);
                        return;
                    }
                    this.ShowMsg("未知错误", false);
                    this.RestCoupon();
                    return;
                }
            }
        }
        private void RestCoupon()
        {
            this.txtVoucherName.Text = string.Empty;
            this.txtAmount.Text = string.Empty;
            this.txtDiscountValue.Text = string.Empty;
        }
        private bool ValidateValues(out decimal? amount, out decimal discount)
        {
            string text = string.Empty;
            amount = null;
            if (!string.IsNullOrEmpty(this.txtAmount.Text.Trim()))
            {
                decimal value;
                if (decimal.TryParse(this.txtAmount.Text.Trim(), out value))
                {
                    amount = new decimal?(value);
                }
                else
                {
                    text += Formatter.FormatErrorMessage("满足金额必须为0-1000万之间");
                }
            }
            if (!decimal.TryParse(this.txtDiscountValue.Text.Trim(), out discount))
            {
                text += Formatter.FormatErrorMessage("现金券金额必须在0.01-1000万之间");
            }
            if (!string.IsNullOrEmpty(text))
            {
                this.ShowMsg(text, false);
                return false;
            }
            return true;
        }
    }
}
