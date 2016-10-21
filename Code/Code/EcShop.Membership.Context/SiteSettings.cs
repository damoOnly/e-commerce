using EcShop.Core;
using Ecdev.Components.Validation.Validators;
using System;
using System.Globalization;
using System.Xml;
namespace EcShop.Membership.Context
{
	public class SiteSettings
	{
		public int ServiceStatus
		{
			get;
			set;
		}
		public int OpenTaobao
		{
			get;
			set;
		}
		public int OpenMobbile
		{
			get;
			set;
		}
		public int OpenAliho
		{
			get;
			set;
		}
		public int OpenWap
		{
			get;
			set;
		}
		public int OpenVstore
		{
			get;
			set;
		}
		[StringLengthValidator(1, 128, Ruleset = "ValMasterSettings", MessageTemplate = "店铺域名必须控制在128个字符以内")]
		public string SiteUrl
		{
			get;
			set;
		}
		public System.DateTime? RequestDate
		{
			get;
			set;
		}
		public System.DateTime? CreateDate
		{
			get;
			set;
		}
		public string CheckCode
		{
			get;
			set;
		}
		public bool EnableMobileClient
		{
			get;
			set;
		}
		public string MobileClientSpread
		{
			get;
			set;
		}
		public string CellPhoneUserCode
		{
			get;
			set;
		}
		public string CellPhoneToken
		{
			get;
			set;
		}
		public string ApplicationMark
		{
			get;
			set;
		}
		public string SiteToken
		{
			get;
			set;
		}
		public string SiteTime
		{
			get;
			set;
		}
		public string LogoUrl
		{
			get;
			set;
		}
		[StringLengthValidator(0, 100, Ruleset = "ValMasterSettings", MessageTemplate = "简单介绍TITLE的长度限制在100字符以内")]
		public string SiteDescription
		{
			get;
			set;
		}
		[StringLengthValidator(1, 60, Ruleset = "ValMasterSettings", MessageTemplate = "店铺名称为必填项，长度限制在60字符以内")]
		public string SiteName
		{
			get;
			set;
		}
		public string Theme
		{
			get;
			set;
		}
		public string Footer
		{
			get;
			set;
		}
		public string RegisterAgreement
		{
			get;
			set;
		}
		[StringLengthValidator(0, 160, Ruleset = "ValMasterSettings", MessageTemplate = "搜索关键字META_KEYWORDS的长度限制在160字符以内")]
		public string SearchMetaKeywords
		{
			get;
			set;
		}
		[HtmlCoding, StringLengthValidator(0, 260, Ruleset = "ValMasterSettings", MessageTemplate = "店铺描述META_DESCRIPTION的长度限制在260字符以内")]
		public string SearchMetaDescription
		{
			get;
			set;
		}
        /// <summary>
        /// 订单发送其实时间
        /// </summary>
        public string SendOrderStartTime
        {
            get;
            set;
        }
        /// <summary>
        /// 订单推送结束时间
        /// </summary>
        public string SendOrderEndTime
        {
            get;
            set;
        }
        /// <summary>
        /// 订单推送延迟时间
        /// </summary>
        public string SendOrderDay
        {
            get;
            set;
        }
        /// <summary>
        /// 订单销售情况接收地址
        /// </summary>
        public string SendOrderEmail
        {
            get;
            set;
        }
        /// <summary>
        /// 是否启动发送
        /// </summary>
        public string IsSendOrderOpen
        {
            get;
            set;
        }
        /// <summary>
        /// 设置执行时间
        /// </summary>
        /// <returns></returns>
        public string IsRunTimes
        {
            get;
            set;
        }
		public bool IsCreateFeed
		{
			get;
			set;
		}
		[StringLengthValidator(0, 60, Ruleset = "ValMasterSettings", MessageTemplate = "一淘账户名称为必填项，长度限制在60字符以内")]
		public string EtaoID
		{
			get;
			set;
		}
		public string EmailSender
		{
			get;
			set;
		}
		public string EmailSettings
		{
			get;
			set;
		}
		public bool EmailEnabled
		{
			get
			{
				return !string.IsNullOrEmpty(this.EmailSender) && !string.IsNullOrEmpty(this.EmailSettings) && this.EmailSender.Trim().Length > 0 && this.EmailSettings.Trim().Length > 0;
			}
		}
		public string SMSSender
		{
			get;
			set;
		}
		public string SMSSettings
		{
			get;
			set;
		}
		public bool SMSEnabled
		{
			get
			{
				return !string.IsNullOrEmpty(this.SMSSender) && !string.IsNullOrEmpty(this.SMSSettings) && this.SMSSender.Trim().Length > 0 && this.SMSSettings.Trim().Length > 0;
			}
		}
        public string XinGeSender
        {
            get;
            set;
        }
        /// <summary>
        /// 是否启动信鸽
        /// </summary>
        public bool XinGeEnabled
        {
            get
            {
                return !string.IsNullOrEmpty(this.XinGeSender) && !string.IsNullOrEmpty(this.XinGeSettings) && this.XinGeSender.Trim().Length > 0 && this.XinGeSettings.Trim().Length > 0;
            }
        }
        /// <summary>
        /// 信鸽配置
        /// </summary>
        public string XinGeSettings
        {
            get;
            set;
        }
		public bool IsOpenEtao
		{
			get;
			set;
		}
		public System.DateTime? EtaoApplyTime
		{
			get;
			set;
		}
		public int EtaoStatus
		{
			get;
			set;
		}
		public int DecimalLength
		{
			get;
			set;
		}
		[StringLengthValidator(0, 10, Ruleset = "ValMasterSettings", MessageTemplate = "“您的价”重命名的长度限制在10字符以内")]
		public string YourPriceName
		{
			get;
			set;
		}
		public string DefaultProductImage
		{
			get;
			set;
		}
		public string DefaultProductThumbnail1
		{
			get;
			set;
		}
		public string DefaultProductThumbnail2
		{
			get;
			set;
		}
		public string DefaultProductThumbnail3
		{
			get;
			set;
		}
		public string DefaultProductThumbnail4
		{
			get;
			set;
		}
		public string DefaultProductThumbnail5
		{
			get;
			set;
		}
		public string DefaultProductThumbnail6
		{
			get;
			set;
		}
		public string DefaultProductThumbnail7
		{
			get;
			set;
		}
		public string DefaultProductThumbnail8
		{
			get;
			set;
		}
		public bool IsOpenSiteSale
		{
			get;
			set;
		}
		public bool Disabled
		{
			get;
			set;
		}
		[RangeValidator(typeof(decimal), "0.1", RangeBoundaryType.Inclusive, "10000000", RangeBoundaryType.Inclusive, Ruleset = "ValMasterSettings", MessageTemplate = "几元一积分必须在0.1-10000000之间")]
		public decimal PointsRate
		{
			get;
			set;
		}
		[RangeValidator(1, RangeBoundaryType.Inclusive, 90, RangeBoundaryType.Inclusive, Ruleset = "ValMasterSettings", MessageTemplate = "最近几天内订单的天数必须在1-90之间")]
		public int OrderShowDays
		{
			get;
			set;
		}
		[RangeValidator(1, RangeBoundaryType.Inclusive, 90, RangeBoundaryType.Inclusive, Ruleset = "ValMasterSettings", MessageTemplate = "过期几天自动关闭订单的天数必须在1-90之间")]
		public int CloseOrderDays
		{
			get;
			set;
		}
		[RangeValidator(1, RangeBoundaryType.Inclusive, 90, RangeBoundaryType.Inclusive, Ruleset = "ValMasterSettings", MessageTemplate = "发货几天自动完成订单的天数必须在1-90之间")]
		public int FinishOrderDays
		{
			get;
			set;
		}
		[RangeValidator(1, RangeBoundaryType.Inclusive, 90, RangeBoundaryType.Inclusive, Ruleset = "ValMasterSettings", MessageTemplate = "订单完成几天自动结束交易的天数必须在1-90之间")]
		public int EndOrderDays
		{
			get;
			set;
		}
		[RangeValidator(typeof(decimal), "0", RangeBoundaryType.Inclusive, "100", RangeBoundaryType.Inclusive, Ruleset = "ValMasterSettings", MessageTemplate = "税率必须在0-100之间")]
		public decimal TaxRate
		{
			get;
			set;
		}
		public decimal ReferralDeduct
		{
			get;
			set;
		}
		public decimal SubMemberDeduct
		{
			get;
			set;
		}
		public decimal SubReferralDeduct
		{
			get;
			set;
		}
		public string ReferralIntroduction
		{
			get;
			set;
		}
		public bool IsAuditReferral
		{
			get;
			set;
		}
		[StringLengthValidator(0, 4000, Ruleset = "ValMasterSettings", MessageTemplate = "网页客服代码长度限制在4000个字符以内")]
		public string HtmlOnlineServiceCode
		{
			get;
			set;
		}
		public string AssistantKey
		{
			get;
			set;
		}
		public string AssistantIv
		{
			get;
			set;
		}
		public int? UserId
		{
			get;
			private set;
		}
		public bool EnabledCnzz
		{
			get;
			set;
		}
		public string CnzzUsername
		{
			get;
			set;
		}
		public string SiteMapTime
		{
			get;
			set;
		}
		public string SiteMapNum
		{
			get;
			set;
		}
		public int TaobaoShippingType
		{
			get;
			set;
		}
		public string CnzzPassword
		{
			get;
			set;
		}
		public string BFDUserName
		{
			get;
			set;
		}
		public bool EnabledBFD
		{
			get;
			set;
		}
		public string VTheme
		{
			get;
			set;
		}
		public string WeixinAppId
		{
			get;
			set;
		}
		public string WeixinAppSecret
		{
			get;
			set;
		}
		public string WeixinToken
		{
			get;
			set;
		}
		public string VipCardBG
		{
			get;
			set;
		}
		public string VipCardLogo
		{
			get;
			set;
		}
		public string VipCardQR
		{
			get;
			set;
		}
		public string VipCardPrefix
		{
			get;
			set;
		}
		public string VipCardName
		{
			get;
			set;
		}
		public bool VipRequireName
		{
			get;
			set;
		}
		public bool VipRequireMobile
		{
			get;
			set;
		}
		public bool VipRequireQQ
		{
			get;
			set;
		}
		public bool VipRequireAdress
		{
			get;
			set;
		}
		public bool VipEnableCoupon
		{
			get;
			set;
		}
		public string VipRemark
		{
			get;
			set;
		}
		public string WeixinPaySignKey
		{
			get;
			set;
		}
		public string WeixinPartnerID
		{
			get;
			set;
		}
		public string WeixinPartnerKey
		{
			get;
			set;
		}
		public bool IsValidationService
		{
			get;
			set;
		}
		public string WeixinNumber
		{
			get;
			set;
		}
		public string WeixinLoginUrl
		{
			get;
			set;
		}
		public string WeiXinCodeImageUrl
		{
			get;
			set;
		}
		public bool EnableWeiXinRequest
		{
			get;
			set;
		}
		public bool EnablePodRequest
		{
			get;
			set;
		}
		public bool EnableOffLineRequest
		{
			get;
			set;
		}
		public bool EnableWeixinWapAliPay
		{
			get;
			set;
		}
		public string OffLinePayContent
		{
			get;
			set;
		}
		public bool EnableVshopShengPay
		{
			get;
			set;
		}
		public bool OpenManyService
		{
			get;
			set;
		}
		public bool EnableAppOffLinePay
		{
			get;
			set;
		}
		public bool EnableAppPodPay
		{
			get;
			set;
		}
		public bool EnableAppAliPay
		{
			get;
			set;
		}
		public bool EnableAppWapAliPay
		{
			get;
			set;
		}
		public bool EnableAppShengPay
		{
			get;
			set;
		}
        public bool EnableAppTFTPay
        {
            get;
            set;
        }
        public bool EnableAppWxPay
        {
            get;
            set;
        }
		public string WapTheme
		{
			get;
			set;
		}
		public bool EnableWapOffLinePay
		{
			get;
			set;
		}
		public bool EnableWapPodPay
		{
			get;
			set;
		}
		public bool EnableWapAliPay
		{
			get;
			set;
		}
		public bool EnableWapShengPay
		{
			get;
			set;
		}
		public string AliOHTheme
		{
			get;
			set;
		}
		public string AliOHAppId
		{
			get;
			set;
		}
		public bool EnableAliOHOffLinePay
		{
			get;
			set;
		}
		public bool EnableAliOHPodPay
		{
			get;
			set;
		}
		public bool EnableAliOHAliPay
		{
			get;
			set;
		}
		public string AliOHFollowRelay
		{
			get;
			set;
		}
		public string AliOHFollowRelayTitle
		{
			get;
			set;
		}
		public string AliOHServerUrl
		{
			get;
			set;
		}
		public bool EnableAliOHShengPay
		{
			get;
			set;
		}
        public bool EnableWxPayQrCode
        {
            get;
            set;
        }

        /// <summary>
        /// 企业组织机构代码
        /// </summary>
        public string OrgCode
        {
            get;
            set;
        }
        /// <summary>
        /// 发送方
        /// </summary>
        public string SenderID
        {
            get;
            set;
        }
        /// <summary>
        /// 用户编号
        /// </summary>
        public string appUid
        {
            get;
            set;
        }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string appUname
        {
            get;
            set;
        }
        /// <summary>
        /// 电商平台代码
        /// </summary>
        public string ebpCode
        {
            get;
            set;
        }
        /// <summary>
        /// 电商平台名称
        /// </summary>
        public string ebpName
        {
            get;
            set;
        }
        /// <summary>
        /// 币制
        /// </summary>
        public string currency
        {
            get;
            set;
        }
        /// <summary>
        /// 收货人所在国
        /// </summary>
        public string consigneeCountry
        {
            get;
            set;
        }
        /// <summary>
        /// 电商企业代码
        /// </summary>
        public string ebcCode
        {
            get;
            set;
        }
        /// <summary>
        /// 电商企业名称
        /// </summary>
        public string ebcName
        {
            get;
            set;
        }
        /// <summary>
        /// 海关代码
        /// </summary>
        public string customsCode
        {
            get;
            set;
        }
        /// <summary>
        /// 易极付调用接口
        /// </summary>
        public string yijifuUrl
        {
            get;
            set;
        }
        /// <summary>
        /// 接口服务代码
        /// </summary>
        public string serviceCode
        {
            get;
            set;
        }
        /// <summary>
        /// 易极付账号对应的合作方ID
        /// </summary>
        public string partnerId
        {
            get;
            set;
        }
        /// <summary>
        /// 签名方式
        /// </summary>
        public string signType
        {
            get;
            set;
        }
        /// <summary>
        /// 页面跳转返回URL
        /// </summary>
        public string returnUrl
        {
            get;
            set;
        }
        /// <summary>
        /// 异步通知URL
        /// </summary>
        public string notifyUrl
        {
            get;
            set;
        }
        /// <summary>
        /// 易极付Key
        /// </summary>
        public string YJFPaySignKey
        {
            get;
            set;
        }


        /// <summary>
        /// 物流企业代码
        /// </summary>
        public string logisticsCode
        {
            get;
            set;
        }

        /// <summary>
        /// 物流企业名称
        /// </summary>
        public string logisticsName
        {
            get;
            set;
        }

        /// <summary>
        /// 箱重量
        /// </summary>
        public string ContainerWeight
        {
            get;
            set;
        }

        /// <summary>
        /// 发货人所在国
        /// </summary>
        public string shipperCountry
        {
            get;
            set;
        }

        /// <summary>
        /// 圆通物流快递单号查询接口地址
        /// </summary>
        public string ExpressAddress
        {
            get;
            set;
        }
        /// <summary>
        /// EMS物流快递单号查询接口地址
        /// </summary>
        public string EmsExpressAddress { get; set; }
        /// <summary>
        /// 身份验证接口
        /// </summary>
        public string efindUrl
        {
            get;
            set;
        }
        /// <summary>
        /// 身份验证启动开关
        /// </summary>
        public string efindRunType
        {
            get;
            set;
        }
        /// <summary>
        /// 身份证验证服务代码
        /// </summary>
        public string serviceCertNoValidCode
        {
            get;
            set;
        }

        /// <summary>
        /// 优惠券到期天数
        /// </summary>
        public string CountDownCouponHours
        {
            get;
            set;
        }

        /// <summary>
        /// 优惠券到期天数提醒语
        /// </summary>
        public string CountDownCouponContent
        {
            get;
            set;
        }

                /// <summary>
        /// 优惠券到期数量的查询点 eg
        /// </summary>
        public string CountDownCouponPoint
        {
            get;
            set;
        }

        /// <summary>
        /// 特定商品类型
        /// </summary>
        public string CurrentCategoryId
        {
            get;
            set;
        }

        /// <summary>
        /// 特定商品类型描述
        /// </summary>
        public string CurrentCategoryDesc
        {
            get;
            set;
        }

        /// <summary>
        /// 申报挂起时间
        /// </summary>
        public int SuspensionTime
        {
            get;
            set;
        }
        
        
		public SiteSettings(string siteUrl)
		{
			this.SiteUrl = siteUrl;
			this.IsOpenSiteSale = true;
			this.Disabled = false;
			this.SiteDescription = "最安全，最专业的网上商店系统";
			this.Theme = "default";
			this.VTheme = "default";
			this.AliOHTheme = "default";
			this.WapTheme = "default";
            this.SiteName = "Ecdev";
			this.LogoUrl = "/utility/pics/logo.jpg";
			this.DefaultProductImage = "/utility/pics/none.gif";
			this.DefaultProductThumbnail1 = "/utility/pics/none.gif";
			this.DefaultProductThumbnail2 = "/utility/pics/none.gif";
			this.DefaultProductThumbnail3 = "/utility/pics/none.gif";
			this.DefaultProductThumbnail4 = "/utility/pics/none.gif";
			this.DefaultProductThumbnail5 = "/utility/pics/none.gif";
			this.DefaultProductThumbnail6 = "/utility/pics/none.gif";
			this.DefaultProductThumbnail7 = "/utility/pics/none.gif";
			this.DefaultProductThumbnail8 = "/utility/pics/none.gif";
			this.VipCardBG = "/Storage/master/Vipcard/vipbg.png";
			this.VipCardQR = "/Storage/master/Vipcard/vipqr.jpg";
			this.DecimalLength = 2;
			this.PointsRate = 1m;
			this.OrderShowDays = 7;
			this.CloseOrderDays = 3;
			this.FinishOrderDays = 7;
			this.EndOrderDays = 7;
			this.IsOpenSiteSale = true;
			this.EnableMobileClient = false;
			this.EnablePodRequest = false;
			this.ServiceStatus = 1;
			this.OpenAliho = 0;
			this.OpenTaobao = 1;
			this.OpenMobbile = 0;
			this.OpenVstore = 1;
			this.OpenWap = 0;
			this.IsAuditReferral = true;
			this.OpenManyService = false;
		}
		public void WriteToXml(XmlDocument doc)
		{
			XmlNode root = doc.SelectSingleNode("Settings");
			SiteSettings.SetNodeValue(doc, root, "SiteUrl", this.SiteUrl);
			SiteSettings.SetNodeValue(doc, root, "AssistantIv", this.AssistantIv);
			SiteSettings.SetNodeValue(doc, root, "AssistantKey", this.AssistantKey);
			SiteSettings.SetNodeValue(doc, root, "DecimalLength", this.DecimalLength.ToString(System.Globalization.CultureInfo.InvariantCulture));
			SiteSettings.SetNodeValue(doc, root, "DefaultProductImage", this.DefaultProductImage);
			SiteSettings.SetNodeValue(doc, root, "DefaultProductThumbnail1", this.DefaultProductThumbnail1);
			SiteSettings.SetNodeValue(doc, root, "DefaultProductThumbnail2", this.DefaultProductThumbnail2);
			SiteSettings.SetNodeValue(doc, root, "DefaultProductThumbnail3", this.DefaultProductThumbnail3);
			SiteSettings.SetNodeValue(doc, root, "DefaultProductThumbnail4", this.DefaultProductThumbnail4);
			SiteSettings.SetNodeValue(doc, root, "DefaultProductThumbnail5", this.DefaultProductThumbnail5);
			SiteSettings.SetNodeValue(doc, root, "DefaultProductThumbnail6", this.DefaultProductThumbnail6);
			SiteSettings.SetNodeValue(doc, root, "DefaultProductThumbnail7", this.DefaultProductThumbnail7);
			SiteSettings.SetNodeValue(doc, root, "DefaultProductThumbnail8", this.DefaultProductThumbnail8);
			SiteSettings.SetNodeValue(doc, root, "CheckCode", this.CheckCode);
			SiteSettings.SetNodeValue(doc, root, "IsOpenSiteSale", this.IsOpenSiteSale ? "true" : "false");
			SiteSettings.SetNodeValue(doc, root, "Disabled", this.Disabled ? "true" : "false");
			SiteSettings.SetNodeValue(doc, root, "ReferralDeduct", this.ReferralDeduct.ToString("F2"));
			SiteSettings.SetNodeValue(doc, root, "SubMemberDeduct", this.SubMemberDeduct.ToString("F2"));
			SiteSettings.SetNodeValue(doc, root, "SubReferralDeduct", this.SubReferralDeduct.ToString("F2"));
			SiteSettings.SetNodeValue(doc, root, "ReferralIntroduction", this.ReferralIntroduction);
			SiteSettings.SetNodeValue(doc, root, "IsAuditReferral", this.IsAuditReferral ? "true" : "false");
			SiteSettings.SetNodeValue(doc, root, "EtaoID", this.EtaoID);
			SiteSettings.SetNodeValue(doc, root, "IsCreateFeed", this.IsCreateFeed ? "true" : "false");
			SiteSettings.SetNodeValue(doc, root, "Footer", this.Footer);
			SiteSettings.SetNodeValue(doc, root, "RegisterAgreement", this.RegisterAgreement);
			SiteSettings.SetNodeValue(doc, root, "HtmlOnlineServiceCode", this.HtmlOnlineServiceCode);
			SiteSettings.SetNodeValue(doc, root, "LogoUrl", this.LogoUrl);
			SiteSettings.SetNodeValue(doc, root, "OrderShowDays", this.OrderShowDays.ToString(System.Globalization.CultureInfo.InvariantCulture));
			SiteSettings.SetNodeValue(doc, root, "CloseOrderDays", this.CloseOrderDays.ToString(System.Globalization.CultureInfo.InvariantCulture));
			SiteSettings.SetNodeValue(doc, root, "FinishOrderDays", this.FinishOrderDays.ToString(System.Globalization.CultureInfo.InvariantCulture));
			SiteSettings.SetNodeValue(doc, root, "EndOrderDays", this.EndOrderDays.ToString(System.Globalization.CultureInfo.InvariantCulture));
			SiteSettings.SetNodeValue(doc, root, "TaxRate", this.TaxRate.ToString(System.Globalization.CultureInfo.InvariantCulture));
			SiteSettings.SetNodeValue(doc, root, "PointsRate", this.PointsRate.ToString("F"));
			SiteSettings.SetNodeValue(doc, root, "SearchMetaDescription", this.SearchMetaDescription);
			SiteSettings.SetNodeValue(doc, root, "SearchMetaKeywords", this.SearchMetaKeywords);
			SiteSettings.SetNodeValue(doc, root, "SiteDescription", this.SiteDescription);
			SiteSettings.SetNodeValue(doc, root, "SiteName", this.SiteName);
			SiteSettings.SetNodeValue(doc, root, "Theme", this.Theme);
			SiteSettings.SetNodeValue(doc, root, "YourPriceName", this.YourPriceName);
			SiteSettings.SetNodeValue(doc, root, "EmailSender", this.EmailSender);
			SiteSettings.SetNodeValue(doc, root, "EmailSettings", this.EmailSettings);
			SiteSettings.SetNodeValue(doc, root, "SMSSender", this.SMSSender);
			SiteSettings.SetNodeValue(doc, root, "SMSSettings", this.SMSSettings);
			SiteSettings.SetNodeValue(doc, root, "SiteMapNum", this.SiteMapNum);
			SiteSettings.SetNodeValue(doc, root, "TaobaoShippingType", this.TaobaoShippingType.ToString());
			SiteSettings.SetNodeValue(doc, root, "SiteMapTime", this.SiteMapTime);
			SiteSettings.SetNodeValue(doc, root, "EnabledBFD", this.EnabledBFD ? "true" : "false");
			SiteSettings.SetNodeValue(doc, root, "BFDUserName", this.BFDUserName);
			SiteSettings.SetNodeValue(doc, root, "EnabledCnzz", this.EnabledCnzz ? "true" : "false");
			SiteSettings.SetNodeValue(doc, root, "CnzzUsername", this.CnzzUsername);
			SiteSettings.SetNodeValue(doc, root, "CnzzPassword", this.CnzzPassword);
			SiteSettings.SetNodeValue(doc, root, "EnableMobileClient", this.EnableMobileClient ? "true" : "false");
			SiteSettings.SetNodeValue(doc, root, "MobileClientSpread", this.MobileClientSpread);
			SiteSettings.SetNodeValue(doc, root, "CellPhoneUserCode", this.CellPhoneUserCode);
			SiteSettings.SetNodeValue(doc, root, "CellPhoneToken", this.CellPhoneToken);
			SiteSettings.SetNodeValue(doc, root, "ApplicationMark", this.ApplicationMark);
			SiteSettings.SetNodeValue(doc, root, "SiteToken", this.SiteToken);
			SiteSettings.SetNodeValue(doc, root, "SiteTime", this.SiteTime);
			SiteSettings.SetNodeValue(doc, root, "WeixinAppId", this.WeixinAppId);
			SiteSettings.SetNodeValue(doc, root, "WeixinAppSecret", this.WeixinAppSecret);
			SiteSettings.SetNodeValue(doc, root, "WeixinToken", this.WeixinToken);
			SiteSettings.SetNodeValue(doc, root, "WeixinPaySignKey", this.WeixinPaySignKey);
			SiteSettings.SetNodeValue(doc, root, "WeixinPartnerID", this.WeixinPartnerID);
			SiteSettings.SetNodeValue(doc, root, "WeixinPartnerKey", this.WeixinPartnerKey);
			SiteSettings.SetNodeValue(doc, root, "VTheme", this.VTheme);
			SiteSettings.SetNodeValue(doc, root, "VipCardBG", this.VipCardBG);
			SiteSettings.SetNodeValue(doc, root, "VipCardLogo", this.VipCardLogo);
			SiteSettings.SetNodeValue(doc, root, "VipCardQR", this.VipCardQR);
			SiteSettings.SetNodeValue(doc, root, "VipCardPrefix", this.VipCardPrefix);
			SiteSettings.SetNodeValue(doc, root, "VipCardName", this.VipCardName);
			SiteSettings.SetNodeValue(doc, root, "VipRequireName", this.VipRequireName ? "true" : "false");
			SiteSettings.SetNodeValue(doc, root, "VipRequireMobile", this.VipRequireMobile ? "true" : "false");
			SiteSettings.SetNodeValue(doc, root, "VipRequireQQ", this.VipRequireQQ ? "true" : "false");
			SiteSettings.SetNodeValue(doc, root, "VipRequireAdress", this.VipRequireAdress ? "true" : "false");
			SiteSettings.SetNodeValue(doc, root, "VipEnableCoupon", this.VipEnableCoupon ? "true" : "false");
			SiteSettings.SetNodeValue(doc, root, "VipRemark", this.VipRemark);
			SiteSettings.SetNodeValue(doc, root, "EnableWeiXinRequest", this.EnableWeiXinRequest ? "true" : "false");
			SiteSettings.SetNodeValue(doc, root, "EnableOffLineRequest", this.EnableOffLineRequest ? "true" : "false");
			SiteSettings.SetNodeValue(doc, root, "EnableWeixinWapAliPay", this.EnableWeixinWapAliPay ? "true" : "false");
			SiteSettings.SetNodeValue(doc, root, "OffLinePayContent", this.OffLinePayContent);
			SiteSettings.SetNodeValue(doc, root, "IsValidationService", this.IsValidationService ? "true" : "false");
			SiteSettings.SetNodeValue(doc, root, "WeixinNumber", this.WeixinNumber);
			SiteSettings.SetNodeValue(doc, root, "WeixinLoginUrl", this.WeixinLoginUrl);
			SiteSettings.SetNodeValue(doc, root, "WeiXinCodeImageUrl", this.WeiXinCodeImageUrl);
			SiteSettings.SetNodeValue(doc, root, "EnableAppOffLinePay", this.EnableAppOffLinePay ? "true" : "false");
			SiteSettings.SetNodeValue(doc, root, "EnableAppPodPay", this.EnableAppPodPay ? "true" : "false");
			SiteSettings.SetNodeValue(doc, root, "EnableAppAliPay", this.EnableAppAliPay ? "true" : "false");
			SiteSettings.SetNodeValue(doc, root, "EnableAppWapAliPay", this.EnableAppWapAliPay ? "true" : "false");
			SiteSettings.SetNodeValue(doc, root, "EnableWapAliPay", this.EnableWapAliPay ? "true" : "false");
			SiteSettings.SetNodeValue(doc, root, "WapTheme", this.WapTheme);
			SiteSettings.SetNodeValue(doc, root, "EnablePodRequest", this.EnablePodRequest ? "true" : "false");
			SiteSettings.SetNodeValue(doc, root, "EnableWapOffLinePay", this.EnableWapOffLinePay ? "true" : "false");
			SiteSettings.SetNodeValue(doc, root, "EnableWapPodPay", this.EnableWapPodPay ? "true" : "false");
			SiteSettings.SetNodeValue(doc, root, "AliOHAppId", this.AliOHAppId);
			SiteSettings.SetNodeValue(doc, root, "EnableAliOHOffLinePay", this.EnableAliOHOffLinePay ? "true" : "false");
			SiteSettings.SetNodeValue(doc, root, "AliOHTheme", this.AliOHTheme);
			SiteSettings.SetNodeValue(doc, root, "EnableAliOHAliPay", this.EnableAliOHAliPay ? "true" : "false");
			SiteSettings.SetNodeValue(doc, root, "EnableAliOHPodPay", this.EnableAliOHPodPay ? "true" : "false");
			SiteSettings.SetNodeValue(doc, root, "AliOHFollowRelay", this.AliOHFollowRelay);
			SiteSettings.SetNodeValue(doc, root, "AliOHFollowRelayTitle", this.AliOHFollowRelayTitle);
			SiteSettings.SetNodeValue(doc, root, "AliOHServerUrl", this.AliOHServerUrl);
			SiteSettings.SetNodeValue(doc, root, "EnableVshopShengPay", this.EnableVshopShengPay ? "true" : "false");
			SiteSettings.SetNodeValue(doc, root, "EnableWapShengPay", this.EnableWapShengPay ? "true" : "false");
			SiteSettings.SetNodeValue(doc, root, "EnableAppShengPay", this.EnableAppShengPay ? "true" : "false");
            SiteSettings.SetNodeValue(doc, root, "EnableAppTFTPay", this.EnableAppTFTPay ? "true" : "false");
            SiteSettings.SetNodeValue(doc, root, "EnableAppWxPay", this.EnableAppWxPay ? "true" : "false");
			SiteSettings.SetNodeValue(doc, root, "EnableAliOHShengPay", this.EnableAliOHShengPay ? "true" : "false");
			SiteSettings.SetNodeValue(doc, root, "ServiceStatus", this.ServiceStatus.ToString());
			SiteSettings.SetNodeValue(doc, root, "OpenTaobao", this.OpenTaobao.ToString());
			SiteSettings.SetNodeValue(doc, root, "OpenMobbile", this.OpenMobbile.ToString());
			SiteSettings.SetNodeValue(doc, root, "OpenAliho", this.OpenAliho.ToString());
			SiteSettings.SetNodeValue(doc, root, "OpenVstore", this.OpenVstore.ToString());
			SiteSettings.SetNodeValue(doc, root, "OpenWap", this.OpenWap.ToString());
			SiteSettings.SetNodeValue(doc, root, "OpenManyService", this.OpenManyService ? "true" : "false");
            SiteSettings.SetNodeValue(doc, root, "XinGeSender", this.XinGeSender);
            SiteSettings.SetNodeValue(doc, root, "XinGeSettings", this.XinGeSettings);

            SiteSettings.SetNodeValue(doc, root, "OrgCode", this.OrgCode.ToString());
            SiteSettings.SetNodeValue(doc, root, "SenderID", this.SenderID.ToString());
            SiteSettings.SetNodeValue(doc, root, "appUid", this.appUid.ToString());
            SiteSettings.SetNodeValue(doc, root, "appUname", this.appUname.ToString());
            SiteSettings.SetNodeValue(doc, root, "ebpCode", this.ebpCode.ToString());
            SiteSettings.SetNodeValue(doc, root, "ebpName", this.ebpName.ToString());
            SiteSettings.SetNodeValue(doc, root, "currency", this.currency.ToString());
            SiteSettings.SetNodeValue(doc, root, "consigneeCountry", this.consigneeCountry.ToString());
            SiteSettings.SetNodeValue(doc, root, "ebcCode", this.ebcCode.ToString());
            SiteSettings.SetNodeValue(doc, root, "ebcName", this.ebcName.ToString());
            SiteSettings.SetNodeValue(doc, root, "customsCode", this.customsCode.ToString());
            SiteSettings.SetNodeValue(doc, root, "yijifuUrl", this.yijifuUrl.ToString());
            SiteSettings.SetNodeValue(doc, root, "serviceCode", this.serviceCode.ToString());
            SiteSettings.SetNodeValue(doc, root, "partnerId", this.partnerId.ToString());
            SiteSettings.SetNodeValue(doc, root, "signType", this.signType.ToString());
            SiteSettings.SetNodeValue(doc, root, "returnUrl", this.returnUrl.ToString());
            SiteSettings.SetNodeValue(doc, root, "notifyUrl", this.notifyUrl.ToString());
            SiteSettings.SetNodeValue(doc, root, "YJFPaySignKey", this.YJFPaySignKey.ToString());
            SiteSettings.SetNodeValue(doc, root, "efindUrl", this.efindUrl.ToString());
            SiteSettings.SetNodeValue(doc, root, "efindRunType", this.efindRunType.ToString());
            SiteSettings.SetNodeValue(doc, root, "serviceCertNoValidCode", this.serviceCertNoValidCode.ToString());
                

            SiteSettings.SetNodeValue(doc, root, "EnableWxPayQrCode", this.EnableWxPayQrCode ? "true" : "false");

            SiteSettings.SetNodeValue(doc, root, "logisticsCode", this.logisticsCode.ToString());
            SiteSettings.SetNodeValue(doc, root, "logisticsName", this.logisticsName.ToString());
            SiteSettings.SetNodeValue(doc, root, "ContainerWeight", this.ContainerWeight.ToString());
            SiteSettings.SetNodeValue(doc, root, "shipperCountry", this.shipperCountry.ToString());
            SiteSettings.SetNodeValue(doc, root, "ExpressAddress", this.ExpressAddress.ToString());
            SiteSettings.SetNodeValue(doc, root, "EmsExpressAddress", this.EmsExpressAddress);
            SiteSettings.SetNodeValue(doc, root, "CountDownCouponHours", this.CountDownCouponHours.ToString());
            SiteSettings.SetNodeValue(doc, root, "CountDownCouponContent", this.CountDownCouponContent.ToString());
            SiteSettings.SetNodeValue(doc, root, "CountDownCouponPoint", this.CountDownCouponPoint.ToString());
            SiteSettings.SetNodeValue(doc, root, "CurrentCategoryId", this.CurrentCategoryId.ToString());
            SiteSettings.SetNodeValue(doc, root, "CurrentCategoryDesc", this.CurrentCategoryDesc.ToString());

            //订单推送配置
            SiteSettings.SetNodeValue(doc, root, "SendOrderStartTime", this.SendOrderStartTime.ToString());
            SiteSettings.SetNodeValue(doc, root, "SendOrderEndTime", this.SendOrderEndTime.ToString());
            SiteSettings.SetNodeValue(doc, root, "SendOrderDay", this.SendOrderDay.ToString());
            SiteSettings.SetNodeValue(doc, root, "SendOrderEmail", this.SendOrderEmail.ToString());
            SiteSettings.SetNodeValue(doc, root, "IsSendOrderOpen", this.IsSendOrderOpen);
            SiteSettings.SetNodeValue(doc, root, "IsRunTimes", this.IsRunTimes);
            //申报挂起时间
            SiteSettings.SetNodeValue(doc, root, "SuspensionTime", this.SuspensionTime.ToString());

        }
		public static SiteSettings FromXml(XmlDocument doc)
		{
			XmlNode xmlNode = doc.SelectSingleNode("Settings");

			return new SiteSettings(SiteSettings.GetNodeValue(xmlNode, "SiteUrl", ""))
			{
				AssistantIv = SiteSettings.GetNodeValue(xmlNode, "AssistantIv", ""),
				AssistantKey = SiteSettings.GetNodeValue(xmlNode, "AssistantKey", ""),
				DecimalLength = int.Parse(SiteSettings.GetNodeValue(xmlNode, "DecimalLength", "2")),
				DefaultProductImage = SiteSettings.GetNodeValue(xmlNode, "DefaultProductImage", ""),
				DefaultProductThumbnail1 = SiteSettings.GetNodeValue(xmlNode, "DefaultProductThumbnail1", ""),
				DefaultProductThumbnail2 = SiteSettings.GetNodeValue(xmlNode, "DefaultProductThumbnail2", ""),
				DefaultProductThumbnail3 = SiteSettings.GetNodeValue(xmlNode, "DefaultProductThumbnail3", ""),
				DefaultProductThumbnail4 = SiteSettings.GetNodeValue(xmlNode, "DefaultProductThumbnail4", ""),
				DefaultProductThumbnail5 = SiteSettings.GetNodeValue(xmlNode, "DefaultProductThumbnail5", ""),
				DefaultProductThumbnail6 = SiteSettings.GetNodeValue(xmlNode, "DefaultProductThumbnail6", ""),
				DefaultProductThumbnail7 = SiteSettings.GetNodeValue(xmlNode, "DefaultProductThumbnail7", ""),
				DefaultProductThumbnail8 = SiteSettings.GetNodeValue(xmlNode, "DefaultProductThumbnail8", ""),
				CheckCode = SiteSettings.GetNodeValue(xmlNode, "CheckCode", ""),
                IsOpenSiteSale = bool.Parse(SiteSettings.GetNodeValue(xmlNode, "IsOpenSiteSale", "false")),
                Disabled = bool.Parse(SiteSettings.GetNodeValue(xmlNode, "Disabled", "false")),
				ReferralDeduct = decimal.Parse(SiteSettings.GetNodeValue(xmlNode, "ReferralDeduct", "0.00")),
				SubMemberDeduct = decimal.Parse(SiteSettings.GetNodeValue(xmlNode, "SubMemberDeduct", "0.00")),
				SubReferralDeduct = decimal.Parse(SiteSettings.GetNodeValue(xmlNode, "SubReferralDeduct", "0.00")),
				ReferralIntroduction = SiteSettings.GetNodeValue(xmlNode, "ReferralIntroduction", ""),
                IsAuditReferral = bool.Parse(SiteSettings.GetNodeValue(xmlNode, "IsAuditReferral", "false")),
				EtaoID = SiteSettings.GetNodeValue(xmlNode, "EtaoID", ""),
                IsCreateFeed = bool.Parse(SiteSettings.GetNodeValue(xmlNode, "IsCreateFeed", "false")),
				Footer = SiteSettings.GetNodeValue(xmlNode, "Footer", ""),
				RegisterAgreement = SiteSettings.GetNodeValue(xmlNode, "RegisterAgreement", ""),
				HtmlOnlineServiceCode = SiteSettings.GetNodeValue(xmlNode, "HtmlOnlineServiceCode", ""),
				LogoUrl = SiteSettings.GetNodeValue(xmlNode, "LogoUrl", ""),
				OrderShowDays = int.Parse(SiteSettings.GetNodeValue(xmlNode, "OrderShowDays", "7")),
				CloseOrderDays = int.Parse(SiteSettings.GetNodeValue(xmlNode, "CloseOrderDays", "1")),
				FinishOrderDays = int.Parse(SiteSettings.GetNodeValue(xmlNode, "FinishOrderDays", "15")),
				EndOrderDays = int.Parse(SiteSettings.GetNodeValue(xmlNode, "EndOrderDays", "15")),
				TaxRate = decimal.Parse(SiteSettings.GetNodeValue(xmlNode, "TaxRate", "")),
				PointsRate = decimal.Parse(SiteSettings.GetNodeValue(xmlNode, "PointsRate", "")),
				SearchMetaDescription = SiteSettings.GetNodeValue(xmlNode, "SearchMetaDescription", ""),
				SearchMetaKeywords = SiteSettings.GetNodeValue(xmlNode, "SearchMetaKeywords", ""),
				SiteDescription = SiteSettings.GetNodeValue(xmlNode, "SiteDescription", ""),
				SiteName = SiteSettings.GetNodeValue(xmlNode, "SiteName", ""),
				SiteUrl = SiteSettings.GetNodeValue(xmlNode, "SiteUrl", ""),
				UserId = null,
				Theme = SiteSettings.GetNodeValue(xmlNode, "Theme", ""),
				YourPriceName = SiteSettings.GetNodeValue(xmlNode, "YourPriceName", ""),
				EmailSender = SiteSettings.GetNodeValue(xmlNode, "EmailSender", ""),
				EmailSettings = SiteSettings.GetNodeValue(xmlNode, "EmailSettings", ""),
				SMSSender = SiteSettings.GetNodeValue(xmlNode, "SMSSender", ""),
				SMSSettings = SiteSettings.GetNodeValue(xmlNode, "SMSSettings", ""),
				SiteMapTime = SiteSettings.GetNodeValue(xmlNode, "SiteMapTime", ""),
				SiteMapNum = SiteSettings.GetNodeValue(xmlNode, " SiteMapNum", ""),
				TaobaoShippingType = int.Parse(SiteSettings.GetNodeValue(xmlNode, "TaobaoShippingType", "0")),
                EnabledBFD = bool.Parse(SiteSettings.GetNodeValue(xmlNode, "EnabledBFD", "false")),
				BFDUserName = SiteSettings.GetNodeValue(xmlNode, "BFDUserName", ""),
                EnabledCnzz = bool.Parse(SiteSettings.GetNodeValue(xmlNode, "EnabledCnzz", "false")),
				CnzzUsername = SiteSettings.GetNodeValue(xmlNode, "CnzzUsername", ""),
				CnzzPassword = SiteSettings.GetNodeValue(xmlNode, "CnzzPassword", ""),
                EnableMobileClient = bool.Parse(SiteSettings.GetNodeValue(xmlNode, "EnableMobileClient", "false")),
				MobileClientSpread = SiteSettings.GetNodeValue(xmlNode, "MobileClientSpread", ""),
				CellPhoneUserCode = SiteSettings.GetNodeValue(xmlNode, "CellPhoneUserCode", ""),
				CellPhoneToken = SiteSettings.GetNodeValue(xmlNode, "CellPhoneToken", ""),
				ApplicationMark = SiteSettings.GetNodeValue(xmlNode, "ApplicationMark", ""),
				SiteToken = SiteSettings.GetNodeValue(xmlNode, "SiteToken", ""),
				SiteTime = SiteSettings.GetNodeValue(xmlNode, "SiteTime", ""),
				WeixinAppId = SiteSettings.GetNodeValue(xmlNode, "WeixinAppId", ""),
				WeixinAppSecret = SiteSettings.GetNodeValue(xmlNode, "WeixinAppSecret", ""),
				WeixinToken = SiteSettings.GetNodeValue(xmlNode, "WeixinToken", ""),
				WeixinPaySignKey = SiteSettings.GetNodeValue(xmlNode, "WeixinPaySignKey", ""),
				WeixinPartnerID = SiteSettings.GetNodeValue(xmlNode, "WeixinPartnerID", ""),
				WeixinPartnerKey = SiteSettings.GetNodeValue(xmlNode, "WeixinPartnerKey", ""),
				VTheme = SiteSettings.GetNodeValue(xmlNode, "VTheme", ""),
				VipCardLogo = SiteSettings.GetNodeValue(xmlNode, "VipCardLogo", ""),
				VipCardBG = SiteSettings.GetNodeValue(xmlNode, "VipCardBG", ""),
				VipCardQR = SiteSettings.GetNodeValue(xmlNode, "VipCardQR", ""),
				VipCardName = SiteSettings.GetNodeValue(xmlNode, "VipCardName", ""),
				VipCardPrefix = SiteSettings.GetNodeValue(xmlNode, "VipCardPrefix", ""),
                VipRequireName = bool.Parse(SiteSettings.GetNodeValue(xmlNode, "VipRequireName", "false")),
                VipRequireMobile = bool.Parse(SiteSettings.GetNodeValue(xmlNode, "VipRequireMobile", "false")),
                VipRequireAdress = bool.Parse(SiteSettings.GetNodeValue(xmlNode, "VipRequireAdress", "false")),
                VipRequireQQ = bool.Parse(SiteSettings.GetNodeValue(xmlNode, "VipRequireQQ", "false")),
                VipEnableCoupon = bool.Parse(SiteSettings.GetNodeValue(xmlNode, "VipEnableCoupon", "false")),
                EnablePodRequest = bool.Parse(SiteSettings.GetNodeValue(xmlNode, "EnablePodRequest", "false")),
                EnableAppOffLinePay = bool.Parse(SiteSettings.GetNodeValue(xmlNode, "EnableAppOffLinePay", "false")),
                EnableAppPodPay = bool.Parse(SiteSettings.GetNodeValue(xmlNode, "EnableAppPodPay", "false")),
                EnableAppAliPay = bool.Parse(SiteSettings.GetNodeValue(xmlNode, "EnableAppAliPay", "false")),
                EnableAppWapAliPay = bool.Parse(SiteSettings.GetNodeValue(xmlNode, "EnableAppWapAliPay", "false")),
                EnableWapAliPay = bool.Parse(SiteSettings.GetNodeValue(xmlNode, "EnableWapAliPay", "false")),
                IsValidationService = bool.Parse(SiteSettings.GetNodeValue(xmlNode, "IsValidationService", "false")),
                EnableWeiXinRequest = bool.Parse(SiteSettings.GetNodeValue(xmlNode, "EnableWeiXinRequest", "false")),
                EnableOffLineRequest = bool.Parse(SiteSettings.GetNodeValue(xmlNode, "EnableOffLineRequest", "false")),
                EnableWeixinWapAliPay = bool.Parse(SiteSettings.GetNodeValue(xmlNode, "EnableWeixinWapAliPay", "false")),
				WeixinNumber = SiteSettings.GetNodeValue(xmlNode, "WeixinNumber", ""),
				WeixinLoginUrl = SiteSettings.GetNodeValue(xmlNode, "WeixinLoginUrl", ""),
				WeiXinCodeImageUrl = SiteSettings.GetNodeValue(xmlNode, "WeiXinCodeImageUrl", ""),
				OffLinePayContent = SiteSettings.GetNodeValue(xmlNode, "OffLinePayContent", ""),
				WapTheme = SiteSettings.GetNodeValue(xmlNode, "WapTheme", ""),
                EnableWapOffLinePay = bool.Parse(SiteSettings.GetNodeValue(xmlNode, "EnableWapOffLinePay", "false")),
                EnableWapPodPay = bool.Parse(SiteSettings.GetNodeValue(xmlNode, "EnableWapPodPay", "false")),
				AliOHAppId = SiteSettings.GetNodeValue(xmlNode, "AliOHAppId", ""),
                EnableAliOHAliPay = bool.Parse(SiteSettings.GetNodeValue(xmlNode, "EnableAliOHAliPay", "false")),
				AliOHTheme = SiteSettings.GetNodeValue(xmlNode, "AliOHTheme", ""),
                EnableAliOHOffLinePay = bool.Parse(SiteSettings.GetNodeValue(xmlNode, "EnableAliOHOffLinePay", "false")),
                EnableAliOHPodPay = bool.Parse(SiteSettings.GetNodeValue(xmlNode, "EnableAliOHPodPay", "false")),
				AliOHFollowRelay = SiteSettings.GetNodeValue(xmlNode, "AliOHFollowRelay", ""),
				AliOHServerUrl = SiteSettings.GetNodeValue(xmlNode, "AliOHServerUrl", ""),
                EnableVshopShengPay = bool.Parse(SiteSettings.GetNodeValue(xmlNode, "EnableVshopShengPay", "false")),
                EnableWapShengPay = bool.Parse(SiteSettings.GetNodeValue(xmlNode, "EnableWapShengPay", "false")),
                EnableAppShengPay = bool.Parse(SiteSettings.GetNodeValue(xmlNode, "EnableAppShengPay", "false")),
                EnableAppTFTPay = bool.Parse(SiteSettings.GetNodeValue(xmlNode, "EnableAppTFTPay", "false")),
                EnableAppWxPay = bool.Parse(SiteSettings.GetNodeValue(xmlNode, "EnableAppWxPay", "false")),
                EnableAliOHShengPay = bool.Parse(SiteSettings.GetNodeValue(xmlNode, "EnableAliOHShengPay", "false")),
				AliOHFollowRelayTitle = SiteSettings.GetNodeValue(xmlNode, "AliOHFollowRelayTitle", ""),
				ServiceStatus = int.Parse(SiteSettings.GetNodeValue(xmlNode, "ServiceStatus", "0")),
				OpenVstore = int.Parse(SiteSettings.GetNodeValue(xmlNode, "OpenVstore", "1")),
				OpenAliho = int.Parse(SiteSettings.GetNodeValue(xmlNode, "OpenAliho", "0")),
				OpenTaobao = int.Parse(SiteSettings.GetNodeValue(xmlNode, "OpenTaobao", "0")),
				OpenWap = int.Parse(SiteSettings.GetNodeValue(xmlNode, "OpenWap", "0")),
				OpenMobbile = int.Parse(SiteSettings.GetNodeValue(xmlNode, "OpenMobbile", "0")),
                OpenManyService = bool.Parse(SiteSettings.GetNodeValue(xmlNode, "OpenManyService", "false")),
                XinGeSender = SiteSettings.GetNodeValue(xmlNode, "XinGeSender", ""),
                XinGeSettings = SiteSettings.GetNodeValue(xmlNode, "XinGeSettings", ""),
                OrgCode = SiteSettings.GetNodeValue(xmlNode, "OrgCode", ""),
                SenderID = SiteSettings.GetNodeValue(xmlNode, "SenderID", ""),
                appUid = SiteSettings.GetNodeValue(xmlNode, "appUid", ""),
                appUname = SiteSettings.GetNodeValue(xmlNode, "appUname", ""),
                ebpCode = SiteSettings.GetNodeValue(xmlNode, "ebpCode", ""),
                ebpName = SiteSettings.GetNodeValue(xmlNode, "ebpName", ""),
                currency = SiteSettings.GetNodeValue(xmlNode, "currency", ""),
                consigneeCountry = SiteSettings.GetNodeValue(xmlNode, "consigneeCountry", ""),
                ebcCode = SiteSettings.GetNodeValue(xmlNode, "ebcCode", ""),
                ebcName = SiteSettings.GetNodeValue(xmlNode, "ebcName", ""),
                customsCode = SiteSettings.GetNodeValue(xmlNode, "customsCode", ""),
                yijifuUrl = SiteSettings.GetNodeValue(xmlNode, "yijifuUrl", ""),
                serviceCode = SiteSettings.GetNodeValue(xmlNode, "serviceCode", ""),
                partnerId = SiteSettings.GetNodeValue(xmlNode, "partnerId", ""),
                signType = SiteSettings.GetNodeValue(xmlNode, "signType", ""),
                returnUrl = SiteSettings.GetNodeValue(xmlNode, "returnUrl", ""),
                notifyUrl = SiteSettings.GetNodeValue(xmlNode, "notifyUrl", ""),
                YJFPaySignKey = SiteSettings.GetNodeValue(xmlNode, "YJFPaySignKey", ""),
                efindUrl = SiteSettings.GetNodeValue(xmlNode, "efindUrl", ""),
                efindRunType = SiteSettings.GetNodeValue(xmlNode, "efindRunType", ""),
                serviceCertNoValidCode = SiteSettings.GetNodeValue(xmlNode, "serviceCertNoValidCode", ""),
                
                

                logisticsCode = SiteSettings.GetNodeValue(xmlNode, "logisticsCode", ""),
                logisticsName = SiteSettings.GetNodeValue(xmlNode, "logisticsName", ""),
                ContainerWeight = SiteSettings.GetNodeValue(xmlNode, "ContainerWeight", ""),
                shipperCountry = SiteSettings.GetNodeValue(xmlNode, "shipperCountry", ""),
                ExpressAddress = SiteSettings.GetNodeValue(xmlNode,"ExpressAddress",""),
                EmsExpressAddress = SiteSettings.GetNodeValue(xmlNode, "EmsExpressAddress", ""),
                CountDownCouponHours = SiteSettings.GetNodeValue(xmlNode, "CountDownCouponHours", ""),
                CountDownCouponContent = SiteSettings.GetNodeValue(xmlNode, "CountDownCouponContent", ""),
                CountDownCouponPoint = SiteSettings.GetNodeValue(xmlNode, "CountDownCouponPoint", ""),
                CurrentCategoryId = SiteSettings.GetNodeValue(xmlNode, "CurrentCategoryId", ""),
                CurrentCategoryDesc = SiteSettings.GetNodeValue(xmlNode, "CurrentCategoryDesc", ""),
                EnableWxPayQrCode = bool.Parse(SiteSettings.GetNodeValue(xmlNode, "EnableWxPayQrCode", "false")),

                
                //订单推送配置
                SendOrderStartTime = SiteSettings.GetNodeValue(xmlNode, "SendOrderStartTime", ""),
                SendOrderEndTime = SiteSettings.GetNodeValue(xmlNode, "SendOrderEndTime", ""),
                SendOrderDay = SiteSettings.GetNodeValue(xmlNode,"SendOrderDay",""),
                SendOrderEmail = SiteSettings.GetNodeValue(xmlNode, "SendOrderEmail", ""),
                IsSendOrderOpen = SiteSettings.GetNodeValue(xmlNode, "IsSendOrderOpen", ""),
                IsRunTimes = SiteSettings.GetNodeValue(xmlNode, "IsRunTimes", ""),
                //申报挂起时间
                SuspensionTime = int.Parse(SiteSettings.GetNodeValue(xmlNode, "SuspensionTime", "0"))

			};
		}
		private static void SetNodeValue(XmlDocument doc, XmlNode root, string nodeName, string nodeValue)
		{
			XmlNode xmlNode = root.SelectSingleNode(nodeName);
			if (xmlNode == null)
			{
				xmlNode = doc.CreateElement(nodeName);
				root.AppendChild(xmlNode);
			}
			xmlNode.InnerText = nodeValue;
		}

        private static string GetNodeValue(XmlNode node, string key, string defaultValue)
        {
            try
            {
                return node.SelectSingleNode(key).InnerText;
            }
            catch { }

            return defaultValue;
        }
	}
}
