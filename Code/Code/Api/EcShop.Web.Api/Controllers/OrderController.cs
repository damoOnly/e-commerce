using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Http;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using EcShop.Entities;
using EcShop.Entities.Sales;
using EcShop.Entities.Orders;
using EcShop.Entities.Promotions;

using EcShop.SaleSystem.Shopping;
using EcShop.SaleSystem.Catalog;
using EcShop.ControlPanel.Sales;
using EcShop.SaleSystem.Member;

using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Core.Enums;

using EcShop.Membership.Context;
using EcShop.Membership.Core;
using EcShop.Membership.Core.Enums;

using EcShop.Messages;

using EcShop.Web.Api.ApiException;

using EcShop.Web.Api.Utility;
using EcShop.Web.Api.Model;
using EcShop.Web.Api.Model.RequestJsonParams;
using EcShop.Web.Api.Model.Result;
using System.Text;

using Ecdev.Weixin.Pay;
using Ecdev.Weixin.Pay.Pay;
using System.Net;
using System.Xml;
using EcShop.ControlPanel.Commodities;
using EcShop.Core.ErrorLog;

namespace EcShop.Web.Api.Controllers
{
    public class OrderController : EcdevApiController
    {
        #region Shopping Cart

        [HttpPost]
        public IHttpActionResult AddMultiSkuToCart(JObject request)
        {
            Logger.WriterLogger("Order.AddMultiSkuToCart, Params: " + request.ToString(), LoggerType.Info);

            dynamic json = request;
            JArray jsonItems = json.Items;

            string userId = json.UserId;
            string accessToken = json.accessToken;
            int channel = json.channel;
            int platform = json.platform;
            string ver = json.ver;

            base.SaveVisitInfo(userId, channel, platform, ver);

            List<ParamBuySingle> param = new List<ParamBuySingle>();

            try
            {
                param = jsonItems.ToObject<List<ParamBuySingle>>();
            }
            catch
            {
                // 参数无效
                return base.JsonFaultResult(new CommonException(40100).GetMessage(), request.ToString());
            }

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), request.ToString());
            }

            // 验证参数
            //ThrowParamException(skuId);

            string msg = "";

            Member member = GetMember(userId.ToSeesionId());

            if (member == null)
            {
                return base.JsonFaultResult(new FaultInfo(40201, msg), request.ToString());
            }

            try
            {
                foreach (var current in param)
                {
                    string skuId = current.SkuId;
                    int productId = current.ProductId;
                    int quantity = current.Quantity;

                    ShoppingCartProcessor.AddLineItem(member, skuId, quantity, 0);
                }

            }
            catch (Exception ex)
            {
                Logger.WriterLogger(ex, LoggerType.Error);
            }

            ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetShoppingCart(member);
            if (shoppingCart != null)
            {
                StandardResult<BuyResult> result = new StandardResult<BuyResult>()
                {
                    code = 0,
                    msg = "操作成功",
                    data = new BuyResult(true, shoppingCart.GetTotal(), shoppingCart.GetQuantity())
                };

                return base.JsonActionResult(result);
            }

            return base.JsonFaultResult(new FaultInfo(40300, msg), request.ToString());
        }

        [HttpPost]
        public IHttpActionResult AddToCart(JObject request)
        {
            Logger.WriterLogger("Order.AddToCart, Params: " + request.ToString(), LoggerType.Info);

            ParamBuy param = new ParamBuy();

            try
            {
                param = request.ToObject<ParamBuy>();
            }
            catch
            {
                // 参数无效
                return base.JsonFaultResult(new CommonException(40100).GetMessage(), request.ToString());
            }

            string accessToken = param.accessToken;

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), request.ToString());
            }

            // 验证参数
            //ThrowParamException(skuId);

            string userId = param.UserId;
            string skuId = param.SkuId;
            int productId = param.ProductId;
            int quantity = param.Quantity;

            int channel = param.channel;
            int platform = param.platform;
            string ver = param.ver;

            base.SaveVisitInfo(userId, channel, platform, ver);

            string msg = "";

            Member member = GetMember(userId.ToSeesionId());

            if (member == null)
            {
                return base.JsonFaultResult(new FaultInfo(40201, msg), request.ToString());
            }

            try
            {
                #region  捆绑商品判断

                if ((platform == 2 || platform == 3) && Util.ConvertVer(ver) < 110)
                {
                    List<ProductsCombination> list = new List<ProductsCombination>();
                    list = ProductHelper.GetProductsCombinationsBySku(skuId);
                    if (list.Count > 0)
                    {
                        decimal totalprice = 0M;
                        decimal totalTax = 0M;
                        for (int i = 0; i < list.Count; i++)
                        {

                            totalprice += list[i].Price * list[i].Quantity;
                            totalTax += list[i].Price * list[i].Quantity * list[i].TaxRate;
                        }

                        if (totalprice > 0)
                        {
                            decimal taxrate = totalTax / totalprice;

                            decimal converttaxrate = Math.Round(taxrate, 2);

                            if (taxrate != converttaxrate)
                            {
                                return base.JsonActionResult(new StandardResult<string>()
                                {
                                    code = 1,
                                    msg = "存在捆绑商品,请更新最新版APP",
                                    data = null
                                });
                            }


                        }
                    }
                }


                #endregion

                #region  验证限购
                int MaxCount = 0;
                int Payquantity = ProductHelper.CheckPurchaseCount(skuId, member.UserId, out MaxCount);
                if ((Payquantity + quantity) > MaxCount && MaxCount != 0) //当前购买数量大于限购剩余购买数量
                {
                    return base.JsonActionResult(new StandardResult<string>()
                    {
                        code = 1001,
                        msg = string.Format("超过限购数,该商品最多可购买{0}件", MaxCount),
                        data = null
                    });
                }
                #endregion

                //库存判断
                int stock = ShoppingCartProcessor.GetSkuStock(skuId);
                if (stock <= 0 || stock < quantity)
                {
                    return base.JsonActionResult(new StandardResult<string>()
                    {
                        code = 1,
                        msg = "库存不足",
                        data = null
                    });
                }
                ShoppingCartProcessor.AddLineItem(member, skuId, quantity, 0);
            }
            catch (Exception ex)
            {
                Logger.WriterLogger(ex, LoggerType.Error);
            }

            try
            {
                decimal total = 0M;
                int skuQuantity = 0;

                ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetShoppingCart(member);
                if (shoppingCart != null)
                {
                    total = shoppingCart.GetTotal();
                    skuQuantity = shoppingCart.GetQuantity();
                }

                StandardResult<BuyResult> result = new StandardResult<BuyResult>()
                {
                    code = 0,
                    msg = "操作成功",
                    data = new BuyResult(true, total, skuQuantity)
                };

                return base.JsonActionResult(result);

            }
            catch (Exception ex)
            {
                Logger.WriterLogger("Order.AddToCart", ex, LoggerType.Error);
            }

            return base.JsonFaultResult(new FaultInfo(40300, msg), request.ToString());
        }

        [HttpPost]
        public IHttpActionResult UpdateCartItem(JObject request)
        {
            Logger.WriterLogger("Order.UpdateCartItem, Params: " + request.ToString(), LoggerType.Info);

            ParamBuy param = new ParamBuy();

            try
            {
                param = request.ToObject<ParamBuy>();
            }
            catch
            {
                // 参数无效
                return base.JsonFaultResult(new CommonException(40100).GetMessage(), request.ToString());
            }

            string accessToken = param.accessToken;

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), request.ToString());
            }

            // 验证参数
            //ThrowParamException(skuId);

            string userId = param.UserId;
            string skuId = param.SkuId;
            int productId = param.ProductId;
            int quantity = param.Quantity;

            int channel = param.channel;
            int platform = param.platform;
            string ver = param.ver;

            base.SaveVisitInfo(userId, channel, platform, ver);

            string msg = "";

            Member member = GetMember(userId.ToSeesionId());

            if (member == null)
            {
                return base.JsonFaultResult(new FaultInfo(40201, msg), request.ToString());
            }

            try
            {
                ShoppingCartProcessor.UpdateLineItemQuantity(member, skuId, quantity);
            }
            catch (Exception ex)
            {
                Logger.WriterLogger(ex, LoggerType.Error);
            }

            try
            {
                decimal total = 0M;
                int skuQuantity = 0;

                ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetShoppingCart(member);
                if (shoppingCart != null)
                {
                    total = shoppingCart.GetTotal();
                    skuQuantity = shoppingCart.GetQuantity();
                }

                StandardResult<BuyResult> result = new StandardResult<BuyResult>()
                {
                    code = 0,
                    msg = "操作成功",
                    data = new BuyResult(true, total, skuQuantity)
                };

                return base.JsonActionResult(result);
            }
            catch (Exception ex)
            {
                Logger.WriterLogger("Order.UpdateCartItem", ex, LoggerType.Error);
            }

            return base.JsonFaultResult(new FaultInfo(40300, msg), request.ToString());
        }

        [HttpPost]
        public IHttpActionResult RemoveCartItem(JObject request)
        {
            Logger.WriterLogger("Order.RemoveCartItem, Params: " + request.ToString(), LoggerType.Info);

            ParamBuy param = new ParamBuy();

            try
            {
                param = request.ToObject<ParamBuy>();
            }
            catch
            {
                // 参数无效
                return base.JsonFaultResult(new CommonException(40100).GetMessage(), request.ToString());
            }

            string accessToken = param.accessToken;

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), request.ToString());
            }

            // 验证参数
            //ThrowParamException(skuId);

            string userId = param.UserId;
            string skuId = param.SkuId;
            int productId = param.ProductId;
            int quantity = param.Quantity;

            int channel = param.channel;
            int platform = param.platform;
            string ver = param.ver;

            base.SaveVisitInfo(userId, channel, platform, ver);

            string msg = "";

            Member member = GetMember(userId.ToSeesionId());

            if (member == null)
            {
                return base.JsonFaultResult(new FaultInfo(40201, msg), request.ToString());
            }

            try
            {
                ShoppingCartProcessor.RemoveLineItem(member, skuId);
            }
            catch (Exception ex)
            {
                Logger.WriterLogger(ex, LoggerType.Error);
            }

            try
            {
                decimal total = 0M;
                int skuQuantity = 0;

                ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetShoppingCart(member);
                if (shoppingCart != null)
                {
                    total = shoppingCart.GetTotal();
                    skuQuantity = shoppingCart.GetQuantity();
                }

                StandardResult<BuyResult> result = new StandardResult<BuyResult>()
                {
                    code = 0,
                    msg = "操作成功",
                    data = new BuyResult(true, total, skuQuantity)
                };

                return base.JsonActionResult(result);
            }
            catch (Exception ex)
            {
                Logger.WriterLogger("Order.RemoveCartItem", ex, LoggerType.Error);
            }

            return base.JsonFaultResult(new FaultInfo(40300, msg), request.ToString());
        }

        [HttpPost]
        public IHttpActionResult RemoveCartItems(JObject request)
        {
            Logger.WriterLogger("Order.RemoveCartItems, Params: " + request.ToString(), LoggerType.Info);

            dynamic json = request;
            JArray jsonItems = json.Items;

            string userId = json.UserId;
            string accessToken = json.accessToken;
            int channel = json.channel;
            int platform = json.platform;
            string ver = json.ver;

            base.SaveVisitInfo(userId, channel, platform, ver);

            List<ParamBuySku> param = new List<ParamBuySku>();

            try
            {
                param = jsonItems.ToObject<List<ParamBuySku>>();
            }
            catch
            {
                // 参数无效
                return base.JsonFaultResult(new CommonException(40100).GetMessage(), request.ToString());
            }

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), request.ToString());
            }

            // 验证参数
            //ThrowParamException(skuId);

            string msg = "";

            Member member = GetMember(userId.ToSeesionId());

            if (member == null)
            {
                return base.JsonFaultResult(new FaultInfo(40201, msg), request.ToString());
            }

            try
            {
                List<string> skuIds = new List<string>();

                foreach (var current in param)
                {
                    string skuId = current.SkuId;

                    skuIds.Add(skuId);
                }

                ShoppingCartProcessor.RemoveLineItems(member, skuIds);

            }
            catch (Exception ex)
            {
                Logger.WriterLogger(ex, LoggerType.Error);
            }

            ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetShoppingCart(member);
            if (shoppingCart != null)
            {
                StandardResult<BuyResult> result = new StandardResult<BuyResult>()
                {
                    code = 0,
                    msg = "操作成功",
                    data = new BuyResult(true, shoppingCart.GetTotal(), shoppingCart.GetQuantity())
                };

                return base.JsonActionResult(result);
            }

            return base.JsonFaultResult(new FaultInfo(40300, msg), request.ToString());
        }

        [HttpPost]
        public IHttpActionResult RemoveAllCartItem(JObject request)
        {
            Logger.WriterLogger("Order.RemoveAllCartItem, Params: " + request.ToString(), LoggerType.Info);

            ParamUserBase param = new ParamUserBase();

            try
            {
                param = request.ToObject<ParamUserBase>();
            }
            catch
            {
                // 参数无效
                return base.JsonFaultResult(new CommonException(40100).GetMessage(), request.ToString());
            }

            string accessToken = param.accessToken;

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), request.ToString());
            }

            // 验证参数
            //ThrowParamException(skuId);

            string userId = param.UserId;

            int channel = param.channel;
            int platform = param.platform;
            string ver = param.ver;

            base.SaveVisitInfo(userId, channel, platform, ver);

            string msg = "";

            Member member = GetMember(userId.ToSeesionId());

            if (member == null)
            {
                return base.JsonFaultResult(new FaultInfo(40201, msg), request.ToString());
            }

            try
            {
                ShoppingCartProcessor.ClearShoppingCart(member.UserId);
            }
            catch (Exception ex)
            {
                Logger.WriterLogger(ex, LoggerType.Error);
            }

            try
            {
                decimal total = 0M;
                int skuQuantity = 0;

                ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetShoppingCart(member);
                if (shoppingCart != null)
                {
                    total = shoppingCart.GetTotal();
                    skuQuantity = shoppingCart.GetQuantity();
                }

                StandardResult<BuyResult> result = new StandardResult<BuyResult>()
                {
                    code = 0,
                    msg = "操作成功",
                    data = new BuyResult(true, total, skuQuantity)
                };

                return base.JsonActionResult(result);
            }
            catch (Exception ex)
            {
                Logger.WriterLogger("Order.RemoveAllCartItem", ex, LoggerType.Error);
            }

            return base.JsonFaultResult(new FaultInfo(40300, msg), request.ToString());
        }

        #endregion

        #region Buy

        [HttpPost]
        public IHttpActionResult CalculateFreight(JObject request)
        {
            Logger.WriterLogger("Order.CalculateFreight, Params: " + request.ToString(), LoggerType.Info);

            ParamFreight param = new ParamFreight();

            try
            {
                param = request.ToObject<ParamFreight>();
            }
            catch
            {
                // 参数无效
                return base.JsonFaultResult(new CommonException(40100).GetMessage(), request.ToString());
            }

            string accessToken = param.accessToken;

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), request.ToString());
            }

            // 验证参数
            //ThrowParamException(skuId);


            int channel = param.channel;
            int platform = param.platform;
            string ver = param.ver;

            string userId = param.UserId;
            int regionId = param.RegionId;

            base.SaveVisitInfo(userId, channel, platform, ver);

            List<ParamFreightSku> skus = param.Skus;

            try
            {
                Member member = GetMember(userId.ToSeesionId());

                if (member == null)
                {
                    return base.JsonFaultResult(new CommonException(40201).GetMessage(), request.ToString());
                }

                string skuIds = "";

                foreach (ParamFreightSku sku in skus)
                {
                    skuIds = skuIds + (skuIds == "" ? "" : ",") + sku.SkuId;
                }

                ShoppingCartInfo shoppingCartInfo = null;

                if (skuIds == "")
                    shoppingCartInfo = ShoppingCartProcessor.GetShoppingCart(member);
                else
                    shoppingCartInfo = ShoppingCartProcessor.GetShoppingCartInfoBySkus(member, skuIds);

                decimal freight = 0m;

                bool groupFlag = false;

                Dictionary<int, decimal> dictShippingMode = new Dictionary<int, decimal>();

                if (shoppingCartInfo.LineItems.Count != shoppingCartInfo.LineItems.Count((ShoppingCartItemInfo a) => a.IsfreeShipping) && !shoppingCartInfo.IsFreightFree)
                {
                    foreach (ShoppingCartItemInfo item in shoppingCartInfo.LineItems)
                    {
                        if ((!item.IsfreeShipping || !groupFlag))
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
                        freight += ShoppingProcessor.CalcFreight(regionId, item.Value, shippingMode);
                    }

                    StandardResult<decimal> result = new StandardResult<decimal>()
                    {
                        code = 0,
                        msg = "计算运费结果",
                        data = freight
                    };

                    return base.JsonActionResult(result);
                }

                return base.JsonFaultResult(new CommonException(40315).GetMessage(), request.ToString());
            }
            catch (Exception ex)
            {
                Logger.WriterLogger("Order.Refund", ex, LoggerType.Error);
            }

            return base.JsonFaultResult(new CommonException(40999).GetMessage(), request.ToString());
        }

        [HttpPost]
        public IHttpActionResult Buy(JObject request)
        {
            Logger.WriterLogger("Order.Buy, Params: " + request.ToString(), LoggerType.Info);

            ParamOrder param = new ParamOrder();

            try
            {
                param = request.ToObject<ParamOrder>();
            }
            catch
            {
                // 参数无效
                return base.JsonFaultResult(new CommonException(40100).GetMessage(), request.ToString());
            }

            string accessToken = param.accessToken;

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), request.ToString());
            }

            // 验证参数
            //ThrowParamException(skuId);

            string userId = param.UserId;
            int channel = param.channel;
            int platform = param.platform;
            string ver = param.ver;

            base.SaveVisitInfo(userId, channel, platform, ver);

            try
            {
                AddressItem address = param.ShippingAddress;

                if (address == null)
                {
                    return base.JsonFaultResult(new CommonException(40303).GetMessage(), request.ToString());
                }

                string buySkuIds = "";

                if (param.SkuIds != null)
                {
                    List<OrderSkuId> skuIds = param.SkuIds;

                    foreach (OrderSkuId current in skuIds)
                    {
                        buySkuIds = buySkuIds + (buySkuIds == "" ? "" : ",") + current.SkuId;
                    }
                }

                Member member = GetMember(userId.ToSeesionId(), false);

                bool IsUnpackOrder = false;
                if (param.IsUnpackOrder == 1)
                {
                    IsUnpackOrder = true;
                }

                OrderSource ordersource = this.CovertToOrderSource(param.platform);

                if (member != null)
                {
                    return this.ProcessSubmmitOrder(member, param.SiteId, param.StoreId, address.Id, param.UsedCouponCode, 0,
                        param.Remark, param.RealName, param.IdNo, "", 0, buySkuIds, 0, param.ShippingDate, param.ShippingModeId, param.PaymentTypeId, IsUnpackOrder, ordersource, channel, platform, ver);
                }
                else
                {
                    return base.JsonFaultResult(new CommonException(40201).GetMessage(), request.ToString());
                }
            }
            catch (Exception ex)
            {
                Logger.WriterLogger(ex, LoggerType.Error);
            }

            return base.JsonFaultResult(new CommonException(40999).GetMessage(), request.ToString());
        }


        [HttpPost]
        public IHttpActionResult Groupon()
        {
            return null;
        }

        [HttpPost]
        public IHttpActionResult QuickBuy(JObject request)
        {
            Logger.WriterLogger("Order.QuickBuy, Params: " + request.ToString(), LoggerType.Info);

            ParamOrderSignBuy param = new ParamOrderSignBuy();

            try
            {
                param = request.ToObject<ParamOrderSignBuy>();
            }
            catch
            {
                // 参数无效
                return base.JsonFaultResult(new CommonException(40100).GetMessage(), request.ToString());
            }

            string accessToken = param.accessToken;

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), request.ToString());
            }

            // 验证参数
            //ThrowParamException(skuId);

            string userId = param.UserId;
            int channel = param.channel;
            int platform = param.platform;
            string ver = param.ver;

            int productId = param.ProductId;
            string skuId = param.SkuId;
            int quantity = param.Quantity;

            int buyId = param.BuyActivityId;

            try
            {
                AddressItem address = param.ShippingAddress;

                if (address == null)
                {
                    return base.JsonFaultResult(new CommonException(40303).GetMessage(), request.ToString());
                }

                Member member = GetMember(userId.ToSeesionId(), false);

                bool isUnpackOrder = false;
                if (param.IsUnpackOrder == 1)
                {
                    isUnpackOrder = true;
                }

                OrderSource ordersource = this.CovertToOrderSource(param.platform);

                if (member != null)
                {
                    string buyType = (string.IsNullOrEmpty(param.BuyType) ? "signbuy" : param.BuyType.ToLower());
                    return this.ProcessSubmmitOrder(member, param.SiteId, param.StoreId, address.Id, param.UsedCouponCode, buyId,
                        param.Remark, param.RealName, param.IdNo, buyType, quantity, skuId, param.BuyActivityId, param.ShippingDate, param.ShippingModeId, param.PaymentTypeId, isUnpackOrder, ordersource, channel, platform, ver);
                }
                else
                {
                    return base.JsonFaultResult(new CommonException(40201).GetMessage(), request.ToString());
                }
            }
            catch (Exception ex)
            {
                Logger.WriterLogger(ex, LoggerType.Error);
            }

            return base.JsonFaultResult(new CommonException(40999).GetMessage(), request.ToString());
        }


        [HttpPost]
        public IHttpActionResult Prepay(JObject request)
        {
            Logger.WriterLogger("Order.Prepay, Params: " + request.ToString(), LoggerType.Info);

            ParamPrepay param = new ParamPrepay();

            try
            {
                param = request.ToObject<ParamPrepay>();
            }
            catch
            {
                // 参数无效
                return base.JsonFaultResult(new CommonException(40100).GetMessage(), request.ToString());
            }

            string accessToken = param.accessToken;
            string sessionKey = "";
            string sessionSecret = "";
            string appSecret = "";

            // 验证令牌
            int accessTookenCode = base.VerifyAccessToken(accessToken, out sessionKey, out sessionSecret, out appSecret);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), request.ToString());
            }

            // 验证参数
            //ThrowParamException(skuId);


            int channel = param.channel;
            int platform = param.platform;
            string ver = param.ver;

            string tradeNo = param.TradeNo;
            string orderId = param.OrderId;
            string signature = param.Signature;

            DateTime tradeTime = DateTime.Now;

            decimal tradeAmount = param.TradeAmount;
            int paymentTypeId = param.PaymentTypeId;
            string paymentTypeName = param.PaymentTypeName;

            if (base.IsVerifyAccessToken && base.IsPaySignature)
            {
                SortedDictionary<string, string> data = new SortedDictionary<string, string>();
                data.Add("accessToken", accessToken);
                data.Add("TradeNo", tradeNo);
                data.Add("OrderId", orderId);
                data.Add("TradeAmount", tradeAmount.ToString("0.00"));
                data.Add("PaymentTypeId", paymentTypeId.ToString());
                data.Add("PaymentTypeName", paymentTypeName);

                string signatureData = base.Sign(data, appSecret, sessionSecret);

                if (signature != signatureData)
                {
                    return base.JsonFaultResult(new CommonException(40005).GetMessage(), request.ToString());
                }
            }

            try
            {
                OrderInfo order = ShoppingProcessor.GetOrderInfo(orderId);

                if (order == null)
                {
                    return base.JsonFaultResult(new CommonException(40306).GetMessage(), request.ToString());
                }

                if (order.OrderStatus == OrderStatus.BuyerAlreadyPaid)
                {
                    return base.JsonFaultResult(new CommonException(40307).GetMessage(), request.ToString());
                }

                if (order.OrderStatus != OrderStatus.WaitBuyerPay)
                {
                    return base.JsonFaultResult(new CommonException(40308).GetMessage(), request.ToString());
                }

                order.PaymentTypeId = paymentTypeId;
                order.PaymentType = paymentTypeName;
                order.GatewayOrderId = tradeNo;
                order.PayDate = tradeTime;

                if (order.Gateway.ToLower() != "Ecdev.plugins.payment.wx_apppay.wxwappayrequest".ToLower())
                {
                    return base.JsonFaultResult(new CommonException(40396).GetMessage(), request.ToString());
                }

                // 更新支付状态（操作动作）
                OrderHelper.SetOrderPayStatus(orderId, 1, order.PaymentTypeId, order.PaymentType, order.Gateway, "");

                PaymentModeInfo paymentMode = SalesHelper.GetPaymentMode("Ecdev.plugins.payment.wx_apppay.wxwappayrequest");
                string weixinAppId = "";
                string mchKey = "";
                string weixinAppSecret = "";
                string mchId = "";

                if (paymentMode != null)
                {
                    if (paymentMode.Settings != "")
                    {
                        string xml = HiCryptographer.Decrypt(paymentMode.Settings);

                        try
                        {
                            System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
                            xmlDocument.LoadXml(xml);

                            weixinAppId = xmlDocument.GetElementsByTagName("AppId")[0].InnerText;
                            mchKey = xmlDocument.GetElementsByTagName("Key")[0].InnerText;
                            weixinAppSecret = xmlDocument.GetElementsByTagName("AppSecret")[0].InnerText;
                            mchId = xmlDocument.GetElementsByTagName("Mch_id")[0].InnerText;
                        }
                        catch
                        {
                            return base.JsonFaultResult(new CommonException(40371).GetMessage(), request.ToString());
                        }
                    }
                }
                else
                {
                    return base.JsonFaultResult(new CommonException(40371).GetMessage(), request.ToString());
                }


                // 步骤3：统一下单接口返回正常的prepay_id，再按签名规范重新生成签名后，将数据传输给APP。参与签名的字段名为appId，partnerId，prepayId，nonceStr，timeStamp，package。注意：package的值格式为Sign=WXPay
                string payReponseMsg = "";
                Ecdev.Weixin.Pay.Domain.VCodePayResponsEntity reponseEntity = Ecdev.Weixin.Pay.Pay.VCodePayHelper.UnifiedOrder(order.OrderId, order.GetNewTotal(), order.OrderId, weixinAppId, mchId, mchKey, base.HOST + "/pay/wx_Pay_notify_url.aspx", "127.0.0.1", out payReponseMsg);
                if (string.IsNullOrEmpty(payReponseMsg))
                {

                    TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
                    string timestamp = Convert.ToInt64(ts.TotalSeconds).ToString();
                    string noncestr = reponseEntity.Nonce_str;    // Guid.NewGuid().ToString().ToLowerGuid();

                    //string sign = SecurityUtil.MD5Encrypt(order.OrderId + tradeNo + reponseEntity.prepay_id + timestamp + noncestr + sessionSecret).ToLower();
                    Dictionary<string, string> dict = new Dictionary<string, string>();
                    dict.Add("appId", weixinAppId);
                    dict.Add("partnerId", mchId);
                    dict.Add("prepayId", reponseEntity.prepay_id);
                    dict.Add("noncestr", reponseEntity.Nonce_str);
                    dict.Add("timestamp", timestamp);
                    dict.Add("package", "Sign=WXPay");

                    string sign = SecurityUtil.GetResultSign(dict, mchKey);

                    StandardResult<PrepayResult> result = new StandardResult<PrepayResult>()
                    {
                        code = 0,
                        msg = "预支付成功",
                        data = new PrepayResult()
                        {
                            prepay_id = reponseEntity.prepay_id,
                            noncestr = noncestr,
                            timestamp = timestamp,
                            order_id = order.OrderId,
                            out_trade_no = tradeNo,
                            sign = sign
                        }

                    };

                    return base.JsonActionResult(result);
                }
                else
                {
                    StandardResult<String> result = new StandardResult<String>()
                    {
                        code = 40370,
                        msg = "预支付失败, " + payReponseMsg,
                        data = null
                    };

                    return base.JsonActionResult(result);
                }
            }
            catch (Exception ex)
            {
                Logger.WriterLogger("Order.Prepay", ex, LoggerType.Error);
            }

            return base.JsonFaultResult(new CommonException(40999).GetMessage(), request.ToString());
        }

        [HttpPost]
        public IHttpActionResult Pay(JObject request)
        {
            Logger.WriterLogger("Order.Pay, Params: " + request.ToString(), LoggerType.Info);

            ParamPay param = new ParamPay();

            try
            {
                param = request.ToObject<ParamPay>();
            }
            catch
            {
                // 参数无效
                return base.JsonFaultResult(new CommonException(40100).GetMessage(), request.ToString());
            }

            string accessToken = param.accessToken;
            string sessionKey = "";
            string sessionSecret = "";
            string appSecret = "";

            // 验证令牌
            int accessTookenCode = base.VerifyAccessToken(accessToken, out sessionKey, out sessionSecret, out appSecret);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), request.ToString());
            }

            // 验证参数
            //ThrowParamException(skuId);


            int channel = param.channel;
            int platform = param.platform;
            string ver = param.ver;

            string tradeNo = param.TradeNo;
            string orderId = param.OrderId;
            string signature = param.Signature;

            DateTime tradeTime = DateTime.Now;

            //if (DateTime.TryParse(param.TradeTime, out tradeTime))
            //{}

            decimal tradeAmount = param.TradeAmount;
            int paymentTypeId = param.PaymentTypeId;
            string paymentTypeName = param.PaymentTypeName;

            if (base.IsVerifyAccessToken && base.IsPaySignature)
            {
                SortedDictionary<string, string> data = new SortedDictionary<string, string>();
                data.Add("accessToken", accessToken);
                data.Add("TradeNo", tradeNo);
                data.Add("OrderId", orderId);
                data.Add("TradeAmount", tradeAmount.ToString("0.00"));
                data.Add("PaymentTypeId", paymentTypeId.ToString());
                data.Add("PaymentTypeName", paymentTypeName);

                string signatureData = base.Sign(data, appSecret, sessionSecret);

                if (signature != signatureData)
                {
                    return base.JsonFaultResult(new CommonException(40005).GetMessage(), request.ToString());
                }
            }

            try
            {
                OrderInfo order = ShoppingProcessor.GetOrderInfo(orderId);

                if (order == null)
                {
                    return base.JsonFaultResult(new CommonException(40306).GetMessage(), request.ToString());
                }

                Logger.WriterLogger("Order.Pay, Params order.OrderStatus: " + (int)order.OrderStatus, LoggerType.Info);

                if (order.OrderStatus == OrderStatus.BuyerAlreadyPaid)
                {
                    // return base.JsonFaultResult(new CommonException(40307).GetMessage(), request.ToString());
                    // 直接返回成功
                    StandardResult<String> result = new StandardResult<String>()
                    {
                        code = 0,
                        msg = "付款成功",
                        data = order.OrderId
                    };

                    return base.JsonActionResult(result);
                }

                if (order.OrderStatus != OrderStatus.WaitBuyerPay)
                {
                    return base.JsonFaultResult(new CommonException(40308).GetMessage(), request.ToString());
                }

                order.PaymentTypeId = paymentTypeId;
                order.PaymentType = paymentTypeName;
                order.GatewayOrderId = tradeNo;
                //支付宝就不更新
                /*if (!string.IsNullOrWhiteSpace(tradeNo) && order.GatewayOrderId != tradeNo && paymentTypeName !="支付宝")
                {
                    order.GatewayOrderId = tradeNo;
                }*/
                order.PayDate = tradeTime;

                PaymentModeInfo paymentMode = ShoppingProcessor.GetPaymentMode(paymentTypeId);

                if (paymentMode != null)
                {
                    order.PaymentTypeId = paymentMode.ModeId;
                    order.PaymentType = paymentMode.Name;
                    order.Gateway = paymentMode.Gateway;
                }
                else
                {
                    paymentMode = ShoppingProcessor.GetPaymentMode(order.Gateway);
                }

                if (paymentMode != null)
                {
                    // 微信支付
                    if (paymentMode.Gateway.ToLower() == "Ecdev.plugins.payment.wx_apppay.wxwappayrequest".ToLower())
                    {
                        bool payState = GetWxPayState(order.OrderId, paymentMode, out tradeNo);

                        if (payState)
                        {
                            order.GatewayOrderId = tradeNo;
                        }
                    }
                }

                bool ret = this.UserPayOrder(order, request.ToString());

                if (ret)
                {
                    StandardResult<String> result = new StandardResult<String>()
                    {
                        code = 0,
                        msg = "付款成功",
                        data = order.OrderId
                    };

                    return base.JsonActionResult(result);
                }
                else
                {
                    return base.JsonFaultResult(new CommonException(40201).GetMessage(), request.ToString());
                }
            }
            catch (Exception ex)
            {
                Logger.WriterLogger("Order.Pay", ex, LoggerType.Error);
            }

            return base.JsonFaultResult(new CommonException(40999).GetMessage(), request.ToString());
        }

        [HttpPost]
        public IHttpActionResult ChangePaymentType(JObject request)
        {
            Logger.WriterLogger("Order.ChangePaymentType, Params: " + request.ToString(), LoggerType.Info);

            ParamChangePaymentType param = new ParamChangePaymentType();

            try
            {
                param = request.ToObject<ParamChangePaymentType>();
            }
            catch
            {
                // 参数无效
                return base.JsonFaultResult(new CommonException(40100).GetMessage(), request.ToString());
            }

            string accessToken = param.accessToken;

            // 验证令牌
            int accessTookenCode = base.VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), request.ToString());
            }

            // 验证参数
            //ThrowParamException(skuId);


            int channel = param.channel;
            int platform = param.platform;
            string ver = param.ver;

            string orderId = param.OrderId;

            int paymentTypeId = param.PaymentTypeId;
            string paymentTypeName = param.PaymentTypeName;

            try
            {
                OrderInfo order = ShoppingProcessor.GetOrderInfo(orderId);

                if (order == null)
                {
                    return base.JsonFaultResult(new CommonException(40306).GetMessage(), request.ToString());
                }

                if (order.OrderStatus != OrderStatus.WaitBuyerPay)
                {
                    return base.JsonFaultResult(new CommonException(40308).GetMessage(), request.ToString());
                }

                order.PaymentTypeId = paymentTypeId;
                order.PaymentType = paymentTypeName;

                PaymentModeInfo paymentMode = ShoppingProcessor.GetPaymentMode(paymentTypeId);

                if (paymentMode != null)
                {
                    order.PaymentType = paymentMode.Name;
                    order.Gateway = paymentMode.Gateway;
                }

                bool ret = ShoppingProcessor.ChangeOrderPaymentType(order);

                if (ret)
                {
                    StandardResult<String> result = new StandardResult<String>()
                    {
                        code = 0,
                        msg = "支付方式修改成功",
                        data = order.OrderId
                    };

                    return base.JsonActionResult(result);
                }
                else
                {
                    return base.JsonFaultResult(new CommonException(40201).GetMessage(), request.ToString());
                }
            }
            catch (Exception ex)
            {
                Logger.WriterLogger("Order.ChangePaymentTypes", ex, LoggerType.Error);
            }

            return base.JsonFaultResult(new CommonException(40999).GetMessage(), request.ToString());
        }

        // 批量添加

        // 查询购物车
        [HttpGet]
        public IHttpActionResult ShopingCart(string userId, string accessToken, int channel, int platform, string ver)
        {
            //Logger.WriterLogger("Order.ShoppingCart, Params: " + request.ToString(), LoggerType.Info);

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), "ShopingCart: ");
            }

            // 验证参数
            //ThrowParamException(skuId);

            // 保存访问信息
            base.SaveVisitInfo(userId, channel, platform, ver);

            string msg = "";

            try
            {
                Member member = GetMember(userId.ToSeesionId());

                if (member != null)
                {
                    ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetShoppingCart(member);
                    if (shoppingCart != null)
                    {
                        CartResult data = new CartResult();

                        data.ReducedPromotionId = shoppingCart.ReducedPromotionId;
                        data.ReducedPromotionName = shoppingCart.ReducedPromotionName ?? "";
                        data.ReducedPromotionAmount = shoppingCart.ReducedPromotionAmount;
                        data.IsReduced = shoppingCart.IsReduced;
                        data.SendGiftPromotionId = shoppingCart.SendGiftPromotionId;
                        data.SendGiftPromotionName = shoppingCart.SendGiftPromotionName ?? "";
                        data.IsSendGift = shoppingCart.IsSendGift;
                        data.SentTimesPointPromotionId = shoppingCart.SentTimesPointPromotionId;
                        data.SentTimesPointPromotionName = shoppingCart.SentTimesPointPromotionName ?? "";
                        data.IsSendTimesPoint = shoppingCart.IsSendTimesPoint;
                        data.TimesPoint = shoppingCart.TimesPoint;
                        data.FreightFreePromotionId = shoppingCart.FreightFreePromotionId;
                        data.FreightFreePromotionName = shoppingCart.FreightFreePromotionName ?? "";
                        data.IsFreightFree = shoppingCart.IsFreightFree;
                        data.Weight = shoppingCart.Weight;
                        data.TotalWeight = shoppingCart.TotalWeight;
                        data.Total = shoppingCart.GetTotal();
                        data.TotalNeedPoint = shoppingCart.GetTotalNeedPoint();
                        data.Point = shoppingCart.GetPoint();
                        //data.GetPointMoney = shoppingCart.GetPoint();
                        data.Amount = shoppingCart.GetAmount();
                        data.GetOriginalAmount = shoppingCart.GetOriginalAmount();
                        data.TotalTax = shoppingCart.GetTotalTax();
                        data.TotalIncludeTax = shoppingCart.GetTotalTax();   //计算总价，包含税费，不包含运费
                        data.Quantity = shoppingCart.GetQuantity();

                        // List<CartItem> cartItems = new List<CartItem>();

                        Dictionary<int, SupplierCartItem> DtSupplierCartItems = new Dictionary<int, SupplierCartItem>();

                        foreach (ShoppingCartItemInfo current in shoppingCart.LineItems)
                        {
                            CartItem item = new CartItem();

                            if (!DtSupplierCartItems.ContainsKey(current.SupplierId))
                            {
                                SupplierCartItem supplierCartItem = new SupplierCartItem();
                                supplierCartItem.SupplierId = current.SupplierId;
                                supplierCartItem.SupplierName = current.SupplierName;
                                supplierCartItem.Logo = Util.AppendImageHost(current.Logo);
                                DtSupplierCartItems.Add(current.SupplierId, supplierCartItem);
                            }

                            item.SkuId = current.SkuId;
                            item.ProductId = current.ProductId;
                            item.SKU = current.SKU;
                            item.Name = current.Name;
                            item.MemberPrice = current.MemberPrice;
                            item.ThumbnailUrl40 = Util.AppendImageHost(current.ThumbnailUrl40);
                            item.ThumbnailUrl60 = Util.AppendImageHost(current.ThumbnailUrl60);
                            item.ThumbnailUrl100 = Util.AppendImageHost(current.ThumbnailUrl100);
                            item.Weight = current.Weight;
                            item.SkuContent = current.SkuContent;
                            item.Quantity = current.Quantity;
                            item.PromotionId = current.PromotionId;
                            item.PromoteType = current.PromoteType;
                            item.PromotionName = current.PromotionName ?? "";
                            item.AdjustedPrice = current.AdjustedPrice;
                            item.ShippQuantity = current.ShippQuantity;
                            item.IsSendGift = current.IsSendGift;
                            item.SubTotal = current.SubTotal;
                            item.IsFreeShipping = current.IsfreeShipping;
                            item.SubWeight = current.GetSubWeight();
                            item.TaxRate = current.TaxRate;
                            item.TaxRateId = current.TaxRateId;
                            item.TemplateId = current.TemplateId;
                            item.StoreId = current.StoreId;
                            item.SupplierId = current.SupplierId;
                            item.SupplierName = current.SupplierName;
                            item.IsCustomsClearance = current.IsCustomsClearance;
                            item.Stock = current.Stock;
                            item.StockDesc = current.Stock > 20 ? "有库存" : "库存紧张";

                            item.Logo = Util.AppendImageHost(current.Logo);

                            item.BuyCardinality = current.BuyCardinality;

                            //item.SubTax = current.Quantity * current.AdjustedPrice * current.TaxRate;

                            item.Tax = current.GetTax();
                            item.SubTax = current.Quantity * item.Tax;

                            item.ExtendTaxRate = current.ComTaxRate;



                            DtSupplierCartItems[item.SupplierId].CartItems.Add(item);
                        }

                        data.SupplierCartItems = DtSupplierCartItems.Values.ToList();



                        List<GiftItem> giftItems = new List<GiftItem>();

                        foreach (ShoppingCartGiftInfo current in shoppingCart.LineGifts)
                        {
                            GiftItem item = new GiftItem();

                            item.GiftId = item.GiftId;
                            item.Name = current.Name;
                            item.CostPrice = current.CostPrice;
                            item.NeedPoint = current.NeedPoint;
                            item.Quantity = current.Quantity;
                            item.ThumbnailUrl40 = Util.AppendImageHost(current.ThumbnailUrl40);
                            item.ThumbnailUrl60 = Util.AppendImageHost(current.ThumbnailUrl60);
                            item.ThumbnailUrl100 = Util.AppendImageHost(current.ThumbnailUrl100);
                            item.PromoType = current.PromoType;
                            item.SubPointTotal = current.SubPointTotal;

                            giftItems.Add(item);
                        }

                        data.GiftItems = giftItems;

                        StandardResult<CartResult> result = new StandardResult<CartResult>()
                        {
                            code = 0,
                            msg = "",
                            data = data
                        };

                        return base.JsonActionResult(result);
                    }

                    else
                    {
                        StandardResult<CartResult> emptyResult = new StandardResult<CartResult>()
                        {
                            code = 0,
                            msg = "购物车商品为空",
                            data = new CartResult()
                        };

                        return base.JsonActionResult(emptyResult);

                    }
                }

                else
                {
                    return base.JsonFaultResult(new CommonException(40201).GetMessage(), "Member.Get");
                }




            }
            catch (Exception ex)
            {
                Logger.WriterLogger("Order.ShopingCart", ex, LoggerType.Error);
            }

            return base.JsonFaultResult(new FaultInfo(40300, msg), "Shopping cart");
        }

        [HttpGet]
        public IHttpActionResult Details(string userId, string orderId, string accessToken, int channel, int platform, string ver)
        {
            Logger.WriterLogger("Order.Details, Params: " + string.Format("userId={0}&accessToken={1}&channel={2}&platform={3}&ver={4}&orderId={5}", userId, accessToken, channel, platform, ver, orderId), LoggerType.Info);

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), "Order Details: ");
            }

            // 验证参数
            //ThrowParamException(skuId);

            // 保存访问信息
            base.SaveVisitInfo(userId, channel, platform, ver);

            string msg = "";

            Member member = GetMember(userId.ToSeesionId());

            if (member == null)
            {
                return base.JsonFaultResult(new CommonException(40201).GetMessage(), "Order.Details");
            }

            OrderInfo order = ShoppingProcessor.GetOrderInfo(orderId);

            if (order != null)
            {
                int reviewProductQty = 0;

                DataTable productReviewAll = ProductBrowser.GetProductReviewAll(orderId, member.UserId);
                if (productReviewAll != null)
                {
                    List<int> productIds = new List<int>();

                    foreach (DataRow current in productReviewAll.Rows)
                    {
                        int productId = (int)current["ProductId"];

                        if (!productIds.Contains(productId))
                        {
                            if (current["ReviewId"] != DBNull.Value)
                            {
                                productIds.Add(productId);
                            }
                        }
                    }

                    reviewProductQty = productIds.Count;
                }

                List<OrderReviewListItem> reviews = GetOrderReviews(orderId, member.UserId);

                OrderItemResult data = new OrderItemResult();

                data.OrderId = order.OrderId;
                data.OrderDate = order.OrderDate.ToString("yyyy-MM-dd HH:mm:ss");
                data.OrderStatus = (int)order.OrderStatus;
                data.OrderSource = (int)order.OrderSource;
                //data.Address = order.ShippingRegion + " " + order.Address + " " + order.ZipCode;

                data.SourceOrderId = order.SourceOrderId;
                data.IsRefund = order.IsRefund;
                data.IsCancelOrder = order.IsCancelOrder;

                //去除邮编
                data.Address = order.ShippingRegion + " " + order.Address;
                data.Reciver = order.ShipTo;
                data.Telephone = order.TelPhone;
                data.Cellphone = order.CellPhone;
                data.ShipToDate = order.ShipToDate;
                data.ShippingDate = "";
                if (data.OrderStatus == 3 || data.OrderStatus == 5 || data.OrderStatus == 7 || data.OrderStatus == 10)
                {
                    data.ShippingDate = order.ShippingDate.ToString("yyyy-MM-dd HH:mm:ss");
                }
                data.ShippingModeName = order.ModeName;
                data.PaymentTypeId = order.PaymentTypeId;
                data.PaymentTypeName = order.PaymentType;
                data.Gateway = order.Gateway;
                data.GatewayOrderId = order.GatewayOrderId;
                data.CouponName = order.CouponName;
                data.VoucherName = order.VoucherName;
                data.FinishDate = "";
                if (data.OrderStatus == 5 || data.OrderStatus == 7 || data.OrderStatus == 10)
                {
                    data.FinishDate = order.FinishDate.ToString("yyyy-MM-dd HH:mm:ss");
                }
                data.PayDate = "";
                if (data.OrderStatus == 2 || data.OrderStatus == 3 || data.OrderStatus == 5 || data.OrderStatus == 6 || data.OrderStatus == 7 || data.OrderStatus == 8 || data.OrderStatus == 9 || data.OrderStatus == 10)
                {
                    data.PayDate = order.PayDate.ToString("yyyy-MM-dd HH:mm:ss");
                }
                data.IsNeedInvoice = !string.IsNullOrWhiteSpace(order.InvoiceTitle);
                data.InvoiceTitle = order.InvoiceTitle;
                data.Total = order.GetTotal();
                data.Amount = order.GetAmount();
                data.PayCharge = order.PayCharge;
                data.Freight = order.AdjustedFreight;
                data.Tax = order.Tax;
                data.Discount = order.AdjustedDiscount;
                data.Point = order.Points;
                data.IsCanReview = order.LineItems.Count != reviewProductQty;
                data.IdentityCard = order.IdentityCard;

                data.Remark = order.Remark;



                if (data.OrderStatus == 6 || data.OrderStatus == 7 || data.OrderStatus == 8 || data.OrderStatus == 9 || data.OrderStatus == 10)
                {
                    DataTable serverInfo = OrderHelper.GetServiceOrder(data.OrderId);

                    if (serverInfo != null)
                    {
                        if (serverInfo.Rows.Count > 0)
                        {
                            DataRow row = serverInfo.Rows[0];

                            data.ApplyId = 0;
                            if (row["ApplyId"] != DBNull.Value)
                            {
                                data.ApplyId = int.Parse(row["ApplyId"].ToString());
                            }
                            data.ApplyType = 0;
                            if (row["ApplyType"] != DBNull.Value)
                            {
                                data.ApplyType = int.Parse(row["ApplyType"].ToString());
                            }
                            data.ApplyForTime = "";
                            if (row["ApplyForTime"] != DBNull.Value)
                            {
                                data.ApplyForTime = ((DateTime)row["ApplyForTime"]).ToString("yyyy-MM-dd HH:mm:ss");
                            }
                            data.Comments = row["Comments"].ToString();
                            data.AdminRemark = row["AdminRemark"].ToString();
                            data.HandleStatus = 0;
                            if (row["HandleStatus"] != DBNull.Value)
                            {
                                data.RefundMoney = int.Parse(row["HandleStatus"].ToString());
                            }
                            data.HandleTime = "";
                            if (row["HandleTime"] != DBNull.Value)
                            {
                                data.HandleTime = ((DateTime)row["HandleTime"]).ToString("yyyy-MM-dd HH:mm:ss");
                            }
                            data.RefundMoney = 0;
                            if (row["RefundMoney"] != DBNull.Value)
                            {
                                data.RefundMoney = decimal.Parse(row["RefundMoney"].ToString());
                            }
                            data.RefundType = 0;
                            if (row["RefundType"] != DBNull.Value)
                            {
                                data.RefundType = int.Parse(row["RefundType"].ToString());
                            }

                            data.ExpressCompany = row["ExpressCompany"].ToString();
                            data.TrackingNumber = row["TrackingNumber"].ToString();
                        }
                    }
                }

                List<OrderSkuItem> skuItems = new List<OrderSkuItem>();

                foreach (LineItemInfo current in order.LineItems.Values)
                {
                    OrderSkuItem item = new OrderSkuItem();

                    item.SkuId = current.SkuId;
                    item.ProductId = current.ProductId;
                    item.SkuCode = current.SKU;
                    item.Quantity = current.Quantity;
                    item.Price = current.ItemListPrice;
                    item.AdjustedPrice = current.ItemAdjustedPrice;
                    item.Description = current.ItemDescription;
                    item.ThumbnailsUrl = Util.AppendImageHost(current.ThumbnailsUrl);
                    item.Weight = current.ItemWeight;
                    item.SkuContent = current.SKUContent;
                    item.PromotionName = current.PromotionName;
                    item.TaxRate = current.TaxRate;
                    item.SubTotal = current.GetSubTotal();
                    item.ShareUrl = string.Format(base.PRODUCT_SHARE_URL_BASE, current.ProductId);

                    // 评论
                    item.ReviewContent = "";
                    item.ReviewScore = 5;
                    item.IsCanReview = true;

                    foreach (var review in reviews)
                    {
                        if (review.SkuId != current.SkuId)
                        {
                            continue;
                        }

                        item.ReviewContent = review.Content;
                        item.ReviewScore = review.Score;
                        item.IsCanReview = review.IsCanReview;
                    }

                    // 评论(END)

                    skuItems.Add(item);
                }

                data.Items = skuItems;

                List<OrderGiftItem> giftItems = new List<OrderGiftItem>();

                foreach (OrderGiftInfo current in order.Gifts)
                {
                    OrderGiftItem item = new OrderGiftItem();

                    item.GiftId = current.GiftId;
                    item.GiftName = current.GiftName;
                    item.Quantity = current.Quantity;
                    item.ThumbnailsUrl = Util.AppendImageHost(current.ThumbnailsUrl);

                    giftItems.Add(item);
                }

                data.GiftItems = giftItems;

                StandardResult<OrderItemResult> result = new StandardResult<OrderItemResult>()
                {
                    code = 0,
                    msg = "",
                    data = data
                };

                return base.JsonActionResult(result);
            }

            return base.JsonFaultResult(new FaultInfo(40300, msg), "Order Details");
        }

        [HttpGet]
        public IHttpActionResult Reviews(string userId, string orderId, string accessToken, int channel, int platform, string ver)
        {
            Logger.WriterLogger("Order.Reviews, Params: " + string.Format("userId={0}&accessToken={1}&channel={2}&platform={3}&ver={4}&orderId={5}", userId, accessToken, channel, platform, ver, orderId), LoggerType.Info);

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), "Order Details: ");
            }

            // 验证参数
            //ThrowParamException(skuId);

            // 保存访问信息
            base.SaveVisitInfo(userId, channel, platform, ver);

            string msg = "";

            Member member = GetMember(userId.ToSeesionId());

            if (member == null)
            {
                return base.JsonFaultResult(new CommonException(40201).GetMessage(), "Order.Details");
            }

            List<OrderReviewListItem> items = new List<OrderReviewListItem>();

            DataTable productReviewAll = ProductBrowser.GetProductReviewAll(orderId, member.UserId);
            if (productReviewAll != null)
            {
                OrderReviewListItem item = null;

                foreach (DataRow row in productReviewAll.Rows)
                {
                    int productId = (int)row["ProductId"];

                    string skuId = (string)row["SkuId"];

                    bool exist = false;

                    foreach (var current in items)
                    {
                        if (current.SkuId == skuId)
                        {
                            exist = true;
                            continue;
                        }
                    }

                    if (exist)
                    {
                        continue;
                    }

                    item = new OrderReviewListItem();

                    item.ProductId = productId;
                    item.SkuId = (string)row["SkuId"];
                    item.ProductName = (string)row["ProductName"];
                    item.SkuContent = (string)row["SkuContent"];
                    item.Quantity = (int)row["Quantity"];
                    item.Price = (decimal)row["Price"];
                    item.TaxRate = (decimal)row["TaxRate"];
                    string thumbnailsUrl = Util.AppendImageHost((string)row["ThumbnailsUrl"]);
                    if (!string.IsNullOrEmpty(thumbnailsUrl))
                    {
                        thumbnailsUrl = thumbnailsUrl.Replace("40/40", "180/180");
                    }

                    item.ThumbnailsUrl = thumbnailsUrl;

                    item.Id = 0;
                    if (row["ReviewId"] != DBNull.Value)
                    {
                        item.Id = (long)row["ReviewId"];
                    }
                    item.Content = "";
                    if (row["ReviewText"] != DBNull.Value)
                    {
                        item.Content = (string)row["ReviewText"];
                    }
                    item.ReviewDate = "";
                    if (row["ReviewDate"] != DBNull.Value)
                    {
                        item.ReviewDate = ((DateTime)row["ReviewDate"]).ToString("yyyy-MM-dd HH:mm:ss");
                    }

                    item.Score = (item.Id == 0 ? 0 : 5);
                    if (row["Score"] != DBNull.Value)
                    {
                        item.Score = (int)row["Score"];
                    }

                    item.IsAnonymous = true;
                    if (row["IsAnonymous"] != DBNull.Value)
                    {
                        item.IsAnonymous = (bool)row["IsAnonymous"];
                    }

                    item.IsCanReview = item.Id == 0;

                    items.Add(item);
                }
            }

            ListResult<OrderReviewListItem> data = new ListResult<OrderReviewListItem>();
            data.TotalNumOfRecords = items.Count;
            data.Results = items;
            StandardResult<ListResult<OrderReviewListItem>> result = new StandardResult<ListResult<OrderReviewListItem>>()
            {
                code = 0,
                msg = "",
                data = data
            };

            return base.JsonActionResult(result);
        }

        #endregion

        [HttpGet]
        public IHttpActionResult ShippingMode(string accessToken, int channel, int platform, string ver)
        {
            Logger.WriterLogger("Order.ShippingMode, Params: " + string.Format("&accessToken={0}&channel={1}&platform={2}&ver={3}", accessToken, channel, platform, ver), LoggerType.Info);

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), "ShippingMode: ");
            }

            // 验证参数
            //ThrowParamException(skuId);

            string userId = "";

            // 保存访问信息
            base.SaveVisitInfo(userId, channel, platform, ver);

            List<ShippingModeInfo> modes = ShoppingProcessor.GetShippingModes().ToList();

            List<ShippingModeListItem> items = new List<ShippingModeListItem>();

            foreach (ShippingModeInfo current in modes)
            {
                items.Add(new ShippingModeListItem(current.ModeId, current.Name, current.TemplateId, current.TemplateName, current.Description, current.DisplaySequence));
            }

            StandardResult<ListResult<ShippingModeListItem>> result = new StandardResult<ListResult<ShippingModeListItem>>()
            {
                code = 0,
                msg = "",
                data = new ListResult<ShippingModeListItem>()
                {
                    TotalNumOfRecords = items.Count,
                    Results = items
                }
            };

            return base.JsonActionResult(result);
        }

        [HttpGet]
        public IHttpActionResult ShippingDateMode(string accessToken, int channel, int platform, string ver)
        {
            Logger.WriterLogger("Order.ShippingDateMode, Params: " + string.Format("&accessToken={0}&channel={1}&platform={2}&ver={3}", accessToken, channel, platform, ver), LoggerType.Info);

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), "ShippingDateMode: ");
            }

            // 验证参数
            //ThrowParamException(skuId);

            string userId = "";

            // 保存访问信息
            base.SaveVisitInfo(userId, channel, platform, ver);

            List<ShippingDateModeListItem> items = new List<ShippingDateModeListItem>();

            items.Add(new ShippingDateModeListItem("不限(周一至周日)"));
            items.Add(new ShippingDateModeListItem("工作日(周一至周五)"));
            items.Add(new ShippingDateModeListItem("周末(周六、日)"));

            StandardResult<ListResult<ShippingDateModeListItem>> result = new StandardResult<ListResult<ShippingDateModeListItem>>()
            {
                code = 0,
                msg = "",
                data = new ListResult<ShippingDateModeListItem>()
                {
                    TotalNumOfRecords = items.Count,
                    Results = items
                }
            };

            return base.JsonActionResult(result);
        }

        [HttpGet]
        public IHttpActionResult PaymentMode(string accessToken, int channel, int platform, string ver)
        {
            Logger.WriterLogger("Order.PaymentMode, Params: " + string.Format("&accessToken={0}&channel={1}&platform={2}&ver={3}", accessToken, channel, platform, ver), LoggerType.Info);

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), "PaymentMode: ");
            }

            // 验证参数
            //ThrowParamException(skuId);

            string userId = "";

            // 保存访问信息
            base.SaveVisitInfo(userId, channel, platform, ver);

            PayApplicationType payApplicationType = PayApplicationType.payOnAll;

            switch (platform)
            {
                case 1:
                    payApplicationType = PayApplicationType.payOnPC;
                    break;
                case 2:
                case 3:
                    payApplicationType = PayApplicationType.payOnApp;
                    break;
            }

            List<PaymentModeInfo> modes = ShoppingProcessor.GetPaymentModes(payApplicationType).ToList();

            List<PaymentModeListItem> items = new List<PaymentModeListItem>();

            foreach (PaymentModeInfo current in modes)
            {
                items.Add(new PaymentModeListItem(current.ModeId, current.Name, "", current.Description, false, current.DisplaySequence, current.Gateway));
            }

            StandardResult<ListResult<PaymentModeListItem>> result = new StandardResult<ListResult<PaymentModeListItem>>()
            {
                code = 0,
                msg = "",
                data = new ListResult<PaymentModeListItem>()
                {
                    TotalNumOfRecords = items.Count,
                    Results = items
                }
            };

            return base.JsonActionResult(result);
        }

        [HttpGet]
        public IHttpActionResult SubOrders(string userId, string orderId, string accessToken, int channel, int platform, string ver)
        {
            Logger.WriterLogger("Order.SubOrders, Params: " + string.Format("userId={0}&accessToken={1}&channel={2}&platform={3}&ver={4}&orderId={5}", userId, accessToken, channel, platform, ver, orderId), LoggerType.Info);

            // 保存访问信息
            base.SaveVisitInfo(userId, channel, platform, ver);

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), "Member.MyOrders");
            }

            int num = 0;
            List<OrderItemResult> items = new List<OrderItemResult>();

            Member member = GetMember(userId.ToSeesionId());

            if (member != null)
            {
                OrderQuery orderQuery = new OrderQuery();

                orderQuery.PageIndex = 1;
                orderQuery.PageSize = 100;
                orderQuery.UserId = member.UserId;
                orderQuery.SourceOrderId = orderId;

                Globals.EntityCoding(orderQuery, true);

                DataSet tradeOrders = OrderHelper.GetTradeOrders(orderQuery, out num);

                foreach (DataRow row in tradeOrders.Tables[0].Rows)
                {
                    OrderItemResult item = new OrderItemResult();

                    item.OrderId = row["OrderId"].ToString();
                    item.OrderDate = ((DateTime)row["OrderDate"]).ToString("yyyy-MM-dd HH:mm:ss");
                    item.OrderStatus = (int)row["OrderStatus"];
                    item.OrderSource = (int)row["SourceOrder"];
                    item.Address = row["ShippingRegion"].ToString() + " " + row["Address"].ToString() + " " + row["ZipCode"].ToString();
                    item.Reciver = row["ShipTo"].ToString();
                    item.Telephone = row["TelPhone"].ToString();
                    item.Cellphone = row["CellPhone"].ToString();
                    item.ShipToDate = row["ShipToDate"].ToString();
                    item.ShippingDate = "";
                    if (row["ShippingDate"] != DBNull.Value)
                    {
                        item.ShippingDate = ((DateTime)row["ShippingDate"]).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    item.ShippingModeName = row["ModeName"].ToString();
                    item.PaymentTypeId = int.Parse(row["PaymentTypeId"].ToString());
                    item.PaymentTypeName = row["PaymentType"].ToString();
                    item.IsNeedInvoice = !string.IsNullOrWhiteSpace(row["InvoiceTitle"].ToString());
                    item.InvoiceTitle = row["InvoiceTitle"].ToString();
                    item.BuyQuantity = int.Parse(row["Nums"].ToString());
                    item.Total = decimal.Parse(row["OrderTotal"].ToString());
                    item.Amount = decimal.Parse(row["Amount"].ToString());
                    item.PayCharge = decimal.Parse(row["PayCharge"].ToString());
                    item.Freight = decimal.Parse(row["AdjustedFreight"].ToString());
                    item.Tax = decimal.Parse(row["Tax"].ToString());
                    item.Discount = decimal.Parse(row["AdjustedDiscount"].ToString());

                    List<OrderSkuItem> skuItems = new List<OrderSkuItem>();

                    DataRow[] childRows = row.GetChildRows("OrderRelation");

                    for (int i = 0; i < childRows.Length; i++)
                    {
                        DataRow skuRow = childRows[i];
                        string skuContent = Globals.HtmlEncode(skuRow["SKUContent"].ToString());
                        string thumbnailsUrl = skuRow["ThumbnailsUrl"].ToString();

                        OrderSkuItem skuItem = new OrderSkuItem();

                        skuItem.SkuId = skuRow["SkuId"].ToString();
                        skuItem.ProductId = int.Parse(skuRow["ProductId"].ToString());
                        skuItem.SkuCode = skuRow["SKU"].ToString();
                        skuItem.Quantity = int.Parse(skuRow["Quantity"].ToString());
                        skuItem.Price = decimal.Parse(skuRow["ItemListPrice"].ToString());
                        skuItem.AdjustedPrice = decimal.Parse(skuRow["ItemAdjustedPrice"].ToString());
                        skuItem.Description = skuRow["ItemDescription"].ToString();
                        skuItem.ThumbnailsUrl = Util.AppendImageHost(thumbnailsUrl);
                        skuItem.Weight = decimal.Parse(skuRow["ItemWeight"].ToString());
                        skuItem.SkuContent = skuContent;
                        skuItem.PromotionName = skuRow["PromotionName"].ToString();
                        skuItem.TaxRate = decimal.Parse(skuRow["TaxRate"].ToString());
                        skuItem.SubTotal = decimal.Parse(skuRow["ItemAdjustedPrice"].ToString()) * int.Parse(skuRow["Quantity"].ToString());

                        skuItems.Add(skuItem);
                    }

                    item.Items = skuItems;

                    items.Add(item);
                }
            }

            StandardResult<ListResult<OrderItemResult>> result = new StandardResult<ListResult<OrderItemResult>>();
            result.code = 0;
            result.msg = "";
            result.data = new ListResult<OrderItemResult>()
            {
                TotalNumOfRecords = items.Count,
                Results = items
            };

            return base.JsonActionResult(result);
        }

        [HttpPost]
        public IHttpActionResult FinishOrder(JObject request)
        {
            Logger.WriterLogger("Order.FinishOrder, Params: " + request.ToString(), LoggerType.Info);

            ParamFinishOrder param = new ParamFinishOrder();

            try
            {
                param = request.ToObject<ParamFinishOrder>();
            }
            catch
            {
                // 参数无效
                return base.JsonFaultResult(new CommonException(40100).GetMessage(), request.ToString());
            }

            string accessToken = param.accessToken;

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), request.ToString());
            }

            // 验证参数
            //ThrowParamException(skuId);

            string userId = param.UserId;
            string orderId = param.OrderId;

            int channel = param.channel;
            int platform = param.platform;
            string ver = param.ver;

            // 保存访问信息
            base.SaveVisitInfo(userId, channel, platform, ver);

            OrderInfo orderInfo = EcShop.SaleSystem.Shopping.ShoppingProcessor.GetOrderInfo(orderId);
            if (orderInfo != null && EcShop.SaleSystem.Vshop.MemberProcessor.ConfirmOrderFinish(orderInfo))
            {
                return base.JsonActionResult(new StandardResult<string>()
                {
                    code = 0,
                    msg = "确认收货完成",
                    data = ""
                });
            }

            return base.JsonActionResult(new StandardResult<string>()
            {
                code = 40309,
                msg = "订单当前状态不允许完成",
                data = ""
            });
        }

        public IHttpActionResult CancelOrder(JObject request)
        {
            Logger.WriterLogger("Order.CancelOrder, Params: " + request.ToString(), LoggerType.Info);

            ParamFinishOrder param = new ParamFinishOrder();

            try
            {
                param = request.ToObject<ParamFinishOrder>();
            }
            catch
            {
                // 参数无效
                return base.JsonFaultResult(new CommonException(40100).GetMessage(), request.ToString());
            }

            string accessToken = param.accessToken;

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), request.ToString());
            }

            // 验证参数
            //ThrowParamException(skuId);

            string userId = param.UserId;
            string orderId = param.OrderId;

            int channel = param.channel;
            int platform = param.platform;
            string ver = param.ver;

            // 保存访问信息
            base.SaveVisitInfo(userId, channel, platform, ver);

            OrderInfo orderInfo = EcShop.SaleSystem.Shopping.ShoppingProcessor.GetOrderInfo(orderId);
            if (orderInfo != null && EcShop.SaleSystem.Vshop.MemberProcessor.ConfirmOrderCancel(orderInfo))
            {
                return base.JsonActionResult(new StandardResult<string>()
                {
                    code = 0,
                    msg = "订单已取消",
                    data = ""
                });
            }

            return base.JsonActionResult(new StandardResult<string>()
            {
                code = 40310,
                msg = "订单当前状态不允许取消",
                data = ""
            });
        }


        /// <summary>
        /// 查询子单列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Getlistofchildren(JObject request)
        {
            Logger.WriterLogger("Order.Getlistofchildren, Params: " + request.ToString(), LoggerType.Info);

            ParamFinishOrder param = new ParamFinishOrder();

            try
            {
                param = request.ToObject<ParamFinishOrder>();
            }
            catch
            {
                // 参数无效
                return base.JsonFaultResult(new CommonException(40100).GetMessage(), request.ToString());
            }

            string accessToken = param.accessToken;

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), request.ToString());
            }

            // 验证参数
            //ThrowParamException(skuId);

            string userId = param.UserId;
            string SourceOrderId = param.SourceOrderId;

            int channel = param.channel;
            int platform = param.platform;
            string ver = param.ver;

            // 保存访问信息
            base.SaveVisitInfo(userId, channel, platform, ver);

            List<OrderApiCode> list = EcShop.SaleSystem.Shopping.ShoppingProcessor.Getlistofchildren(SourceOrderId);
            int code = 0;
            string mest = "";
            if (list == null || list.Count() == 0)
            {
                code = -1;
                mest = "数据错误，请重试！";
            }
            else
            {
                string OrderArr = string.Join(",", list.Select(t => t.OrderId).ToList()); //list.Select(t => t.OrderId).ToArray().ToString();
               mest = "同一付款订单：" + OrderArr + "会同步取消，是否确定取消所有订单？";
            }
            StandardResult<ListResult<OrderApiCode>> result = new StandardResult<ListResult<OrderApiCode>>();
            result.code = code;
            result.msg  = mest;
            result.data = new ListResult<OrderApiCode>()
            {
                TotalNumOfRecords = list.Count(),
                Results = list
            };
            return base.JsonActionResult(result);
        }

        /// <summary>
        /// 申请退款
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult RequestApplyrefund(JObject request)
        {
            Logger.WriterLogger("Order.RequestApplyrefund, Params: " + request.ToString(), LoggerType.Info);
            ParamFinishOrder param = new ParamFinishOrder();
            try
            {
                param = request.ToObject<ParamFinishOrder>();
            }
            catch
            {
                // 参数无效
                return base.JsonFaultResult(new CommonException(40100).GetMessage(), request.ToString());
            }
            string accessToken = param.accessToken;
            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), request.ToString());
            }
            // 验证参数
            string userId = param.UserId;
            string orderId = param.OrderId;
            int type = param.Type;

            int channel = param.channel;
            int platform = param.platform;
            string ver = param.ver;

            // 保存访问信息
            base.SaveVisitInfo(userId, channel, platform, ver);
            int delaytime = string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["OrderRefunTime"]) ? 30 : int.Parse(System.Configuration.ConfigurationManager.AppSettings["OrderRefunTime"].ToString());
                
            if (TradeHelper.CheckOrderIsRefund(orderId, type, delaytime))
            {
                bool Rest = TradeHelper.ChangeRefundType(orderId, type);
                if (Rest)
                {
                    return base.JsonActionResult(new StandardResult<string>()
                    {
                        code = 0,
                        msg = "退款申请成功，待商家后台审核，如有疑问，请联系客服!",
                        data = ""
                    });
                }
                return base.JsonActionResult(new StandardResult<string>()
                {
                    code = 50001,
                    msg = "申请退款失败，请稍后再试！",
                    data = ""
                });
            }
            return base.JsonActionResult(new StandardResult<string>()
            {
                code = 50002,
                msg = "您的订单已超过" + delaytime + "分钟，无法取消订单，如有疑问请联系客服！",
                data = ""
            });
        }

        [HttpPost]
        public IHttpActionResult DeleteOrder(JObject request)
        {
            Logger.WriterLogger("Order.CancelOrder, Params: " + request.ToString(), LoggerType.Info);

            ParamFinishOrder param = new ParamFinishOrder();

            try
            {
                param = request.ToObject<ParamFinishOrder>();
            }
            catch
            {
                // 参数无效
                return base.JsonFaultResult(new CommonException(40100).GetMessage(), request.ToString());
            }

            string accessToken = param.accessToken;

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), request.ToString());
            }

            // 验证参数
            //ThrowParamException(skuId);

            string userId = param.UserId;
            string orderId = param.OrderId;

            int channel = param.channel;
            int platform = param.platform;
            string ver = param.ver;

            // 保存访问信息
            base.SaveVisitInfo(userId, channel, platform, ver);

            OrderInfo orderInfo = EcShop.SaleSystem.Shopping.ShoppingProcessor.GetOrderInfo(orderId);
            if (orderInfo != null)
            {
                if (orderInfo.OrderStatus == OrderStatus.Closed && TradeHelper.LogicDeleteOrder(orderId, (int)UserStatus.RecycleDelete) > 0)
                {
                    return base.JsonActionResult(new StandardResult<string>()
                    {
                        code = 0,
                        msg = "订单已删除",
                        data = ""
                    });
                }
            }

            return base.JsonActionResult(new StandardResult<string>()
            {
                code = 40316,
                msg = "订单当前状态不允许删除",
                data = ""
            });
        }

        [HttpGet]
        public IHttpActionResult Delivery()
        {
            return null;
        }

        [HttpGet]
        public IHttpActionResult LogisticsTracking(string userId, string orderId, string accessToken, int channel, int platform, string ver)
        {
            Logger.WriterLogger("Order.MyReturns, Params: " + string.Format("userId={0}&accessToken={1}&channel={2}&platform={3}&ver={4}&orderId={5}", userId, accessToken, channel, platform, ver, orderId), LoggerType.Info);

            // 保存访问信息
            base.SaveVisitInfo(userId, channel, platform, ver);

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), "Order.LogisticsTracking");
            }

            LogisticsTrackingResult trackingResult = new LogisticsTrackingResult();

            Member member = GetMember(userId.ToSeesionId());

            if (member != null)
            {
                //OrderExpress orderExpress = ShoppingProcessor.GetOrderExpressInfo(orderId);
                //if (orderExpress != null && (orderExpress.OrderStatus == OrderStatus.SellerAlreadySent || orderExpress.OrderStatus == OrderStatus.Finished))
                //{
                //expressInfoJson = Newtonsoft.Json.JsonConvert.SerializeObject(orderExpress);
                //}

                OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(orderId);
                if (orderInfo != null && (orderInfo.OrderStatus == OrderStatus.SellerAlreadySent || orderInfo.OrderStatus == OrderStatus.Finished) && !string.IsNullOrEmpty(orderInfo.ExpressCompanyAbb))
                {
                    trackingResult.LogisticsName = orderInfo.ExpressCompanyName;
                    trackingResult.TrackingNumber = orderInfo.ShipOrderNumber;
                    trackingResult.DeliveryDate = orderInfo.ShippingDate.ToString("yyyy-MM-dd HH:mm:ss");

                    #region == yuantong

                    List<object> expressInfo = ExpressHelper.GetExpressInfoByNum(orderInfo.ExpressCompanyAbb, orderInfo.ShipOrderNumber);
                    if (expressInfo.Count > 0)
                    {

                        TrackingDataListItem item = null;
                        foreach (object express in expressInfo)
                        {
                            item = new TrackingDataListItem();
                            item.Time = ExpressHelper.GetPropertyValue(express, "time").ToString();
                            item.Content = ExpressHelper.GetPropertyValue(express, "context").ToString();
                            trackingResult.TrackingItems.Add(item);
                        }
                    }
                    trackingResult.TrackingItems.Sort(delegate(TrackingDataListItem v1, TrackingDataListItem v2) { return DateTime.Parse(v2.Time).CompareTo(DateTime.Parse(v1.Time)); });


                    #endregion

                    #region ==kuaidi100
                    //string expressData = Express.GetExpressData(orderInfo.ExpressCompanyAbb, orderInfo.ShipOrderNumber);

                    //try
                    //{
                    //    TrackingDataListItem item = null;

                    //    Kuaidi100ExpressJson kuaidi100Json = JsonConvert.DeserializeObject<Kuaidi100ExpressJson>(expressData);

                    //    if (kuaidi100Json != null)
                    //    {
                    //        foreach (var current in kuaidi100Json.data)
                    //        {
                    //            item = new TrackingDataListItem();

                    //            item.Time = current.time;
                    //            item.Content = current.context;

                    //            trackingResult.TrackingItems.Add(item);
                    //        }
                    //    }
                    //}
                    //catch (Exception ex)
                    //{

                    //}
                    #endregion

                }
            }

            return base.JsonActionResult(new StandardResult<LogisticsTrackingResult>()
            {
                code = 0,
                msg = "",
                data = trackingResult
            });
        }

        [HttpGet]
        public IHttpActionResult UserCoupons(string userId, decimal amount, string accessToken, int channel, int platform, string ver)
        {
            Logger.WriterLogger("Order.MyReturns, Params: " + string.Format("userId={0}&accessToken={1}&channel={2}&platform={3}&ver={4}&amount={5}", userId, accessToken, channel, platform, ver, amount), LoggerType.Info);

            // 保存访问信息
            base.SaveVisitInfo(userId, channel, platform, ver);

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), "Order.UserCoupon");
            }

            List<UseCouponListItem> items = new List<UseCouponListItem>();

            Member member = GetMember(userId.ToSeesionId());

            if (member != null)
            {
                DataTable userCoupons = EcShop.SaleSystem.Shopping.ShoppingProcessor.UseCoupon(member.UserId, amount);

                if (userCoupons != null)
                {
                    UseCouponListItem item = null;

                    foreach (DataRow current in userCoupons.Rows)
                    {
                        item = new UseCouponListItem();

                        item.Code = "";
                        if (current["ClaimCode"] != DBNull.Value)
                        {
                            item.Code = (string)current["ClaimCode"];
                        }

                        item.Name = "";
                        if (current["Name"] != DBNull.Value)
                        {
                            item.Name = (string)current["Name"];
                        }

                        item.Amount = 0;
                        if (current["Amount"] != DBNull.Value)
                        {
                            item.Amount = (decimal)current["Amount"];
                        }

                        item.DiscountValue = 0;
                        if (current["DiscountValue"] != DBNull.Value)
                        {
                            item.DiscountValue = (decimal)current["DiscountValue"];
                        }

                        items.Add(item);
                    }
                }
            }

            ListResult<UseCouponListItem> data = new ListResult<UseCouponListItem>();
            data.TotalNumOfRecords = items.Count; ;
            data.Results = items;

            return base.JsonActionResult(new StandardResult<ListResult<UseCouponListItem>>()
            {
                code = 0,
                msg = "",
                data = data
            });

        }

        #region 售后

        [HttpPost]
        public IHttpActionResult ApplyRefund(JObject request)
        {
            Logger.WriterLogger("Order.ApplyRefund, Params: " + request.ToString(), LoggerType.Info);

            ParamRefund param = new ParamRefund();

            try
            {
                param = request.ToObject<ParamRefund>();
            }
            catch
            {
                // 参数无效
                return base.JsonFaultResult(new CommonException(40100).GetMessage(), request.ToString());
            }

            string accessToken = param.accessToken;

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), request.ToString());
            }

            // 验证参数
            //ThrowParamException(skuId);


            int channel = param.channel;
            int platform = param.platform;
            string ver = param.ver;

            string userId = param.UserId;
            string orderId = param.OrderId;
            int refundType = param.RefundType;
            string remark = param.Remark;

            string flagMsg = "";

            try
            {
                if (string.IsNullOrEmpty(orderId) || string.IsNullOrEmpty(remark) || refundType == 0)
                {
                    return base.JsonFaultResult(new FaultInfo(40100, "传入参数错误"), request.ToString());
                }

                if (!TradeHelper.CanRefund(orderId))
                {
                    return base.JsonFaultResult(new CommonException(40311).GetMessage(), request.ToString());
                }

                if (TradeHelper.ApplyForRefund(orderId, remark, refundType, out flagMsg))
                {
                    StandardResult<String> result = new StandardResult<String>()
                    {
                        code = 0,
                        msg = "成功的申请了退款",
                        data = orderId
                    };

                    return base.JsonActionResult(result);
                }


                StandardResult<String> result1 = new StandardResult<String>()
                {
                    code = 40312,
                    msg = "申请退款失败 " + flagMsg,
                    data = request.ToString()
                };

                return base.JsonActionResult(result1);
            }
            catch (Exception ex)
            {
                Logger.WriterLogger("Order.Refund", ex, LoggerType.Error);
            }

            return base.JsonFaultResult(new CommonException(40999).GetMessage(), request.ToString());
        }

        [HttpPost]
        public IHttpActionResult ApplyReturn(JObject request)
        {
            Logger.WriterLogger("Order.ApplyReturn, Params: " + request.ToString(), LoggerType.Info);

            ParamReturn param = new ParamReturn();

            try
            {
                param = request.ToObject<ParamReturn>();
            }
            catch
            {
                // 参数无效
                return base.JsonFaultResult(new CommonException(40100).GetMessage(), request.ToString());
            }

            string accessToken = param.accessToken;

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), request.ToString());
            }

            // 验证参数
            //ThrowParamException(skuId);


            int channel = param.channel;
            int platform = param.platform;
            string ver = param.ver;

            string userId = param.UserId;
            string orderId = param.OrderId;
            int returnType = param.ReturnType;
            string remark = param.Remark;
            string expressCompany = param.ExpressCompany;
            string trackingNumber = param.TrackingNumber;
            List<ParamReturnSku> skus = param.Skus;

            try
            {
                if (string.IsNullOrEmpty(orderId) || string.IsNullOrEmpty(remark) || returnType == 0 || skus == null)
                {
                    return base.JsonFaultResult(new FaultInfo(40100, "传入参数错误"), request.ToString());
                }

                if (skus.Count == 0)
                {
                    return base.JsonFaultResult(new FaultInfo(40100, "传入参数错误：请选择商品"), request.ToString());
                }

                string skuIds = "";
                string quantityList = "";

                foreach (ParamReturnSku sku in skus)
                {
                    if (sku.Quantity <= 0)
                    {
                        return base.JsonFaultResult(new FaultInfo(40100, "传入参数错误：数量不能为空"), request.ToString());
                    }

                    skuIds = skuIds + (skuIds == "" ? "" : ",") + sku.SkuId;
                    quantityList = quantityList + (quantityList == "" ? "" : ",") + sku.Quantity.ToString();
                }

                if (string.IsNullOrEmpty(quantityList))
                {
                    return base.JsonFaultResult(new FaultInfo(40100, "传入参数错误：数量不能为空"), request.ToString());
                }

                if (!TradeHelper.CanReturn(orderId))
                {
                    return base.JsonFaultResult(new CommonException(40311).GetMessage(), request.ToString());
                }

                if (TradeHelper.CreateReturnsEntityAndAdd(orderId, remark, returnType, skuIds, quantityList, expressCompany, trackingNumber))
                {
                    StandardResult<String> result = new StandardResult<String>()
                    {
                        code = 0,
                        msg = "成功的申请了退货",
                        data = orderId
                    };

                    return base.JsonActionResult(result);
                }

                return base.JsonFaultResult(new CommonException(40313).GetMessage(), request.ToString());
            }
            catch (Exception ex)
            {
                Logger.WriterLogger("Order.Refund", ex, LoggerType.Error);
            }

            return base.JsonFaultResult(new CommonException(40999).GetMessage(), request.ToString());
        }

        /// <summary>
        /// 申请换货
        /// </summary>
        /// <param name="request">JSON参数，见文档</param>
        /// <returns>标准接口</returns>
        [HttpPost]
        public IHttpActionResult ApplyReplacement(JObject request)
        {
            Logger.WriterLogger("Order.ApplyReplacement, Params: " + request.ToString(), LoggerType.Info);

            ParamReplacement param = new ParamReplacement();

            try
            {
                param = request.ToObject<ParamReplacement>();
            }
            catch
            {
                // 参数无效
                return base.JsonFaultResult(new CommonException(40100).GetMessage(), request.ToString());
            }

            string accessToken = param.accessToken;

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), request.ToString());
            }

            // 验证参数
            //ThrowParamException(skuId);


            int channel = param.channel;
            int platform = param.platform;
            string ver = param.ver;

            string userId = param.UserId;
            string orderId = param.OrderId;
            string remark = param.Remark;
            string expressCompany = param.ExpressCompany;
            string trackingNumber = param.TrackingNumber;
            List<ParamReplacementSku> skus = param.Skus;

            try
            {
                if (string.IsNullOrEmpty(orderId) || string.IsNullOrEmpty(remark) || skus == null)
                {
                    return base.JsonFaultResult(new FaultInfo(40100, "传入参数错误"), request.ToString());
                }

                if (skus.Count == 0)
                {
                    return base.JsonFaultResult(new FaultInfo(40100, "传入参数错误：请选择商品"), request.ToString());
                }

                string skuIds = "";
                string quantityList = "";

                foreach (ParamReplacementSku sku in skus)
                {
                    if (sku.Quantity <= 0)
                    {
                        return base.JsonFaultResult(new FaultInfo(40100, "传入参数错误：数量不能为空"), request.ToString());
                    }

                    skuIds = skuIds + (skuIds == "" ? "" : ",") + sku.SkuId;
                    quantityList = quantityList + (quantityList == "" ? "" : ",") + sku.Quantity.ToString();
                }

                if (string.IsNullOrEmpty(quantityList))
                {
                    return base.JsonFaultResult(new FaultInfo(40100, "传入参数错误：数量不能为空"), request.ToString());
                }

                if (!TradeHelper.CanReplace(orderId))
                {
                    return base.JsonFaultResult(new CommonException(40311).GetMessage(), request.ToString());
                }

                if (TradeHelper.CreateReplaceEntityAndAdd(orderId, remark, skuIds, quantityList))
                {
                    StandardResult<String> result = new StandardResult<String>()
                    {
                        code = 0,
                        msg = "成功的申请了换货",
                        data = orderId
                    };

                    return base.JsonActionResult(result);
                }

                return base.JsonFaultResult(new CommonException(40314).GetMessage(), request.ToString());
            }
            catch (Exception ex)
            {
                Logger.WriterLogger("Order.Replacement", ex, LoggerType.Error);
            }

            return base.JsonFaultResult(new CommonException(40999).GetMessage(), request.ToString());
        }

        [HttpGet]
        public IHttpActionResult MyRefunds()
        {
            /// TODO
            return null;
        }

        [HttpGet]
        public IHttpActionResult MyReturns(string userId, int pageIndex, int pageSize, string accessToken, int channel, int platform, string ver)
        {
            Logger.WriterLogger("Order.MyReturns, Params: " + string.Format("userId={0}&accessToken={1}&channel={2}&platform={3}&ver={4}&pageIndex={5}&pageSize={6}", userId, accessToken, channel, platform, ver, pageIndex, pageSize), LoggerType.Info);

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), "Order.MyReturns");
            }

            // 验证参数
            //ThrowParamException(skuId);

            base.SaveVisitInfo(userId, channel, platform, ver);

            Member member = GetMember(userId.ToSeesionId());

            if (member == null)
            {
                return base.JsonFaultResult(new CommonException(40201).GetMessage(), "Order.MyReturns");
            }


            ReturnsApplyQuery returnsApplyQuery = new ReturnsApplyQuery();
            returnsApplyQuery.PageIndex = pageIndex;
            returnsApplyQuery.PageSize = pageSize;
            returnsApplyQuery.SortBy = "ApplyForTime";
            returnsApplyQuery.SortOrder = SortAction.Desc;
            returnsApplyQuery.UserId = member.UserId;

            DbQueryResult returnsApplys = TradeHelper.GetReturnsApplys(returnsApplyQuery);

            DataTable dt = returnsApplys.Data as DataTable;

            if (dt != null)
            {
                foreach (DataRow current in dt.Rows)
                {

                }
            }


            return null;
        }

        [HttpGet]
        public IHttpActionResult MyReplacements()
        {
            /// TODO
            return null;
        }

        #endregion

        #region Private

        private List<OrderReviewListItem> GetOrderReviews(string orderId, int userId)
        {
            List<OrderReviewListItem> items = new List<OrderReviewListItem>();

            DataTable productReviewAll = ProductBrowser.GetProductReviewAll(orderId, userId);
            if (productReviewAll != null)
            {
                OrderReviewListItem item = null;

                foreach (DataRow row in productReviewAll.Rows)
                {
                    int productId = (int)row["ProductId"];

                    string skuId = (string)row["SkuId"];

                    bool exist = false;

                    foreach (var current in items)
                    {
                        if (current.SkuId == skuId)
                        {
                            exist = true;
                            continue;
                        }
                    }

                    if (exist)
                    {
                        continue;
                    }

                    item = new OrderReviewListItem();

                    item.ProductId = productId;
                    item.SkuId = (string)row["SkuId"];
                    item.ProductName = (string)row["ProductName"];
                    item.SkuContent = (string)row["SkuContent"];
                    item.Quantity = (int)row["Quantity"];
                    item.Price = (decimal)row["Price"];
                    item.TaxRate = (decimal)row["TaxRate"];
                    string thumbnailsUrl = "";
                    if (row["ThumbnailsUrl"] != DBNull.Value)
                    {
                        thumbnailsUrl = Util.AppendImageHost((string)row["ThumbnailsUrl"]);
                        if (!string.IsNullOrEmpty(thumbnailsUrl))
                        {
                            thumbnailsUrl = thumbnailsUrl.Replace("40/40", "180/180");
                        }
                    }

                    item.ThumbnailsUrl = thumbnailsUrl;

                    item.Id = 0;
                    if (row["ReviewId"] != DBNull.Value)
                    {
                        item.Id = (long)row["ReviewId"];
                    }
                    item.Content = "";
                    if (row["ReviewText"] != DBNull.Value)
                    {
                        item.Content = (string)row["ReviewText"];
                    }
                    item.ReviewDate = "";
                    if (row["ReviewDate"] != DBNull.Value)
                    {
                        item.ReviewDate = ((DateTime)row["ReviewDate"]).ToString("yyyy-MM-dd HH:mm:ss");
                    }

                    item.Score = (item.Id == 0 ? 0 : 5);
                    if (row["Score"] != DBNull.Value)
                    {
                        item.Score = (int)row["Score"];
                    }

                    item.IsAnonymous = true;
                    if (row["IsAnonymous"] != DBNull.Value)
                    {
                        item.IsAnonymous = (bool)row["IsAnonymous"];
                    }

                    item.IsCanReview = item.Id == 0;

                    items.Add(item);
                }
            }

            return items;
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
        /// 提交订单
        /// </summary>
        /// <param name="member"></param>
        /// <param name="siteId"></param>
        /// <param name="storeId"></param>
        /// <param name="shippingAddressId"></param>
        /// <param name="couponCode"></param>
        /// <param name="buyId"></param>
        /// <param name="remark"></param>
        /// <param name="realName"></param>
        /// <param name="idNo"></param>
        /// <param name="buyType"></param>
        /// <param name="buyAmount"></param>
        /// <param name="skuIds"></param>
        /// <param name="orderProcessType"></param>
        /// <param name="shipToDate"></param>
        /// <param name="shippingModeId"></param>
        /// <param name="paymentTypeId"></param>
        /// <param name="isunpackOrder">是否拆单</param>
        /// <param name="orderSource">订单来源</param>
        /// <returns></returns>
        private IHttpActionResult ProcessSubmmitOrder(Member member, int siteId, int storeId, int shippingAddressId, string couponCode, int buyId,
            string remark, string realName, string idNo,
            string buyType, int buyAmount, string skuIds, int orderProcessType, string shipToDate, int shippingModeId, int paymentTypeId, bool isunpackOrder, OrderSource orderSource, int channel, int platform, string ver)
        {
            int userId = member.UserId;
            string message = "";

            //OrderSource orderSource = (OrderSource)6;

            //bool unpackOrder = orderProcessType == 1;

            bool unpackOrder = isunpackOrder;
            bool mergeOrder = orderProcessType == 2;

            string identityCard = idNo;//身份证号码


            #region 验证是否通过身份证验证
            if ((platform == 2 || platform == 3) && Util.ConvertVer(ver) >= 120)
            {
                identityCard = member.IdentityCard;
                if (member.IsVerify != 1)
                {
                    return base.JsonFaultResult(new CommonException(40400).GetMessage(), "订单提交: ");
                }
            }
            #endregion

            ShoppingCartInfo shoppingCartInfo = null;
            if (buyAmount > 0 && !string.IsNullOrEmpty(skuIds) && (buyType == "signbuy" || buyType == "groupbuy" || buyType == "countdown"))
            {
                if (buyType == "signbuy")
                {
                    shoppingCartInfo = ShoppingCartProcessor.GetShoppingCart(member, skuIds, buyAmount, storeId);
                }
                else if (buyType == "groupbuy")
                {
                    shoppingCartInfo = ShoppingCartProcessor.GetGroupBuyShoppingCart(member, skuIds, buyAmount, storeId);
                }
                else if (buyType == "countdown")
                {
                    shoppingCartInfo = ShoppingCartProcessor.GetCountDownShoppingCart(member, skuIds, buyAmount, storeId);
                }
            }
            else
            {
                if (skuIds != null && !string.IsNullOrWhiteSpace(skuIds))
                {
                    shoppingCartInfo = ShoppingCartProcessor.GetShoppingCartInfoBySkus(member, skuIds);//获取用户选择的商品
                }
                else
                {
                    shoppingCartInfo = ShoppingCartProcessor.GetShoppingCart(member);
                }
                // shoppingCartInfo = ShoppingCartProcessor.GetShoppingCart(member);
            }


            if (shoppingCartInfo == null)
            {
                message = "购物车为空";
                return base.JsonFaultResult(new CommonException(40305).GetMessage(), "订单提交: ");
            }

            #region 限时抢购数量验证
            if (buyType.ToLower() == "countdown")
            {
                CountDownInfo countDownInfo = ProductBrowser.GetCountDownInfo(shoppingCartInfo.LineItems[0].ProductId);
                if (countDownInfo.EndDate < System.DateTime.Now)
                {
                    StandardResult<string> result = new StandardResult<string>()
                    {
                        code = 1,
                        msg = "此订单为抢购订单，但抢购时间已到！",
                        data = null
                    };

                    return base.JsonActionResult(result);

                }

                //限购数量为0表示限购数量不限制
                if (countDownInfo.MaxCount > 0)
                {
                    if (shoppingCartInfo.LineItems[0].Quantity > countDownInfo.MaxCount)
                    {
                        StandardResult<string> result = new StandardResult<string>()
                        {
                            code = 1,
                            msg = "你购买的数量超过限购数量:" + countDownInfo.MaxCount.ToString(),
                            data = null
                        };

                        return base.JsonActionResult(result);


                    }
                    int num = ShoppingProcessor.CountDownOrderCount(shoppingCartInfo.LineItems[0].ProductId, member.UserId, countDownInfo.CountDownId);
                    if (num + shoppingCartInfo.LineItems[0].Quantity > countDownInfo.MaxCount)
                    {
                        StandardResult<string> result = new StandardResult<string>()
                        {
                            code = 1,
                            msg = string.Format("你已经抢购过该商品{0}件，每个用户只允许抢购{1}件,如果你有未付款的抢购单，请及时支付！", num, countDownInfo.MaxCount),
                            data = null
                        };
                        return base.JsonActionResult(result);

                    }
                }

                int num2 = ShoppingProcessor.AllCountDownOrderCount(shoppingCartInfo.LineItems[0].ProductId, countDownInfo.CountDownId);
                if (num2 + shoppingCartInfo.LineItems[0].Quantity > countDownInfo.PlanCount)
                {
                    StandardResult<string> result = new StandardResult<string>()
                    {
                        code = 1,
                        msg = "该商品活动数量不足",
                        data = null
                    };
                    return base.JsonActionResult(result);
                }
            }

            #endregion


            if (buyType.ToLower() == "countdown" || buyType.ToLower() == "groupbuy")
            {
                if (!string.IsNullOrEmpty(couponCode))
                {
                    StandardResult<string> result = new StandardResult<string>()
                    {
                        code = 1,
                        msg = "此订单含有限购商品，不能使用优惠券！",
                        data = null
                    };

                    return base.JsonActionResult(result);
                }
            }

            #region 判断是否是特定二级类型的商品使用了优惠券
            string CurrentCategoryDesc = ShoppingProcessor.GetCurrentCategoryDesc(shoppingCartInfo, couponCode, userId);

            if (!string.IsNullOrWhiteSpace(CurrentCategoryDesc))
            {
                StandardResult<string> result = new StandardResult<string>()
                {
                    code = 1,
                    msg = CurrentCategoryDesc,
                    data = null
                };
                return base.JsonActionResult(result);
            }
            #endregion

            //unpackOrder = false;
            int totalQty = 0;
            decimal totalTax = 0M;
            int hasTaxQty = 0;

            List<StockoutListItem> buyCardinalityItems = new List<StockoutListItem>();


            #region  添加是否组合商品，且税率是否不同的判断

            if ((platform == 2 || platform == 3) && Util.ConvertVer(ver) < 110)
            {
                foreach (ShoppingCartItemInfo item in shoppingCartInfo.LineItems)
                {
                    List<ProductsCombination> list = new List<ProductsCombination>();
                    list = ProductHelper.GetProductsCombinationsBySku(item.SkuId);
                    if (list.Count > 0)
                    {
                        decimal itemtotalprice = 0M;
                        decimal itemtotalTax = 0M;
                        for (int i = 0; i < list.Count; i++)
                        {

                            itemtotalprice += list[i].Price * list[i].Quantity;
                            itemtotalTax += list[i].Price * list[i].Quantity * list[i].TaxRate;
                        }

                        if (itemtotalprice > 0)
                        {
                            decimal itemtaxrate = itemtotalTax / itemtotalprice;

                            decimal itemconverttaxrate = Math.Round(itemtaxrate, 2);

                            if (itemtaxrate != itemconverttaxrate)
                            {
                                return base.JsonActionResult(new StandardResult<string>()
                                {
                                    code = 1,
                                    msg = "存在捆绑商品,请更新最新版APP",
                                    data = null
                                });
                            }


                        }
                    }
                }
            }
            #endregion


            foreach (ShoppingCartItemInfo item in shoppingCartInfo.LineItems)
            {
                totalQty += item.Quantity;
                totalTax += item.AdjustedPrice * item.TaxRate * item.Quantity;

                if (item.TaxRate > 0M)
                {
                    hasTaxQty += item.Quantity;
                }

                // 如果购买数量不是购买基数的倍数据，则记录
                if (item.BuyCardinality > 0)
                {
                    if (item.Quantity < item.BuyCardinality)
                    {
                        buyCardinalityItems.Add(new StockoutListItem() { SkuId = item.SkuId, Stock = 0, ProductName = item.Name.Replace("\r\n", "") });
                    }
                }
            }

            if (buyCardinalityItems.Count > 0)    //40317 商品不符合购买条件（购买基数）
            {
                return base.JsonActionResult(new StandardResult<ListResult<StockoutListItem>>()
                {
                    code = 40317,
                    msg = string.Format("有{0}个商品不符合购买基数条件", buyCardinalityItems.Count),
                    data = new ListResult<StockoutListItem>()
                    {
                        TotalNumOfRecords = buyCardinalityItems.Count,
                        Results = buyCardinalityItems
                    }
                });
            }

            //if (totalQty > 1 && hasTaxQty > 1)
            //{
            //    unpackOrder = true;
            //}

            OrderInfo orderInfo = ShoppingProcessor.ConvertShoppingCartToOrder(shoppingCartInfo, false, false, false, userId);
            if (orderInfo != null)
            {
                orderInfo.OrderId = this.GenerateOrderId();
                orderInfo.OrderDate = System.DateTime.Now;
                orderInfo.OrderSource = orderSource; //订单来源
                orderInfo.UserId = member.UserId;
                orderInfo.Username = member.Username;
                orderInfo.EmailAddress = member.Email;
                orderInfo.RealName = member.RealName;
                orderInfo.QQ = member.QQ;
                orderInfo.Remark = remark;
                //orderInfo.RealName = realName;
                orderInfo.SiteId = siteId;
                orderInfo.IdentityCard = identityCard;//收货人身份证号码
                if (buyId > 0)
                {
                    if (buyType == "groupbuy")
                    {
                        GroupBuyInfo groupBuy = ProductBrowser.GetGroupBuy(buyId);
                        orderInfo.GroupBuyId = buyId;
                        orderInfo.NeedPrice = groupBuy.NeedPrice;
                        orderInfo.GroupBuyStatus = groupBuy.Status;
                    }
                    else if (buyType == "countdown")
                    {
                        orderInfo.CountDownBuyId = buyId;
                    }

                }
                orderInfo.OrderStatus = OrderStatus.WaitBuyerPay;
                orderInfo.RefundStatus = RefundStatus.None;
                orderInfo.ShipToDate = shipToDate;

                ShippingAddressInfo shippingAddress = EcShop.SaleSystem.Member.MemberProcessor.GetShippingAddress(shippingAddressId);

                if (shippingAddress != null)
                {
                    #region 验证每人每日最多消费1000元，1000元以上为单件商品

                    int mayCount = 0;

                    List<StockoutListItem> stockoutItems = new List<StockoutListItem>();
                    List<StockoutListItem> CheckItems = new List<StockoutListItem>();

                    foreach (ShoppingCartItemInfo item in shoppingCartInfo.LineItems)
                    {
                        mayCount += item.Quantity;

                        //#region 验证库存
                        int stock = ShoppingProcessor.GetProductStock(item.SkuId);

                        if (stock <= 0 || stock < item.Quantity)
                        {
                            ////40301 库存不足
                            //message = item.Name.Replace("\r\n", "");
                            //return base.JsonFaultResult(new CommonException(40301).GetMessage(), "订单提交: " + message);

                            stockoutItems.Add(new StockoutListItem() { SkuId = item.SkuId, Stock = stock, ProductName = item.Name.Replace("\r\n", "") });
                        }

                        int MaxCount = 0;
                        int count = ProductHelper.CheckPurchaseCount(item.SkuId, member.UserId, out MaxCount);
                        if ((count + item.Quantity) > MaxCount && MaxCount != 0) //当前购买数量大于限购剩余购买数量
                        {
                            //CheckItems.Add(new StockoutListItem() { SkuId = item.SkuId, Stock = ((MaxCount - count) < 0 ? 0 : MaxCount - count), ProductName = item.Name.Replace("\r\n", "") });

                            CheckItems.Add(new StockoutListItem() { SkuId = item.SkuId, Stock = MaxCount, ProductName = item.Name.Replace("\r\n", "") });

                        }
                    }
                    if (CheckItems.Count > 0)
                    {
                        return base.JsonActionResult(new StandardResult<ListResult<StockoutListItem>>()
                        {
                            code = 1001,
                            msg = "超过限购数",
                            data = new ListResult<StockoutListItem>()
                            {
                                TotalNumOfRecords = stockoutItems.Count,
                                Results = CheckItems
                            }
                        });
                    }
                    if (stockoutItems.Count > 0)    //40301 库存不足
                    {

                        return base.JsonActionResult(new StandardResult<ListResult<StockoutListItem>>()
                        {
                            code = 40301,
                            msg = "库存不足",
                            data = new ListResult<StockoutListItem>()
                            {
                                TotalNumOfRecords = stockoutItems.Count,
                                Results = stockoutItems
                            }
                        });
                    }

                    #endregion
                    if (!isunpackOrder)
                    {
                        if (mayCount > 1)
                        {
                            decimal money = orderInfo.GetAmount();
                            if (money > 1000)
                            {
                                return base.JsonActionResult(new StandardResult<string>()
                                {
                                    code = 40317,
                                    msg = "抱歉，您已超过海关限额￥1000，请确认拆单或者分次购买。",
                                    data = null
                                });
                            }
                        }
                    }


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
                            // 40302 有需要清关的商品，身份证号码不能为空
                            message = "有需要清关的商品，身份证号码不能为空";
                            return base.JsonFaultResult(new CommonException(40302).GetMessage(), "订单提交: ");
                        }
                    }
                    else
                    {
                        orderInfo.IsCustomsClearance = 0;
                    }


                    #endregion

                    orderInfo.ShippingModeId = shippingModeId;
                    ShippingModeInfo shippingMode = ShoppingProcessor.GetShippingMode(shippingModeId);
                    if (shippingMode != null)
                    {
                        orderInfo.ModeName = shippingMode.Name;
                    }

                    orderInfo.ShippingRegion = RegionHelper.GetFullRegion(shippingAddress.RegionId, "，");
                    orderInfo.RegionId = shippingAddress.RegionId;
                    orderInfo.Address = shippingAddress.Address;
                    orderInfo.ZipCode = shippingAddress.Zipcode;
                    orderInfo.ShipTo = shippingAddress.ShipTo;
                    orderInfo.TelPhone = shippingAddress.TelPhone;
                    orderInfo.CellPhone = shippingAddress.CellPhone;
                    orderInfo.ShippingId = shippingAddress.ShippingId;//新增地址ShippingId

                    EcShop.SaleSystem.Member.MemberProcessor.SetDefaultShippingAddress(shippingAddressId, userId);

                    if (shippingAddress.IdentityCard != identityCard)
                    {
                        shippingAddress.IdentityCard = identityCard;
                        EcShop.SaleSystem.Member.MemberProcessor.UpdateShippingAddress(shippingAddress);
                    }
                }
                else
                {
                    // 40303 收货地址不能为空
                    message = "收货地址不能为空";
                    return base.JsonFaultResult(new CommonException(40303).GetMessage(), "订单提交: ");
                }

                //订单确认不再出现配送方式,运费改为按商品的运费模版
                //if (int.TryParse(context.Request["shippingType"], out modeId))
                //{
                //ShippingModeInfo shippingMode = ShoppingProcessor.GetShippingMode(modeId, true);
                //if (shippingMode != null)
                //{
                orderInfo.ShippingModeId = -1;
                orderInfo.ModeName = "";
                decimal tax = 0m;
                decimal freight = 0m;
                decimal productAmount = 0m;
                int totalGoodsCount = 0;
                //
                HashSet<int> hsSupplierId = new HashSet<int>();
                HashSet<int> hsTemplateId = new HashSet<int>();
                Dictionary<int, decimal> dictShippingMode = new Dictionary<int, decimal>();
                if (shoppingCartInfo.LineItems.Count != shoppingCartInfo.LineItems.Count((ShoppingCartItemInfo a) => a.IsfreeShipping) && !shoppingCartInfo.IsFreightFree)
                {
                    //orderInfo.AdjustedFreight = (orderInfo.Freight = ShoppingProcessor.CalcFreight(orderInfo.RegionId, shoppingCartInfo.Weight, shippingMode));                
                    foreach (ShoppingCartItemInfo item in shoppingCartInfo.LineItems)
                    {
                        //totalGoodsCount += item.Quantity;
                        productAmount += item.AdjustedPrice * item.Quantity;
                        //tax += item.AdjustedPrice * item.TaxRate * item.Quantity;
                        if ((!item.IsfreeShipping || buyId == 0))
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
                        hsSupplierId.Add(item.SupplierId);//商品供应商集合
                        hsTemplateId.Add(item.TemplateId);//运费模版Id
                    }
                    foreach (var item in dictShippingMode)
                    {
                        ShippingModeInfo shippingMode = ShoppingProcessor.GetShippingMode(item.Key);
                        freight += ShoppingProcessor.CalcFreight(shippingAddress.RegionId, item.Value, shippingMode);
                    }
                    orderInfo.AdjustedFreight = (orderInfo.Freight = freight);
                    //orderInfo.Tax = tax <= 50 ? 0 : tax;
                    //orderInfo.OriginalTax = tax;
                }
                else
                {
                    orderInfo.AdjustedFreight = (orderInfo.Freight = 0m);
                }


                #region 计算税费
                foreach (ShoppingCartItemInfo item in shoppingCartInfo.LineItems)
                {
                    totalGoodsCount += item.Quantity;
                    //tax += item.AdjustedPrice * item.TaxRate * item.Quantity;
                }

                tax = shoppingCartInfo.GetNewTotalTax();
                orderInfo.Tax = tax <= 50 ? 0 : tax;
                orderInfo.OriginalTax = tax;
                #endregion


                //}
                //}

                // 检查购买限制，性能原因
                //totalGoodsCount = 0;
                //Dictionary<int, Dictionary<int, int>> dictCheck = new Dictionary<int, Dictionary<int, int>>();

                //foreach (ShoppingCartItemInfo item in shoppingCartInfo.LineItems)
                //{
                //    totalGoodsCount += item.Quantity;

                //    int supplierId = item.SupplierId;
                //    string skuId = item.SkuId;
                //    int templateid = item.TemplateId;

                //    if (dictCheck.ContainsKey(supplierId))
                //    {

                //    }

                //}

                // 如果每个供货商的每一个运费模板下仅有一个商品，则数量不限
                // 40304 购买数量不能超过30个
                //if (totalGoodsCount > 30)
                //{

                //    message = "购买数量不能超过30个";
                //    return base.JsonFaultResult(new CommonException(40304).GetMessage(), "订单提交: ");
                //}

                #region 付款方式
                orderInfo.PaymentTypeId = paymentTypeId;
                if (paymentTypeId == 0)
                {
                    orderInfo.PaymentType = "货到付款";
                    orderInfo.Gateway = "Ecdev.plugins.payment.podrequest";
                }
                else
                {
                    if (paymentTypeId == -2)
                    {
                        orderInfo.PaymentType = "微信支付";
                        orderInfo.Gateway = "Ecdev.plugins.payment.weixinrequest";
                    }
                    else
                    {
                        if (paymentTypeId == -1)
                        {
                            orderInfo.PaymentType = "线下付款";
                            orderInfo.Gateway = "Ecdev.plugins.payment.bankrequest";
                        }
                        else
                        {
                            if (paymentTypeId == -3)
                            {
                                orderInfo.PaymentType = "支付宝手机应用内支付";
                                orderInfo.Gateway = "Ecdev.plugins.payment.ws_apppay.wswappayrequest";
                            }
                            else
                            {
                                if (paymentTypeId == -4)
                                {
                                    orderInfo.PaymentType = "支付宝手机网页支付";
                                    orderInfo.Gateway = "Ecdev.plugins.payment.ws_wappay.wswappayrequest";
                                }
                                else
                                {
                                    if (paymentTypeId == -5)
                                    {
                                        orderInfo.PaymentType = "盛付通手机网页支付";
                                        orderInfo.Gateway = "Ecdev.Plugins.Payment.ShengPayMobile.ShengPayMobileRequest";
                                    }
                                    else
                                    {
                                        if (paymentTypeId == -6)
                                        {
                                            orderInfo.PaymentType = "预付款帐户支付";
                                            orderInfo.Gateway = "Ecdev.plugins.payment.advancerequest";
                                        }
                                        else
                                        {
                                            PaymentModeInfo paymentMode = ShoppingProcessor.GetPaymentMode(paymentTypeId);
                                            if (paymentMode != null)
                                            {
                                                orderInfo.PaymentTypeId = paymentMode.ModeId;
                                                orderInfo.PaymentType = paymentMode.Name;
                                                orderInfo.Gateway = paymentMode.Gateway;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                #endregion



                if (!string.IsNullOrEmpty(couponCode))
                {
                    CouponInfo couponInfo = ShoppingProcessor.UseCoupon(shoppingCartInfo.GetTotal(), couponCode);
                    if (couponInfo != null)
                    {
                        orderInfo.CouponName = couponInfo.Name;
                        if (couponInfo.Amount.HasValue)
                        {
                            orderInfo.CouponAmount = couponInfo.Amount.Value;
                        }
                        orderInfo.CouponCode = couponCode;
                        orderInfo.CouponValue = couponInfo.DiscountValue;
                    }
                }

                try
                {
                    string firstOrderId = orderInfo.OrderId;
                    orderInfo.OrderType = (int)OrderType.Normal;

                    if (unpackOrder)
                    {
                        orderInfo.OrderType = (int)OrderType.WillSplit;
                    }
                    if (mergeOrder)
                    {
                        orderInfo.OrderType = (int)OrderType.WillMerge;
                    }

                    DateTime pCustomsClearanceDate = ShoppingProcessor.GetUserLastOrderPCustomsClearanceDate(orderInfo.UserId);

                    bool b = false;
                    ErrorLog.Write("Ready,Order is coming");
                    if (ShoppingProcessor.CreateOrder(orderInfo, true, true))
                    {

                        ErrorLog.Write("End,Order is over");
                        decimal unpackedTaxTotal = orderInfo.Tax;
                        decimal unpackedOrderTotal = orderInfo.GetTotal();
                        decimal unpackedFreight = orderInfo.AdjustedFreight;



                        if (unpackOrder)
                        {

                            //拆单处理

                            if ((unpackOrder && totalGoodsCount > 1 && tax > 50)
                                || (totalGoodsCount > 1) && orderInfo.GetNewAmount() > 1000)
                            {
                                unpackedTaxTotal = 0M;
                                unpackedOrderTotal = 0M;
                                unpackedFreight = 0M;

                                //b = ShoppingProcessor.UnpackOrderBySupplier(orderInfo, ref unpackedTaxTotal, ref unpackedOrderTotal, ref unpackedFreight);
                                ErrorLog.Write("Ready,ChildOrders is coming");
                                b = ShoppingProcessor.CreateChildOrders(orderInfo, ref unpackedTaxTotal, ref unpackedOrderTotal, ref unpackedFreight);
                                if (b)//修改原订单金额
                                {
                                    ShoppingProcessor.UpdateWillSplitOrder(firstOrderId, unpackedTaxTotal, unpackedOrderTotal, unpackedFreight);

                                    orderInfo = ShoppingProcessor.GetOrderInfo(firstOrderId);

                                    Messenger.OrderCreated(orderInfo, member);
                                }
                            }
                        }


                        if (!b)
                        {
                            Messenger.OrderCreated(orderInfo, member);
                        }



                        // ShoppingCartProcessor.ClearShoppingCart();
                        //有选择删除购物车商品
                        if (skuIds == null || string.IsNullOrWhiteSpace(skuIds))
                        {
                            ShoppingCartProcessor.ClearShoppingCart(member.UserId);
                        }
                        else
                        {
                            ShoppingCartProcessor.ClearPartShoppingCart(member.UserId, skuIds);
                        }

                        OrderQuery orderQuery = new OrderQuery();
                        orderQuery.Status = OrderStatus.WaitBuyerPay;
                        int userOrderCount = EcShop.SaleSystem.Member.MemberProcessor.GetUserOrderCount(userId, orderQuery);


                        ExtendOrderResult extendOrderResult = new ExtendOrderResult();
                        extendOrderResult.OrderId = firstOrderId;
                        extendOrderResult.WaitBuyerPayOrderCount = userOrderCount;
                        extendOrderResult.IsNeedPayment = (paymentTypeId != -6);
                        extendOrderResult.Amount = Math.Round(unpackedOrderTotal, 2);
                        extendOrderResult.ProductAmount = Math.Round(orderInfo.GetAmount(), 2);
                        extendOrderResult.Freight = Math.Round(unpackedFreight, 2);
                        extendOrderResult.Tax = Math.Round(unpackedTaxTotal, 2);
                        extendOrderResult.PaymentTypeId = orderInfo.PaymentTypeId;

                        foreach (var item in orderInfo.LineItems)
                        {
                            extendOrderResult.ProductNames.Add(item.Value.ItemDescription);
                        }
                        StandardResult<ExtendOrderResult> result = new StandardResult<ExtendOrderResult>()
                        {
                            code = 0,
                            msg = "订单提交成功",
                            data = extendOrderResult
                        };

                        return base.JsonActionResult(result);
                    }
                    else
                    {
                        // 40399 创建订单失败
                        message = "创建订单失败";
                        return base.JsonFaultResult(new CommonException(40399).GetMessage(), "订单提交: ");
                    }
                }
                catch (OrderException ex)
                {
                    // 40398 创建订单异常
                    message = ex.Message;
                    return base.JsonFaultResult(new CommonException(40398).GetMessage(), "订单提交: ");
                }
            }

            message = "购物车为空";
            return base.JsonFaultResult(new CommonException(40305).GetMessage(), "订单提交: ");
        }





        private bool UserPayOrder(OrderInfo order, string message)
        {
            if (order.OrderStatus == OrderStatus.BuyerAlreadyPaid)
            {
                return true;
            }

            //如果需要拆单
            if (TradeHelper.CheckIsUnpack(order.OrderId) && order.OrderStatus == OrderStatus.WaitBuyerPay)
            {
                OrderHelper.SetOrderPayStatus(order.OrderId, 2);

                Logger.WriterLogger(string.Format("拆单，原订单{0},返回信息{1}", order.OrderId, message), LoggerType.Info);

                if (order.CheckAction(OrderActions.BUYER_PAY) && TradeHelper.UserPayOrder(order, false, 1))
                {
                    if (order.UserId != 0 && order.UserId != 1100)
                    {
                        IUser user = Users.GetUser(order.UserId);
                        if (user != null && user.UserRole == UserRole.Member)
                        {
                            Messenger.OrderPayment(user, order, order.GetTotal());
                        }
                    }
                    order.OnPayment();

                    return true;
                }
            }
            else if (order.OrderType == (int)OrderType.WillMerge && order.OrderStatus == OrderStatus.WaitBuyerPay)//合并单据
            {
                OrderHelper.SetOrderPayStatus(order.OrderId, 2);

                Logger.WriterLogger(string.Format("合并单据，原订单{0},返回信息{1}", order.OrderId, message), LoggerType.Info);

                bool b = ShoppingProcessor.mergeOrder(order);
                int flag = 0;
                if (b)
                {
                    flag = 2;
                }
                if (order.CheckAction(OrderActions.BUYER_PAY) && TradeHelper.UserPayOrder(order, false, flag))
                {
                    if (order.UserId != 0 && order.UserId != 1100)
                    {
                        IUser user = Users.GetUser(order.UserId);
                        if (user != null && user.UserRole == UserRole.Member)
                        {
                            Messenger.OrderPayment(user, order, order.GetTotal());
                        }
                    }
                    order.OnPayment();

                    return true;
                }
            }
            else
            {
                OrderHelper.SetOrderPayStatus(order.OrderId, 2);

                Logger.WriterLogger(string.Format("正常单据，原订单{0},返回信息{1}", order.OrderId, message), LoggerType.Info);

                if (order.CheckAction(OrderActions.BUYER_PAY) && TradeHelper.UserPayOrder(order, false))
                {
                    if (order.UserId != 0 && order.UserId != 1100)
                    {
                        IUser user = Users.GetUser(order.UserId);
                        if (user != null && user.UserRole == UserRole.Member)
                        {
                            Messenger.OrderPayment(user, order, order.GetTotal());
                        }
                    }
                    order.OnPayment();

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 微信支付状态获取
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="paymentMode">支付方式信息</param>
        /// <param name="transactionId">支付成功时输出交易流水号</param>
        /// <returns>支付成功/失败</returns>
        private bool GetWxPayState(string orderId, PaymentModeInfo paymentMode, out string transactionId)
        {
            string appId = "";
            string partnerKey = "";
            string appSecret = "";
            string partnerId = "";
            transactionId = "";

            if (paymentMode != null)
            {
                if (paymentMode.Settings != "")
                {
                    try
                    {
                        string xml = HiCryptographer.Decrypt(paymentMode.Settings);
                        Logger.WriterLogger("微信App支付信息设置：" + xml);

                        System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
                        xmlDocument.LoadXml(xml);
                        //<xml><AppId>{0}</AppId><Key>{1}</Key><AppSecret>{2}</AppSecret><Mch_id>{3}</Mch_id></xml>
                        appId = xmlDocument.GetElementsByTagName("AppId")[0].InnerText;
                        partnerKey = xmlDocument.GetElementsByTagName("Key")[0].InnerText;
                        appSecret = xmlDocument.GetElementsByTagName("AppSecret")[0].InnerText;
                        partnerId = xmlDocument.GetElementsByTagName("Mch_id")[0].InnerText;
                    }
                    catch (Exception ex)
                    {
                        Logger.WriterLogger("微信App支付信息未设置：" + ex.Message);

                        return false;
                    }
                }
                else
                {
                    Logger.WriterLogger("支付信息未设置");
                    return false;
                }
            }

            Dictionary<string, string> dic = new Dictionary<string, string>();

            dic.Add("appid", appId);
            dic.Add("mch_id", partnerId);
            dic.Add("out_trade_no", orderId);
            dic.Add("nonce_str", VCodePayHelper.CreateRandom(20));
            VCodePayHelper.key = partnerKey;

            //微信支付订单号

            string state = VCodePayHelper.GetOrderState(dic, null, out transactionId);
            if (state == "SUCCESS")
            {
                return true;
            }
            else if (state == "NOTPAY")
            {
                dic.Clear();

            }
            else if (state == "CLOSED")
            {
                dic.Clear();

            }

            return false;
        }

        /// <summary>
        /// 根据platform获取订单来源
        /// </summary>
        /// <param name="platform"></param>
        /// <returns></returns>
        private OrderSource CovertToOrderSource(int platform)
        {
            OrderSource ordersource = OrderSource.PC;
            switch (platform)
            {
                case 1:
                    ordersource = OrderSource.PC;
                    break;
                case 2:
                    ordersource = OrderSource.IOS;
                    break;
                case 3:
                    ordersource = OrderSource.Android;
                    break;
                case 4:
                    ordersource = OrderSource.Wap;
                    break;
                default:
                    ordersource = OrderSource.PC;
                    break;
            }

            return ordersource;
        }
        #endregion

    }
}
