using EcShop.ControlPanel.Commodities;
using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Entities.Commodities;
using EcShop.Entities.Store;
using EcShop.Membership.Context;
using EcShop.UI.Common.Controls;
using Ecdev.Components.Validation;
using kindeditor.Net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using EcShop.UI.ControlPanel.Utility;
using ASPNET.WebControls;
using EcShop.Entities;
using System.Data;
using EcShop.Entities.Sales;
using EcShop.SaleSystem.Member;
using System.Security.Cryptography;
using System.Text;
using EcShop.Membership.Core;
using EcShop.Entities.Orders;
using EcShop.SaleSystem.Shopping;
using EcShop.Entities.Promotions;
using System.Linq;
using System.Web;
using EcShop.Core.ErrorLog;
using EcShop.Membership.Core.Enums;
using System.Web.Security;
using EcShop.ControlPanel.Sales;

namespace EcShop.UI.Web.Admin.sales
{
    [PrivilegeCheck(Privilege.StoreOrderAdd)]
    public class AddStoreOrder : AdminPage
    {
        protected System.Web.UI.WebControls.Button btnAdd;
        protected ShippingModeDropDownList ddlshippingMode;
        protected PaymentDropDownList ddlpayment;
        protected System.Web.UI.WebControls.Label lblProductTotalPrice;
        protected System.Web.UI.WebControls.Label lblToalFreight;
        protected System.Web.UI.WebControls.Label lblTotalTax;
        protected System.Web.UI.WebControls.TextBox txtDeductible;
        protected System.Web.UI.WebControls.TextBox txtShippingId;
        protected System.Web.UI.WebControls.TextBox txtShipTo;
        protected RegionSelector dropRegions;
        protected System.Web.UI.WebControls.TextBox txtDetailsAddress;
        protected System.Web.UI.WebControls.TextBox txtZipcode;
        protected System.Web.UI.WebControls.TextBox txtTelPhone;
        protected System.Web.UI.WebControls.TextBox txtCellPhone;
        protected System.Web.UI.WebControls.TextBox txtIdentityCard;
        protected System.Web.UI.WebControls.TextBox txtBak;
        protected System.Web.UI.WebControls.TextBox txtUserId;
        protected System.Web.UI.WebControls.TextBox txtUserName;
        protected System.Web.UI.WebControls.TextBox txtRegionId;
        protected System.Web.UI.WebControls.HiddenField hiddenSkus;

        protected override void OnInitComplete(System.EventArgs e)
        {
            base.OnInitComplete(e);
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
        }
        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.ddlpayment.DataBind();
                this.ddlshippingMode.DataBind();
                this.ddlpayment.SelectedValue = 16;//默认为线下支付
            }
        }

        private void btnAdd_Click(object sender, System.EventArgs e)
        {
            #region 获取商品规格信息,构造购物车信息
            //实例化购物车
            ShoppingCartInfo shoppingCartInfo = new ShoppingCartInfo();
            
            //实例化SKU信息
            SkuItemInfo skuItemInfo = new SkuItemInfo();

            List<SkuInfo> skuInfoList = new List<SkuInfo>();
            List<ShoppingCartItemInfo> itemInfo = new List<ShoppingCartItemInfo>();

            //区县Id
            int regionId = 0;
            int.TryParse(txtRegionId.Text, out regionId);
            if (regionId == 0 && dropRegions.GetSelectedRegionId() != null && this.dropRegions.GetSelectedRegionId().Value > 0)
            {
                regionId = this.dropRegions.GetSelectedRegionId().Value;
            }

            string skuList = !string.IsNullOrWhiteSpace(hiddenSkus.Value) ? hiddenSkus.Value : "";

            if (!string.IsNullOrEmpty(skuList))
            {
                skuItemInfo.skuInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<SkuInfo[]>(skuList);
            }

            if (skuItemInfo!= null && skuItemInfo.skuInfo != null && skuItemInfo.skuInfo.Count() > 0)
            {
                skuInfoList = skuItemInfo.skuInfo.OrderByDescending(a => a.SkuId).ToList();
            }
            string skuIdStr = "";

            if (skuInfoList.Count > 0)
            {
                skuInfoList.ForEach(a => { skuIdStr += "'" + a.SkuId + "'" + ","; });
            }

            if (!string.IsNullOrEmpty(skuIdStr) && skuIdStr.Length > 0)
            {
                skuIdStr = skuIdStr.Substring(0, skuIdStr.Length - 1);
                skuIdStr = "(" + skuIdStr + ")";

                itemInfo = ShoppingProcessor.GetSkuList(skuIdStr).ToList();
            }

            if (itemInfo.Count > 0)
            {
                //循环处理
                itemInfo.ForEach(a =>
                {
                    skuInfoList.ForEach(b =>
                    {
                        if (b.SkuId == a.SkuId)
                        {
                            a.Quantity = b.BuyQty;
                            a.ShippQuantity = b.BuyQty;
                        }
                    });
                });
            }
            #endregion

            //收货地址ID
            int shippingId = 0;
            int.TryParse(txtShippingId.Text, out shippingId);
            //身份证号码
            string identityCard = txtIdentityCard.Text.Trim();
            //用户ID
            int userId = 0;
            int.TryParse(txtUserId.Text, out userId);
            //手机号码 
            string cellPhone = txtCellPhone.Text.Trim();
            //收货人姓名
            string shipTo = txtShipTo.Text.Trim();
            //地址
            string address = txtDetailsAddress.Text.Trim();
            string addressproc = "";
            if (!string.IsNullOrEmpty(dropRegions.SelectedRegions))
            {
                addressproc = (dropRegions.SelectedRegions).Replace("，", "") + address;
            }
            //邮政编码
            string zipcode = txtZipcode.Text.Trim();
            //电话号码
            string telPhone = txtTelPhone.Text.Trim();
            //用户名
            string username = txtUserName.Text.Trim();

            #region 新增或修改收货地址信息
            ShippingAddressInfo shippingAddress = new ShippingAddressInfo()
            {
                ShipTo = shipTo,
                Address = addressproc,
                Zipcode = zipcode,
                TelPhone = telPhone,
                CellPhone = cellPhone,
                IdentityCard = identityCard,
                IsDefault = true,
                RegionId = regionId,
                UserId = userId,
                ShippingId = shippingId
            };

            //如果没有选择收获地址，那么新增一个用户，手机号作为用户名，密码为手机号
            int newUserId = 0;
            if (userId == 0)
            {
                Member member = new Member(UserRole.Member);
                member.GradeId = MemberProcessor.GetDefaultMemberGrade();
                member.SessionId = Globals.GetGenerateId();
                member.Username = cellPhone;
                member.Email = "";// cellPhone + "@mail.haimylife.com";
                member.Password = cellPhone;
                member.PasswordFormat = MembershipPasswordFormat.Hashed;
                member.TradePasswordFormat = MembershipPasswordFormat.Hashed;
                member.TradePassword = cellPhone;
                member.IsApproved = true;
                member.RealName = string.Empty;
                member.Address = string.Empty;
                CreateUserStatus createUserStatus = MemberProcessor.CreateMember(member);
                if (createUserStatus == CreateUserStatus.DuplicateUsername || createUserStatus == CreateUserStatus.DisallowedUsername)
                {
                    ErrorLog.Write("用户名重复，用户名为：" + member.Username);
                }
                if (createUserStatus == CreateUserStatus.DuplicateEmailAddress)
                {
                    ErrorLog.Write("邮件名重复，邮箱为：" + member.Email);
                }
                if (createUserStatus == CreateUserStatus.Created)
                {
                    newUserId = member.UserId; 
                    userId = member.UserId; 
                }
            }

            int newShippingId = 0;

            //新增或修改收货地址信息 
            int addressResult = MemberProcessor.AddOrUpdateShippingAddress(shippingAddress, newUserId, out newShippingId);
            if (addressResult > 0)
            {
                ErrorLog.Write("新增或修改收货地址信息成功，收获地址Id[newShippingId]" + newShippingId);
            }

            #endregion


            itemInfo.ForEach(a =>
            {
                //门店Id为当前登录用户的Id
                a.StoreId = HiContext.Current.User.UserId;
                a.UserId = userId;
                shoppingCartInfo.LineItems.Add(a);
            });

            //获取订单信息
            OrderInfo orderInfo = ShoppingProcessor.ConvertShoppingCartToOrder(shoppingCartInfo, false, false, false, userId);
            if (orderInfo != null)
            {
                orderInfo.OrderId = this.GenerateOrderId();
                orderInfo.OrderDate = System.DateTime.Now;
                orderInfo.UserId = userId;

                if (!string.IsNullOrEmpty(username))
                {
                    orderInfo.Username = username;
                }
                else
                {
                    orderInfo.Username = cellPhone;
                }
                orderInfo.EmailAddress = "";
                orderInfo.RealName = "";
                orderInfo.QQ = "";
                orderInfo.Remark = txtBak.Text.Trim();
                //站点默认为深圳
                orderInfo.SiteId = 0;
                orderInfo.IdentityCard = identityCard;
                orderInfo.OrderStatus = OrderStatus.WaitBuyerPay;
                orderInfo.RefundStatus = RefundStatus.None;
                orderInfo.ShipToDate = DateTime.Now.ToString();
                //门店Id
                int userStoreId = ManagerHelper.GetStoreIdByUserId(HiContext.Current.User.UserId);
                orderInfo.StoreId = userStoreId;

                if (!string.IsNullOrEmpty(txtDeductible.Text.Trim()))
                {
                    orderInfo.Deductible = Decimal.Parse(txtDeductible.Text.Trim());
                }
                

                if (shippingAddress != null)
                {
                    #region 验证每人每日最多消费1000元，1000元以上为单件商品
                    int mayCount = 0;
                    foreach (ShoppingCartItemInfo item in shoppingCartInfo.LineItems)
                    {
                        mayCount += item.Quantity;
                        //#region 验证库存
                        int stock = ShoppingProcessor.GetProductStock(item.SkuId);
                        if (stock <= 0 || stock < item.Quantity)
                        {
                            this.ShowMsg("商品库存不足!商品名称为：" + item.Name, false);
                            return;
                        }
                    }
                    #endregion

                    #region 验证是否符合清关条件
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < shoppingCartInfo.LineItems.Count; i++)
                    {
                        if (i == (shoppingCartInfo.LineItems.Count - 1))
                        {
                            sb.Append(shoppingCartInfo.LineItems[i].ProductId);
                        }
                        else
                        {
                            sb.AppendFormat("{0},", shoppingCartInfo.LineItems[i].ProductId);
                        }
                    }
                    bool b = ShoppingProcessor.CheckIsCustomsClearance(sb.ToString());
                    if (b)
                    {
                        orderInfo.IsCustomsClearance = 1;
                        if (string.IsNullOrEmpty(identityCard))
                        {
                            Member memberNew = Users.GetUser(HiContext.Current.User.UserId, false) as Member;
                            identityCard = memberNew.IdentityCard;
                        }
                        if (string.IsNullOrEmpty(identityCard))
                        {
                            this.ShowMsg("有需要清关的商品，身份证号码不能为空，请到个人资料里面填写身份信息!", false);
                            return;
                        }
                    }
                    else
                    {
                        orderInfo.IsCustomsClearance = 0;
                    }
                    #endregion

                    orderInfo.ShippingRegion = RegionHelper.GetFullRegion(regionId, "，");
                    orderInfo.RegionId = regionId;
                    orderInfo.Address = address;
                    orderInfo.ZipCode = zipcode;
                    orderInfo.ShipTo = shipTo;
                    orderInfo.TelPhone = telPhone;
                    orderInfo.CellPhone = cellPhone;

                    orderInfo.ShippingId = shippingId <= 0 ? newShippingId : shippingId;
                }

                //配送方式
                orderInfo.ShippingModeId = ddlshippingMode.SelectedValue > 0 ? (int)ddlshippingMode.SelectedValue : 0;
                orderInfo.ModeName = !string.IsNullOrEmpty(ddlshippingMode.SelectedItem.Text) ? ddlshippingMode.SelectedItem.Text : "";
                decimal tax = 0m;
                decimal freight = 0m;

                Dictionary<int, decimal> dictShippingMode = new Dictionary<int, decimal>();
                if (shoppingCartInfo.LineItems.Count != shoppingCartInfo.LineItems.Count((ShoppingCartItemInfo a) => a.IsfreeShipping) && !shoppingCartInfo.IsFreightFree)
                {          
                    foreach (ShoppingCartItemInfo item in shoppingCartInfo.LineItems)
                    {
                        tax += item.AdjustedPrice * item.TaxRate * item.Quantity;
                        if ((!item.IsfreeShipping))
                        {
                            if (item.TemplateId > 0)
                            {
                                if (dictShippingMode.ContainsKey(item.TemplateId))
                                {
                                    dictShippingMode[item.TemplateId] += item.Weight * item.Quantity;
                                }
                                else
                                {
                                    dictShippingMode.Add(item.TemplateId, item.Weight * item.Quantity);
                                }
                            }
                        }
                    }
                    foreach (var item in dictShippingMode)
                    {
                        ShippingModeInfo shippingMode = ShoppingProcessor.GetShippingMode(item.Key);
                        freight += ShoppingProcessor.CalcFreight(shippingAddress.RegionId, item.Value, shippingMode);
                    }
                    //运费需要减去优惠
                    orderInfo.Freight = freight;
                    orderInfo.Tax = tax <= 50 ? 0 : tax;
                    orderInfo.OriginalTax = tax;
                }
                else
                {
                    orderInfo.Freight = 0m;
                }
                orderInfo.AdjustedFreight = orderInfo.Freight;

                int num = ddlpayment.SelectedValue >0 ? (int)ddlpayment.SelectedValue:0;
                orderInfo.PaymentTypeId = num;
                if (num == 16)
                {
                    orderInfo.PaymentType = "线下支付";
                    orderInfo.Gateway = "Ecdev.plugins.payment.bankrequest";
                }
                else
                {
                    PaymentModeInfo paymentMode = ShoppingProcessor.GetPaymentMode(num);
                    if (paymentMode != null)
                    {
                        orderInfo.PaymentTypeId = paymentMode.ModeId;
                        orderInfo.PaymentType = paymentMode.Name;
                        orderInfo.Gateway = paymentMode.Gateway;
                    }
                }

                orderInfo.OrderSource = OrderSource.storeAdd;

                try
                {
                    orderInfo.OrderType = (int)OrderType.Normal;

                    if (ShoppingProcessor.CreateOrder(orderInfo, true,true))
                    {

                        Reset();
                        //this.ShowMsg("添加门店订单成功", true);
                        string url = Globals.GetAdminAbsolutePath("/sales/StoreManageOrder.aspx");
                        //清除前端的cookie
                        ClientScript.RegisterStartupScript(ClientScript.GetType(), "myscript", "<script>ClearsCookie();alert('添加门店订单成功');window.location.href='" + url + "';</script>");
                        //base.Response.Redirect(Globals.GetAdminAbsolutePath("'/sales/StoreManageOrder.aspx'"), true);
                    }
                    else
                    {
                        this.ShowMsg("添加门店订单失败!", false);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    ErrorLog.Write("后台成功添加门店订单错误：", Newtonsoft.Json.JsonConvert.SerializeObject(orderInfo), ex);
                }
            } 
        }

        private string GenerateOrderId()
        {
            string text = string.Empty;
            System.Random random = new System.Random();
            for (int i = 0; i < 7; i++)
            {
                int num = random.Next();
                text += ((char)(48 + (ushort)(num % 10))).ToString();
            }
            return System.DateTime.Now.ToString("yyyyMMdd") + text;
        }


        /// <summary>
        /// 重置
        /// </summary>
        private void Reset()
        {
            this.dropRegions.SetSelectedRegionId(null);
            this.txtShippingId.Text = string.Empty;
            this.txtShipTo.Text = string.Empty;
            this.txtDetailsAddress.Text = string.Empty;
            this.txtZipcode.Text = string.Empty;
            this.txtDeductible.Text = string.Empty;
            this.txtTelPhone.Text = string.Empty;
            this.txtCellPhone.Text = string.Empty;
            this.txtIdentityCard.Text = string.Empty;
            this.txtUserId.Text = string.Empty;
            this.txtRegionId.Text = string.Empty;
            this.hiddenSkus.Value = string.Empty;
            this.txtBak.Text = string.Empty;
            this.txtDeductible.Text = string.Empty;
            this.lblTotalTax.Text = string.Empty;
            this.lblToalFreight.Text = string.Empty;
            this.lblProductTotalPrice.Text = string.Empty;
            this.ddlshippingMode.Text = string.Empty;
            this.ddlpayment.Text = string.Empty;
        }
    }
}
