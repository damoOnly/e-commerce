$(document).ready(function () {

    var $0 = $("#ctl00_contentHolder_cbAll"); $0.attr("Privilege", "0");

    $0 = $("#ctl00_contentHolder_cbSummary"); $0.attr("Privilege", "99999");

    // 商城管理
    $0 = $("#ctl00_contentHolder_cbShop"); $0.attr("Privilege", "10");
    $0 = $("#ctl00_contentHolder_cbSiteContent"); $0.attr("Privilege", "101");
    $0 = $("#ctl00_contentHolder_cbSMSSettings"); $0.attr("Privilege", "102");
    $0 = $("#ctl00_contentHolder_cbEmailSettings"); $0.attr("Privilege", "103");
    $0 = $("#ctl00_contentHolder_cbPaymentModes"); $0.attr("Privilege", "104");
    $0 = $("#ctl00_contentHolder_cbShippingModes"); $0.attr("Privilege", "105");
    $0 = $("#ctl00_contentHolder_cbShippingTemplets"); $0.attr("Privilege", "106");
    $0 = $("#ctl00_contentHolder_cbExpressComputerpes"); $0.attr("Privilege", "107");
    $0 = $("#ctl00_contentHolder_cbMessageTemplets"); $0.attr("Privilege", "108");
    $0 = $("#ctl00_contentHolder_cbPictureMange"); $0.attr("Privilege", "109");

    //页面管理
    $0 = $("#ctl00_contentHolder_cbPageManger"); $0.attr("Privilege", "11");
    $0 = $("#ctl00_contentHolder_cbManageThemes"); $0.attr("Privilege", "111");
    $0 = $("#ctl00_contentHolder_cbAfficheList"); $0.attr("Privilege", "112");
    $0 = $("#ctl00_contentHolder_cbHelpCategories"); $0.attr("Privilege", "113");
    $0 = $("#ctl00_contentHolder_cbHelpList"); $0.attr("Privilege", "114");
    $0 = $("#ctl00_contentHolder_cbArticleCategories"); $0.attr("Privilege", "115");
    $0 = $("#ctl00_contentHolder_cbArticleList"); $0.attr("Privilege", "116");
    $0 = $("#ctl00_contentHolder_cbFriendlyLinks"); $0.attr("Privilege", "117");
    $0 = $("#ctl00_contentHolder_cbManageHotKeywords"); $0.attr("Privilege", "118");
    $0 = $("#ctl00_contentHolder_cbVotes"); $0.attr("Privilege", "119");


    //商品管理
    $0 = $("#ctl00_contentHolder_cbProductCatalog"); $0.attr("Privilege", "12");
    $0 = $("#ctl00_contentHolder_cbManageProducts"); $0.attr("Privilege", "121");
    $0 = $("#ctl00_contentHolder_cbManageProductsView"); $0.attr("Privilege", "121_1");
    $0 = $("#ctl00_contentHolder_cbManageProductsAdd"); $0.attr("Privilege", "121_2");
    $0 = $("#ctl00_contentHolder_cbManageProductsEdit"); $0.attr("Privilege", "121_3");
    $0 = $("#ctl00_contentHolder_cbManageProductsDelete"); $0.attr("Privilege", "121_4");
    $0 = $("#ctl00_contentHolder_cbInStock"); $0.attr("Privilege", "121_5");
    $0 = $("#ctl00_contentHolder_cbManageProductsUp"); $0.attr("Privilege", "121_6");
    $0 = $("#ctl00_contentHolder_cbManageProductsDown"); $0.attr("Privilege", "121_7");

    $0 = $("#ctl00_contentHolder_cbProductUnclassified"); $0.attr("Privilege", "122");
    $0 = $("#ctl00_contentHolder_cbSubjectProducts"); $0.attr("Privilege", "123");
    $0 = $("#ctl00_contentHolder_cbProductBatchUpload"); $0.attr("Privilege", "124");
    $0 = $("#ctl00_contentHolder_cbProductBatchExport"); $0.attr("Privilege", "125")

    $0 = $("#ctl00_contentHolder_cbProductTypes"); $0.attr("Privilege", "126");
    $0 = $("#ctl00_contentHolder_cbProductTypesView"); $0.attr("Privilege", "126_1");
    $0 = $("#ctl00_contentHolder_cbProductTypesAdd"); $0.attr("Privilege", "126_2");
    $0 = $("#ctl00_contentHolder_cbProductTypesEdit"); $0.attr("Privilege", "126_3");
    $0 = $("#ctl00_contentHolder_cbProductTypesDelete"); $0.attr("Privilege", "126_4");

    $0 = $("#ctl00_contentHolder_cbManageCategories"); $0.attr("Privilege", "127");
    $0 = $("#ctl00_contentHolder_cbManageCategoriesView"); $0.attr("Privilege", "127_1");
    $0 = $("#ctl00_contentHolder_cbManageCategoriesAdd"); $0.attr("Privilege", "127_2");
    $0 = $("#ctl00_contentHolder_cbManageCategoriesEdit"); $0.attr("Privilege", "127_3");
    $0 = $("#ctl00_contentHolder_cbManageCategoriesDelete"); $0.attr("Privilege", "127_4");

    $0 = $("#ctl00_contentHolder_cbBrandCategories"); $0.attr("Privilege", "128");

    $0 = $("#ctl00_contentHolder_cbTopicManager"); $0.attr("Privilege", "129");
    $0 = $("#ctl00_contentHolder_cbTopicAdd"); $0.attr("Privilege", "129_1");
    $0 = $("#ctl00_contentHolder_cbTopicEdit"); $0.attr("Privilege", "129_2");
    $0 = $("#ctl00_contentHolder_cbTopicDelete"); $0.attr("Privilege", "129_3");

    //原产地
    $0 = $("#ctl00_contentHolder_cbImportSourceTypeManager"); $0.attr("Privilege", "120");
    $0 = $("#ctl00_contentHolder_cbImportSourceTypeView"); $0.attr("Privilege", "120_1");
    $0 = $("#ctl00_contentHolder_cbImportSourceTypeAdd"); $0.attr("Privilege", "120_2");
    $0 = $("#ctl00_contentHolder_cbImportSourceTypeEdit"); $0.attr("Privilege", "120_3");
    $0 = $("#ctl00_contentHolder_cbImportSourceTypeDelete"); $0.attr("Privilege", "120_4");

    //税率管理
    $0 = $("#ctl00_contentHolder_cbTaxRateManager"); $0.attr("Privilege", "128");
    $0 = $("#ctl00_contentHolder_cbTaxRateView"); $0.attr("Privilege", "128_1");
    $0 = $("#ctl00_contentHolder_cbTaxRateAdd"); $0.attr("Privilege", "128_2");
    $0 = $("#ctl00_contentHolder_cbTaxRateEdit"); $0.attr("Privilege", "128_3");
    $0 = $("#ctl00_contentHolder_cbTaxRateDelete"); $0.attr("Privilege", "128_4");

    //供货商管理
    $0 = $("#ctl00_contentHolder_cbSupplierManager"); $0.attr("Privilege", "1201");
    $0 = $("#ctl00_contentHolder_cbSupplierView"); $0.attr("Privilege", "1201_1");
    $0 = $("#ctl00_contentHolder_cbSupplierAdd"); $0.attr("Privilege", "1201_2");
    $0 = $("#ctl00_contentHolder_cbSupplierEdit"); $0.attr("Privilege", "1201_3");
    $0 = $("#ctl00_contentHolder_cbSupplierDelete"); $0.attr("Privilege", "1201_4");
    $0 = $("#ctl00_contentHolder_cbSupplierAddManager"); $0.attr("Privilege", "1201_5");

    //计量单位
    $0 = $("#ctl00_contentHolder_cbUnitsManager"); $0.attr("Privilege", "1202");
    $0 = $("#ctl00_contentHolder_cbUnitsView"); $0.attr("Privilege", "1202_1");
    $0 = $("#ctl00_contentHolder_cbUnitsAdd"); $0.attr("Privilege", "1202_2");
    $0 = $("#ctl00_contentHolder_cbUnitsEdit"); $0.attr("Privilege", "1202_3");
    $0 = $("#ctl00_contentHolder_cbUnitsDelete"); $0.attr("Privilege", "1202_4");

    //订单管理
    $0 = $("#ctl00_contentHolder_cbSales"); $0.attr("Privilege", "13");
    $0 = $("#ctl00_contentHolder_cbManageOrder"); $0.attr("Privilege", "131");
    $0 = $("#ctl00_contentHolder_cbManageOrderView"); $0.attr("Privilege", "131_1");
    $0 = $("#ctl00_contentHolder_cbManageOrderDelete"); $0.attr("Privilege", "131_2");
    $0 = $("#ctl00_contentHolder_cbManageOrderEdit"); $0.attr("Privilege", "131_3");
    $0 = $("#ctl00_contentHolder_cbManageOrderConfirm"); $0.attr("Privilege", "131_4");
    $0 = $("#ctl00_contentHolder_cbManageOrderSendedGoods"); $0.attr("Privilege", "131_5");
    $0 = $("#ctl00_contentHolder_cbExpressPrint"); $0.attr("Privilege", "131_6");
    $0 = $("#ctl00_contentHolder_cbManageOrderRefund"); $0.attr("Privilege", "131_7");
    $0 = $("#ctl00_contentHolder_cbManageOrderRemark"); $0.attr("Privilege", "131_8");

    $0 = $("#ctl00_contentHolder_cbExpressTemplates"); $0.attr("Privilege", "132");
    $0 = $("#ctl00_contentHolder_cbShipper"); $0.attr("Privilege", "133");

    $0 = $("#ctl00_contentHolder_cbOrderRefundApply"); $0.attr("Privilege", "134");
    $0 = $("#ctl00_contentHolder_cbOrderRefundApplyView"); $0.attr("Privilege", "134_1");
    $0 = $("#ctl00_contentHolder_cbOrderRefundApplyDelete"); $0.attr("Privilege", "134_2");
    $0 = $("#ctl00_contentHolder_cbOrderRefundApplyAccept"); $0.attr("Privilege", "134_3");
    $0 = $("#ctl00_contentHolder_cbOrderRefundApplyRefuse"); $0.attr("Privilege", "134_4");
    $0 = $("#ctl00_contentHolder_cbRefundOrderApplySelExcel"); $0.attr("Privilege", "134_5");
    $0 = $("#ctl00_contentHolder_cbRefundOrderApplyTimeExcel"); $0.attr("Privilege", "134_6");
    
    $0 = $("#ctl00_contentHolder_cbOrderReturnsApply"); $0.attr("Privilege", "135");
    $0 = $("#ctl00_contentHolder_cbOrderReturnsApplyView"); $0.attr("Privilege", "135_1");
    $0 = $("#ctl00_contentHolder_cbOrderReturnsApplyDelete"); $0.attr("Privilege", "135_2");
    $0 = $("#ctl00_contentHolder_cbOrderReturnsApplyAccept"); $0.attr("Privilege", "135_3");
    $0 = $("#ctl00_contentHolder_cbOrderReturnsApplyRefuse"); $0.attr("Privilege", "135_4");
    $0 = $("#ctl00_contentHolder_cbOrderReturnsApplySelExcel"); $0.attr("Privilege", "135_5");
    $0 = $("#ctl00_contentHolder_cbOrderReturnsApplyTimeExcel"); $0.attr("Privilege", "135_6");
    
    $0 = $("#ctl00_contentHolder_cbOrderReplaceApply"); $0.attr("Privilege", "136");
    $0 = $("#ctl00_contentHolder_cbOrderReplaceApplyView"); $0.attr("Privilege", "136_1");
    $0 = $("#ctl00_contentHolder_cbOrderReplaceApplyDelete"); $0.attr("Privilege", "136_2");
    $0 = $("#ctl00_contentHolder_cbOrderReplaceApplyAccept"); $0.attr("Privilege", "136_3");
    $0 = $("#ctl00_contentHolder_cbOrderReplaceApplyRefuse"); $0.attr("Privilege", "136_4");
    $0 = $("#ctl00_contentHolder_cbOrderReplaceApplySelExcel"); $0.attr("Privilege", "136_5");
    $0 = $("#ctl00_contentHolder_cbOrderReplaceApplyTimeExcel"); $0.attr("Privilege", "136_6");


    $0 = $("#ctl00_contentHolder_cbRefundOrder"); $0.attr("Privilege", "137");
    $0 = $("#ctl00_contentHolder_cbRefundOrderView"); $0.attr("Privilege", "137_1");
    $0 = $("#ctl00_contentHolder_cbRefundOrderDelete"); $0.attr("Privilege", "137_2");
    $0 = $("#ctl00_contentHolder_cbRefundOrderExcel"); $0.attr("Privilege", "137_3");

    $0 = $("#ctl00_contentHolder_cbReturnOrder"); $0.attr("Privilege", "138");
    $0 = $("#ctl00_contentHolder_cbReturnOrderView"); $0.attr("Privilege", "138_1");
    $0 = $("#ctl00_contentHolder_cbReturnOrderDelete"); $0.attr("Privilege", "138_2");
    $0 = $("#ctl00_contentHolder_cbReturnOrderExcel"); $0.attr("Privilege", "138_3");


    //会员管理
    $0 = $("#ctl00_contentHolder_cbManageUsers"); $0.attr("Privilege", "14");
    $0 = $("#ctl00_contentHolder_cbManageMembers"); $0.attr("Privilege", "141");
    $0 = $("#ctl00_contentHolder_cbManageMembersView"); $0.attr("Privilege", "141_1");
    $0 = $("#ctl00_contentHolder_cbManageMembersEdit"); $0.attr("Privilege", "141_2");
    $0 = $("#ctl00_contentHolder_cbManageMembersDelete"); $0.attr("Privilege", "141_3");

    $0 = $("#ctl00_contentHolder_cbMemberRanks"); $0.attr("Privilege", "142");
    $0 = $("#ctl00_contentHolder_cbMemberRanksView"); $0.attr("Privilege", "142_1");
    $0 = $("#ctl00_contentHolder_cbMemberRanksAdd"); $0.attr("Privilege", "142_2");
    $0 = $("#ctl00_contentHolder_cbMemberRanksEdit"); $0.attr("Privilege", "142_3");
    $0 = $("#ctl00_contentHolder_cbMemberRanksDelete"); $0.attr("Privilege", "142_4");

    $0 = $("#ctl00_contentHolder_cbOpenIdServices"); $0.attr("Privilege", "143");
    $0 = $("#ctl00_contentHolder_cbOpenIdSettings"); $0.attr("Privilege", "144");

    //CRM管理
    $0 = $("#ctl00_contentHolder_cbCRMmanager"); $0.attr("Privilege", "15");
    $0 = $("#ctl00_contentHolder_cbProductConsultationsManage"); $0.attr("Privilege", "151");
    $0 = $("#ctl00_contentHolder_cbProductReviewsManage"); $0.attr("Privilege", "152");
    $0 = $("#ctl00_contentHolder_cbReceivedMessages"); $0.attr("Privilege", "153");
    $0 = $("#ctl00_contentHolder_cbSendedMessages"); $0.attr("Privilege", "154");
    $0 = $("#ctl00_contentHolder_cbSendMessage"); $0.attr("Privilege", "155");
    $0 = $("#ctl00_contentHolder_cbManageLeaveComments"); $0.attr("Privilege", "156");
    $0 = $("#ctl00_contentHolder_cbMemberMarket"); $0.attr("Privilege", "157");
    $0 = $("#ctl00_contentHolder_cbClientGroup"); $0.attr("Privilege", "157_1");
    $0 = $("#ctl00_contentHolder_cbClientNew"); $0.attr("Privilege", "157_2");
    $0 = $("#ctl00_contentHolder_cbClientActivy"); $0.attr("Privilege", "157_3");
    $0 = $("#ctl00_contentHolder_cbClientSleep"); $0.attr("Privilege", "157_4");


    //营销推广
    $0 = $("#ctl00_contentHolder_cbMarketing"); $0.attr("Privilege", "16");
    $0 = $("#ctl00_contentHolder_cbGifts"); $0.attr("Privilege", "161");
    $0 = $("#ctl00_contentHolder_cbProductPromotion"); $0.attr("Privilege", "162");
    $0 = $("#ctl00_contentHolder_cbOrderPromotion"); $0.attr("Privilege", "163");
    $0 = $("#ctl00_contentHolder_cbOrderPromotion"); $0.attr("Privilege", "164");
    $0 = $("#ctl00_contentHolder_cbBundPromotion"); $0.attr("Privilege", "165");
    $0 = $("#ctl00_contentHolder_cbGroupBuy"); $0.attr("Privilege", "166");
    $0 = $("#ctl00_contentHolder_cbCountDown"); $0.attr("Privilege", "167");
    $0 = $("#ctl00_contentHolder_cbCoupons"); $0.attr("Privilege", "168");


    //财务统计
    $0 = $("#ctl00_contentHolder_cbFinancial"); $0.attr("Privilege", "17");
    $0 = $("#ctl00_contentHolder_cbAccountSummary"); $0.attr("Privilege", "171");
    $0 = $("#ctl00_contentHolder_cbReCharge"); $0.attr("Privilege", "172");
    $0 = $("#ctl00_contentHolder_cbBalanceDrawRequest"); $0.attr("Privilege", "173");
    $0 = $("#ctl00_contentHolder_cbBalanceDetailsStatistics"); $0.attr("Privilege", "174");
    $0 = $("#ctl00_contentHolder_cbBalanceDrawRequestStatistics"); $0.attr("Privilege", "175");

    //对账订单报表
    $0 = $("#ctl00_contentHolder_cbReconciOrders"); $0.attr("Privilege", "176");
    $0 = $("#ctl00_contentHolder_cbReconciOrdersView"); $0.attr("Privilege", "176_1");
    $0 = $("#ctl00_contentHolder_cbReconciOrdersCreateReport"); $0.attr("Privilege", "176_2");

    //对账订单明细报表
    $0 = $("#ctl00_contentHolder_cbReconciOrdersDetails"); $0.attr("Privilege", "177");
    $0 = $("#ctl00_contentHolder_cbReconciOrdersDetailsView"); $0.attr("Privilege", "177_1");
    $0 = $("#ctl00_contentHolder_cbReconciOrdersDetailsCreateReport"); $0.attr("Privilege", "177_2");

    //平台销售额及应付总汇
    $0 = $("#ctl00_contentHolder_cbAggregatePayable"); $0.attr("Privilege", "178");

    //统计报表
    $0 = $("#ctl00_contentHolder_cbTotalReport"); $0.attr("Privilege", "18");
    $0 = $("#ctl00_contentHolder_cbSaleTotalStatistics"); $0.attr("Privilege", "181");
    $0 = $("#ctl00_contentHolder_cbUserOrderStatistics"); $0.attr("Privilege", "182");
    $0 = $("#ctl00_contentHolder_cbSaleList"); $0.attr("Privilege", "183");
    $0 = $("#ctl00_contentHolder_cbSaleTargetAnalyse"); $0.attr("Privilege", "184");

    $0 = $("#ctl00_contentHolder_cbProductSaleRanking"); $0.attr("Privilege", "185");
    $0 = $("#ctl00_contentHolder_cbProductSaleStatistics"); $0.attr("Privilege", "186");
    $0 = $("#ctl00_contentHolder_cbMemberRanking"); $0.attr("Privilege", "187");
    $0 = $("#ctl00_contentHolder_cbMemberArealDistributionStatistics"); $0.attr("Privilege", "188");
    $0 = $("#ctl00_contentHolder_cbUserIncreaseStatistics"); $0.attr("Privilege", "189");
    $0 = $("#ctl00_contentHolder_cbPurchaseOrderStatistics"); $0.attr("Privilege", "1810");

    $0 = $("#ctl00_contentHolder_cbCustomService"); $0.attr("Privilege", "1811");
    $0 = $("#ctl00_contentHolder_cbCustomServiceView"); $0.attr("Privilege", "1811_1");
    $0 = $("#ctl00_contentHolder_cbCustomServiceCreateReport"); $0.attr("Privilege", "1811_2");

    
    $0 = $("#ctl00_contentHolder_cbSuppliersSales"); $0.attr("Privilege", "1812");
    $0 = $("#ctl00_contentHolder_cbSuppliersSalesView"); $0.attr("Privilege", "1812_1");
    $0 = $("#ctl00_contentHolder_cbSuppliersSalesCreateReport"); $0.attr("Privilege", "1812_2");

    $0 = $("#ctl00_contentHolder_cbProductSaleAsBrand"); $0.attr("Privilege", "1813");
    $0 = $("#ctl00_contentHolder_cbProductSaleAsBrandView"); $0.attr("Privilege", "1813_1");
    $0 = $("#ctl00_contentHolder_cbProductSaleAsBrandExcel"); $0.attr("Privilege", "1813_2");

    $0 = $("#ctl00_contentHolder_cbProductSaleAsImportSource"); $0.attr("Privilege", "1814");
    $0 = $("#ctl00_contentHolder_cbProductSaleAsImportSourceView"); $0.attr("Privilege", "1814_1");
    $0 = $("#ctl00_contentHolder_cbProductSaleAsImportSourceExcel"); $0.attr("Privilege", "1814_2");
    
    //微商城
    $0 = $("#ctl00_contentHolder_cbManageVShop"); $0.attr("Privilege", "19");
    $0 = $("#ctl00_contentHolder_cbVServerConfig"); $0.attr("Privilege", "191");
    $0 = $("#ctl00_contentHolder_cbManageMenu"); $0.attr("Privilege", "192");
    $0 = $("#ctl00_contentHolder_cbAddMenu"); $0.attr("Privilege", "192_1");
    $0 = $("#ctl00_contentHolder_cbEditMenu"); $0.attr("Privilege", "192_2");
    $0 = $("#ctl00_contentHolder_cbDeleteMenu"); $0.attr("Privilege", "192_3");

    $0 = $("#ctl00_contentHolder_cbCustomeReplyManager"); $0.attr("Privilege", "193");
    $0 = $("#ctl00_contentHolder_cbDeleteCustomeReply"); $0.attr("Privilege", "193_1");

    $0 = $("#ctl00_contentHolder_cbAddReplyOnkey"); $0.attr("Privilege", "193_2");
    $0 = $("#ctl00_contentHolder_cbEditReplyOnkey"); $0.attr("Privilege", "193_3");
    $0 = $("#ctl00_contentHolder_cbAddArticle"); $0.attr("Privilege", "193_4");
    $0 = $("#ctl00_contentHolder_cbEditArticle"); $0.attr("Privilege", "193_5");
    $0 = $("#ctl00_contentHolder_cbMutiArticleAdd"); $0.attr("Privilege", "193_6");
    $0 = $("#ctl00_contentHolder_cbMutiArticleEdit"); $0.attr("Privilege", "193_7");

    $0 = $("#ctl00_contentHolder_cbTopicList"); $0.attr("Privilege", "194");
    $0 = $("#ctl00_contentHolder_cbManageVThemes"); $0.attr("Privilege", "195");
    $0 = $("#ctl00_contentHolder_cbSetVThemes"); $0.attr("Privilege", "196");
    $0 = $("#ctl00_contentHolder_cbvIconSet"); $0.attr("Privilege", "197");
    $0 = $("#ctl00_contentHolder_cbvProductSet"); $0.attr("Privilege", "198");

    $0 = $("#ctl00_contentHolder_cbvBannerManage"); $0.attr("Privilege", "199");
    $0 = $("#ctl00_contentHolder_cbvBannerAdd"); $0.attr("Privilege", "199_1");
    $0 = $("#ctl00_contentHolder_cbvBannerEdit"); $0.attr("Privilege", "199_2");
    $0 = $("#ctl00_contentHolder_cbvBannerDelete"); $0.attr("Privilege", "199_3");

    $0 = $("#ctl00_contentHolder_cbvIsViewEdit"); $0.attr("Privilege", "190");

    //$0 = $("#ctl00_contentHolder_cbManageVThemes"); $0.attr("Privilege", "1910");
    //$0 = $("#ctl00_contentHolder_cbSetVThemes"); $0.attr("Privilege", "1911");

    $0 = $("#ctl00_contentHolder_cbManageLotteryActivity"); $0.attr("Privilege", "1912");
    $0 = $("#ctl00_contentHolder_cbAddLotteryActivity"); $0.attr("Privilege", "1912_1");
    $0 = $("#ctl00_contentHolder_cbEditLotteryActivity"); $0.attr("Privilege", "1912_2");
    $0 = $("#ctl00_contentHolder_cbDeleteLotteryActivity"); $0.attr("Privilege", "1912_3");
    $0 = $("#ctl00_contentHolder_cbManageActivity"); $0.attr("Privilege", "1913");
    $0 = $("#ctl00_contentHolder_cbAddActivity"); $0.attr("Privilege", "1913_1");
    $0 = $("#ctl00_contentHolder_cbEditActivity"); $0.attr("Privilege", "1913_2");
    $0 = $("#ctl00_contentHolder_cbDeleteActivity"); $0.attr("Privilege", "1913_3");
    $0 = $("#ctl00_contentHolder_cbManageLotteryTicket"); $0.attr("Privilege", "1914");
    $0 = $("#ctl00_contentHolder_cbAddLotteryTicket"); $0.attr("Privilege", "1914_1");
    $0 = $("#ctl00_contentHolder_cbEditLotteryTicket"); $0.attr("Privilege", "1914_2");
    $0 = $("#ctl00_contentHolder_cbDeleteLotteryTicket"); $0.attr("Privilege", "1914_3");
    $0 = $("#ctl00_contentHolder_cbVpayConfig"); $0.attr("Privilege", "1915");
    $0 = $("#ctl00_contentHolder_cbvMobileAlipaySet"); $0.attr("Privilege", "1916");
    $0 = $("#ctl00_contentHolder_cbvShengPaySet"); $0.attr("Privilege", "1917");
    $0 = $("#ctl00_contentHolder_cbvOfflinePaySet"); $0.attr("Privilege", "1918");

    //触屏版

    $0 = $("#ctl00_contentHolder_cbWapManage"); $0.attr("Privilege", "20");
    $0 = $("#ctl00_contentHolder_cbWapHomeTopicSet"); $0.attr("Privilege", "201");
    $0 = $("#ctl00_contentHolder_cbWapManageThemes"); $0.attr("Privilege", "202");
    $0 = $("#ctl00_contentHolder_cbWapThemesSet"); $0.attr("Privilege", "203");
    $0 = $("#ctl00_contentHolder_cbWapIconSet"); $0.attr("Privilege", "204");
    $0 = $("#ctl00_contentHolder_cbWapProductSet"); $0.attr("Privilege", "205");

    $0 = $("#ctl00_contentHolder_cbWapBannerManage"); $0.attr("Privilege", "206");
    $0 = $("#ctl00_contentHolder_cbWapBannerAdd"); $0.attr("Privilege", "206_1");
    $0 = $("#ctl00_contentHolder_cbWapBannerEdit"); $0.attr("Privilege", "206_2");
    $0 = $("#ctl00_contentHolder_cbWapBannerDelete"); $0.attr("Privilege", "206_3");

    $0 = $("#ctl00_contentHolder_cbWapMobileAlipaySet"); $0.attr("Privilege", "207");
    $0 = $("#ctl00_contentHolder_cbWapShengPaySet"); $0.attr("Privilege", "208");
    $0 = $("#ctl00_contentHolder_cbWapOfflinePaySet"); $0.attr("Privilege", "209");
    //APP版
    $0 = $("#ctl00_contentHolder_cbAppManage"); $0.attr("Privilege", "21");
    $0 = $("#ctl00_contentHolder_cbAppHomeTopicSet"); $0.attr("Privilege", "211");
    $0 = $("#ctl00_contentHolder_cbAppProductSet"); $0.attr("Privilege", "212");

    $0 = $("#ctl00_contentHolder_cbAppADImageSet"); $0.attr("Privilege", "213");
    $0 = $("#ctl00_contentHolder_cbAppADImageAdd"); $0.attr("Privilege", "213_1");
    $0 = $("#ctl00_contentHolder_cbAppADImageEdit"); $0.attr("Privilege", "213_2");
    $0 = $("#ctl00_contentHolder_cbAppADImageDelete"); $0.attr("Privilege", "213_3");

    $0 = $("#ctl00_contentHolder_cbAndroidUP"); $0.attr("Privilege", "214");
    $0 = $("#ctl00_contentHolder_cbIOSUP"); $0.attr("Privilege", "215");

    $0 = $("#ctl00_contentHolder_cbAppMobileAlipaySet"); $0.attr("Privilege", "217");
    $0 = $("#ctl00_contentHolder_cbAppShengPaySet"); $0.attr("Privilege", "218");
    $0 = $("#ctl00_contentHolder_cbAppOfflinePaySet"); $0.attr("Privilege", "219");

    //服务窗
    $0 = $("#ctl00_contentHolder_cbAliohManage"); $0.attr("Privilege", "22");
    $0 = $("#ctl00_contentHolder_cbAliohBaseConfig"); $0.attr("Privilege", "221");
    $0 = $("#ctl00_contentHolder_cbAliohManageMenu"); $0.attr("Privilege", "222");
    $0 = $("#ctl00_contentHolder_cbAliohAddMenu"); $0.attr("Privilege", "222_1");
    $0 = $("#ctl00_contentHolder_cbAliohEditMenu"); $0.attr("Privilege", "222_2");
    $0 = $("#ctl00_contentHolder_cbAliohDeleteMenu"); $0.attr("Privilege", "222_3");

    $0 = $("#ctl00_contentHolder_cbAliohHomeTopicSet"); $0.attr("Privilege", "223");
    $0 = $("#ctl00_contentHolder_cbAliohManageThemes"); $0.attr("Privilege", "224");
    $0 = $("#ctl00_contentHolder_cbAliohThemesSet"); $0.attr("Privilege", "225");
    $0 = $("#ctl00_contentHolder_cbAliohIconSet"); $0.attr("Privilege", "226");
    $0 = $("#ctl00_contentHolder_cbAliohProductSet"); $0.attr("Privilege", "227");

    $0 = $("#ctl00_contentHolder_cbAliohBannerManage"); $0.attr("Privilege", "228");
    $0 = $("#ctl00_contentHolder_cbAliohBannerAdd"); $0.attr("Privilege", "228_1");
    $0 = $("#ctl00_contentHolder_cbAliohBannerEdit"); $0.attr("Privilege", "228_2");
    $0 = $("#ctl00_contentHolder_cbAliohBannerDelete"); $0.attr("Privilege", "228_3");

    $0 = $("#ctl00_contentHolder_cbAliohMobileAlipaySet"); $0.attr("Privilege", "229");
    $0 = $("#ctl00_contentHolder_cbAliohShengPaySet"); $0.attr("Privilege", "2210");
    $0 = $("#ctl00_contentHolder_cbAliohOfflinePaySet"); $0.attr("Privilege", "2211");

    //站点管理
    $0 = $("#ctl00_contentHolder_cbSitesManager"); $0.attr("Privilege", "23");
    $0 = $("#ctl00_contentHolder_cbSites"); $0.attr("Privilege", "230");
    $0 = $("#ctl00_contentHolder_cbSitesView"); $0.attr("Privilege", "230_1");
    $0 = $("#ctl00_contentHolder_cbSitesAdd"); $0.attr("Privilege", "230_2");
    $0 = $("#ctl00_contentHolder_cbSitesEdit"); $0.attr("Privilege", "230_3");
    $0 = $("#ctl00_contentHolder_cbSitesDelete"); $0.attr("Privilege", "230_4");

    //门店管理
    $0 = $("#ctl00_contentHolder_cbStoreManager"); $0.attr("Privilege", "24");

    //门店管理
    $0 = $("#ctl00_contentHolder_cbStore"); $0.attr("Privilege", "240");
    $0 = $("#ctl00_contentHolder_cbStoreView"); $0.attr("Privilege", "240_1");
    $0 = $("#ctl00_contentHolder_cbStoreAdd"); $0.attr("Privilege", "240_2");
    $0 = $("#ctl00_contentHolder_cbStoreEdit"); $0.attr("Privilege", "240_3");
    $0 = $("#ctl00_contentHolder_cbStoreDelete"); $0.attr("Privilege", "240_4");
    $0 = $("#ctl00_contentHolder_cbStoreRelatedProduct"); $0.attr("Privilege", "240_5");
    $0 = $("#ctl00_contentHolder_cbStoreAddManager"); $0.attr("Privilege", "240_6");

    //门店订单管理
    $0 = $("#ctl00_contentHolder_cbStoreOrder"); $0.attr("Privilege", "241");
    $0 = $("#ctl00_contentHolder_cbStoreOrderView"); $0.attr("Privilege", "241_1");
    $0 = $("#ctl00_contentHolder_cbStoreOrderDelete"); $0.attr("Privilege", "241_2");
    $0 = $("#ctl00_contentHolder_cbStoreOrderClose"); $0.attr("Privilege", "241_3");
    $0 = $("#ctl00_contentHolder_cbStoreOrderSend"); $0.attr("Privilege", "241_4");
    $0 = $("#ctl00_contentHolder_cbStoreOrderPrintPosts"); $0.attr("Privilege", "241_5");
    $0 = $("#ctl00_contentHolder_cbStoreOrderPrintGoods"); $0.attr("Privilege", "241_6");
    $0 = $("#ctl00_contentHolder_cbStoreOrderReplaceAccept"); $0.attr("Privilege", "241_7");
    $0 = $("#ctl00_contentHolder_cbStoreOrderReplaceRefuse"); $0.attr("Privilege", "241_8");
    $0 = $("#ctl00_contentHolder_cbStoreOrderReturnAccept"); $0.attr("Privilege", "241_9");
    $0 = $("#ctl00_contentHolder_cbStoreOrderReturnRefuse"); $0.attr("Privilege", "241_10");
    $0 = $("#ctl00_contentHolder_cbStoreOrderRefundAccept"); $0.attr("Privilege", "241_11");
    $0 = $("#ctl00_contentHolder_cbStoreOrderRefundRefuse"); $0.attr("Privilege", "241_12");
    $0 = $("#ctl00_contentHolder_cbStoreOrderBatchSend"); $0.attr("Privilege", "241_13");
    $0 = $("#ctl00_contentHolder_cbStoreOrderUploadGoods"); $0.attr("Privilege", "241_14");
    $0 = $("#ctl00_contentHolder_cbStoreOrderAdd"); $0.attr("Privilege", "241_15");
    $0 = $("#ctl00_contentHolder_cbSkusSelect"); $0.attr("Privilege", "241_16");
    $0 = $("#ctl00_contentHolder_cbShipAddressSelect"); $0.attr("Privilege", "241_17");

    $0 = $("#ctl00_contentHolder_cbStoreOrderDown"); $0.attr("Privilege", "242");
    $0 = $("#ctl00_contentHolder_cbStoreOrderDownSelExcel"); $0.attr("Privilege", "242_1");
    $0 = $("#ctl00_contentHolder_cbStoreOrderDownTimeExcel"); $0.attr("Privilege", "242_2");
    $0 = $("#ctl00_contentHolder_cbStoreOrderDownGoodsPick"); $0.attr("Privilege", "242_3");

    $0 = $("#ctl00_contentHolder_cbStoreOrderClear"); $0.attr("Privilege", "243");
    $0 = $("#ctl00_contentHolder_cbStoreOrderClearSelExcel"); $0.attr("Privilege", "243_1");
    $0 = $("#ctl00_contentHolder_cbStoreOrderClearTimeExcel"); $0.attr("Privilege", "243_2");
    $0 = $("#ctl00_contentHolder_cbStoreOrderClearGoodsPick"); $0.attr("Privilege", "243_3");

    //供应商
    $0 = $("#ctl00_contentHolder_cbSupplierManage"); $0.attr("Privilege", "25");

    //订单管理
    $0 = $("#ctl00_contentHolder_cbSupplierOrder"); $0.attr("Privilege", "250");
    $0 = $("#ctl00_contentHolder_cbSupplierOrderView"); $0.attr("Privilege", "250_1");
    $0 = $("#ctl00_contentHolder_cbSupplierOrderAdd"); $0.attr("Privilege", "250_2");
    $0 = $("#ctl00_contentHolder_cbSupplierOrderDelete"); $0.attr("Privilege", "250_3");
    $0 = $("#ctl00_contentHolder_cbSupplierOrderClose"); $0.attr("Privilege", "250_4");
    $0 = $("#ctl00_contentHolder_cbSupplierOrderSend"); $0.attr("Privilege", "250_5");
    $0 = $("#ctl00_contentHolder_cbSupplierOrderReplaceAccept"); $0.attr("Privilege", "250_6");
    $0 = $("#ctl00_contentHolder_cbSupplierOrderReplaceRefuse"); $0.attr("Privilege", "250_7");
    $0 = $("#ctl00_contentHolder_cbSupplierOrderReturnAccept"); $0.attr("Privilege", "250_8");
    $0 = $("#ctl00_contentHolder_cbSupplierOrderReturnRefuse"); $0.attr("Privilege", "250_9");
    $0 = $("#ctl00_contentHolder_cbSupplierOrderRefundAccept"); $0.attr("Privilege", "250_10");
    $0 = $("#ctl00_contentHolder_cbSupplierOrderRefundRefuse"); $0.attr("Privilege", "250_11");
    $0 = $("#ctl00_contentHolder_cbSupplierOrderPrintPosts"); $0.attr("Privilege", "250_12");
    $0 = $("#ctl00_contentHolder_cbSupplierOrderPrintGoods"); $0.attr("Privilege", "250_13");
    $0 = $("#ctl00_contentHolder_cbSupplierOrderBatchSend"); $0.attr("Privilege", "250_14");
    $0 = $("#ctl00_contentHolder_cbSupplierOrderUploadGoods"); $0.attr("Privilege", "250_15");
    $0 = $("#ctl00_contentHolder_cbSupplierOrderRemark"); $0.attr("Privilege", "250_16");

    $0 = $("#ctl00_contentHolder_cbSupplierOrderDown"); $0.attr("Privilege", "251");
    $0 = $("#ctl00_contentHolder_cbSupplierOrderDownSelExcel"); $0.attr("Privilege", "251_1");
    $0 = $("#ctl00_contentHolder_cbSupplierOrderDownTimeExcel"); $0.attr("Privilege", "251_2");
    $0 = $("#ctl00_contentHolder_cbSupplierOrderDownGoodsPick"); $0.attr("Privilege", "251_3");

    $0 = $("#ctl00_contentHolder_cbSupplierOrderClear"); $0.attr("Privilege", "252");
    $0 = $("#ctl00_contentHolder_cbSupplierOrderClearSelExcel"); $0.attr("Privilege", "252_1");
    $0 = $("#ctl00_contentHolder_cbSupplierOrderClearTimeExcel"); $0.attr("Privilege", "252_2");
    $0 = $("#ctl00_contentHolder_cbSupplierOrderClearGoodsPick"); $0.attr("Privilege", "252_3");


    //商品管理
    $0 = $("#ctl00_contentHolder_cbSupplierProductManage"); $0.attr("Privilege", "253");
    $0 = $("#ctl00_contentHolder_cbSupplierProductView"); $0.attr("Privilege", "253_1");
    $0 = $("#ctl00_contentHolder_cbSupplierProductAdd"); $0.attr("Privilege", "253_2");
    $0 = $("#ctl00_contentHolder_cbSupplierProductEdit"); $0.attr("Privilege", "253_3");
    $0 = $("#ctl00_contentHolder_cbSupplierProductDelete"); $0.attr("Privilege", "253_4");
    $0 = $("#ctl00_contentHolder_cbSupplierProductInStock"); $0.attr("Privilege", "253_5");
    $0 = $("#ctl00_contentHolder_cbSupplierProductUp"); $0.attr("Privilege", "253_6");
    $0 = $("#ctl00_contentHolder_cbSupplierProductDown"); $0.attr("Privilege", "253_7");

    // 加载后的全选操作 ------------------------------------------------------------------------------------------------------------------------------------

    showOneLayerOnLoad();

    function showOneLayerOnLoad() {

        var flag;

        for (var i = 10; i <= 25; i++) {
            $_Control1 = $("input[type='checkbox'][Privilege='" + i + "']");
            var result1 = showTwoLayerOnLoad(i);
            // 如果当前一级下没有下级则判断自己，如果没有选择设置为 false            
            if (result1 == "no" && !$_Control1.attr("checked"))
                flag = false;
            else if (result1)
                $_Control1.attr("checked", true);
            else
                flag = false;
        }
        if (flag)
            $("input[type='checkbox'][Privilege='0']").attr("checked", true);
    };

    function showTwoLayerOnLoad(one) {

        var flag2 = true;
        for (var j = 1; j <= 15; j++) {

            $_Control2 = $("input[type='checkbox'][Privilege='" + one + j + "']");

            // 如果当前一级下没有下级则返回 no ,告诉上级无下级
            if ($_Control2.attr("id") == undefined && j == 1) {
                flag2 = "no";
                return flag2;
            }
            // 如果已经循环到尽头则返回结果
            else if ($_Control2.attr("id") == undefined) {
                return flag2;
            }
            // 如果有下级且没到尽头继续操作
            else if ($_Control2.attr("id") != undefined) {

                // 判断当前的二级下的三级情况
                var result2 = showTheeLayerOnLoad(one, j);


                // 如果当前二级下没有三级则判断自己,如果没选择设置为 false
                if (result2 == "no" && !$_Control2.attr("checked"))
                    flag2 = false;
                else if (result2)
                    $_Control2.attr("checked", true);
                else
                    flag2 = false;
            }
        }

        return flag2;
    };

    function showTheeLayerOnLoad(one, two) {

        var flag3 = true;
        for (var k = 1; k <= 15; k++) {

            $_Control3 = $("input[type='checkbox'][Privilege$='" + one + two + "_" + k + "']");

            // 如果当前二级下没有下级则返回 no ,告诉上级无下级
            if ($_Control3.attr("id") == undefined && k == 1)
                return "no";
            // 如果已经循环到尽头则返回结果
            else if ($_Control3.attr("id") == undefined)
                return flag3;
            // 如果有下级且没到尽头继续操作
            else if ($_Control3.attr("id") != undefined && !$_Control3.attr("checked"))
                flag3 = false;
        }

        return flag3;
    };

    // 单击触发事件 -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    $("input[type='checkbox']").bind('click', function () {

        var value = this.checked;
        // 全选
        if ($(this).attr("privilege") == "0")
            $("input[type='checkbox']").attr("checked", value);
        // 一层的选择
        else if ($(this).attr("privilege") >= 10 && $(this).attr("privilege") <= 25) {
            // 没有被选择时
            if (!value) {
                $("input[type='checkbox'][Privilege='0']").attr("checked", false);
                $("input[type='checkbox'][Privilege^='" + $(this).attr("privilege") + "']").attr("checked", value);
            }
            // 选择时
            else {
                if (IsOneLayerAllChecked()) {
                    $("input[type='checkbox'][Privilege='0']").attr("checked", true);
                }
                $("input[type='checkbox'][Privilege^='" + $(this).attr("privilege") + "']").attr("checked", value);
            }
            //showTheeLayer2($(this).attr("privilege"), value);
        }
        // 二层的选择
        else if ($(this).attr("privilege").length = 3 && $(this).attr("privilege") > 100) {
            // 没有被选择时
            if (!value) {
                $("input[type='checkbox'][Privilege='0']").attr("checked", false);
                $("input[type='checkbox'][Privilege='" + $(this).attr("privilege").substring(0, 1) + "']").attr("checked", false);
            }
            // 选择时
            else {
                if (IsTwoLayerAllCheckedOfOne($(this).attr("privilege").substring(0, 1)))
                    $("input[type='checkbox'][Privilege='" + $(this).attr("privilege").substring(0, 1) + "']").attr("checked", true);
                if (IsOneLayerAllChecked())
                    $("input[type='checkbox'][Privilege='0']").attr("checked", true);
            }
            showTheeLayer2($(this).attr("privilege"), value);
        }
        // 三层的选择
        else {
            // 没有被选择时
            if (!value) {
                $("input[type='checkbox'][Privilege='0']").attr("checked", false);
                $("input[type='checkbox'][Privilege='" + $(this).attr("privilege").substring(0, 1) + "']").attr("checked", false);
                $("input[type='checkbox'][Privilege='" + $(this).attr("privilege").substring(0, this.Privilege.indexOf("_")) + "']").attr("checked", false);
            }
            // 选择时
            else {
                if (IsThreeLayerAllCheckedOfTwo($(this).attr("privilege").substring(0, $(this).attr("privilege").indexOf("_"))))
                    $("input[type='checkbox'][Privilege='" + $(this).attr("privilege").substring(0, $(this).attr("privilege").indexOf("_")) + "']").attr("checked", true);
                if (IsTwoLayerAllCheckedOfOne($(this).attr("privilege").substring(0, 1)))
                    $("input[type='checkbox'][Privilege='" + $(this).attr("privilege").substring(0, 1) + "']").attr("checked", true);
                if (IsOneLayerAllChecked())
                    $("input[type='checkbox'][Privilege='0']").attr("checked", true);
            }
        }
    })

    // 选择后判断父类是否应该被选择-----------------------------------------------------------------------------------------------------------------------------------------

    // 判断一层是否都选中了
    var IsOneLayerAllChecked = function () {
        for (var i = 10; i <= 25; i++) {
            if (!$("input[type='checkbox'][Privilege='" + i + "']").attr("checked")) {
                return false;
            }
        }
        return true;
    }

    // 判断某一层下的二层是否都选中了
    var IsTwoLayerAllCheckedOfOne = function (one) {
        for (var i = 1; i <= 15; i++) {
            $_Control2 = $("input[type='checkbox'][Privilege='" + one + i + "']");
            if ($_Control2.attr("id") == undefined) {
                break;
            }

            if (!$_Control2.attr("checked")) {
                return false;
            }
        }
        return true;
    }

    // 判断某二层下的三层是否都选中了
    var IsThreeLayerAllCheckedOfTwo = function (two) {
        for (var i = 1; i <= 15; i++) {
            $_Control3 = $("input[type='checkbox'][Privilege='" + two + "_" + i + "']");

            if ($_Control3.attr("id") == undefined) {
                break;
            }

            if (!$_Control3.attr("checked")) {
                return false;
            }
        }
        return true;
    }

    // 全选反选操作-----------------------------------------------------------------------------------------------------------------------------------------------------------------

    //    var showOneLayer = function(one,value) { 
    //    
    //        for(var i=1;i<=10;i++)
    //        {
    //            
    //            $("input[type='checkbox'][Privilege='"+i+"']).attr("checked",value)"); 
    //                      
    //            showTwoLayer(i,value);
    //        } 
    //    };

    //    var showTwoLayer = function(one,value) { 
    //    
    //        for(var j=1;j<=13;j++)
    //        {
    //            
    //            $_Control1=$("input[type='checkbox'][Privilege='"+one+j+"']"); 
    //                      
    //            if ($_Control1.attr("id")==undefined)
    //            {
    //                break;
    //            }
    //            $_Control1.attr("checked",value);
    //            
    //            showTheeLayer(one,j ,value);
    //        } 
    //    };

    var showTheeLayer = function (one, two, value) {

        for (var k = 1; k <= 15; k++) {

            $_Control2 = $("input[type='checkbox'][Privilege$='" + one + two + "_" + k + "']");

            if ($_Control2.attr("id") == undefined) {
                break;
            }
            $_Control2.attr("checked", value);

        }
    };

    var showTheeLayer2 = function (two, value) {

        for (var k = 1; k <= 15; k++) {

            $_Control2 = $("input[type='checkbox'][Privilege$='" + two + "_" + k + "']");

            if ($_Control2.attr("id") == undefined) {
                break;
            }
            $_Control2.attr("checked", value);

        }
    };

}
);
  