using EcShop.Entities.Promotions;
using EcShop.Membership.Context;
using System;
using System.Collections.Generic;
namespace EcShop.Entities.Sales
{
    public class ShoppingCartInfo
    {
        private bool isSendGift;
        private decimal timesPoint = 1m;
        private IList<ShoppingCartItemInfo> lineItems;
        private IList<ShoppingCartGiftInfo> lineGifts;
        private IList<ShoppingCartPresentInfo> linePresentPro;
        public int ReducedPromotionId
        {
            get;
            set;
        }
        public string ReducedPromotionName
        {
            get;
            set;
        }
        public decimal ReducedPromotionAmount
        {
            get;
            set;
        }
        public bool IsReduced
        {
            get;
            set;
        }
        public int SendGiftPromotionId
        {
            get;
            set;
        }
        public string SendGiftPromotionName
        {
            get;
            set;
        }
        public bool IsSendGift
        {
            get
            {
                bool result;
                if (this.lineItems == null || this.lineItems.Count == 0)
                {
                    result = false;
                }
                else
                {
                    foreach (ShoppingCartItemInfo current in this.lineItems)
                    {
                        if (current.IsSendGift)
                        {
                            result = true;
                            return result;
                        }
                    }
                    result = this.isSendGift;
                }
                return result;
            }
            set
            {
                this.isSendGift = value;
            }
        }
        public int SentTimesPointPromotionId
        {
            get;
            set;
        }
        public string SentTimesPointPromotionName
        {
            get;
            set;
        }
        public bool IsSendTimesPoint
        {
            get;
            set;
        }
        public decimal TimesPoint
        {
            get
            {
                return this.timesPoint;
            }
            set
            {
                this.timesPoint = value;
            }
        }
        public int FreightFreePromotionId
        {
            get;
            set;
        }
        public string FreightFreePromotionName
        {
            get;
            set;
        }
        public bool IsFreightFree
        {
            get;
            set;
        }

        public IList<ShoppingCartItemInfo> LineItems
        {
            get
            {
                if (this.lineItems == null)
                {
                    this.lineItems = new List<ShoppingCartItemInfo>();
                }
                return this.lineItems;
            }
        }
        public IList<ShoppingCartGiftInfo> LineGifts
        {
            get
            {
                if (this.lineGifts == null)
                {
                    this.lineGifts = new List<ShoppingCartGiftInfo>();
                }
                return this.lineGifts;
            }
        }

        public IList<ShoppingCartPresentInfo> LinePresentPro
        {
            get
            {
                if (this.linePresentPro == null)
                {
                    this.linePresentPro = new List<ShoppingCartPresentInfo>();
                }
                return this.linePresentPro;
            }
            set
            {
                this.linePresentPro = value;
            }
        }
        public decimal Weight
        {
            get
            {
                decimal num = 0m;
                decimal result;
                if (this.lineItems == null || this.lineItems.Count == 0)
                {
                    result = num;
                }
                else
                {
                    foreach (ShoppingCartItemInfo current in this.lineItems)
                    {
                        if (!current.IsfreeShipping)
                        {
                            num += current.GetSubWeight();
                        }
                    }
                    result = num;
                }
                return result;
            }
        }
        public decimal TotalWeight
        {
            get
            {
                decimal num = 0m;
                decimal result;
                if (this.lineItems == null || this.lineItems.Count == 0)
                {
                    result = num;
                }
                else
                {
                    foreach (ShoppingCartItemInfo current in this.lineItems)
                    {
                        num += current.GetSubWeight();
                    }
                    result = num;
                }
                return result;
            }
        }
        public decimal GetTotal()
        {
            return this.GetAmount() - this.ReducedPromotionAmount;
        }

        public decimal GetNewTotal()
        {
            return this.GetNewAmount() - this.ReducedPromotionAmount;
        }

        public int GetTotalNeedPoint()
        {
            int num = 0;
            int result;
            if (this.lineItems == null || this.lineItems.Count == 0)
            {
                result = num;
            }
            else
            {
                foreach (ShoppingCartGiftInfo current in this.LineGifts)
                {
                    num += current.SubPointTotal;
                }
                result = num;
            }
            return result;
        }
        public int GetPoint()
        {
            int result = 0;
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
            if (this.GetNewTotal() * this.TimesPoint / masterSettings.PointsRate > 2147483647m)
            {
                result = 2147483647;
            }
            else
            {
                if (masterSettings.PointsRate != 0m)
                {
                    result = (int)System.Math.Round(this.GetNewTotal() * this.TimesPoint / masterSettings.PointsRate, 0);
                }
            }
            return result;
        }

        public int GetPoint(decimal money)
        {
            int result = 0;
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
            if (money * this.TimesPoint / masterSettings.PointsRate > 2147483647m)
            {
                result = 2147483647;
            }
            else
            {
                if (masterSettings.PointsRate != 0m)
                {
                    result = (int)System.Math.Round(money * this.TimesPoint / masterSettings.PointsRate, 0);
                }
            }
            return result;
        }
        public decimal GetAmount()
        {
            decimal num = 0m;
            decimal result;
            if (this.lineItems == null || this.lineItems.Count == 0)
            {
                result = num;
            }
            else
            {
                foreach (ShoppingCartItemInfo current in this.lineItems)
                {
                    num += current.SubTotal;
                }
                result = num;
            }
            return result;
        }

        public decimal GetNewAmount()
        {
            decimal num = 0m;
            decimal result;
            if (this.lineItems == null || this.lineItems.Count == 0)
            {
                result = num;
            }
            else
            {
                foreach (ShoppingCartItemInfo current in this.lineItems)
                {
                    num += current.SubNewTotal;
                }
                result = num;
            }
            return result;
        }

        public decimal GetOriginalAmount()//计算商品原价
        {
            decimal num = 0m;
            decimal result;
            if (this.lineItems == null || this.lineItems.Count == 0)
            {
                result = num;
            }
            else
            {
                foreach (ShoppingCartItemInfo current in this.lineItems)
                {
                    num += current.MemberPrice;
                }
                result = num;
            }
            return result;
        }

        public decimal GetTotalTax()//计算税费
        {
            decimal tax = 0m;
            if (lineItems != null || this.lineItems.Count > 0)
            {
                foreach (ShoppingCartItemInfo current in this.lineItems)
                {
                    tax += current.AdjustedPrice * current.TaxRate * current.Quantity;
                }
            }
            return tax;
        }

        private int singeQty = 1;

        public int SingeQty
        {
            get { return singeQty; }
            set { singeQty = value; }
        }

        public decimal GetNewTotalTax()//新计算税费
        {

            decimal tax = 0m;
            //|| 改 &&
            if (lineItems != null && this.lineItems.Count > 0)
            {
                foreach (ShoppingCartItemInfo current in this.lineItems)
                {
                    if (current.PromoteType == PromoteType.SecondReducePrice || current.PromoteType == PromoteType.ProductDiscount)
                    {
                        if (current.Quantity > 1)
                        {
                            switch (current.PromoteType)
                            {
                                //第二件减价
                                case PromoteType.SecondReducePrice:
                                    //第二件减去的价格之后的税费
                                    tax += current.PromotionPrice * current.TaxRate * SingeQty;
                                    //除第二件的其他所有的税费
                                    tax += current.AdjustedPrice * current.TaxRate * (current.Quantity - SingeQty);
                                    break;
                                //第二件折扣
                                case PromoteType.ProductDiscount:
                                    //第二件打折之后的税费
                                    tax += current.PromotionPrice * current.TaxRate * SingeQty;
                                    //除第二件的其他所有的税费
                                    tax += current.AdjustedPrice * current.TaxRate * (current.Quantity - SingeQty);
                                    break;
                            }
                        }
                        else
                        {
                            tax += current.AdjustedPrice * current.TaxRate * current.Quantity;
                        }
                    }
                    else if (current.PromoteType == PromoteType.ProductPromotion && current.PromotionPrice > 0 && current.Quantity * current.AdjustedPrice >= current.PromotionPrice)
                    {
                        if (current.Quantity > 1)
                        {
                            //第二件减去的价格之后的税费
                            tax += current.PromotionPrice * current.TaxRate * SingeQty;
                            //除第二件的其他所有的税费
                            tax += current.AdjustedPrice * current.TaxRate * (current.Quantity - SingeQty);
                        }
                        else
                        {
                            tax += current.AdjustedPrice * current.TaxRate * current.Quantity;
                        }
                    }
                    else
                    {
                        // 组合商品
                        if (current.SaleType == 2)
                        {
                            if (current.CombinationItemInfos != null && current.CombinationItemInfos.Count > 0)
                            {
                                foreach (ProductsCombination com in current.CombinationItemInfos)
                                {
                                    tax += com.Price * com.Quantity * com.TaxRate * current.Quantity;
                                }
                            }
                        }
                        else
                        {
                            tax += current.AdjustedPrice * current.TaxRate * current.Quantity;
                        }
                    }

                }
            }
            return tax;
        }
        public decimal GetTotalIncludeTax()//计算总价，包含税费，不包含运费
        {
            decimal tax = GetTotalTax();
            if (tax > 50)
                return this.GetAmount() - this.ReducedPromotionAmount + GetTotalTax();
            else
                return this.GetAmount() - this.ReducedPromotionAmount;
        }

        public decimal GetNewTotalIncludeTax()//计算总价，包含税费，不包含运费
        {
            decimal tax = GetNewTotalTax();
            if (tax > 50)
                return this.GetNewAmount() - this.ReducedPromotionAmount + GetNewTotalTax();
                //return this.GetNewAmount() - this.ReducedPromotionAmount + tax;
            else
                return this.GetNewAmount() - this.ReducedPromotionAmount;


        }


        //计算总关税
        public decimal CalTotalTax()
        {
            decimal activityReduct = 0;
            if (this.lineItems == null || this.lineItems.Count == 0)
            {
                return activityReduct;
            }
            else
            {
                foreach (ShoppingCartItemInfo current in this.lineItems)
                {
                    // 组合商品
                    if (current.SaleType == 2)
                    {
                        if (current.CombinationItemInfos != null && current.CombinationItemInfos.Count > 0)
                        {
                            foreach (ProductsCombination com in current.CombinationItemInfos)
                            {
                                activityReduct += com.Price * com.Quantity * com.TaxRate * current.Quantity;
                            }
                        }
                    }
                    else
                    {
                        activityReduct += current.SubNewTotal * current.TaxRate;
                    }
                }
            }
            return activityReduct;
        }


        //活动优惠价
        public decimal GetActivityPrice()
        {
            decimal Price = 0m;
            if (lineItems != null || this.lineItems.Count > 0)
            {
                foreach (ShoppingCartItemInfo current in this.lineItems)
                {
                    if (current.PromoteType == PromoteType.SecondReducePrice || current.PromoteType == PromoteType.ProductDiscount)
                    {
                        if (current.Quantity > 1)
                        {
                            switch (current.PromoteType)
                            {
                                case PromoteType.SecondReducePrice:
                                    Price += current.AdjustedPrice - current.PromotionPrice;
                                    break;
                                case PromoteType.ProductDiscount:
                                    Price += current.AdjustedPrice - current.PromotionPrice;
                                    break;
                            }
                        }
                    }
                    else if (current.PromoteType == PromoteType.ProductPromotion && current.PromotionPrice > 0 && current.Quantity * current.AdjustedPrice >= current.PromotionPrice)
                    {
                        Price += current.AdjustedPrice - current.PromotionPrice;
                    }
                }
            }
            return Price;
        }


        public int GetQuantity()
        {
            int num = 0;
            int result;
            if (this.lineItems == null || this.lineItems.Count == 0)
            {
                result = num;
            }
            else
            {
                foreach (ShoppingCartItemInfo current in this.lineItems)
                {
                    num += current.Quantity;
                }
                result = num;
            }
            return result;
        }

        public int UserId
        {
            get;
            set;
        }

        public int GetQuantity_Sku(string SkuId)
        {
            int num = 0;
            int result;
            if (this.lineItems == null || this.lineItems.Count == 0)
            {
                result = num;
            }
            else
            {
                foreach (ShoppingCartItemInfo current in this.lineItems)
                {
                    if (current.SkuId == SkuId)
                    {
                        num += current.Quantity;
                    }
                }
                result = num;
            }
            return result;
        }

        public int SupplierId { get; set; }

        public string SupplierName { get; set; }

        public string SupplierImageUrl { get; set; }
    }
}
