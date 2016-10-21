using ASPNET.WebControls;
using EcShop.ControlPanel.Promotions;
using EcShop.ControlPanel.Store;
using EcShop.Entities.Promotions;
using EcShop.Entities.Store;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using Ecdev.Components.Validation;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Saplin.Controls;
using EcShop.ControlPanel.Commodities;
using System.Data;
using System.Text;
using EcShop.Membership.Context;
using EcShop.Core;
using System.Text.RegularExpressions;
namespace EcShop.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.Coupons)]
    public class AddCoupon : AdminPage
    {
        private int couponId = 0;
        protected System.Web.UI.WebControls.TextBox txtCouponName;
        protected System.Web.UI.WebControls.TextBox txtAmount;
        protected System.Web.UI.WebControls.TextBox txtDiscountValue;
        protected WebCalendar calendarStartDate;
        protected WebCalendar calendarEndDate;
        protected WebCalendar outBeginDate;
        protected WebCalendar outEndDate;
        protected System.Web.UI.WebControls.TextBox txtNeedPoint;
        protected System.Web.UI.WebControls.Button btnAddCoupons;

        protected System.Web.UI.HtmlControls.HtmlInputRadioButton rdoAll;
        protected System.Web.UI.HtmlControls.HtmlInputRadioButton rdoProduct;
        protected System.Web.UI.HtmlControls.HtmlInputRadioButton rdoProductType;
        protected System.Web.UI.HtmlControls.HtmlInputRadioButton rdoBrand;
        protected System.Web.UI.HtmlControls.HtmlInputRadioButton rdoSupplier;
        protected System.Web.UI.HtmlControls.HtmlInputRadioButton rdoManually;
        protected System.Web.UI.HtmlControls.HtmlInputRadioButton rdoOverMoney;
        protected System.Web.UI.HtmlControls.HtmlInputRadioButton rdoRegist;
        protected System.Web.UI.HtmlControls.HtmlInputRadioButton rdoLq;
        protected System.Web.UI.HtmlControls.HtmlInputRadioButton rdorefund;
        protected System.Web.UI.HtmlControls.HtmlInputRadioButton rdZC;

        protected System.Web.UI.WebControls.TextBox txtOverMoney;
        protected System.Web.UI.HtmlControls.HtmlInputRadioButton rdoSEnable;
        protected System.Web.UI.HtmlControls.HtmlInputRadioButton rdoSDisEnable;
        protected System.Web.UI.WebControls.TextBox txtRemark;

        protected DropDownCheckBoxes ddcSupplier;
        protected DropDownCheckBoxes ddcBrand;

        protected static Dictionary<int, string> dcProduct = new Dictionary<int, string>();

        protected static Dictionary<int, string> dcProductType = new Dictionary<int, string>();

        protected static Dictionary<int, string> dcBrand = new Dictionary<int, string>();

        protected static Dictionary<int, string> dcSupplier = new Dictionary<int, string>();
        protected System.Web.UI.WebControls.TextBox txtTotalQ;
        protected System.Web.UI.WebControls.TextBox txtValidity;
        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!int.TryParse(this.Page.Request.QueryString["couponId"], out this.couponId))
            {
                base.GotoResourceNotFound();
                return;
            }
            this.btnAddCoupons.Click += new System.EventHandler(this.btnAddCoupons_Click);

            this.ddcSupplier.SelectedIndexChanged += new System.EventHandler(this.ddcSupplier_SelectedIndexChanged);
            this.ddcBrand.SelectedIndexChanged += new System.EventHandler(this.ddcBrand_SelectedIndexChanged);

            if(this.couponId>0)
            {
                btnAddCoupons.Text = "保存";
            }
            if (!IsPostBack)
            {
                BindSupplier();
                BindBrand();
                if (couponId > 0)
                {
                    CouponInfo coupon = CouponHelper.GetCoupon(this.couponId);
                    if (coupon == null)
                    {
                        base.GotoResourceNotFound();
                        return;
                    }
                    if (coupon.SendType!=4 && coupon.ClosingTime.CompareTo(System.DateTime.Now) < 0)
                    {
                        this.ShowMsg("该优惠券已经结束！", false);
                        return;
                    }
                    Globals.EntityCoding(coupon, false);
                    this.txtCouponName.Text = coupon.Name;
                    if (coupon.Amount.HasValue)
                    {
                        this.txtAmount.Text = string.Format("{0:F2}", coupon.Amount);
                    }
                    this.txtDiscountValue.Text = coupon.DiscountValue.ToString("F2");
                    this.calendarEndDate.SelectedDate = new System.DateTime?(coupon.ClosingTime);
                    this.calendarStartDate.SelectedDate = new System.DateTime?(coupon.StartTime);
                    this.txtNeedPoint.Text = coupon.NeedPoint.ToString();
                    this.outBeginDate.SelectedDate = new DateTime?(coupon.OutBeginDate);
                    this.outEndDate.SelectedDate = new DateTime?(coupon.OutEndDate);
                    this.txtTotalQ.Text = coupon.TotalQ.ToString();
                    this.txtRemark.Text = coupon.Remark.ToString();
                    if (coupon.Validity > 0)
                    {
                        this.txtValidity.Text = coupon.Validity.ToString();
                    }

                    IList<CouponsSendTypeItem> listSendTypeItem = CouponHelper.GetCouponsSendTypeItems(coupon.CouponId);
                    switch(coupon.UseType)
                    {
                        case 0: rdoAll.Checked = true; break;

                        case 1: rdoProduct.Checked = true; break;

                        case 2: rdoProductType.Checked = true; break;

                        case 3: rdoBrand.Checked = true; setSelectBrand(listSendTypeItem); break;

                        case 4: rdoSupplier.Checked = true; setSelectSupplier(listSendTypeItem); break;
                        default: break;

                    }

                    switch(coupon.SendType)
                    {
                        case 0: rdoManually.Checked = true; break;

                        case 1: rdoOverMoney.Checked = true; this.txtOverMoney.Text = coupon.SendTypeItem; break;

                        case 2: rdoRegist.Checked = true; break;

                        case 3: rdoLq.Checked = true; break;

                        case 4: rdorefund.Checked = true; break;
                        case 5: rdZC.Checked = true; break;
                    }
                    switch (coupon.Status)
                    { 
                        case 0:
                            rdoSDisEnable.Checked = true;
                            break;
                        case 1:
                            this.rdoSEnable.Checked = true;
                            break;
                    }
                }

                else
                {
                    rdoAll.Checked = true;
                    rdoManually.Checked = true;
                    rdoSEnable.Checked = true;
                }

            }
        }
        private void btnAddCoupons_Click(object sender, System.EventArgs e)
        {
            string text = string.Empty;
            string arg_0B_0 = string.Empty;
            decimal? amount;
            decimal discountValue;
            int needPoint;
            int totalq;
            if (!this.ValidateValues(out amount, out discountValue, out needPoint,out totalq))
            {
                return;
            }
            CouponInfo couponInfo = new CouponInfo();
            #region 使用方式
            int UseType = 0;
            if (this.rdoAll.Checked)
            {
                UseType = int.Parse(this.rdoAll.Value);
            }

            else if (this.rdoProduct.Checked)
            {
                UseType = int.Parse(this.rdoProduct.Value);
            }

            else if (this.rdoProductType.Checked)
            {
                UseType = int.Parse(this.rdoProductType.Value);
            }

            else if (this.rdoBrand.Checked)
            {
                UseType = int.Parse(this.rdoBrand.Value);
            }

            else if (this.rdoSupplier.Checked)
            {
                UseType = int.Parse(this.rdoSupplier.Value);
            }
            #endregion


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
            else if (this.rdorefund.Checked)
            {
                SendType = int.Parse(this.rdorefund.Value);
            }
            else if (this.rdZC.Checked)
            {
                SendType = int.Parse(this.rdZC.Value);
            }

            #endregion
            if (SendType != 4)
            {
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
                if (this.calendarStartDate.SelectedDate.Value.Date.CompareTo(this.calendarEndDate.SelectedDate.Value.Date) > 0)
                {
                    this.ShowMsg("开始日期不能晚于结束日期！", false);
                    return;
                }
               
            }
            string strOverMoney = txtOverMoney.Text;
            if (this.rdoOverMoney.Checked && !Regex.IsMatch(strOverMoney, @"^(?!0+(?:\.0+)?$)(?:[1-9]\d*|0)(?:\.\d{1,2})?$"))
            {
                this.ShowMsg(" 满足金额必须为正数，且最多只能有两位小数", false);
                return;
            }
            if (string.IsNullOrEmpty(base.Request.Form["selectProductsinfo"]) && this.rdoProduct.Checked)
            {
                this.ShowMsg("请选择商品", false);
                return;
            }
            if (!this.outBeginDate.SelectedDate.HasValue)
            {
                this.ShowMsg("请选择发劵开始日期！", false);
                return;
            }
            if (!this.outEndDate.SelectedDate.HasValue)
            {
                this.ShowMsg("请选择发劵结束日期！", false);
                return;
            }
            if (this.outBeginDate.SelectedDate.Value.Date.CompareTo(this.outEndDate.SelectedDate.Value.Date) > 0)
            {
                this.ShowMsg("发劵开始日期不能晚于发劵结束日期！", false);
                return;
            }
            if (SendType != 4)
            {
                if (this.outEndDate.SelectedDate.Value.Date.CompareTo(this.calendarEndDate.SelectedDate.Value.Date) > 0)
                {
                    this.ShowMsg("发劵结束日期不能晚于使用结束日期！", false);
                    return;
                }
            }
            string strValidity = this.txtValidity.Text;
            if (!string.IsNullOrWhiteSpace(strValidity))
            {
                if (!Regex.IsMatch(strValidity, @"^[1-9][0-9]*$"))
                {
                    this.ShowMsg(" 有效期只能是数字，必须大于O！", false);
                    return;
                }
            }
            couponInfo.Name = this.txtCouponName.Text;
            couponInfo.ClosingTime = (SendType == 4 ? Convert.ToDateTime("1/1/1753 12:00:00") : this.calendarEndDate.SelectedDate.Value.AddDays(1).AddSeconds(-1));
            couponInfo.StartTime = (SendType == 4 ? Convert.ToDateTime("1/1/1753 12:00:00") : this.calendarStartDate.SelectedDate.Value);
            couponInfo.Amount = amount;
            couponInfo.DiscountValue = discountValue;
            couponInfo.NeedPoint = needPoint;
            couponInfo.OutBeginDate = this.outBeginDate.SelectedDate.Value;
            couponInfo.OutEndDate = this.outEndDate.SelectedDate.Value;
            couponInfo.TotalQ = totalq;
            couponInfo.Remark = this.txtRemark.Text;
            if(!string.IsNullOrEmpty(txtValidity.Text.Trim()))
            {
                couponInfo.Validity = int.Parse(this.txtValidity.Text);
            }
            if (this.rdoSDisEnable.Checked)
            {
                couponInfo.Status = int.Parse(this.rdoSDisEnable.Value);
            }
            else {
                couponInfo.Status = 1;
            }
            ValidationResults validationResults = Validation.Validate<CouponInfo>(couponInfo, new string[]
			{
				"ValCoupon"
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
            string empty = string.Empty;

            int newcouponId = 0;


            couponInfo.UseType = UseType;
            couponInfo.SendType = SendType;
            couponInfo.SendTypeItem = stroverMoney;

            #region 添加优惠券
            if (this.couponId == 0)
            {
                CouponActionStatus couponActionStatus = CouponHelper.CreateCoupon(couponInfo, 0, out empty, out newcouponId);


                if (couponActionStatus == CouponActionStatus.UnknowError)
                {
                    this.ShowMsg("未知错误", false);
                }
                else
                {
                    //if (couponActionStatus == CouponActionStatus.DuplicateName)
                    //{
                    //    this.ShowMsg("已经存在相同的优惠券名称", false);
                    //    return;
                    //}
                    if (couponActionStatus == CouponActionStatus.CreateClaimCodeError)
                    {
                        this.ShowMsg("生成优惠券号码错误", false);
                        return;
                    }

                    #region 插入优惠券子表
                    switch (UseType)
                    {
                        case 0: break;
                        case 1:
                            string productselectList = base.Request.Form["selectProductsinfo"];
                            string[] array = productselectList.Split(new char[] { ',' });
                            for (int i = 0; i < array.Length; i++)
                            {
                                //string text2 = array[i];
                                //string[] array2 = text2.Split(new char[] { '|' });
                                CouponsSendTypeItem senditem = new CouponsSendTypeItem();
                                senditem.CouponId = newcouponId;
                                senditem.BindId = System.Convert.ToInt32(array[i]);
                                senditem.UserId = HiContext.Current.User.UserId;
                                CouponHelper.CreateCouponsSendTypeItem(senditem);
                            }
                            break;
                        case 2:
                            string producttypeList = base.Request.Form["ProductTypelist"];
                            string[] arrayproducttype = producttypeList.Split(new char[] { ',' });
                            for (int i = 0; i < arrayproducttype.Length; i++)
                            {

                                CouponsSendTypeItem senditem = new CouponsSendTypeItem();
                                senditem.CouponId = newcouponId;
                                senditem.BindId = System.Convert.ToInt32(arrayproducttype[i]);
                                senditem.UserId = HiContext.Current.User.UserId;
                                CouponHelper.CreateCouponsSendTypeItem(senditem);
                            }
                            break;
                        case 3:
                            foreach (var item in dcBrand)
                            {
                                CouponsSendTypeItem senditem = new CouponsSendTypeItem();
                                senditem.CouponId = newcouponId;
                                senditem.BindId = item.Key;
                                senditem.UserId = HiContext.Current.User.UserId;
                                CouponHelper.CreateCouponsSendTypeItem(senditem);
                            }
                            break;
                        case 4:
                            foreach (var item in dcSupplier)
                            {
                                CouponsSendTypeItem senditem = new CouponsSendTypeItem();
                                senditem.CouponId = newcouponId;
                                senditem.BindId = item.Key;
                                senditem.UserId = HiContext.Current.User.UserId;
                                CouponHelper.CreateCouponsSendTypeItem(senditem);
                            }
                            break;
                        default: break;
                    }
                    #endregion

                    this.ShowMsg("添加优惠券成功", true);
                    this.RestCoupon();
                    return;

                }
            }

            #endregion 

            #region 编辑优惠券

            else
            {
                couponInfo.CouponId = this.couponId;
                CouponActionStatus couponActionStatus = CouponHelper.NewUpdateCoupon(couponInfo);


                if (couponActionStatus == CouponActionStatus.UnknowError)
                {
                    this.ShowMsg("未知错误", false);
                }
                else
                {
                    if (couponActionStatus == CouponActionStatus.DuplicateName)
                    {
                        this.ShowMsg("已经存在相同的优惠券名称", false);
                        return;
                    }
                    if (couponActionStatus == CouponActionStatus.CreateClaimCodeError)
                    {
                        this.ShowMsg("生成优惠券号码错误", false);
                        return;
                    }

                    //删除原来的子表信息
                    CouponHelper.DeleteCouponSendTypeItem(this.couponId);
                    #region 插入优惠券子表
                    switch (UseType)
                    {
                        case 0: break;
                        case 1:
                            string productselectList = base.Request.Form["selectProductsinfo"];
                            string[] array = productselectList.Split(new char[] { ',' });
                            for (int i = 0; i < array.Length; i++)
                            {
                                //string text2 = array[i];
                                //string[] array2 = text2.Split(new char[] { '|' });
                                CouponsSendTypeItem senditem = new CouponsSendTypeItem();
                                senditem.CouponId = this.couponId;
                                senditem.BindId = System.Convert.ToInt32(array[i]);
                                senditem.UserId = HiContext.Current.User.UserId;
                                CouponHelper.CreateCouponsSendTypeItem(senditem);
                            }
                            break;
                        case 2:
                            string producttypeList = base.Request.Form["ProductTypelist"];
                            string[] arrayproducttype = producttypeList.Split(new char[] { ',' });
                            for (int i = 0; i < arrayproducttype.Length; i++)
                            {

                                CouponsSendTypeItem senditem = new CouponsSendTypeItem();
                                senditem.CouponId = this.couponId;
                                senditem.BindId = System.Convert.ToInt32(arrayproducttype[i]);
                                senditem.UserId = HiContext.Current.User.UserId;
                                CouponHelper.CreateCouponsSendTypeItem(senditem);
                            }
                            break;
                        case 3:
                            foreach (var item in dcBrand)
                            {
                                CouponsSendTypeItem senditem = new CouponsSendTypeItem();
                                senditem.CouponId = this.couponId;
                                senditem.BindId = item.Key;
                                senditem.UserId = HiContext.Current.User.UserId;
                                CouponHelper.CreateCouponsSendTypeItem(senditem);
                            }
                            break;
                        case 4:
                            foreach (var item in dcSupplier)
                            {
                                CouponsSendTypeItem senditem = new CouponsSendTypeItem();
                                senditem.CouponId = this.couponId;
                                senditem.BindId = item.Key;
                                senditem.UserId = HiContext.Current.User.UserId;
                                CouponHelper.CreateCouponsSendTypeItem(senditem);
                            }
                            break;
                        default: break;
                    }
                    #endregion

                    this.ShowMsg("编辑优惠券成功", true);
                    this.RestCoupon();
                    return;

                }
            }
            #endregion 
        }
        private void RestCoupon()
        {
            this.txtCouponName.Text = string.Empty;
            this.txtAmount.Text = string.Empty;
            this.txtDiscountValue.Text = string.Empty;
        }
        private bool ValidateValues(out decimal? amount, out decimal discount, out int needPoint,out int totalq)
        {
            string text ="";
            amount = 0;

            if (!string.IsNullOrEmpty(this.txtAmount.Text.Trim()) && this.rdorefund.Checked==false)
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
            if (!decimal.TryParse(this.txtDiscountValue.Text.Trim(), out discount) && this.rdorefund.Value != "4")
            {
                text += Formatter.FormatErrorMessage("可抵扣金额必须在0.01-1000万之间");
            }
           
            if (!int.TryParse(this.txtNeedPoint.Text.Trim(), out needPoint))
            {
                text += Formatter.FormatErrorMessage("兑换所需积分不能为空，大小0-10000之间");
            }
         
            if (!int.TryParse(this.txtTotalQ.Text.Trim(), out totalq))
            {
                text += Formatter.FormatErrorMessage("总数量必须为数字或0，0表示不限制发送数量");
            }
            if (!string.IsNullOrEmpty(text))
            {
                this.ShowMsg(text, false);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 选择添加供货商
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ddcSupplier_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            dcSupplier.Clear();
            StringBuilder sb = new StringBuilder();
            int i = 0;
            foreach (ListItem item in (sender as ListControl).Items)
            {
                if (item.Selected)
                {
                    //selectedItemsPanel.Controls.Add(new Literal() { Text = item.Text+" " });
                    sb.Append(item.Text);
                    sb.Append(" ");
                    i++;
                    dcSupplier.Add(int.Parse(item.Value), item.Text);
                }
            }
            if (i > 0)
            {
                ddcSupplier.Texts.SelectBoxCaption = sb.ToString();
            }
        }

        /// <summary>
        /// 绑定供货商
        /// </summary>
        private void BindSupplier()
        {

            DataTable supplierdt = SupplierHelper.GetSupplier();
            ddcSupplier.DataTextField = "SupplierName";

            ddcSupplier.DataValueField = "SupplierId";

            ddcSupplier.DataSource = supplierdt;

            ddcSupplier.DataBind();


        }

        /// <summary>
        /// 选择添加品牌
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ddcBrand_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            dcBrand.Clear();
            StringBuilder sb = new StringBuilder();
            int i = 0;
            foreach (ListItem item in (sender as ListControl).Items)
            {
                if (item.Selected)
                {
                    //selectedItemsPanel.Controls.Add(new Literal() { Text = item.Text+" " });
                    sb.Append(item.Text);
                    sb.Append(" ");
                    i++;
                    dcBrand.Add(int.Parse(item.Value), item.Text);
                }
            }

            if (i > 0)
            {
                ddcBrand.Texts.SelectBoxCaption = sb.ToString();
            }
        }

        private void setSelectBrand(IList<CouponsSendTypeItem> listSendTypeItem)
        {
            dcBrand.Clear();
            StringBuilder sb = new StringBuilder();
            int i = 0;
            foreach(ListItem item in ddcBrand.Items)
            {
                foreach(CouponsSendTypeItem sendTypeItem in listSendTypeItem)
                {
                    if(item.Value==sendTypeItem.BindId.ToString())
                    {
                         item.Selected = true;
                         i++;
                         sb.Append(item.Text);
                         sb.Append(" ");
                         dcBrand.Add(int.Parse(item.Value), item.Text);
                        break;
                    }

                }
               
            }

            if (i > 0)
            {
                ddcBrand.Texts.SelectBoxCaption = sb.ToString();
            }
        }

        private void setSelectSupplier(IList<CouponsSendTypeItem> listSendTypeItem)
        {
            dcSupplier.Clear();
            StringBuilder sb = new StringBuilder();
            int i = 0;
            foreach (ListItem item in ddcSupplier.Items)
            {
                foreach (CouponsSendTypeItem sendTypeItem in listSendTypeItem)
                {
                    if (item.Value == sendTypeItem.BindId.ToString())
                    {
                        item.Selected = true;
                        i++;
                        sb.Append(item.Text);
                        sb.Append(" ");
                        dcSupplier.Add(int.Parse(item.Value), item.Text);
                        break;
                    }

                }

            }

            if (i > 0)
            {
                ddcSupplier.Texts.SelectBoxCaption = sb.ToString();
            }

        }

        /// <summary>
        /// 绑定品牌
        /// </summary>
        private void BindBrand()
        {
            DataTable brandCategories = ControlProvider.Instance().GetBrandCategories();
            ddcBrand.DataTextField = "BrandName";

            ddcBrand.DataValueField = "BrandId";

            ddcBrand.DataSource = brandCategories;


            ddcBrand.DataBind();
        }
    }
}
