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
                this.ddlpayment.SelectedValue = 16;//Ĭ��Ϊ����֧��
            }
        }

        private void btnAdd_Click(object sender, System.EventArgs e)
        {
            #region ��ȡ��Ʒ�����Ϣ,���칺�ﳵ��Ϣ
            //ʵ�������ﳵ
            ShoppingCartInfo shoppingCartInfo = new ShoppingCartInfo();
            
            //ʵ����SKU��Ϣ
            SkuItemInfo skuItemInfo = new SkuItemInfo();

            List<SkuInfo> skuInfoList = new List<SkuInfo>();
            List<ShoppingCartItemInfo> itemInfo = new List<ShoppingCartItemInfo>();

            //����Id
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
                //ѭ������
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

            //�ջ���ַID
            int shippingId = 0;
            int.TryParse(txtShippingId.Text, out shippingId);
            //���֤����
            string identityCard = txtIdentityCard.Text.Trim();
            //�û�ID
            int userId = 0;
            int.TryParse(txtUserId.Text, out userId);
            //�ֻ����� 
            string cellPhone = txtCellPhone.Text.Trim();
            //�ջ�������
            string shipTo = txtShipTo.Text.Trim();
            //��ַ
            string address = txtDetailsAddress.Text.Trim();
            string addressproc = "";
            if (!string.IsNullOrEmpty(dropRegions.SelectedRegions))
            {
                addressproc = (dropRegions.SelectedRegions).Replace("��", "") + address;
            }
            //��������
            string zipcode = txtZipcode.Text.Trim();
            //�绰����
            string telPhone = txtTelPhone.Text.Trim();
            //�û���
            string username = txtUserName.Text.Trim();

            #region �������޸��ջ���ַ��Ϣ
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

            //���û��ѡ���ջ��ַ����ô����һ���û����ֻ�����Ϊ�û���������Ϊ�ֻ���
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
                    ErrorLog.Write("�û����ظ����û���Ϊ��" + member.Username);
                }
                if (createUserStatus == CreateUserStatus.DuplicateEmailAddress)
                {
                    ErrorLog.Write("�ʼ����ظ�������Ϊ��" + member.Email);
                }
                if (createUserStatus == CreateUserStatus.Created)
                {
                    newUserId = member.UserId; 
                    userId = member.UserId; 
                }
            }

            int newShippingId = 0;

            //�������޸��ջ���ַ��Ϣ 
            int addressResult = MemberProcessor.AddOrUpdateShippingAddress(shippingAddress, newUserId, out newShippingId);
            if (addressResult > 0)
            {
                ErrorLog.Write("�������޸��ջ���ַ��Ϣ�ɹ����ջ��ַId[newShippingId]" + newShippingId);
            }

            #endregion


            itemInfo.ForEach(a =>
            {
                //�ŵ�IdΪ��ǰ��¼�û���Id
                a.StoreId = HiContext.Current.User.UserId;
                a.UserId = userId;
                shoppingCartInfo.LineItems.Add(a);
            });

            //��ȡ������Ϣ
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
                //վ��Ĭ��Ϊ����
                orderInfo.SiteId = 0;
                orderInfo.IdentityCard = identityCard;
                orderInfo.OrderStatus = OrderStatus.WaitBuyerPay;
                orderInfo.RefundStatus = RefundStatus.None;
                orderInfo.ShipToDate = DateTime.Now.ToString();
                //�ŵ�Id
                int userStoreId = ManagerHelper.GetStoreIdByUserId(HiContext.Current.User.UserId);
                orderInfo.StoreId = userStoreId;

                if (!string.IsNullOrEmpty(txtDeductible.Text.Trim()))
                {
                    orderInfo.Deductible = Decimal.Parse(txtDeductible.Text.Trim());
                }
                

                if (shippingAddress != null)
                {
                    #region ��֤ÿ��ÿ���������1000Ԫ��1000Ԫ����Ϊ������Ʒ
                    int mayCount = 0;
                    foreach (ShoppingCartItemInfo item in shoppingCartInfo.LineItems)
                    {
                        mayCount += item.Quantity;
                        //#region ��֤���
                        int stock = ShoppingProcessor.GetProductStock(item.SkuId);
                        if (stock <= 0 || stock < item.Quantity)
                        {
                            this.ShowMsg("��Ʒ��治��!��Ʒ����Ϊ��" + item.Name, false);
                            return;
                        }
                    }
                    #endregion

                    #region ��֤�Ƿ�����������
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
                            this.ShowMsg("����Ҫ��ص���Ʒ�����֤���벻��Ϊ�գ��뵽��������������д�����Ϣ!", false);
                            return;
                        }
                    }
                    else
                    {
                        orderInfo.IsCustomsClearance = 0;
                    }
                    #endregion

                    orderInfo.ShippingRegion = RegionHelper.GetFullRegion(regionId, "��");
                    orderInfo.RegionId = regionId;
                    orderInfo.Address = address;
                    orderInfo.ZipCode = zipcode;
                    orderInfo.ShipTo = shipTo;
                    orderInfo.TelPhone = telPhone;
                    orderInfo.CellPhone = cellPhone;

                    orderInfo.ShippingId = shippingId <= 0 ? newShippingId : shippingId;
                }

                //���ͷ�ʽ
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
                    //�˷���Ҫ��ȥ�Ż�
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
                    orderInfo.PaymentType = "����֧��";
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
                        //this.ShowMsg("����ŵ궩���ɹ�", true);
                        string url = Globals.GetAdminAbsolutePath("/sales/StoreManageOrder.aspx");
                        //���ǰ�˵�cookie
                        ClientScript.RegisterStartupScript(ClientScript.GetType(), "myscript", "<script>ClearsCookie();alert('����ŵ궩���ɹ�');window.location.href='" + url + "';</script>");
                        //base.Response.Redirect(Globals.GetAdminAbsolutePath("'/sales/StoreManageOrder.aspx'"), true);
                    }
                    else
                    {
                        this.ShowMsg("����ŵ궩��ʧ��!", false);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    ErrorLog.Write("��̨�ɹ�����ŵ궩������", Newtonsoft.Json.JsonConvert.SerializeObject(orderInfo), ex);
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
        /// ����
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
