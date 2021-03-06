/****** Object:  ForeignKey [FK__aspnet_Us__RoleI__31EC6D26]    Script Date: 12/10/2014 10:04:31 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__aspnet_Us__RoleI__31EC6D26]') AND parent_object_id = OBJECT_ID(N'[dbo].[aspnet_UsersInRoles]'))
ALTER TABLE [dbo].[aspnet_UsersInRoles] DROP CONSTRAINT [FK__aspnet_Us__RoleI__31EC6D26]
GO
/****** Object:  ForeignKey [FK__aspnet_Us__UserI__30F848ED]    Script Date: 12/10/2014 10:04:31 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__aspnet_Us__UserI__30F848ED]') AND parent_object_id = OBJECT_ID(N'[dbo].[aspnet_UsersInRoles]'))
ALTER TABLE [dbo].[aspnet_UsersInRoles] DROP CONSTRAINT [FK__aspnet_Us__UserI__30F848ED]
GO
/****** Object:  ForeignKey [FK_Ecshop_Articles__ArticleCategories]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_Articles__ArticleCategories]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_Articles]'))
ALTER TABLE [dbo].[Ecshop_Articles] DROP CONSTRAINT [FK_Ecshop_Articles__ArticleCategories]
GO
/****** Object:  ForeignKey [FK_Ecshop_Attributes_ProductTypes]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_Attributes_ProductTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_Attributes]'))
ALTER TABLE [dbo].[Ecshop_Attributes] DROP CONSTRAINT [FK_Ecshop_Attributes_ProductTypes]
GO
/****** Object:  ForeignKey [FK_Ecshop_AttributeValues_Attributes]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_AttributeValues_Attributes]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_AttributeValues]'))
ALTER TABLE [dbo].[Ecshop_AttributeValues] DROP CONSTRAINT [FK_Ecshop_AttributeValues_Attributes]
GO
/****** Object:  ForeignKey [FK_Ecshop_BalanceDetails_aspnet_Memberss]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_BalanceDetails_aspnet_Memberss]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_BalanceDetails]'))
ALTER TABLE [dbo].[Ecshop_BalanceDetails] DROP CONSTRAINT [FK_Ecshop_BalanceDetails_aspnet_Memberss]
GO
/****** Object:  ForeignKey [FK_Ecshop_BalanceDrawRequest_aspnet_Memberss]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_BalanceDrawRequest_aspnet_Memberss]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_BalanceDrawRequest]'))
ALTER TABLE [dbo].[Ecshop_BalanceDrawRequest] DROP CONSTRAINT [FK_Ecshop_BalanceDrawRequest_aspnet_Memberss]
GO
/****** Object:  ForeignKey [FK_BundlingProductsItems_Ecshop_BundlingProducts]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_BundlingProductsItems_Ecshop_BundlingProducts]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_BundlingProductItems]'))
ALTER TABLE [dbo].[Ecshop_BundlingProductItems] DROP CONSTRAINT [FK_BundlingProductsItems_Ecshop_BundlingProducts]
GO
/****** Object:  ForeignKey [FK_Ecshop_CountDown_Products]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_CountDown_Products]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_CountDown]'))
ALTER TABLE [dbo].[Ecshop_CountDown] DROP CONSTRAINT [FK_Ecshop_CountDown_Products]
GO
/****** Object:  ForeignKey [FK_Ecshop_CouponItems__Coupons]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_CouponItems__Coupons]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_CouponItems]'))
ALTER TABLE [dbo].[Ecshop_CouponItems] DROP CONSTRAINT [FK_Ecshop_CouponItems__Coupons]
GO
/****** Object:  ForeignKey [FK_Ecshop_Favorite_aspnet_Members]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_Favorite_aspnet_Members]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_Favorite]'))
ALTER TABLE [dbo].[Ecshop_Favorite] DROP CONSTRAINT [FK_Ecshop_Favorite_aspnet_Members]
GO
/****** Object:  ForeignKey [FK_Ecshop_GiftShoppingCarts_aspnet_Members]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_GiftShoppingCarts_aspnet_Members]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_GiftShoppingCarts]'))
ALTER TABLE [dbo].[Ecshop_GiftShoppingCarts] DROP CONSTRAINT [FK_Ecshop_GiftShoppingCarts_aspnet_Members]
GO
/****** Object:  ForeignKey [FK_Ecshop_GroupBuy_Products]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_GroupBuy_Products]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_GroupBuy]'))
ALTER TABLE [dbo].[Ecshop_GroupBuy] DROP CONSTRAINT [FK_Ecshop_GroupBuy_Products]
GO
/****** Object:  ForeignKey [FK_Ecshop_GroupBuyCondition_Ecshop_GroupBuy]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_GroupBuyCondition_Ecshop_GroupBuy]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_GroupBuyCondition]'))
ALTER TABLE [dbo].[Ecshop_GroupBuyCondition] DROP CONSTRAINT [FK_Ecshop_GroupBuyCondition_Ecshop_GroupBuy]
GO
/****** Object:  ForeignKey [FK_Ecshop_Helps_HelpCategories]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_Helps_HelpCategories]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_Helps]'))
ALTER TABLE [dbo].[Ecshop_Helps] DROP CONSTRAINT [FK_Ecshop_Helps_HelpCategories]
GO
/****** Object:  ForeignKey [FK_Ecshop_Hotkeywords_Ecshop_Categories]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_Hotkeywords_Ecshop_Categories]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_Hotkeywords]'))
ALTER TABLE [dbo].[Ecshop_Hotkeywords] DROP CONSTRAINT [FK_Ecshop_Hotkeywords_Ecshop_Categories]
GO
/****** Object:  ForeignKey [FK_Ecshop_InpourRequest_aspnet_Memberss]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_InpourRequest_aspnet_Memberss]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_InpourRequest]'))
ALTER TABLE [dbo].[Ecshop_InpourRequest] DROP CONSTRAINT [FK_Ecshop_InpourRequest_aspnet_Memberss]
GO
/****** Object:  ForeignKey [FK_Ecshop_LeaveCommentReplys_LeaveComments]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_LeaveCommentReplys_LeaveComments]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_LeaveCommentReplys]'))
ALTER TABLE [dbo].[Ecshop_LeaveCommentReplys] DROP CONSTRAINT [FK_Ecshop_LeaveCommentReplys_LeaveComments]
GO
/****** Object:  ForeignKey [FK_Ecshop_OrderDebitNote_Orders]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_OrderDebitNote_Orders]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_OrderDebitNote]'))
ALTER TABLE [dbo].[Ecshop_OrderDebitNote] DROP CONSTRAINT [FK_Ecshop_OrderDebitNote_Orders]
GO
/****** Object:  ForeignKey [FK_Ecshop_OrderGifts_Orders]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_OrderGifts_Orders]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_OrderGifts]'))
ALTER TABLE [dbo].[Ecshop_OrderGifts] DROP CONSTRAINT [FK_Ecshop_OrderGifts_Orders]
GO
/****** Object:  ForeignKey [FK_Ecshop_OrderItems_Orders]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_OrderItems_Orders]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_OrderItems]'))
ALTER TABLE [dbo].[Ecshop_OrderItems] DROP CONSTRAINT [FK_Ecshop_OrderItems_Orders]
GO
/****** Object:  ForeignKey [FK_Ecshop_OrderRefund_Orders]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_OrderRefund_Orders]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_OrderRefund]'))
ALTER TABLE [dbo].[Ecshop_OrderRefund] DROP CONSTRAINT [FK_Ecshop_OrderRefund_Orders]
GO
/****** Object:  ForeignKey [FK_Ecshop_OrderReplace_Orders]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_OrderReplace_Orders]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_OrderReplace]'))
ALTER TABLE [dbo].[Ecshop_OrderReplace] DROP CONSTRAINT [FK_Ecshop_OrderReplace_Orders]
GO
/****** Object:  ForeignKey [FK_Ecshop_OrderReturns_Orders]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_OrderReturns_Orders]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_OrderReturns]'))
ALTER TABLE [dbo].[Ecshop_OrderReturns] DROP CONSTRAINT [FK_Ecshop_OrderReturns_Orders]
GO
/****** Object:  ForeignKey [FK_Ecshop_OrderSendNote_Orders]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_OrderSendNote_Orders]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_OrderSendNote]'))
ALTER TABLE [dbo].[Ecshop_OrderSendNote] DROP CONSTRAINT [FK_Ecshop_OrderSendNote_Orders]
GO
/****** Object:  ForeignKey [FK_Ecshop_PointDetails_aspnet_Memberss]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_PointDetails_aspnet_Memberss]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_PointDetails]'))
ALTER TABLE [dbo].[Ecshop_PointDetails] DROP CONSTRAINT [FK_Ecshop_PointDetails_aspnet_Memberss]
GO
/****** Object:  ForeignKey [FK_Ecshop_ProductAttributes_Attributes]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_ProductAttributes_Attributes]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_ProductAttributes]'))
ALTER TABLE [dbo].[Ecshop_ProductAttributes] DROP CONSTRAINT [FK_Ecshop_ProductAttributes_Attributes]
GO
/****** Object:  ForeignKey [FK_Ecshop_ProductAttributes_Products]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_ProductAttributes_Products]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_ProductAttributes]'))
ALTER TABLE [dbo].[Ecshop_ProductAttributes] DROP CONSTRAINT [FK_Ecshop_ProductAttributes_Products]
GO
/****** Object:  ForeignKey [FK_Ecshop_ProductConsultations_Products]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_ProductConsultations_Products]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_ProductConsultations]'))
ALTER TABLE [dbo].[Ecshop_ProductConsultations] DROP CONSTRAINT [FK_Ecshop_ProductConsultations_Products]
GO
/****** Object:  ForeignKey [FK_Ecshop_ProductReviews_Products]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_ProductReviews_Products]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_ProductReviews]'))
ALTER TABLE [dbo].[Ecshop_ProductReviews] DROP CONSTRAINT [FK_Ecshop_ProductReviews_Products]
GO
/****** Object:  ForeignKey [FK_Ecshop_ProductTag_Ecshop_Products]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_ProductTag_Ecshop_Products]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_ProductTag]'))
ALTER TABLE [dbo].[Ecshop_ProductTag] DROP CONSTRAINT [FK_Ecshop_ProductTag_Ecshop_Products]
GO
/****** Object:  ForeignKey [FK_Ecshop_ProductTypeBrands_Ecshop_BrandCategories]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_ProductTypeBrands_Ecshop_BrandCategories]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_ProductTypeBrands]'))
ALTER TABLE [dbo].[Ecshop_ProductTypeBrands] DROP CONSTRAINT [FK_Ecshop_ProductTypeBrands_Ecshop_BrandCategories]
GO
/****** Object:  ForeignKey [FK_Ecshop_ProductTypeBrands_Ecshop_ProductTypes]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_ProductTypeBrands_Ecshop_ProductTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_ProductTypeBrands]'))
ALTER TABLE [dbo].[Ecshop_ProductTypeBrands] DROP CONSTRAINT [FK_Ecshop_ProductTypeBrands_Ecshop_ProductTypes]
GO
/****** Object:  ForeignKey [FK_Ecshop_PromotionMemberGrades_aspnet_MemberGrades]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_PromotionMemberGrades_aspnet_MemberGrades]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_PromotionMemberGrades]'))
ALTER TABLE [dbo].[Ecshop_PromotionMemberGrades] DROP CONSTRAINT [FK_Ecshop_PromotionMemberGrades_aspnet_MemberGrades]
GO
/****** Object:  ForeignKey [FK_Ecshop_PromotionMemberGrades_Ecshop_Promotions]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_PromotionMemberGrades_Ecshop_Promotions]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_PromotionMemberGrades]'))
ALTER TABLE [dbo].[Ecshop_PromotionMemberGrades] DROP CONSTRAINT [FK_Ecshop_PromotionMemberGrades_Ecshop_Promotions]
GO
/****** Object:  ForeignKey [FK_Ecshop_PromotionProducts_Ecshop_Products]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_PromotionProducts_Ecshop_Products]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_PromotionProducts]'))
ALTER TABLE [dbo].[Ecshop_PromotionProducts] DROP CONSTRAINT [FK_Ecshop_PromotionProducts_Ecshop_Products]
GO
/****** Object:  ForeignKey [FK_Ecshop_PromotionProducts_Ecshop_Promotions]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_PromotionProducts_Ecshop_Promotions]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_PromotionProducts]'))
ALTER TABLE [dbo].[Ecshop_PromotionProducts] DROP CONSTRAINT [FK_Ecshop_PromotionProducts_Ecshop_Promotions]
GO
/****** Object:  ForeignKey [FK_Ecshop_ShippingRegions_ShippingTypes]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_ShippingRegions_ShippingTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_ShippingRegions]'))
ALTER TABLE [dbo].[Ecshop_ShippingRegions] DROP CONSTRAINT [FK_Ecshop_ShippingRegions_ShippingTypes]
GO
/****** Object:  ForeignKey [FK_Ecshop_ShippingTypeGroups_ShippingTemplates]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_ShippingTypeGroups_ShippingTemplates]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_ShippingTypeGroups]'))
ALTER TABLE [dbo].[Ecshop_ShippingTypeGroups] DROP CONSTRAINT [FK_Ecshop_ShippingTypeGroups_ShippingTemplates]
GO
/****** Object:  ForeignKey [FK_Ecshop_ShippingTypes_ShippingTemplates]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_ShippingTypes_ShippingTemplates]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_ShippingTypes]'))
ALTER TABLE [dbo].[Ecshop_ShippingTypes] DROP CONSTRAINT [FK_Ecshop_ShippingTypes_ShippingTemplates]
GO
/****** Object:  ForeignKey [FK_Ecshop_ShoppingCarts_aspnet_Members]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_ShoppingCarts_aspnet_Members]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_ShoppingCarts]'))
ALTER TABLE [dbo].[Ecshop_ShoppingCarts] DROP CONSTRAINT [FK_Ecshop_ShoppingCarts_aspnet_Members]
GO
/****** Object:  ForeignKey [FK_Ecshop_SKUItems_SKUs]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_SKUItems_SKUs]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_SKUItems]'))
ALTER TABLE [dbo].[Ecshop_SKUItems] DROP CONSTRAINT [FK_Ecshop_SKUItems_SKUs]
GO
/****** Object:  ForeignKey [FK_Ecshop_SKUMemberPrice_aspnet_MemberGrades]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_SKUMemberPrice_aspnet_MemberGrades]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_SKUMemberPrice]'))
ALTER TABLE [dbo].[Ecshop_SKUMemberPrice] DROP CONSTRAINT [FK_Ecshop_SKUMemberPrice_aspnet_MemberGrades]
GO
/****** Object:  ForeignKey [FK_Ecshop_SKUMemberPrice_SKUs]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_SKUMemberPrice_SKUs]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_SKUMemberPrice]'))
ALTER TABLE [dbo].[Ecshop_SKUMemberPrice] DROP CONSTRAINT [FK_Ecshop_SKUMemberPrice_SKUs]
GO
/****** Object:  ForeignKey [FK_Ecshop_SKUs_Products]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_SKUs_Products]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_SKUs]'))
ALTER TABLE [dbo].[Ecshop_SKUs] DROP CONSTRAINT [FK_Ecshop_SKUs_Products]
GO
/****** Object:  ForeignKey [FK_Ecshop_UserShippingAddresses_aspnet_Memberss]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_UserShippingAddresses_aspnet_Memberss]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_UserShippingAddresses]'))
ALTER TABLE [dbo].[Ecshop_UserShippingAddresses] DROP CONSTRAINT [FK_Ecshop_UserShippingAddresses_aspnet_Memberss]
GO
/****** Object:  ForeignKey [FK_Ecshop_VoteItems_Votes]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_VoteItems_Votes]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_VoteItems]'))
ALTER TABLE [dbo].[Ecshop_VoteItems] DROP CONSTRAINT [FK_Ecshop_VoteItems_Votes]
GO
/****** Object:  ForeignKey [FK_Taobao_Products_Ecshop_Products]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Taobao_Products_Ecshop_Products]') AND parent_object_id = OBJECT_ID(N'[dbo].[Taobao_Products]'))
ALTER TABLE [dbo].[Taobao_Products] DROP CONSTRAINT [FK_Taobao_Products_Ecshop_Products]
GO
/****** Object:  StoredProcedure [dbo].[cp_Product_GetExportList]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cp_Product_GetExportList]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[cp_Product_GetExportList]
GO
/****** Object:  StoredProcedure [dbo].[ss_ShoppingCart_GetItemInfo]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ss_ShoppingCart_GetItemInfo]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ss_ShoppingCart_GetItemInfo]
GO
/****** Object:  StoredProcedure [dbo].[ss_GroupBuyProducts_Get]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ss_GroupBuyProducts_Get]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ss_GroupBuyProducts_Get]
GO
/****** Object:  View [dbo].[vw_Ecshop_CountDown]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_Ecshop_CountDown]'))
DROP VIEW [dbo].[vw_Ecshop_CountDown]
GO
/****** Object:  View [dbo].[vw_Ecshop_GroupBuy]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_Ecshop_GroupBuy]'))
DROP VIEW [dbo].[vw_Ecshop_GroupBuy]
GO
/****** Object:  StoredProcedure [dbo].[ac_Member_Favorites_Get]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ac_Member_Favorites_Get]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ac_Member_Favorites_Get]
GO
/****** Object:  Table [dbo].[Ecshop_AttributeValues]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_AttributeValues]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_AttributeValues]
GO
/****** Object:  StoredProcedure [dbo].[cp_ShippingMode_Create]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cp_ShippingMode_Create]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[cp_ShippingMode_Create]
GO
/****** Object:  StoredProcedure [dbo].[cp_ShippingMode_Update]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cp_ShippingMode_Update]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[cp_ShippingMode_Update]
GO
/****** Object:  StoredProcedure [dbo].[cp_ClaimCode_Create]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cp_ClaimCode_Create]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[cp_ClaimCode_Create]
GO
/****** Object:  StoredProcedure [dbo].[cp_Hotkeywords_Log]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cp_Hotkeywords_Log]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[cp_Hotkeywords_Log]
GO
/****** Object:  StoredProcedure [dbo].[cp_LeaveComments_Get]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cp_LeaveComments_Get]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[cp_LeaveComments_Get]
GO
/****** Object:  StoredProcedure [dbo].[cp_Manager_Delete]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cp_Manager_Delete]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[cp_Manager_Delete]
GO
/****** Object:  StoredProcedure [dbo].[cp_Member_Delete]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cp_Member_Delete]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[cp_Member_Delete]
GO
/****** Object:  StoredProcedure [dbo].[cp_ProductConsultation_Get]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cp_ProductConsultation_Get]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[cp_ProductConsultation_Get]
GO
/****** Object:  StoredProcedure [dbo].[cp_ProductReviews_Get]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cp_ProductReviews_Get]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[cp_ProductReviews_Get]
GO
/****** Object:  StoredProcedure [dbo].[aspnet_UsersInRoles_AddUsersToRoles]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_UsersInRoles_AddUsersToRoles]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_UsersInRoles_AddUsersToRoles]
GO
/****** Object:  StoredProcedure [dbo].[aspnet_UsersInRoles_FindUsersInRole]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_UsersInRoles_FindUsersInRole]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_UsersInRoles_FindUsersInRole]
GO
/****** Object:  StoredProcedure [dbo].[aspnet_UsersInRoles_GetRolesForUser]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_UsersInRoles_GetRolesForUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_UsersInRoles_GetRolesForUser]
GO
/****** Object:  StoredProcedure [dbo].[aspnet_UsersInRoles_GetUsersInRoles]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_UsersInRoles_GetUsersInRoles]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_UsersInRoles_GetUsersInRoles]
GO
/****** Object:  StoredProcedure [dbo].[aspnet_UsersInRoles_IsUserInRole]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_UsersInRoles_IsUserInRole]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_UsersInRoles_IsUserInRole]
GO
/****** Object:  StoredProcedure [dbo].[aspnet_UsersInRoles_RemoveUsersFromRoles]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_UsersInRoles_RemoveUsersFromRoles]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_UsersInRoles_RemoveUsersFromRoles]
GO
/****** Object:  StoredProcedure [dbo].[cp_API_Orders_Get]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cp_API_Orders_Get]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[cp_API_Orders_Get]
GO
/****** Object:  StoredProcedure [dbo].[ac_Member_ConsultationsAndReplys_Get]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ac_Member_ConsultationsAndReplys_Get]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ac_Member_ConsultationsAndReplys_Get]
GO
/****** Object:  StoredProcedure [dbo].[cp_BalanceDrawRequest_Update]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cp_BalanceDrawRequest_Update]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[cp_BalanceDrawRequest_Update]
GO
/****** Object:  StoredProcedure [dbo].[ac_Member_InpourRequest_Create]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ac_Member_InpourRequest_Create]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ac_Member_InpourRequest_Create]
GO
/****** Object:  StoredProcedure [dbo].[ac_Member_ShippingAddress_CreateUpdateDelete]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ac_Member_ShippingAddress_CreateUpdateDelete]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ac_Member_ShippingAddress_CreateUpdateDelete]
GO
/****** Object:  StoredProcedure [dbo].[ac_Member_UserReviewsAndReplys_Get]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ac_Member_UserReviewsAndReplys_Get]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ac_Member_UserReviewsAndReplys_Get]
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_DeleteUser]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Membership_DeleteUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_Membership_DeleteUser]
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Roles_DeleteRole]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Roles_DeleteRole]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_Roles_DeleteRole]
GO
/****** Object:  Table [dbo].[Ecshop_GroupBuyCondition]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_GroupBuyCondition]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_GroupBuyCondition]
GO
/****** Object:  Table [dbo].[Ecshop_ProductAttributes]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_ProductAttributes]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_ProductAttributes]
GO
/****** Object:  View [dbo].[vw_Ecshop_Helps]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_Ecshop_Helps]'))
DROP VIEW [dbo].[vw_Ecshop_Helps]
GO
/****** Object:  View [dbo].[vw_Ecshop_CouponInfo]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_Ecshop_CouponInfo]'))
DROP VIEW [dbo].[vw_Ecshop_CouponInfo]
GO
/****** Object:  View [dbo].[vw_Ecshop_Articles]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_Ecshop_Articles]'))
DROP VIEW [dbo].[vw_Ecshop_Articles]
GO
/****** Object:  View [dbo].[vw_Ecshop_BrowseProductList]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_Ecshop_BrowseProductList]'))
DROP VIEW [dbo].[vw_Ecshop_BrowseProductList]
GO
/****** Object:  View [dbo].[vw_Ecshop_OrderDebitNote]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_Ecshop_OrderDebitNote]'))
DROP VIEW [dbo].[vw_Ecshop_OrderDebitNote]
GO
/****** Object:  View [dbo].[vw_Ecshop_OrderItem]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_Ecshop_OrderItem]'))
DROP VIEW [dbo].[vw_Ecshop_OrderItem]
GO
/****** Object:  View [dbo].[vw_Ecshop_OrderRefund]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_Ecshop_OrderRefund]'))
DROP VIEW [dbo].[vw_Ecshop_OrderRefund]
GO
/****** Object:  View [dbo].[vw_Ecshop_OrderReplace]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_Ecshop_OrderReplace]'))
DROP VIEW [dbo].[vw_Ecshop_OrderReplace]
GO
/****** Object:  View [dbo].[vw_Ecshop_OrderReturns]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_Ecshop_OrderReturns]'))
DROP VIEW [dbo].[vw_Ecshop_OrderReturns]
GO
/****** Object:  View [dbo].[vw_Ecshop_OrderSendNote]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_Ecshop_OrderSendNote]'))
DROP VIEW [dbo].[vw_Ecshop_OrderSendNote]
GO
/****** Object:  View [dbo].[vw_Ecshop_ProductConsultations]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_Ecshop_ProductConsultations]'))
DROP VIEW [dbo].[vw_Ecshop_ProductConsultations]
GO
/****** Object:  View [dbo].[vw_Ecshop_ProductReviews]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_Ecshop_ProductReviews]'))
DROP VIEW [dbo].[vw_Ecshop_ProductReviews]
GO
/****** Object:  View [dbo].[vw_Ecshop_ProductSkuList]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_Ecshop_ProductSkuList]'))
DROP VIEW [dbo].[vw_Ecshop_ProductSkuList]
GO
/****** Object:  View [dbo].[vw_Ecshop_SaleDetails]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_Ecshop_SaleDetails]'))
DROP VIEW [dbo].[vw_Ecshop_SaleDetails]
GO
/****** Object:  StoredProcedure [dbo].[ss_LeaveComments_Get]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ss_LeaveComments_Get]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ss_LeaveComments_Get]
GO
/****** Object:  StoredProcedure [dbo].[ss_ShoppingCart_AddLineItem]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ss_ShoppingCart_AddLineItem]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ss_ShoppingCart_AddLineItem]
GO
/****** Object:  Table [dbo].[Ecshop_SKUItems]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_SKUItems]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_SKUItems]
GO
/****** Object:  Table [dbo].[Ecshop_SKUMemberPrice]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_SKUMemberPrice]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_SKUMemberPrice]
GO
/****** Object:  Table [dbo].[Ecshop_SKUs]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_SKUs]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_SKUs]
GO
/****** Object:  StoredProcedure [dbo].[cp_Product_Update]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cp_Product_Update]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[cp_Product_Update]
GO
/****** Object:  Table [dbo].[Ecshop_PointDetails]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_PointDetails]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_PointDetails]
GO
/****** Object:  Table [dbo].[Ecshop_OrderSendNote]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_OrderSendNote]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_OrderSendNote]
GO
/****** Object:  Table [dbo].[Ecshop_PromotionMemberGrades]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_PromotionMemberGrades]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_PromotionMemberGrades]
GO
/****** Object:  Table [dbo].[Ecshop_PromotionProducts]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_PromotionProducts]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_PromotionProducts]
GO
/****** Object:  StoredProcedure [dbo].[ss_CreateOrder]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ss_CreateOrder]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ss_CreateOrder]
GO
/****** Object:  StoredProcedure [dbo].[ss_ShoppingCart_AddAPILineItem]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ss_ShoppingCart_AddAPILineItem]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ss_ShoppingCart_AddAPILineItem]
GO
/****** Object:  Table [dbo].[Ecshop_ShippingTypeGroups]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_ShippingTypeGroups]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_ShippingTypeGroups]
GO
/****** Object:  Table [dbo].[Ecshop_ShippingTypes]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_ShippingTypes]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_ShippingTypes]
GO
/****** Object:  Table [dbo].[Ecshop_ShoppingCarts]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_ShoppingCarts]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_ShoppingCarts]
GO
/****** Object:  Table [dbo].[Ecshop_ProductTag]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_ProductTag]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_ProductTag]
GO
/****** Object:  Table [dbo].[Ecshop_ProductTypeBrands]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_ProductTypeBrands]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_ProductTypeBrands]
GO
/****** Object:  Table [dbo].[Ecshop_ShippingRegions]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_ShippingRegions]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_ShippingRegions]
GO
/****** Object:  Table [dbo].[Ecshop_UserShippingAddresses]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_UserShippingAddresses]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_UserShippingAddresses]
GO
/****** Object:  Table [dbo].[Ecshop_VoteItems]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_VoteItems]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_VoteItems]
GO
/****** Object:  Table [dbo].[Taobao_Products]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Taobao_Products]') AND type in (N'U'))
DROP TABLE [dbo].[Taobao_Products]
GO
/****** Object:  View [dbo].[vw_Ecshop_BundlingProducts]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_Ecshop_BundlingProducts]'))
DROP VIEW [dbo].[vw_Ecshop_BundlingProducts]
GO
/****** Object:  View [dbo].[vw_aspnet_Managers]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_aspnet_Managers]'))
DROP VIEW [dbo].[vw_aspnet_Managers]
GO
/****** Object:  View [dbo].[vw_aspnet_Members]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_aspnet_Members]'))
DROP VIEW [dbo].[vw_aspnet_Members]
GO
/****** Object:  Table [dbo].[Ecshop_ProductConsultations]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_ProductConsultations]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_ProductConsultations]
GO
/****** Object:  Table [dbo].[Ecshop_ProductReviews]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_ProductReviews]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_ProductReviews]
GO
/****** Object:  Table [dbo].[Ecshop_OrderDebitNote]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_OrderDebitNote]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_OrderDebitNote]
GO
/****** Object:  Table [dbo].[Ecshop_OrderGifts]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_OrderGifts]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_OrderGifts]
GO
/****** Object:  Table [dbo].[Ecshop_OrderItems]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_OrderItems]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_OrderItems]
GO
/****** Object:  Table [dbo].[Ecshop_OrderRefund]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_OrderRefund]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_OrderRefund]
GO
/****** Object:  Table [dbo].[Ecshop_OrderReplace]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_OrderReplace]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_OrderReplace]
GO
/****** Object:  Table [dbo].[Ecshop_OrderReturns]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_OrderReturns]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_OrderReturns]
GO
/****** Object:  Table [dbo].[Ecshop_GiftShoppingCarts]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_GiftShoppingCarts]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_GiftShoppingCarts]
GO
/****** Object:  Table [dbo].[Ecshop_GroupBuy]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_GroupBuy]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_GroupBuy]
GO
/****** Object:  Table [dbo].[Ecshop_Articles]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_Articles]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_Articles]
GO
/****** Object:  Table [dbo].[Ecshop_Attributes]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_Attributes]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_Attributes]
GO
/****** Object:  Table [dbo].[Ecshop_Favorite]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_Favorite]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_Favorite]
GO
/****** Object:  Table [dbo].[Ecshop_BundlingProductItems]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_BundlingProductItems]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_BundlingProductItems]
GO
/****** Object:  Table [dbo].[Ecshop_CountDown]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_CountDown]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_CountDown]
GO
/****** Object:  Table [dbo].[Ecshop_CouponItems]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_CouponItems]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_CouponItems]
GO
/****** Object:  Table [dbo].[Ecshop_LeaveCommentReplys]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_LeaveCommentReplys]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_LeaveCommentReplys]
GO
/****** Object:  Table [dbo].[Ecshop_Helps]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_Helps]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_Helps]
GO
/****** Object:  Table [dbo].[Ecshop_Hotkeywords]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_Hotkeywords]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_Hotkeywords]
GO
/****** Object:  Table [dbo].[Ecshop_InpourRequest]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_InpourRequest]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_InpourRequest]
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Roles_GetAllRoles]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Roles_GetAllRoles]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_Roles_GetAllRoles]
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Roles_RoleExists]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Roles_RoleExists]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_Roles_RoleExists]
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_FindUsersByEmail]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Membership_FindUsersByEmail]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_Membership_FindUsersByEmail]
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_FindUsersByName]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Membership_FindUsersByName]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_Membership_FindUsersByName]
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_GetAllUsers]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Membership_GetAllUsers]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_Membership_GetAllUsers]
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_GetNumberOfUsersOnline]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Membership_GetNumberOfUsersOnline]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_Membership_GetNumberOfUsersOnline]
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_GetPassword]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Membership_GetPassword]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_Membership_GetPassword]
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_GetPasswordWithFormat]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Membership_GetPasswordWithFormat]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_Membership_GetPasswordWithFormat]
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_GetUserByEmail]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Membership_GetUserByEmail]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_Membership_GetUserByEmail]
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_GetUserByName]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Membership_GetUserByName]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_Membership_GetUserByName]
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_GetUserByUserId]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Membership_GetUserByUserId]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_Membership_GetUserByUserId]
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_ResetPassword]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Membership_ResetPassword]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_Membership_ResetPassword]
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_SetPassword]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Membership_SetPassword]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_Membership_SetPassword]
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_UnlockUser]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Membership_UnlockUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_Membership_UnlockUser]
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_UpdateUser]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Membership_UpdateUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_Membership_UpdateUser]
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_UpdateUserInfo]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Membership_UpdateUserInfo]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_Membership_UpdateUserInfo]
GO
/****** Object:  StoredProcedure [dbo].[aspnet_OpenId_Bind]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_OpenId_Bind]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_OpenId_Bind]
GO
/****** Object:  StoredProcedure [dbo].[cp_BrandCategory_DisplaySequence]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cp_BrandCategory_DisplaySequence]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[cp_BrandCategory_DisplaySequence]
GO
/****** Object:  StoredProcedure [dbo].[cp_Category_Create]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cp_Category_Create]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[cp_Category_Create]
GO
/****** Object:  StoredProcedure [dbo].[cp_Category_Delete]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cp_Category_Delete]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[cp_Category_Delete]
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Roles_CreateRole]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Roles_CreateRole]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_Roles_CreateRole]
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_ChangePasswordQuestionAndAnswer]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Membership_ChangePasswordQuestionAndAnswer]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_Membership_ChangePasswordQuestionAndAnswer]
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_CreateUser]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Membership_CreateUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_Membership_CreateUser]
GO
/****** Object:  StoredProcedure [dbo].[cp_ArticleCategory_CreateUpdateDelete]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cp_ArticleCategory_CreateUpdateDelete]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[cp_ArticleCategory_CreateUpdateDelete]
GO
/****** Object:  StoredProcedure [dbo].[aspnet_OpenIdSettings_Save]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_OpenIdSettings_Save]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_OpenIdSettings_Save]
GO
/****** Object:  Table [dbo].[aspnet_UsersInRoles]    Script Date: 12/10/2014 10:04:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_UsersInRoles]') AND type in (N'U'))
DROP TABLE [dbo].[aspnet_UsersInRoles]
GO
/****** Object:  StoredProcedure [dbo].[cp_ProductSales_Get]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cp_ProductSales_Get]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[cp_ProductSales_Get]
GO
/****** Object:  StoredProcedure [dbo].[cp_ProductSalesNoPage_Get]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cp_ProductSalesNoPage_Get]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[cp_ProductSalesNoPage_Get]
GO
/****** Object:  StoredProcedure [dbo].[cp_ProductVisitAndBuyStatistics_Get]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cp_ProductVisitAndBuyStatistics_Get]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[cp_ProductVisitAndBuyStatistics_Get]
GO
/****** Object:  StoredProcedure [dbo].[cp_Menu_SwapDisplaySequence]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cp_Menu_SwapDisplaySequence]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[cp_Menu_SwapDisplaySequence]
GO
/****** Object:  StoredProcedure [dbo].[cp_Orders_Get]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cp_Orders_Get]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[cp_Orders_Get]
GO
/****** Object:  StoredProcedure [dbo].[cp_OrderStatistics_Get]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cp_OrderStatistics_Get]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[cp_OrderStatistics_Get]
GO
/****** Object:  StoredProcedure [dbo].[cp_OrderStatisticsNoPage_Get]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cp_OrderStatisticsNoPage_Get]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[cp_OrderStatisticsNoPage_Get]
GO
/****** Object:  StoredProcedure [dbo].[cp_PaymentType_CreateUpdateDelete]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cp_PaymentType_CreateUpdateDelete]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[cp_PaymentType_CreateUpdateDelete]
GO
/****** Object:  StoredProcedure [dbo].[cp_Product_Create]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cp_Product_Create]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[cp_Product_Create]
GO
/****** Object:  StoredProcedure [dbo].[cp_EmailQueue_Failure]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cp_EmailQueue_Failure]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[cp_EmailQueue_Failure]
GO
/****** Object:  StoredProcedure [dbo].[cp_FriendlyLink_CreateUpdateDelete]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cp_FriendlyLink_CreateUpdateDelete]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[cp_FriendlyLink_CreateUpdateDelete]
GO
/****** Object:  StoredProcedure [dbo].[cp_Gift_CreateUpdateDelete]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cp_Gift_CreateUpdateDelete]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[cp_Gift_CreateUpdateDelete]
GO
/****** Object:  StoredProcedure [dbo].[cp_HelpCategory_CreateUpdateDelete]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cp_HelpCategory_CreateUpdateDelete]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[cp_HelpCategory_CreateUpdateDelete]
GO
/****** Object:  StoredProcedure [dbo].[cp_Votes_Create]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cp_Votes_Create]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[cp_Votes_Create]
GO
/****** Object:  StoredProcedure [dbo].[cp_Votes_IsBackup]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cp_Votes_IsBackup]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[cp_Votes_IsBackup]
GO
/****** Object:  Table [dbo].[Ecshop_BalanceDetails]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_BalanceDetails]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_BalanceDetails]
GO
/****** Object:  Table [dbo].[Ecshop_BalanceDrawRequest]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_BalanceDrawRequest]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_BalanceDrawRequest]
GO
/****** Object:  Table [dbo].[Ecshop_Banner]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_Banner]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_Banner]
GO
/****** Object:  Table [dbo].[Ecshop_BrandCategories]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_BrandCategories]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_BrandCategories]
GO
/****** Object:  UserDefinedFunction [dbo].[F_SplitToInt]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[F_SplitToInt]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[F_SplitToInt]
GO
/****** Object:  UserDefinedFunction [dbo].[F_SplitToString]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[F_SplitToString]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[F_SplitToString]
GO
/****** Object:  Table [dbo].[Ecshop_Affiche]    Script Date: 12/10/2014 10:04:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_Affiche]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_Affiche]
GO
/****** Object:  Table [dbo].[Ecshop_ApiShorpCart]    Script Date: 12/10/2014 10:04:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_ApiShorpCart]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_ApiShorpCart]
GO
/****** Object:  Table [dbo].[Ecshop_AppInstallRecords]    Script Date: 12/10/2014 10:04:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_AppInstallRecords]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_AppInstallRecords]
GO
/****** Object:  Table [dbo].[Ecshop_AppVersionRecords]    Script Date: 12/10/2014 10:04:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_AppVersionRecords]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_AppVersionRecords]
GO
/****** Object:  Table [dbo].[Ecshop_ArticleCategories]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_ArticleCategories]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_ArticleCategories]
GO
/****** Object:  StoredProcedure [dbo].[cp_MemberStatistics_Get]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cp_MemberStatistics_Get]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[cp_MemberStatistics_Get]
GO
/****** Object:  StoredProcedure [dbo].[cp_RegionsUsers_Get]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cp_RegionsUsers_Get]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[cp_RegionsUsers_Get]
GO
/****** Object:  Table [dbo].[aspnet_Roles]    Script Date: 12/10/2014 10:04:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Roles]') AND type in (N'U'))
DROP TABLE [dbo].[aspnet_Roles]
GO
/****** Object:  Table [dbo].[aspnet_OpenIdSettings]    Script Date: 12/10/2014 10:04:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_OpenIdSettings]') AND type in (N'U'))
DROP TABLE [dbo].[aspnet_OpenIdSettings]
GO
/****** Object:  Table [dbo].[aspnet_Managers]    Script Date: 12/10/2014 10:04:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Managers]') AND type in (N'U'))
DROP TABLE [dbo].[aspnet_Managers]
GO
/****** Object:  Table [dbo].[aspnet_MemberGrades]    Script Date: 12/10/2014 10:04:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_MemberGrades]') AND type in (N'U'))
DROP TABLE [dbo].[aspnet_MemberGrades]
GO
/****** Object:  Table [dbo].[aspnet_Members]    Script Date: 12/10/2014 10:04:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Members]') AND type in (N'U'))
DROP TABLE [dbo].[aspnet_Members]
GO
/****** Object:  View [dbo].[vw_Ecshop_ManagerMessageBox]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_Ecshop_ManagerMessageBox]'))
DROP VIEW [dbo].[vw_Ecshop_ManagerMessageBox]
GO
/****** Object:  Table [dbo].[aspnet_Users]    Script Date: 12/10/2014 10:04:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Users]') AND type in (N'U'))
DROP TABLE [dbo].[aspnet_Users]
GO
/****** Object:  Table [dbo].[Ecshop_IntegrationSettings]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_IntegrationSettings]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_IntegrationSettings]
GO
/****** Object:  Table [dbo].[Ecshop_MemberClientSet]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_MemberClientSet]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_MemberClientSet]
GO
/****** Object:  Table [dbo].[Ecshop_ManagerMessageBox]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_ManagerMessageBox]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_ManagerMessageBox]
GO
/****** Object:  View [dbo].[vw_Ecshop_MemberMessageBox]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_Ecshop_MemberMessageBox]'))
DROP VIEW [dbo].[vw_Ecshop_MemberMessageBox]
GO
/****** Object:  Table [dbo].[Ecshop_MemberMessageBox]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_MemberMessageBox]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_MemberMessageBox]
GO
/****** Object:  Table [dbo].[Ecshop_MessageContent]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_MessageContent]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_MessageContent]
GO
/****** Object:  Table [dbo].[Ecshop_MessageTemplates]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_MessageTemplates]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_MessageTemplates]
GO
/****** Object:  Table [dbo].[Ecshop_LeaveComments]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_LeaveComments]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_LeaveComments]
GO
/****** Object:  Table [dbo].[Ecshop_Logs]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_Logs]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_Logs]
GO
/****** Object:  Table [dbo].[Ecshop_Coupons]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_Coupons]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_Coupons]
GO
/****** Object:  Table [dbo].[Ecshop_EmailQueue]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_EmailQueue]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_EmailQueue]
GO
/****** Object:  Table [dbo].[Ecshop_ExpressTemplates]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_ExpressTemplates]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_ExpressTemplates]
GO
/****** Object:  Table [dbo].[Ecshop_BundlingProducts]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_BundlingProducts]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_BundlingProducts]
GO
/****** Object:  Table [dbo].[Ecshop_Categories]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_Categories]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_Categories]
GO
/****** Object:  Table [dbo].[Ecshop_CellphoneQueue]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_CellphoneQueue]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_CellphoneQueue]
GO
/****** Object:  Table [dbo].[Ecshop_FavoriteTags]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_FavoriteTags]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_FavoriteTags]
GO
/****** Object:  Table [dbo].[Ecshop_FriendlyLinks]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_FriendlyLinks]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_FriendlyLinks]
GO
/****** Object:  Table [dbo].[Ecshop_Gifts]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_Gifts]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_Gifts]
GO
/****** Object:  Table [dbo].[Ecshop_HelpCategories]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_HelpCategories]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_HelpCategories]
GO
/****** Object:  Table [dbo].[Ecshop_Orders]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_Orders]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_Orders]
GO
/****** Object:  Table [dbo].[Ecshop_Products]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_Products]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_Products]
GO
/****** Object:  Table [dbo].[vshop_Activity]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[vshop_Activity]') AND type in (N'U'))
DROP TABLE [dbo].[vshop_Activity]
GO
/****** Object:  Table [dbo].[vshop_ActivitySignUp]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[vshop_ActivitySignUp]') AND type in (N'U'))
DROP TABLE [dbo].[vshop_ActivitySignUp]
GO
/****** Object:  Table [dbo].[vshop_AlarmNotify]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[vshop_AlarmNotify]') AND type in (N'U'))
DROP TABLE [dbo].[vshop_AlarmNotify]
GO
/****** Object:  Table [dbo].[vshop_FeedBackNotify]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[vshop_FeedBackNotify]') AND type in (N'U'))
DROP TABLE [dbo].[vshop_FeedBackNotify]
GO
/****** Object:  Table [dbo].[Vshop_HomeProducts]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Vshop_HomeProducts]') AND type in (N'U'))
DROP TABLE [dbo].[Vshop_HomeProducts]
GO
/****** Object:  Table [dbo].[Vshop_HomeTopics]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Vshop_HomeTopics]') AND type in (N'U'))
DROP TABLE [dbo].[Vshop_HomeTopics]
GO
/****** Object:  Table [dbo].[Vshop_LotteryActivity]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Vshop_LotteryActivity]') AND type in (N'U'))
DROP TABLE [dbo].[Vshop_LotteryActivity]
GO
/****** Object:  Table [dbo].[vshop_Menu]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[vshop_Menu]') AND type in (N'U'))
DROP TABLE [dbo].[vshop_Menu]
GO
/****** Object:  Table [dbo].[vshop_Message]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[vshop_Message]') AND type in (N'U'))
DROP TABLE [dbo].[vshop_Message]
GO
/****** Object:  Table [dbo].[Vshop_PrizeRecord]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Vshop_PrizeRecord]') AND type in (N'U'))
DROP TABLE [dbo].[Vshop_PrizeRecord]
GO
/****** Object:  Table [dbo].[Vshop_RelatedTopicProducts]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Vshop_RelatedTopicProducts]') AND type in (N'U'))
DROP TABLE [dbo].[Vshop_RelatedTopicProducts]
GO
/****** Object:  Table [dbo].[vshop_Reply]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[vshop_Reply]') AND type in (N'U'))
DROP TABLE [dbo].[vshop_Reply]
GO
/****** Object:  Table [dbo].[Vshop_Topics]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Vshop_Topics]') AND type in (N'U'))
DROP TABLE [dbo].[Vshop_Topics]
GO
/****** Object:  Table [dbo].[Ecshop_Votes]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_Votes]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_Votes]
GO
/****** Object:  Table [dbo].[Ecshop_ShippingTemplates]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_ShippingTemplates]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_ShippingTemplates]
GO
/****** Object:  Table [dbo].[Ecshop_ProductTypes]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_ProductTypes]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_ProductTypes]
GO
/****** Object:  Table [dbo].[Ecshop_Promotions]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_Promotions]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_Promotions]
GO
/****** Object:  Table [dbo].[Ecshop_RelatedArticsProducts]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_RelatedArticsProducts]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_RelatedArticsProducts]
GO
/****** Object:  Table [dbo].[Ecshop_RelatedProducts]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_RelatedProducts]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_RelatedProducts]
GO
/****** Object:  Table [dbo].[Ecshop_Shippers]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_Shippers]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_Shippers]
GO
/****** Object:  Table [dbo].[Ecshop_PaymentTypes]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_PaymentTypes]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_PaymentTypes]
GO
/****** Object:  Table [dbo].[Ecshop_PhotoCategories]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_PhotoCategories]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_PhotoCategories]
GO
/****** Object:  Table [dbo].[Ecshop_PhotoGallery]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_PhotoGallery]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_PhotoGallery]
GO
/****** Object:  Table [dbo].[Ecshop_PrivilegeInRoles]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_PrivilegeInRoles]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_PrivilegeInRoles]
GO
/****** Object:  Table [dbo].[Ecshop_SplittinDetails]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_SplittinDetails]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_SplittinDetails]
GO
/****** Object:  Table [dbo].[Ecshop_SplittinDraws]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_SplittinDraws]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_SplittinDraws]
GO
/****** Object:  Table [dbo].[Ecshop_Tags]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_Tags]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_Tags]
GO
/****** Object:  Table [dbo].[Ecshop_TemplateRelatedShipping]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_TemplateRelatedShipping]') AND type in (N'U'))
DROP TABLE [dbo].[Ecshop_TemplateRelatedShipping]
GO
/****** Object:  Default [DF_aspnet_Members_ReferralStatus]    Script Date: 12/10/2014 10:04:31 ******/
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_aspnet_Members_ReferralStatus]') AND parent_object_id = OBJECT_ID(N'[dbo].[aspnet_Members]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_aspnet_Members_ReferralStatus]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[aspnet_Members] DROP CONSTRAINT [DF_aspnet_Members_ReferralStatus]
END


End
GO
/****** Object:  Default [DF__tmp_ms_xx__IsOpe__5ECA0095]    Script Date: 12/10/2014 10:04:31 ******/
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF__tmp_ms_xx__IsOpe__5ECA0095]') AND parent_object_id = OBJECT_ID(N'[dbo].[aspnet_Members]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__tmp_ms_xx__IsOpe__5ECA0095]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[aspnet_Members] DROP CONSTRAINT [DF__tmp_ms_xx__IsOpe__5ECA0095]
END


End
GO
/****** Object:  Default [DF_aspnet_Members_OrderNumber]    Script Date: 12/10/2014 10:04:31 ******/
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_aspnet_Members_OrderNumber]') AND parent_object_id = OBJECT_ID(N'[dbo].[aspnet_Members]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_aspnet_Members_OrderNumber]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[aspnet_Members] DROP CONSTRAINT [DF_aspnet_Members_OrderNumber]
END


End
GO
/****** Object:  Default [DF_aspnet_Members_Expenditure]    Script Date: 12/10/2014 10:04:31 ******/
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_aspnet_Members_Expenditure]') AND parent_object_id = OBJECT_ID(N'[dbo].[aspnet_Members]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_aspnet_Members_Expenditure]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[aspnet_Members] DROP CONSTRAINT [DF_aspnet_Members_Expenditure]
END


End
GO
/****** Object:  Default [DF_aspnet_Members_Points]    Script Date: 12/10/2014 10:04:31 ******/
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_aspnet_Members_Points]') AND parent_object_id = OBJECT_ID(N'[dbo].[aspnet_Members]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_aspnet_Members_Points]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[aspnet_Members] DROP CONSTRAINT [DF_aspnet_Members_Points]
END


End
GO
/****** Object:  Default [DF_aspnet_Members_Balance]    Script Date: 12/10/2014 10:04:31 ******/
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_aspnet_Members_Balance]') AND parent_object_id = OBJECT_ID(N'[dbo].[aspnet_Members]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_aspnet_Members_Balance]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[aspnet_Members] DROP CONSTRAINT [DF_aspnet_Members_Balance]
END


End
GO
/****** Object:  Default [DF_aspnet_Members_RequestBalance]    Script Date: 12/10/2014 10:04:31 ******/
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_aspnet_Members_RequestBalance]') AND parent_object_id = OBJECT_ID(N'[dbo].[aspnet_Members]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_aspnet_Members_RequestBalance]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[aspnet_Members] DROP CONSTRAINT [DF_aspnet_Members_RequestBalance]
END


End
GO
/****** Object:  Default [DF__aspnet_Ro__RoleI__2E1BDC42]    Script Date: 12/10/2014 10:04:31 ******/
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF__aspnet_Ro__RoleI__2E1BDC42]') AND parent_object_id = OBJECT_ID(N'[dbo].[aspnet_Roles]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__aspnet_Ro__RoleI__2E1BDC42]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[aspnet_Roles] DROP CONSTRAINT [DF__aspnet_Ro__RoleI__2E1BDC42]
END


End
GO
/****** Object:  Default [DF__aspnet_Us__IsAno__014935CB]    Script Date: 12/10/2014 10:04:31 ******/
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF__aspnet_Us__IsAno__014935CB]') AND parent_object_id = OBJECT_ID(N'[dbo].[aspnet_Users]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__aspnet_Us__IsAno__014935CB]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[aspnet_Users] DROP CONSTRAINT [DF__aspnet_Us__IsAno__014935CB]
END


End
GO
/****** Object:  Default [DF__aspnet_Us__Sessi__09E968C4]    Script Date: 12/10/2014 10:04:31 ******/
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF__aspnet_Us__Sessi__09E968C4]') AND parent_object_id = OBJECT_ID(N'[dbo].[aspnet_Users]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__aspnet_Us__Sessi__09E968C4]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[aspnet_Users] DROP CONSTRAINT [DF__aspnet_Us__Sessi__09E968C4]
END


End
GO
/****** Object:  Default [DF__Ecshop_Ap__APITi__0ADD8CFD]    Script Date: 12/10/2014 10:04:31 ******/
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF__Ecshop_Ap__APITi__0ADD8CFD]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_ApiShorpCart]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Ecshop_Ap__APITi__0ADD8CFD]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Ecshop_ApiShorpCart] DROP CONSTRAINT [DF__Ecshop_Ap__APITi__0ADD8CFD]
END


End
GO
/****** Object:  Default [DF_Ecshop_Articles_IsRelease]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Ecshop_Articles_IsRelease]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_Articles]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Ecshop_Articles_IsRelease]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Ecshop_Articles] DROP CONSTRAINT [DF_Ecshop_Articles_IsRelease]
END


End
GO
/****** Object:  Default [DF_Ecshop_Banner_Client]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Ecshop_Banner_Client]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_Banner]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Ecshop_Banner_Client]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Ecshop_Banner] DROP CONSTRAINT [DF_Ecshop_Banner_Client]
END


End
GO
/****** Object:  Default [DF_Ecshop_BundlingProductItems_ProductNum]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Ecshop_BundlingProductItems_ProductNum]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_BundlingProductItems]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Ecshop_BundlingProductItems_ProductNum]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Ecshop_BundlingProductItems] DROP CONSTRAINT [DF_Ecshop_BundlingProductItems_ProductNum]
END


End
GO
/****** Object:  Default [DF_Ecshop_BundlingProducts_Num]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Ecshop_BundlingProducts_Num]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_BundlingProducts]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Ecshop_BundlingProducts_Num]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Ecshop_BundlingProducts] DROP CONSTRAINT [DF_Ecshop_BundlingProducts_Num]
END


End
GO
/****** Object:  Default [DF_Ecshop_BundlingProducts_Price]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Ecshop_BundlingProducts_Price]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_BundlingProducts]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Ecshop_BundlingProducts_Price]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Ecshop_BundlingProducts] DROP CONSTRAINT [DF_Ecshop_BundlingProducts_Price]
END


End
GO
/****** Object:  Default [DF_Ecshop_Categories_HasChildren]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Ecshop_Categories_HasChildren]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_Categories]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Ecshop_Categories_HasChildren]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Ecshop_Categories] DROP CONSTRAINT [DF_Ecshop_Categories_HasChildren]
END


End
GO
/****** Object:  Default [DF_Ecshop_CountDown_DisplaySequence]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Ecshop_CountDown_DisplaySequence]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_CountDown]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Ecshop_CountDown_DisplaySequence]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Ecshop_CountDown] DROP CONSTRAINT [DF_Ecshop_CountDown_DisplaySequence]
END


End
GO
/****** Object:  Default [DF_Ecshop_CountDown_MaxCount]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Ecshop_CountDown_MaxCount]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_CountDown]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Ecshop_CountDown_MaxCount]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Ecshop_CountDown] DROP CONSTRAINT [DF_Ecshop_CountDown_MaxCount]
END


End
GO
/****** Object:  Default [DF_Ecshop_CouponItems_CouponStatus]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Ecshop_CouponItems_CouponStatus]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_CouponItems]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Ecshop_CouponItems_CouponStatus]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Ecshop_CouponItems] DROP CONSTRAINT [DF_Ecshop_CouponItems_CouponStatus]
END


End
GO
/****** Object:  Default [DF_Ecshop_Coupons_SentCount]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Ecshop_Coupons_SentCount]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_Coupons]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Ecshop_Coupons_SentCount]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Ecshop_Coupons] DROP CONSTRAINT [DF_Ecshop_Coupons_SentCount]
END


End
GO
/****** Object:  Default [DF_Ecshop_Coupons_UsedCount]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Ecshop_Coupons_UsedCount]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_Coupons]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Ecshop_Coupons_UsedCount]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Ecshop_Coupons] DROP CONSTRAINT [DF_Ecshop_Coupons_UsedCount]
END


End
GO
/****** Object:  Default [DF_Ecshop_Coupons_NeedPoint]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Ecshop_Coupons_NeedPoint]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_Coupons]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Ecshop_Coupons_NeedPoint]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Ecshop_Coupons] DROP CONSTRAINT [DF_Ecshop_Coupons_NeedPoint]
END


End
GO
/****** Object:  Default [DF_Ecshop_Gifts_NeedPoint]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Ecshop_Gifts_NeedPoint]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_Gifts]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Ecshop_Gifts_NeedPoint]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Ecshop_Gifts] DROP CONSTRAINT [DF_Ecshop_Gifts_NeedPoint]
END


End
GO
/****** Object:  Default [DF_Ecshop_GiftShoppingCarts_AddTime]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Ecshop_GiftShoppingCarts_AddTime]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_GiftShoppingCarts]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Ecshop_GiftShoppingCarts_AddTime]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Ecshop_GiftShoppingCarts] DROP CONSTRAINT [DF_Ecshop_GiftShoppingCarts_AddTime]
END


End
GO
/****** Object:  Default [DF_Ecshop_GiftShoppingCarts_PromoType]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Ecshop_GiftShoppingCarts_PromoType]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_GiftShoppingCarts]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Ecshop_GiftShoppingCarts_PromoType]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Ecshop_GiftShoppingCarts] DROP CONSTRAINT [DF_Ecshop_GiftShoppingCarts_PromoType]
END


End
GO
/****** Object:  Default [DF_Ecshop_GroupBuy_DisplaySequence]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Ecshop_GroupBuy_DisplaySequence]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_GroupBuy]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Ecshop_GroupBuy_DisplaySequence]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Ecshop_GroupBuy] DROP CONSTRAINT [DF_Ecshop_GroupBuy_DisplaySequence]
END


End
GO
/****** Object:  Default [DF_Ecshop_LeaveComments_IsReply]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Ecshop_LeaveComments_IsReply]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_LeaveComments]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Ecshop_LeaveComments_IsReply]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Ecshop_LeaveComments] DROP CONSTRAINT [DF_Ecshop_LeaveComments_IsReply]
END


End
GO
/****** Object:  Default [DF_Ecshop_MessageTemplates_SendEmail]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Ecshop_MessageTemplates_SendEmail]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_MessageTemplates]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Ecshop_MessageTemplates_SendEmail]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Ecshop_MessageTemplates] DROP CONSTRAINT [DF_Ecshop_MessageTemplates_SendEmail]
END


End
GO
/****** Object:  Default [DF_Ecshop_MessageTemplates_SendSMS]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Ecshop_MessageTemplates_SendSMS]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_MessageTemplates]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Ecshop_MessageTemplates_SendSMS]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Ecshop_MessageTemplates] DROP CONSTRAINT [DF_Ecshop_MessageTemplates_SendSMS]
END


End
GO
/****** Object:  Default [DF_Ecshop_MessageTemplates_SendInnerMessage]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Ecshop_MessageTemplates_SendInnerMessage]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_MessageTemplates]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Ecshop_MessageTemplates_SendInnerMessage]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Ecshop_MessageTemplates] DROP CONSTRAINT [DF_Ecshop_MessageTemplates_SendInnerMessage]
END


End
GO
/****** Object:  Default [DF_Ecshop_MessageTemplates_SendWeixinMessage]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Ecshop_MessageTemplates_SendWeixinMessage]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_MessageTemplates]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Ecshop_MessageTemplates_SendWeixinMessage]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Ecshop_MessageTemplates] DROP CONSTRAINT [DF_Ecshop_MessageTemplates_SendWeixinMessage]
END


End
GO
/****** Object:  Default [DF_Ecshop_OrderGifts_PromoType]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Ecshop_OrderGifts_PromoType]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_OrderGifts]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Ecshop_OrderGifts_PromoType]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Ecshop_OrderGifts] DROP CONSTRAINT [DF_Ecshop_OrderGifts_PromoType]
END


End
GO
/****** Object:  Default [DF__tmp_ms_xx__Updat__49CEE3AF]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF__tmp_ms_xx__Updat__49CEE3AF]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_Orders]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__tmp_ms_xx__Updat__49CEE3AF]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Ecshop_Orders] DROP CONSTRAINT [DF__tmp_ms_xx__Updat__49CEE3AF]
END


End
GO
/****** Object:  Default [DF__tmp_ms_xx__Sourc__4AC307E8]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF__tmp_ms_xx__Sourc__4AC307E8]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_Orders]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__tmp_ms_xx__Sourc__4AC307E8]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Ecshop_Orders] DROP CONSTRAINT [DF__tmp_ms_xx__Sourc__4AC307E8]
END


End
GO
/****** Object:  Default [DF_Ecshop_PaymentTypes_IsUseInpour]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Ecshop_PaymentTypes_IsUseInpour]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_PaymentTypes]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Ecshop_PaymentTypes_IsUseInpour]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Ecshop_PaymentTypes] DROP CONSTRAINT [DF_Ecshop_PaymentTypes_IsUseInpour]
END


End
GO
/****** Object:  Default [DF__tmp_ms_xx__Updat__22B5168E]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF__tmp_ms_xx__Updat__22B5168E]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_Products]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__tmp_ms_xx__Updat__22B5168E]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Ecshop_Products] DROP CONSTRAINT [DF__tmp_ms_xx__Updat__22B5168E]
END


End
GO
/****** Object:  Default [DF_Ecshop_Products_VistiCounts]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Ecshop_Products_VistiCounts]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_Products]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Ecshop_Products_VistiCounts]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Ecshop_Products] DROP CONSTRAINT [DF_Ecshop_Products_VistiCounts]
END


End
GO
/****** Object:  Default [DF_Ecshop_Products_SaleCounts]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Ecshop_Products_SaleCounts]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_Products]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Ecshop_Products_SaleCounts]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Ecshop_Products] DROP CONSTRAINT [DF_Ecshop_Products_SaleCounts]
END


End
GO
/****** Object:  Default [DF_Ecshop_Products_ShowSaleCounts]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Ecshop_Products_ShowSaleCounts]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_Products]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Ecshop_Products_ShowSaleCounts]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Ecshop_Products] DROP CONSTRAINT [DF_Ecshop_Products_ShowSaleCounts]
END


End
GO
/****** Object:  Default [DF_Ecshop_Products_DisplaySequence]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Ecshop_Products_DisplaySequence]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_Products]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Ecshop_Products_DisplaySequence]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Ecshop_Products] DROP CONSTRAINT [DF_Ecshop_Products_DisplaySequence]
END


End
GO
/****** Object:  Default [DF_Ecshop_Shippers_DistributorUserId]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Ecshop_Shippers_DistributorUserId]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_Shippers]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Ecshop_Shippers_DistributorUserId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Ecshop_Shippers] DROP CONSTRAINT [DF_Ecshop_Shippers_DistributorUserId]
END


End
GO
/****** Object:  Default [DF_Ecshop_ShoppingCarts_AddTime]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Ecshop_ShoppingCarts_AddTime]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_ShoppingCarts]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Ecshop_ShoppingCarts_AddTime]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Ecshop_ShoppingCarts] DROP CONSTRAINT [DF_Ecshop_ShoppingCarts_AddTime]
END


End
GO
/****** Object:  Default [DF__Ecshop_Us__IsDef__0CC5D56F]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF__Ecshop_Us__IsDef__0CC5D56F]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_UserShippingAddresses]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Ecshop_Us__IsDef__0CC5D56F]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Ecshop_UserShippingAddresses] DROP CONSTRAINT [DF__Ecshop_Us__IsDef__0CC5D56F]
END


End
GO
/****** Object:  Default [DF_Vshop_HomeProducts_Client]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Vshop_HomeProducts_Client]') AND parent_object_id = OBJECT_ID(N'[dbo].[Vshop_HomeProducts]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Vshop_HomeProducts_Client]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Vshop_HomeProducts] DROP CONSTRAINT [DF_Vshop_HomeProducts_Client]
END


End
GO
/****** Object:  Default [DF_Vshop_HomeTopics_Client]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Vshop_HomeTopics_Client]') AND parent_object_id = OBJECT_ID(N'[dbo].[Vshop_HomeTopics]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Vshop_HomeTopics_Client]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Vshop_HomeTopics] DROP CONSTRAINT [DF_Vshop_HomeTopics_Client]
END


End
GO
/****** Object:  Default [DF_vshop_Menu_Client]    Script Date: 12/10/2014 10:04:32 ******/
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_vshop_Menu_Client]') AND parent_object_id = OBJECT_ID(N'[dbo].[vshop_Menu]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_vshop_Menu_Client]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[vshop_Menu] DROP CONSTRAINT [DF_vshop_Menu_Client]
END


End
GO
/****** Object:  Role [hishopdev_user]    Script Date: 12/10/2014 10:04:32 ******/
DECLARE @RoleName sysname
set @RoleName = N'hishopdev_user'
IF  EXISTS (SELECT * FROM sys.database_principals WHERE name = @RoleName AND type = 'R')
Begin
	DECLARE @RoleMemberName sysname
	DECLARE Member_Cursor CURSOR FOR
	select [name]
	from sys.database_principals 
	where principal_id in ( 
		select member_principal_id 
		from sys.database_role_members 
		where role_principal_id in (
			select principal_id
			FROM sys.database_principals where [name] = @RoleName  AND type = 'R' ))

	OPEN Member_Cursor;

	FETCH NEXT FROM Member_Cursor
	into @RoleMemberName

	WHILE @@FETCH_STATUS = 0
	BEGIN

		exec sp_droprolemember @rolename=@RoleName, @membername= @RoleMemberName

		FETCH NEXT FROM Member_Cursor
		into @RoleMemberName
	END;

	CLOSE Member_Cursor;
	DEALLOCATE Member_Cursor;
End
GO
IF  EXISTS (SELECT * FROM sys.database_principals WHERE name = N'hishopdev_user' AND type = 'R')
DROP ROLE [hishopdev_user]
GO
/****** Object:  Role [hishopdev_user]    Script Date: 12/10/2014 10:04:32 ******/
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'hishopdev_user')
BEGIN
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'hishopdev_user' AND type = 'R')
CREATE ROLE [hishopdev_user] AUTHORIZATION [dbo]

END
GO
/****** Object:  Table [dbo].[Ecshop_TemplateRelatedShipping]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_TemplateRelatedShipping]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_TemplateRelatedShipping](
	[ModeId] [int] NOT NULL,
	[ExpressCompanyName] [nvarchar](500) COLLATE Chinese_PRC_CI_AS NULL
)
END
GO
/****** Object:  Table [dbo].[Ecshop_Tags]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_Tags]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_Tags](
	[TagID] [int] IDENTITY(1,1) NOT NULL,
	[TagName] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
 CONSTRAINT [PK_Ecshop_Tags] PRIMARY KEY NONCLUSTERED 
(
	[TagID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Ecshop_SplittinDraws]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_SplittinDraws]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_SplittinDraws](
	[JournalNumber] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[UserName] [nvarchar](256) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[RequestDate] [datetime] NOT NULL,
	[Amount] [money] NOT NULL,
	[Account] [nvarchar](300) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[AuditStatus] [int] NOT NULL,
	[AccountDate] [datetime] NULL,
	[ManagerRemark] [nvarchar](300) COLLATE Chinese_PRC_CI_AS NULL,
 CONSTRAINT [PK_Ecshop_SplittinDraws] PRIMARY KEY CLUSTERED 
(
	[JournalNumber] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Ecshop_SplittinDetails]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_SplittinDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_SplittinDetails](
	[JournalNumber] [bigint] IDENTITY(1,1) NOT NULL,
	[OrderId] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[UserId] [int] NOT NULL,
	[UserName] [nvarchar](256) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[IsUse] [bit] NOT NULL,
	[TradeDate] [datetime] NOT NULL,
	[TradeType] [int] NOT NULL,
	[SubUserId] [int] NULL,
	[Income] [money] NULL,
	[Expenses] [money] NULL,
	[Balance] [money] NOT NULL,
	[Remark] [nvarchar](2000) COLLATE Chinese_PRC_CI_AS NULL,
 CONSTRAINT [PK_Ecshop_SplittinDetails] PRIMARY KEY CLUSTERED 
(
	[JournalNumber] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Ecshop_PrivilegeInRoles]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_PrivilegeInRoles]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_PrivilegeInRoles](
	[RoleId] [uniqueidentifier] NOT NULL,
	[Privilege] [int] NOT NULL,
 CONSTRAINT [PK_Ecshop_PrivilegeInRoles] PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC,
	[Privilege] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Ecshop_PhotoGallery]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_PhotoGallery]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_PhotoGallery](
	[PhotoId] [int] IDENTITY(1,1) NOT NULL,
	[CategoryId] [int] NOT NULL,
	[PhotoName] [nvarchar](100) COLLATE Chinese_PRC_CI_AS NULL,
	[PhotoPath] [varchar](300) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[FileSize] [int] NOT NULL,
	[UploadTime] [datetime] NOT NULL,
	[LastUpdateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_distro_PhotoGallery] PRIMARY KEY CLUSTERED 
(
	[PhotoId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Ecshop_PhotoCategories]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_PhotoCategories]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_PhotoCategories](
	[CategoryId] [int] IDENTITY(1,1) NOT NULL,
	[CategoryName] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[DisplaySequence] [int] NOT NULL,
 CONSTRAINT [PK_distro_PhotoCategories] PRIMARY KEY CLUSTERED 
(
	[CategoryId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Ecshop_PaymentTypes]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_PaymentTypes]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_PaymentTypes](
	[ModeId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[Description] [ntext] COLLATE Chinese_PRC_CI_AS NULL,
	[Gateway] [nvarchar](200) COLLATE Chinese_PRC_CI_AS NULL,
	[DisplaySequence] [int] NOT NULL,
	[IsUseInpour] [bit] NOT NULL,
	[Charge] [money] NOT NULL,
	[IsPercent] [bit] NOT NULL,
	[Settings] [ntext] COLLATE Chinese_PRC_CI_AS NULL,
	[ApplicationType] [int] NULL,
 CONSTRAINT [PK_Ecshop_PaymentTypes] PRIMARY KEY CLUSTERED 
(
	[ModeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Ecshop_Shippers]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_Shippers]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_Shippers](
	[ShipperId] [int] IDENTITY(1,1) NOT NULL,
	[DistributorUserId] [int] NOT NULL,
	[IsDefault] [bit] NOT NULL,
	[ShipperTag] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[ShipperName] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[RegionId] [int] NOT NULL,
	[Address] [nvarchar](300) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[CellPhone] [nvarchar](20) COLLATE Chinese_PRC_CI_AS NULL,
	[TelPhone] [nvarchar](20) COLLATE Chinese_PRC_CI_AS NULL,
	[Zipcode] [nvarchar](20) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[Remark] [nvarchar](300) COLLATE Chinese_PRC_CI_AS NULL,
 CONSTRAINT [PK_Ecshop_Shippers] PRIMARY KEY CLUSTERED 
(
	[ShipperId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Ecshop_RelatedProducts]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_RelatedProducts]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_RelatedProducts](
	[ProductId] [int] NOT NULL,
	[RelatedProductId] [int] NOT NULL,
 CONSTRAINT [PK_Ecshop_RelatedProducts] PRIMARY KEY CLUSTERED 
(
	[ProductId] ASC,
	[RelatedProductId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Ecshop_RelatedArticsProducts]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_RelatedArticsProducts]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_RelatedArticsProducts](
	[ArticleId] [int] NOT NULL,
	[RelatedProductId] [int] NOT NULL,
 CONSTRAINT [PK_Ecshop_RelatedArticsProducts] PRIMARY KEY CLUSTERED 
(
	[ArticleId] ASC,
	[RelatedProductId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Ecshop_Promotions]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_Promotions]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_Promotions](
	[ActivityId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[PromoteType] [int] NOT NULL,
	[Condition] [money] NOT NULL,
	[DiscountValue] [money] NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[EndDate] [datetime] NOT NULL,
	[Description] [ntext] COLLATE Chinese_PRC_CI_AS NULL,
 CONSTRAINT [PK_Ecshop_Promotions] PRIMARY KEY CLUSTERED 
(
	[ActivityId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Ecshop_ProductTypes]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_ProductTypes]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_ProductTypes](
	[TypeId] [int] IDENTITY(1,1) NOT NULL,
	[TypeName] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[Remark] [nvarchar](200) COLLATE Chinese_PRC_CI_AS NULL,
 CONSTRAINT [PK_Ecshop_ProductTypes] PRIMARY KEY CLUSTERED 
(
	[TypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Ecshop_ShippingTemplates]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_ShippingTemplates]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_ShippingTemplates](
	[TemplateId] [int] IDENTITY(1,1) NOT NULL,
	[TemplateName] [nvarchar](100) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[Weight] [money] NOT NULL,
	[AddWeight] [money] NULL,
	[Price] [money] NOT NULL,
	[AddPrice] [money] NULL,
 CONSTRAINT [PK_Ecshop_ShippingTemplates] PRIMARY KEY CLUSTERED 
(
	[TemplateId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Ecshop_Votes]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_Votes]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_Votes](
	[VoteId] [bigint] IDENTITY(1,1) NOT NULL,
	[VoteName] [nvarchar](100) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[IsBackup] [bit] NOT NULL,
	[MaxCheck] [int] NOT NULL,
 CONSTRAINT [PK_Ecshop_Votes] PRIMARY KEY NONCLUSTERED 
(
	[VoteId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Vshop_Topics]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Vshop_Topics]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Vshop_Topics](
	[TopicId] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](200) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[IconUrl] [nvarchar](300) COLLATE Chinese_PRC_CI_AS NULL,
	[Content] [nvarchar](max) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[AddedDate] [datetime] NOT NULL,
	[IsRelease] [bit] NOT NULL,
	[DisplaySequence] [int] NULL,
 CONSTRAINT [PK__tmp_ms_x__022E0F5D070CFC19] PRIMARY KEY CLUSTERED 
(
	[TopicId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[vshop_Reply]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[vshop_Reply]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[vshop_Reply](
	[ReplyId] [int] IDENTITY(1,1) NOT NULL,
	[Keys] [nvarchar](200) COLLATE Chinese_PRC_CI_AS NULL,
	[MatchType] [int] NULL,
	[ReplyType] [int] NULL,
	[MessageType] [int] NULL,
	[IsDisable] [bit] NULL,
	[LastEditDate] [datetime] NOT NULL,
	[LastEditor] [nvarchar](255) COLLATE Chinese_PRC_CI_AS NULL,
	[Content] [ntext] COLLATE Chinese_PRC_CI_AS NULL,
	[Type] [int] NULL,
	[ActivityId] [int] NULL,
 CONSTRAINT [PK_vshop_Reply] PRIMARY KEY CLUSTERED 
(
	[ReplyId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Vshop_RelatedTopicProducts]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Vshop_RelatedTopicProducts]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Vshop_RelatedTopicProducts](
	[TopicId] [int] NOT NULL,
	[RelatedProductId] [int] NOT NULL,
	[DisplaySequence] [int] NULL
)
END
GO
/****** Object:  Table [dbo].[Vshop_PrizeRecord]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Vshop_PrizeRecord]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Vshop_PrizeRecord](
	[RecordId] [int] IDENTITY(1,1) NOT NULL,
	[ActivityID] [int] NULL,
	[PrizeTime] [datetime] NULL,
	[UserID] [int] NULL,
	[RealName] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[CellPhone] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[PrizeName] [nvarchar](200) COLLATE Chinese_PRC_CI_AS NULL,
	[Prizelevel] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[IsPrize] [bit] NULL,
 CONSTRAINT [PK__tmp_ms_x__FBDF78E9033C6B35] PRIMARY KEY CLUSTERED 
(
	[RecordId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[vshop_Message]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[vshop_Message]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[vshop_Message](
	[ReplyId] [int] NULL,
	[MsgID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](200) COLLATE Chinese_PRC_CI_AS NULL,
	[ImageUrl] [nvarchar](255) COLLATE Chinese_PRC_CI_AS NULL,
	[Url] [nvarchar](255) COLLATE Chinese_PRC_CI_AS NULL,
	[Description] [nvarchar](1000) COLLATE Chinese_PRC_CI_AS NULL,
	[Content] [ntext] COLLATE Chinese_PRC_CI_AS NULL,
 CONSTRAINT [PK_Ecshop_Message] PRIMARY KEY NONCLUSTERED 
(
	[MsgID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[vshop_Menu]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[vshop_Menu]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[vshop_Menu](
	[MenuId] [int] IDENTITY(1,1) NOT NULL,
	[ParentMenuId] [int] NULL,
	[Name] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Type] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[ReplyId] [int] NULL,
	[DisplaySequence] [int] NULL,
	[Bind] [int] NULL,
	[Content] [nvarchar](300) COLLATE Chinese_PRC_CI_AS NULL,
	[Client] [tinyint] NOT NULL,
 CONSTRAINT [PK_vshop_Menu] PRIMARY KEY CLUSTERED 
(
	[MenuId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Vshop_LotteryActivity]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Vshop_LotteryActivity]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Vshop_LotteryActivity](
	[ActivityId] [int] IDENTITY(1,1) NOT NULL,
	[ActivityName] [nvarchar](150) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[ActivityType] [int] NULL,
	[StartTime] [datetime] NULL,
	[EndTime] [datetime] NULL,
	[ActivityDesc] [nvarchar](1000) COLLATE Chinese_PRC_CI_AS NULL,
	[ActivityKey] [nvarchar](150) COLLATE Chinese_PRC_CI_AS NULL,
	[ActivityPic] [nvarchar](300) COLLATE Chinese_PRC_CI_AS NULL,
	[PrizeSetting] [nvarchar](1000) COLLATE Chinese_PRC_CI_AS NULL,
	[MaxNum] [int] NULL,
	[GradeIds] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[MinValue] [int] NULL,
	[InvitationCode] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[OpenTime] [datetime] NULL,
	[IsOpened] [bit] NULL,
 CONSTRAINT [PK__tmp_ms_x__45F4A7917F6BDA51] PRIMARY KEY CLUSTERED 
(
	[ActivityId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Vshop_HomeTopics]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Vshop_HomeTopics]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Vshop_HomeTopics](
	[TopicId] [int] NOT NULL,
	[DisplaySequence] [int] NULL,
	[Client] [int] NOT NULL
)
END
GO
/****** Object:  Table [dbo].[Vshop_HomeProducts]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Vshop_HomeProducts]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Vshop_HomeProducts](
	[ProductId] [int] NOT NULL,
	[DisplaySequence] [int] NULL,
	[Client] [int] NOT NULL
)
END
GO
/****** Object:  Table [dbo].[vshop_FeedBackNotify]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[vshop_FeedBackNotify]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[vshop_FeedBackNotify](
	[FeedBackNotifyID] [int] IDENTITY(1,1) NOT NULL,
	[AppId] [nvarchar](100) COLLATE Chinese_PRC_CI_AS NULL,
	[TimeStamp] [datetime] NULL,
	[OpenId] [nvarchar](100) COLLATE Chinese_PRC_CI_AS NULL,
	[MsgType] [nvarchar](20) COLLATE Chinese_PRC_CI_AS NULL,
	[FeedBackId] [nvarchar](255) COLLATE Chinese_PRC_CI_AS NULL,
	[TransId] [nvarchar](255) COLLATE Chinese_PRC_CI_AS NULL,
	[Reason] [nvarchar](1000) COLLATE Chinese_PRC_CI_AS NULL,
	[Solution] [nvarchar](1000) COLLATE Chinese_PRC_CI_AS NULL,
	[ExtInfo] [nvarchar](1000) COLLATE Chinese_PRC_CI_AS NULL,
 CONSTRAINT [PK_Ecshop_FeedBackNotify] PRIMARY KEY NONCLUSTERED 
(
	[FeedBackNotifyID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[vshop_AlarmNotify]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[vshop_AlarmNotify]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[vshop_AlarmNotify](
	[AlarmNotifyId] [int] IDENTITY(1,1) NOT NULL,
	[AppId] [nvarchar](100) COLLATE Chinese_PRC_CI_AS NULL,
	[ErrorType] [int] NULL,
	[Description] [nvarchar](500) COLLATE Chinese_PRC_CI_AS NULL,
	[AlarmContent] [nvarchar](2000) COLLATE Chinese_PRC_CI_AS NULL,
	[TimeStamp] [datetime] NULL,
 CONSTRAINT [PK_Ecshop_AlarmNotify] PRIMARY KEY NONCLUSTERED 
(
	[AlarmNotifyId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[vshop_ActivitySignUp]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[vshop_ActivitySignUp]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[vshop_ActivitySignUp](
	[ActivitySignUpId] [int] IDENTITY(1,1) NOT NULL,
	[ActivityId] [int] NULL,
	[UserId] [int] NULL,
	[UserName] [nvarchar](256) COLLATE Chinese_PRC_CI_AS NULL,
	[RealName] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[SignUpDate] [datetime] NULL,
	[Item1] [nvarchar](200) COLLATE Chinese_PRC_CI_AS NULL,
	[Item2] [nvarchar](200) COLLATE Chinese_PRC_CI_AS NULL,
	[Item3] [nvarchar](200) COLLATE Chinese_PRC_CI_AS NULL,
	[Item4] [nvarchar](200) COLLATE Chinese_PRC_CI_AS NULL,
	[Item5] [nvarchar](200) COLLATE Chinese_PRC_CI_AS NULL,
 CONSTRAINT [PK_vshop_ActivitySignUp] PRIMARY KEY CLUSTERED 
(
	[ActivitySignUpId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[vshop_Activity]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[vshop_Activity]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[vshop_Activity](
	[ActivityId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) COLLATE Chinese_PRC_CI_AS NULL,
	[Description] [nvarchar](500) COLLATE Chinese_PRC_CI_AS NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[CloseRemark] [nvarchar](1000) COLLATE Chinese_PRC_CI_AS NULL,
	[Keys] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[MaxValue] [nchar](10) COLLATE Chinese_PRC_CI_AS NULL,
	[PicUrl] [nvarchar](300) COLLATE Chinese_PRC_CI_AS NULL,
	[Item1] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Item2] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Item3] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Item4] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Item5] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
 CONSTRAINT [PK_vshop_Activity] PRIMARY KEY CLUSTERED 
(
	[ActivityId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Ecshop_Products]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_Products]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_Products](
	[CategoryId] [int] NOT NULL,
	[TypeId] [int] NULL,
	[ProductId] [int] IDENTITY(1,1) NOT NULL,
	[ProductName] [nvarchar](200) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[ProductCode] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[ShortDescription] [nvarchar](2000) COLLATE Chinese_PRC_CI_AS NULL,
	[Unit] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Description] [ntext] COLLATE Chinese_PRC_CI_AS NULL,
	[MobbileDescription] [ntext] COLLATE Chinese_PRC_CI_AS NULL,
	[Title] [nvarchar](100) COLLATE Chinese_PRC_CI_AS NULL,
	[Meta_Description] [nvarchar](1000) COLLATE Chinese_PRC_CI_AS NULL,
	[Meta_Keywords] [nvarchar](1000) COLLATE Chinese_PRC_CI_AS NULL,
	[SaleStatus] [int] NOT NULL,
	[AddedDate] [datetime] NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
	[VistiCounts] [int] NOT NULL,
	[SaleCounts] [int] NOT NULL,
	[ShowSaleCounts] [int] NOT NULL,
	[DisplaySequence] [int] NOT NULL,
	[ImageUrl1] [nvarchar](255) COLLATE Chinese_PRC_CI_AS NULL,
	[ImageUrl2] [nvarchar](255) COLLATE Chinese_PRC_CI_AS NULL,
	[ImageUrl3] [nvarchar](255) COLLATE Chinese_PRC_CI_AS NULL,
	[ImageUrl4] [nvarchar](255) COLLATE Chinese_PRC_CI_AS NULL,
	[ImageUrl5] [nvarchar](255) COLLATE Chinese_PRC_CI_AS NULL,
	[ThumbnailUrl40] [nvarchar](255) COLLATE Chinese_PRC_CI_AS NULL,
	[ThumbnailUrl60] [nvarchar](255) COLLATE Chinese_PRC_CI_AS NULL,
	[ThumbnailUrl100] [nvarchar](255) COLLATE Chinese_PRC_CI_AS NULL,
	[ThumbnailUrl160] [nvarchar](255) COLLATE Chinese_PRC_CI_AS NULL,
	[ThumbnailUrl180] [nvarchar](255) COLLATE Chinese_PRC_CI_AS NULL,
	[ThumbnailUrl220] [nvarchar](255) COLLATE Chinese_PRC_CI_AS NULL,
	[ThumbnailUrl310] [nvarchar](255) COLLATE Chinese_PRC_CI_AS NULL,
	[ThumbnailUrl410] [nvarchar](255) COLLATE Chinese_PRC_CI_AS NULL,
	[MarketPrice] [money] NULL,
	[BrandId] [int] NULL,
	[MainCategoryPath] [nvarchar](256) COLLATE Chinese_PRC_CI_AS NULL,
	[ExtendCategoryPath] [nvarchar](256) COLLATE Chinese_PRC_CI_AS NULL,
	[HasSKU] [bit] NOT NULL,
	[IsfreeShipping] [bit] NULL,
	[TaobaoProductId] [bigint] NULL,
	[ReferralDeduct] [money] NULL,
	[SubMemberDeduct] [money] NULL,
	[SubReferralDeduct] [money] NULL,
 CONSTRAINT [PK_Ecshop_Products] PRIMARY KEY NONCLUSTERED 
(
	[ProductId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_Products]') AND name = N'Ecshop_Products_Index')
CREATE CLUSTERED INDEX [Ecshop_Products_Index] ON [dbo].[Ecshop_Products] 
(
	[DisplaySequence] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_Products]') AND name = N'Ecshop_Products_Index2')
CREATE NONCLUSTERED INDEX [Ecshop_Products_Index2] ON [dbo].[Ecshop_Products] 
(
	[MainCategoryPath] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_Products]') AND name = N'Ecshop_Products_Index3')
CREATE NONCLUSTERED INDEX [Ecshop_Products_Index3] ON [dbo].[Ecshop_Products] 
(
	[ExtendCategoryPath] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
GO
/****** Object:  Table [dbo].[Ecshop_Orders]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_Orders]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_Orders](
	[OrderId] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[Remark] [nvarchar](4000) COLLATE Chinese_PRC_CI_AS NULL,
	[ManagerMark] [int] NULL,
	[ManagerRemark] [nvarchar](4000) COLLATE Chinese_PRC_CI_AS NULL,
	[AdjustedDiscount] [money] NULL,
	[OrderStatus] [int] NOT NULL,
	[CloseReason] [nvarchar](4000) COLLATE Chinese_PRC_CI_AS NULL,
	[OrderDate] [datetime] NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
	[PayDate] [datetime] NULL,
	[ShippingDate] [datetime] NULL,
	[FinishDate] [datetime] NULL,
	[ReferralUserId] [int] NULL,
	[UserId] [int] NOT NULL,
	[Username] [nvarchar](64) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[EmailAddress] [nvarchar](255) COLLATE Chinese_PRC_CI_AS NULL,
	[RealName] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[QQ] [nvarchar](20) COLLATE Chinese_PRC_CI_AS NULL,
	[Wangwang] [nvarchar](20) COLLATE Chinese_PRC_CI_AS NULL,
	[MSN] [nvarchar](128) COLLATE Chinese_PRC_CI_AS NULL,
	[ShippingRegion] [nvarchar](300) COLLATE Chinese_PRC_CI_AS NULL,
	[Address] [nvarchar](300) COLLATE Chinese_PRC_CI_AS NULL,
	[ZipCode] [nvarchar](20) COLLATE Chinese_PRC_CI_AS NULL,
	[ShipTo] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[TelPhone] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[CellPhone] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[ShipToDate] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[ShippingModeId] [int] NULL,
	[ModeName] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[RealShippingModeId] [int] NULL,
	[RealModeName] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[RegionId] [int] NULL,
	[Freight] [money] NULL,
	[AdjustedFreight] [money] NULL,
	[ShipOrderNumber] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Weight] [money] NULL,
	[ExpressCompanyName] [nvarchar](500) COLLATE Chinese_PRC_CI_AS NULL,
	[ExpressCompanyAbb] [nvarchar](500) COLLATE Chinese_PRC_CI_AS NULL,
	[PaymentTypeId] [int] NULL,
	[PaymentType] [nvarchar](100) COLLATE Chinese_PRC_CI_AS NULL,
	[PayCharge] [money] NULL,
	[RefundStatus] [int] NULL,
	[RefundAmount] [money] NULL,
	[RefundRemark] [nvarchar](4000) COLLATE Chinese_PRC_CI_AS NULL,
	[Gateway] [nvarchar](200) COLLATE Chinese_PRC_CI_AS NULL,
	[OrderTotal] [money] NULL,
	[OrderPoint] [int] NULL,
	[OrderCostPrice] [money] NULL,
	[OrderProfit] [money] NULL,
	[ActualFreight] [money] NULL,
	[OtherCost] [money] NULL,
	[OptionPrice] [money] NULL,
	[Amount] [money] NULL,
	[DiscountAmount] [money] NULL,
	[ReducedPromotionId] [int] NULL,
	[ReducedPromotionName] [nvarchar](100) COLLATE Chinese_PRC_CI_AS NULL,
	[ReducedPromotionAmount] [money] NULL,
	[IsReduced] [bit] NULL,
	[SentTimesPointPromotionId] [int] NULL,
	[SentTimesPointPromotionName] [nvarchar](100) COLLATE Chinese_PRC_CI_AS NULL,
	[TimesPoint] [money] NULL,
	[IsSendTimesPoint] [bit] NULL,
	[FreightFreePromotionId] [int] NULL,
	[FreightFreePromotionName] [nvarchar](100) COLLATE Chinese_PRC_CI_AS NULL,
	[IsFreightFree] [bit] NULL,
	[DiscountValue] [nvarchar](100) COLLATE Chinese_PRC_CI_AS NULL,
	[CouponName] [nvarchar](100) COLLATE Chinese_PRC_CI_AS NULL,
	[CouponCode] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[CouponAmount] [money] NULL,
	[CouponValue] [money] NULL,
	[GroupBuyId] [int] NULL,
	[NeedPrice] [money] NULL,
	[GroupBuyStatus] [int] NULL,
	[CountDownBuyId] [int] NULL,
	[BundlingId] [int] NULL,
	[BundlingNum] [int] NULL,
	[BundlingPrice] [money] NULL,
	[GatewayOrderId] [nvarchar](100) COLLATE Chinese_PRC_CI_AS NULL,
	[IsPrinted] [bit] NULL,
	[TaobaoOrderId] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[SourceOrder] [int] NOT NULL,
	[Tax] [money] NULL,
	[InvoiceTitle] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Sender] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
 CONSTRAINT [PK_Ecshop_Orders] PRIMARY KEY NONCLUSTERED 
(
	[OrderId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_Orders]') AND name = N'Ecshop_Orders_Index')
CREATE CLUSTERED INDEX [Ecshop_Orders_Index] ON [dbo].[Ecshop_Orders] 
(
	[OrderDate] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_Orders]') AND name = N'Ecshop_Orders_Index2')
CREATE NONCLUSTERED INDEX [Ecshop_Orders_Index2] ON [dbo].[Ecshop_Orders] 
(
	[PaymentTypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_Orders]') AND name = N'Ecshop_Orders_Index3')
CREATE NONCLUSTERED INDEX [Ecshop_Orders_Index3] ON [dbo].[Ecshop_Orders] 
(
	[Username] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_Orders]') AND name = N'Ecshop_Orders_Index4')
CREATE NONCLUSTERED INDEX [Ecshop_Orders_Index4] ON [dbo].[Ecshop_Orders] 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
GO
/****** Object:  Table [dbo].[Ecshop_HelpCategories]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_HelpCategories]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_HelpCategories](
	[CategoryId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[DisplaySequence] [int] NOT NULL,
	[IconUrl] [nvarchar](255) COLLATE Chinese_PRC_CI_AS NULL,
	[IndexChar] [char](1) COLLATE Chinese_PRC_CI_AS NULL,
	[Description] [nvarchar](1000) COLLATE Chinese_PRC_CI_AS NULL,
	[IsShowFooter] [bit] NOT NULL,
 CONSTRAINT [PK_Ecshop_HelpCategories] PRIMARY KEY NONCLUSTERED 
(
	[CategoryId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Ecshop_Gifts]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_Gifts]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_Gifts](
	[GiftId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[ShortDescription] [nvarchar](2000) COLLATE Chinese_PRC_CI_AS NULL,
	[Unit] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[LongDescription] [ntext] COLLATE Chinese_PRC_CI_AS NULL,
	[Title] [nvarchar](100) COLLATE Chinese_PRC_CI_AS NULL,
	[Meta_Description] [nvarchar](1000) COLLATE Chinese_PRC_CI_AS NULL,
	[Meta_Keywords] [nvarchar](1000) COLLATE Chinese_PRC_CI_AS NULL,
	[CostPrice] [money] NULL,
	[ImageUrl] [nvarchar](255) COLLATE Chinese_PRC_CI_AS NULL,
	[ThumbnailUrl40] [nvarchar](255) COLLATE Chinese_PRC_CI_AS NULL,
	[ThumbnailUrl60] [nvarchar](255) COLLATE Chinese_PRC_CI_AS NULL,
	[ThumbnailUrl100] [nvarchar](255) COLLATE Chinese_PRC_CI_AS NULL,
	[ThumbnailUrl160] [nvarchar](255) COLLATE Chinese_PRC_CI_AS NULL,
	[ThumbnailUrl180] [nvarchar](255) COLLATE Chinese_PRC_CI_AS NULL,
	[ThumbnailUrl220] [nvarchar](255) COLLATE Chinese_PRC_CI_AS NULL,
	[ThumbnailUrl310] [nvarchar](255) COLLATE Chinese_PRC_CI_AS NULL,
	[ThumbnailUrl410] [nvarchar](255) COLLATE Chinese_PRC_CI_AS NULL,
	[MarketPrice] [money] NULL,
	[NeedPoint] [int] NOT NULL,
	[IsPromotion] [bit] NOT NULL,
 CONSTRAINT [PK_Ecshop_Gifts] PRIMARY KEY CLUSTERED 
(
	[GiftId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Ecshop_FriendlyLinks]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_FriendlyLinks]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_FriendlyLinks](
	[LinkId] [int] IDENTITY(1,1) NOT NULL,
	[ImageUrl] [nvarchar](255) COLLATE Chinese_PRC_CI_AS NULL,
	[LinkUrl] [nvarchar](255) COLLATE Chinese_PRC_CI_AS NULL,
	[Title] [nvarchar](100) COLLATE Chinese_PRC_CI_AS NULL,
	[Visible] [bit] NOT NULL,
	[DisplaySequence] [int] NOT NULL,
 CONSTRAINT [PK_Ecshop_Links] PRIMARY KEY CLUSTERED 
(
	[LinkId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Ecshop_FavoriteTags]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_FavoriteTags]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_FavoriteTags](
	[TagId] [int] IDENTITY(1,1) NOT NULL,
	[TagName] [varchar](100) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[UserId] [int] NOT NULL,
	[UpdateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_Ecshop_FavoriteTags] PRIMARY KEY CLUSTERED 
(
	[TagId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Ecshop_CellphoneQueue]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_CellphoneQueue]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_CellphoneQueue](
	[CellphoneId] [uniqueidentifier] NOT NULL,
	[CellphoneNumber] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[Subject] [nvarchar](1024) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[NextTryTime] [datetime] NOT NULL,
	[NumberOfTries] [int] NOT NULL,
 CONSTRAINT [PK_Ecshop_CellphoneQueue] PRIMARY KEY CLUSTERED 
(
	[CellphoneId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Ecshop_Categories]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_Categories]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_Categories](
	[CategoryId] [int] NOT NULL,
	[Name] [nvarchar](100) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[DisplaySequence] [int] NOT NULL,
	[Meta_Title] [nvarchar](1000) COLLATE Chinese_PRC_CI_AS NULL,
	[Meta_Description] [nvarchar](1000) COLLATE Chinese_PRC_CI_AS NULL,
	[Meta_Keywords] [nvarchar](1000) COLLATE Chinese_PRC_CI_AS NULL,
	[ParentCategoryId] [int] NULL,
	[Depth] [int] NOT NULL,
	[Path] [varchar](4000) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[RewriteName] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[SKUPrefix] [nvarchar](10) COLLATE Chinese_PRC_CI_AS NULL,
	[AssociatedProductType] [int] NULL,
	[Notes1] [ntext] COLLATE Chinese_PRC_CI_AS NULL,
	[Notes2] [ntext] COLLATE Chinese_PRC_CI_AS NULL,
	[Notes3] [ntext] COLLATE Chinese_PRC_CI_AS NULL,
	[Notes4] [ntext] COLLATE Chinese_PRC_CI_AS NULL,
	[Notes5] [ntext] COLLATE Chinese_PRC_CI_AS NULL,
	[Theme] [varchar](100) COLLATE Chinese_PRC_CI_AS NULL,
	[HasChildren] [bit] NOT NULL,
	[Icon] [nvarchar](1000) COLLATE Chinese_PRC_CI_AS NULL,
 CONSTRAINT [PK_Ecshop_Categories] PRIMARY KEY CLUSTERED 
(
	[CategoryId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Ecshop_BundlingProducts]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_BundlingProducts]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_BundlingProducts](
	[BundlingID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](150) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[ShortDescription] [nvarchar](2000) COLLATE Chinese_PRC_CI_AS NULL,
	[Num] [int] NOT NULL,
	[Price] [money] NOT NULL,
	[SaleStatus] [int] NOT NULL,
	[AddTime] [datetime] NULL,
	[DisplaySequence] [int] NULL,
 CONSTRAINT [PK_Ecshop_BundlingProducts] PRIMARY KEY NONCLUSTERED 
(
	[BundlingID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Ecshop_ExpressTemplates]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_ExpressTemplates]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_ExpressTemplates](
	[ExpressId] [int] IDENTITY(1,1) NOT NULL,
	[ExpressName] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[XmlFile] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[IsUse] [bit] NOT NULL,
 CONSTRAINT [PK_Ecshop_ExpressTemplates] PRIMARY KEY CLUSTERED 
(
	[ExpressId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Ecshop_EmailQueue]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_EmailQueue]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_EmailQueue](
	[EmailId] [uniqueidentifier] NOT NULL,
	[EmailPriority] [int] NOT NULL,
	[IsBodyHtml] [bit] NOT NULL,
	[EmailTo] [nvarchar](256) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[EmailCc] [ntext] COLLATE Chinese_PRC_CI_AS NULL,
	[EmailBcc] [ntext] COLLATE Chinese_PRC_CI_AS NULL,
	[EmailSubject] [nvarchar](1024) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[EmailBody] [ntext] COLLATE Chinese_PRC_CI_AS NOT NULL,
	[NextTryTime] [datetime] NOT NULL,
	[NumberOfTries] [int] NOT NULL,
 CONSTRAINT [PK_Ecshop_EmailQueue] PRIMARY KEY CLUSTERED 
(
	[EmailId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Ecshop_Coupons]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_Coupons]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_Coupons](
	[CouponId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[StartTime] [datetime] NOT NULL,
	[ClosingTime] [datetime] NOT NULL,
	[Description] [ntext] COLLATE Chinese_PRC_CI_AS NULL,
	[Amount] [money] NULL,
	[DiscountValue] [money] NOT NULL,
	[SentCount] [int] NOT NULL,
	[UsedCount] [int] NOT NULL,
	[NeedPoint] [int] NOT NULL,
 CONSTRAINT [PK_Ecshop_Coupons] PRIMARY KEY CLUSTERED 
(
	[CouponId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Ecshop_Logs]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_Logs]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_Logs](
	[LogId] [bigint] IDENTITY(1,1) NOT NULL,
	[PageUrl] [nvarchar](1000) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[AddedTime] [datetime] NOT NULL,
	[UserName] [nvarchar](64) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[IPAddress] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[Privilege] [int] NOT NULL,
	[Description] [ntext] COLLATE Chinese_PRC_CI_AS NULL,
 CONSTRAINT [PK_Ecshop_Logs] PRIMARY KEY NONCLUSTERED 
(
	[LogId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_Logs]') AND name = N'Ecshop_Logs_Index')
CREATE CLUSTERED INDEX [Ecshop_Logs_Index] ON [dbo].[Ecshop_Logs] 
(
	[AddedTime] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
GO
/****** Object:  Table [dbo].[Ecshop_LeaveComments]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_LeaveComments]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_LeaveComments](
	[LeaveId] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[UserName] [nvarchar](256) COLLATE Chinese_PRC_CI_AS NULL,
	[Title] [nvarchar](100) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[PublishContent] [nvarchar](3000) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[PublishDate] [datetime] NOT NULL,
	[LastDate] [datetime] NOT NULL,
	[IsReply] [bit] NOT NULL,
 CONSTRAINT [PK_Ecshop_LeaveComments] PRIMARY KEY NONCLUSTERED 
(
	[LeaveId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_LeaveComments]') AND name = N'Ecshop_LeaveComments_Index')
CREATE CLUSTERED INDEX [Ecshop_LeaveComments_Index] ON [dbo].[Ecshop_LeaveComments] 
(
	[LastDate] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
GO
/****** Object:  Table [dbo].[Ecshop_MessageTemplates]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_MessageTemplates]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_MessageTemplates](
	[MessageType] [nvarchar](100) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[Name] [nvarchar](100) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[SendEmail] [bit] NOT NULL,
	[SendSMS] [bit] NOT NULL,
	[SendInnerMessage] [bit] NOT NULL,
	[SendWeixin] [bit] NOT NULL,
	[WeixinTemplateId] [varchar](150) COLLATE Chinese_PRC_CI_AS NULL,
	[TagDescription] [nvarchar](500) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[EmailSubject] [nvarchar](1024) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[EmailBody] [ntext] COLLATE Chinese_PRC_CI_AS NOT NULL,
	[InnerMessageSubject] [nvarchar](1024) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[InnerMessageBody] [ntext] COLLATE Chinese_PRC_CI_AS NOT NULL,
	[SMSBody] [nvarchar](1024) COLLATE Chinese_PRC_CI_AS NOT NULL,
 CONSTRAINT [PK_Ecshop_MessageTemplates] PRIMARY KEY CLUSTERED 
(
	[MessageType] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Ecshop_MessageContent]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_MessageContent]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_MessageContent](
	[ContentId] [bigint] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](100) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[Content] [nvarchar](3000) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[Date] [datetime] NOT NULL,
 CONSTRAINT [PK_Ecshop_MessageContent] PRIMARY KEY NONCLUSTERED 
(
	[ContentId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Ecshop_MemberMessageBox]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_MemberMessageBox]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_MemberMessageBox](
	[MessageId] [bigint] IDENTITY(1,1) NOT NULL,
	[ContentId] [bigint] NOT NULL,
	[Sernder] [nvarchar](100) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[Accepter] [nvarchar](100) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[IsRead] [bit] NOT NULL,
 CONSTRAINT [PK_Ecshop_MemberMessageBox] PRIMARY KEY NONCLUSTERED 
(
	[MessageId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  View [dbo].[vw_Ecshop_MemberMessageBox]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_Ecshop_MemberMessageBox]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [dbo].[vw_Ecshop_MemberMessageBox]
AS
SELECT m.MessageId, m.Accepter, m.Sernder, m.IsRead, c.* 
FROM Ecshop_MemberMessageBox m INNER JOIN Ecshop_MessageContent c ON c.ContentId = m.ContentId'
GO
/****** Object:  Table [dbo].[Ecshop_ManagerMessageBox]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_ManagerMessageBox]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_ManagerMessageBox](
	[MessageId] [bigint] IDENTITY(1,1) NOT NULL,
	[ContentId] [bigint] NOT NULL,
	[Sernder] [nvarchar](100) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[Accepter] [nvarchar](100) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[IsRead] [bit] NOT NULL,
 CONSTRAINT [PK_Ecshop_MangerMessageBox] PRIMARY KEY NONCLUSTERED 
(
	[MessageId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Trigger [T_Ecshop_MemberMessageBox_Delete]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[T_Ecshop_MemberMessageBox_Delete]'))
EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER [dbo].[T_Ecshop_MemberMessageBox_Delete] ON [dbo].[Ecshop_MemberMessageBox] AFTER DELETE AS 

----删除Ecshop_MessageContent表中当前删除记录集合在Ecshop_ManagerMessageBox表和Ecshop_DistributorMessageBox表中不存在的所有记录
DELETE FROM Ecshop_MessageContent  WHERE  ContentId IN (SELECT ContentId FROM deleted)
	AND ContentId NOT IN (SELECT ContentId FROM Ecshop_ManagerMessageBox)'
GO
/****** Object:  Trigger [T_Ecshop_ManagerMessageBox_Delete]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[T_Ecshop_ManagerMessageBox_Delete]'))
EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER [dbo].[T_Ecshop_ManagerMessageBox_Delete] ON [dbo].[Ecshop_ManagerMessageBox] AFTER DELETE AS 

----删除Ecshop_MessageContent表中当前删除记录集合在Ecshop_MemberMessageBox表中不存在的所有记录
DELETE FROM Ecshop_MessageContent  WHERE  ContentId IN (SELECT ContentId FROM deleted)
	AND ContentId NOT IN (SELECT ContentId FROM Ecshop_MemberMessageBox)'
GO
/****** Object:  Table [dbo].[Ecshop_MemberClientSet]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_MemberClientSet]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_MemberClientSet](
	[ClientTypeId] [int] NOT NULL,
	[StartTime] [datetime] NULL,
	[EndTime] [datetime] NULL,
	[LastDay] [int] NULL,
	[ClientChar] [nvarchar](2) COLLATE Chinese_PRC_CI_AS NULL,
	[ClientValue] [decimal](18, 0) NULL,
 CONSTRAINT [PK_Ecshop_MemberClientSet] PRIMARY KEY NONCLUSTERED 
(
	[ClientTypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Ecshop_IntegrationSettings]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_IntegrationSettings]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_IntegrationSettings](
	[IntegrationForumId] [int] IDENTITY(1,1) NOT NULL,
	[applicationName] [nvarchar](200) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[IntegrationForumXML] [ntext] COLLATE Chinese_PRC_CI_AS NOT NULL,
	[IsOff] [bit] NOT NULL,
	[IsUsing] [bit] NOT NULL,
	[IntegrationForumURL] [nvarchar](200) COLLATE Chinese_PRC_CI_AS NOT NULL
)
END
GO
/****** Object:  Table [dbo].[aspnet_Users]    Script Date: 12/10/2014 10:04:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Users]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[aspnet_Users](
	[UserId] [int] IDENTITY(1100,1) NOT NULL,
	[UserName] [nvarchar](256) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[LoweredUserName] [nvarchar](256) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[MobilePIN] [nvarchar](16) COLLATE Chinese_PRC_CI_AS NULL,
	[IsAnonymous] [bit] NOT NULL,
	[LastActivityDate] [datetime] NOT NULL,
	[Password] [nvarchar](128) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[PasswordFormat] [int] NOT NULL,
	[PasswordSalt] [nvarchar](128) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[Email] [nvarchar](256) COLLATE Chinese_PRC_CI_AS NULL,
	[LoweredEmail] [nvarchar](256) COLLATE Chinese_PRC_CI_AS NULL,
	[PasswordQuestion] [nvarchar](256) COLLATE Chinese_PRC_CI_AS NULL,
	[PasswordAnswer] [nvarchar](128) COLLATE Chinese_PRC_CI_AS NULL,
	[IsApproved] [bit] NOT NULL,
	[IsLockedOut] [bit] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[LastLoginDate] [datetime] NOT NULL,
	[LastPasswordChangedDate] [datetime] NOT NULL,
	[LastLockoutDate] [datetime] NOT NULL,
	[FailedPasswordAttemptCount] [int] NOT NULL,
	[FailedPasswordAttemptWindowStart] [datetime] NOT NULL,
	[FailedPasswordAnswerAttemptCount] [int] NOT NULL,
	[FailedPasswordAnswerAttemptWindowStart] [datetime] NOT NULL,
	[Comment] [ntext] COLLATE Chinese_PRC_CI_AS NULL,
	[Gender] [int] NULL,
	[BirthDate] [datetime] NULL,
	[UserRole] [int] NULL,
	[AliOpenId] [varchar](100) COLLATE Chinese_PRC_CI_AS NULL,
	[OpenId] [nvarchar](128) COLLATE Chinese_PRC_CI_AS NULL,
	[OpenIdType] [nvarchar](200) COLLATE Chinese_PRC_CI_AS NULL,
	[SessionId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_aspnet_Users] PRIMARY KEY NONCLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Users]') AND name = N'aspnet_Users_Index')
CREATE UNIQUE CLUSTERED INDEX [aspnet_Users_Index] ON [dbo].[aspnet_Users] 
(
	[LoweredUserName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Users]') AND name = N'aspnet_Users_Index2')
CREATE NONCLUSTERED INDEX [aspnet_Users_Index2] ON [dbo].[aspnet_Users] 
(
	[LastActivityDate] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
GO
/****** Object:  View [dbo].[vw_Ecshop_ManagerMessageBox]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_Ecshop_ManagerMessageBox]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [dbo].[vw_Ecshop_ManagerMessageBox]
AS
SELECT m.MessageId, m.Accepter,(select UserId from dbo.aspnet_Users where UserName= m.Accepter) as AccepterUserId, m.Sernder,(select UserId from dbo.aspnet_Users where UserName= m.Sernder) as UserId, m.IsRead, c.* 
FROM Ecshop_ManagerMessageBox m INNER JOIN Ecshop_MessageContent c ON c.ContentId = m.ContentId'
GO
/****** Object:  Table [dbo].[aspnet_Members]    Script Date: 12/10/2014 10:04:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Members]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[aspnet_Members](
	[UserId] [int] NOT NULL,
	[GradeId] [int] NOT NULL,
	[ReferralUserId] [int] NULL,
	[ReferralStatus] [int] NOT NULL,
	[ReferralReason] [nvarchar](300) COLLATE Chinese_PRC_CI_AS NULL,
	[ReferralRequetsDate] [datetime] NULL,
	[RefusalReason] [nvarchar](300) COLLATE Chinese_PRC_CI_AS NULL,
	[ReferralAuditDate] [datetime] NULL,
	[IsOpenBalance] [bit] NOT NULL,
	[TradePassword] [nvarchar](128) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[TradePasswordSalt] [nvarchar](128) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[TradePasswordFormat] [int] NOT NULL,
	[OrderNumber] [int] NOT NULL,
	[Expenditure] [money] NOT NULL,
	[Points] [int] NOT NULL,
	[Balance] [money] NOT NULL,
	[RequestBalance] [money] NOT NULL,
	[TopRegionId] [int] NULL,
	[RegionId] [int] NULL,
	[RealName] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[IdentityCard] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Address] [nvarchar](300) COLLATE Chinese_PRC_CI_AS NULL,
	[Zipcode] [nvarchar](20) COLLATE Chinese_PRC_CI_AS NULL,
	[TelPhone] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[CellPhone] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[QQ] [nvarchar](20) COLLATE Chinese_PRC_CI_AS NULL,
	[Wangwang] [nvarchar](20) COLLATE Chinese_PRC_CI_AS NULL,
	[MSN] [nvarchar](128) COLLATE Chinese_PRC_CI_AS NULL,
	[WeChat] [nvarchar](128) COLLATE Chinese_PRC_CI_AS NULL,
	[VipCardNumber] [nvarchar](150) COLLATE Chinese_PRC_CI_AS NULL,
	[VipCardDate] [datetime] NULL,
	[OpenId] [nvarchar](128) COLLATE Chinese_PRC_CI_AS NULL,
	[SessionId] [nvarchar](128) COLLATE Chinese_PRC_CI_AS NULL,
	[SessionEndTime] [datetime] NULL,
	[EmailVerification] [bit] NOT NULL,
	[CellPhoneVerification] [bit] NOT NULL,
 CONSTRAINT [PK_aspnet_Members] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[aspnet_MemberGrades]    Script Date: 12/10/2014 10:04:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_MemberGrades]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[aspnet_MemberGrades](
	[GradeId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[Description] [nvarchar](500) COLLATE Chinese_PRC_CI_AS NULL,
	[Points] [int] NOT NULL,
	[IsDefault] [bit] NOT NULL,
	[Discount] [int] NOT NULL,
 CONSTRAINT [PK_aspnet_MemberGrades] PRIMARY KEY CLUSTERED 
(
	[GradeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON),
 CONSTRAINT [UQ_aspnet_MemberGrades_Points] UNIQUE NONCLUSTERED 
(
	[Points] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[aspnet_Managers]    Script Date: 12/10/2014 10:04:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Managers]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[aspnet_Managers](
	[UserId] [int] NOT NULL,
 CONSTRAINT [PK_aspnet_Managers] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[aspnet_OpenIdSettings]    Script Date: 12/10/2014 10:04:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_OpenIdSettings]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[aspnet_OpenIdSettings](
	[OpenIdType] [nvarchar](200) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[Name] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[Description] [ntext] COLLATE Chinese_PRC_CI_AS NULL,
	[Settings] [ntext] COLLATE Chinese_PRC_CI_AS NOT NULL,
 CONSTRAINT [PK_aspnet_OpenIdSettings] PRIMARY KEY CLUSTERED 
(
	[OpenIdType] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[aspnet_Roles]    Script Date: 12/10/2014 10:04:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Roles]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[aspnet_Roles](
	[RoleId] [uniqueidentifier] NOT NULL,
	[RoleName] [nvarchar](256) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[LoweredRoleName] [nvarchar](256) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[Description] [nvarchar](256) COLLATE Chinese_PRC_CI_AS NULL,
 CONSTRAINT [PK_aspnet_Roles] PRIMARY KEY NONCLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Roles]') AND name = N'aspnet_Roles_index1')
CREATE UNIQUE CLUSTERED INDEX [aspnet_Roles_index1] ON [dbo].[aspnet_Roles] 
(
	[LoweredRoleName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
GO
/****** Object:  StoredProcedure [dbo].[cp_RegionsUsers_Get]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cp_RegionsUsers_Get]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'Create PROCEDURE [dbo].[cp_RegionsUsers_Get]
(
	@PageIndex int,
	@PageSize int,
	@IsCount bit,
	@sqlPopulate ntext,
	@TotalRegionsUsers int = 0 output
)
AS
	SET Transaction Isolation Level Read UNCOMMITTED

BEGIN
	DECLARE @PageLowerBound int
	DECLARE @PageUpperBound int

	-- Set the page bounds
	SET @PageLowerBound = @PageSize * (@PageIndex-1)
	SET @PageUpperBound = @PageLowerBound + @PageSize + 1

	CREATE TABLE #PageIndexForRegionsUsers
	(
		IndexId int IDENTITY (1, 1) NOT NULL,
		RegionId int,
		RegionName char(100),
		UserCounts int,
		AllUserCounts int
	)	

	INSERT INTO #PageIndexForRegionsUsers(RegionId, RegionName, UserCounts, AllUserCounts)
	Exec sp_executesql @sqlPopulate

	SET @TotalRegionsUsers = @@rowcount
	
	SELECT RU.RegionId, RU.RegionName, RU.UserCounts, RU.AllUserCounts
	FROM   #PageIndexForRegionsUsers RU
	WHERE 
			RU.IndexId > @PageLowerBound AND
			RU.IndexId < @PageUpperBound
	ORDER BY RU.IndexId
	
	drop table #PageIndexForRegionsUsers
END' 
END
GO
/****** Object:  StoredProcedure [dbo].[cp_MemberStatistics_Get]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cp_MemberStatistics_Get]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[cp_MemberStatistics_Get]
(
	@PageIndex int,
	@PageSize int,
	@IsCount bit,
	@sqlPopulate ntext,
	@TotalProductSales int = 0 output
)
AS
	SET Transaction Isolation Level Read UNCOMMITTED

BEGIN
	DECLARE @PageLowerBound int
	DECLARE @PageUpperBound int

	-- Set the page bounds
	SET @PageLowerBound = @PageSize * (@PageIndex-1)
	SET @PageUpperBound = @PageLowerBound + @PageSize + 1

	CREATE TABLE #PageIndexForProductSales
	(
		IndexId int IDENTITY (1, 1) NOT NULL,
		UserName nvarchar(256) NOT NULL,
		UserId int,
		SaleTotals money default(0),
		OrderCount int default(0)
	)	

	INSERT INTO #PageIndexForProductSales(UserId, UserName, SaleTotals, OrderCount)
	Exec sp_executesql @sqlPopulate

	SET @TotalProductSales = @@rowcount
	
	SELECT S.IndexId,
		S.Username, ISNULL(S.SaleTotals, 0) as SaleTotals , ISNULL(S.OrderCount, 0) as OrderCount
	FROM   #PageIndexForProductSales S
	WHERE 
			S.IndexId > @PageLowerBound AND
			S.IndexId < @PageUpperBound 
	ORDER BY S.IndexId
	
	drop table #PageIndexForProductSales
END' 
END
GO
/****** Object:  Table [dbo].[Ecshop_ArticleCategories]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_ArticleCategories]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_ArticleCategories](
	[CategoryId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[DisplaySequence] [int] NOT NULL,
	[IconUrl] [nvarchar](255) COLLATE Chinese_PRC_CI_AS NULL,
	[Description] [nvarchar](1000) COLLATE Chinese_PRC_CI_AS NULL,
 CONSTRAINT [PK_Ecshop_ArticleCategories] PRIMARY KEY CLUSTERED 
(
	[CategoryId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Ecshop_AppVersionRecords]    Script Date: 12/10/2014 10:04:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_AppVersionRecords]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_AppVersionRecords](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Device] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[Version] [money] NOT NULL,
	[Description] [nvarchar](2000) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[IsForcibleUpgrade] [bit] NOT NULL,
	[UpgradeUrl] [nvarchar](200) COLLATE Chinese_PRC_CI_AS NOT NULL,
 CONSTRAINT [PK_Ecshop_AppVersionRecords] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Ecshop_AppInstallRecords]    Script Date: 12/10/2014 10:04:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_AppInstallRecords]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_AppInstallRecords](
	[VID] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[Device] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NOT NULL,
 CONSTRAINT [PK_Ecshop_AppInstallRecords] PRIMARY KEY CLUSTERED 
(
	[VID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Ecshop_ApiShorpCart]    Script Date: 12/10/2014 10:04:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_ApiShorpCart]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_ApiShorpCart](
	[APIuserId] [varchar](50) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[APIType] [varchar](10) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[APIProductId] [int] NOT NULL,
	[APISkuId] [varchar](20) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[APIQuantity] [int] NOT NULL,
	[APITime] [datetime] NOT NULL
)
END
GO
/****** Object:  Table [dbo].[Ecshop_Affiche]    Script Date: 12/10/2014 10:04:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_Affiche]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_Affiche](
	[AfficheId] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](200) COLLATE Chinese_PRC_CI_AS NULL,
	[Content] [ntext] COLLATE Chinese_PRC_CI_AS NOT NULL,
	[AddedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Ecshop_Affiche] PRIMARY KEY CLUSTERED 
(
	[AfficheId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  UserDefinedFunction [dbo].[F_SplitToString]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[F_SplitToString]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[F_SplitToString]
(
	@str nvarchar(4000), 
	@spliter nvarchar(2)
)
RETURNS @returntable TABLE (UnitString nvarchar(50))
AS
BEGIN
	WHILE(CHARINDEX(@spliter,@str)<>0)  
	BEGIN  
		INSERT INTO @returntable(UnitString) VALUES (SUBSTRING(@str,1,CHARINDEX(@spliter,@str)-1))  
		SET @str = STUFF(@str,1,CHARINDEX(@spliter,@str),'''')  
	END
	
	INSERT INTO @returntable(UnitString) VALUES (@str) 
      
	RETURN 
END' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[F_SplitToInt]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[F_SplitToInt]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[F_SplitToInt]
(
	@str nvarchar(4000), 
	@spliter nvarchar(2)
)
RETURNS @returntable TABLE (UnitInt INT)
AS
BEGIN
	WHILE(CHARINDEX(@spliter,@str)<>0)  
	BEGIN  
		INSERT INTO @returntable(UnitInt) SELECT CAST (SUBSTRING(@str,1,CHARINDEX(@spliter,@str)-1) AS INT)
		SET @str = STUFF(@str,1,CHARINDEX(@spliter,@str),'''')  
	END
	
	INSERT INTO @returntable(UnitInt) SELECT CAST (@str AS INT) 
      
	RETURN 
END' 
END
GO
/****** Object:  Table [dbo].[Ecshop_BrandCategories]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_BrandCategories]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_BrandCategories](
	[BrandId] [int] IDENTITY(1,1) NOT NULL,
	[BrandName] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[Logo] [nvarchar](255) COLLATE Chinese_PRC_CI_AS NULL,
	[CompanyUrl] [nvarchar](255) COLLATE Chinese_PRC_CI_AS NULL,
	[RewriteName] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[MetaKeywords] [nvarchar](100) COLLATE Chinese_PRC_CI_AS NULL,
	[MetaDescription] [nvarchar](100) COLLATE Chinese_PRC_CI_AS NULL,
	[Description] [ntext] COLLATE Chinese_PRC_CI_AS NULL,
	[DisplaySequence] [int] NOT NULL,
	[Theme] [nvarchar](100) COLLATE Chinese_PRC_CI_AS NULL,
 CONSTRAINT [PK_Ecshop_BrandCategories] PRIMARY KEY CLUSTERED 
(
	[BrandId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Ecshop_Banner]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_Banner]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_Banner](
	[BannerId] [int] IDENTITY(1,1) NOT NULL,
	[ShortDesc] [nvarchar](200) COLLATE Chinese_PRC_CI_AS NULL,
	[ImageUrl] [nvarchar](350) COLLATE Chinese_PRC_CI_AS NULL,
	[DisplaySequence] [int] NULL,
	[LocationType] [int] NULL,
	[Url] [nvarchar](350) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[Type] [int] NULL,
	[Client] [int] NOT NULL,
	[IsDisable] [bit] NOT NULL,
 CONSTRAINT [PK_Ecshop_Banner] PRIMARY KEY NONCLUSTERED 
(
	[BannerId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Ecshop_BalanceDrawRequest]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_BalanceDrawRequest]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_BalanceDrawRequest](
	[UserId] [int] NOT NULL,
	[UserName] [nvarchar](256) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[RequestTime] [datetime] NOT NULL,
	[Amount] [money] NOT NULL,
	[AccountName] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[BankName] [nvarchar](100) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[MerchantCode] [nvarchar](300) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[Remark] [nvarchar](2000) COLLATE Chinese_PRC_CI_AS NULL,
 CONSTRAINT [PK_Ecshop_BalanceDrawRequest] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Ecshop_BalanceDetails]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_BalanceDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_BalanceDetails](
	[JournalNumber] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[UserName] [nvarchar](256) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[TradeDate] [datetime] NOT NULL,
	[TradeType] [int] NOT NULL,
	[Income] [money] NULL,
	[Expenses] [money] NULL,
	[Balance] [money] NOT NULL,
	[Remark] [nvarchar](2000) COLLATE Chinese_PRC_CI_AS NULL,
	[InpourId] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
 CONSTRAINT [PK_Ecshop_BalanceDetails] PRIMARY KEY CLUSTERED 
(
	[JournalNumber] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_BalanceDetails]') AND name = N'Ecshop_BalanceDetails_Index2')
CREATE NONCLUSTERED INDEX [Ecshop_BalanceDetails_Index2] ON [dbo].[Ecshop_BalanceDetails] 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
GO
/****** Object:  StoredProcedure [dbo].[cp_Votes_IsBackup]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cp_Votes_IsBackup]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'Create Procedure [dbo].[cp_Votes_IsBackup]
(
  @VoteId INT
)
AS
BEGIN
  Update Ecshop_Votes 
   Set IsBackup = (~IsBackup)
    Where VoteId =@VoteId
    
    Update Ecshop_Votes 
   Set IsBackup = (~IsBackup)
    Where VoteId <>@VoteId AND IsBackup = 1
 END' 
END
GO
/****** Object:  StoredProcedure [dbo].[cp_Votes_Create]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cp_Votes_Create]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[cp_Votes_Create]
	(
		@VoteName NVARCHAR(100),
		@IsBackup BIT,
		@MaxCheck INT,
		@VoteId int OUTPUT
	)
AS

IF @IsBackup = 1
	BEGIN
		UPDATE Ecshop_Votes SET IsBackup = 0
	END

INSERT INTO Ecshop_Votes (VoteName, IsBackup, MaxCheck)
 VALUES (@VoteName, @IsBackup, @MaxCheck)
SET @VoteId = @@IDENTITY' 
END
GO
/****** Object:  StoredProcedure [dbo].[cp_HelpCategory_CreateUpdateDelete]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cp_HelpCategory_CreateUpdateDelete]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[cp_HelpCategory_CreateUpdateDelete]
	(
		@CategoryId INT = NULL,
		@Name NVARCHAR(100) = NULL,

		@IconUrl NVARCHAR(255) = NULL,
		@IndexChar CHAR(1) = NULL,
		@Description NVARCHAR(1000) = NULL,
		@IsShowFooter BIT = NULL,
		@Action INT,
		@Status INT OUTPUT
	)
AS	
    DECLARE @DisplaySequence INT
    DECLARE @intErrorCode INT
	-- 初始化信息
	SELECT @Status = 99, @intErrorCode = 0
	
	IF @Action = 2 -- 删除
	BEGIN 
		DELETE FROM Ecshop_HelpCategories WHERE CategoryId = @CategoryId
		IF @@ROWCOUNT = 1
			SET @Status = 0
	END

	IF @Action = 0 -- 创建
	BEGIN
	  IF (SELECT MAX(DisplaySequence) FROM Ecshop_HelpCategories) IS NULL
	      SET @DisplaySequence=1
	  ELSE
	     SET @DisplaySequence=(SELECT MAX(DisplaySequence) FROM Ecshop_HelpCategories)+1
				
		INSERT INTO Ecshop_HelpCategories ([Name], DisplaySequence, IconUrl, IndexChar, Description,IsShowFooter)
		VALUES (@Name, @DisplaySequence, @IconUrl, @IndexChar, @Description,@IsShowFooter)
		
		IF @@ROWCOUNT = 1
			SET @Status = 0
		
		RETURN
	END

	IF @Action = 1 -- 修改
	BEGIN
		SET @DisplaySequence=(SELECT DisplaySequence FROM Ecshop_HelpCategories WHERE CategoryId=@CategoryId)
			
		-- 修改分类信息
		UPDATE Ecshop_HelpCategories SET [Name] = @Name, DisplaySequence = @DisplaySequence, IconUrl = @IconUrl, IndexChar = @IndexChar, Description = @Description, IsShowFooter = @IsShowFooter
		WHERE CategoryId = @CategoryId
		
		SET @intErrorCode = @intErrorCode + @@ERROR
		
		IF @intErrorCode = 0
		BEGIN
			SET @Status = 0
		END
		
		RETURN
	END' 
END
GO
/****** Object:  StoredProcedure [dbo].[cp_Gift_CreateUpdateDelete]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cp_Gift_CreateUpdateDelete]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'Create PROCEDURE [dbo].[cp_Gift_CreateUpdateDelete]
(
@GiftId INT = NULL OUTPUT,
@Name NVARCHAR(100) = NULL,
@ShortDescription NVARCHAR(2000) = NULL,
@Unit NVARCHAR(10) = NULL,
@LongDescription NTEXT = NULL,
@Title NVARCHAR(100) = NULL,
@Meta_Description NVARCHAR(1000) = NULL,
@Meta_Keywords NVARCHAR(1000) = NULL,
@CostPrice MONEY = NULL,
@ImageUrl [nvarchar] (255) = NULL,
@ThumbnailUrl40 [nvarchar] (255) = NULL,
@ThumbnailUrl60 [nvarchar] (255) = NULL,
@ThumbnailUrl100 [nvarchar] (255) = NULL,
@ThumbnailUrl160 [nvarchar] (255) = NULL,
@ThumbnailUrl180 [nvarchar] (255) = NULL,
@ThumbnailUrl220 [nvarchar] (255) = NULL,
@ThumbnailUrl310 [nvarchar] (255) = NULL,
@ThumbnailUrl410 [nvarchar] (255) = NULL,
@MarketPrice MONEY = NULL,
@NeedPoint INT = NULL,
@Action INT,
@IsPromotion bit,
@Status INT OUTPUT
)
AS
SET @Status = 99

IF @Action = 2 -- 删除
BEGIN
DELETE FROM Ecshop_Gifts WHERE GiftId = @GiftId
IF @@ROWCOUNT = 1
SET @Status = 0
END

IF @Action = 0 -- 创建
BEGIN

INSERT INTO
Ecshop_Gifts
([Name], ShortDescription, Unit, LongDescription, Title,
Meta_Description, Meta_Keywords,
ImageUrl, ThumbnailUrl40, ThumbnailUrl60, ThumbnailUrl100, ThumbnailUrl160, ThumbnailUrl180,
ThumbnailUrl220, ThumbnailUrl310, ThumbnailUrl410,
CostPrice, MarketPrice, NeedPoint,IsPromotion)
VALUES
(@Name, @ShortDescription, @Unit, @LongDescription, @Title,
@Meta_Description, @Meta_Keywords,
@ImageUrl, @ThumbnailUrl40, @ThumbnailUrl60, @ThumbnailUrl100, @ThumbnailUrl160, @ThumbnailUrl180,
@ThumbnailUrl220, @ThumbnailUrl310, @ThumbnailUrl410,
@CostPrice, @MarketPrice, @NeedPoint,@IsPromotion)

SELECT @GiftId = @@IDENTITY

IF @@ROWCOUNT = 1
SET @Status = 0

RETURN
END

IF @Action = 1 -- 修改
BEGIN

UPDATE
Ecshop_Gifts
SET
[Name] = @Name,
ShortDescription = @ShortDescription,
Unit = @Unit,
LongDescription = @LongDescription,
Title = @Title,
Meta_Description = @Meta_Description,
Meta_Keywords = @Meta_Keywords,
ImageUrl = @ImageUrl,
ThumbnailUrl40 = @ThumbnailUrl40, ThumbnailUrl60 = @ThumbnailUrl60, ThumbnailUrl100 = @ThumbnailUrl100, ThumbnailUrl160 = @ThumbnailUrl160, ThumbnailUrl180 = @ThumbnailUrl180,
ThumbnailUrl220 = @ThumbnailUrl220, ThumbnailUrl310 = @ThumbnailUrl310, ThumbnailUrl410 = @ThumbnailUrl410,
CostPrice = @CostPrice,MarketPrice = @MarketPrice, NeedPoint = @NeedPoint,IsPromotion=@IsPromotion
WHERE GiftId = @GiftId

IF @@ROWCOUNT = 1
SET @Status = 0

RETURN
END' 
END
GO
/****** Object:  StoredProcedure [dbo].[cp_FriendlyLink_CreateUpdateDelete]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cp_FriendlyLink_CreateUpdateDelete]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[cp_FriendlyLink_CreateUpdateDelete]
	(
		@LinkId INT = NULL,
		@ImageUrl NVARCHAR(255) = NULL,
		@LinkUrl NVARCHAR(255) = NULL,
		@Title NVARCHAR(100) = NULL,
		@Visible BIT = NULL,
		@Action INT,
		@Status INT OUTPUT
	)
AS	
    DECLARE @DisplaySequence INT
    DECLARE @intErrorCode INT
	-- 初始化信息
	SELECT @Status = 99, @intErrorCode = 0
	
	IF @Action = 2 -- 删除
	BEGIN 
	
		SET XACT_ABORT ON
		
		BEGIN TRAN
		
		DELETE FROM Ecshop_FriendlyLinks WHERE LinkId = @LinkId
		SET @intErrorCode = @@ERROR
			
		IF @intErrorCode = 0
		BEGIN
			SET @Status = 0
			COMMIT TRAN
		END
		ELSE
			ROLLBACK TRAN
		
		RETURN
	END

	IF @Action = 0 -- 创建
	BEGIN
	
		--如果取最大序号为空 则直接将序号设置为1
		IF (Select Max(DisplaySequence) From Ecshop_FriendlyLinks) IS NULL
		    SET @DisplaySequence=1
		-- 如果不为空则将 将序号设置为表中现有的最大值加1
		ELSE
		    SELECT @DisplaySequence = MAX(DisplaySequence)+1 FROM Ecshop_FriendlyLinks
		
		
		INSERT INTO Ecshop_FriendlyLinks
			([ImageUrl], DisplaySequence, LinkUrl, Title, Visible)
		VALUES 
			(@ImageUrl, @DisplaySequence, @LinkUrl, @Title, @Visible)
		
		IF @@ROWCOUNT = 1
			SET @Status = 0
		
		RETURN
	END

	IF @Action = 1 -- 修改
	BEGIN
		
        SET @DisplaySequence =(SELECT DisplaySequence FROM Ecshop_FriendlyLinks WHERE LinkId=@LinkId)
		
		-- 修改分类信息
		UPDATE 
			Ecshop_FriendlyLinks 
		SET 
			[ImageUrl] = @ImageUrl,
			DisplaySequence = @DisplaySequence,
			LinkUrl = @LinkUrl,
			Title = @Title,
			Visible = @Visible

		WHERE LinkId = @LinkId
		
		SET @intErrorCode = @intErrorCode + @@ERROR
		
		IF @intErrorCode = 0
		BEGIN
			SET @Status = 0
		END
		
		RETURN
	END' 
END
GO
/****** Object:  StoredProcedure [dbo].[cp_EmailQueue_Failure]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cp_EmailQueue_Failure]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[cp_EmailQueue_Failure]
	(
	@EmailId uniqueidentifier,
	@FailureInterval int,
	@MaxNumberOfTries int
	)
AS
	SET Transaction Isolation Level Read UNCOMMITTED
	declare @NumberOfTries int
	select @NumberOfTries = NumberOfTries + 1 from Ecshop_EmailQueue where EmailId = @EmailId

	if @NumberOfTries <= @MaxNumberOfTries
	begin
		update Ecshop_EmailQueue set
			NumberOfTries = @NumberOfTries,
			NextTryTime = dateadd(minute, @NumberOfTries * @FailureInterval, getdate())
		where EmailId = @EmailId
	end
	else
	begin
		delete from Ecshop_EmailQueue where EmailId = @EmailId
	end' 
END
GO
/****** Object:  StoredProcedure [dbo].[cp_Product_Create]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cp_Product_Create]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[cp_Product_Create]
(
@CategoryId INT,
@MainCategoryPath NVARCHAR(256),
@TypeId INT = NULL,
@ProductName NVARCHAR(200),
@ProductCode [nvarchar] (50),
@ShortDescription NVARCHAR(2000) = NULL,
@Unit NVARCHAR(10) = NULL,
@Description NTEXT = NULL,
@MobbileDescription NTEXT = NULL,
@Title NVARCHAR(100) = NULL,
@Meta_Description NVARCHAR(1000) = NULL,
@Meta_Keywords NVARCHAR(1000) = NULL,
@SaleStatus INT,
@AddedDate DATETIME,
@ImageUrl1 [nvarchar] (255) = NULL,
@ImageUrl2 [nvarchar] (255) = NULL,
@ImageUrl3 [nvarchar] (255) = NULL,
@ImageUrl4 [nvarchar] (255) = NULL,
@ImageUrl5 [nvarchar] (255) = NULL,
@ThumbnailUrl40 [nvarchar] (255) = NULL,
@ThumbnailUrl60 [nvarchar] (255) = NULL,
@ThumbnailUrl100 [nvarchar] (255) = NULL,
@ThumbnailUrl160 [nvarchar] (255) = NULL,
@ThumbnailUrl180 [nvarchar] (255) = NULL,
@ThumbnailUrl220 [nvarchar] (255) = NULL,
@ThumbnailUrl310 [nvarchar] (255) = NULL,
@ThumbnailUrl410 [nvarchar] (255) = NULL,
@MarketPrice MONEY = NULL,
@BrandId [int],
@HasSKU [BIT],
@TaobaoProductId [bigint],
@IsfreeShipping [bit],
@ReferralDeduct money,
@SubMemberDeduct money,
@SubReferralDeduct money,
@ProductId INT OUTPUT
)
AS

SET @ProductId = 0

--商品的顺序号取当前的最大值加1
DECLARE @DisplaySequence INT
SELECT @DisplaySequence = MAX(DisplaySequence) + 1 FROM  Ecshop_Products
if @DisplaySequence is null
 	set @DisplaySequence = 1

INSERT INTO Ecshop_Products
(CategoryId, MainCategoryPath, TypeId, ProductName, ProductCode, ShortDescription, Unit, [Description], [MobbileDescription], SaleStatus, AddedDate, DisplaySequence,
ImageUrl1, ImageUrl2, ImageUrl3, ImageUrl4, ImageUrl5, ThumbnailUrl40, ThumbnailUrl60, ThumbnailUrl100, ThumbnailUrl160, ThumbnailUrl180,
ThumbnailUrl220, ThumbnailUrl310, ThumbnailUrl410,
MarketPrice, BrandId, HasSKU, TaobaoProductId,IsfreeShipping,Title,Meta_Description,Meta_Keywords,[ReferralDeduct],[SubMemberDeduct], [SubReferralDeduct])
Values
(@CategoryId, @MainCategoryPath, @TypeId, @ProductName, @ProductCode, @ShortDescription, @Unit,  @Description,@MobbileDescription,@SaleStatus, @AddedDate, @DisplaySequence,
@ImageUrl1, @ImageUrl2, @ImageUrl3, @ImageUrl4, @ImageUrl5, @ThumbnailUrl40, @ThumbnailUrl60, @ThumbnailUrl100, @ThumbnailUrl160, @ThumbnailUrl180,
@ThumbnailUrl220, @ThumbnailUrl310, @ThumbnailUrl410,@MarketPrice, @BrandId, @HasSKU, @TaobaoProductId,@IsfreeShipping,@Title,@Meta_Description,@Meta_Keywords,@ReferralDeduct,@SubMemberDeduct, @SubReferralDeduct)
SET @ProductId = @@IDENTITY;' 
END
GO
/****** Object:  StoredProcedure [dbo].[cp_PaymentType_CreateUpdateDelete]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cp_PaymentType_CreateUpdateDelete]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'Create PROCEDURE [dbo].[cp_PaymentType_CreateUpdateDelete]
(
@ModeId INT = NULL OUTPUT,
@Name NVARCHAR(100) = null,
@Description NVARCHAR(4000) = NULL,
@Gateway NVARCHAR(200) = NULL,
@IsUseInpour BIT = NULL,
@Charge MONEY = NULL,
@IsPercent BIT = NULL,
@Settings NTEXT = NULL,
@Action INT,
@ApplicationType INT=NULL,
@Status INT OUTPUT
)
AS

DECLARE @DisplaySequence INT

SET @Status = 99

IF @Action = 2 -- 删除
BEGIN
DELETE FROM Ecshop_PaymentTypes WHERE ModeId = @ModeId
IF @@ROWCOUNT = 1
BEGIN
SET @Status = 0
END

RETURN
END

IF @Action = 0 -- 创建
BEGIN

--如果取最大序号为空 则直接将序号设置为1
IF (Select Max(DisplaySequence) From Ecshop_PaymentTypes) IS NULL
SET @DisplaySequence=1
-- 如果不为空则将 将序号设置为表中现有的最大值加1
ELSE
SELECT @DisplaySequence = MAX(DisplaySequence)+1 FROM Ecshop_PaymentTypes

INSERT INTO
Ecshop_PaymentTypes([Name], Description, Gateway, DisplaySequence, IsUseInpour, Charge, IsPercent, Settings,ApplicationType)
VALUES
(@Name, @Description, @Gateway, @DisplaySequence, @IsUseInpour,@Charge, @IsPercent, @Settings,@ApplicationType)

SELECT @ModeId = @@IDENTITY
IF @@ROWCOUNT = 1
SET @Status = 0

RETURN
END

IF @Action = 1 -- 修改
BEGIN

UPDATE
Ecshop_PaymentTypes
SET
[Name] = @Name,
Description = @Description,
IsUseInpour = @IsUseInpour,
Charge = @Charge,
IsPercent = @IsPercent,
Settings = @Settings,
ApplicationType=@ApplicationType
WHERE ModeId = @ModeId
IF @@ROWCOUNT = 1
SET @Status = 0

RETURN
END' 
END
GO
/****** Object:  StoredProcedure [dbo].[cp_OrderStatisticsNoPage_Get]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cp_OrderStatisticsNoPage_Get]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[cp_OrderStatisticsNoPage_Get]
(
	@sqlPopulate ntext,
	@TotalUserOrders int = 0 output
)
AS
	SET Transaction Isolation Level Read UNCOMMITTED

BEGIN

	CREATE TABLE #PageIndexForUserOrders
	(
		IndexId int IDENTITY (1, 1) NOT NULL,
		OrderId nvarchar(50)
	)	

	INSERT INTO #PageIndexForUserOrders(OrderId)
	Exec sp_executesql @sqlPopulate

	SET @TotalUserOrders = @@rowcount
	
	SELECT O.OrderId, OrderDate, Isnull(OrderTotal,0) as Total, Username, ShipTo, Isnull(OrderProfit,0) As Profits
	FROM Ecshop_Orders O, #PageIndexForUserOrders UO 
	WHERE 
			O.OrderId = UO.OrderId
	ORDER BY UO.IndexId 
    ------------------------------------------------------------
    -- 当次搜索结果,总金额,利润
    SELECT 
		Isnull(SUM(OrderTotal),0) AS OrderTotal, -- 总金额
        Isnull(SUM(OrderProfit),0) AS Profits --利润
	FROM Ecshop_Orders o,#PageIndexForUserOrders
    where
       o.OrderId = #PageIndexForUserOrders.OrderId
   drop table #PageIndexForUserOrders
END' 
END
GO
/****** Object:  StoredProcedure [dbo].[cp_OrderStatistics_Get]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cp_OrderStatistics_Get]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'Create PROCEDURE [dbo].[cp_OrderStatistics_Get]
(
	@PageIndex int,
	@PageSize int,
	@IsCount bit,
	@sqlPopulate ntext,
	@TotalUserOrders int = 0 output
)
AS
	SET Transaction Isolation Level Read UNCOMMITTED

BEGIN
	DECLARE @PageLowerBound int
	DECLARE @PageUpperBound int

	-- Set the page bounds
	SET @PageLowerBound = @PageSize * (@PageIndex-1)
	SET @PageUpperBound = @PageLowerBound + @PageSize + 1

	CREATE TABLE #PageIndexForUserOrders
	(
		IndexId int IDENTITY (1, 1) NOT NULL,
		OrderId nvarchar(50)
	)	

	INSERT INTO #PageIndexForUserOrders(OrderId)
	Exec sp_executesql @sqlPopulate

	SET @TotalUserOrders = @@rowcount
	
	SELECT O.OrderId, OrderDate, Isnull(OrderTotal,0) as Total, Username, ShipTo,Isnull(OrderProfit,0) As Profits
	FROM Ecshop_Orders O, #PageIndexForUserOrders UO 
	WHERE 
			O.OrderId = UO.OrderId AND
			UO.IndexId > @PageLowerBound AND
			UO.IndexId < @PageUpperBound
	ORDER BY UO.IndexId 
    ------------------------------------------------------------
    -- ��ҳ�������,�ܽ��,����
    SELECT 
		Isnull(SUM(OrderTotal),0) AS OrderTotal, -- �ܽ��
        Isnull(SUM(OrderProfit),0) AS Profits --����
	FROM Ecshop_Orders o,#PageIndexForUserOrders
    where
       o.OrderId = #PageIndexForUserOrders.OrderId and
       #PageIndexForUserOrders.IndexId > @PageLowerBound and  
	   #PageIndexForUserOrders.IndexId < @PageUpperBound
   
    -- �����������,�ܽ��,����
    SELECT 
		Isnull(SUM(OrderTotal),0) AS OrderTotal, -- �ܽ��
        Isnull(SUM(OrderProfit),0) AS Profits --����
	FROM Ecshop_Orders o,#PageIndexForUserOrders
    where
       o.OrderId = #PageIndexForUserOrders.OrderId  
      
    drop table #PageIndexForUserOrders
END' 
END
GO
/****** Object:  StoredProcedure [dbo].[cp_Orders_Get]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cp_Orders_Get]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'Create PROCEDURE [dbo].[cp_Orders_Get]
(
	@PageIndex int,
	@PageSize int,
	@IsCount bit,
	@sqlPopulate ntext,
	@TotalOrders int = 0 output
)
AS
	SET Transaction Isolation Level Read UNCOMMITTED

BEGIN
	DECLARE @PageLowerBound int
	DECLARE @PageUpperBound int

	-- Set the page bounds
	SET @PageLowerBound = @PageSize * (@PageIndex-1)
	SET @PageUpperBound = @PageLowerBound + @PageSize + 1

	CREATE TABLE #PageIndexForOrders
	(
		IndexId int IDENTITY (1, 1) NOT NULL,
		OrderId nvarchar(50)
	)	

	INSERT INTO #PageIndexForOrders(OrderId)
	Exec sp_executesql @sqlPopulate

	SET @TotalOrders = @@rowcount
	
	SELECT o.OrderId, OrderDate, UserId, Username, Wangwang, RealName, ShipTo, OrderTotal,ISNULL(GroupBuyId,0) as GroupBuyId,ISNULL(GroupBuyStatus,0) as GroupBuyStatus, PaymentType,Gateway,ManagerMark, OrderStatus, RefundStatus,ManagerRemark,ISNULL(IsPrinted,0) IsPrinted, ShipOrderNumber,SourceOrder
	FROM Ecshop_Orders o, #PageIndexForOrders
	WHERE 
		o.OrderId = #PageIndexForOrders.OrderId AND
		#PageIndexForOrders.IndexId > @PageLowerBound AND
		#PageIndexForOrders.IndexId < @PageUpperBound 
	ORDER BY #PageIndexForOrders.IndexId

	drop table #PageIndexForOrders
END' 
END
GO
/****** Object:  StoredProcedure [dbo].[cp_Menu_SwapDisplaySequence]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cp_Menu_SwapDisplaySequence]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[cp_Menu_SwapDisplaySequence]
	(
		@MenuId INT,
		@ZIndex INT
	)
AS
	
	DECLARE @ParentMenuId INT, @DisplaySequence INT
	DECLARE @swap_MenuId INT, @swap_DisplaySequence INT
	
	SELECT @ParentMenuId = ParentMenuId, @DisplaySequence = DisplaySequence FROM vshop_Menu WHERE MenuId = @MenuId

	IF @ZIndex = 0
		SELECT TOP 1 @swap_MenuId = MenuId, @swap_DisplaySequence = DisplaySequence FROM vshop_Menu WHERE ParentMenuId = @ParentMenuId AND DisplaySequence < @DisplaySequence ORDER BY DisplaySequence DESC
	ELSE
		SELECT TOP 1 @swap_MenuId = MenuId, @swap_DisplaySequence = DisplaySequence FROM vshop_Menu WHERE ParentMenuId = @ParentMenuId AND DisplaySequence > @DisplaySequence ORDER BY DisplaySequence ASC
	
	IF @swap_MenuId IS NULL
		RETURN;

	SET XACT_ABORT ON
	BEGIN TRAN
	
	UPDATE vshop_Menu SET DisplaySequence = @swap_DisplaySequence WHERE MenuId = @MenuId
	UPDATE vshop_Menu SET DisplaySequence = @DisplaySequence WHERE MenuId = @swap_MenuId
	
	COMMIT TRAN' 
END
GO
/****** Object:  StoredProcedure [dbo].[cp_ProductVisitAndBuyStatistics_Get]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cp_ProductVisitAndBuyStatistics_Get]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'Create PROCEDURE [dbo].[cp_ProductVisitAndBuyStatistics_Get]
(
	@PageIndex int,
	@PageSize int,
	@sqlPopulate ntext,
	@TotalProductSales int = 0 output
)
AS
	SET Transaction Isolation Level Read UNCOMMITTED

BEGIN
	DECLARE @PageLowerBound int
	DECLARE @PageUpperBound int

	-- Set the page bounds
	SET @PageLowerBound = @PageSize * (@PageIndex-1)
	SET @PageUpperBound = @PageLowerBound + @PageSize + 1

	CREATE TABLE #PageIndexForProductSales
	(
		IndexId int IDENTITY (1, 1) NOT NULL,
		ProductId int,
		BuyPercentage decimal(18, 0)		
	)	

	INSERT INTO #PageIndexForProductSales(ProductId,BuyPercentage)
	Exec sp_executesql @sqlPopulate

	SET @TotalProductSales = @@rowcount
	
	SELECT S.IndexId,P.ProductName,P.VistiCounts,P.SaleCounts as BuyCount ,S.BuyPercentage    
	FROM   Ecshop_Products P, #PageIndexForProductSales S
	WHERE 
			P.ProductId = S.ProductId AND
			S.IndexId > @PageLowerBound AND
			S.IndexId < @PageUpperBound 
	ORDER BY S.IndexId
	
	drop table #PageIndexForProductSales
END' 
END
GO
/****** Object:  StoredProcedure [dbo].[cp_ProductSalesNoPage_Get]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cp_ProductSalesNoPage_Get]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[cp_ProductSalesNoPage_Get]
(
	@sqlPopulate ntext,
	@TotalProductSales int = 0 output
)
AS
	SET Transaction Isolation Level Read UNCOMMITTED

BEGIN
	CREATE TABLE #PageIndexForProductSales
	(
		IndexId int IDENTITY (1, 1) NOT NULL,
		ProductId int,
		ProductSaleCounts int,
		ProductSaleTotals money,
		ProductProfitsTotals money
	)	

	INSERT INTO #PageIndexForProductSales(ProductId, ProductSaleCounts, ProductSaleTotals, ProductProfitsTotals)
	Exec sp_executesql @sqlPopulate

	SET @TotalProductSales = @@rowcount
	
	SELECT IDOfSaleTotals=(select count(1)+1 from #PageIndexForProductSales where ProductSaleCounts>s.ProductSaleCounts),
		P.ProductName,P.ProductCode as SKU,
		case when S.ProductSaleCounts is null then 0 else S.ProductSaleCounts end as ProductSaleCounts , 
		case when S.ProductSaleTotals is null then 0 else S.ProductSaleTotals end as ProductSaleTotals , 
		case when S.ProductProfitsTotals is null then 0 else S.ProductProfitsTotals end as ProductProfitsTotals
	FROM   Ecshop_Products P, #PageIndexForProductSales S
	WHERE 
			P.ProductId = S.ProductId
	ORDER BY S.IndexId
END' 
END
GO
/****** Object:  StoredProcedure [dbo].[cp_ProductSales_Get]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cp_ProductSales_Get]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'Create PROCEDURE [dbo].[cp_ProductSales_Get]
(
	@PageIndex int,
	@PageSize int,
	@IsCount bit,
	@sqlPopulate ntext,
	@TotalProductSales int = 0 output
)
AS
	SET Transaction Isolation Level Read UNCOMMITTED

BEGIN
	DECLARE @PageLowerBound int
	DECLARE @PageUpperBound int

	-- Set the page bounds
	SET @PageLowerBound = @PageSize * (@PageIndex-1)
	SET @PageUpperBound = @PageLowerBound + @PageSize + 1

	CREATE TABLE #PageIndexForProductSales
	(
		IndexId int IDENTITY (1, 1) NOT NULL,
		ProductId int,
		ProductSaleCounts int,
		ProductSaleTotals money,
		ProductProfitsTotals money
	)	

	INSERT INTO #PageIndexForProductSales(ProductId, ProductSaleCounts, ProductSaleTotals, ProductProfitsTotals)
	Exec sp_executesql @sqlPopulate

	SET @TotalProductSales = @@rowcount
	
	SELECT IDOfSaleTotals=(select count(1)+1 from #PageIndexForProductSales where ProductSaleCounts>s.ProductSaleCounts),
		P.ProductName,P.ProductCode as SKU,
		case when S.ProductSaleCounts is null then 0 else S.ProductSaleCounts end as ProductSaleCounts , 
		case when S.ProductSaleTotals is null then 0 else S.ProductSaleTotals end as ProductSaleTotals , 
		case when S.ProductProfitsTotals is null then 0 else S.ProductProfitsTotals end as ProductProfitsTotals
	FROM   Ecshop_Products P, #PageIndexForProductSales S
	WHERE 
			P.ProductId = S.ProductId AND
			S.IndexId > @PageLowerBound AND
			S.IndexId < @PageUpperBound 
	ORDER BY S.IndexId
	
	drop table #PageIndexForProductSales
END' 
END
GO
/****** Object:  Table [dbo].[aspnet_UsersInRoles]    Script Date: 12/10/2014 10:04:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_UsersInRoles]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[aspnet_UsersInRoles](
	[UserId] [int] NOT NULL,
	[RoleId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_[aspnet_UsersInRoles] PRIMARY KEY NONCLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_UsersInRoles]') AND name = N'aspnet_UsersInRoles_index')
CREATE NONCLUSTERED INDEX [aspnet_UsersInRoles_index] ON [dbo].[aspnet_UsersInRoles] 
(
	[RoleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
GO
/****** Object:  StoredProcedure [dbo].[aspnet_OpenIdSettings_Save]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_OpenIdSettings_Save]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[aspnet_OpenIdSettings_Save]
	@OpenIdType nvarchar(200),
	@Name nvarchar(50),
	@Description ntext,
	@Settings ntext
AS
	IF (SELECT COUNT(OpenIdType) FROM aspnet_OpenIdSettings WHERE LOWER(OpenIdType)=LOWER(@OpenIdType))>0
	BEGIN
		UPDATE aspnet_OpenIdSettings
		SET [Name] = @Name,
				[Description] = @Description,
				[Settings] = @Settings
		WHERE LOWER(OpenIdType)=LOWER(@OpenIdType)
	END
	
	ELSE
	BEGIN
		INSERT INTO aspnet_OpenIdSettings ([OpenIdType], [Name], [Description], [Settings])
		VALUES (LOWER(@OpenIdType) ,@Name, @Description, @Settings)
	END' 
END
GO
/****** Object:  StoredProcedure [dbo].[cp_ArticleCategory_CreateUpdateDelete]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cp_ArticleCategory_CreateUpdateDelete]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[cp_ArticleCategory_CreateUpdateDelete]
	(
		@CategoryId INT = NULL,
		@Name NVARCHAR(100) = NULL,
		@DisplaySequence INT = NULL,
		@IconUrl NVARCHAR(255) = NULL,
		@Description NVARCHAR(1000) = NULL,
		@Action INT,
		@Status INT OUTPUT
	)
AS
	
	-- 初始化信息
	SELECT @Status = 99
	
	IF @Action = 2 -- 删除
	BEGIN -- 同时删除分类下的文章

		DELETE FROM Ecshop_ArticleCategories WHERE CategoryId = @CategoryId
		
		IF @@ROWCOUNT >= 1
			SET @Status = 0
		
		RETURN
	END

	IF @Action = 0 -- 创建
	BEGIN
		
		 IF (SELECT MAX(DisplaySequence) FROM Ecshop_ArticleCategories) IS NULL
	      SET @DisplaySequence=1
	  ELSE
	     SET @DisplaySequence=(SELECT MAX(DisplaySequence) FROM Ecshop_ArticleCategories)+1
		
		INSERT INTO Ecshop_ArticleCategories
			([Name], DisplaySequence, IconUrl, Description)
		VALUES 
			(@Name, @DisplaySequence, @IconUrl, @Description)
		
		IF @@ROWCOUNT = 1
			SET @Status = 0
		
		RETURN
	END

	IF @Action = 1 -- 修改
	BEGIN
		
		SET @DisplaySequence=(SELECT DisplaySequence FROM Ecshop_ArticleCategories WHERE CategoryId=@CategoryId)
		
		-- 修改分类信息
		UPDATE 
			Ecshop_ArticleCategories 
		SET 
			[Name] = @Name,
			DisplaySequence = @DisplaySequence,
			IconUrl = @IconUrl,
			Description = @Description
		WHERE CategoryId = @CategoryId
		
		IF @@ROWCOUNT >= 1
			SET @Status = 0
		
		RETURN
	END' 
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_CreateUser]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Membership_CreateUser]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[aspnet_Membership_CreateUser]
    @UserName                               nvarchar(256),
    @Password                               nvarchar(128),
    @PasswordSalt                           nvarchar(128),
    @Email                                  nvarchar(256),
    @PasswordQuestion                       nvarchar(256),
    @PasswordAnswer                         nvarchar(128),
    @IsApproved                             bit,
    @CurrentTime                         datetime,
    @CreateDate                             datetime = NULL,
    @UniqueEmail                            int      = 0,
    @PasswordFormat                         int      = 0,
    @UserId                                 int OUTPUT
AS
BEGIN

    DECLARE @NewUserId int
    SELECT @NewUserId = NULL

    DECLARE @IsLockedOut bit
    SET @IsLockedOut = 0

    DECLARE @LastLockoutDate  datetime
    SET @LastLockoutDate = CONVERT( datetime, ''17540101'', 112 )

    DECLARE @FailedPasswordAttemptCount int
    SET @FailedPasswordAttemptCount = 0

    DECLARE @FailedPasswordAttemptWindowStart  datetime
    SET @FailedPasswordAttemptWindowStart = CONVERT( datetime, ''17540101'', 112 )

    DECLARE @FailedPasswordAnswerAttemptCount int
    SET @FailedPasswordAnswerAttemptCount = 0

    DECLARE @FailedPasswordAnswerAttemptWindowStart  datetime
    SET @FailedPasswordAnswerAttemptWindowStart = CONVERT( datetime, ''17540101'', 112 )

    DECLARE @NewUserCreated bit
    DECLARE @ReturnValue   int
    SET @ReturnValue = 0

    DECLARE @ErrorCode     int
    SET @ErrorCode = 0

    DECLARE @TranStarted   bit
    SET @TranStarted = 0

    IF( @@TRANCOUNT = 0 )
    BEGIN
	    BEGIN TRANSACTION
	    SET @TranStarted = 1
    END
    ELSE
    	SET @TranStarted = 0

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END
    
    IF (@UniqueEmail = 1)
    BEGIN
        IF (EXISTS (SELECT *
                    FROM  dbo.aspnet_Users WITH ( UPDLOCK, HOLDLOCK )
                    WHERE LoweredEmail = LOWER(@Email)))
        BEGIN
            SET @ErrorCode = 7
            GOTO Cleanup
        END
    END

    SET @CreateDate = @CurrentTime

    SELECT  @NewUserId = UserId FROM dbo.aspnet_Users WHERE LOWER(@UserName) = LoweredUserName
    IF ( @NewUserId IS NULL )
    BEGIN
        SET @NewUserId = @UserId

        INSERT INTO dbo.aspnet_Users
                (
                LoweredUserName,
                UserName,
                IsAnonymous,
                LastActivityDate,
                  Password,
                  PasswordSalt,
                  Email,
                  LoweredEmail,
                  PasswordQuestion,
                  PasswordAnswer,
                  PasswordFormat,
                  IsApproved,
                  IsLockedOut,
                  CreateDate,
                  LastLoginDate,
                  LastPasswordChangedDate,
                  LastLockoutDate,
                  FailedPasswordAttemptCount,
                  FailedPasswordAttemptWindowStart,
                  FailedPasswordAnswerAttemptCount,
                  FailedPasswordAnswerAttemptWindowStart )
         VALUES (
				LOWER(@UserName),
				@UserName,
				0,
				@CreateDate,
                  @Password,
                  @PasswordSalt,
                  @Email,
                  LOWER(@Email),
                  @PasswordQuestion,
                  @PasswordAnswer,
                  @PasswordFormat,
                  @IsApproved,
                  @IsLockedOut,
                  @CreateDate,
                  @CreateDate,
                  @CreateDate,
                  @LastLockoutDate,
                  @FailedPasswordAttemptCount,
                  @FailedPasswordAttemptWindowStart,
                  @FailedPasswordAnswerAttemptCount,
                  @FailedPasswordAnswerAttemptWindowStart )
                  
        SELECT @NewUserId = @@IDENTITY, @NewUserCreated = 1, @ReturnValue = 0
    END
    ELSE
    BEGIN
        SET @NewUserCreated = 0

        SET @ErrorCode = 6
        GOTO Cleanup
    END

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    IF( @ReturnValue = -1 )
    BEGIN
        SET @ErrorCode = 10
        GOTO Cleanup
    END

    SET @UserId = @NewUserId

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    IF( @TranStarted = 1 )
    BEGIN
	    SET @TranStarted = 0
	    COMMIT TRANSACTION
    END

    RETURN 0

Cleanup:

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
    	ROLLBACK TRANSACTION
    END

    RETURN @ErrorCode

END' 
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_ChangePasswordQuestionAndAnswer]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Membership_ChangePasswordQuestionAndAnswer]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[aspnet_Membership_ChangePasswordQuestionAndAnswer]
    @UserName              nvarchar(256),
    @NewPasswordQuestion   nvarchar(256),
    @NewPasswordAnswer     nvarchar(128)
AS
BEGIN

    UPDATE dbo.aspnet_Users
    SET    PasswordQuestion = @NewPasswordQuestion, PasswordAnswer = @NewPasswordAnswer
    WHERE  LoweredUserName = LOWER(@UserName)
    RETURN(0)
END' 
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Roles_CreateRole]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Roles_CreateRole]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[aspnet_Roles_CreateRole]
    @RoleName         nvarchar(256)
AS
BEGIN

    DECLARE @ErrorCode     int
    SET @ErrorCode = 0

    DECLARE @TranStarted   bit
    SET @TranStarted = 0

    IF( @@TRANCOUNT = 0 )
    BEGIN
        BEGIN TRANSACTION
        SET @TranStarted = 1
    END
    ELSE
        SET @TranStarted = 0

    IF (EXISTS(SELECT RoleId FROM dbo.aspnet_Roles WHERE LoweredRoleName = LOWER(@RoleName)))
    BEGIN
        SET @ErrorCode = 1
        GOTO Cleanup
    END

    INSERT INTO dbo.aspnet_Roles
                (RoleName, LoweredRoleName)
         VALUES (@RoleName, LOWER(@RoleName))

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
        COMMIT TRANSACTION
    END

    RETURN(0)

Cleanup:

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
        ROLLBACK TRANSACTION
    END

    RETURN @ErrorCode

END' 
END
GO
/****** Object:  StoredProcedure [dbo].[cp_Category_Delete]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cp_Category_Delete]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[cp_Category_Delete]
(
	@CategoryId INT
)
AS
Declare @Err As int, @Count INT, @DisplaySequence INT
DECLARE @Path nvarchar(4000)

SET XACT_ABORT ON
Begin Tran

CREATE TABLE #tempTable (CategoryId INT)

SELECT @Err = 0, @Path = Path, @DisplaySequence = DisplaySequence FROM Ecshop_Categories WHERE CategoryId = @CategoryId
INSERT INTO #tempTable SELECT CategoryId FROM Ecshop_Categories WHERE CategoryId = @CategoryId OR Path LIKE '''' + @Path + ''|%''

SET @Count = @@ROWCOUNT

-- 删除自身和所有子分类
DELETE From Ecshop_Categories Where CategoryId IN (SELECT CategoryId FROM #tempTable)

-- 修改商品分类的编号为0，表示未分类
UPDATE Ecshop_Products SET CategoryId = 0, MainCategoryPath = null WHERE CategoryId IN (SELECT CategoryId FROM #tempTable)


DROP TABLE #tempTable

  Commit Tran' 
END
GO
/****** Object:  StoredProcedure [dbo].[cp_Category_Create]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cp_Category_Create]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[cp_Category_Create]
	(
		@Name NVARCHAR(100) = NULL,
		@DisplaySequence INT = NULL,
		@Meta_Title NVARCHAR(1000)=NULL,
		@Meta_Description NVARCHAR(1000) = NULL,
		@Meta_Keywords NVARCHAR(1000) = NULL,
		@ParentCategoryId INT = NULL,
		@RewriteName NVARCHAR(50) = NULL,
		@SKUPrefix NVARCHAR(10) = NULL,
		@AssociatedProductType INT = NULL,
		@Notes1 NTEXT = NULL,
		@Notes2 NTEXT = NULL,
		@Notes3 NTEXT = NULL,
		@Notes4 NTEXT = NULL,
		@Notes5 NTEXT = NULL,
		@CategoryId INT OUTPUT,
		@Icon NVARCHAR(1000) = NULL
	)
AS
Declare @Err As int
SELECT @Err=0

SET XACT_ABORT ON
Begin Tran
	
IF @ParentCategoryId IS NULL OR @ParentCategoryId < 0
	SET @ParentCategoryId = 0
	
--通过现有记录获取栏目ID


Select @CategoryId = ISNULL(Max(CategoryId),0) From Ecshop_Categories
IF @CategoryId Is Not Null
	Set @CategoryId = @CategoryId+1
Else
	Set @CategoryId = 1

--判断是否是顶级栏目，设置其Path和Depth
Declare @Depth As int
Declare @Path As nvarchar(4000)

IF @ParentCategoryId = 0
Begin
	Select @DisplaySequence = ISNULL(MAX(DisplaySequence),0) + 1 from Ecshop_Categories where ParentCategoryId = 0
	Set @Path =Ltrim(RTRIM(Str(@CategoryId)))
	Set @Depth = 1
End
Else
Begin
	--获取父节点的路径和深度
	Select @Path = [Path] ,@Depth = Depth From Ecshop_Categories Where CategoryId=@ParentCategoryId
	Select @DisplaySequence = ISNULL(MAX(DisplaySequence),0) + 1 from Ecshop_Categories where ParentCategoryId = @ParentCategoryId
	IF @Path Is Null
	Begin
		Set @Err = 1
		Goto theEnd
	End
	
	Set @Path = @Path + ''|'' + Ltrim(RTRIM(Str(@CategoryId)))
	Set @Depth = @Depth+1
End

Insert Into Ecshop_Categories(
	CategoryId, [Name], DisplaySequence,Meta_Title, Meta_Description, Meta_Keywords, SKUPrefix,AssociatedProductType,
	ParentCategoryId, Depth, Path, RewriteName, Notes1, Notes2, Notes3, Notes4, Notes5,Icon
) 
Values(
	@CategoryId, @Name, @DisplaySequence,@Meta_Title, @Meta_Description, @Meta_Keywords, @SKUPrefix,@AssociatedProductType,
	@ParentCategoryId, @Depth, @Path, @RewriteName, @Notes1, @Notes2, @Notes3, @Notes4, @Notes5,@Icon
)

IF @@Error<>0 
Begin
	Set @Err=1
	Goto theEnd
End

theEnd:
IF @Err=0
Begin
	Commit Tran
	Return @CategoryId
End
Else
Begin
    Rollback Tran
	Return 0
End' 
END
GO
/****** Object:  StoredProcedure [dbo].[cp_BrandCategory_DisplaySequence]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cp_BrandCategory_DisplaySequence]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[cp_BrandCategory_DisplaySequence]
(
  @Sort INT , -- 是升 还是降 1 表示升 2表示降
  @BrandId INT
)
As 
    --  当前要修改的序号
	 DECLARE @oldSequence INT
	 -- 要修改成的序号
	 DECLARE @newSequence INT
	 -- 和当前对换的编号
	 DECLARE @newServiceId INT
 --升
 IF @Sort =1
    
 BEGIN
	
	 SELECT @oldSequence = DisplaySequence FROM [Ecshop_BrandCategories]
     WHERE BrandId =@BrandId
 
     SELECT @newSequence = DisplaySequence FROM [Ecshop_BrandCategories]
     WHERE BrandId =
		(SELECT TOP 1 BrandId FROM [Ecshop_BrandCategories]
		WHERE DisplaySequence < @oldSequence
		Order by DisplaySequence DESC)

     SELECT @newServiceId = (SELECT TOP 1 BrandId FROM [Ecshop_BrandCategories]
     WHERE DisplaySequence < @oldSequence
     ORDER BY DisplaySequence DESC)
    
    IF @newServiceId IS NOT NULL
     BEGIN
		  Update [Ecshop_BrandCategories] SET DisplaySequence =@newSequence 
		  WHERE BrandId = @BrandId
     END
    IF @newServiceId IS NOT NULL
     BEGIN
		  Update [Ecshop_BrandCategories] SET DisplaySequence = @oldSequence
		  WHERE BrandId = @newServiceId 
     END
  END 
 --降低
 IF @Sort =0
 BEGIN
	SELECT @oldSequence = DisplaySequence FROM [Ecshop_BrandCategories]
	WHERE BrandId =@BrandId 
	 
	SELECT @newSequence = DisplaySequence FROM [Ecshop_BrandCategories]
	WHERE BrandId =
		 (SELECT TOP 1 BrandId FROM [Ecshop_BrandCategories]
		 WHERE DisplaySequence>@oldSequence 
		 Order by DisplaySequence ASC) 
	 
	SELECT @newServiceId = (SELECT TOP 1 BrandId FROM [Ecshop_BrandCategories]
	WHERE DisplaySequence > @oldSequence 
	Order by DisplaySequence ASC)
    
    IF @newSequence IS NOT NULL
     BEGIN
		  UPDATE [Ecshop_BrandCategories] SET DisplaySequence =@newSequence 
		  WHERE BrandId = @BrandId 
     END
    IF @newServiceId IS NOT NULL
     BEGIN
		  UPDATE [Ecshop_BrandCategories] Set DisplaySequence = @oldSequence
		  WHERE BrandId =@newServiceId 
     END
 END' 
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_OpenId_Bind]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_OpenId_Bind]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[aspnet_OpenId_Bind]
	@UserName nvarchar(256),
	@OpenId nvarchar(128),
	@OpenIdType nvarchar(200)
AS
	-- 检查当前用户是否已经绑定了信任登录用户
	IF (SELECT OpenId FROM aspnet_Users WHERE LoweredUserName=LOWER(@UserName)) IS NOT NULL
		RETURN

	-- 检查当前信任登录用户是否已经绑定了其他用户
	IF (SELECT COUNT(UserId) FROM aspnet_Users WHERE LOWER(OpenId)=LOWER(@OpenId) AND LOWER(OpenIdType)=LOWER(@OpenIdType))>0
		RETURN

	UPDATE aspnet_Users
	SET 
			OpenId = @OpenId,
			OpenIdType = @OpenIdType
	WHERE 
			LoweredUserName=LOWER(@UserName)' 
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_UpdateUserInfo]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Membership_UpdateUserInfo]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[aspnet_Membership_UpdateUserInfo]
    @UserName                       nvarchar(256),
    @IsPasswordCorrect              bit,
    @UpdateLastLoginActivityDate    bit,
    @MaxInvalidPasswordAttempts     int,
    @PasswordAttemptWindow          int,
    @CurrentTime                 datetime,
    @LastLoginDate                  datetime,
    @LastActivityDate               datetime
AS
BEGIN
    DECLARE @UserId                                 int
    DECLARE @IsApproved                             bit
    --DECLARE @IsLockedOut                            bit
    --DECLARE @LastLockoutDate                        datetime
    DECLARE @FailedPasswordAttemptCount             int
    DECLARE @FailedPasswordAttemptWindowStart       datetime
    DECLARE @FailedPasswordAnswerAttemptCount       int
    DECLARE @FailedPasswordAnswerAttemptWindowStart datetime

    DECLARE @ErrorCode     int
    SET @ErrorCode = 0

    DECLARE @TranStarted   bit
    SET @TranStarted = 0

    IF( @@TRANCOUNT = 0 )
    BEGIN
	    BEGIN TRANSACTION
	    SET @TranStarted = 1
    END
    ELSE
    	SET @TranStarted = 0

    SELECT  @UserId = UserId,
            @IsApproved = IsApproved,
            --@IsLockedOut = IsLockedOut,
            --@LastLockoutDate = LastLockoutDate,
            @FailedPasswordAttemptCount = FailedPasswordAttemptCount,
            @FailedPasswordAttemptWindowStart = FailedPasswordAttemptWindowStart,
            @FailedPasswordAnswerAttemptCount = FailedPasswordAnswerAttemptCount,
            @FailedPasswordAnswerAttemptWindowStart = FailedPasswordAnswerAttemptWindowStart
    FROM    dbo.aspnet_Users
    WHERE   LOWER(@UserName) = LoweredUserName

    IF ( @@rowcount = 0 )
    BEGIN
        SET @ErrorCode = 1
        GOTO Cleanup
    END

    --IF( @IsLockedOut = 1 )
    --BEGIN
        --GOTO Cleanup
    --END

    IF( @IsPasswordCorrect = 0 )
    BEGIN
        IF( @CurrentTime > DATEADD( minute, @PasswordAttemptWindow, @FailedPasswordAttemptWindowStart ) )
        BEGIN
            SET @FailedPasswordAttemptWindowStart = @CurrentTime
            SET @FailedPasswordAttemptCount = 1
        END
        ELSE
        BEGIN
            SET @FailedPasswordAttemptWindowStart = @CurrentTime
            SET @FailedPasswordAttemptCount = @FailedPasswordAttemptCount + 1
        END

        --BEGIN
            --IF( @FailedPasswordAttemptCount >= @MaxInvalidPasswordAttempts )
            --BEGIN
                --SET @IsLockedOut = 1
                --SET @LastLockoutDate = @CurrentTime
            --END
        --END
    END
    ELSE
    BEGIN
        IF( @FailedPasswordAttemptCount > 0 OR @FailedPasswordAnswerAttemptCount > 0 )
        BEGIN
            SET @FailedPasswordAttemptCount = 0
            SET @FailedPasswordAttemptWindowStart = CONVERT( datetime, ''17540101'', 112 )
            SET @FailedPasswordAnswerAttemptCount = 0
            SET @FailedPasswordAnswerAttemptWindowStart = CONVERT( datetime, ''17540101'', 112 )
            --SET @LastLockoutDate = CONVERT( datetime, ''17540101'', 112 )
        END
    END

    IF( @UpdateLastLoginActivityDate = 1 )
    BEGIN
        UPDATE  dbo.aspnet_Users
        SET     LastActivityDate = @LastActivityDate
        WHERE   @UserId = UserId

        IF( @@ERROR <> 0 )
        BEGIN
            SET @ErrorCode = -1
            GOTO Cleanup
        END

        UPDATE  dbo.aspnet_Users
        SET     LastLoginDate = @LastLoginDate
        WHERE   UserId = @UserId

        IF( @@ERROR <> 0 )
        BEGIN
            SET @ErrorCode = -1
            GOTO Cleanup
        END
    END


    --UPDATE dbo.aspnet_Users
    --SET IsLockedOut = @IsLockedOut, LastLockoutDate = @LastLockoutDate,
        --FailedPasswordAttemptCount = @FailedPasswordAttemptCount,
        --FailedPasswordAttemptWindowStart = @FailedPasswordAttemptWindowStart,
        --FailedPasswordAnswerAttemptCount = @FailedPasswordAnswerAttemptCount,
        --FailedPasswordAnswerAttemptWindowStart = @FailedPasswordAnswerAttemptWindowStart
    --WHERE @UserId = UserId
    
    UPDATE dbo.aspnet_Users
    SET 
        FailedPasswordAttemptCount = @FailedPasswordAttemptCount,
        FailedPasswordAttemptWindowStart = @FailedPasswordAttemptWindowStart,
        FailedPasswordAnswerAttemptCount = @FailedPasswordAnswerAttemptCount,
        FailedPasswordAnswerAttemptWindowStart = @FailedPasswordAnswerAttemptWindowStart
    WHERE @UserId = UserId

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    IF( @TranStarted = 1 )
    BEGIN
	SET @TranStarted = 0
	COMMIT TRANSACTION
    END

    RETURN @ErrorCode

Cleanup:

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
    	ROLLBACK TRANSACTION
    END

    RETURN @ErrorCode

END' 
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_UpdateUser]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Membership_UpdateUser]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[aspnet_Membership_UpdateUser]
    @UserName             nvarchar(256),
    @Email                nvarchar(256),
    @Comment              ntext,
    @IsApproved           bit,
    @LastLoginDate        datetime,
    @LastActivityDate     datetime,
    @UniqueEmail          int,
    @CurrentTime       datetime
AS
BEGIN

    IF (@UniqueEmail = 1)
    BEGIN
        IF (EXISTS (SELECT *
                    FROM  dbo.aspnet_Users WITH (UPDLOCK, HOLDLOCK)
                    WHERE LoweredUserName <> LOWER(@UserName) AND LoweredEmail = LOWER(@Email)))
        BEGIN
            RETURN(7)
        END
    END

    DECLARE @TranStarted   bit
    SET @TranStarted = 0

    IF( @@TRANCOUNT = 0 )
    BEGIN
	    BEGIN TRANSACTION
	    SET @TranStarted = 1
    END
    ELSE
	SET @TranStarted = 0

    UPDATE dbo.aspnet_Users WITH (ROWLOCK)
    SET
         Email            = @Email,
         LoweredEmail     = LOWER(@Email),
         Comment          = @Comment,
         IsApproved       = @IsApproved,
         LastLoginDate    = @LastLoginDate,
         LastActivityDate = @LastActivityDate
    WHERE
       LoweredUserName = LOWER(@UserName)

    IF( @@ERROR <> 0 )
        GOTO Cleanup

    IF( @TranStarted = 1 )
    BEGIN
	SET @TranStarted = 0
	COMMIT TRANSACTION
    END

    RETURN 0

Cleanup:

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
    	ROLLBACK TRANSACTION
    END

    RETURN -1
END' 
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_UnlockUser]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Membership_UnlockUser]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[aspnet_Membership_UnlockUser]
    @UserName                                nvarchar(256)
AS
BEGIN

    UPDATE dbo.aspnet_Users
    SET IsLockedOut = 0,
        FailedPasswordAttemptCount = 0,
        FailedPasswordAttemptWindowStart = CONVERT( datetime, ''17540101'', 112 ),
        FailedPasswordAnswerAttemptCount = 0,
        FailedPasswordAnswerAttemptWindowStart = CONVERT( datetime, ''17540101'', 112 ),
        LastLockoutDate = CONVERT( datetime, ''17540101'', 112 )
    WHERE LoweredUserName = LOWER(@UserName)

    RETURN 0
END' 
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_SetPassword]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Membership_SetPassword]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[aspnet_Membership_SetPassword]
    @UserName         nvarchar(256),
    @NewPassword      nvarchar(128),
    @PasswordSalt     nvarchar(128),
    @CurrentTime   datetime,
    @PasswordFormat   int = 0
AS
BEGIN

    UPDATE dbo.aspnet_Users
    SET Password = @NewPassword, PasswordFormat = @PasswordFormat, PasswordSalt = @PasswordSalt,
        LastPasswordChangedDate = @CurrentTime
    WHERE LoweredUserName = LOWER(@UserName)
    RETURN(0)
END' 
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_ResetPassword]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Membership_ResetPassword]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[aspnet_Membership_ResetPassword]
    @UserName                    nvarchar(256),
    @NewPassword                 nvarchar(128),
    @MaxInvalidPasswordAttempts  int,
    @PasswordAttemptWindow       int,
    @PasswordSalt                nvarchar(128),
    @CurrentTime              datetime,
    @PasswordFormat              int = 0,
    @PasswordAnswer              nvarchar(128) = NULL
AS
BEGIN
    DECLARE @IsLockedOut                            bit
    DECLARE @LastLockoutDate                        datetime
    DECLARE @FailedPasswordAttemptCount             int
    DECLARE @FailedPasswordAttemptWindowStart       datetime
    DECLARE @FailedPasswordAnswerAttemptCount       int
    DECLARE @FailedPasswordAnswerAttemptWindowStart datetime

    DECLARE @UserId                                 int
    SET     @UserId = NULL

    DECLARE @ErrorCode     int
    SET @ErrorCode = 0

    DECLARE @TranStarted   bit
    SET @TranStarted = 0

    IF( @@TRANCOUNT = 0 )
    BEGIN
	    BEGIN TRANSACTION
	    SET @TranStarted = 1
    END
    ELSE
    	SET @TranStarted = 0

    SELECT  @UserId = UserId
    FROM    dbo.aspnet_Users
    WHERE   LoweredUserName = LOWER(@UserName)

    IF ( @UserId IS NULL )
    BEGIN
        SET @ErrorCode = 1
        GOTO Cleanup
    END

    SELECT @IsLockedOut = IsLockedOut,
           @LastLockoutDate = LastLockoutDate,
           @FailedPasswordAttemptCount = FailedPasswordAttemptCount,
           @FailedPasswordAttemptWindowStart = FailedPasswordAttemptWindowStart,
           @FailedPasswordAnswerAttemptCount = FailedPasswordAnswerAttemptCount,
           @FailedPasswordAnswerAttemptWindowStart = FailedPasswordAnswerAttemptWindowStart
    FROM dbo.aspnet_Users WITH ( UPDLOCK )
    WHERE @UserId = UserId

    IF( @IsLockedOut = 1 )
    BEGIN
        SET @ErrorCode = 99
        GOTO Cleanup
    END

    UPDATE dbo.aspnet_Users
    SET    Password = @NewPassword,
           LastPasswordChangedDate = @CurrentTime,
           PasswordFormat = @PasswordFormat,
           PasswordSalt = @PasswordSalt
    WHERE  @UserId = UserId AND
           ( ( @PasswordAnswer IS NULL ) OR ( LOWER( PasswordAnswer ) = LOWER( @PasswordAnswer ) ) )

    IF ( @@ROWCOUNT = 0 )
        BEGIN
            IF( @CurrentTime > DATEADD( minute, @PasswordAttemptWindow, @FailedPasswordAnswerAttemptWindowStart ) )
            BEGIN
                SET @FailedPasswordAnswerAttemptWindowStart = @CurrentTime
                SET @FailedPasswordAnswerAttemptCount = 1
            END
            ELSE
            BEGIN
                SET @FailedPasswordAnswerAttemptWindowStart = @CurrentTime
                SET @FailedPasswordAnswerAttemptCount = @FailedPasswordAnswerAttemptCount + 1
            END

            BEGIN
                IF( @FailedPasswordAnswerAttemptCount >= @MaxInvalidPasswordAttempts )
                BEGIN
                    SET @IsLockedOut = 1
                    SET @LastLockoutDate = @CurrentTime
                END
            END

            SET @ErrorCode = 3
        END
    ELSE
        BEGIN
            IF( @FailedPasswordAnswerAttemptCount > 0 )
            BEGIN
                SET @FailedPasswordAnswerAttemptCount = 0
                SET @FailedPasswordAnswerAttemptWindowStart = CONVERT( datetime, ''17540101'', 112 )
            END
        END

    IF( NOT ( @PasswordAnswer IS NULL ) )
    BEGIN
        UPDATE dbo.aspnet_Users
        SET IsLockedOut = @IsLockedOut, LastLockoutDate = @LastLockoutDate,
            FailedPasswordAttemptCount = @FailedPasswordAttemptCount,
            FailedPasswordAttemptWindowStart = @FailedPasswordAttemptWindowStart,
            FailedPasswordAnswerAttemptCount = @FailedPasswordAnswerAttemptCount,
            FailedPasswordAnswerAttemptWindowStart = @FailedPasswordAnswerAttemptWindowStart
        WHERE @UserId = UserId

        IF( @@ERROR <> 0 )
        BEGIN
            SET @ErrorCode = -1
            GOTO Cleanup
        END
    END

    IF( @TranStarted = 1 )
    BEGIN
	SET @TranStarted = 0
	COMMIT TRANSACTION
    END

    RETURN @ErrorCode

Cleanup:

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
    	ROLLBACK TRANSACTION
    END

    RETURN @ErrorCode

END' 
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_GetUserByUserId]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Membership_GetUserByUserId]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[aspnet_Membership_GetUserByUserId]
    @UserId               int,
    @CurrentTime       datetime,
    @UpdateLastActivity   bit = 0
AS
BEGIN
    IF ( @UpdateLastActivity = 1 )
    BEGIN
        UPDATE   dbo.aspnet_Users
        SET      LastActivityDate = @CurrentTime
        FROM     dbo.aspnet_Users
        WHERE    @UserId = UserId

        IF ( @@ROWCOUNT = 0 ) -- User ID not found
            RETURN -1
    END

    SELECT  Email, PasswordQuestion, Comment, IsApproved,
            CreateDate, LastLoginDate, LastActivityDate,
            LastPasswordChangedDate, UserName, IsLockedOut,
            LastLockoutDate
    FROM    dbo.aspnet_Users
    WHERE   @UserId = UserId

    IF ( @@ROWCOUNT = 0 ) -- User ID not found
       RETURN -1

    RETURN 0
END' 
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_GetUserByName]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Membership_GetUserByName]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[aspnet_Membership_GetUserByName]
    @UserName             nvarchar(256),
    @CurrentTime       datetime,
    @UpdateLastActivity   bit = 0
AS
BEGIN
    DECLARE @UserId int

    IF (@UpdateLastActivity = 1)
    BEGIN
        SELECT TOP 1 Email, PasswordQuestion, Comment, IsApproved,
                CreateDate, LastLoginDate, @CurrentTime, LastPasswordChangedDate,
                UserId, IsLockedOut,LastLockoutDate
        FROM    dbo.aspnet_Users
        WHERE    LOWER(@UserName) = LoweredUserName

        IF (@@ROWCOUNT = 0) -- Username not found
            RETURN -1

        UPDATE   dbo.aspnet_Users
        SET      LastActivityDate = @CurrentTime
        WHERE    @UserId = UserId
    END
    ELSE
    BEGIN
        SELECT TOP 1 Email, PasswordQuestion, Comment, IsApproved,
                CreateDate, LastLoginDate, LastActivityDate, LastPasswordChangedDate,
                UserId, IsLockedOut,LastLockoutDate
        FROM    dbo.aspnet_Users
        WHERE    LOWER(@UserName) = LoweredUserName

        IF (@@ROWCOUNT = 0) -- Username not found
            RETURN -1
    END

    RETURN 0
END' 
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_GetUserByEmail]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Membership_GetUserByEmail]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[aspnet_Membership_GetUserByEmail]
    @Email            nvarchar(256)
AS
BEGIN
    IF( @Email IS NULL )
        SELECT  UserName
        FROM    dbo.aspnet_Users
        WHERE   LoweredEmail IS NULL
    ELSE
        SELECT  UserName
        FROM    dbo.aspnet_Users
        WHERE   LOWER(@Email) = LoweredEmail

    IF (@@rowcount = 0)
        RETURN(1)
    RETURN(0)
END' 
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_GetPasswordWithFormat]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Membership_GetPasswordWithFormat]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[aspnet_Membership_GetPasswordWithFormat]
    @UserName                       nvarchar(256),
    @UpdateLastLoginActivityDate    bit,
    @CurrentTime                 datetime
AS
BEGIN
    DECLARE @IsLockedOut                        bit
    DECLARE @UserId                             int
    DECLARE @Password                           nvarchar(128)
    DECLARE @PasswordSalt                       nvarchar(128)
    DECLARE @PasswordFormat                     int
    DECLARE @FailedPasswordAttemptCount         int
    DECLARE @FailedPasswordAnswerAttemptCount   int
    DECLARE @IsApproved                         bit
    DECLARE @LastActivityDate                   datetime
    DECLARE @LastLoginDate                      datetime

    SELECT  @UserId          = NULL

    SELECT  @UserId = UserId, @IsLockedOut = IsLockedOut, @Password=Password, @PasswordFormat=PasswordFormat,
            @PasswordSalt=PasswordSalt, @FailedPasswordAttemptCount=FailedPasswordAttemptCount,
		    @FailedPasswordAnswerAttemptCount=FailedPasswordAnswerAttemptCount, @IsApproved=IsApproved,
            @LastActivityDate = LastActivityDate, @LastLoginDate = LastLoginDate
    FROM    dbo.aspnet_Users
    WHERE  LOWER(@UserName) = LoweredUserName

    IF (@UserId IS NULL)
        RETURN 1

    IF (@IsLockedOut = 1)
        RETURN 99

    SELECT   @Password, @PasswordFormat, @PasswordSalt, @FailedPasswordAttemptCount,
             @FailedPasswordAnswerAttemptCount, @IsApproved, @LastLoginDate, @LastActivityDate

    IF (@UpdateLastLoginActivityDate = 1 AND @IsApproved = 1)
    BEGIN
        UPDATE  dbo.aspnet_Users
        SET     LastLoginDate = @CurrentTime
        WHERE   UserId = @UserId

        UPDATE  dbo.aspnet_Users
        SET     LastActivityDate = @CurrentTime
        WHERE   @UserId = UserId
    END


    RETURN 0
END' 
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_GetPassword]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Membership_GetPassword]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[aspnet_Membership_GetPassword]
    @UserName                       nvarchar(256),
    @MaxInvalidPasswordAttempts     int,
    @PasswordAttemptWindow          int,
    @CurrentTime                 datetime,
    @PasswordAnswer                 nvarchar(128) = NULL
AS
BEGIN
    DECLARE @UserId                                 int
    DECLARE @PasswordFormat                         int
    DECLARE @Password                               nvarchar(128)
    DECLARE @passAns                                nvarchar(128)
    DECLARE @IsLockedOut                            bit
    DECLARE @LastLockoutDate                        datetime
    DECLARE @FailedPasswordAttemptCount             int
    DECLARE @FailedPasswordAttemptWindowStart       datetime
    DECLARE @FailedPasswordAnswerAttemptCount       int
    DECLARE @FailedPasswordAnswerAttemptWindowStart datetime

    DECLARE @ErrorCode     int
    SET @ErrorCode = 0

    DECLARE @TranStarted   bit
    SET @TranStarted = 0

    IF( @@TRANCOUNT = 0 )
    BEGIN
	    BEGIN TRANSACTION
	    SET @TranStarted = 1
    END
    ELSE
    	SET @TranStarted = 0

    SELECT  @UserId = u.UserId,
            @Password = u.Password,
            @passAns = u.PasswordAnswer,
            @PasswordFormat = u.PasswordFormat,
            @IsLockedOut = u.IsLockedOut,
            @LastLockoutDate = u.LastLockoutDate,
            @FailedPasswordAttemptCount = u.FailedPasswordAttemptCount,
            @FailedPasswordAttemptWindowStart = u.FailedPasswordAttemptWindowStart,
            @FailedPasswordAnswerAttemptCount = u.FailedPasswordAnswerAttemptCount,
            @FailedPasswordAnswerAttemptWindowStart = u.FailedPasswordAnswerAttemptWindowStart
    FROM    dbo.aspnet_Users u
    WHERE   LOWER(@UserName) = u.LoweredUserName

    IF ( @@rowcount = 0 )
    BEGIN
        SET @ErrorCode = 1
        GOTO Cleanup
    END

    IF( @IsLockedOut = 1 )
    BEGIN
        SET @ErrorCode = 99
        GOTO Cleanup
    END

    IF ( NOT( @PasswordAnswer IS NULL ) )
    BEGIN
        IF( ( @passAns IS NULL ) OR ( LOWER( @passAns ) <> LOWER( @PasswordAnswer ) ) )
        BEGIN
            IF( @CurrentTime > DATEADD( minute, @PasswordAttemptWindow, @FailedPasswordAnswerAttemptWindowStart ) )
            BEGIN
                SET @FailedPasswordAnswerAttemptWindowStart = @CurrentTime
                SET @FailedPasswordAnswerAttemptCount = 1
            END
            ELSE
            BEGIN
                SET @FailedPasswordAnswerAttemptCount = @FailedPasswordAnswerAttemptCount + 1
                SET @FailedPasswordAnswerAttemptWindowStart = @CurrentTime
            END

            BEGIN
                IF( @FailedPasswordAnswerAttemptCount >= @MaxInvalidPasswordAttempts )
                BEGIN
                    SET @IsLockedOut = 1
                    SET @LastLockoutDate = @CurrentTime
                END
            END

            SET @ErrorCode = 3
        END
        ELSE
        BEGIN
            IF( @FailedPasswordAnswerAttemptCount > 0 )
            BEGIN
                SET @FailedPasswordAnswerAttemptCount = 0
                SET @FailedPasswordAnswerAttemptWindowStart = CONVERT( datetime, ''17540101'', 112 )
            END
        END

        UPDATE dbo.aspnet_Users
        SET IsLockedOut = @IsLockedOut, LastLockoutDate = @LastLockoutDate,
            FailedPasswordAttemptCount = @FailedPasswordAttemptCount,
            FailedPasswordAttemptWindowStart = @FailedPasswordAttemptWindowStart,
            FailedPasswordAnswerAttemptCount = @FailedPasswordAnswerAttemptCount,
            FailedPasswordAnswerAttemptWindowStart = @FailedPasswordAnswerAttemptWindowStart
        WHERE @UserId = UserId

        IF( @@ERROR <> 0 )
        BEGIN
            SET @ErrorCode = -1
            GOTO Cleanup
        END
    END

    IF( @TranStarted = 1 )
    BEGIN
	SET @TranStarted = 0
	COMMIT TRANSACTION
    END

    IF( @ErrorCode = 0 )
        SELECT @Password, @PasswordFormat

    RETURN @ErrorCode

Cleanup:

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
    	ROLLBACK TRANSACTION
    END

    RETURN @ErrorCode

END' 
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_GetNumberOfUsersOnline]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Membership_GetNumberOfUsersOnline]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[aspnet_Membership_GetNumberOfUsersOnline]
    @MinutesSinceLastInActive   int,
    @CurrentTime             datetime
AS
BEGIN
    DECLARE @DateActive datetime
    SELECT  @DateActive = DATEADD(minute,  -(@MinutesSinceLastInActive), @CurrentTime)

    DECLARE @NumOnline int
    SELECT  @NumOnline = COUNT(*)
    FROM    dbo.aspnet_Users (NOLOCK)
    WHERE   LastActivityDate > @DateActive

    RETURN(@NumOnline)
END' 
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_GetAllUsers]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Membership_GetAllUsers]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[aspnet_Membership_GetAllUsers]
    @PageIndex             int,
    @PageSize              int
AS
BEGIN

    -- Set the page bounds
    DECLARE @PageLowerBound int
    DECLARE @PageUpperBound int
    DECLARE @TotalRecords   int
    SET @PageLowerBound = @PageSize * (@PageIndex-1)
    SET @PageUpperBound = @PageSize - 1 + @PageLowerBound

    -- Create a temp table TO store the select results
    CREATE TABLE #PageIndexForUsers
    (
        IndexId int IDENTITY (0, 1) NOT NULL,
        UserId int
    )

    -- Insert into our temp table
    INSERT INTO #PageIndexForUsers (UserId)
    SELECT UserId
    FROM   dbo.aspnet_Users
    ORDER BY UserName

    SELECT @TotalRecords = @@ROWCOUNT

    SELECT u.UserName, u.Email, u.PasswordQuestion, u.Comment, u.IsApproved,
            u.CreateDate,
            u.LastLoginDate,
            u.LastActivityDate,
            u.LastPasswordChangedDate,
            u.UserId, u.IsLockedOut,
            u.LastLockoutDate
    FROM   dbo.aspnet_Users u, #PageIndexForUsers p
    WHERE  u.UserId = p.UserId AND
           p.IndexId >= @PageLowerBound AND p.IndexId <= @PageUpperBound
    ORDER BY u.UserName
    RETURN @TotalRecords
    
    DROP TABLE #PageIndexForUsers
END' 
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_FindUsersByName]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Membership_FindUsersByName]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[aspnet_Membership_FindUsersByName]
    @UserNameToMatch       nvarchar(256),
    @PageIndex             int,
    @PageSize              int
AS
BEGIN

    -- Set the page bounds
    DECLARE @PageLowerBound int
    DECLARE @PageUpperBound int
    DECLARE @TotalRecords   int
    SET @PageLowerBound = @PageSize * (@PageIndex-1)
    SET @PageUpperBound = @PageSize - 1 + @PageLowerBound

    -- Create a temp table TO store the select results
    CREATE TABLE #PageIndexForUsers
    (
        IndexId int IDENTITY (0, 1) NOT NULL,
        UserId int
    )

    -- Insert into our temp table
    INSERT INTO #PageIndexForUsers (UserId)
        SELECT UserId
        FROM   dbo.aspnet_Users
        WHERE  LoweredUserName LIKE LOWER(@UserNameToMatch)
        ORDER BY UserName

    SELECT  u.UserName, u.Email, u.PasswordQuestion, u.Comment, u.IsApproved,
            u.CreateDate,
            u.LastLoginDate,
            u.LastActivityDate,
            u.LastPasswordChangedDate,
            u.UserId, u.IsLockedOut,
            u.LastLockoutDate
    FROM   dbo.aspnet_Users u, #PageIndexForUsers p
    WHERE  u.UserId = p.UserId AND
           p.IndexId >= @PageLowerBound AND p.IndexId <= @PageUpperBound
    ORDER BY u.UserName

    SELECT  @TotalRecords = COUNT(*)
    FROM    #PageIndexForUsers
    RETURN @TotalRecords
    
    DROP TABLE #PageIndexForUsers
END' 
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_FindUsersByEmail]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Membership_FindUsersByEmail]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[aspnet_Membership_FindUsersByEmail]
    @EmailToMatch          nvarchar(256),
    @PageIndex             int,
    @PageSize              int
AS
BEGIN

    -- Set the page bounds
    DECLARE @PageLowerBound int
    DECLARE @PageUpperBound int
    DECLARE @TotalRecords   int
    SET @PageLowerBound = @PageSize * (@PageIndex-1)
    SET @PageUpperBound = @PageSize - 1 + @PageLowerBound

    -- Create a temp table TO store the select results
    CREATE TABLE #PageIndexForUsers
    (
        IndexId int IDENTITY (0, 1) NOT NULL,
        UserId int
    )

    -- Insert into our temp table
    IF( @EmailToMatch IS NULL )
        INSERT INTO #PageIndexForUsers (UserId)
            SELECT UserId
            FROM   dbo.aspnet_Users
            WHERE  Email IS NULL
            ORDER BY LoweredEmail
    ELSE
        INSERT INTO #PageIndexForUsers (UserId)
            SELECT UserId
            FROM   dbo.aspnet_Users
            WHERE  LoweredEmail LIKE LOWER(@EmailToMatch)
            ORDER BY LoweredEmail

    SELECT  u.UserName, u.Email, u.PasswordQuestion, u.Comment, u.IsApproved,
            u.CreateDate,
            u.LastLoginDate,
            u.LastActivityDate,
            u.LastPasswordChangedDate,
            u.UserId, u.IsLockedOut,
            u.LastLockoutDate
    FROM   dbo.aspnet_Users u, #PageIndexForUsers p
    WHERE  u.UserId = p.UserId AND
           p.IndexId >= @PageLowerBound AND p.IndexId <= @PageUpperBound
    ORDER BY u.LoweredEmail

    SELECT  @TotalRecords = COUNT(*)
    FROM    #PageIndexForUsers
    RETURN @TotalRecords
    
    DROP TABLE #PageIndexForUsers
END' 
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Roles_RoleExists]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Roles_RoleExists]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[aspnet_Roles_RoleExists]
    @RoleName         nvarchar(256)
AS
BEGIN

    IF (EXISTS (SELECT RoleName FROM dbo.aspnet_Roles WHERE LOWER(@RoleName) = LoweredRoleName))
        RETURN(1)
    ELSE
        RETURN(0)
END' 
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Roles_GetAllRoles]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Roles_GetAllRoles]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[aspnet_Roles_GetAllRoles] 
AS
BEGIN
    SELECT RoleName
    FROM   dbo.aspnet_Roles
END' 
END
GO
/****** Object:  Table [dbo].[Ecshop_InpourRequest]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_InpourRequest]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_InpourRequest](
	[InpourId] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[TradeDate] [datetime] NOT NULL,
	[InpourBlance] [money] NOT NULL,
	[UserId] [int] NOT NULL,
	[PaymentId] [int] NOT NULL,
 CONSTRAINT [PK_Ecshop_InpourRequest] PRIMARY KEY CLUSTERED 
(
	[InpourId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Ecshop_Hotkeywords]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_Hotkeywords]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_Hotkeywords](
	[Hid] [int] IDENTITY(1,1) NOT NULL,
	[CategoryId] [int] NOT NULL,
	[Keywords] [nvarchar](512) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[SearchTime] [datetime] NOT NULL,
	[Lasttime] [datetime] NOT NULL,
	[Frequency] [int] NOT NULL,
 CONSTRAINT [PK_Ecshop_Hotkeywords] PRIMARY KEY CLUSTERED 
(
	[Hid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Ecshop_Helps]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_Helps]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_Helps](
	[CategoryId] [int] NOT NULL,
	[HelpId] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](200) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[Meta_Description] [nvarchar](1000) COLLATE Chinese_PRC_CI_AS NULL,
	[Meta_Keywords] [nvarchar](1000) COLLATE Chinese_PRC_CI_AS NULL,
	[Description] [nvarchar](1000) COLLATE Chinese_PRC_CI_AS NULL,
	[Content] [ntext] COLLATE Chinese_PRC_CI_AS NOT NULL,
	[AddedDate] [datetime] NOT NULL,
	[IsShowFooter] [bit] NOT NULL,
 CONSTRAINT [PK_Ecshop_Helps] PRIMARY KEY NONCLUSTERED 
(
	[HelpId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Ecshop_LeaveCommentReplys]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_LeaveCommentReplys]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_LeaveCommentReplys](
	[LeaveId] [bigint] NOT NULL,
	[ReplyId] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[ReplyContent] [nvarchar](3000) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[ReplyDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Ecshop_LeaveCommentReplys] PRIMARY KEY CLUSTERED 
(
	[ReplyId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_LeaveCommentReplys]') AND name = N'Ecshop_LeaveCommentReplys_Index2')
CREATE NONCLUSTERED INDEX [Ecshop_LeaveCommentReplys_Index2] ON [dbo].[Ecshop_LeaveCommentReplys] 
(
	[LeaveId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
GO
/****** Object:  Table [dbo].[Ecshop_CouponItems]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_CouponItems]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_CouponItems](
	[CouponId] [int] NOT NULL,
	[LotNumber] [uniqueidentifier] NOT NULL,
	[ClaimCode] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[UserId] [int] NULL,
	[UserName] [nvarchar](256) COLLATE Chinese_PRC_CI_AS NULL,
	[EmailAddress] [nvarchar](255) COLLATE Chinese_PRC_CI_AS NULL,
	[GenerateTime] [datetime] NOT NULL,
	[CouponStatus] [int] NOT NULL,
	[UsedTime] [datetime] NULL,
	[OrderId] [nvarchar](60) COLLATE Chinese_PRC_CI_AS NULL,
 CONSTRAINT [PK_Ecshop_CouponItems] PRIMARY KEY CLUSTERED 
(
	[ClaimCode] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Ecshop_CountDown]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_CountDown]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_CountDown](
	[CountDownId] [int] IDENTITY(1,1) NOT NULL,
	[ProductId] [int] NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[EndDate] [datetime] NOT NULL,
	[Content] [ntext] COLLATE Chinese_PRC_CI_AS NULL,
	[DisplaySequence] [int] NOT NULL,
	[CountDownPrice] [money] NOT NULL,
	[MaxCount] [int] NULL,
 CONSTRAINT [PK_Ecshop_CountDown] PRIMARY KEY NONCLUSTERED 
(
	[CountDownId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Ecshop_BundlingProductItems]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_BundlingProductItems]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_BundlingProductItems](
	[BundlingItemID] [int] IDENTITY(1,1) NOT NULL,
	[BundlingID] [int] NULL,
	[ProductID] [int] NOT NULL,
	[SkuId] [nvarchar](100) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[ProductNum] [int] NOT NULL,
 CONSTRAINT [PK_BundlingProductItems] PRIMARY KEY NONCLUSTERED 
(
	[BundlingItemID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Ecshop_Favorite]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_Favorite]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_Favorite](
	[FavoriteId] [int] IDENTITY(1,1) NOT NULL,
	[ProductId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[Tags] [nvarchar](100) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[Remark] [nvarchar](500) COLLATE Chinese_PRC_CI_AS NULL,
 CONSTRAINT [PK_Ecshop_Favorite] PRIMARY KEY CLUSTERED 
(
	[ProductId] ASC,
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Ecshop_Attributes]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_Attributes]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_Attributes](
	[AttributeId] [int] IDENTITY(1,1) NOT NULL,
	[AttributeName] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[DisplaySequence] [int] NOT NULL,
	[TypeId] [int] NOT NULL,
	[UsageMode] [int] NOT NULL,
	[UseAttributeImage] [bit] NOT NULL,
 CONSTRAINT [PK_Ecshop_Attributes] PRIMARY KEY CLUSTERED 
(
	[AttributeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Ecshop_Articles]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_Articles]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_Articles](
	[CategoryId] [int] NOT NULL,
	[ArticleId] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](200) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[Meta_Description] [nvarchar](1000) COLLATE Chinese_PRC_CI_AS NULL,
	[Meta_Keywords] [nvarchar](1000) COLLATE Chinese_PRC_CI_AS NULL,
	[IconUrl] [nvarchar](255) COLLATE Chinese_PRC_CI_AS NULL,
	[Description] [nvarchar](1000) COLLATE Chinese_PRC_CI_AS NULL,
	[Content] [ntext] COLLATE Chinese_PRC_CI_AS NOT NULL,
	[AddedDate] [datetime] NOT NULL,
	[IsRelease] [bit] NOT NULL,
 CONSTRAINT [PK_Ecshop_Articles] PRIMARY KEY NONCLUSTERED 
(
	[ArticleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Ecshop_GroupBuy]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_GroupBuy]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_GroupBuy](
	[GroupBuyId] [int] IDENTITY(1,1) NOT NULL,
	[ProductId] [int] NOT NULL,
	[NeedPrice] [money] NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[EndDate] [datetime] NOT NULL,
	[MaxCount] [int] NOT NULL,
	[Content] [ntext] COLLATE Chinese_PRC_CI_AS NULL,
	[Status] [int] NOT NULL,
	[DisplaySequence] [int] NOT NULL,
 CONSTRAINT [PK_Ecshop_GroupBuy] PRIMARY KEY NONCLUSTERED 
(
	[GroupBuyId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Ecshop_GiftShoppingCarts]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_GiftShoppingCarts]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_GiftShoppingCarts](
	[UserId] [int] NOT NULL,
	[GiftId] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[AddTime] [datetime] NOT NULL,
	[PromoType] [int] NOT NULL,
 CONSTRAINT [PK_GiftEcshop_ShoppingCarts] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[GiftId] ASC,
	[PromoType] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Ecshop_OrderReturns]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_OrderReturns]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_OrderReturns](
	[ReturnsId] [int] IDENTITY(10000,1) NOT NULL,
	[OrderId] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[ApplyForTime] [datetime] NOT NULL,
	[RefundType] [int] NOT NULL,
	[RefundMoney] [money] NOT NULL,
	[Comments] [ntext] COLLATE Chinese_PRC_CI_AS NULL,
	[HandleStatus] [int] NOT NULL,
	[HandleTime] [datetime] NULL,
	[AdminRemark] [ntext] COLLATE Chinese_PRC_CI_AS NULL,
	[Operator] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
 CONSTRAINT [PK_Ecshop_Returns] PRIMARY KEY NONCLUSTERED 
(
	[ReturnsId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Ecshop_OrderReplace]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_OrderReplace]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_OrderReplace](
	[ReplaceId] [int] IDENTITY(10000,1) NOT NULL,
	[OrderId] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[ApplyForTime] [datetime] NOT NULL,
	[Comments] [ntext] COLLATE Chinese_PRC_CI_AS NULL,
	[HandleStatus] [int] NOT NULL,
	[HandleTime] [datetime] NULL,
	[AdminRemark] [ntext] COLLATE Chinese_PRC_CI_AS NULL,
 CONSTRAINT [PK_Ecshop_OrderReplace] PRIMARY KEY NONCLUSTERED 
(
	[ReplaceId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Ecshop_OrderRefund]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_OrderRefund]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_OrderRefund](
	[RefundId] [int] IDENTITY(10000,1) NOT NULL,
	[OrderId] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[ApplyForTime] [datetime] NOT NULL,
	[RefundType] [int] NULL,
	[RefundRemark] [ntext] COLLATE Chinese_PRC_CI_AS NULL,
	[AdminRemark] [ntext] COLLATE Chinese_PRC_CI_AS NULL,
	[HandleStatus] [int] NOT NULL,
	[HandleTime] [datetime] NULL,
	[Operator] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
 CONSTRAINT [PK_Ecshop_Refund] PRIMARY KEY NONCLUSTERED 
(
	[RefundId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Ecshop_OrderItems]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_OrderItems]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_OrderItems](
	[OrderId] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[SkuId] [nvarchar](100) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[ProductId] [int] NOT NULL,
	[SKU] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Quantity] [int] NOT NULL,
	[ShipmentQuantity] [int] NOT NULL,
	[CostPrice] [money] NOT NULL,
	[ItemListPrice] [money] NOT NULL,
	[ItemAdjustedPrice] [money] NOT NULL,
	[ItemDescription] [nvarchar](200) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[ThumbnailsUrl] [nvarchar](255) COLLATE Chinese_PRC_CI_AS NULL,
	[Weight] [money] NULL,
	[SKUContent] [nvarchar](4000) COLLATE Chinese_PRC_CI_AS NULL,
	[PromotionId] [int] NULL,
	[PromotionName] [nvarchar](100) COLLATE Chinese_PRC_CI_AS NULL,
 CONSTRAINT [PK_Ecshop_OrderItems] PRIMARY KEY NONCLUSTERED 
(
	[OrderId] ASC,
	[SkuId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Ecshop_OrderGifts]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_OrderGifts]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_OrderGifts](
	[OrderId] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[GiftId] [int] NOT NULL,
	[GiftName] [nvarchar](100) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[CostPrice] [money] NULL,
	[ThumbnailsUrl] [nvarchar](255) COLLATE Chinese_PRC_CI_AS NULL,
	[Quantity] [int] NULL,
	[PromoType] [int] NOT NULL,
 CONSTRAINT [PK_Ecshop_OrderGifts] PRIMARY KEY CLUSTERED 
(
	[OrderId] ASC,
	[GiftId] ASC,
	[PromoType] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Ecshop_OrderDebitNote]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_OrderDebitNote]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_OrderDebitNote](
	[NoteId] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[OrderId] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[Operator] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[Remark] [nvarchar](200) COLLATE Chinese_PRC_CI_AS NOT NULL,
 CONSTRAINT [PK_Ecshop_OrderDebitNote] PRIMARY KEY CLUSTERED 
(
	[NoteId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Ecshop_ProductReviews]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_ProductReviews]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_ProductReviews](
	[ReviewId] [bigint] IDENTITY(1,1) NOT NULL,
	[ProductId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[ReviewText] [nvarchar](3000) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[UserName] [nvarchar](100) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[UserEmail] [nvarchar](100) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[ReviewDate] [datetime] NOT NULL,
	[OrderId] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[SkuId] [nvarchar](100) COLLATE Chinese_PRC_CI_AS NULL,
 CONSTRAINT [PK_Ecshop_ProductReviews] PRIMARY KEY NONCLUSTERED 
(
	[ReviewId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_ProductReviews]') AND name = N'Ecshop_ProductReviews_Index')
CREATE CLUSTERED INDEX [Ecshop_ProductReviews_Index] ON [dbo].[Ecshop_ProductReviews] 
(
	[ReviewDate] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_ProductReviews]') AND name = N'Ecshop_ProductReviews_Index2')
CREATE NONCLUSTERED INDEX [Ecshop_ProductReviews_Index2] ON [dbo].[Ecshop_ProductReviews] 
(
	[ProductId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
GO
/****** Object:  Table [dbo].[Ecshop_ProductConsultations]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_ProductConsultations]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_ProductConsultations](
	[ConsultationId] [int] IDENTITY(1,1) NOT NULL,
	[ProductId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[UserName] [nvarchar](100) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[UserEmail] [nvarchar](100) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[ConsultationText] [nvarchar](1000) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[ConsultationDate] [datetime] NOT NULL,
	[ReplyText] [ntext] COLLATE Chinese_PRC_CI_AS NULL,
	[ReplyDate] [datetime] NULL,
	[ReplyUserId] [int] NULL,
	[ViewDate] [datetime] NULL,
 CONSTRAINT [PK_Ecshop_ProductConsultations] PRIMARY KEY NONCLUSTERED 
(
	[ConsultationId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_ProductConsultations]') AND name = N'Ecshop_ProductConsultations_Index')
CREATE CLUSTERED INDEX [Ecshop_ProductConsultations_Index] ON [dbo].[Ecshop_ProductConsultations] 
(
	[ReplyDate] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_ProductConsultations]') AND name = N'Ecshop_ProductConsultations_Index2')
CREATE NONCLUSTERED INDEX [Ecshop_ProductConsultations_Index2] ON [dbo].[Ecshop_ProductConsultations] 
(
	[ProductId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
GO
/****** Object:  View [dbo].[vw_aspnet_Members]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_aspnet_Members]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [dbo].[vw_aspnet_Members]
AS
SELECT    mg.Name AS GradeName, u.UserId, u.UserName, u.IsAnonymous, u.LastActivityDate, u.PasswordFormat, u.Email, u.PasswordQuestion, u.IsApproved, u.IsLockedOut, 
                      u.CreateDate, u.LastLoginDate, u.LastPasswordChangedDate, u.LastLockoutDate, u.Comment, u.Gender, u.BirthDate, m.GradeId, 
					  m.ReferralUserId,m.ReferralStatus, m.ReferralReason, m.ReferralRequetsDate, m.RefusalReason, m.ReferralAuditDate,
                      m.TradePasswordFormat, m.OrderNumber, m.Expenditure, m.Points, m.Balance, m.RequestBalance, m.TopRegionId, m.RegionId, m.RealName, m.Address, m.Zipcode, m.TelPhone, 
                      m.CellPhone, m.QQ, m.MSN,m.Wangwang, m.VipCardNumber
FROM         dbo.aspnet_Users AS u INNER JOIN
                      dbo.aspnet_Members AS m ON u.UserId = m.UserId INNER JOIN aspnet_MemberGrades mg ON m.GradeId = mg.GradeId WHERE u.UserRole = 3'
GO
/****** Object:  View [dbo].[vw_aspnet_Managers]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_aspnet_Managers]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [dbo].[vw_aspnet_Managers]
AS
SELECT     u.UserId, u.UserName, u.IsAnonymous, u.LastActivityDate, u.PasswordFormat, u.Email, u.PasswordQuestion, u.IsApproved, u.IsLockedOut, 
                      u.CreateDate, u.LastLoginDate, u.LastPasswordChangedDate, u.LastLockoutDate, u.Comment, u.Gender, u.BirthDate
FROM         dbo.aspnet_Users AS u INNER JOIN
                      dbo.aspnet_Managers AS m ON u.UserId = m.UserId WHERE u.UserRole = 1'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_DiagramPane1' , N'SCHEMA',N'dbo', N'VIEW',N'vw_aspnet_Managers', NULL,NULL))
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "u"
            Begin Extent = 
               Top = 0
               Left = 38
               Bottom = 327
               Right = 377
            End
            DisplayFlags = 280
            TopColumn = 8
         End
         Begin Table = "m"
            Begin Extent = 
               Top = 42
               Left = 456
               Bottom = 236
               Right = 747
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_aspnet_Managers'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_DiagramPaneCount' , N'SCHEMA',N'dbo', N'VIEW',N'vw_aspnet_Managers', NULL,NULL))
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_aspnet_Managers'
GO
/****** Object:  View [dbo].[vw_Ecshop_BundlingProducts]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_Ecshop_BundlingProducts]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [dbo].[vw_Ecshop_BundlingProducts]
AS
SELECT g.BundlingID, g.Name, g.Num, g.SaleStatus,g.Price,g.AddTime,g.DisplaySequence,g.ShortDescription,
(SELECT COUNT(OrderId) FROM Ecshop_Orders o WHERE o.BundlingId = g.BundlingID AND o.OrderStatus <> 1 AND o.OrderStatus <> 4) AS OrderCount
FROM Ecshop_BundlingProducts g'
GO
/****** Object:  Table [dbo].[Taobao_Products]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Taobao_Products]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Taobao_Products](
	[Cid] [bigint] NOT NULL,
	[StuffStatus] [nvarchar](10) COLLATE Chinese_PRC_CI_AS NULL,
	[ProductId] [int] NOT NULL,
	[ProTitle] [nvarchar](200) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[Num] [bigint] NOT NULL,
	[LocationState] [nvarchar](10) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[LocationCity] [nvarchar](10) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[FreightPayer] [nvarchar](10) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[PostFee] [money] NULL,
	[ExpressFee] [money] NULL,
	[EMSFee] [money] NULL,
	[HasInvoice] [bit] NOT NULL,
	[HasWarranty] [bit] NOT NULL,
	[HasDiscount] [bit] NOT NULL,
	[ValidThru] [bigint] NOT NULL,
	[ListTime] [datetime] NULL,
	[PropertyAlias] [nvarchar](4000) COLLATE Chinese_PRC_CI_AS NULL,
	[InputPids] [nvarchar](2000) COLLATE Chinese_PRC_CI_AS NULL,
	[InputStr] [nvarchar](2000) COLLATE Chinese_PRC_CI_AS NULL,
	[SkuProperties] [nvarchar](4000) COLLATE Chinese_PRC_CI_AS NULL,
	[SkuQuantities] [nvarchar](2000) COLLATE Chinese_PRC_CI_AS NULL,
	[SkuPrices] [nvarchar](2000) COLLATE Chinese_PRC_CI_AS NULL,
	[SkuOuterIds] [nvarchar](2000) COLLATE Chinese_PRC_CI_AS NULL,
	[FoodAttributes] [nvarchar](2000) COLLATE Chinese_PRC_CI_AS NULL,
 CONSTRAINT [PK_Taobao_Products] PRIMARY KEY NONCLUSTERED 
(
	[ProductId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Ecshop_VoteItems]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_VoteItems]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_VoteItems](
	[VoteId] [bigint] NOT NULL,
	[VoteItemId] [bigint] IDENTITY(1,1) NOT NULL,
	[VoteItemName] [nvarchar](100) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[ItemCount] [int] NOT NULL,
 CONSTRAINT [PK_Ecshop_VoteItems] PRIMARY KEY NONCLUSTERED 
(
	[VoteItemId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Ecshop_UserShippingAddresses]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_UserShippingAddresses]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_UserShippingAddresses](
	[RegionId] [int] NOT NULL,
	[ShippingId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[ShipTo] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[Address] [nvarchar](500) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[Zipcode] [nvarchar](20) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[TelPhone] [nvarchar](20) COLLATE Chinese_PRC_CI_AS NULL,
	[CellPhone] [nvarchar](20) COLLATE Chinese_PRC_CI_AS NULL,
	[IsDefault] [bit] NOT NULL,
 CONSTRAINT [PK_Ecshop_UserShippingAddresses] PRIMARY KEY CLUSTERED 
(
	[ShippingId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Ecshop_ShippingRegions]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_ShippingRegions]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_ShippingRegions](
	[TemplateId] [int] NOT NULL,
	[GroupId] [int] NOT NULL,
	[RegionId] [int] NOT NULL,
 CONSTRAINT [PK_Ecshop_ShippingRegions] PRIMARY KEY CLUSTERED 
(
	[TemplateId] ASC,
	[RegionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Ecshop_ProductTypeBrands]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_ProductTypeBrands]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_ProductTypeBrands](
	[ProductTypeId] [int] NOT NULL,
	[BrandId] [int] NOT NULL,
 CONSTRAINT [PK_Ecshop_ProductTypeBrands] PRIMARY KEY CLUSTERED 
(
	[ProductTypeId] ASC,
	[BrandId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Ecshop_ProductTag]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_ProductTag]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_ProductTag](
	[TagId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
 CONSTRAINT [PK_Ecshop_ProductTag] PRIMARY KEY CLUSTERED 
(
	[TagId] ASC,
	[ProductId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Ecshop_ShoppingCarts]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_ShoppingCarts]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_ShoppingCarts](
	[UserId] [int] NOT NULL,
	[SkuId] [nvarchar](100) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[Quantity] [int] NOT NULL,
	[AddTime] [datetime] NOT NULL,
 CONSTRAINT [PK_Ecshop_ShoppingCarts] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[SkuId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Ecshop_ShippingTypes]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_ShippingTypes]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_ShippingTypes](
	[ModeId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[TemplateId] [int] NOT NULL,
	[Description] [nvarchar](4000) COLLATE Chinese_PRC_CI_AS NULL,
	[DisplaySequence] [int] NOT NULL,
 CONSTRAINT [PK_Ecshop_ShippingTypes] PRIMARY KEY CLUSTERED 
(
	[ModeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Ecshop_ShippingTypeGroups]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_ShippingTypeGroups]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_ShippingTypeGroups](
	[GroupId] [int] IDENTITY(1,1) NOT NULL,
	[TemplateId] [int] NOT NULL,
	[Price] [money] NOT NULL,
	[AddPrice] [money] NULL,
 CONSTRAINT [PK_Ecshop_ShippingTypeGroups] PRIMARY KEY CLUSTERED 
(
	[GroupId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  StoredProcedure [dbo].[ss_ShoppingCart_AddAPILineItem]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ss_ShoppingCart_AddAPILineItem]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[ss_ShoppingCart_AddAPILineItem]
	(
		@APIuserId VARCHAR(50),
		@APIType VARCHAR(10),
		@APIProductId INT,
		@APISkuId NVARCHAR(100),
		@APIQuantity INT
	)
AS
	IF EXISTS (SELECT [APISkuId] FROM Ecshop_ApiShorpCart WHERE APIuserId = @APIuserId AND APIType = @APIType AND APISkuId=@APISkuId)
	BEGIN
		UPDATE 
				Ecshop_ApiShorpCart 
		SET 
				APIQuantity = APIQuantity + @APIQuantity
		WHERE 
				APIuserId = @APIuserId AND APIType = @APIType AND APISkuId=@APISkuId
	END
	ELSE
	BEGIN
		INSERT INTO Ecshop_ApiShorpCart 
			(APIuserId, APIType, APIProductId,APISkuId, APIQuantity) 
		VALUES 
			(@APIuserId,@APIType,@APIProductId, @APISkuId,@APIQuantity)
	END' 
END
GO
/****** Object:  StoredProcedure [dbo].[ss_CreateOrder]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ss_CreateOrder]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[ss_CreateOrder]
(
	-- 基本信息
    @OrderId nvarchar(50),
	@OrderDate	datetime,	
	@ReferralUserId int,
	@UserId	int,
    @UserName nvarchar(50),
    @Wangwang nvarchar(20),
    @RealName nvarchar(50),
    @EmailAddress	nvarchar(255) = null,
    @Remark Nvarchar(4000) =null,
	@AdjustedDiscount money,
	@OrderStatus int,
	-- 配送信息
	@ShippingRegion Nvarchar(300) = null,
	@Address Nvarchar(300) = null,
	@ZipCode Nvarchar(20) = null,
	@ShipTo Nvarchar(50) = null,
	@TelPhone Nvarchar(50) = null,
	@CellPhone Nvarchar(50) = null,
	@ShipToDate Nvarchar(50) = null,
	@ShippingModeId int = null,
	@ModeName Nvarchar(50) = null,
	@RegionId int = null,
	@Freight money = null,
	@AdjustedFreight money = null,
	@ShipOrderNumber Nvarchar(50) = null,	
    @Weight money = null,
	@ExpressCompanyName nvarchar(500),
    @ExpressCompanyAbb nvarchar(500),
    -- 支付信息
    @PaymentTypeId INT = null,
    @PaymentType Nvarchar(100) = null,	
    @PayCharge money = null,
    @RefundStatus int,
	@Gateway nvarchar(200)=null,
    -- 统计字段
    @OrderTotal money = null,
    @OrderPoint int = null,
    @OrderCostPrice money = null,
    @OrderProfit money = null,
    @OptionPrice money = null,
    @Amount money = null,    
    @DiscountAmount money=null,
	-- 促销信息
	@ReducedPromotionId int = null,
	@ReducedPromotionName nvarchar(100) = null,
	@ReducedPromotionAmount money = null,
	@IsReduced bit = null,

	@SentTimesPointPromotionId int = null,
	@SentTimesPointPromotionName nvarchar(100) = null,
	@TimesPoint money = null,
	@IsSendTimesPoint bit = null,

	@FreightFreePromotionId int = null,
	@FreightFreePromotionName nvarchar(100) = null,
	@IsFreightFree bit = null,
    -- 优惠券信息
    @CouponName nvarchar(100) = null,
	@CouponCode nvarchar(50) = null,
	@CouponAmount money = null,    
	@CouponValue money = null,
	--团购活动信息
	@GroupBuyId int = null,
	@NeedPrice money = null,
	@GroupBuyStatus int = null,
		--限时抢购信息
	@CountDownBuyId int = null,

	--捆绑商品
	@Bundlingid int=null,
	--捆绑价格
    @BundlingPrice money = null,
	--税金相关
	@TaobaoOrderId nvarchar(50) = null,
	@Tax money=null,
	@SourceOrder int,
	@InvoiceTitle nvarchar(50)
)
as 
  IF EXISTS (SELECT OrderId  FROM Ecshop_Orders WHERE OrderId = @OrderId)
    Return
  ELSE
  INSERT INTO Ecshop_Orders
   (OrderId, OrderDate, ReferralUserId, UserId, Username,RealName, EmailAddress, Remark, AdjustedDiscount, OrderStatus,
   ShippingRegion, Address, ZipCode, ShipTo, TelPhone, CellPhone, ShipToDate, ShippingModeId, ModeName, RegionId, Freight, AdjustedFreight, ShipOrderNumber, [Weight], 
   PaymentTypeId,PaymentType, PayCharge, RefundStatus, OrderTotal, OrderPoint, OrderCostPrice, OrderProfit, OptionPrice, Amount, 
   ReducedPromotionId,ReducedPromotionName,ReducedPromotionAmount,IsReduced,SentTimesPointPromotionId,SentTimesPointPromotionName,TimesPoint,IsSendTimesPoint,
   FreightFreePromotionId,FreightFreePromotionName,IsFreightFree,CouponName, CouponCode, CouponAmount, CouponValue,GroupBuyId,NeedPrice,GroupBuyStatus,CountDownBuyId,BundlingId,
   DiscountAmount,ExpressCompanyName,ExpressCompanyAbb,BundlingPrice,TaobaoOrderId,Tax,SourceOrder,InvoiceTitle,Gateway
  )
  VALUES 
  (@OrderId, @OrderDate, @ReferralUserId, @UserId, @Username,@RealName, @EmailAddress, @Remark, @AdjustedDiscount, @OrderStatus,
   @ShippingRegion, @Address, @ZipCode, @ShipTo, @TelPhone, @CellPhone, @ShipToDate, @ShippingModeId, @ModeName, @RegionId, @Freight, @AdjustedFreight, @ShipOrderNumber, @Weight, 
   @PaymentTypeId,@PaymentType, @PayCharge, @RefundStatus, @OrderTotal, @OrderPoint, @OrderCostPrice, @OrderProfit, @OptionPrice, @Amount, 
   @ReducedPromotionId,@ReducedPromotionName,@ReducedPromotionAmount,@IsReduced,@SentTimesPointPromotionId,@SentTimesPointPromotionName,@TimesPoint,@IsSendTimesPoint,
   @FreightFreePromotionId,@FreightFreePromotionName,@IsFreightFree,@CouponName, @CouponCode, @CouponAmount, @CouponValue,@GroupBuyId,@NeedPrice,@GroupBuyStatus,@CountDownBuyId,@Bundlingid,
   @DiscountAmount,@ExpressCompanyName,@ExpressCompanyAbb,@BundlingPrice,@TaobaoOrderId,@Tax,@SourceOrder,@InvoiceTitle,@Gateway
   )' 
END
GO
/****** Object:  Trigger [T_Ecshop_Categories_Insert]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[T_Ecshop_Categories_Insert]'))
EXEC dbo.sp_executesql @statement = N'CREATE   Trigger   [dbo].[T_Ecshop_Categories_Insert] ON [dbo].[Ecshop_Categories] FOR Insert AS
BEGIN
	DECLARE @ParentCategoryId INT
	SELECT @ParentCategoryId= ParentCategoryId FROM inserted;

	IF @ParentCategoryId = 0 OR @ParentCategoryId IS NULL
		RETURN

	IF EXISTS(SELECT CategoryId FROM Ecshop_Categories WHERE CategoryId = @ParentCategoryId AND HasChildren = 0)
	BEGIN
		-- 如果上级分类存在，且上级分类当前无子分类，则更新上级分类的HasChildren字段为1
		UPDATE Ecshop_Categories SET HasChildren = 1 WHERE CategoryId = @ParentCategoryId
	END
END'
GO
/****** Object:  Trigger [T_Ecshop_Categories_Delete]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[T_Ecshop_Categories_Delete]'))
EXEC dbo.sp_executesql @statement = N'CREATE   Trigger   [dbo].[T_Ecshop_Categories_Delete] ON [dbo].[Ecshop_Categories] FOR Delete AS
BEGIN
	DECLARE @ParentCategoryId INT
	SELECT @ParentCategoryId= ParentCategoryId FROM deleted;

	IF @ParentCategoryId = 0 OR @ParentCategoryId IS NULL
		RETURN
		
	IF EXISTS(SELECT CategoryId FROM Ecshop_Categories WHERE CategoryId = @ParentCategoryId)
	BEGIN
		-- 如果上级分类存在，且上级分类已没有任何子分类，则更新上级分类的HasChildren字段为0
		IF (SELECT COUNT(CategoryId) FROM Ecshop_Categories WHERE ParentCategoryId = @ParentCategoryId) = 0
			UPDATE Ecshop_Categories SET HasChildren = 0 WHERE CategoryId = @ParentCategoryId
	END
END'
GO
/****** Object:  Table [dbo].[Ecshop_PromotionProducts]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_PromotionProducts]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_PromotionProducts](
	[ActivityId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
 CONSTRAINT [PK_Ecshop_PromotionProducts] PRIMARY KEY CLUSTERED 
(
	[ProductId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Ecshop_PromotionMemberGrades]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_PromotionMemberGrades]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_PromotionMemberGrades](
	[ActivityId] [int] NOT NULL,
	[GradeId] [int] NOT NULL,
 CONSTRAINT [PK_Ecshop_PromotionMemberGrades] PRIMARY KEY CLUSTERED 
(
	[ActivityId] ASC,
	[GradeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Ecshop_OrderSendNote]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_OrderSendNote]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_OrderSendNote](
	[NoteId] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[OrderId] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[Operator] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[Remark] [nvarchar](200) COLLATE Chinese_PRC_CI_AS NOT NULL,
 CONSTRAINT [PK_Ecshop_OrderSendNote] PRIMARY KEY CLUSTERED 
(
	[NoteId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Ecshop_PointDetails]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_PointDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_PointDetails](
	[JournalNumber] [bigint] IDENTITY(1,1) NOT NULL,
	[OrderId] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[UserId] [int] NOT NULL,
	[TradeDate] [datetime] NOT NULL,
	[TradeType] [int] NOT NULL,
	[Increased] [int] NULL,
	[Reduced] [int] NULL,
	[Points] [int] NULL,
	[Remark] [nvarchar](200) COLLATE Chinese_PRC_CI_AS NULL,
 CONSTRAINT [PK__tmp_ms_x__B69723347B9B496D] PRIMARY KEY CLUSTERED 
(
	[JournalNumber] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_PointDetails]') AND name = N'Ecshop_PointDetails_Index2')
CREATE NONCLUSTERED INDEX [Ecshop_PointDetails_Index2] ON [dbo].[Ecshop_PointDetails] 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
GO
/****** Object:  StoredProcedure [dbo].[cp_Product_Update]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cp_Product_Update]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[cp_Product_Update]
(
@CategoryId INT,
@MainCategoryPath NVARCHAR(256),
@TypeId INT = NULL,
@ProductName NVARCHAR(200),
@ProductCode [nvarchar] (50),
@ShortDescription NVARCHAR(2000) = NULL,
@Unit NVARCHAR(10) = NULL,
@Description NTEXT = NULL,
@MobbileDescription NTEXT = NULL,
@Title NVARCHAR(100) = NULL,
@Meta_Description NVARCHAR(1000) = NULL,
@Meta_Keywords NVARCHAR(1000) = NULL,
@SaleStatus INT,
@DisplaySequence INT,
@ImageUrl1 [nvarchar] (255) = NULL,
@ImageUrl2 [nvarchar] (255) = NULL,
@ImageUrl3 [nvarchar] (255) = NULL,
@ImageUrl4 [nvarchar] (255) = NULL,
@ImageUrl5 [nvarchar] (255) = NULL,
@ThumbnailUrl40 [nvarchar] (255) = NULL,
@ThumbnailUrl60 [nvarchar] (255) = NULL,
@ThumbnailUrl100 [nvarchar] (255) = NULL,
@ThumbnailUrl160 [nvarchar] (255) = NULL,
@ThumbnailUrl180 [nvarchar] (255) = NULL,
@ThumbnailUrl220 [nvarchar] (255) = NULL,
@ThumbnailUrl310 [nvarchar] (255) = NULL,
@ThumbnailUrl410 [nvarchar] (255) = NULL,
@MarketPrice MONEY = NULL,
@BrandId INT,
@HasSKU BIT,
@IsfreeShipping BIT,
@VistiCounts [int],
@SaleCounts [int],
@ShowSaleCounts [int],
@ReferralDeduct money,
@SubMemberDeduct money,
@SubReferralDeduct money,
@ProductId INT
)
AS

--如果商品显示顺序存在，则所有这个商品后台的顺序加一
IF (SELECT DisplaySequence FROM Ecshop_Products WHERE ProductId = @ProductId) != @DisplaySequence AND EXISTS(SELECT ProductId FROM Ecshop_Products WHERE DisplaySequence = @DisplaySequence)
UPDATE Ecshop_Products SET DisplaySequence = DisplaySequence + 1 WHERE DisplaySequence >= @DisplaySequence

UPDATE Ecshop_Products SET
CategoryId = @CategoryId, MainCategoryPath = @MainCategoryPath,UpdateDate=GETDATE(), TypeId = @TypeId, ProductName = @ProductName, ProductCode = @ProductCode,
ShortDescription = @ShortDescription, Unit = @Unit, [Description] = @Description,[MobbileDescription] = @MobbileDescription,Title=@Title,Meta_Description=@Meta_Description,Meta_Keywords=@Meta_Keywords, MarketPrice = @MarketPrice, SaleStatus = @SaleStatus, DisplaySequence = @DisplaySequence,
ImageUrl1 = @ImageUrl1, ImageUrl2 = @ImageUrl2, ImageUrl3 = @ImageUrl3, ImageUrl4 = @ImageUrl4, ImageUrl5 = @ImageUrl5,
ThumbnailUrl40 = @ThumbnailUrl40, ThumbnailUrl60 = @ThumbnailUrl60, ThumbnailUrl100 = @ThumbnailUrl100, ThumbnailUrl160 = @ThumbnailUrl160, ThumbnailUrl180 = @ThumbnailUrl180,
ThumbnailUrl220 = @ThumbnailUrl220, ThumbnailUrl310 = @ThumbnailUrl310, ThumbnailUrl410 = @ThumbnailUrl410, BrandId = @BrandId, HasSKU = @HasSKU,IsfreeShipping=@IsfreeShipping,
VistiCounts=@VistiCounts,SaleCounts=@SaleCounts,ShowSaleCounts=@ShowSaleCounts, ReferralDeduct=@ReferralDeduct, SubMemberDeduct=@SubMemberDeduct, SubReferralDeduct=@SubReferralDeduct
WHERE ProductId = @ProductId' 
END
GO
/****** Object:  Table [dbo].[Ecshop_SKUs]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_SKUs]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_SKUs](
	[SkuId] [nvarchar](100) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[ProductId] [int] NOT NULL,
	[SKU] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Weight] [money] NULL,
	[Stock] [int] NOT NULL,
	[CostPrice] [money] NULL,
	[SalePrice] [money] NOT NULL,
 CONSTRAINT [PK_Ecshop_SKUs] PRIMARY KEY CLUSTERED 
(
	[SkuId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_SKUs]') AND name = N'Ecshop_SKUs_Index2')
CREATE NONCLUSTERED INDEX [Ecshop_SKUs_Index2] ON [dbo].[Ecshop_SKUs] 
(
	[ProductId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
GO
/****** Object:  Table [dbo].[Ecshop_SKUMemberPrice]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_SKUMemberPrice]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_SKUMemberPrice](
	[SkuId] [nvarchar](100) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[GradeId] [int] NOT NULL,
	[MemberSalePrice] [money] NOT NULL,
 CONSTRAINT [PK_Ecshop_SKUMemberPrice] PRIMARY KEY CLUSTERED 
(
	[SkuId] ASC,
	[GradeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Ecshop_SKUItems]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_SKUItems]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_SKUItems](
	[SkuId] [nvarchar](100) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[AttributeId] [int] NOT NULL,
	[ValueId] [int] NOT NULL,
 CONSTRAINT [PK_Ecshop_SKUItems] PRIMARY KEY CLUSTERED 
(
	[SkuId] ASC,
	[AttributeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Trigger [T_Ecshop_LeaveCommentReplys_Delete]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[T_Ecshop_LeaveCommentReplys_Delete]'))
EXEC dbo.sp_executesql @statement = N'CREATE   Trigger   [dbo].[T_Ecshop_LeaveCommentReplys_Delete] ON [dbo].[Ecshop_LeaveCommentReplys] FOR Delete AS
Declare @LeaveId int
BEGIN
SELECT @LeaveId=LeaveId from deleted 
	UPDATE Ecshop_LeaveComments SET IsReply=0 WHERE LeaveId=@LeaveId AND (SELECT COUNT(*) FROM Ecshop_LeaveCommentReplys WHERE LeaveId=@LeaveId)<=0	
END'
GO
/****** Object:  Trigger [T_Ecshop_LeaveCommentReplys]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[T_Ecshop_LeaveCommentReplys]'))
EXEC dbo.sp_executesql @statement = N'CREATE   Trigger   [dbo].[T_Ecshop_LeaveCommentReplys] ON [dbo].[Ecshop_LeaveCommentReplys] FOR Insert AS
BEGIN
Declare @LeaveId INT
Declare @LastDate DATETIME
SELECT @LeaveId= LeaveId, @LastDate = ReplyDate FROM inserted;
UPDATE Ecshop_LeaveComments SET IsReply = 1, LastDate = @LastDate WHERE LeaveId = @LeaveId
END'
GO
/****** Object:  Trigger [T_Ecshop_CouponItems_Update]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[T_Ecshop_CouponItems_Update]'))
EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER [dbo].[T_Ecshop_CouponItems_Update]
ON [dbo].[Ecshop_CouponItems]
FOR update
AS 
BEGIN
	DECLARE @CouponId INT
	declare @CouponStatus int
	SELECT @CouponId= CouponId,@CouponStatus=CouponStatus FROM inserted;
	if @CouponStatus=1
	begin
		UPDATE Ecshop_Coupons SET UsedCount =(select COUNT(CouponId) from dbo.Ecshop_CouponItems  WHERE CouponStatus = 1 and CouponId = @CouponId )where CouponId = @CouponId
	end
END'
GO
/****** Object:  Trigger [T_Ecshop_CouponItems_Insert]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[T_Ecshop_CouponItems_Insert]'))
EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER [dbo].[T_Ecshop_CouponItems_Insert]
ON [dbo].[Ecshop_CouponItems]
FOR INSERT
AS 
BEGIN
	DECLARE @CouponId INT
	declare @CouponStatus int
	SELECT @CouponId= CouponId,@CouponStatus=CouponStatus FROM inserted;
	if @CouponStatus=0
	begin
	UPDATE Ecshop_Coupons SET SentCount = (select COUNT(*) from dbo.Ecshop_CouponItems where CouponId = @CouponId) WHERE CouponId = @CouponId
end
END'
GO
/****** Object:  Trigger [T_Ecshop_BalanceDrawRequest_Insert]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[T_Ecshop_BalanceDrawRequest_Insert]'))
EXEC dbo.sp_executesql @statement = N'CREATE   Trigger   [dbo].[T_Ecshop_BalanceDrawRequest_Insert] ON [dbo].[Ecshop_BalanceDrawRequest] FOR Insert AS
BEGIN
Declare @UserId INT
Declare @Amount MONEY
SELECT @UserId= UserId, @Amount = Amount FROM inserted;
UPDATE aspnet_Members SET RequestBalance = @Amount WHERE UserId = @UserId
END'
GO
/****** Object:  Trigger [T_Ecshop_BalanceDrawRequest_Delete]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[T_Ecshop_BalanceDrawRequest_Delete]'))
EXEC dbo.sp_executesql @statement = N'CREATE   Trigger   [dbo].[T_Ecshop_BalanceDrawRequest_Delete] ON [dbo].[Ecshop_BalanceDrawRequest] FOR Delete AS
BEGIN
Declare @UserId INT
SELECT @UserId= UserId FROM deleted;
UPDATE aspnet_Members SET RequestBalance = 0 WHERE UserId = @UserId
END'
GO
/****** Object:  Trigger [T_Ecshop_BalanceDetails_Insert]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[T_Ecshop_BalanceDetails_Insert]'))
EXEC dbo.sp_executesql @statement = N'CREATE   Trigger   [dbo].[T_Ecshop_BalanceDetails_Insert] ON [dbo].[Ecshop_BalanceDetails] FOR Insert AS
BEGIN
Declare @UserId INT
Declare @Balance MONEY
SELECT @UserId= UserId, @Balance = Balance FROM inserted;
UPDATE aspnet_Members SET Balance = @Balance WHERE UserId = @UserId
END'
GO
/****** Object:  StoredProcedure [dbo].[ss_ShoppingCart_AddLineItem]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ss_ShoppingCart_AddLineItem]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[ss_ShoppingCart_AddLineItem]
	(
		@UserId INT,
		@SkuId NVARCHAR(100),
		@Quantity INT
	)
AS
	IF EXISTS (SELECT [SkuId] FROM Ecshop_ShoppingCarts WHERE UserId = @UserId AND SkuId = @SkuId)
	BEGIN
		UPDATE 
				Ecshop_ShoppingCarts 
		SET 
				Quantity = Quantity + @Quantity
		WHERE 
				UserId = @UserId AND SkuId = @SkuId
	END
	ELSE
	BEGIN
		INSERT INTO Ecshop_ShoppingCarts 
			(UserId, SkuId, Quantity) 
		VALUES 
			(@UserId, @SkuId, @Quantity)
	END' 
END
GO
/****** Object:  StoredProcedure [dbo].[ss_LeaveComments_Get]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ss_LeaveComments_Get]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[ss_LeaveComments_Get]
(
	@PageIndex int,
	@PageSize int,
	@IsCount bit,
	@sqlPopulate ntext,
	@Total int = 0 output
)
AS
SET Transaction Isolation Level Read UNCOMMITTED

BEGIN
	DECLARE @PageLowerBound int
	DECLARE @PageUpperBound int

	-- Set the page bounds
	SET @PageLowerBound = @PageSize * (@PageIndex - 1)
	SET @PageUpperBound = @PageLowerBound + @PageSize + 1

	CREATE TABLE #PageIndexForSearch
	(
		IndexId INT IDENTITY (1, 1) NOT NULL,
		LeaveId BIGINT
	)

	INSERT INTO #PageIndexForSearch (LeaveId) 
    Exec sp_executesql @sqlPopulate
    
    SET @Total = @@rowcount
    
		-- 第一层
	SELECT l.*
	FROM 
	 Ecshop_LeaveComments l ,#PageIndexForSearch
	WHERE 
		l.LeaveId = #PageIndexForSearch.LeaveId AND
		#PageIndexForSearch.IndexId > @PageLowerBound AND
		#PageIndexForSearch.IndexId < @PageUpperBound
	ORDER BY
		#PageIndexForSearch.IndexId
	
	-- 第二层
	SELECT r.*
		FROM Ecshop_LeaveCommentReplys r,#PageIndexForSearch
	WHERE
		R.LeaveId = #PageIndexForSearch.LeaveId
		order by ReplyDate desc
		
	drop table #PageIndexForSearch	

END' 
END
GO
/****** Object:  Trigger [T_Ecshop_PointDetails_Insert]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[T_Ecshop_PointDetails_Insert]'))
EXEC dbo.sp_executesql @statement = N'Create   Trigger   [dbo].[T_Ecshop_PointDetails_Insert] ON [dbo].[Ecshop_PointDetails] FOR Insert AS
BEGIN
Declare @UserId INT
Declare @Points MONEY
SELECT @UserId= UserId, @Points = Points FROM inserted;
UPDATE aspnet_Members SET Points = @Points WHERE UserId = @UserId
END'
GO
/****** Object:  View [dbo].[vw_Ecshop_SaleDetails]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_Ecshop_SaleDetails]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [dbo].[vw_Ecshop_SaleDetails]
AS
SELECT oi.OrderId, oi.ItemDescription AS ProductName, oi.Quantity, oi.ItemAdjustedPrice, o.orderDate, o.OrderStatus 
FROM Ecshop_OrderItems oi join Ecshop_orders o on oi.OrderId = o.OrderId'
GO
/****** Object:  View [dbo].[vw_Ecshop_ProductSkuList]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_Ecshop_ProductSkuList]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [dbo].[vw_Ecshop_ProductSkuList]
AS
SELECT CategoryId, BrandId,ProductName,ProductCode,MarketPrice,ThumbnailUrl40, ThumbnailUrl60,
 SaleStatus, DisplaySequence, MainCategoryPath, ExtendCategoryPath, AddedDate, VistiCounts, s.*
FROM Ecshop_Products p JOIN Ecshop_SKUs s ON p.ProductId = s.ProductId'
GO
/****** Object:  View [dbo].[vw_Ecshop_ProductReviews]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_Ecshop_ProductReviews]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [dbo].[vw_Ecshop_ProductReviews]
AS
SELECT     p.ProductId, p.ProductCode, p.ProductName, p.CategoryId, r.ReviewId, r.ReviewText, r.ReviewDate, r.UserId, r.UserName, p.ThumbnailUrl40, p.ThumbnailUrl60, p.ThumbnailUrl100, 
                      p.ThumbnailUrl160, p.ThumbnailUrl180, p.ThumbnailUrl220, p.ThumbnailUrl310, p.ThumbnailUrl410
FROM         dbo.Ecshop_Products AS p WITH (nolock) INNER JOIN
                      dbo.Ecshop_ProductReviews AS r ON r.ProductId = p.ProductId
'
GO
/****** Object:  View [dbo].[vw_Ecshop_ProductConsultations]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_Ecshop_ProductConsultations]'))
EXEC dbo.sp_executesql @statement = N'
CREATE VIEW [dbo].[vw_Ecshop_ProductConsultations]
AS
SELECT
	  p.[ProductId]
      ,p.[ProductName]     
	  ,p.[ProductCode]
      ,p.[ThumbnailUrl40] 
	  ,p.[CategoryId]
      ,c.[ConsultationId]
	  ,c.[ConsultationText]
      ,c.[ConsultationDate]
      ,c.[ReplyText]
      ,c.[UserName]
      ,c.[ReplyUserId]
      ,c.[Userid]
	  ,c.[ReplyDate]
FROM Ecshop_Products p inner join Ecshop_ProductConsultations c ON p.productId=c.ProductId
'
GO
/****** Object:  View [dbo].[vw_Ecshop_OrderSendNote]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_Ecshop_OrderSendNote]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [dbo].[vw_Ecshop_OrderSendNote]
as
select a.NoteId,b.OrderTotal,a.OrderId,b.PaymentType,b.ShippingDate,b.ExpressCompanyName,b.ZipCode,b.TelPhone,b.CellPhone,
b.ShipOrderNumber,b.ShipTo,b.ShippingRegion,a.Operator,b.Username,a.Remark
from Ecshop_OrderSendNote a inner join Ecshop_Orders b on a.OrderId=b.OrderId'
GO
/****** Object:  View [dbo].[vw_Ecshop_OrderReturns]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_Ecshop_OrderReturns]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [dbo].[vw_Ecshop_OrderReturns]
AS
SELECT a.ReturnsId,a.OrderId,a.Operator,b.Username,a.RefundMoney,a.ApplyForTime,a.AdminRemark,a.Comments,a.HandleStatus, OrderStatus,b.UserId,
CASE a.RefundType WHEN 1 THEN ''预存款'' ELSE ''银行转帐'' END AS RefundType, a.HandleTime
FROM Ecshop_OrderReturns a INNER JOIN Ecshop_Orders b on a.OrderId=b.OrderId;'
GO
/****** Object:  View [dbo].[vw_Ecshop_OrderReplace]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_Ecshop_OrderReplace]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [dbo].[vw_Ecshop_OrderReplace]
AS
SELECT a.ReplaceId,a.OrderId,b.Username,b.OrderTotal,a.ApplyForTime,a.Comments,a.HandleStatus,b.OrderStatus,b.UserId,a.HandleTime, a.AdminRemark
FROM Ecshop_OrderReplace a INNER JOIN Ecshop_Orders b on a.OrderId=b.OrderId;'
GO
/****** Object:  View [dbo].[vw_Ecshop_OrderRefund]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_Ecshop_OrderRefund]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [dbo].[vw_Ecshop_OrderRefund]
AS
SELECT a.RefundId,a.OrderId,a.Operator,a.RefundRemark,b.Username,b.OrderTotal,a.ApplyForTime,a.HandleTime,
a.AdminRemark,a.HandleStatus,b.OrderStatus,b.UserId,CASE a.RefundType WHEN 1 THEN ''预存款'' ELSE ''银行转帐'' END AS RefundType
FROM Ecshop_OrderRefund a INNER JOIN Ecshop_Orders b on a.OrderId=b.OrderId;'
GO
/****** Object:  View [dbo].[vw_Ecshop_OrderItem]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_Ecshop_OrderItem]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [dbo].[vw_Ecshop_OrderItem]
AS
select top 100 percent items.*,orders.PayDate,orders.Username,orders.ShipTo from dbo.Ecshop_OrderItems as items
left join Ecshop_Orders orders on items.OrderId=orders.OrderId where orders.OrderStatus!=1 and orders.OrderStatus!=4
order by orders.PayDate desc'
GO
/****** Object:  View [dbo].[vw_Ecshop_OrderDebitNote]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_Ecshop_OrderDebitNote]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [dbo].[vw_Ecshop_OrderDebitNote]
as
select a.NoteId,a.OrderId,b.Username,b.OrderTotal,b.PayCharge,b.PaymentType,a.Operator,b.PayDate,a.Remark
from Ecshop_OrderDebitNote a inner join Ecshop_Orders b on a.OrderId=b.OrderId'
GO
/****** Object:  View [dbo].[vw_Ecshop_BrowseProductList]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_Ecshop_BrowseProductList]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [dbo].[vw_Ecshop_BrowseProductList]
AS
SELECT CategoryId, TypeId,BrandId,ProductId,ProductName,ProductCode,ShortDescription,MarketPrice,Unit,
ThumbnailUrl40,ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160, ThumbnailUrl180, ThumbnailUrl220,ThumbnailUrl310,
 SaleStatus, DisplaySequence, MainCategoryPath, ExtendCategoryPath, SaleCounts, ShowSaleCounts,UpdateDate, AddedDate, VistiCounts,[Description],
 ImageUrl1,ImageUrl2,ImageUrl3,ImageUrl4,ImageUrl5,ISNULL(TaobaoProductId, 0) AS TaobaoProductId,
(SELECT MIN(SalePrice) FROM Ecshop_SKUs WHERE ProductId = p.ProductId) AS SalePrice,
(SELECT MIN(CostPrice) FROM Ecshop_SKUs WHERE ProductId = p.ProductId) AS CostPrice,
(SELECT TOP 1 SkuId FROM Ecshop_SKUs WHERE ProductId = p.ProductId ORDER BY SalePrice) AS SkuId,
(SELECT SUM(Stock) FROM Ecshop_SKUs WHERE ProductId = p.ProductId) AS Stock,
(SELECT TOP 1 [Weight] FROM Ecshop_SKUs WHERE ProductId = p.ProductId ORDER BY SalePrice) AS [Weight],
(SELECT COUNT(*) FROM Taobao_Products WHERE ProductId = p.ProductId) AS IsMakeTaobao
FROM Ecshop_Products p'
GO
/****** Object:  View [dbo].[vw_Ecshop_Articles]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_Ecshop_Articles]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [dbo].[vw_Ecshop_Articles]
AS
SELECT     a.ArticleId, a.Title, a.Meta_Description, a.Meta_Keywords, a.IconUrl,a.[Content], AddedDate, a.Description,a.IsRelease ,[Name], ac.CategoryId
FROM dbo.Ecshop_Articles AS a INNER JOIN dbo.Ecshop_ArticleCategories AS ac ON a.CategoryId = ac.CategoryId'
GO
/****** Object:  View [dbo].[vw_Ecshop_CouponInfo]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_Ecshop_CouponInfo]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [dbo].[vw_Ecshop_CouponInfo]
	AS 
	SELECT     dbo.Ecshop_CouponItems.LotNumber, dbo.Ecshop_CouponItems.ClaimCode, dbo.Ecshop_CouponItems.UserId, dbo.Ecshop_CouponItems.UserName, 
                      dbo.Ecshop_CouponItems.EmailAddress, dbo.Ecshop_CouponItems.GenerateTime, dbo.Ecshop_CouponItems.CouponStatus, dbo.Ecshop_CouponItems.UsedTime, 
                      dbo.Ecshop_CouponItems.OrderId, dbo.Ecshop_CouponItems.CouponId, dbo.Ecshop_Coupons.Name,dbo.Ecshop_Coupons.ClosingTime
FROM         dbo.Ecshop_CouponItems INNER JOIN
                      dbo.Ecshop_Coupons ON dbo.Ecshop_CouponItems.CouponId = dbo.Ecshop_Coupons.CouponId'
GO
/****** Object:  View [dbo].[vw_Ecshop_Helps]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_Ecshop_Helps]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [dbo].[vw_Ecshop_Helps]
AS
SELECT HelpId, Title, AddedDate, h.IsShowFooter, h.Description, [Name], hc.CategoryId
FROM dbo.Ecshop_Helps AS h INNER JOIN dbo.Ecshop_HelpCategories AS hc ON h.CategoryId = hc.CategoryId'
GO
/****** Object:  Table [dbo].[Ecshop_ProductAttributes]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_ProductAttributes]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_ProductAttributes](
	[ProductId] [int] NOT NULL,
	[AttributeId] [int] NOT NULL,
	[ValueId] [int] NOT NULL,
 CONSTRAINT [PK_Ecshop_ProductAttributes] PRIMARY KEY CLUSTERED 
(
	[ProductId] ASC,
	[AttributeId] ASC,
	[ValueId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Ecshop_GroupBuyCondition]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_GroupBuyCondition]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_GroupBuyCondition](
	[GroupBuyId] [int] NOT NULL,
	[Count] [int] NOT NULL,
	[Price] [money] NOT NULL,
 CONSTRAINT [PK_Ecshop_GroupBuyCondition] PRIMARY KEY NONCLUSTERED 
(
	[GroupBuyId] ASC,
	[Count] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Roles_DeleteRole]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Roles_DeleteRole]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[aspnet_Roles_DeleteRole]
    @RoleName                   nvarchar(256),
    @DeleteOnlyIfRoleIsEmpty    bit
AS
BEGIN

    DECLARE @ErrorCode     int
    SET @ErrorCode = 0

    DECLARE @TranStarted   bit
    SET @TranStarted = 0

    IF( @@TRANCOUNT = 0 )
    BEGIN
        BEGIN TRANSACTION
        SET @TranStarted = 1
    END
    ELSE
        SET @TranStarted = 0

    DECLARE @RoleId   uniqueidentifier
    SELECT  @RoleId = NULL
    SELECT  @RoleId = RoleId FROM dbo.aspnet_Roles WHERE LoweredRoleName = LOWER(@RoleName)

    IF (@RoleId IS NULL)
    BEGIN
        SELECT @ErrorCode = 1
        GOTO Cleanup
    END
    IF (@DeleteOnlyIfRoleIsEmpty <> 0)
    BEGIN
        IF (EXISTS (SELECT RoleId FROM dbo.aspnet_UsersInRoles  WHERE @RoleId = RoleId))
        BEGIN
            SELECT @ErrorCode = 2
            GOTO Cleanup
        END
    END


    DELETE FROM dbo.aspnet_UsersInRoles  WHERE @RoleId = RoleId

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    DELETE FROM dbo.aspnet_Roles WHERE @RoleId = RoleId

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
        COMMIT TRANSACTION
    END

    RETURN(0)

Cleanup:

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
        ROLLBACK TRANSACTION
    END

    RETURN @ErrorCode
END' 
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_DeleteUser]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Membership_DeleteUser]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[aspnet_Membership_DeleteUser]
	(
		@UserName nvarchar(256),
		@NumTablesDeletedFrom int output
	)
AS
	DECLARE @UserId INT
	SELECT @UserId = UserId FROM aspnet_Users WHERE LOWER(@UserName) = LoweredUserName
	SELECT @NumTablesDeletedFrom = 0
	
	IF @UserId IS NOT NULL
	BEGIN
		DELETE FROM aspnet_UsersInRoles WHERE UserId =  @UserId
		
		IF (@@ROWCOUNT <> 0)
			SELECT @NumTablesDeletedFrom = @NumTablesDeletedFrom + 1
			
		DELETE FROM aspnet_Users WHERE UserId = @UserId
		
		IF (@@ROWCOUNT <> 0)
			SELECT @NumTablesDeletedFrom = @NumTablesDeletedFrom + 1
	END' 
END
GO
/****** Object:  StoredProcedure [dbo].[ac_Member_UserReviewsAndReplys_Get]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ac_Member_UserReviewsAndReplys_Get]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[ac_Member_UserReviewsAndReplys_Get]
(
	@PageIndex int,
	@PageSize int,
	@IsCount bit,
	@UserId int,
	@sqlPopulate ntext,
	@Total int = 0 output
)
AS
SET Transaction Isolation Level Read UNCOMMITTED

BEGIN
	DECLARE @PageLowerBound int
	DECLARE @PageUpperBound int

	-- Set the page bounds
	SET @PageLowerBound = @PageSize * (@PageIndex-1)
	SET @PageUpperBound = @PageLowerBound + @PageSize + 1

	CREATE TABLE #PageIndexForReviewProducts
	(
		[IndexId] int IDENTITY (1, 1) NOT NULL,
		[ProductId] int,
	)	

	INSERT INTO #PageIndexForReviewProducts (ProductId) 
    Exec sp_executesql @sqlPopulate
 
	SET @Total = @@rowcount

	SELECT pt.ProductId,
		(SELECT ProductName   FROM Ecshop_Products WHERE ProductId = pt.ProductId) AS ProductName,
		(SELECT ThumbnailUrl100   FROM Ecshop_Products WHERE ProductId = pt.ProductId) AS ProductImage,
		(SELECT TOP 1 ReviewDate FROM Ecshop_ProductReviews 
			WHERE ProductId = pt.ProductId 
			ORDER BY ReviewDate DESC ) AS ReviewLastDate
	FROM Ecshop_ProductReviews pt, #PageIndexForReviewProducts
	WHERE 
		pt.UserId = @UserId AND 
		pt.ProductId = #PageIndexForReviewProducts.ProductId AND
		#PageIndexForReviewProducts.IndexId > @PageLowerBound AND
		#PageIndexForReviewProducts.IndexId < @PageUpperBound 
	GROUP BY pt.ProductId
	ORDER BY ReviewLastDate desc	
	

	drop table #PageIndexForReviewProducts
	
	SELECT *
	FROM Ecshop_ProductReviews
	WHERE UserId = @UserId
	ORDER BY  ReviewDate DESC

END' 
END
GO
/****** Object:  StoredProcedure [dbo].[ac_Member_ShippingAddress_CreateUpdateDelete]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ac_Member_ShippingAddress_CreateUpdateDelete]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'Create procedure [dbo].[ac_Member_ShippingAddress_CreateUpdateDelete]
(
   @RegionId	int = null,
   @ShippingId int= null,
   @UserId	int = null,
   @ShipTo	nvarchar(50) = null,
   @Address	nvarchar(500) = null,
   @Zipcode	nvarchar(20) = null,	
   @TelPhone	nvarchar(20) = null,
   @CellPhone	nvarchar(20) = null,
   @Action INT,
   @IsDefault bit=0,
   @Status INT OUTPUT
)
as 
  SET @Status = 99
  IF @Action = 2 -- 删除
  BEGIN 
     DELETE FROM Ecshop_UserShippingAddresses
     WHERE  ShippingId = @ShippingId
     IF @@ROWCOUNT = 1
		    SET @Status = 0
     RETURN
  END
 
 IF @Action = 0 -- 创建
 BEGIN
   UPDATE Ecshop_UserShippingAddresses SET IsDefault=0 WHERE UserId=@UserId
   INSERT INTO Ecshop_UserShippingAddresses(RegionId,UserId,ShipTo,Address,Zipcode,TelPhone,CellPhone,IsDefault)
   VALUES(@RegionId,@UserId,@ShipTo,@Address,@Zipcode,@TelPhone,@CellPhone,@IsDefault)
   IF @@ROWCOUNT = 1
			SET @Status = 0	
   RETURN
 END 
 
IF @Action = 1 --修改
 BEGIN 
  UPDATE Ecshop_UserShippingAddresses
  set 
    RegionId = @RegionId,UserId= @UserId,ShipTo =@ShipTo,
    [Address] = @Address,Zipcode = @Zipcode,TelPhone =@TelPhone,
    CellPhone = @CellPhone,IsDefault=@IsDefault
    WHERE ShippingId = @ShippingId
   IF @@ROWCOUNT = 1
	 SET @Status = 0		
	RETURN
 END' 
END
GO
/****** Object:  StoredProcedure [dbo].[ac_Member_InpourRequest_Create]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ac_Member_InpourRequest_Create]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[ac_Member_InpourRequest_Create]
	(
		@InpourId NVARCHAR(50),
		@TradeDate DATETIME,
		@InpourBlance MONEY,
		@UserId INT,
		@PaymentId NVARCHAR(50),
		@Status INT = 99 OUTPUT
	)
AS
	-- 添加一条预付款充值记录	
	BEGIN
		INSERT INTO Ecshop_InpourRequest VALUES(@InpourId,@TradeDate,@InpourBlance,@UserId,@PaymentId)
		
		IF @@ROWCOUNT = 1
			SET @Status = 0	 
	END' 
END
GO
/****** Object:  StoredProcedure [dbo].[cp_BalanceDrawRequest_Update]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cp_BalanceDrawRequest_Update]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[cp_BalanceDrawRequest_Update]
(
	@UserId int,
	@Agree BIT,
	@Status INT = 0 OUTPUT 
)
AS

DECLARE @intErrorCode int
DECLARE @OldBalance money
DECLARE @NewBalance money

DECLARE @DealTime datetime
DECLARE @DealAmount money
DECLARE @UserName [nvarchar] (256)
DECLARE @Remark [nvarchar] (2000)

SELECT 
	@Status = 99, @intErrorCode = 0, @DealTime = RequestTime, @DealAmount = Amount, @UserName = UserName, @Remark = Remark
FROM Ecshop_BalanceDrawRequest WHERE UserId = @UserId

SET XACT_ABORT ON

BEGIN TRAN
	-- 更改提现申请状态
	IF @intErrorCode = 0
	BEGIN
		DELETE FROM Ecshop_BalanceDrawRequest  WHERE  UserId = @UserId
		SET @intErrorCode = @@ERROR
	END

	-- 添加相应的预付款明细记录
	IF @intErrorCode = 0 AND @Agree = 1
	BEGIN
		SELECT @OldBalance = Balance FROM aspnet_Members WHERE UserId = @UserId 
		SET @NewBalance = @OldBalance - @DealAmount
		
		INSERT INTO Ecshop_BalanceDetails (UserId,UserName, TradeDate, TradeType, Expenses, Balance, Remark)
		VALUES (@UserId,@UserName, getdate(), 4, @DealAmount, @NewBalance, @Remark)
		SET @intErrorCode = @@ERROR
	END
	
	IF @intErrorCode = 0
	BEGIN
		SET @Status = 0
		COMMIT TRAN
	END
	ELSE
		ROLLBACK TRAN' 
END
GO
/****** Object:  StoredProcedure [dbo].[ac_Member_ConsultationsAndReplys_Get]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ac_Member_ConsultationsAndReplys_Get]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[ac_Member_ConsultationsAndReplys_Get]
(
	@PageIndex int,
	@PageSize int,
	@IsCount bit,
	@sqlPopulate ntext,
	@Total int = 0 output
)
AS
SET Transaction Isolation Level Read UNCOMMITTED

BEGIN
	DECLARE @PageLowerBound int
	DECLARE @PageUpperBound int

	-- Set the page bounds
	SET @PageLowerBound = @PageSize * (@PageIndex-1)
	SET @PageUpperBound = @PageLowerBound + @PageSize + 1

	CREATE TABLE #PageIndexForReviews
	(
		[IndexId] int IDENTITY (1, 1) NOT NULL,
		[ConsultationId] int
	)	

	INSERT INTO #PageIndexForReviews (ConsultationId) 
    Exec sp_executesql @sqlPopulate

	SET @Total = @@rowcount

	-- 评论记录
	SELECT pr.ConsultationId,pr.ProductId,pr.ConsultationText,pr.ConsultationDate,
		#PageIndexForReviews.IndexId,pr.UserName,pr.UserId,
		(select ThumbnailUrl40 from Ecshop_Products where ProductId = pr.ProductId) as ThumbnailUrl40,
		(select ProductName from Ecshop_Products where ProductId = pr.ProductId) as ProductName 
	
	FROM Ecshop_ProductConsultations pr, #PageIndexForReviews
	WHERE  
		pr.ConsultationId = #PageIndexForReviews.ConsultationId AND
		#PageIndexForReviews.IndexId > @PageLowerBound AND
		#PageIndexForReviews.IndexId < @PageUpperBound 
	ORDER BY ReplyDate DESC

	drop table #PageIndexForReviews
	-- 回复记录
	SELECT ConsultationId,ReplyText,ReplyDate,UserId 
	FROM Ecshop_ProductConsultations pc
	ORDER BY ReplyDate DESC	
	
END' 
END
GO
/****** Object:  StoredProcedure [dbo].[cp_API_Orders_Get]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cp_API_Orders_Get]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[cp_API_Orders_Get]
(
	@PageIndex int,
	@PageSize int,
	@IsCount bit,
	@sqlPopulate ntext,
	@TotalOrders int = 0 output
)
AS
	SET Transaction Isolation Level Read UNCOMMITTED

BEGIN
	DECLARE @PageLowerBound int
	DECLARE @PageUpperBound int

	-- Set the page bounds
	SET @PageLowerBound = @PageSize * (@PageIndex-1)
	SET @PageUpperBound = @PageLowerBound + @PageSize + 1

	CREATE TABLE #PageIndexForOrders
	(
		IndexId int IDENTITY (1, 1) NOT NULL,
		OrderId nvarchar(50)
	)	

	INSERT INTO #PageIndexForOrders(OrderId)
	Exec sp_executesql @sqlPopulate

	SET @TotalOrders = @@rowcount
	
	SELECT o.OrderId, 0 as SellerUid,Username,EmailAddress,ShipTo,
ShippingRegion,Address,ZipCode,CellPhone,TelPhone,Remark,ManagerMark,ManagerRemark,
(select sum(Quantity) from Ecshop_OrderItems where Ecshop_OrderItems.OrderId=o.OrderId) as Nums,
 OrderTotal,AdjustedFreight,DiscountValue,AdjustedDiscount,PaymentTypeId,PaymentType,PayDate,ShippingDate,''0.00'' as DiscountValue,OrderDate,UpdateDate,
ReFundStatus,RefundAmount,RefundRemark,OrderStatus,ModeName,Gateway from Ecshop_Orders as o, #PageIndexForOrders
	WHERE 
		o.OrderId = #PageIndexForOrders.OrderId AND
		#PageIndexForOrders.IndexId > @PageLowerBound AND
		#PageIndexForOrders.IndexId < @PageUpperBound 
	ORDER BY #PageIndexForOrders.IndexId;
	SELECT 0 as Tid,OrderId,ProductId,ItemDescription,SKU,SkuId,SKUContent,Quantity,ItemListPrice,ItemAdjustedPrice,
	''0.00'' as DiscountFee,''0.00'' as Fee,ThumbnailsUrl from 
	Ecshop_OrderItems where OrderId in (SELECT OrderId FROM #PageIndexForOrders WHERE IndexId > @PageLowerBound AND
		IndexId < @PageUpperBound)

	drop table #PageIndexForOrders
END' 
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_UsersInRoles_RemoveUsersFromRoles]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_UsersInRoles_RemoveUsersFromRoles]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[aspnet_UsersInRoles_RemoveUsersFromRoles]
	@UserNames		  nvarchar(4000),
	@RoleNames		  nvarchar(4000)
AS
BEGIN

	DECLARE @TranStarted   bit
	SET @TranStarted = 0

	IF( @@TRANCOUNT = 0 )
	BEGIN
		BEGIN TRANSACTION
		SET @TranStarted = 1
	END

	DECLARE @tbNames  table([Name] nvarchar(256) NOT NULL PRIMARY KEY)
	DECLARE @tbRoles  table(RoleId uniqueidentifier NOT NULL PRIMARY KEY)
	DECLARE @tbUsers  table(UserId int NOT NULL PRIMARY KEY)
	DECLARE @Num	  int
	DECLARE @Pos	  int
	DECLARE @NextPos  int
	DECLARE @Name	  nvarchar(256)
	DECLARE @CountAll int
	DECLARE @CountU	  int
	DECLARE @CountR	  int


	SET @Num = 0
	SET @Pos = 1
	WHILE(@Pos <= LEN(@RoleNames))
	BEGIN
		SELECT @NextPos = CHARINDEX(N'','', @RoleNames,  @Pos)
		IF (@NextPos = 0 OR @NextPos IS NULL)
			SELECT @NextPos = LEN(@RoleNames) + 1
		SELECT @Name = RTRIM(LTRIM(SUBSTRING(@RoleNames, @Pos, @NextPos - @Pos)))
		SELECT @Pos = @NextPos+1

		INSERT INTO @tbNames VALUES (@Name)
		SET @Num = @Num + 1
	END

	INSERT INTO @tbRoles
	  SELECT RoleId
	  FROM   dbo.aspnet_Roles ar, @tbNames t
	  WHERE  LOWER(t.Name) = ar.LoweredRoleName
	SELECT @CountR = @@ROWCOUNT

	IF (@CountR <> @Num)
	BEGIN
		SELECT TOP 1 N'''', [Name]
		FROM   @tbNames
		WHERE  LOWER([Name]) NOT IN (SELECT ar.LoweredRoleName FROM dbo.aspnet_Roles ar,  @tbRoles r WHERE r.RoleId = ar.RoleId)
		IF( @TranStarted = 1 )
			ROLLBACK TRANSACTION
		RETURN(2)
	END


	DELETE FROM @tbNames WHERE 1=1
	SET @Num = 0
	SET @Pos = 1


	WHILE(@Pos <= LEN(@UserNames))
	BEGIN
		SELECT @NextPos = CHARINDEX(N'','', @UserNames,  @Pos)
		IF (@NextPos = 0 OR @NextPos IS NULL)
			SELECT @NextPos = LEN(@UserNames) + 1
		SELECT @Name = RTRIM(LTRIM(SUBSTRING(@UserNames, @Pos, @NextPos - @Pos)))
		SELECT @Pos = @NextPos+1

		INSERT INTO @tbNames VALUES (@Name)
		SET @Num = @Num + 1
	END

	INSERT INTO @tbUsers
	  SELECT UserId
	  FROM   dbo.aspnet_Users ar, @tbNames t
	  WHERE  LOWER(t.Name) = ar.LoweredUserName

	SELECT @CountU = @@ROWCOUNT
	IF (@CountU <> @Num)
	BEGIN
		SELECT TOP 1 [Name], N''''
		FROM   @tbNames
		WHERE  LOWER([Name]) NOT IN (SELECT au.LoweredUserName FROM dbo.aspnet_Users au,  @tbUsers u WHERE u.UserId = au.UserId)

		IF( @TranStarted = 1 )
			ROLLBACK TRANSACTION
		RETURN(1)
	END

	SELECT  @CountAll = COUNT(*)
	FROM	dbo.aspnet_UsersInRoles ur, @tbUsers u, @tbRoles r
	WHERE   ur.UserId = u.UserId AND ur.RoleId = r.RoleId

	IF (@CountAll <> @CountU * @CountR)
	BEGIN
		SELECT TOP 1 UserName, RoleName
		FROM		 @tbUsers tu, @tbRoles tr, dbo.aspnet_Users u, dbo.aspnet_Roles r
		WHERE		 u.UserId = tu.UserId AND r.RoleId = tr.RoleId AND
					 tu.UserId NOT IN (SELECT ur.UserId FROM dbo.aspnet_UsersInRoles ur WHERE ur.RoleId = tr.RoleId) AND
					 tr.RoleId NOT IN (SELECT ur.RoleId FROM dbo.aspnet_UsersInRoles ur WHERE ur.UserId = tu.UserId)
		IF( @TranStarted = 1 )
			ROLLBACK TRANSACTION
		RETURN(3)
	END

	DELETE FROM dbo.aspnet_UsersInRoles
	WHERE UserId IN (SELECT UserId FROM @tbUsers)
	  AND RoleId IN (SELECT RoleId FROM @tbRoles)
	IF( @TranStarted = 1 )
		COMMIT TRANSACTION
	RETURN(0)
END' 
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_UsersInRoles_IsUserInRole]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_UsersInRoles_IsUserInRole]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[aspnet_UsersInRoles_IsUserInRole]
    @UserName         nvarchar(256),
    @RoleName         nvarchar(256)
AS
BEGIN

    DECLARE @UserId int
    SELECT  @UserId = NULL
    DECLARE @RoleId uniqueidentifier
    SELECT  @RoleId = NULL

    SELECT  @UserId = UserId
    FROM    dbo.aspnet_Users
    WHERE   LoweredUserName = LOWER(@UserName)

    IF (@UserId IS NULL)
        RETURN(2)

    SELECT  @RoleId = RoleId
    FROM    dbo.aspnet_Roles
    WHERE   LoweredRoleName = LOWER(@RoleName)

    IF (@RoleId IS NULL)
        RETURN(3)

    IF (EXISTS( SELECT * FROM dbo.aspnet_UsersInRoles WHERE  UserId = @UserId AND RoleId = @RoleId))
        RETURN(1)
    ELSE
        RETURN(0)
END' 
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_UsersInRoles_GetUsersInRoles]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_UsersInRoles_GetUsersInRoles]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[aspnet_UsersInRoles_GetUsersInRoles]
    @RoleName         nvarchar(256)
AS
BEGIN

     DECLARE @RoleId uniqueidentifier
     SELECT  @RoleId = NULL

     SELECT  @RoleId = RoleId
     FROM    dbo.aspnet_Roles
     WHERE   LOWER(@RoleName) = LoweredRoleName

     IF (@RoleId IS NULL)
         RETURN(1)

    SELECT u.UserName
    FROM   dbo.aspnet_Users u, dbo.aspnet_UsersInRoles ur
    WHERE  u.UserId = ur.UserId AND @RoleId = ur.RoleId
    ORDER BY u.UserName
    RETURN(0)
END' 
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_UsersInRoles_GetRolesForUser]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_UsersInRoles_GetRolesForUser]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[aspnet_UsersInRoles_GetRolesForUser]
    @UserName         nvarchar(256)
AS
BEGIN

    DECLARE @UserId int
    SELECT  @UserId = NULL

    SELECT  @UserId = UserId
    FROM    dbo.aspnet_Users
    WHERE   LoweredUserName = LOWER(@UserName)
    
    IF (@UserId IS NULL)
        RETURN(1)

    SELECT r.RoleName
    FROM   dbo.aspnet_Roles r, dbo.aspnet_UsersInRoles ur
    WHERE  r.RoleId = ur.RoleId AND ur.UserId = @UserId
    ORDER BY r.RoleName
    RETURN (0)
END' 
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_UsersInRoles_FindUsersInRole]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_UsersInRoles_FindUsersInRole]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[aspnet_UsersInRoles_FindUsersInRole]
    @RoleName         nvarchar(256),
    @UserNameToMatch  nvarchar(256)
AS
BEGIN

     DECLARE @RoleId uniqueidentifier
     SELECT  @RoleId = NULL

     SELECT  @RoleId = RoleId
     FROM    dbo.aspnet_Roles
     WHERE   LOWER(@RoleName) = LoweredRoleName

     IF (@RoleId IS NULL)
         RETURN(1)

    SELECT u.UserName
    FROM   dbo.aspnet_Users u, dbo.aspnet_UsersInRoles ur
    WHERE  u.UserId = ur.UserId AND @RoleId = ur.RoleId AND LoweredUserName LIKE LOWER(@UserNameToMatch)
    ORDER BY u.UserName
    RETURN(0)
END' 
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_UsersInRoles_AddUsersToRoles]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_UsersInRoles_AddUsersToRoles]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[aspnet_UsersInRoles_AddUsersToRoles]
	@UserNames		  nvarchar(4000),
	@RoleNames		  nvarchar(4000),
	@CurrentTime   datetime
AS
BEGIN

	DECLARE @TranStarted   bit
	SET @TranStarted = 0

	IF( @@TRANCOUNT = 0 )
	BEGIN
		BEGIN TRANSACTION
		SET @TranStarted = 1
	END

	DECLARE @tbNames	table([Name] nvarchar(256) NOT NULL PRIMARY KEY)
	DECLARE @tbRoles	table(RoleId uniqueidentifier NOT NULL PRIMARY KEY)
	DECLARE @tbUsers	table(UserId int NOT NULL PRIMARY KEY)
	DECLARE @Num		int
	DECLARE @Pos		int
	DECLARE @NextPos	int
	DECLARE @Name		nvarchar(256)

	SET @Num = 0
	SET @Pos = 1
	WHILE(@Pos <= LEN(@RoleNames))
	BEGIN
		SELECT @NextPos = CHARINDEX(N'','', @RoleNames,  @Pos)
		IF (@NextPos = 0 OR @NextPos IS NULL)
			SELECT @NextPos = LEN(@RoleNames) + 1
		SELECT @Name = RTRIM(LTRIM(SUBSTRING(@RoleNames, @Pos, @NextPos - @Pos)))
		SELECT @Pos = @NextPos+1

		INSERT INTO @tbNames VALUES (@Name)
		SET @Num = @Num + 1
	END

	INSERT INTO @tbRoles
	  SELECT RoleId
	  FROM   dbo.aspnet_Roles ar, @tbNames t
	  WHERE  LOWER(t.Name) = ar.LoweredRoleName

	--IF (@@ROWCOUNT <> @Num)
	--BEGIN
		--SELECT TOP 1 [Name]
		--FROM   @tbNames
		--WHERE  LOWER([Name]) NOT IN (SELECT ar.LoweredRoleName FROM dbo.aspnet_Roles ar,  @tbRoles r WHERE r.RoleId = ar.RoleId)
		--IF( @TranStarted = 1 )
			--ROLLBACK TRANSACTION
		--RETURN(2)
	--END

	DELETE FROM @tbNames WHERE 1=1
	SET @Num = 0
	SET @Pos = 1

	WHILE(@Pos <= LEN(@UserNames))
	BEGIN
		SELECT @NextPos = CHARINDEX(N'','', @UserNames,  @Pos)
		IF (@NextPos = 0 OR @NextPos IS NULL)
			SELECT @NextPos = LEN(@UserNames) + 1
		SELECT @Name = RTRIM(LTRIM(SUBSTRING(@UserNames, @Pos, @NextPos - @Pos)))
		SELECT @Pos = @NextPos+1

		INSERT INTO @tbNames VALUES (@Name)
		SET @Num = @Num + 1
	END

	INSERT INTO @tbUsers
	  SELECT UserId
	  FROM   dbo.aspnet_Users ar, @tbNames t
	  WHERE  LOWER(t.Name) = ar.LoweredUserName

	--IF (@@ROWCOUNT <> @Num)
	--BEGIN
		--DELETE FROM @tbNames
		--WHERE LOWER([Name]) IN (SELECT LoweredUserName FROM dbo.aspnet_Users au,  @tbUsers u WHERE au.UserId = u.UserId)

		--INSERT dbo.aspnet_Users (UserId, UserName, LoweredUserName, IsAnonymous, LastActivityDate)
		  --SELECT NEWID(), [Name], LOWER([Name]), 0, @CurrentTime
		  --FROM   @tbNames

		--INSERT INTO @tbUsers
		  --SELECT  UserId
		  --FROM	dbo.aspnet_Users au, @tbNames t
		  --WHERE   LOWER(t.Name) = au.LoweredUserName
	--END

	IF (EXISTS (SELECT * FROM dbo.aspnet_UsersInRoles ur, @tbUsers tu, @tbRoles tr WHERE tu.UserId = ur.UserId AND tr.RoleId = ur.RoleId))
	BEGIN
		SELECT TOP 1 UserName, RoleName
		FROM		 dbo.aspnet_UsersInRoles ur, @tbUsers tu, @tbRoles tr, aspnet_Users u, aspnet_Roles r
		WHERE		u.UserId = tu.UserId AND r.RoleId = tr.RoleId AND tu.UserId = ur.UserId AND tr.RoleId = ur.RoleId

		IF( @TranStarted = 1 )
			ROLLBACK TRANSACTION
		RETURN(3)
	END

	INSERT INTO dbo.aspnet_UsersInRoles (UserId, RoleId)
	SELECT UserId, RoleId
	FROM @tbUsers, @tbRoles

	IF( @TranStarted = 1 )
		COMMIT TRANSACTION
	RETURN(0)
END' 
END
GO
/****** Object:  StoredProcedure [dbo].[cp_ProductReviews_Get]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cp_ProductReviews_Get]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[cp_ProductReviews_Get]
(
	@PageIndex int,
	@PageSize int,
	@IsCount bit,
	@sqlPopulate ntext,
	@Total int = 0 output,
	@CategoryId INT = 0
)
AS
SET Transaction Isolation Level Read UNCOMMITTED

BEGIN
	DECLARE @PageLowerBound int
	DECLARE @PageUpperBound int

	-- Set the page bounds
	SET @PageLowerBound = @PageSize * (@PageIndex-1)
	SET @PageUpperBound = @PageLowerBound + @PageSize + 1

	CREATE TABLE #PageIndexForSearch
	(
		IndexId INT IDENTITY (1, 1) NOT NULL,		
	    ReviewId int
	)

	INSERT INTO #PageIndexForSearch (ReviewId) 
	Exec sp_executesql @sqlPopulate
    
    SET @Total = @@rowcount
    
		SELECT
		  p.[ProductId]
		  ,p.ProductCode
		  ,p.[ProductName]
          ,r.ReviewId
          ,r.ReviewText
          ,r.ReviewDate
          ,r.UserId
          ,r.UserName
		FROM 
		 Ecshop_Products p (nolock)inner join Ecshop_ProductReviews r on (r.productId=p.ProductId)
		,#PageIndexForSearch
		WHERE 
			r.ReviewId = #PageIndexForSearch.ReviewId AND
			#PageIndexForSearch.IndexId > @PageLowerBound AND
			#PageIndexForSearch.IndexId < @PageUpperBound
		ORDER BY
			#PageIndexForSearch.IndexId

		drop table #PageIndexForSearch

END' 
END
GO
/****** Object:  StoredProcedure [dbo].[cp_ProductConsultation_Get]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cp_ProductConsultation_Get]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[cp_ProductConsultation_Get] 
		(
		@PageIndex INT,
		@PageSize INT,
		@IsCount BIT,
		@CategoryId INT = 0,
		@SqlPopulate NTEXT
	)

AS
SET Transaction Isolation Level Read UNCOMMITTED

BEGIN
	DECLARE @PageLowerBound INT
	DECLARE @PageUpperBound INT
	DECLARE @RowsToReturn INT
	DECLARE @TotalProducts INT

	SET @TotalProducts = 0
	-- Set the page bounds
	SET @PageLowerBound = @PageSize * (@PageIndex-1)
	SET @PageUpperBound = @PageLowerBound + @PageSize + 1
	
	-- Create a temp table to store the select results
	CREATE TABLE #PageIndexForSearch 
	(
		IndexId INT IDENTITY (1, 1) NOT NULL,
		ConsultationId int
	)
	
	INSERT INTO #PageIndexForSearch (ConsultationId)  EXEC sp_executesql @SqlPopulate
	
	SET @TotalProducts = @@ROWCOUNT
	
	SELECT
	  p.[ProductId]
      ,p.[ProductName]     
	  ,p.[ProductCode]
      ,p.[ThumbnailUrl40]    
      ,c.[ConsultationId]
	  ,c.[ConsultationText]
      ,c.[ConsultationDate]
      ,c.[ReplyText]
      ,c.[UserName]
      ,c.[ReplyUserId]
      ,c.[Userid]
	  ,c.[ReplyDate]
      ,(SELECT UserName FROM aspnet_Users WHERE UserId = c.ReplyUserId) AS ReplyUserName
	FROM 
		Ecshop_Products p (nolock) inner join Ecshop_ProductConsultations c on (p.productId=c.ProductId),
		#PageIndexForSearch
	WHERE 
		c.ConsultationId = #PageIndexForSearch.ConsultationId AND
		#PageIndexForSearch.IndexId > @PageLowerBound AND
		#PageIndexForSearch.IndexId < @PageUpperBound
	ORDER BY
		#PageIndexForSearch.IndexId
	
	drop table #PageIndexForSearch
END

IF (@IsCount = 1)
	SELECT @TotalProducts' 
END
GO
/****** Object:  StoredProcedure [dbo].[cp_Member_Delete]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cp_Member_Delete]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[cp_Member_Delete]
	(
		@UserId INT,
		@UserName Nvarchar(256)
	)
AS

	DECLARE @intErrorCode INT
	SELECT @intErrorCode = 0

	SET XACT_ABORT ON
	BEGIN TRAN
				

	DELETE FROM aspnet_Members WHERE UserId = @UserId
	SELECT @intErrorCode = @@ERROR
	
	IF @intErrorCode = 0
	BEGIN
		DELETE FROM aspnet_UsersInRoles WHERE UserId = @UserId
		SELECT @intErrorCode = @@ERROR
	END
	
	IF @intErrorCode = 0
	BEGIN
		DELETE FROM aspnet_Users WHERE UserId = @UserId
		SELECT @intErrorCode = @@ERROR
	END
	
	IF @intErrorCode = 0
	BEGIN
		COMMIT TRAN
	END
	ELSE
	BEGIN
		ROLLBACK TRAN
	END
	
	RETURN @intErrorCode' 
END
GO
/****** Object:  StoredProcedure [dbo].[cp_Manager_Delete]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cp_Manager_Delete]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[cp_Manager_Delete]
	(
		@UserId INT
	)
AS

	DECLARE @intErrorCode INT
	SELECT @intErrorCode = 0

	SET XACT_ABORT ON
	BEGIN TRAN
	
	DELETE FROM aspnet_Managers WHERE UserId = @UserId
	SELECT @intErrorCode = @@ERROR
	
	IF @intErrorCode = 0
	BEGIN
		DELETE FROM aspnet_UsersInRoles WHERE UserId = @UserId
		SELECT @intErrorCode = @@ERROR
	END
	
	IF @intErrorCode = 0
	BEGIN
		DELETE FROM aspnet_Users WHERE UserId = @UserId
		SELECT @intErrorCode = @@ERROR
	END
	
	IF @intErrorCode = 0
	BEGIN
		COMMIT TRAN
	END
	ELSE
	BEGIN
		ROLLBACK TRAN
	END
	
	RETURN @intErrorCode' 
END
GO
/****** Object:  StoredProcedure [dbo].[cp_LeaveComments_Get]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cp_LeaveComments_Get]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[cp_LeaveComments_Get]
(
	@PageIndex int,
	@PageSize int,
	@IsCount bit,
	@sqlPopulate ntext,
	@Total int = 0 output
)
AS
SET Transaction Isolation Level Read UNCOMMITTED

BEGIN
	DECLARE @PageLowerBound int
	DECLARE @PageUpperBound int

	-- Set the page bounds
	SET @PageLowerBound = @PageSize * (@PageIndex - 1)
	SET @PageUpperBound = @PageLowerBound + @PageSize + 1

	CREATE TABLE #PageIndexForSearch
	(
		IndexId INT IDENTITY (1, 1) NOT NULL,
		LeaveId BIGINT
	)

	INSERT INTO #PageIndexForSearch (LeaveId) 
    Exec sp_executesql @sqlPopulate
    
    SET @Total = @@rowcount
    
		-- 第一层
	SELECT l.*
	FROM 
	 Ecshop_LeaveComments l ,#PageIndexForSearch
	WHERE 
		l.LeaveId = #PageIndexForSearch.LeaveId AND
		#PageIndexForSearch.IndexId > @PageLowerBound AND
		#PageIndexForSearch.IndexId < @PageUpperBound
	ORDER BY
		#PageIndexForSearch.IndexId
	
	
	-- 第二层
	SELECT r.*
		FROM Ecshop_LeaveCommentReplys r ,#PageIndexForSearch
		WHERE 
		r.LeaveId = #PageIndexForSearch.LeaveId AND
		#PageIndexForSearch.IndexId > @PageLowerBound AND
		#PageIndexForSearch.IndexId < @PageUpperBound
		order by ReplyDate desc
		
		drop table #PageIndexForSearch
END' 
END
GO
/****** Object:  StoredProcedure [dbo].[cp_Hotkeywords_Log]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cp_Hotkeywords_Log]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE procedure [dbo].[cp_Hotkeywords_Log]
(
	@Keywords NVARCHAR(512),
	@CategoryId INT,
	@SearchTime DateTime
)
AS
BEGIN
DECLARE @Frequency INT
SET @Frequency = 0
SELECT @Frequency =  Frequency  FROM Ecshop_Hotkeywords ORDER BY Frequency 
IF EXISTS (SELECT Keywords FROM Ecshop_Hotkeywords WHERE Lower(Keywords) = Lower(@Keywords) AND CategoryId=@CategoryId)

	UPDATE
		Ecshop_Hotkeywords
	SET
		Lasttime  = @SearchTime,
		Frequency = Frequency + 1
	WHERE
		Lower(Keywords) =Lower(@Keywords) AND CategoryId=@CategoryId
ELSE
	INSERT INTO Ecshop_Hotkeywords(CategoryId, Keywords, SearchTime, Lasttime, Frequency)
	VALUES (@CategoryId, @Keywords, @SearchTime, @SearchTime, @Frequency + 1 )
END


IF (SELECT COUNT(HID) FROM Ecshop_Hotkeywords) > 100
DELETE FROM Ecshop_Hotkeywords 
Where Hid NOT IN 
(SELECT TOP 100 HID FROM Ecshop_HotKeyWords ORDER BY Frequency  DESC)' 
END
GO
/****** Object:  StoredProcedure [dbo].[cp_ClaimCode_Create]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cp_ClaimCode_Create]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[cp_ClaimCode_Create]
	(
		@row int,
        @CouponId int,
        @UserId int,
        @EmailAddress nvarchar(255),
		@UserName nvarchar(256),
        @ReturnLotNumber nvarchar(300) OUTPUT
	)
AS
declare @LotNumber uniqueidentifier
set  @LotNumber=newid()
 WHILE(@row>0)
BEGIN
SET ROWCOUNT @row
    INSERT Ecshop_CouponItems SELECT
         CouponId=@CouponId,
         LotNumber=@LotNumber,
        ClaimCode =SUBSTRING(REPLACE(newid(),''-'',''''),1,15),
        UserId=@UserId,UserName=@UserName,EmailAddress=@EmailAddress,GenerateTime=getdate(),CouponStatus=0,UsedTime=null,OrderId=null
    FROM syscolumns c1, syscolumns c2   
 SET @row = @row - @@ROWCOUNT
END
SET @ReturnLotNumber=CONVERT(NVARCHAR(300),@LotNumber)' 
END
GO
/****** Object:  StoredProcedure [dbo].[cp_ShippingMode_Update]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cp_ShippingMode_Update]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'Create Procedure [dbo].[cp_ShippingMode_Update]
(
 @ModeId INT =null,
 @Name nvarchar(100),
 @TemplateId INT,
 @Description nvarchar(4000)=null,
 @Status INT Output
)
as 
DECLARE @DisplaySequence INT
DECLARE @intErrorCode INT
	SELECT @Status = 99, @intErrorCode = 0
BEGIN TRAN
  --直接取出原来的序号   
   SET @DisplaySequence = (Select DisplaySequence  From Ecshop_ShippingTypes where ModeId=@ModeId)
     
   Update Ecshop_ShippingTypes 
   SET [Name]=@Name,TemplateId=@TemplateId , Description =@Description
   Where ModeId=@ModeId
   SET @intErrorCode = @intErrorCode + @@ERROR
      
   IF @intErrorCode = 0
    BEGIN
       DELETE FROM Ecshop_TemplateRelatedShipping Where ModeId=@ModeId
   END
  IF @intErrorCode = 0
	BEGIN
		COMMIT TRAN
		SET @Status = 0
	END
	ELSE
		ROLLBACK TRAN' 
END
GO
/****** Object:  StoredProcedure [dbo].[cp_ShippingMode_Create]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cp_ShippingMode_Create]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'Create Procedure [dbo].[cp_ShippingMode_Create]
(
 @ModeId int output,
 @Name nvarchar(100),
 @TemplateId int,	
 @Description nvarchar(4000)=null,
 @Status int Output
)
as 
DECLARE @DisplaySequence INT
SET @Status = 99
BEGIN TRAN
--如果取最大序号为空 则直接将序号设置为1
   IF  (Select Max(DisplaySequence) From Ecshop_ShippingTypes) IS NUll
      SET @DisplaySequence = 1
   -- 如果不为空则将 将序号设置为表中现有的最大值加1
   ELSE
      SET @DisplaySequence = (Select Max(DisplaySequence) From Ecshop_ShippingTypes) + 1
 INSERT INTO Ecshop_ShippingTypes ([Name],TemplateId,[Description],DisplaySequence)
 VALUES (@Name,@TemplateId,@Description,@DisplaySequence)
  SET @ModeId = @@IDENTITY
     IF @@ROWCOUNT = 1
	   SET @Status = 0
	 
COMMIT' 
END
GO
/****** Object:  Table [dbo].[Ecshop_AttributeValues]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecshop_AttributeValues]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ecshop_AttributeValues](
	[ValueId] [int] IDENTITY(1,1) NOT NULL,
	[AttributeId] [int] NOT NULL,
	[DisplaySequence] [int] NOT NULL,
	[ValueStr] [nvarchar](200) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[ImageUrl] [nvarchar](255) COLLATE Chinese_PRC_CI_AS NULL,
 CONSTRAINT [PK_Ecshop_AttributeValues] PRIMARY KEY CLUSTERED 
(
	[ValueId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  StoredProcedure [dbo].[ac_Member_Favorites_Get]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ac_Member_Favorites_Get]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[ac_Member_Favorites_Get]
	(
	@PageIndex int,
	@PageSize int,
	@IsCount bit,
    @GradeId int,
	@sqlPopulate ntext,
	@TotalFavorites int = 0 output
)
AS
	SET Transaction Isolation Level Read UNCOMMITTED

BEGIN
	DECLARE @PageLowerBound int
	DECLARE @PageUpperBound int
    DECLARE @Discount int

	-- Set the page bounds
	SET @PageLowerBound = @PageSize * (@PageIndex-1)
	SET @PageUpperBound = @PageLowerBound + @PageSize + 1

	CREATE TABLE #PageIndexForFavorites
	(
		IndexId int IDENTITY (1, 1) NOT NULL,
		FavoriteId int
	)	

	INSERT INTO #PageIndexForFavorites(FavoriteId)
	Exec sp_executesql @sqlPopulate

	SET @TotalFavorites = @@rowcount
     
    SELECT @Discount = Discount FROM aspnet_MemberGrades WHERE GradeId = @GradeId;
    
    begin
		SELECT
			F.FavoriteId, F.ProductId, F.UserId, F.Tags, F.Remark,
			 P.Stock, P.ProductName, P.MarketPrice,
			 p.SalePrice, ThumbnailUrl60,ThumbnailUrl100,
            CASE WHEN (SELECT COUNT(*) FROM Ecshop_SKUMemberPrice WHERE SkuId = p.SkuId AND GradeId = @GradeId) = 1  
			THEN (SELECT MemberSalePrice FROM Ecshop_SKUMemberPrice WHERE SkuId = p.SkuId AND GradeId = @GradeId) ELSE SalePrice*@Discount/100 END AS RankPrice
		FROM 
			vw_Ecshop_BrowseProductList P,
			Ecshop_Favorite F (nolock),	
			#PageIndexForFavorites
		WHERE 
			F.ProductId = P.ProductId AND
			F.FavoriteId = #PageIndexForFavorites.FavoriteId AND
			P.SaleStatus=1 AND
			#PageIndexForFavorites.IndexId > @PageLowerBound AND
			#PageIndexForFavorites.IndexId < @PageUpperBound
		ORDER BY
			#PageIndexForFavorites.IndexId
	end

	drop table #PageIndexForFavorites
END

IF (@IsCount = 1)
	SELECT @TotalFavorites' 
END
GO
/****** Object:  View [dbo].[vw_Ecshop_GroupBuy]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_Ecshop_GroupBuy]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [dbo].[vw_Ecshop_GroupBuy]
AS
SELECT g.GroupBuyId, g.ProductId, p.ProductName, g.Status,g.StartDate,g.EndDate, g.NeedPrice,g.MaxCount,g.DisplaySequence,c.Price,g.Content,
(SELECT COUNT(OrderId) FROM Ecshop_Orders WHERE GroupBuyId = g.GroupBuyId) AS OrderCount,
(SELECT COUNT(OrderId) FROM Ecshop_Orders WHERE GroupBuyId = g.GroupBuyId) AS SoldCount,
(SELECT SUM(Quantity) FROM Ecshop_OrderItems WHERE OrderId IN (SELECT OrderId FROM Ecshop_Orders WHERE GroupBuyId = g.GroupBuyId AND OrderStatus <> 1 AND OrderStatus <> 4)) AS ProdcutQuantity
FROM Ecshop_GroupBuy g JOIN Ecshop_Products p ON g.ProductId = p.ProductId
left join  Ecshop_GroupBuyCondition  c on c.GroupBuyId = g.GroupBuyId
'
GO
/****** Object:  View [dbo].[vw_Ecshop_CountDown]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_Ecshop_CountDown]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [dbo].[vw_Ecshop_CountDown]
	AS
SELECT CountDownId,P.ProductId,p.ProductName,p.MarketPrice,p.SalePrice,c.CountDownPrice,c.StartDate,c.EndDate,c.[MaxCount],c.DisplaySequence,
p.ThumbnailUrl40,p.ThumbnailUrl60,p.ThumbnailUrl100,p.ThumbnailUrl160, p.ThumbnailUrl180, p.ThumbnailUrl220,ThumbnailUrl310
FROM Ecshop_CountDown c join vw_Ecshop_BrowseProductList p ON c.ProductId = p.ProductId'
GO
/****** Object:  StoredProcedure [dbo].[ss_GroupBuyProducts_Get]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ss_GroupBuyProducts_Get]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[ss_GroupBuyProducts_Get]
(
	@PageIndex int,
	@PageSize int,
	@IsCount bit,
	@sqlPopulate ntext,
	@TotalGroupBuyProducts int = 0 output
)
AS
	SET Transaction Isolation Level Read UNCOMMITTED

BEGIN
	DECLARE @PageLowerBound int
	DECLARE @PageUpperBound int

	-- Set the page bounds
	SET @PageLowerBound = @PageSize * (@PageIndex-1)
	SET @PageUpperBound = @PageLowerBound + @PageSize + 1

	CREATE TABLE #PageIndexForGroupBuyProducts
	(
		IndexId int IDENTITY (1, 1) NOT NULL,	
		GroupBuyId int,
		ProductId int,
	    StartDate DateTime,
		EndDate DateTime	
	)

	INSERT INTO #PageIndexForGroupBuyProducts(GroupBuyId,ProductId,StartDate,EndDate)
	Exec sp_executesql @sqlPopulate

	SET @TotalGroupBuyProducts = @@rowcount
	
	SELECT  S.GroupBuyId,S.StartDate,S.EndDate,P.ProductName,p.MarketPrice, P.SalePrice as OldPrice,
		ThumbnailUrl60,ThumbnailUrl100, ThumbnailUrl160,ThumbnailUrl180, ThumbnailUrl220,ThumbnailUrl310, P.ProductId,G.[Count],G.Price	
	FROM   #PageIndexForGroupBuyProducts S JOIN vw_Ecshop_BrowseProductList P on S.ProductId=P.ProductId JOIN Ecshop_GroupBuyCondition G on S.GroupBuyId=G.GroupBuyId
	WHERE 
			S.IndexId > @PageLowerBound AND
			S.IndexId < @PageUpperBound
	ORDER BY S.IndexId
	------------------------------------------------------------
	 
   
	drop table #PageIndexForGroupBuyProducts
END' 
END
GO
/****** Object:  Trigger [T_Ecshop_AttributeValues_Delete]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[T_Ecshop_AttributeValues_Delete]'))
EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER [dbo].[T_Ecshop_AttributeValues_Delete]
    ON [dbo].[Ecshop_AttributeValues] FOR DELETE                          
    AS       
        DELETE Ecshop_ProductAttributes 
		FROM Ecshop_ProductAttributes at, Deleted d  
        WHERE at.ValueID=d.ValueID'
GO
/****** Object:  StoredProcedure [dbo].[ss_ShoppingCart_GetItemInfo]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ss_ShoppingCart_GetItemInfo]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[ss_ShoppingCart_GetItemInfo]
	(
		@Quantity INT,
		@UserId INT,
		@SkuId NVARCHAR(100),
		@GradeId INT
	)
AS

DECLARE @ProductId INT, @Weight MONEY, @Stock INT, @SalePrice MONEY, @MemberPrice MONEY, @Discount INT, @SKU NVARCHAR(50) 

 SELECT @ProductId = ProductId, @SKU = SKU, @Weight = [Weight], @Stock = Stock, @SalePrice = SalePrice FROM Ecshop_SKUs WHERE SkuId = @SkuId
-- 会员查询
IF @UserId>0 
BEGIN
	SELECT @MemberPrice = MemberSalePrice FROM Ecshop_SKUMemberPrice WHERE SkuId = @SkuId AND GradeId = @GradeId
	SELECT @Discount = Discount FROM aspnet_MemberGrades WHERE GradeId = @GradeId		
	SELECT @Quantity=Quantity FROM Ecshop_ShoppingCarts WHERE UserId = @UserId AND SkuId = @SkuId

	IF @MemberPrice IS NOT NULL
		SET @SalePrice = @MemberPrice
	ELSE
		SET @SalePrice = (@SalePrice * @Discount)/100
 END
	
 -- 返回商品基本信息
SELECT ProductId, SaleStatus, @SKU as SKU, @Stock as Stock, @Quantity as TotalQuantity, ProductName, CategoryId, @Weight AS [Weight], @SalePrice AS SalePrice, 
	ThumbnailUrl40,ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160, ThumbnailUrl180, ThumbnailUrl220,IsfreeShipping FROM Ecshop_Products WHERE ProductId = @ProductId AND SaleStatus=1
-- 返回当前规格信息
SELECT s.SkuId, s.SKU, s.ProductId, s.Stock, AttributeName, ValueStr FROM Ecshop_SKUs s left join Ecshop_SKUItems si on s.SkuId = si.SkuId
left join Ecshop_Attributes a on si.AttributeId = a.AttributeId left join Ecshop_AttributeValues av on si.ValueId = av.ValueId WHERE s.SkuId = @SkuId
AND s.ProductId IN (SELECT ProductId FROM Ecshop_Products WHERE SaleStatus=1)
--返回促销信息
SELECT * FROM Ecshop_Promotions p INNER JOIN Ecshop_PromotionProducts pp ON p.ActivityId = pp.ActivityId WHERE ProductId = @ProductId
AND DateDiff(DD, StartDate, getdate()) >= 0 AND DateDiff(DD, EndDate, getdate()) <= 0
AND p.ActivityId IN (SELECT ActivityId FROM Ecshop_PromotionMemberGrades WHERE GradeId=@GradeId)' 
END
GO
/****** Object:  StoredProcedure [dbo].[cp_Product_GetExportList]    Script Date: 12/10/2014 10:04:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cp_Product_GetExportList]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[cp_Product_GetExportList]
	@sqlPopulate ntext
AS
	CREATE TABLE #Products
	(
		[ProductId] int,
		[TypeId] int,
		[ProductName] [nvarchar] (200),
		[ProductCode] [nvarchar] (50),
		[ShortDescription] [nvarchar] (2000),
		[Unit] [nvarchar] (50),
		[Description] [ntext],	
		[Title] [nvarchar] (100),
		[Meta_Description]	[nvarchar] (1000),
		[Meta_Keywords] [nvarchar] (1000),
		[SaleStatus] [int],
		[ImageUrl1] [nvarchar] (255),
		[ImageUrl2] [nvarchar] (255),
		[ImageUrl3] [nvarchar] (255),
		[ImageUrl4] [nvarchar] (255),
		[ImageUrl5] [nvarchar] (255),
		[MarketPrice] [money],
		[HasSKU] [BIT]
	)

	-- 商品
	INSERT INTO #Products ([ProductId], [TypeId], [ProductName], [ProductCode], [ShortDescription], [Unit], [Description],[Title],[Meta_Description],[Meta_Keywords],
		[SaleStatus], [ImageUrl1], [ImageUrl2], [ImageUrl3], [ImageUrl4], [ImageUrl5], [MarketPrice], [HasSKU]) 
    Exec sp_executesql @sqlPopulate
	-- 类型
	SELECT TypeId, TypeName, Remark INTO  #Types FROM Ecshop_ProductTypes WHERE TypeId IN (SELECT DISTINCT([TypeId]) FROM #Products WHERE #Products.TypeId IS NOT NULL)
	-- 规格
	SELECT [SkuId], [ProductId], [SKU], [Weight], [Stock], [CostPrice], [SalePrice] INTO #Skus
		FROM Ecshop_SKUs WHERE ProductId IN (SELECT ProductId FROM #Products)
	-- 规格项
	SELECT [SkuId], [AttributeId], [ValueId] INTO #SKUItems FROM Ecshop_SKUItems WHERE SkuId IN (SELECT SkuId FROM #Skus)
	-- 商品属性
	SELECT [ProductId], [AttributeId], [ValueId] INTO #ProductAttributes FROM Ecshop_ProductAttributes WHERE ProductId IN (SELECT ProductId FROM #Products)
	-- 属性
	SELECT [AttributeId], [AttributeName], [DisplaySequence], [TypeId], [UsageMode], [UseAttributeImage] INTO #Attributes 
		FROM Ecshop_Attributes WHERE [AttributeId] IN (SELECT DISTINCT([AttributeId]) FROM #SKUItems UNION SELECT DISTINCT([AttributeId]) FROM #ProductAttributes)
	-- 属性值
	SELECT [ValueId], [AttributeId], [DisplaySequence], [ValueStr], [ImageUrl] INTO #Values 
		FROM Ecshop_AttributeValues WHERE [ValueId] IN (SELECT DISTINCT([ValueId]) FROM #SKUItems UNION SELECT DISTINCT([ValueId]) FROM #ProductAttributes)
		
	---淘宝属性值
	SELECT  *
		INTO #TaoBaoSKU FROM dbo.Taobao_Products WHERE [ProductId] IN (SELECT ProductId FROM #Products) 


	-- 输出商品类型
	SELECT * FROM #Types
	-- 输出类型的属性
	SELECT * FROM #Attributes
	--输出属性值
	SELECT * FROM #Values
	--输出商品信息
	SELECT * FROM #Products
	-- 输出商品规格信息
	SELECT * FROM #Skus
	-- 输出商品规格的字段值
	SELECT * FROM #SKUItems
	-- 输出商品属性
	SELECT * FROM #ProductAttributes
	-- 输出淘宝属性值
	SELECT * FROM #TaoBaoSKU

	DROP TABLE #Types
	DROP TABLE #Attributes
	DROP TABLE #Values
	DROP TABLE #Products
	DROP TABLE #Skus
	DROP TABLE #SKUItems
	DROP TABLE #ProductAttributes
	DROP TABLE #TaoBaoSKU' 
END
GO
/****** Object:  Default [DF_aspnet_Members_ReferralStatus]    Script Date: 12/10/2014 10:04:31 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_aspnet_Members_ReferralStatus]') AND parent_object_id = OBJECT_ID(N'[dbo].[aspnet_Members]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_aspnet_Members_ReferralStatus]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[aspnet_Members] ADD  CONSTRAINT [DF_aspnet_Members_ReferralStatus]  DEFAULT ((0)) FOR [ReferralStatus]
END


End
GO
/****** Object:  Default [DF__tmp_ms_xx__IsOpe__5ECA0095]    Script Date: 12/10/2014 10:04:31 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF__tmp_ms_xx__IsOpe__5ECA0095]') AND parent_object_id = OBJECT_ID(N'[dbo].[aspnet_Members]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__tmp_ms_xx__IsOpe__5ECA0095]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[aspnet_Members] ADD  CONSTRAINT [DF__tmp_ms_xx__IsOpe__5ECA0095]  DEFAULT ((0)) FOR [IsOpenBalance]
END


End
GO
/****** Object:  Default [DF_aspnet_Members_OrderNumber]    Script Date: 12/10/2014 10:04:31 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_aspnet_Members_OrderNumber]') AND parent_object_id = OBJECT_ID(N'[dbo].[aspnet_Members]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_aspnet_Members_OrderNumber]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[aspnet_Members] ADD  CONSTRAINT [DF_aspnet_Members_OrderNumber]  DEFAULT ((0)) FOR [OrderNumber]
END


End
GO
/****** Object:  Default [DF_aspnet_Members_Expenditure]    Script Date: 12/10/2014 10:04:31 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_aspnet_Members_Expenditure]') AND parent_object_id = OBJECT_ID(N'[dbo].[aspnet_Members]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_aspnet_Members_Expenditure]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[aspnet_Members] ADD  CONSTRAINT [DF_aspnet_Members_Expenditure]  DEFAULT ((0)) FOR [Expenditure]
END


End
GO
/****** Object:  Default [DF_aspnet_Members_Points]    Script Date: 12/10/2014 10:04:31 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_aspnet_Members_Points]') AND parent_object_id = OBJECT_ID(N'[dbo].[aspnet_Members]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_aspnet_Members_Points]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[aspnet_Members] ADD  CONSTRAINT [DF_aspnet_Members_Points]  DEFAULT ((0)) FOR [Points]
END


End
GO
/****** Object:  Default [DF_aspnet_Members_Balance]    Script Date: 12/10/2014 10:04:31 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_aspnet_Members_Balance]') AND parent_object_id = OBJECT_ID(N'[dbo].[aspnet_Members]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_aspnet_Members_Balance]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[aspnet_Members] ADD  CONSTRAINT [DF_aspnet_Members_Balance]  DEFAULT ((0)) FOR [Balance]
END


End
GO
/****** Object:  Default [DF_aspnet_Members_RequestBalance]    Script Date: 12/10/2014 10:04:31 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_aspnet_Members_RequestBalance]') AND parent_object_id = OBJECT_ID(N'[dbo].[aspnet_Members]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_aspnet_Members_RequestBalance]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[aspnet_Members] ADD  CONSTRAINT [DF_aspnet_Members_RequestBalance]  DEFAULT ((0)) FOR [RequestBalance]
END


End
GO
/****** Object:  Default [DF__aspnet_Ro__RoleI__2E1BDC42]    Script Date: 12/10/2014 10:04:31 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF__aspnet_Ro__RoleI__2E1BDC42]') AND parent_object_id = OBJECT_ID(N'[dbo].[aspnet_Roles]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__aspnet_Ro__RoleI__2E1BDC42]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[aspnet_Roles] ADD  CONSTRAINT [DF__aspnet_Ro__RoleI__2E1BDC42]  DEFAULT (newid()) FOR [RoleId]
END


End
GO
/****** Object:  Default [DF__aspnet_Us__IsAno__014935CB]    Script Date: 12/10/2014 10:04:31 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF__aspnet_Us__IsAno__014935CB]') AND parent_object_id = OBJECT_ID(N'[dbo].[aspnet_Users]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__aspnet_Us__IsAno__014935CB]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[aspnet_Users] ADD  CONSTRAINT [DF__aspnet_Us__IsAno__014935CB]  DEFAULT ((0)) FOR [IsAnonymous]
END


End
GO
/****** Object:  Default [DF__aspnet_Us__Sessi__09E968C4]    Script Date: 12/10/2014 10:04:31 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF__aspnet_Us__Sessi__09E968C4]') AND parent_object_id = OBJECT_ID(N'[dbo].[aspnet_Users]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__aspnet_Us__Sessi__09E968C4]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[aspnet_Users] ADD  CONSTRAINT [DF__aspnet_Us__Sessi__09E968C4]  DEFAULT (newid()) FOR [SessionId]
END


End
GO
/****** Object:  Default [DF__Ecshop_Ap__APITi__0ADD8CFD]    Script Date: 12/10/2014 10:04:31 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF__Ecshop_Ap__APITi__0ADD8CFD]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_ApiShorpCart]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Ecshop_Ap__APITi__0ADD8CFD]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Ecshop_ApiShorpCart] ADD  CONSTRAINT [DF__Ecshop_Ap__APITi__0ADD8CFD]  DEFAULT (getdate()) FOR [APITime]
END


End
GO
/****** Object:  Default [DF_Ecshop_Articles_IsRelease]    Script Date: 12/10/2014 10:04:32 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Ecshop_Articles_IsRelease]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_Articles]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Ecshop_Articles_IsRelease]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Ecshop_Articles] ADD  CONSTRAINT [DF_Ecshop_Articles_IsRelease]  DEFAULT ((1)) FOR [IsRelease]
END


End
GO
/****** Object:  Default [DF_Ecshop_Banner_Client]    Script Date: 12/10/2014 10:04:32 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Ecshop_Banner_Client]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_Banner]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Ecshop_Banner_Client]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Ecshop_Banner] ADD  CONSTRAINT [DF_Ecshop_Banner_Client]  DEFAULT ((1)) FOR [Client]
END


End
GO
/****** Object:  Default [DF_Ecshop_BundlingProductItems_ProductNum]    Script Date: 12/10/2014 10:04:32 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Ecshop_BundlingProductItems_ProductNum]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_BundlingProductItems]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Ecshop_BundlingProductItems_ProductNum]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Ecshop_BundlingProductItems] ADD  CONSTRAINT [DF_Ecshop_BundlingProductItems_ProductNum]  DEFAULT ((0)) FOR [ProductNum]
END


End
GO
/****** Object:  Default [DF_Ecshop_BundlingProducts_Num]    Script Date: 12/10/2014 10:04:32 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Ecshop_BundlingProducts_Num]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_BundlingProducts]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Ecshop_BundlingProducts_Num]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Ecshop_BundlingProducts] ADD  CONSTRAINT [DF_Ecshop_BundlingProducts_Num]  DEFAULT ((0)) FOR [Num]
END


End
GO
/****** Object:  Default [DF_Ecshop_BundlingProducts_Price]    Script Date: 12/10/2014 10:04:32 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Ecshop_BundlingProducts_Price]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_BundlingProducts]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Ecshop_BundlingProducts_Price]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Ecshop_BundlingProducts] ADD  CONSTRAINT [DF_Ecshop_BundlingProducts_Price]  DEFAULT ((0)) FOR [Price]
END


End
GO
/****** Object:  Default [DF_Ecshop_Categories_HasChildren]    Script Date: 12/10/2014 10:04:32 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Ecshop_Categories_HasChildren]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_Categories]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Ecshop_Categories_HasChildren]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Ecshop_Categories] ADD  CONSTRAINT [DF_Ecshop_Categories_HasChildren]  DEFAULT ((0)) FOR [HasChildren]
END


End
GO
/****** Object:  Default [DF_Ecshop_CountDown_DisplaySequence]    Script Date: 12/10/2014 10:04:32 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Ecshop_CountDown_DisplaySequence]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_CountDown]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Ecshop_CountDown_DisplaySequence]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Ecshop_CountDown] ADD  CONSTRAINT [DF_Ecshop_CountDown_DisplaySequence]  DEFAULT ((0)) FOR [DisplaySequence]
END


End
GO
/****** Object:  Default [DF_Ecshop_CountDown_MaxCount]    Script Date: 12/10/2014 10:04:32 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Ecshop_CountDown_MaxCount]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_CountDown]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Ecshop_CountDown_MaxCount]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Ecshop_CountDown] ADD  CONSTRAINT [DF_Ecshop_CountDown_MaxCount]  DEFAULT ((1)) FOR [MaxCount]
END


End
GO
/****** Object:  Default [DF_Ecshop_CouponItems_CouponStatus]    Script Date: 12/10/2014 10:04:32 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Ecshop_CouponItems_CouponStatus]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_CouponItems]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Ecshop_CouponItems_CouponStatus]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Ecshop_CouponItems] ADD  CONSTRAINT [DF_Ecshop_CouponItems_CouponStatus]  DEFAULT ((0)) FOR [CouponStatus]
END


End
GO
/****** Object:  Default [DF_Ecshop_Coupons_SentCount]    Script Date: 12/10/2014 10:04:32 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Ecshop_Coupons_SentCount]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_Coupons]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Ecshop_Coupons_SentCount]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Ecshop_Coupons] ADD  CONSTRAINT [DF_Ecshop_Coupons_SentCount]  DEFAULT ((0)) FOR [SentCount]
END


End
GO
/****** Object:  Default [DF_Ecshop_Coupons_UsedCount]    Script Date: 12/10/2014 10:04:32 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Ecshop_Coupons_UsedCount]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_Coupons]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Ecshop_Coupons_UsedCount]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Ecshop_Coupons] ADD  CONSTRAINT [DF_Ecshop_Coupons_UsedCount]  DEFAULT ((0)) FOR [UsedCount]
END


End
GO
/****** Object:  Default [DF_Ecshop_Coupons_NeedPoint]    Script Date: 12/10/2014 10:04:32 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Ecshop_Coupons_NeedPoint]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_Coupons]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Ecshop_Coupons_NeedPoint]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Ecshop_Coupons] ADD  CONSTRAINT [DF_Ecshop_Coupons_NeedPoint]  DEFAULT ((0)) FOR [NeedPoint]
END


End
GO
/****** Object:  Default [DF_Ecshop_Gifts_NeedPoint]    Script Date: 12/10/2014 10:04:32 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Ecshop_Gifts_NeedPoint]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_Gifts]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Ecshop_Gifts_NeedPoint]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Ecshop_Gifts] ADD  CONSTRAINT [DF_Ecshop_Gifts_NeedPoint]  DEFAULT ((0)) FOR [NeedPoint]
END


End
GO
/****** Object:  Default [DF_Ecshop_GiftShoppingCarts_AddTime]    Script Date: 12/10/2014 10:04:32 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Ecshop_GiftShoppingCarts_AddTime]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_GiftShoppingCarts]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Ecshop_GiftShoppingCarts_AddTime]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Ecshop_GiftShoppingCarts] ADD  CONSTRAINT [DF_Ecshop_GiftShoppingCarts_AddTime]  DEFAULT (getdate()) FOR [AddTime]
END


End
GO
/****** Object:  Default [DF_Ecshop_GiftShoppingCarts_PromoType]    Script Date: 12/10/2014 10:04:32 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Ecshop_GiftShoppingCarts_PromoType]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_GiftShoppingCarts]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Ecshop_GiftShoppingCarts_PromoType]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Ecshop_GiftShoppingCarts] ADD  CONSTRAINT [DF_Ecshop_GiftShoppingCarts_PromoType]  DEFAULT ((0)) FOR [PromoType]
END


End
GO
/****** Object:  Default [DF_Ecshop_GroupBuy_DisplaySequence]    Script Date: 12/10/2014 10:04:32 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Ecshop_GroupBuy_DisplaySequence]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_GroupBuy]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Ecshop_GroupBuy_DisplaySequence]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Ecshop_GroupBuy] ADD  CONSTRAINT [DF_Ecshop_GroupBuy_DisplaySequence]  DEFAULT ((0)) FOR [DisplaySequence]
END


End
GO
/****** Object:  Default [DF_Ecshop_LeaveComments_IsReply]    Script Date: 12/10/2014 10:04:32 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Ecshop_LeaveComments_IsReply]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_LeaveComments]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Ecshop_LeaveComments_IsReply]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Ecshop_LeaveComments] ADD  CONSTRAINT [DF_Ecshop_LeaveComments_IsReply]  DEFAULT ((0)) FOR [IsReply]
END


End
GO
/****** Object:  Default [DF_Ecshop_MessageTemplates_SendEmail]    Script Date: 12/10/2014 10:04:32 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Ecshop_MessageTemplates_SendEmail]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_MessageTemplates]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Ecshop_MessageTemplates_SendEmail]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Ecshop_MessageTemplates] ADD  CONSTRAINT [DF_Ecshop_MessageTemplates_SendEmail]  DEFAULT ((0)) FOR [SendEmail]
END


End
GO
/****** Object:  Default [DF_Ecshop_MessageTemplates_SendSMS]    Script Date: 12/10/2014 10:04:32 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Ecshop_MessageTemplates_SendSMS]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_MessageTemplates]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Ecshop_MessageTemplates_SendSMS]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Ecshop_MessageTemplates] ADD  CONSTRAINT [DF_Ecshop_MessageTemplates_SendSMS]  DEFAULT ((0)) FOR [SendSMS]
END


End
GO
/****** Object:  Default [DF_Ecshop_MessageTemplates_SendInnerMessage]    Script Date: 12/10/2014 10:04:32 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Ecshop_MessageTemplates_SendInnerMessage]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_MessageTemplates]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Ecshop_MessageTemplates_SendInnerMessage]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Ecshop_MessageTemplates] ADD  CONSTRAINT [DF_Ecshop_MessageTemplates_SendInnerMessage]  DEFAULT ((0)) FOR [SendInnerMessage]
END


End
GO
/****** Object:  Default [DF_Ecshop_MessageTemplates_SendWeixinMessage]    Script Date: 12/10/2014 10:04:32 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Ecshop_MessageTemplates_SendWeixinMessage]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_MessageTemplates]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Ecshop_MessageTemplates_SendWeixinMessage]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Ecshop_MessageTemplates] ADD  CONSTRAINT [DF_Ecshop_MessageTemplates_SendWeixinMessage]  DEFAULT ((0)) FOR [SendWeixin]
END


End
GO
/****** Object:  Default [DF_Ecshop_OrderGifts_PromoType]    Script Date: 12/10/2014 10:04:32 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Ecshop_OrderGifts_PromoType]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_OrderGifts]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Ecshop_OrderGifts_PromoType]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Ecshop_OrderGifts] ADD  CONSTRAINT [DF_Ecshop_OrderGifts_PromoType]  DEFAULT ((0)) FOR [PromoType]
END


End
GO
/****** Object:  Default [DF__tmp_ms_xx__Updat__49CEE3AF]    Script Date: 12/10/2014 10:04:32 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF__tmp_ms_xx__Updat__49CEE3AF]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_Orders]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__tmp_ms_xx__Updat__49CEE3AF]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Ecshop_Orders] ADD  CONSTRAINT [DF__tmp_ms_xx__Updat__49CEE3AF]  DEFAULT (getdate()) FOR [UpdateDate]
END


End
GO
/****** Object:  Default [DF__tmp_ms_xx__Sourc__4AC307E8]    Script Date: 12/10/2014 10:04:32 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF__tmp_ms_xx__Sourc__4AC307E8]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_Orders]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__tmp_ms_xx__Sourc__4AC307E8]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Ecshop_Orders] ADD  CONSTRAINT [DF__tmp_ms_xx__Sourc__4AC307E8]  DEFAULT ((1)) FOR [SourceOrder]
END


End
GO
/****** Object:  Default [DF_Ecshop_PaymentTypes_IsUseInpour]    Script Date: 12/10/2014 10:04:32 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Ecshop_PaymentTypes_IsUseInpour]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_PaymentTypes]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Ecshop_PaymentTypes_IsUseInpour]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Ecshop_PaymentTypes] ADD  CONSTRAINT [DF_Ecshop_PaymentTypes_IsUseInpour]  DEFAULT ('true') FOR [IsUseInpour]
END


End
GO
/****** Object:  Default [DF__tmp_ms_xx__Updat__22B5168E]    Script Date: 12/10/2014 10:04:32 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF__tmp_ms_xx__Updat__22B5168E]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_Products]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__tmp_ms_xx__Updat__22B5168E]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Ecshop_Products] ADD  CONSTRAINT [DF__tmp_ms_xx__Updat__22B5168E]  DEFAULT (getdate()) FOR [UpdateDate]
END


End
GO
/****** Object:  Default [DF_Ecshop_Products_VistiCounts]    Script Date: 12/10/2014 10:04:32 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Ecshop_Products_VistiCounts]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_Products]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Ecshop_Products_VistiCounts]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Ecshop_Products] ADD  CONSTRAINT [DF_Ecshop_Products_VistiCounts]  DEFAULT ((0)) FOR [VistiCounts]
END


End
GO
/****** Object:  Default [DF_Ecshop_Products_SaleCounts]    Script Date: 12/10/2014 10:04:32 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Ecshop_Products_SaleCounts]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_Products]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Ecshop_Products_SaleCounts]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Ecshop_Products] ADD  CONSTRAINT [DF_Ecshop_Products_SaleCounts]  DEFAULT ((0)) FOR [SaleCounts]
END


End
GO
/****** Object:  Default [DF_Ecshop_Products_ShowSaleCounts]    Script Date: 12/10/2014 10:04:32 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Ecshop_Products_ShowSaleCounts]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_Products]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Ecshop_Products_ShowSaleCounts]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Ecshop_Products] ADD  CONSTRAINT [DF_Ecshop_Products_ShowSaleCounts]  DEFAULT ((0)) FOR [ShowSaleCounts]
END


End
GO
/****** Object:  Default [DF_Ecshop_Products_DisplaySequence]    Script Date: 12/10/2014 10:04:32 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Ecshop_Products_DisplaySequence]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_Products]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Ecshop_Products_DisplaySequence]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Ecshop_Products] ADD  CONSTRAINT [DF_Ecshop_Products_DisplaySequence]  DEFAULT ((0)) FOR [DisplaySequence]
END


End
GO
/****** Object:  Default [DF_Ecshop_Shippers_DistributorUserId]    Script Date: 12/10/2014 10:04:32 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Ecshop_Shippers_DistributorUserId]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_Shippers]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Ecshop_Shippers_DistributorUserId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Ecshop_Shippers] ADD  CONSTRAINT [DF_Ecshop_Shippers_DistributorUserId]  DEFAULT ((0)) FOR [DistributorUserId]
END


End
GO
/****** Object:  Default [DF_Ecshop_ShoppingCarts_AddTime]    Script Date: 12/10/2014 10:04:32 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Ecshop_ShoppingCarts_AddTime]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_ShoppingCarts]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Ecshop_ShoppingCarts_AddTime]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Ecshop_ShoppingCarts] ADD  CONSTRAINT [DF_Ecshop_ShoppingCarts_AddTime]  DEFAULT (getdate()) FOR [AddTime]
END


End
GO
/****** Object:  Default [DF__Ecshop_Us__IsDef__0CC5D56F]    Script Date: 12/10/2014 10:04:32 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF__Ecshop_Us__IsDef__0CC5D56F]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_UserShippingAddresses]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Ecshop_Us__IsDef__0CC5D56F]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Ecshop_UserShippingAddresses] ADD  CONSTRAINT [DF__Ecshop_Us__IsDef__0CC5D56F]  DEFAULT ((0)) FOR [IsDefault]
END


End
GO
/****** Object:  Default [DF_Vshop_HomeProducts_Client]    Script Date: 12/10/2014 10:04:32 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Vshop_HomeProducts_Client]') AND parent_object_id = OBJECT_ID(N'[dbo].[Vshop_HomeProducts]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Vshop_HomeProducts_Client]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Vshop_HomeProducts] ADD  CONSTRAINT [DF_Vshop_HomeProducts_Client]  DEFAULT ((1)) FOR [Client]
END


End
GO
/****** Object:  Default [DF_Vshop_HomeTopics_Client]    Script Date: 12/10/2014 10:04:32 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Vshop_HomeTopics_Client]') AND parent_object_id = OBJECT_ID(N'[dbo].[Vshop_HomeTopics]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Vshop_HomeTopics_Client]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Vshop_HomeTopics] ADD  CONSTRAINT [DF_Vshop_HomeTopics_Client]  DEFAULT ((1)) FOR [Client]
END


End
GO
/****** Object:  Default [DF_vshop_Menu_Client]    Script Date: 12/10/2014 10:04:32 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_vshop_Menu_Client]') AND parent_object_id = OBJECT_ID(N'[dbo].[vshop_Menu]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_vshop_Menu_Client]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[vshop_Menu] ADD  CONSTRAINT [DF_vshop_Menu_Client]  DEFAULT ((1)) FOR [Client]
END


End
GO
/****** Object:  ForeignKey [FK__aspnet_Us__RoleI__31EC6D26]    Script Date: 12/10/2014 10:04:31 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__aspnet_Us__RoleI__31EC6D26]') AND parent_object_id = OBJECT_ID(N'[dbo].[aspnet_UsersInRoles]'))
ALTER TABLE [dbo].[aspnet_UsersInRoles]  WITH CHECK ADD  CONSTRAINT [FK__aspnet_Us__RoleI__31EC6D26] FOREIGN KEY([RoleId])
REFERENCES [dbo].[aspnet_Roles] ([RoleId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__aspnet_Us__RoleI__31EC6D26]') AND parent_object_id = OBJECT_ID(N'[dbo].[aspnet_UsersInRoles]'))
ALTER TABLE [dbo].[aspnet_UsersInRoles] CHECK CONSTRAINT [FK__aspnet_Us__RoleI__31EC6D26]
GO
/****** Object:  ForeignKey [FK__aspnet_Us__UserI__30F848ED]    Script Date: 12/10/2014 10:04:31 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__aspnet_Us__UserI__30F848ED]') AND parent_object_id = OBJECT_ID(N'[dbo].[aspnet_UsersInRoles]'))
ALTER TABLE [dbo].[aspnet_UsersInRoles]  WITH CHECK ADD  CONSTRAINT [FK__aspnet_Us__UserI__30F848ED] FOREIGN KEY([UserId])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__aspnet_Us__UserI__30F848ED]') AND parent_object_id = OBJECT_ID(N'[dbo].[aspnet_UsersInRoles]'))
ALTER TABLE [dbo].[aspnet_UsersInRoles] CHECK CONSTRAINT [FK__aspnet_Us__UserI__30F848ED]
GO
/****** Object:  ForeignKey [FK_Ecshop_Articles__ArticleCategories]    Script Date: 12/10/2014 10:04:32 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_Articles__ArticleCategories]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_Articles]'))
ALTER TABLE [dbo].[Ecshop_Articles]  WITH CHECK ADD  CONSTRAINT [FK_Ecshop_Articles__ArticleCategories] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Ecshop_ArticleCategories] ([CategoryId])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_Articles__ArticleCategories]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_Articles]'))
ALTER TABLE [dbo].[Ecshop_Articles] CHECK CONSTRAINT [FK_Ecshop_Articles__ArticleCategories]
GO
/****** Object:  ForeignKey [FK_Ecshop_Attributes_ProductTypes]    Script Date: 12/10/2014 10:04:32 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_Attributes_ProductTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_Attributes]'))
ALTER TABLE [dbo].[Ecshop_Attributes]  WITH CHECK ADD  CONSTRAINT [FK_Ecshop_Attributes_ProductTypes] FOREIGN KEY([TypeId])
REFERENCES [dbo].[Ecshop_ProductTypes] ([TypeId])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_Attributes_ProductTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_Attributes]'))
ALTER TABLE [dbo].[Ecshop_Attributes] CHECK CONSTRAINT [FK_Ecshop_Attributes_ProductTypes]
GO
/****** Object:  ForeignKey [FK_Ecshop_AttributeValues_Attributes]    Script Date: 12/10/2014 10:04:32 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_AttributeValues_Attributes]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_AttributeValues]'))
ALTER TABLE [dbo].[Ecshop_AttributeValues]  WITH CHECK ADD  CONSTRAINT [FK_Ecshop_AttributeValues_Attributes] FOREIGN KEY([AttributeId])
REFERENCES [dbo].[Ecshop_Attributes] ([AttributeId])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_AttributeValues_Attributes]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_AttributeValues]'))
ALTER TABLE [dbo].[Ecshop_AttributeValues] CHECK CONSTRAINT [FK_Ecshop_AttributeValues_Attributes]
GO
/****** Object:  ForeignKey [FK_Ecshop_BalanceDetails_aspnet_Memberss]    Script Date: 12/10/2014 10:04:32 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_BalanceDetails_aspnet_Memberss]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_BalanceDetails]'))
ALTER TABLE [dbo].[Ecshop_BalanceDetails]  WITH CHECK ADD  CONSTRAINT [FK_Ecshop_BalanceDetails_aspnet_Memberss] FOREIGN KEY([UserId])
REFERENCES [dbo].[aspnet_Members] ([UserId])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_BalanceDetails_aspnet_Memberss]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_BalanceDetails]'))
ALTER TABLE [dbo].[Ecshop_BalanceDetails] CHECK CONSTRAINT [FK_Ecshop_BalanceDetails_aspnet_Memberss]
GO
/****** Object:  ForeignKey [FK_Ecshop_BalanceDrawRequest_aspnet_Memberss]    Script Date: 12/10/2014 10:04:32 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_BalanceDrawRequest_aspnet_Memberss]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_BalanceDrawRequest]'))
ALTER TABLE [dbo].[Ecshop_BalanceDrawRequest]  WITH CHECK ADD  CONSTRAINT [FK_Ecshop_BalanceDrawRequest_aspnet_Memberss] FOREIGN KEY([UserId])
REFERENCES [dbo].[aspnet_Members] ([UserId])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_BalanceDrawRequest_aspnet_Memberss]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_BalanceDrawRequest]'))
ALTER TABLE [dbo].[Ecshop_BalanceDrawRequest] CHECK CONSTRAINT [FK_Ecshop_BalanceDrawRequest_aspnet_Memberss]
GO
/****** Object:  ForeignKey [FK_BundlingProductsItems_Ecshop_BundlingProducts]    Script Date: 12/10/2014 10:04:32 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_BundlingProductsItems_Ecshop_BundlingProducts]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_BundlingProductItems]'))
ALTER TABLE [dbo].[Ecshop_BundlingProductItems]  WITH CHECK ADD  CONSTRAINT [FK_BundlingProductsItems_Ecshop_BundlingProducts] FOREIGN KEY([BundlingID])
REFERENCES [dbo].[Ecshop_BundlingProducts] ([BundlingID])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_BundlingProductsItems_Ecshop_BundlingProducts]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_BundlingProductItems]'))
ALTER TABLE [dbo].[Ecshop_BundlingProductItems] CHECK CONSTRAINT [FK_BundlingProductsItems_Ecshop_BundlingProducts]
GO
/****** Object:  ForeignKey [FK_Ecshop_CountDown_Products]    Script Date: 12/10/2014 10:04:32 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_CountDown_Products]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_CountDown]'))
ALTER TABLE [dbo].[Ecshop_CountDown]  WITH CHECK ADD  CONSTRAINT [FK_Ecshop_CountDown_Products] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Ecshop_Products] ([ProductId])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_CountDown_Products]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_CountDown]'))
ALTER TABLE [dbo].[Ecshop_CountDown] CHECK CONSTRAINT [FK_Ecshop_CountDown_Products]
GO
/****** Object:  ForeignKey [FK_Ecshop_CouponItems__Coupons]    Script Date: 12/10/2014 10:04:32 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_CouponItems__Coupons]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_CouponItems]'))
ALTER TABLE [dbo].[Ecshop_CouponItems]  WITH CHECK ADD  CONSTRAINT [FK_Ecshop_CouponItems__Coupons] FOREIGN KEY([CouponId])
REFERENCES [dbo].[Ecshop_Coupons] ([CouponId])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_CouponItems__Coupons]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_CouponItems]'))
ALTER TABLE [dbo].[Ecshop_CouponItems] CHECK CONSTRAINT [FK_Ecshop_CouponItems__Coupons]
GO
/****** Object:  ForeignKey [FK_Ecshop_Favorite_aspnet_Members]    Script Date: 12/10/2014 10:04:32 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_Favorite_aspnet_Members]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_Favorite]'))
ALTER TABLE [dbo].[Ecshop_Favorite]  WITH CHECK ADD  CONSTRAINT [FK_Ecshop_Favorite_aspnet_Members] FOREIGN KEY([UserId])
REFERENCES [dbo].[aspnet_Members] ([UserId])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_Favorite_aspnet_Members]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_Favorite]'))
ALTER TABLE [dbo].[Ecshop_Favorite] CHECK CONSTRAINT [FK_Ecshop_Favorite_aspnet_Members]
GO
/****** Object:  ForeignKey [FK_Ecshop_GiftShoppingCarts_aspnet_Members]    Script Date: 12/10/2014 10:04:32 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_GiftShoppingCarts_aspnet_Members]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_GiftShoppingCarts]'))
ALTER TABLE [dbo].[Ecshop_GiftShoppingCarts]  WITH CHECK ADD  CONSTRAINT [FK_Ecshop_GiftShoppingCarts_aspnet_Members] FOREIGN KEY([UserId])
REFERENCES [dbo].[aspnet_Members] ([UserId])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_GiftShoppingCarts_aspnet_Members]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_GiftShoppingCarts]'))
ALTER TABLE [dbo].[Ecshop_GiftShoppingCarts] CHECK CONSTRAINT [FK_Ecshop_GiftShoppingCarts_aspnet_Members]
GO
/****** Object:  ForeignKey [FK_Ecshop_GroupBuy_Products]    Script Date: 12/10/2014 10:04:32 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_GroupBuy_Products]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_GroupBuy]'))
ALTER TABLE [dbo].[Ecshop_GroupBuy]  WITH CHECK ADD  CONSTRAINT [FK_Ecshop_GroupBuy_Products] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Ecshop_Products] ([ProductId])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_GroupBuy_Products]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_GroupBuy]'))
ALTER TABLE [dbo].[Ecshop_GroupBuy] CHECK CONSTRAINT [FK_Ecshop_GroupBuy_Products]
GO
/****** Object:  ForeignKey [FK_Ecshop_GroupBuyCondition_Ecshop_GroupBuy]    Script Date: 12/10/2014 10:04:32 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_GroupBuyCondition_Ecshop_GroupBuy]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_GroupBuyCondition]'))
ALTER TABLE [dbo].[Ecshop_GroupBuyCondition]  WITH CHECK ADD  CONSTRAINT [FK_Ecshop_GroupBuyCondition_Ecshop_GroupBuy] FOREIGN KEY([GroupBuyId])
REFERENCES [dbo].[Ecshop_GroupBuy] ([GroupBuyId])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_GroupBuyCondition_Ecshop_GroupBuy]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_GroupBuyCondition]'))
ALTER TABLE [dbo].[Ecshop_GroupBuyCondition] CHECK CONSTRAINT [FK_Ecshop_GroupBuyCondition_Ecshop_GroupBuy]
GO
/****** Object:  ForeignKey [FK_Ecshop_Helps_HelpCategories]    Script Date: 12/10/2014 10:04:32 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_Helps_HelpCategories]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_Helps]'))
ALTER TABLE [dbo].[Ecshop_Helps]  WITH CHECK ADD  CONSTRAINT [FK_Ecshop_Helps_HelpCategories] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Ecshop_HelpCategories] ([CategoryId])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_Helps_HelpCategories]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_Helps]'))
ALTER TABLE [dbo].[Ecshop_Helps] CHECK CONSTRAINT [FK_Ecshop_Helps_HelpCategories]
GO
/****** Object:  ForeignKey [FK_Ecshop_Hotkeywords_Ecshop_Categories]    Script Date: 12/10/2014 10:04:32 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_Hotkeywords_Ecshop_Categories]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_Hotkeywords]'))
ALTER TABLE [dbo].[Ecshop_Hotkeywords]  WITH CHECK ADD  CONSTRAINT [FK_Ecshop_Hotkeywords_Ecshop_Categories] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Ecshop_Categories] ([CategoryId])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_Hotkeywords_Ecshop_Categories]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_Hotkeywords]'))
ALTER TABLE [dbo].[Ecshop_Hotkeywords] CHECK CONSTRAINT [FK_Ecshop_Hotkeywords_Ecshop_Categories]
GO
/****** Object:  ForeignKey [FK_Ecshop_InpourRequest_aspnet_Memberss]    Script Date: 12/10/2014 10:04:32 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_InpourRequest_aspnet_Memberss]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_InpourRequest]'))
ALTER TABLE [dbo].[Ecshop_InpourRequest]  WITH CHECK ADD  CONSTRAINT [FK_Ecshop_InpourRequest_aspnet_Memberss] FOREIGN KEY([UserId])
REFERENCES [dbo].[aspnet_Members] ([UserId])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_InpourRequest_aspnet_Memberss]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_InpourRequest]'))
ALTER TABLE [dbo].[Ecshop_InpourRequest] CHECK CONSTRAINT [FK_Ecshop_InpourRequest_aspnet_Memberss]
GO
/****** Object:  ForeignKey [FK_Ecshop_LeaveCommentReplys_LeaveComments]    Script Date: 12/10/2014 10:04:32 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_LeaveCommentReplys_LeaveComments]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_LeaveCommentReplys]'))
ALTER TABLE [dbo].[Ecshop_LeaveCommentReplys]  WITH CHECK ADD  CONSTRAINT [FK_Ecshop_LeaveCommentReplys_LeaveComments] FOREIGN KEY([LeaveId])
REFERENCES [dbo].[Ecshop_LeaveComments] ([LeaveId])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_LeaveCommentReplys_LeaveComments]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_LeaveCommentReplys]'))
ALTER TABLE [dbo].[Ecshop_LeaveCommentReplys] CHECK CONSTRAINT [FK_Ecshop_LeaveCommentReplys_LeaveComments]
GO
/****** Object:  ForeignKey [FK_Ecshop_OrderDebitNote_Orders]    Script Date: 12/10/2014 10:04:32 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_OrderDebitNote_Orders]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_OrderDebitNote]'))
ALTER TABLE [dbo].[Ecshop_OrderDebitNote]  WITH CHECK ADD  CONSTRAINT [FK_Ecshop_OrderDebitNote_Orders] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Ecshop_Orders] ([OrderId])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_OrderDebitNote_Orders]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_OrderDebitNote]'))
ALTER TABLE [dbo].[Ecshop_OrderDebitNote] CHECK CONSTRAINT [FK_Ecshop_OrderDebitNote_Orders]
GO
/****** Object:  ForeignKey [FK_Ecshop_OrderGifts_Orders]    Script Date: 12/10/2014 10:04:32 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_OrderGifts_Orders]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_OrderGifts]'))
ALTER TABLE [dbo].[Ecshop_OrderGifts]  WITH CHECK ADD  CONSTRAINT [FK_Ecshop_OrderGifts_Orders] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Ecshop_Orders] ([OrderId])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_OrderGifts_Orders]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_OrderGifts]'))
ALTER TABLE [dbo].[Ecshop_OrderGifts] CHECK CONSTRAINT [FK_Ecshop_OrderGifts_Orders]
GO
/****** Object:  ForeignKey [FK_Ecshop_OrderItems_Orders]    Script Date: 12/10/2014 10:04:32 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_OrderItems_Orders]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_OrderItems]'))
ALTER TABLE [dbo].[Ecshop_OrderItems]  WITH CHECK ADD  CONSTRAINT [FK_Ecshop_OrderItems_Orders] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Ecshop_Orders] ([OrderId])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_OrderItems_Orders]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_OrderItems]'))
ALTER TABLE [dbo].[Ecshop_OrderItems] CHECK CONSTRAINT [FK_Ecshop_OrderItems_Orders]
GO
/****** Object:  ForeignKey [FK_Ecshop_OrderRefund_Orders]    Script Date: 12/10/2014 10:04:32 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_OrderRefund_Orders]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_OrderRefund]'))
ALTER TABLE [dbo].[Ecshop_OrderRefund]  WITH CHECK ADD  CONSTRAINT [FK_Ecshop_OrderRefund_Orders] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Ecshop_Orders] ([OrderId])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_OrderRefund_Orders]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_OrderRefund]'))
ALTER TABLE [dbo].[Ecshop_OrderRefund] CHECK CONSTRAINT [FK_Ecshop_OrderRefund_Orders]
GO
/****** Object:  ForeignKey [FK_Ecshop_OrderReplace_Orders]    Script Date: 12/10/2014 10:04:32 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_OrderReplace_Orders]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_OrderReplace]'))
ALTER TABLE [dbo].[Ecshop_OrderReplace]  WITH CHECK ADD  CONSTRAINT [FK_Ecshop_OrderReplace_Orders] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Ecshop_Orders] ([OrderId])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_OrderReplace_Orders]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_OrderReplace]'))
ALTER TABLE [dbo].[Ecshop_OrderReplace] CHECK CONSTRAINT [FK_Ecshop_OrderReplace_Orders]
GO
/****** Object:  ForeignKey [FK_Ecshop_OrderReturns_Orders]    Script Date: 12/10/2014 10:04:32 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_OrderReturns_Orders]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_OrderReturns]'))
ALTER TABLE [dbo].[Ecshop_OrderReturns]  WITH CHECK ADD  CONSTRAINT [FK_Ecshop_OrderReturns_Orders] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Ecshop_Orders] ([OrderId])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_OrderReturns_Orders]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_OrderReturns]'))
ALTER TABLE [dbo].[Ecshop_OrderReturns] CHECK CONSTRAINT [FK_Ecshop_OrderReturns_Orders]
GO
/****** Object:  ForeignKey [FK_Ecshop_OrderSendNote_Orders]    Script Date: 12/10/2014 10:04:32 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_OrderSendNote_Orders]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_OrderSendNote]'))
ALTER TABLE [dbo].[Ecshop_OrderSendNote]  WITH CHECK ADD  CONSTRAINT [FK_Ecshop_OrderSendNote_Orders] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Ecshop_Orders] ([OrderId])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_OrderSendNote_Orders]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_OrderSendNote]'))
ALTER TABLE [dbo].[Ecshop_OrderSendNote] CHECK CONSTRAINT [FK_Ecshop_OrderSendNote_Orders]
GO
/****** Object:  ForeignKey [FK_Ecshop_PointDetails_aspnet_Memberss]    Script Date: 12/10/2014 10:04:32 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_PointDetails_aspnet_Memberss]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_PointDetails]'))
ALTER TABLE [dbo].[Ecshop_PointDetails]  WITH CHECK ADD  CONSTRAINT [FK_Ecshop_PointDetails_aspnet_Memberss] FOREIGN KEY([UserId])
REFERENCES [dbo].[aspnet_Members] ([UserId])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_PointDetails_aspnet_Memberss]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_PointDetails]'))
ALTER TABLE [dbo].[Ecshop_PointDetails] CHECK CONSTRAINT [FK_Ecshop_PointDetails_aspnet_Memberss]
GO
/****** Object:  ForeignKey [FK_Ecshop_ProductAttributes_Attributes]    Script Date: 12/10/2014 10:04:32 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_ProductAttributes_Attributes]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_ProductAttributes]'))
ALTER TABLE [dbo].[Ecshop_ProductAttributes]  WITH CHECK ADD  CONSTRAINT [FK_Ecshop_ProductAttributes_Attributes] FOREIGN KEY([AttributeId])
REFERENCES [dbo].[Ecshop_Attributes] ([AttributeId])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_ProductAttributes_Attributes]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_ProductAttributes]'))
ALTER TABLE [dbo].[Ecshop_ProductAttributes] CHECK CONSTRAINT [FK_Ecshop_ProductAttributes_Attributes]
GO
/****** Object:  ForeignKey [FK_Ecshop_ProductAttributes_Products]    Script Date: 12/10/2014 10:04:32 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_ProductAttributes_Products]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_ProductAttributes]'))
ALTER TABLE [dbo].[Ecshop_ProductAttributes]  WITH CHECK ADD  CONSTRAINT [FK_Ecshop_ProductAttributes_Products] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Ecshop_Products] ([ProductId])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_ProductAttributes_Products]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_ProductAttributes]'))
ALTER TABLE [dbo].[Ecshop_ProductAttributes] CHECK CONSTRAINT [FK_Ecshop_ProductAttributes_Products]
GO
/****** Object:  ForeignKey [FK_Ecshop_ProductConsultations_Products]    Script Date: 12/10/2014 10:04:32 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_ProductConsultations_Products]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_ProductConsultations]'))
ALTER TABLE [dbo].[Ecshop_ProductConsultations]  WITH CHECK ADD  CONSTRAINT [FK_Ecshop_ProductConsultations_Products] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Ecshop_Products] ([ProductId])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_ProductConsultations_Products]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_ProductConsultations]'))
ALTER TABLE [dbo].[Ecshop_ProductConsultations] CHECK CONSTRAINT [FK_Ecshop_ProductConsultations_Products]
GO
/****** Object:  ForeignKey [FK_Ecshop_ProductReviews_Products]    Script Date: 12/10/2014 10:04:32 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_ProductReviews_Products]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_ProductReviews]'))
ALTER TABLE [dbo].[Ecshop_ProductReviews]  WITH CHECK ADD  CONSTRAINT [FK_Ecshop_ProductReviews_Products] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Ecshop_Products] ([ProductId])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_ProductReviews_Products]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_ProductReviews]'))
ALTER TABLE [dbo].[Ecshop_ProductReviews] CHECK CONSTRAINT [FK_Ecshop_ProductReviews_Products]
GO
/****** Object:  ForeignKey [FK_Ecshop_ProductTag_Ecshop_Products]    Script Date: 12/10/2014 10:04:32 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_ProductTag_Ecshop_Products]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_ProductTag]'))
ALTER TABLE [dbo].[Ecshop_ProductTag]  WITH CHECK ADD  CONSTRAINT [FK_Ecshop_ProductTag_Ecshop_Products] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Ecshop_Products] ([ProductId])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_ProductTag_Ecshop_Products]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_ProductTag]'))
ALTER TABLE [dbo].[Ecshop_ProductTag] CHECK CONSTRAINT [FK_Ecshop_ProductTag_Ecshop_Products]
GO
/****** Object:  ForeignKey [FK_Ecshop_ProductTypeBrands_Ecshop_BrandCategories]    Script Date: 12/10/2014 10:04:32 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_ProductTypeBrands_Ecshop_BrandCategories]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_ProductTypeBrands]'))
ALTER TABLE [dbo].[Ecshop_ProductTypeBrands]  WITH CHECK ADD  CONSTRAINT [FK_Ecshop_ProductTypeBrands_Ecshop_BrandCategories] FOREIGN KEY([BrandId])
REFERENCES [dbo].[Ecshop_BrandCategories] ([BrandId])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_ProductTypeBrands_Ecshop_BrandCategories]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_ProductTypeBrands]'))
ALTER TABLE [dbo].[Ecshop_ProductTypeBrands] CHECK CONSTRAINT [FK_Ecshop_ProductTypeBrands_Ecshop_BrandCategories]
GO
/****** Object:  ForeignKey [FK_Ecshop_ProductTypeBrands_Ecshop_ProductTypes]    Script Date: 12/10/2014 10:04:32 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_ProductTypeBrands_Ecshop_ProductTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_ProductTypeBrands]'))
ALTER TABLE [dbo].[Ecshop_ProductTypeBrands]  WITH CHECK ADD  CONSTRAINT [FK_Ecshop_ProductTypeBrands_Ecshop_ProductTypes] FOREIGN KEY([ProductTypeId])
REFERENCES [dbo].[Ecshop_ProductTypes] ([TypeId])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_ProductTypeBrands_Ecshop_ProductTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_ProductTypeBrands]'))
ALTER TABLE [dbo].[Ecshop_ProductTypeBrands] CHECK CONSTRAINT [FK_Ecshop_ProductTypeBrands_Ecshop_ProductTypes]
GO
/****** Object:  ForeignKey [FK_Ecshop_PromotionMemberGrades_aspnet_MemberGrades]    Script Date: 12/10/2014 10:04:32 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_PromotionMemberGrades_aspnet_MemberGrades]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_PromotionMemberGrades]'))
ALTER TABLE [dbo].[Ecshop_PromotionMemberGrades]  WITH CHECK ADD  CONSTRAINT [FK_Ecshop_PromotionMemberGrades_aspnet_MemberGrades] FOREIGN KEY([GradeId])
REFERENCES [dbo].[aspnet_MemberGrades] ([GradeId])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_PromotionMemberGrades_aspnet_MemberGrades]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_PromotionMemberGrades]'))
ALTER TABLE [dbo].[Ecshop_PromotionMemberGrades] CHECK CONSTRAINT [FK_Ecshop_PromotionMemberGrades_aspnet_MemberGrades]
GO
/****** Object:  ForeignKey [FK_Ecshop_PromotionMemberGrades_Ecshop_Promotions]    Script Date: 12/10/2014 10:04:32 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_PromotionMemberGrades_Ecshop_Promotions]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_PromotionMemberGrades]'))
ALTER TABLE [dbo].[Ecshop_PromotionMemberGrades]  WITH CHECK ADD  CONSTRAINT [FK_Ecshop_PromotionMemberGrades_Ecshop_Promotions] FOREIGN KEY([ActivityId])
REFERENCES [dbo].[Ecshop_Promotions] ([ActivityId])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_PromotionMemberGrades_Ecshop_Promotions]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_PromotionMemberGrades]'))
ALTER TABLE [dbo].[Ecshop_PromotionMemberGrades] CHECK CONSTRAINT [FK_Ecshop_PromotionMemberGrades_Ecshop_Promotions]
GO
/****** Object:  ForeignKey [FK_Ecshop_PromotionProducts_Ecshop_Products]    Script Date: 12/10/2014 10:04:32 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_PromotionProducts_Ecshop_Products]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_PromotionProducts]'))
ALTER TABLE [dbo].[Ecshop_PromotionProducts]  WITH CHECK ADD  CONSTRAINT [FK_Ecshop_PromotionProducts_Ecshop_Products] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Ecshop_Products] ([ProductId])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_PromotionProducts_Ecshop_Products]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_PromotionProducts]'))
ALTER TABLE [dbo].[Ecshop_PromotionProducts] CHECK CONSTRAINT [FK_Ecshop_PromotionProducts_Ecshop_Products]
GO
/****** Object:  ForeignKey [FK_Ecshop_PromotionProducts_Ecshop_Promotions]    Script Date: 12/10/2014 10:04:32 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_PromotionProducts_Ecshop_Promotions]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_PromotionProducts]'))
ALTER TABLE [dbo].[Ecshop_PromotionProducts]  WITH CHECK ADD  CONSTRAINT [FK_Ecshop_PromotionProducts_Ecshop_Promotions] FOREIGN KEY([ActivityId])
REFERENCES [dbo].[Ecshop_Promotions] ([ActivityId])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_PromotionProducts_Ecshop_Promotions]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_PromotionProducts]'))
ALTER TABLE [dbo].[Ecshop_PromotionProducts] CHECK CONSTRAINT [FK_Ecshop_PromotionProducts_Ecshop_Promotions]
GO
/****** Object:  ForeignKey [FK_Ecshop_ShippingRegions_ShippingTypes]    Script Date: 12/10/2014 10:04:32 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_ShippingRegions_ShippingTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_ShippingRegions]'))
ALTER TABLE [dbo].[Ecshop_ShippingRegions]  WITH CHECK ADD  CONSTRAINT [FK_Ecshop_ShippingRegions_ShippingTypes] FOREIGN KEY([TemplateId])
REFERENCES [dbo].[Ecshop_ShippingTemplates] ([TemplateId])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_ShippingRegions_ShippingTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_ShippingRegions]'))
ALTER TABLE [dbo].[Ecshop_ShippingRegions] CHECK CONSTRAINT [FK_Ecshop_ShippingRegions_ShippingTypes]
GO
/****** Object:  ForeignKey [FK_Ecshop_ShippingTypeGroups_ShippingTemplates]    Script Date: 12/10/2014 10:04:32 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_ShippingTypeGroups_ShippingTemplates]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_ShippingTypeGroups]'))
ALTER TABLE [dbo].[Ecshop_ShippingTypeGroups]  WITH CHECK ADD  CONSTRAINT [FK_Ecshop_ShippingTypeGroups_ShippingTemplates] FOREIGN KEY([TemplateId])
REFERENCES [dbo].[Ecshop_ShippingTemplates] ([TemplateId])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_ShippingTypeGroups_ShippingTemplates]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_ShippingTypeGroups]'))
ALTER TABLE [dbo].[Ecshop_ShippingTypeGroups] CHECK CONSTRAINT [FK_Ecshop_ShippingTypeGroups_ShippingTemplates]
GO
/****** Object:  ForeignKey [FK_Ecshop_ShippingTypes_ShippingTemplates]    Script Date: 12/10/2014 10:04:32 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_ShippingTypes_ShippingTemplates]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_ShippingTypes]'))
ALTER TABLE [dbo].[Ecshop_ShippingTypes]  WITH CHECK ADD  CONSTRAINT [FK_Ecshop_ShippingTypes_ShippingTemplates] FOREIGN KEY([TemplateId])
REFERENCES [dbo].[Ecshop_ShippingTemplates] ([TemplateId])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_ShippingTypes_ShippingTemplates]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_ShippingTypes]'))
ALTER TABLE [dbo].[Ecshop_ShippingTypes] CHECK CONSTRAINT [FK_Ecshop_ShippingTypes_ShippingTemplates]
GO
/****** Object:  ForeignKey [FK_Ecshop_ShoppingCarts_aspnet_Members]    Script Date: 12/10/2014 10:04:32 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_ShoppingCarts_aspnet_Members]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_ShoppingCarts]'))
ALTER TABLE [dbo].[Ecshop_ShoppingCarts]  WITH CHECK ADD  CONSTRAINT [FK_Ecshop_ShoppingCarts_aspnet_Members] FOREIGN KEY([UserId])
REFERENCES [dbo].[aspnet_Members] ([UserId])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_ShoppingCarts_aspnet_Members]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_ShoppingCarts]'))
ALTER TABLE [dbo].[Ecshop_ShoppingCarts] CHECK CONSTRAINT [FK_Ecshop_ShoppingCarts_aspnet_Members]
GO
/****** Object:  ForeignKey [FK_Ecshop_SKUItems_SKUs]    Script Date: 12/10/2014 10:04:32 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_SKUItems_SKUs]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_SKUItems]'))
ALTER TABLE [dbo].[Ecshop_SKUItems]  WITH CHECK ADD  CONSTRAINT [FK_Ecshop_SKUItems_SKUs] FOREIGN KEY([SkuId])
REFERENCES [dbo].[Ecshop_SKUs] ([SkuId])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_SKUItems_SKUs]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_SKUItems]'))
ALTER TABLE [dbo].[Ecshop_SKUItems] CHECK CONSTRAINT [FK_Ecshop_SKUItems_SKUs]
GO
/****** Object:  ForeignKey [FK_Ecshop_SKUMemberPrice_aspnet_MemberGrades]    Script Date: 12/10/2014 10:04:32 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_SKUMemberPrice_aspnet_MemberGrades]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_SKUMemberPrice]'))
ALTER TABLE [dbo].[Ecshop_SKUMemberPrice]  WITH CHECK ADD  CONSTRAINT [FK_Ecshop_SKUMemberPrice_aspnet_MemberGrades] FOREIGN KEY([GradeId])
REFERENCES [dbo].[aspnet_MemberGrades] ([GradeId])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_SKUMemberPrice_aspnet_MemberGrades]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_SKUMemberPrice]'))
ALTER TABLE [dbo].[Ecshop_SKUMemberPrice] CHECK CONSTRAINT [FK_Ecshop_SKUMemberPrice_aspnet_MemberGrades]
GO
/****** Object:  ForeignKey [FK_Ecshop_SKUMemberPrice_SKUs]    Script Date: 12/10/2014 10:04:32 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_SKUMemberPrice_SKUs]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_SKUMemberPrice]'))
ALTER TABLE [dbo].[Ecshop_SKUMemberPrice]  WITH CHECK ADD  CONSTRAINT [FK_Ecshop_SKUMemberPrice_SKUs] FOREIGN KEY([SkuId])
REFERENCES [dbo].[Ecshop_SKUs] ([SkuId])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_SKUMemberPrice_SKUs]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_SKUMemberPrice]'))
ALTER TABLE [dbo].[Ecshop_SKUMemberPrice] CHECK CONSTRAINT [FK_Ecshop_SKUMemberPrice_SKUs]
GO
/****** Object:  ForeignKey [FK_Ecshop_SKUs_Products]    Script Date: 12/10/2014 10:04:32 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_SKUs_Products]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_SKUs]'))
ALTER TABLE [dbo].[Ecshop_SKUs]  WITH CHECK ADD  CONSTRAINT [FK_Ecshop_SKUs_Products] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Ecshop_Products] ([ProductId])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_SKUs_Products]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_SKUs]'))
ALTER TABLE [dbo].[Ecshop_SKUs] CHECK CONSTRAINT [FK_Ecshop_SKUs_Products]
GO
/****** Object:  ForeignKey [FK_Ecshop_UserShippingAddresses_aspnet_Memberss]    Script Date: 12/10/2014 10:04:32 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_UserShippingAddresses_aspnet_Memberss]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_UserShippingAddresses]'))
ALTER TABLE [dbo].[Ecshop_UserShippingAddresses]  WITH CHECK ADD  CONSTRAINT [FK_Ecshop_UserShippingAddresses_aspnet_Memberss] FOREIGN KEY([UserId])
REFERENCES [dbo].[aspnet_Members] ([UserId])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_UserShippingAddresses_aspnet_Memberss]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_UserShippingAddresses]'))
ALTER TABLE [dbo].[Ecshop_UserShippingAddresses] CHECK CONSTRAINT [FK_Ecshop_UserShippingAddresses_aspnet_Memberss]
GO
/****** Object:  ForeignKey [FK_Ecshop_VoteItems_Votes]    Script Date: 12/10/2014 10:04:32 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_VoteItems_Votes]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_VoteItems]'))
ALTER TABLE [dbo].[Ecshop_VoteItems]  WITH CHECK ADD  CONSTRAINT [FK_Ecshop_VoteItems_Votes] FOREIGN KEY([VoteId])
REFERENCES [dbo].[Ecshop_Votes] ([VoteId])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecshop_VoteItems_Votes]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecshop_VoteItems]'))
ALTER TABLE [dbo].[Ecshop_VoteItems] CHECK CONSTRAINT [FK_Ecshop_VoteItems_Votes]
GO
/****** Object:  ForeignKey [FK_Taobao_Products_Ecshop_Products]    Script Date: 12/10/2014 10:04:32 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Taobao_Products_Ecshop_Products]') AND parent_object_id = OBJECT_ID(N'[dbo].[Taobao_Products]'))
ALTER TABLE [dbo].[Taobao_Products]  WITH CHECK ADD  CONSTRAINT [FK_Taobao_Products_Ecshop_Products] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Ecshop_Products] ([ProductId])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Taobao_Products_Ecshop_Products]') AND parent_object_id = OBJECT_ID(N'[dbo].[Taobao_Products]'))
ALTER TABLE [dbo].[Taobao_Products] CHECK CONSTRAINT [FK_Taobao_Products_Ecshop_Products]
GO
