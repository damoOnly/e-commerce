using EcShop.Core;
using EcShop.Entities;
using EcShop.Entities.Promotions;
using EcShop.Entities.Sales;
using EcShop.Membership.Context;
using EcShop.SaleSystem.Member;
using EcShop.SaleSystem.Shopping;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace EcShop.UI.SaleSystem.CodeBehind
{
    public class SubmmitOrderHandler : System.Web.IHttpHandler
    {
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        public void ProcessRequest(System.Web.HttpContext context)
        {
            try
            {
                string text = context.Request["Action"];
                string key;
                switch (key = text)
                {
                    case "GetUserShippingAddress":
                        this.GetUserShippingAddress(context);
                        break;
                    case "CalculateFreight":
                        this.CalculateFreight(context);
                        break;
                    case "ProcessorPaymentMode":
                        this.ProcessorPaymentMode(context);
                        break;
                    case "ProcessorUseCoupon":
                        this.ProcessorUseCoupon(context);
                        break;
                    case "GetRegionId":
                        this.GetUserRegionId(context);
                        break;
                    case "AddShippingAddress":
                        this.AddUserShippingAddress(context);
                        break;
                    case "UpdateShippingAddress":
                        this.UpdateShippingAddress(context);
                        break;
                    case "CalcBackAddOrderFreight":
                        this.CalcBackAddOrderFreight(context);
                        break;

                    case "ProcessorUseVoucherBySelect":
                        this.ProcessorUseVoucherBySelect(context);
                        break;

                    case "ProcessorUseVoucherByCode":
                        this.ProcessorUseVoucherByCode(context);
                        break;

                    case "SetDefaultShippingAddress":
                        this.SetDefaultShippingAddress(context);
                        break;

                    case "DeleteShippingAddress":
                        this.DeleteShippingAddress(context);
                        break;
                }
            }
            catch
            {
            }
        }
        private void GetUserRegionId(System.Web.HttpContext context)
        {
            string text = context.Request["Prov"];
            string text2 = context.Request["City"];
            string text3 = context.Request["Areas"];
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            stringBuilder.Append("{");
            if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(text2) && !string.IsNullOrEmpty(text3))
            {
                stringBuilder.Append("\"Status\":\"OK\",\"RegionId\":\"" + RegionHelper.GetRegionId(text3, text2, text) + "\"}");
            }
            else
            {
                stringBuilder.Append("\"Status\":\"NOK\"}");
            }
            context.Response.ContentType = "text/plain";
            context.Response.Write(stringBuilder);
        }
        private void GetUserShippingAddress(System.Web.HttpContext context)
        {
            ShippingAddressInfo shippingAddress = MemberProcessor.GetShippingAddress(int.Parse(context.Request["ShippingId"], System.Globalization.NumberStyles.None));
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            string strRegion1=string.Empty;
            string strRegion2=string.Empty;
            string strRegion3=string.Empty;
            if(shippingAddress.RegionId>0)
            {
               string[] regions= RegionHelper.GetFullPath(shippingAddress.RegionId).Split(',');
                if(regions.Length==3)
                {
                    strRegion1 = regions[0];
                    strRegion2 = regions[1];
                    strRegion3 = regions[2];
                }
            }
            stringBuilder.Append("{");
            if (shippingAddress != null)
            {
                stringBuilder.Append("\"Status\":\"OK\",");
                stringBuilder.AppendFormat("\"ShipTo\":\"{0}\",", Globals.HtmlDecode(shippingAddress.ShipTo));
                stringBuilder.AppendFormat("\"Address\":\"{0}\",", Globals.HtmlDecode(shippingAddress.Address));
                stringBuilder.AppendFormat("\"Zipcode\":\"{0}\",", Globals.HtmlDecode(shippingAddress.Zipcode));
                stringBuilder.AppendFormat("\"CellPhone\":\"{0}\",", Globals.HtmlDecode(shippingAddress.CellPhone));
                stringBuilder.AppendFormat("\"TelPhone\":\"{0}\",", Globals.HtmlDecode(shippingAddress.TelPhone));
                stringBuilder.AppendFormat("\"IdentityCard\":\"{0}\",", Globals.HtmlDecode(shippingAddress.IdentityCard));
                stringBuilder.AppendFormat("\"RegionId\":\"{0}\",", shippingAddress.RegionId);
                stringBuilder.AppendFormat("\"ShippingId\":\"{0}\",", shippingAddress.ShippingId);
                stringBuilder.AppendFormat("\"Region1\":\"{0}\",", strRegion1);
                stringBuilder.AppendFormat("\"Region2\":\"{0}\",", strRegion2);
                stringBuilder.AppendFormat("\"Region3\":\"{0}\"", strRegion3);
            }
            else
            {
                stringBuilder.Append("\"Status\":\"0\"");
            }
            stringBuilder.Append("}");
            context.Response.ContentType = "text/plain";
            context.Response.Write(stringBuilder);
        }
        private void CalculateFreight(System.Web.HttpContext context)//海美生活 运费计算方法
        {
            decimal money = 0m;
            if (!string.IsNullOrEmpty(context.Request["RegionId"]))
            {
                //int modeId = int.Parse(context.Request["ModeId"], System.Globalization.NumberStyles.None);
                decimal num = 0m;
                decimal.TryParse(context.Request["Weight"], out num);
                //int shippingId = int.Parse(context.Request["shippingId"], System.Globalization.NumberStyles.None);

                int regionId = int.Parse(context.Request["RegionId"], System.Globalization.NumberStyles.None);
                //ShippingModeInfo shippingMode = ShoppingProcessor.GetShippingMode(modeId, true);
                int buyAmount;
                ShoppingCartInfo shoppingCart;
                if (int.TryParse(context.Request["buyAmount"], out buyAmount) && !string.IsNullOrWhiteSpace(context.Request["productSku"]))
                {
                    string productSkuId = System.Convert.ToString(context.Request["productSku"]);
                    shoppingCart = ShoppingCartProcessor.GetShoppingCart(productSkuId, buyAmount, 0);
                }
                else
                {
                    int boundlingid;
                    if (int.TryParse(context.Request["buyAmount"], out buyAmount) && int.TryParse(context.Request["Bundlingid"], out boundlingid))
                    {
                        shoppingCart = ShoppingCartProcessor.GetShoppingCart(boundlingid, buyAmount);
                    }
                    else
                    {
                        //

                        HttpCookie cookieSkuIds = context.Request.Cookies["UserSession-SkuIds"];
                        if (cookieSkuIds == null || string.IsNullOrEmpty(cookieSkuIds.Value))
                        {
                            shoppingCart = ShoppingCartProcessor.GetShoppingCart();
                        }
                        else
                        {
                            shoppingCart = ShoppingCartProcessor.GetPartShoppingCartInfo(cookieSkuIds.Value);
                        }
                    }
                }
                //  ShippingAddressInfo memberDefaultShippingAddressInfo = MemberProcessor.GetShippingAddress(shippingId);

                money = ShoppingCartProcessor.GetFreight(shoppingCart, regionId, false);
                //money = ShoppingProcessor.CalcShoppingCartFreight(shoppingCart, regionId);
                System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
                stringBuilder.Append("{");
                stringBuilder.Append("\"Status\":\"OK\",");
                stringBuilder.AppendFormat("\"Price\":\"{0}\"", Globals.FormatMoney(money));
                stringBuilder.Append("}");
                context.Response.ContentType = "text/plain";
                context.Response.Write(stringBuilder.ToString());
            }
        }

        //后台添加门店订单，计算运费
        public void CalcBackAddOrderFreight(System.Web.HttpContext context)
        {

            StringBuilder stringBuilder = new StringBuilder();

            //实例化购物车
            ShoppingCartInfo shoppingCartInfo = new ShoppingCartInfo();

            //实例化SKU信息
            SkuItemInfo skuItemInfo = new SkuItemInfo();

            List<SkuInfo> skuInfoList = new List<SkuInfo>();
            List<ShoppingCartItemInfo> itemInfo = new List<ShoppingCartItemInfo>();

            //区县Id
            int regionId;
            int.TryParse(context.Request["RegionId"], out regionId);

            string skuList = context.Request["SkuList"];

            if (string.IsNullOrEmpty(skuList))
            {
                stringBuilder.Append("{");
                stringBuilder.Append("\"Status\":\"NO\"");
                stringBuilder.Append("}");
                return;
            }

            skuItemInfo.skuInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<SkuInfo[]>(skuList);

            if (skuItemInfo == null || skuItemInfo.skuInfo == null || skuItemInfo.skuInfo.Length <= 0)
            {
                stringBuilder.Append("{");
                stringBuilder.Append("\"Status\":\"NO\"");
                stringBuilder.Append("}");
                return;
            }

            skuInfoList = skuItemInfo.skuInfo.OrderByDescending(a => a.SkuId).ToList();
            string skuIdStr = "";

            if (skuInfoList.Count > 0)
            {
                skuInfoList.ForEach(a => { skuIdStr += "'" + a.SkuId + "'" + ","; });
            }

            if (!string.IsNullOrEmpty(skuIdStr) && skuIdStr.Length > 0)
            {
                skuIdStr = skuIdStr.Substring(0, skuIdStr.Length - 1);
                skuIdStr = "(" + skuIdStr + ")";
            }

            itemInfo = ShoppingProcessor.GetSkuList(skuIdStr).ToList();

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

                itemInfo.ForEach(t =>
                {
                    //门店Id为当前登录用户的Id
                    t.StoreId = HiContext.Current.User.UserId;
                    shoppingCartInfo.LineItems.Add(t);
                });
            }

            decimal freight = 0m;
            if (shoppingCartInfo == null
                || shoppingCartInfo.LineItems.Count == 0)
            {
                freight = 0m;
            }

            Dictionary<int, decimal> dictShippingMode = new Dictionary<int, decimal>();

            if (shoppingCartInfo.LineItems.Count != shoppingCartInfo.LineItems.Count((ShoppingCartItemInfo a) => a.IsfreeShipping) && !shoppingCartInfo.IsFreightFree)
            {
                foreach (ShoppingCartItemInfo item in shoppingCartInfo.LineItems)
                {
                    if ((!shoppingCartInfo.IsFreightFree || !item.IsfreeShipping))
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

                foreach (var item in dictShippingMode)// 计算运费
                {
                    ShippingModeInfo shippingMode = ShoppingProcessor.GetShippingMode(item.Key);
                    freight += ShoppingProcessor.CalcFreight(regionId, item.Value, shippingMode);
                }
            }

            stringBuilder.Append("{");
            stringBuilder.Append("\"Status\":\"OK\",");
            stringBuilder.AppendFormat("\"Freight\":\"{0}\"", Globals.FormatMoney(freight));
            stringBuilder.Append("}");
            context.Response.ContentType = "text/plain";
            context.Response.Write(stringBuilder.ToString());
        }

        private void CalculateFreight1(System.Web.HttpContext context)//原有的计算运费方法
        {
            decimal money = 0m;
            if (!string.IsNullOrEmpty(context.Request.Params["ModeId"]) && !string.IsNullOrEmpty(context.Request["RegionId"]))
            {
                int modeId = int.Parse(context.Request["ModeId"], System.Globalization.NumberStyles.None);
                decimal num = 0m;
                decimal.TryParse(context.Request["Weight"], out num);
                int regionId = int.Parse(context.Request["RegionId"], System.Globalization.NumberStyles.None);
                ShippingModeInfo shippingMode = ShoppingProcessor.GetShippingMode(modeId, true);
                int buyAmount;
                ShoppingCartInfo shoppingCart;
                if (int.TryParse(context.Request["buyAmount"], out buyAmount) && !string.IsNullOrWhiteSpace(context.Request["productSku"]))
                {
                    string productSkuId = System.Convert.ToString(context.Request["productSku"]);
                    shoppingCart = ShoppingCartProcessor.GetShoppingCart(productSkuId, buyAmount, 0);
                }
                else
                {
                    int boundlingid;
                    if (int.TryParse(context.Request["buyAmount"], out buyAmount) && int.TryParse(context.Request["Bundlingid"], out boundlingid))
                    {
                        shoppingCart = ShoppingCartProcessor.GetShoppingCart(boundlingid, buyAmount);
                    }
                    else
                    {
                        shoppingCart = ShoppingCartProcessor.GetShoppingCart();
                    }
                }
                if (!shoppingCart.IsFreightFree)
                {
                    if (shoppingCart.LineItems.Count((ShoppingCartItemInfo a) => !a.IsfreeShipping) > 0 || shoppingCart.LineGifts.Count > 0)
                    {
                        money = ShoppingProcessor.CalcFreight(regionId, shoppingCart.Weight, shippingMode);
                    }
                }
            }
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            stringBuilder.Append("{");
            stringBuilder.Append("\"Status\":\"OK\",");
            stringBuilder.AppendFormat("\"Price\":\"{0}\"", Globals.FormatMoney(money));
            stringBuilder.Append("}");
            context.Response.ContentType = "text/plain";
            context.Response.Write(stringBuilder.ToString());
        }
        private void ProcessorPaymentMode(System.Web.HttpContext context)
        {
            decimal money = 0m;
            if (!string.IsNullOrEmpty(context.Request.Params["ModeId"]))
            {
                int modeId = int.Parse(context.Request["ModeId"], System.Globalization.NumberStyles.None);
                decimal cartMoney = decimal.Parse(context.Request["TotalPrice"]);
                PaymentModeInfo paymentMode = ShoppingProcessor.GetPaymentMode(modeId);
                money = paymentMode.CalcPayCharge(cartMoney);
            }
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            stringBuilder.Append("{");
            stringBuilder.Append("\"Status\":\"OK\",");
            stringBuilder.AppendFormat("\"Charge\":\"{0}\"", Globals.FormatMoney(money));
            stringBuilder.Append("}");
            context.Response.ContentType = "text/plain";
            context.Response.Write(stringBuilder.ToString());
        }
        private void ProcessorUseCoupon(System.Web.HttpContext context)
        {
            decimal orderAmount = decimal.Parse(context.Request["CartTotal"]);
            string claimCode = context.Request["CouponCode"];
            CouponInfo couponInfo = ShoppingProcessor.UseCoupon(orderAmount, claimCode);
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            if (couponInfo != null)
            {
                stringBuilder.Append("{");
                stringBuilder.Append("\"Status\":\"OK\",");
                stringBuilder.AppendFormat("\"CouponName\":\"{0}\",", couponInfo.Name);
                stringBuilder.AppendFormat("\"DiscountValue\":\"{0}\"", Globals.FormatMoney(couponInfo.DiscountValue));
                stringBuilder.Append("}");
            }
            else
            {
                stringBuilder.Append("{");
                stringBuilder.Append("\"Status\":\"ERROR\"");
                stringBuilder.Append("}");
            }
            context.Response.ContentType = "application/json";
            context.Response.Write(stringBuilder);
        }
        private void AddUserShippingAddress(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string str = "";
            if (!this.ValShippingAddress(context, ref str))
            {
                context.Response.Write("{\"Status\":\"Error\",\"Result\":\"" + str + "\"}");
                return;
            }
            ShippingAddressInfo shippingAddressInfo = this.GetShippingAddressInfo(context);
            int shippingId = MemberProcessor.AddShippingAddress(shippingAddressInfo);
            if (shippingId > 0)
            {
                //System.Collections.Generic.IList<ShippingAddressInfo> shippingAddresses = MemberProcessor.GetShippingAddresses();
                //shippingAddressInfo = shippingAddresses[shippingAddresses.Count - 1];
                shippingAddressInfo.ShippingId = shippingId;
                string fullAddress = RegionHelper.GetFullRegion(shippingAddressInfo.RegionId, " ");
                StringBuilder stringBuilder = new StringBuilder();

                context.Response.Write(string.Concat(new object[]
				{
					"{\"Status\":\"OK\",\"Result\":{\"ShipTo\":\"",
					shippingAddressInfo.ShipTo,
					"\",\"RegionId\":\"",
					fullAddress,
					"\",\"ShippingAddress\":\"",
					shippingAddressInfo.Address,
					"\",\"ShippingId\":\"",
					shippingAddressInfo.ShippingId,
					"\",\"CellPhone\":\"",
					shippingAddressInfo.CellPhone,
                    "\",\"AddressList\":\"",
                    "",
					"\"}}"
				}));
                return;
            }
            context.Response.Write("{\"Status\":\"Error\",\"Result\":\"地址已经在，请重新输入一次再试\"}");
        }
        private void UpdateShippingAddress(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string str = "";
            str = "请选择要修改的收货地址";
            if (!this.ValShippingAddress(context, ref str))
            {
                context.Response.Write("{\"Status\":\"Error\",\"Result\":\"" + str + "\"}");
                return;
            }

            if (string.IsNullOrEmpty(context.Request.Params["ShippingId"]))
            {
                context.Response.Write("{\"Status\":\"Error\",\"Result\":\"" + str + "\"}");
                return;
            }

           
            if (System.Convert.ToInt32(context.Request.Params["ShippingId"]) <= 0)
            {
                context.Response.Write("{\"Status\":\"Error\",\"Result\":\"" + str + "\"}");
                return;
            }
           
            ShippingAddressInfo shippingAddressInfo = this.GetShippingAddressInfo(context);
            shippingAddressInfo.ShippingId = System.Convert.ToInt32(context.Request.Params["ShippingId"]);
            if (MemberProcessor.UpdateShippingAddress(shippingAddressInfo))
            {
                context.Response.Write(string.Concat(new object[]
				{
					"{\"Status\":\"OK\",\"Result\":{\"ShipTo\":\"",
					shippingAddressInfo.ShipTo,
					"\",\"RegionId\":\"",
					RegionHelper.GetFullRegion(shippingAddressInfo.RegionId, " "),
					"\",\"ShippingAddress\":\"",
					shippingAddressInfo.Address,
					"\",\"ShippingId\":\"",
					shippingAddressInfo.ShippingId,
					"\",\"CellPhone\":\"",
					shippingAddressInfo.CellPhone,
					"\"}}"
				}));
                return;
            }
            context.Response.Write("{\"Status\":\"Error\",\"Result\":\"地址已经在，请重新输入一次再试\"}");
        }
        private bool ValShippingAddress(System.Web.HttpContext context, ref string erromsg)
        {
            //System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex("[\\u4e00-\\u9fa5a-zA-Z]+[\\u4e00-\\u9fa5_a-zA-Z0-9]*");
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex("^[\\u4e00-\\u9fa5]{2,6}$");
            if (string.IsNullOrEmpty(context.Request.Params["ShippingTo"].Trim()) || !regex.IsMatch(context.Request.Params["ShippingTo"].Trim()))
            {
                //erromsg = "收货人名字不能为空，只能是汉字或字母开头，长度在2-20个字符之间";
                erromsg = "收货人名字只能为2-6个汉字";
                return false;
            }
            if (string.IsNullOrEmpty(context.Request.Params["AddressDetails"].Trim()))
            {
                erromsg = "详细地址不能为空";
                return false;
            }
            if (context.Request.Params["AddressDetails"].Trim().Length < 3 || context.Request.Params["AddressDetails"].Trim().Length > 60)
            {
                erromsg = "详细地址长度在3-60个字符之间";
                return false;
            }
            int cityId = 0,provinceId = 0;
            if(context.Request.Params["CityId"] != null && !string.IsNullOrEmpty(context.Request.Params["CityId"].Trim()))
            {
               cityId = Convert.ToInt32(context.Request.Params["CityId"].Trim());
            }

            if (context.Request.Params["ProvinceId"] != null && !string.IsNullOrEmpty(context.Request.Params["ProvinceId"].Trim()))
            {
                provinceId = Convert.ToInt32(context.Request.Params["ProvinceId"].Trim());
            }

            Dictionary<int, string> dic = RegionHelper.GetCitys(provinceId);
            Dictionary<int, string> dictionary = RegionHelper.GetCountys(cityId);
            if ((dic.Count > 0 && cityId <= 0) || (dictionary.Count > 0 && (string.IsNullOrEmpty(context.Request.Params["RegionId"].Trim()) || System.Convert.ToInt32(context.Request.Params["RegionId"].Trim()) <= 0)))
            {
                erromsg = "请选择收货地址";
                return false;
            }

            int regionId = Convert.ToInt32(context.Request.Params["RegionId"].Trim());
            string regionIdstr = RegionHelper.GetFullPath(regionId);

            if ((dic.Count > 0 && cityId <= 0) || (dictionary.Count > 0 && regionIdstr.Split(',').Length != 3))
            {
                erromsg = "请选择完整的地区";
                return false;
            }
            
            if (string.IsNullOrEmpty(context.Request.Params["TelPhone"].Trim()) && string.IsNullOrEmpty(context.Request.Params["CellHphone"].Trim().Trim()))
            {
                erromsg = "电话号码和手机二者必填其一";
                return false;
            }
            if (!string.IsNullOrEmpty(context.Request.Params["TelPhone"].Trim()) && (context.Request.Params["TelPhone"].Trim().Length < 3 || context.Request.Params["TelPhone"].Trim().Length > 20))
            {
                erromsg = "电话号码长度限制在3-20个字符之间";
                return false;
            }
            if (!string.IsNullOrEmpty(context.Request.Params["CellHphone"].Trim()) && (context.Request.Params["CellHphone"].Trim().Length < 3 || context.Request.Params["CellHphone"].Trim().Length > 20))
            {
                erromsg = "手机号码长度限制在3-20个字符之间";
                return false;
            }
            string patternIdentityCard = "^[1-9]{1}[0-9]{14}$|^[1-9]{1}[0-9]{16}([0-9]|[xX])$";
            System.Text.RegularExpressions.Regex regexIdentityCard = new System.Text.RegularExpressions.Regex(patternIdentityCard);
            string IdentityCard = context.Request.Params["IdentityCard"].Trim();
            //if (!string.IsNullOrEmpty(IdentityCard) && !regexIdentityCard.IsMatch(IdentityCard))
            //{
            //     erromsg="请输入正确的身份证号码";
            //    return false;
            //}
            if (MemberProcessor.GetShippingAddressCount() > HiContext.Current.Config.ShippingAddressQuantity)
            {
                erromsg = string.Format("最多只能添加{0}个收货地址", HiContext.Current.Config.ShippingAddressQuantity);
                return false;
            }
            return true;
        }
        private ShippingAddressInfo GetShippingAddressInfo(System.Web.HttpContext context)
        {
            return new ShippingAddressInfo
            {
                UserId = HiContext.Current.User.UserId,
                ShipTo = context.Request.Params["ShippingTo"].Trim(),
                RegionId = System.Convert.ToInt32(context.Request.Params["RegionId"].Trim()),
                Address = context.Request.Params["AddressDetails"].Trim(),
                Zipcode = context.Request.Params["ZipCode"].Trim(),
                CellPhone = context.Request.Params["CellHphone"].Trim(),
                TelPhone = context.Request.Params["TelPhone"].Trim(),
                IdentityCard = context.Request.Params["IdentityCard"].Trim()
            };
        }

        private void ProcessorUseVoucherBySelect(System.Web.HttpContext context)
        {
            decimal orderAmount = decimal.Parse(context.Request["CartTotal"]);
            string claimCode = context.Request["VoucherCode"];
            VoucherInfo voucherInfo = ShoppingProcessor.UseVoucher(orderAmount, claimCode);
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            if (voucherInfo != null)
            {
                stringBuilder.Append("{");
                stringBuilder.Append("\"Status\":\"OK\",");
                stringBuilder.AppendFormat("\"VoucherName\":\"{0}\",", voucherInfo.Name);
                stringBuilder.AppendFormat("\"DiscountValue\":\"{0}\"", Globals.FormatMoney(voucherInfo.DiscountValue));
                stringBuilder.Append("}");
            }
            else
            {
                stringBuilder.Append("{");
                stringBuilder.Append("\"Status\":\"ERROR\"");
                stringBuilder.Append("}");
            }
            context.Response.ContentType = "application/json";
            context.Response.Write(stringBuilder);
        }


        private void ProcessorUseVoucherByCode(System.Web.HttpContext context)
        {
            decimal orderAmount = decimal.Parse(context.Request["CartTotal"]);
            string claimCode = context.Request["VoucherCode"];
            string Password = context.Request["VoucherPassword"];
            VoucherInfo voucherInfo = ShoppingProcessor.UseVoucher(orderAmount, claimCode, Password);
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            if (voucherInfo != null)
            {
                stringBuilder.Append("{");
                stringBuilder.Append("\"Status\":\"OK\",");
                stringBuilder.AppendFormat("\"VoucherName\":\"{0}\",", voucherInfo.Name);
                stringBuilder.AppendFormat("\"DiscountValue\":\"{0}\"", Globals.FormatMoney(voucherInfo.DiscountValue));
                stringBuilder.Append("}");
            }
            else
            {
                stringBuilder.Append("{");
                stringBuilder.Append("\"Status\":\"ERROR\"");
                stringBuilder.Append("}");
            }
            context.Response.ContentType = "application/json";
            context.Response.Write(stringBuilder);
        }

        private void SetDefaultShippingAddress(System.Web.HttpContext context)
        {
            string str = "";
            context.Response.ContentType = "application/json";
            Member member = HiContext.Current.User as Member;
            if (member == null)
            {
                str = "登录失效,请重新登录";
                context.Response.Write("{\"Status\":\"Error\",\"Result\":\"" + str + "\"}");
                return;
            }
            int userId = member.UserId;
            int shippingId = System.Convert.ToInt32(context.Request.Form["ShippingId"]);
            if (EcShop.SaleSystem.Member.MemberProcessor.SetDefaultShippingAddressPC(shippingId, userId))
            {
                str = "设置成功";
                context.Response.Write("{\"Status\":\"OK\",\"Result\":\"" + str + "\"}");
            }
            else
            {
                str = "设置失败";
                context.Response.Write("{\"Status\":\"Error\",\"Result\":\"" + str + "\"}");
            }
        }

        private void DeleteShippingAddress(System.Web.HttpContext context)
        {
            string str = "";
            context.Response.ContentType = "application/json";
            Member member = HiContext.Current.User as Member;
            if (member == null)
            {
                str = "登录失效,请重新登录";
                context.Response.Write("{\"Status\":\"Error\",\"Result\":\"" + str + "\"}");
                return;
            }
            int userId = member.UserId;
            int shippingid = System.Convert.ToInt32(context.Request.Form["ShippingId"]);
            if (EcShop.SaleSystem.Member.MemberProcessor.DelShippingAddress(shippingid, userId))
            {
                str = "删除成功";
                context.Response.Write("{\"Status\":\"OK\",\"Result\":\"" + str + "\"}");
            }
            else
            {
                str = "删除失败";
                context.Response.Write("{\"Status\":\"Error\",\"Result\":\"" + str + "\"}");
            }
           
        }
    }
}
