using EcShop.Core;
using EcShop.Entities.Promotions;
using EcShop.SaleSystem.Catalog;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.Tags
{
	public class ProductPromote : WebControl
	{
		public const string TagID = "ProductPromote";
		public override string ID
		{
			get
			{
				return base.ID;
			}
			set
			{
			}
		}
		public int ProductId
		{
			get;
			set;
		}
		public bool IsAnonymous
		{
			get;
			set;
		}
		public ProductPromote()
		{
			base.ID = "ProductPromote";
		}

        /// <summary>
        /// 是否为购物车中调用该控件
        /// </summary>
        private bool isShoppingCart = false;
        /// <summary>
        /// 是否为购物车中调用该控件
        /// </summary>
        public bool IsShoppingCart
        {
            get { return isShoppingCart; }
            set { isShoppingCart = value; }
        }

        private bool isVshop = false;
        /// <summary>
        /// 是否为微信端调用，true,false为PC端调用
        /// </summary>
        public bool IsVshop
        {
            get { return isVshop; }
            set { isVshop = value; }
        }

        private bool isVProductList = false;
        /// <summary>
        /// 是否微商城列表显示
        /// </summary>
        public bool IsVProductList
        {
            get { return isVProductList; }
            set { isVProductList = value; }
        }


        private bool isVCartList = false;
        /// <summary>
        /// 是否微商城提交订单列表显示
        /// </summary>
        public bool IsVCartList
        {
            get { return isVCartList; }
            set { isVCartList = value; }
        }


		protected override void Render(HtmlTextWriter writer)
		{
			PromotionInfo promotionInfo;
			if (this.IsAnonymous)
			{
				promotionInfo = ProductBrowser.GetAllProductPromotionInfo(this.ProductId);
			}
			else
			{
				promotionInfo = ProductBrowser.GetProductPromotionInfo(this.ProductId);
			}
			string value = string.Empty;
			if (promotionInfo != null)
			{
				string arg = Globals.GetSiteUrls().UrlData.FormatUrl("FavourableDetails", new object[]
				{
					promotionInfo.ActivityId
				});

                 //判断是否为购物车控件调用 true：是
                if (!isShoppingCart)
                {
                    switch (promotionInfo.PromoteType)
                    {
                        case PromoteType.Discount:
                            value = string.Format("<div>促销信息：<span class=\"promotiontype\">直接打折</span><span class=\"promotionname\">{0}</span></div>", promotionInfo.Name);
                            //PC端调用
                            if (!IsVshop)
                            {
                                value = string.Format("<dt>促销信息 </dt><dd><span class=\"promotiontype\">直接打折</span><span class=\"promotionname\">{0}</span></dd>    ", promotionInfo.Name);
                            }
                            if (IsVProductList)
                            {
                                value = "<div class='promoteImg'>促</div>";
                            }
                            break;
                        case PromoteType.Amount:
                            value = string.Format("<div>促销信息：<span class=\"promotiontype\">固定金额出售</span><span class=\"promotionname\">{0}</span></div>", promotionInfo.Name);
                            //PC端调用
                            if (!IsVshop)
                            {
                                value = string.Format("<dt>促销信息 </dt><dd><span class=\"promotiontype\">固定金额出售</span><span class=\"promotionname\">{0}</span></dd>    ", promotionInfo.Name);
                            }
                            if (IsVProductList)
                            {
                                value = "<div class='promoteImg'>促</div>";
                            }
                            break;
                        case PromoteType.Reduced:
                            value = string.Format("<div>促销信息：<span class=\"promotiontype\">减价优惠</span><span class=\"promotionname\">{0}</span></div>", promotionInfo.Name);
                            if (!IsVshop)
                            {
                                value = string.Format("<dt>促销信息 </dt><dd><span class=\"promotiontype\">减价优惠</span><span class=\"promotionname\">{0}</span></dd>", promotionInfo.Name);
                            }
                            if (IsVProductList)
                            {
                                value = "<div class='promoteImg'>促</div>";
                            }
                            break;
                        case PromoteType.QuantityDiscount:
                            value = string.Format("<div>促销信息：<span class=\"promotiontype\">批发打折</span><span class=\"promotionname\">{0}</span></div>", promotionInfo.Name);
                            if (!IsVshop)
                            {
                                value = string.Format("<dt>促销信息 </dt><dd><span class=\"promotiontype\">批发打折</span><span class=\"promotionname\">{0}</span></dd>", promotionInfo.Name);
                            }
                            if (IsVProductList)
                            {
                                value = "<div class='promoteImg'>促</div>";
                            }
                            break;
                        case PromoteType.SentGift:
                            {
                                GiftInfo gift = ProductBrowser.GetGift(promotionInfo.GiftID);
                                if (gift != null)
                                {
                                    value = string.Format("<div>促销信息：<span class=\"promotiontype\">赠品</span><span class=\"promotionname\">{1}</span></div>",gift.Name);
                                    if (!IsVshop)
                                    {
                                        value = string.Format("<dt>促销信息 </dt><dd><span class=\"promotiontype\">赠品</span><span class=\"promotionname\">{1}</span></dd>",gift.Name);
                                    }
                                    if (IsVProductList)
                                    {
                                        value = "<div class='promoteImg'>促</div>";
                                    }
                                }
                                else
                                {
                                    value = string.Format("<div>促销信息：<span class=\"promotiontype\">买商品送礼品</span><span class=\"promotionname\">{0}</span></div>", promotionInfo.Name);
                                    if (!IsVshop)
                                    {
                                        value = string.Format("<dt>促销信息 </dt><dd><span class=\"promotiontype\">买商品送礼品</span><span class=\"promotionname\">{0}</span></dd>", promotionInfo.Name);
                                    }
                                    if (IsVProductList)
                                    {
                                        value = "<div class='promoteImg'>促</div>";
                                    }
                                }
                                break;
                            }
                        case PromoteType.SentProduct:
                            value = string.Format("<div>促销信息：<span class=\"promotiontype\">有买有送</span><span class=\"promotionname\">{0}</span></div>", promotionInfo.Name);
                            if (!IsVshop)
                            {
                                value = string.Format("<dt>促销信息 </dt><dd><span class=\"promotiontype\">有买有送</span><span class=\"promotionname\">{0}</span></dd>", promotionInfo.Name);
                            }
                            if (IsVProductList)
                            {
                                value = "<div class='promoteImg'>促</div>";
                            }
                            break;
                        case PromoteType.ProductPromotion:
                            value = string.Format("<div>促销信息：<span class=\"promotiontype\">单品满减</span><span class=\"promotionname\">{0}</span></div>", promotionInfo.Name);
                            if (!IsVshop)
                            {
                                value = string.Format("<dt>促销信息 </dt><dd><span class=\"promotiontype\">单品满减</span><span class=\"promotionname\">{0}</span></dd>", promotionInfo.Name);
                            }
                            if (IsVProductList)
                            {
                                value = "<div class='promoteImg'>促</div>";
                            }
                            break;
                        case PromoteType.PresentProduct:
                            value = string.Format("<div>促销信息：<span class=\"promotiontype\">有买有送(选择商品)</span><span class=\"promotionname\">{0}</span></div>", promotionInfo.Name);
                            if (!IsVshop)
                            {
                                value = string.Format("<dt>促销信息 </dt><dd><span class=\"promotiontype\">有买有送(选择商品)</span><span class=\"promotionname\">{0}</span></dd>", promotionInfo.Name);
                            }
                            if (IsVProductList)
                            {
                                value = "<div class='promoteImg'>促</div>";
                            }
                            break;
                        case PromoteType.SecondReducePrice:
                            value = string.Format("<div>促销信息：<span class=\"promotiontype\">第二件减价</span><span class=\"promotionname\">{0}</span></div>", promotionInfo.Name);
                            if (!IsVshop)
                            {
                                value = string.Format("<dt>促销信息 </dt><dd><span class=\"promotiontype\">第二件减价</span><span class=\"promotionname\">{0}</span></dd>", promotionInfo.Name);
                            }
                            if (IsVProductList)
                            {
                                value = "<div class='promoteImg'>促</div>";
                            }
                            break;
                        case PromoteType.ProductDiscount:
                            value = string.Format("<div>促销信息：<span class=\"promotiontype\">第二件打折</span><span class=\"promotionname\">{0}</span></div>", promotionInfo.Name);
                            if (!IsVshop)
                            {
                                value = string.Format("<dt>促销信息 </dt><dd><span class=\"promotiontype\">第二件打折</span><span class=\"promotionname\">{0}</span></dd>", promotionInfo.Name);
                            }
                            if (IsVProductList)
                            {
                                value = "<div class='promoteImg'>促</div>";
                            }
                            break;
                    }
                    writer.Write(value);
                }
                else
                {
                    if (promotionInfo.PromoteType != PromoteType.PresentProduct)
                    {
                        if (IsVCartList)
                        {
                            //微信购物车显示
                            value = string.Format("<span class=\"activityname\">{0}</span>", promotionInfo.Name);
                        }
                        else
                        {
                            value = string.Format("<div class=\"promotionname\">{0}</div>", promotionInfo.Name);
                        }
                        writer.Write(value);
                    }
                }

			}
		}
	}
}
