<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="RolePermissions.aspx.cs" Inherits="EcShop.UI.Web.Admin.RolePermissions" %>

<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<%@ Import Namespace="EcShop.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
<style>
    .PurviewItem { clear: both; }
    .PurviewItemSave { float: left; height: 25px; line-height: 25px; padding-left: 20px; margin: 0 0 0 5px; _margin-left: 3px; vertical-align: middle; background: url(images/saveitem.gif) no-repeat 0px 5px; padding-left: 20px; }
    .PurviewItem ul { width: 850px; list-style: none; }
    .PurviewItem ul li { float: left; height: 20px; line-height: 20px; margin-right: 8px; width: 140px; }
    .PurviewItem ol { clear: both; padding-left: 98px; }
    .PurviewItem ol li { float: left; height: 20px; line-height: 20px; margin-right: 8px; width: 140px; }
    .PurviewItemDivide { height: 1px; width: 100%; overflow: hidden; background-color: #ddd; margin: 5px 0; }
    .PurviewItemBackground { background: #E1F3FF; border: 1px solid #8ACEFF; }
    .clear { clear: both; }
</style>
<script>
	
	
    $(function(){
		//var roleId = UrlParm.parm('roleid');
		var roleid_str = [];
		var roleid_str = $('#aspnetForm').attr('action').split('=');
		var roleid = roleid_str[1];
		var url = '/API/AuthValid.ashx?action=GetAllMenuByRoleId&status=1&menuId=1010&roleId=' + roleid;
		var str = '';
		$.ajax({
			url: url,
			type: 'get',
			dataType: 'text',
		
			success:function(data){
				var jsonData = eval("("+data+")");
				for(var i=0;i<jsonData.length;i++){
					str += '<div style="border:1px solid #ccc;">' + jsonData[i].PrivilegeName + '</div>';
					for(var j=0;j<jsonData[i].SubMenuItem.length;j++){
						str += '<span style="border:1px solid #ff4777;">' + jsonData[i].SubMenuItem[j].PrivilegeName + '</span>';
						for(var k=0;k<jsonData[i].SubMenuItem[j].SubMenuItem.length;k++){
							str += '<span>' + jsonData[i].SubMenuItem[j].SubMenuItem[k].PrivilegeName + '</span>';
						}
					}
				}
				$('#grdGroupList').append(str);
			},
			error:function(){
				
			}
		});
    })
</script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <asp:HiddenField  ID="hf_menuValues" runat="server"/>
    <div class="dataarea mainwidth databody">
        <div class="title">
            <em>
                <img src="../images/03.gif" width="32" height="32" /></em>
            <h1>设置部门权限</h1>
            <span>根据不同的部门设置不同的权限，方便控制商城的管理 </span>
        </div>
        <div class="datalist">
            <table width="100%" height="30px" border="0" cellspacing="0">
                <tr class="table_title">
                    <td width="9%" class="td_left"><strong>当前部门：</strong></td>
                    <td align="left" class="td_right"><span style="font-weight: 800;"><strong>
                        <asp:Literal runat="server" ID="lblRoleName"></asp:Literal></strong></span></td>
                </tr>

            </table>

            <div style="margin-left: 15px; margin-top: 10px;">
                <span class="submit_btnquanxuan">
                    <asp:LinkButton ID="btnSetTop" runat="server" Text="保存" /></span>
                <span class="submit_btnchexiao"><a href="Roles.aspx">返回</a></span>
            </div>
            
            <div id="grdGroupList" class="grdGroupList clear" style="padding-left:10px;margin-top:5px">
            	
            </div>
            
            <!--<div class="grdGroupList clear" style="padding-left: 10px; margin-top: 5px">

                <div style="color: Blue; font-weight: 700;">
                    <label>
                        <asp:CheckBox ID="cbAll" runat="server" />全部选定</label>
                </div>
                <div class="PurviewItemDivide"></div>
                <div style="font-weight: 700; color: #000066">
                    <label>
                        <asp:CheckBox ID="cbSummary" runat="server" />后台即时营业信息</label>
                </div>
                <div class="PurviewItemDivide"></div>
                <div style="font-weight: 700; color: #000066">
                    <label>
                        <asp:CheckBox ID="cbShop" runat="server" />商城管理</label>
                </div>
                <div class="PurviewItemDivide"></div>
                <div style="padding-left: 20px;">
                    <div class="PurviewItem">
                        <ul>
                            <li style="width: 90px; font-weight: 700;">基本设置：</li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbSiteContent" runat="server" />网店基本设置</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbEmailSettings" runat="server" />邮件设置</label></li>
                            <li style="margin-left: -10px;">
                                <label>
                                    <asp:CheckBox ID="cbSMSSettings" runat="server" />手机短信设置</label></li>
                        </ul>
                    </div>

                    <div class="PurviewItem">
                        <ul>
                            <li style="width: 90px; font-weight: 700;">支付设置：</li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbPaymentModes" runat="server" />支付方式</label></li>
                        </ul>
                    </div>
                    <div class="PurviewItem">
                        <ul>
                            <li style="width: 90px; font-weight: 700;">配送设置：</li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbShippingModes" runat="server" />配送方式列表</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbShippingTemplets" runat="server" />运费模板</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbExpressComputerpes" runat="server" />物流公司</label></li>
                        </ul>
                    </div>


                    <div class="PurviewItem">
                        <ul>
                            <li style="width: 100px; font-weight: 700;">邮件短信模板：</li>
                            <li style="margin-left: -10px;">
                                <label>
                                    <asp:CheckBox ID="cbMessageTemplets" runat="server" />邮件短信模板</label></li>

                        </ul>
                    </div>

                    <div class="PurviewItem">
                        <ul>
                            <li style="width: 90px; font-weight: 700;">图库管理：</li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbPictureMange" runat="server" />图库管理</label></li>
                        </ul>
                    </div>
                </div>


                <div style="clear: both; margin-top: 40px; font-weight: 700; color: #000066">
                    <label>
                        <asp:CheckBox ID="cbPageManger" runat="server" />页面管理</label>
                </div>
                <div class="PurviewItemDivide"></div>
                <div style="padding-left: 20px;">
                    <div class="PurviewItem">
                        <ul>
                            <li style="width: 90px; font-weight: 700;">模板管理：</li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbManageThemes" runat="server" />模板管理</label></li>
                        </ul>
                    </div>
                    <div class="PurviewItem">
                        <ul>
                            <li style="width: 90px; font-weight: 700;">内容管理：</li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbAfficheList" runat="server" />商城公告</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbHelpCategories" runat="server" />帮助分类</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbHelpList" runat="server" />帮助管理</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbArticleCategories" runat="server" />文章分类</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbArticleList" runat="server" />文章管理</label></li>
                        </ul>
                        <ol>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbFriendlyLinks" runat="server" />友情链接</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbManageHotKeywords" runat="server" />热门关键字</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbVotes" runat="server" />投票调查</label></li>
                        </ol>
                    </div>
                </div>


                <div style="clear: both; margin-top: 40px; font-weight: 700; color: #000066">
                    <label>
                        <asp:CheckBox ID="cbProductCatalog" runat="server" />商品管理</label>
                </div>
                <div class="PurviewItemDivide"></div>
                <div style="padding-left: 20px;">


                    <div class="PurviewItem">
                        <ul>
                            <li style="width: 90px; font-weight: 700;">商品管理：</li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbManageProducts" runat="server" />商品：</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbManageProductsView" runat="server" />查看</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbManageProductsAdd" runat="server" />添加</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbManageProductsEdit" runat="server" />编辑</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbManageProductsDelete" runat="server"/>删除</label></li>
                            <li style="width: 90px">&nbsp;</li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbInStock" runat="server" />入库</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbManageProductsUp" runat="server" />上架</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbManageProductsDown" runat="server" />下架</label></li>
                            <li style="width: 90px">&nbsp;</li>
                            <li>&nbsp;</li>
                        </ul>
                        <ol>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbProductUnclassified" runat="server" />未分类商品</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbSubjectProducts" runat="server" />商品标签管理</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbProductBatchUpload" runat="server" />批量上传</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbProductBatchExport" runat="server" />批量导出</label></li>
                        </ol>
                    </div>

                    <div class="PurviewItem">
                        <ul>
                            <li style="width: 90px; font-weight: 700;">商品类型：</li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbProductTypes" runat="server" />商品类型：</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbProductTypesView" runat="server" />查看</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbProductTypesAdd" runat="server" />添加</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbProductTypesEdit" runat="server" />编辑</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbProductTypesDelete" runat="server" />删除</label></li>
                        </ul>
                    </div>
                    <div class="PurviewItem">
                        <ul>
                            <li style="width: 90px; font-weight: 700;">商品分类：</li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbManageCategories" runat="server" />商品分类：</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbManageCategoriesView" runat="server" />查看</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbManageCategoriesAdd" runat="server" />添加</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbManageCategoriesEdit" runat="server" />编辑</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbManageCategoriesDelete" runat="server" />删除</label></li>
                        </ul>
                    </div>
                    <div class="PurviewItem">
                        <ul>
                            <li style="width: 90px; font-weight: 700;">品牌分类：</li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbBrandCategories" runat="server" />品牌分类</label></li>
                        </ul>
                    </div>
                    <div class="PurviewItem">
                        <ul>
                            <li style="width: 90px; font-weight: 700;">移动端专题：</li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbTopicManager" runat="server" />专题管理：</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbTopicAdd" runat="server" />添加</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbTopicEdit" runat="server" />编辑</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbTopicDelete" runat="server" />删除</label></li>
                        </ul>
                    </div>
                    <div class="PurviewItem">
                        <ul>
                            <li style="width: 90px; font-weight: 700;">原产地：</li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbImportSourceTypeManager" runat="server" />原产地：</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbImportSourceTypeView" runat="server" />查看</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbImportSourceTypeAdd" runat="server" />添加</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbImportSourceTypeEdit" runat="server" />编辑</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbImportSourceTypeDelete" runat="server" />删除</label></li>
                        </ul>
                    </div>
                    <div class="PurviewItem">
                        <ul>
                            <li style="width: 90px; font-weight: 700;">税率管理：</li>
                            <li>
                               <label>
                                   <asp:CheckBox ID="cbTaxRateManager" runat="server"/>税率管理：</label></li>
                            <li>
                               <label>
                                   <asp:CheckBox ID="cbTaxRateView" runat="server"/>查看</label></li>
                            <li>
                               <label>
                                   <asp:CheckBox ID="cbTaxRateAdd" runat="server"/>添加</label></li>
                            <li>
                              <label>
                                 <asp:CheckBox ID="cbTaxRateEdit" runat="server"/>编辑</label></li>
                            <li>
                              <label>
                                <asp:CheckBox ID="cbTaxRateDelete" runat="server"/>删除</label></li>
                        </ul>
                    </div>
                    <div class="PurviewItem">
                        <ul>
                            <li style="width: 90px; font-weight: 700;">供货商管理：</li>
                              <li>
                               <label>
                                   <asp:CheckBox ID="cbSupplierManager" runat="server"/>供货商管理：</label></li>
                            <li>
                               <label>
                                   <asp:CheckBox ID="cbSupplierView" runat="server"/>查看</label></li>
                            <li>
                               <label>
                                   <asp:CheckBox ID="cbSupplierAdd" runat="server"/>添加</label></li>
                            <li>
                              <label>
                                  <asp:CheckBox ID="cbSupplierEdit" runat="server"/>编辑</label></li>
                            <li>
                              <label>
                                  <asp:CheckBox ID="cbSupplierDelete" runat="server"/>删除</label></li>
                            <li style="width: 90px; font-weight:700;"></li>
                            <li>
                              <label>
                                  <asp:CheckBox ID="cbSupplierAddManager" runat="server"/>添加管理员</label></li>
                        </ul>
                   </div>



                <div style="clear: both; margin-top: 40px; font-weight: 700; color: #000066">
                    <label>
                        <asp:CheckBox ID="cbSales" runat="server" />订单管理</label>
                </div>
                <div class="PurviewItemDivide"></div>
                <div style="padding-left: 20px;">
                    <div class="PurviewItem">
                        <ul>
                            <li style="width: 90px; font-weight: 700;">订单管理：</li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbManageOrder" runat="server" />订单管理：</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbManageOrderView" runat="server" />查看</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbManageOrderDelete" runat="server" />删除</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbManageOrderEdit" runat="server" />修改</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbManageOrderConfirm" runat="server" />确认收款</label></li>
                            <li style="width: 90px;">&nbsp;</li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbManageOrderSendedGoods" runat="server" />订单发货</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbExpressPrint" runat="server" />快递单打印</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbManageOrderRemark" runat="server" />管理员备注</label></li>
                        </ul>

                    </div>

                    <div class="PurviewItem">
                        <ul>
                            <li style="width: 90px; font-weight: 700;">快递单模板：</li>
                            <li style="width: 90px;">
                                <label>
                                    <asp:CheckBox ID="cbExpressTemplates" runat="server" />快递单模板</label></li>
                        </ul>
                    </div>
                    <div class="PurviewItem">
                        <ul>
                            <li style="width: 90px; font-weight: 700;">发货人信息：</li>
                            <li style="width: 90px;">
                                <label>
                                    <asp:CheckBox ID="cbShipper" runat="server" />发货人信息</label></li>
                        </ul>
                    </div>
                    <div class="PurviewItem">
                        <ul>
                            <li style="width: 90px; font-weight: 700;">退换货设置：</li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbOrderRefundApply" runat="server" />退款申请单：</label></li>
                             <li>
                               <label>
                                   <asp:CheckBox ID="cbOrderRefundApplyView" runat="server"/>查看</label></li>
                            <li>
                              <label>
                                <asp:CheckBox ID="cbOrderRefundApplyDelete" runat="server"/>删除</label></li>
                            <li>
                              <label>
                                  <asp:CheckBox ID="cbOrderRefundApplyAccept" runat="server"/>确认退款</label></li>
                            <li>
                              <label>
                                  <asp:CheckBox ID="cbOrderRefundApplyRefuse" runat="server"/>拒绝退款</label></li>
                            <li style="width: 90px;"></li>
                            <li>
                               <label>
                                   <asp:CheckBox ID="cbRefundOrderApplySelExcel" runat="server"/>选中单号导出</label></li>
                            <li>
                               <label>
                                   <asp:CheckBox ID="cbRefundOrderApplyTimeExcel" runat="server"/>按时间段导出</label></li>
                        </ul>
                    </div>
                    <div class="PurviewItem">
                        <ul>
                            <li style="width: 90px; font-weight: 700;"></li>
                            <li>
                               <label>
                                   <asp:CheckBox ID="cbOrderReturnsApply" runat="server"/>退货申请单：</label></li>
                            <li>
                               <label>
                                   <asp:CheckBox ID="cbOrderReturnsApplyView" runat="server"/>查看</label></li>
                            <li>
                              <label>
                                  <asp:CheckBox ID="cbOrderReturnsApplyDelete" runat="server"/>删除</label></li>
                            <li>
                              <label>
                                  <asp:CheckBox ID="cbOrderReturnsApplyAccept" runat="server"/>确认退货</label></li>
                            <li>
                              <label>
                                  <asp:CheckBox ID="cbOrderReturnsApplyRefuse" runat="server"/>拒绝退货</label></li>
                            <li style="width: 90px;"></li>
                            <li>
                               <label>
                                   <asp:CheckBox ID="cbOrderReturnsApplySelExcel" runat="server"/>选中单号导出</label></li>
                            <li>
                               <label>
                                   <asp:CheckBox ID="cbOrderReturnsApplyTimeExcel" runat="server"/>按时间段导出</label></li>
                        </ul>
                    </div>
                    <div class="PurviewItem">
                        <ul>
                            <li style="width: 90px; font-weight: 700;"></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbOrderReplaceApply" runat="server" />换货申请单：</label></li>
                            <li>
                               <label>
                                   <asp:CheckBox ID="cbOrderReplaceApplyView" runat="server"/>查看</label></li>
                            <li>
                              <label>
                                  <asp:CheckBox ID="cbOrderReplaceApplyDelete" runat="server"/>删除</label></li>
                            <li>
                              <label>
                                  <asp:CheckBox ID="cbOrderReplaceApplyAccept" runat="server"/>确认换货</label></li>
                            <li>
                              <label>
                                  <asp:CheckBox ID="cbOrderReplaceApplyRefuse" runat="server"/>拒绝换货</label></li>
                            <li style="width: 90px;"></li>
                            <li>
                            <label>
                                  <asp:CheckBox ID="cbOrderReplaceApplySelExcel" runat="server"/>选中单号导出</label></li>
                            <li>
                              <label>
                                  <asp:CheckBox ID="cbOrderReplaceApplyTimeExcel" runat="server"/>按时间段导出</label></li>
                        </ul>
                    </div>

                    <div class="PurviewItem">
                        <ul>
                            <li style="width: 90px; font-weight: 700;">退款单：</li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbRefundOrder" runat="server" />退款单：</label></li>
                             <li>
                               <label>
                                   <asp:CheckBox ID="cbRefundOrderView" runat="server"/>查看</label></li>
                            <li>
                              <label>
                                <asp:CheckBox ID="cbRefundOrderDelete" runat="server"/>删除</label></li>
                            <li>
                               <label>
                                   <asp:CheckBox ID="cbRefundOrderExcel" runat="server"/>生成报告</label></li>
                        </ul>
                    </div>

                    <div class="PurviewItem">
                        <ul>
                            <li style="width: 90px; font-weight: 700;">退货单：</li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbReturnOrder" runat="server" />退货单：</label></li>
                             <li>
                               <label>
                                   <asp:CheckBox ID="cbReturnOrderView" runat="server"/>查看</label></li>
                            <li>
                              <label>
                                <asp:CheckBox ID="cbReturnOrderDelete" runat="server"/>删除</label></li>
                            <li>
                               <label>
                                   <asp:CheckBox ID="cbReturnOrderExcel" runat="server"/>生成报告</label></li>
                        </ul>
                    </div>

                    

                </div>
                <asp:Panel ID="vPanel" runat="server">
                    <div style="clear: both;margin-top:40px;font-weight:700;color: #000066;">
                        <label>
                            <asp:CheckBox ID="cbManageVShop" runat="server" />微商城管理</label>
                    </div>
                    <div class="PurviewItemDivide"></div>
                    <div style="padding-left: 20px;">
                        <div class="PurviewItem">
                            <ul>
                                <li style="width: 90px; font-weight: 700;">微信配置：</li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbVServerConfig" runat="server" />基本配置</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbManageMenu" runat="server" />自定义菜单配置</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbAddMenu" runat="server" />自定义菜单添加</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbEditMenu" runat="server" />自定义菜单编辑</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbDeleteMenu" runat="server" />自定义菜单删除</label></li>
                                <li style="display: none;">
                                    <label>
                                        <asp:CheckBox ID="cbVIPcard" runat="server" />会员卡</label></li>
                                <li style="width: 90px;">&nbsp;</li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbCustomeReplyManager" runat="server" />自定义回复管理</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbDeleteCustomeReply" runat="server" />自定义回复删除</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbAddReplyOnkey" runat="server" />文本回复添加</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbEditReplyOnkey" runat="server" />文本回复编辑</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbAddArticle" runat="server" />单图文回复添加</label></li>
                                <li style="width: 90px;">&nbsp;</li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbEditArticle" runat="server" />单图文回复编辑</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbMutiArticleAdd" runat="server" />多图文回复添加</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbMutiArticleEdit" runat="server" />多图文回复编辑</label></li>

                            </ul>
                        </div>

                        <div class="PurviewItem">
                            <ul>
                                <li style="width: 90px; font-weight: 700;">首页配置：</li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbTopicList" runat="server" />微信专题</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbManageVThemes" runat="server" />模板选择</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbSetVThemes" runat="server" />模板配置</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbvIconSet" runat="server" />图标配置</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbvProductSet" runat="server" />商品配置</label></li>
                                <li style="width: 90px;">&nbsp;</li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbvBannerManage" runat="server" />轮播图管理</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbvBannerAdd" runat="server" />轮播图添加</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbvBannerEdit" runat="server" />轮播图编辑</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbvBannerDelete" runat="server" />轮播图删除</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbvIsViewEdit" runat="server" />可视化编辑</label></li>
                            </ul>
                        </div>



                        <div class="PurviewItem">
                            <ul>
                                <li style="width: 90px; font-weight: 700;">微信活动：</li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbManageLotteryActivity" runat="server" />大转盘/刮刮卡/砸金蛋</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbAddLotteryActivity" runat="server" />添加</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbEditLotteryActivity" runat="server" />编辑</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbDeleteLotteryActivity" runat="server" />删除</label></li>
                                <li>&nbsp;</li>
                                <li style="width: 90px;">&nbsp;</li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbManageActivity" runat="server" />微报名</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbAddActivity" runat="server" />添加</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbEditActivity" runat="server" />编辑</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbDeleteActivity" runat="server" />删除</label></li>
                                <li>&nbsp;</li>
                                <li style="width: 90px;">&nbsp;</li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbManageLotteryTicket" runat="server" />微抽奖</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbAddLotteryTicket" runat="server" />添加</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbEditLotteryTicket" runat="server" />编辑</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbDeleteLotteryTicket" runat="server" />删除</label></li>
                            </ul>
                        </div>

                        <div class="PurviewItem">
                            <ul>
                                <li style="width: 90px; font-weight: 700;">支付设置：</li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbVpayConfig" runat="server" />微信支付设置</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbvMobileAlipaySet" runat="server" />手机支付宝设置</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbvShengPaySet" runat="server" />盛付通支付设置</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbvOfflinePaySet" runat="server" />线下支付设置</label></li>
                                <li>&nbsp;</li>
                            </ul>
                        </div>
                    </div>
                </asp:Panel>
                <asp:Panel ID="wapPanel" runat="server">
                    <div style="clear: both; margin-top: 40px; font-weight: 700; color: #000066;">
                        <label>
                            <asp:CheckBox ID="cbWapManage" runat="server" />触屏版管理</label>
                    </div>
                    <div class="PurviewItemDivide"></div>
                    <div style="padding-left: 20px;">
                        <div class="PurviewItem">
                            <ul>
                                <li style="width: 90px; font-weight: 700;">首页配置：</li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbWapManageThemes" runat="server" />模板选择</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbWapThemesSet" runat="server" />模板配置</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbWapHomeTopicSet" runat="server" />专题专题</label></li>

                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbWapIconSet" runat="server" />图标配置</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbWapProductSet" runat="server" />商品配置</label></li>
                                <li style="width: 90px;">&nbsp;</li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbWapBannerManage" runat="server" />轮播图管理</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbWapBannerAdd" runat="server" />轮播图添加</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbWapBannerEdit" runat="server" />轮播图编辑</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbWapBannerDelete" runat="server" />轮播图删除</label></li>
                            </ul>
                        </div>

                        <div class="PurviewItem">
                            <ul>
                                <li style="width: 90px; font-weight: 700;">支付设置：</li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbWapMobileAlipaySet" runat="server" />手机支付宝设置</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbWapShengPaySet" runat="server" />盛付通支付设置</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbWapOfflinePaySet" runat="server" />线下支付设置</label></li>
                                <li>&nbsp;</li>
                            </ul>
                        </div>
                    </div>
                </asp:Panel>

                <asp:Panel ID="appPaenl" runat="server">
                    <div style="clear: both; margin-top: 40px; font-weight: 700; color: #000066;display:none;">
                        <label>
                            <asp:CheckBox ID="cbAppManage" runat="server" />APP管理</label>
                    </div>
                    <div class="PurviewItemDivide"></div>
                    <div style="padding-left: 20px;">
                        <div class="PurviewItem">
                            <ul>
                                <li style="width: 90px; font-weight: 700;">首页配置：</li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbAppADImageSet" runat="server" />广告图配置</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbAppADImageAdd" runat="server" />广告图添加</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbAppADImageEdit" runat="server" />广告图编辑</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbAppADImageDelete" runat="server" />广告图删除</label></li>
                                <li>&nbsp;</li>
                                <li style="width: 90px;">&nbsp;</li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbAppProductSet" runat="server" />商品配置</label></li>
                                <li style="display: none;">
                                    <label>
                                        <asp:CheckBox ID="cbAppHomeTopicSet" runat="server" />专题专题</label></li>
                            </ul>
                        </div>
                        <div class="PurviewItem">
                            <ul>
                                <li style="width: 90px; font-weight: 700;">版本管理：</li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbAndroidUP" runat="server" />Android升级</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbIOSUP" runat="server" />IOS升级</label></li>
                                <li>&nbsp;</li>
                            </ul>
                        </div>
                        <div class="PurviewItem">
                            <ul>
                                <li style="width: 90px; font-weight: 700;">支付设置：</li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbAppMobileAlipaySet" runat="server" />手机支付宝设置</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbAppShengPaySet" runat="server" />盛付通支付设置</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbAppOfflinePaySet" runat="server" />线下支付设置</label></li>
                                <li>&nbsp;</li>
                            </ul>
                        </div>
                    </div>
                </asp:Panel>

                <asp:Panel ID="AliohPanel" runat="server">
                    <div style="clear: both; margin-top: 40px; font-weight: 700; color: #000066;display:none;">
                        <label>
                            <asp:CheckBox ID="cbAliohManage" runat="server" />服务窗管理</label>
                    </div>
                    <div class="PurviewItemDivide"></div>
                    <div style="padding-left: 20px;">
                        <div class="PurviewItem">
                            <ul>
                                <li style="width: 90px; font-weight: 700;">服务窗配置：</li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbAliohBaseConfig" runat="server" />基本配置</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbAliohManageMenu" runat="server" />自定义菜单配置</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbAliohAddMenu" runat="server" />自定义菜单添加</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbAliohEditMenu" runat="server" />自定义菜单编辑</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbAliohDeleteMenu" runat="server" />自定义菜单删除</label></li>

                            </ul>
                        </div>

                        <div class="PurviewItem">
                            <ul>
                                <li style="width: 90px; font-weight: 700;">首页配置：</li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbAliohHomeTopicSet" runat="server" />专题配置</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbAliohManageThemes" runat="server" />模板选择</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbAliohThemesSet" runat="server" />模板配置</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbAliohIconSet" runat="server" />图标配置</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbAliohProductSet" runat="server" />商品配置</label></li>
                                <li style="width: 90px;">&nbsp;</li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbAliohBannerManage" runat="server" />轮播图管理</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbAliohBannerAdd" runat="server" />轮播图添加</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbAliohBannerEdit" runat="server" />轮播图编辑</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbAliohBannerDelete" runat="server" />轮播图删除</label></li>
                            </ul>
                        </div>
                        <div class="PurviewItem">
                            <ul>
                                <li style="width: 90px; font-weight: 700;">支付设置：</li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbAliohMobileAlipaySet" runat="server" />手机支付宝设置</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbAliohShengPaySet" runat="server" />盛付通支付设置</label></li>
                                <li>
                                    <label>
                                        <asp:CheckBox ID="cbAliohOfflinePaySet" runat="server" />线下支付设置</label></li>
                                <li>&nbsp;</li>
                            </ul>
                        </div>
                    </div>
                </asp:Panel>

                <div style="clear: both; margin-top: 40px; font-weight: 700; color: #000066">
                    <label>
                        <asp:CheckBox ID="cbManageUsers" runat="server" />会员管理</label>
                </div>
                <div class="PurviewItemDivide"></div>
                <div style="padding-left: 20px;">

                    <div class="PurviewItem">
                        <ul>
                            <li style="width: 90px; font-weight: 700;">会员：</li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbManageMembers" runat="server" />会员：</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbManageMembersView" runat="server" />查看</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbManageMembersEdit" runat="server" />编辑</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbManageMembersDelete" runat="server" />删除</label></li>
                        </ul>
                    </div>


                    <div class="PurviewItem">
                        <ul>
                            <li style="width: 90px; font-weight: 700;">会员等级：</li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbMemberRanks" runat="server" />会员等级：</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbMemberRanksView" runat="server" />查看</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbMemberRanksAdd" runat="server" />添加</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbMemberRanksEdit" runat="server" />编辑</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbMemberRanksDelete" runat="server" />删除</label></li>
                        </ul>
                    </div>

                    <div class="PurviewItem">
                        <ul>
                            <li style="width: 91px; font-weight: 700;">信任登录：</li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbOpenIdServices" runat="server" />信任登录列表</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbOpenIdSettings" runat="server" />信任登录配置</label></li>

                        </ul>
                    </div>
                </div>

                <div style="clear: both; margin-top: 40px; font-weight: 700; color: #000066">
                    <label>
                        <asp:CheckBox ID="cbCRMmanager" runat="server" />CRM管理</label>
                </div>
                <div class="PurviewItemDivide"></div>
                <div style="padding-left: 20px;">
                    <div class="PurviewItem">
                        <ul>
                            <li style="width: 100px; font-weight: 700;">会员深度营销：</li>
                            <li style="margin-left: -10px;">
                                <label>
                                    <asp:CheckBox ID="cbMemberMarket" runat="server" />会员深度营销：</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbClientGroup" runat="server" />客户分组</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbClientNew" runat="server" />新客户</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbClientActivy" runat="server" />活跃客户</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbClientSleep" runat="server" />休眠客户</label></li>
                        </ul>
                    </div>
                    <div class="PurviewItem">
                        <ul>
                            <li style="width: 90px; font-weight: 700;">商品评论：</li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbProductConsultationsManage" runat="server" />商品咨询管理</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbProductReviewsManage" runat="server" />商品评论管理</label></li>
                        </ul>
                    </div>
                    <div class="PurviewItem">
                        <ul>
                            <li style="width: 90px; font-weight: 700;">站内消息：</li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbReceivedMessages" runat="server" />收件箱</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbSendedMessages" runat="server" />发件箱</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbSendMessage" runat="server" />写新消息</label></li>
                        </ul>
                    </div>
                    <div class="PurviewItem">
                        <ul>
                            <li style="width: 90px; font-weight: 700;">客户留言：</li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbManageLeaveComments" runat="server" />客户留言管理</label></li>
                        </ul>
                    </div>
                </div>

                <div style="clear: both; margin-top: 40px; font-weight: 700; color: #000066">
                    <label>
                        <asp:CheckBox ID="cbMarketing" runat="server" />营销推广</label>
                </div>
                <div class="PurviewItemDivide"></div>
                <div style="padding-left: 20px;">
                    <div class="PurviewItem">
                        <ul>
                            <li style="width: 90px; font-weight: 700;">积分商城：</li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbGifts" runat="server" />礼品</label></li>
                        </ul>
                    </div>

                    <div class="PurviewItem">
                        <ul>
                            <li style="width: 100px; font-weight: 700;">商城促销活动：</li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbProductPromotion" runat="server" />商品促销</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbOrderPromotion" runat="server" />订单促销</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbBundPromotion" runat="server" />捆绑促销</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbGroupBuy" runat="server" />团购</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbCountDown" runat="server" />限时抢购</label></li>

                        </ul>
                    </div>
                    <div class="PurviewItem">
                        <ul>
                            <li style="width: 90px; font-weight: 700;">优惠券：</li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbCoupons" runat="server" />优惠券</label></li>
                        </ul>
                    </div>
                </div>

                <div style="clear: both; margin-top: 40px; font-weight: 700; color: #000066">
                    <label>
                        <asp:CheckBox ID="cbFinancial" runat="server" />财务管理</label>
                </div>
                <div class="PurviewItemDivide"></div>
                <div style="padding-left: 20px;">
                    <div class="PurviewItem">
                        <ul>
                            <li style="width: 90px; font-weight: 700;">会员预付款：</li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbAccountSummary" runat="server" />账户查询</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbReCharge" runat="server" />账户加款</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbBalanceDrawRequest" runat="server" />提现申请明细</label></li>
                        </ul>
                    </div>
                    <div class="PurviewItem">
                        <ul>
                            <li style="width: 91px; font-weight: 700;">预付款报表：</li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbBalanceDetailsStatistics" runat="server" />预付款报表</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbBalanceDrawRequestStatistics" runat="server" />提现报表</label></li>

                        </ul>
                    </div>

                    <div class="PurviewItem">
                        <ul>
                            <li style="width: 91px; font-weight: 700;">对账报表：</li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbReconciOrders" runat="server" />对账订单报表：</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbReconciOrdersView" runat="server" />查看</label></li>
                             <li>
                                <label>
                                    <asp:CheckBox ID="cbReconciOrdersCreateReport" runat="server" />生成报告</label></li>

                        </ul>
                   </div>
                   <div class="PurviewItem">
                        <ul>
                            <li style="width: 91px; font-weight: 700;">对账订单明细：</li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbReconciOrdersDetails" runat="server" />对账订单明细报表：</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbReconciOrdersDetailsView" runat="server" />查看</label></li>
                             <li>
                                <label>
                                    <asp:CheckBox ID="cbReconciOrdersDetailsCreateReport" runat="server" />生成报告</label></li>

                        </ul>
                    </div>

                    <div class="PurviewItem">
                        <ul>
                            <li style="width: 91px; font-weight: 700;"></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbAggregatePayable" runat="server" />销售额及应付总汇</label></li>
                        </ul>
                    </div>



                </div>


                <div style="clear: both; margin-top: 40px; font-weight: 700; color: #000066">
                    <label>
                        <asp:CheckBox ID="cbTotalReport" runat="server" />统计报表</label>
                </div>
                <div class="PurviewItemDivide"></div>
                <div style="padding-left: 20px;">
                    <div class="PurviewItem">
                        <ul>
                            <li style="width: 90px; font-weight: 700;">零售业务统计：</li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbSaleTotalStatistics" runat="server" />生意报告</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbUserOrderStatistics" runat="server" />订单统计</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbSaleList" runat="server" />销售明细</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbSaleTargetAnalyse" runat="server" />销售指标分析</label></li>
                        </ul>
                        <ol>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbProductSaleRanking" runat="server" />商品销售排行</label></li>
                            <li style="width: 130px;">
                                <label>
                                    <asp:CheckBox ID="cbProductSaleStatistics" runat="server" />商品访问与购买次数
                                </label>
                            </li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbMemberRanking" runat="server" />会员消费排行</label></li>
                            <li style="width: 110px;">
                                <label>
                                    <asp:CheckBox ID="cbMemberArealDistributionStatistics" runat="server" />会员分布统计</label></li>
                            <li style="width: 110px;">
                                <label>
                                    <asp:CheckBox ID="cbUserIncreaseStatistics" runat="server" />会员增长统计</label></li>
                        </ol>
                    </div>

                    <div class="PurviewItem">
                        <ul>
                            <li style="width: 90px; font-weight: 700;"></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbCustomService" runat="server" />客服数据统计：</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbCustomServiceView" runat="server" />查看</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbCustomServiceCreateReport" runat="server" />生成报告</label></li>
                        </ul>
                      </div>


                     <div class="PurviewItem">
                        <ul>
                            <li style="width: 90px; font-weight: 700;"></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbSuppliersSales" runat="server" />供应商销售统计：</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbSuppliersSalesView" runat="server" />查看</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbSuppliersSalesCreateReport" runat="server" />生成报告</label></li>
                        </ul>
                      </div>

                    <div class="PurviewItem">
                        <ul>
                            <li style="width: 90px; font-weight: 700;"></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbProductSaleAsBrand" runat="server" />按品牌汇总商品销售：</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbProductSaleAsBrandView" runat="server" />查看</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbProductSaleAsBrandExcel" runat="server" />生成报告</label></li>
                        </ul>
                    </div>

                    <div class="PurviewItem">
                        <ul>
                            <li style="width: 90px; font-weight: 700;"></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbProductSaleAsImportSource" runat="server" />按产地汇总商品销售：</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbProductSaleAsImportSourceView" runat="server" />查看</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbProductSaleAsImportSourceExcel" runat="server" />生成报告</label></li>
                        </ul>
                     </div>

                </div>


                <div style="clear: both; margin-top: 40px; font-weight: 700; color: #000066">
                    <label>
                        <asp:CheckBox ID="cbSitesManager" runat="server" />站点管理：</label>
                </div>
                <div class="PurviewItemDivide"></div>
                <div style="padding-left: 20px;">

                    <div class="PurviewItem">
                        <ul>
                            <li style="width: 90px; font-weight: 700;">站点管理：</li>
                            <li>
                                <label>
                                   <asp:CheckBox ID="cbSites" runat="server" />站点管理：</label></li>
                            <li>
                                <label>
                                   <asp:CheckBox ID="cbSitesView" runat="server" />查看</label></li>
                            <li>
                                <label>
                                   <asp:CheckBox ID="cbSitesAdd" runat="server" />添加</label></li>
                            <li>
                                <label>
                                   <asp:CheckBox ID="cbSitesEdit" runat="server" />编辑</label></li>
                            <li>
                                <label>
                                   <asp:CheckBox ID="cbSitesDelete" runat="server" />删除</label></li>
                        </ul>
                    </div>
                </div>


                <div style="clear: both; margin-top: 40px; font-weight: 700; color: #000066">
                    <label>
                        <asp:CheckBox ID="cbStoreManager" runat="server" />门店管理：</label>
                </div>
                <div class="PurviewItemDivide"></div>
                <div style="padding-left: 20px;">

                    <div class="PurviewItem">
                        <ul>
                            <li style="width: 90px; font-weight: 700;">门店管理：</li>
                            <li>
                                <label>
                                   <asp:CheckBox ID="cbStore" runat="server" />门店管理：</label></li>
                            <li>
                                <label>
                                   <asp:CheckBox ID="cbStoreView" runat="server" />查看</label></li>
                            <li>
                                <label>
                                   <asp:CheckBox ID="cbStoreAdd" runat="server" />添加</label></li>
                            <li>
                                <label>
                                   <asp:CheckBox ID="cbStoreEdit" runat="server" />编辑</label></li>
                            <li>
                                <label>
                                   <asp:CheckBox ID="cbStoreDelete" runat="server" />删除</label></li>
                            <li style="width: 90px">&nbsp;</li>
                             <li>
                                <label>
                                   <asp:CheckBox ID="cbStoreRelatedProduct" runat="server" />关联商品</label></li>
                            <li>
                                <label>
                                   <asp:CheckBox ID="cbStoreAddManager" runat="server" />添加管理员</label>
                            </li>
                        </ul>
                    </div>

                     <div class="PurviewItem">
                        <ul>
                            <li style="width: 90px; font-weight: 700;">门店订单管理：</li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbStoreOrder" runat="server"/>门店订单管理：</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbStoreOrderView" runat="server"/>查看</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbStoreOrderAdd" runat="server"/>添加</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbSkusSelect" runat="server" />商品规格选择</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbShipAddressSelect" runat="server" />收获地址选择</label></li>
                            <li style="width: 90px">&nbsp;</li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbStoreOrderDelete" runat="server"/>删除</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbStoreOrderClose" runat="server"/>关闭订单</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbStoreOrderSend" runat="server"/>发货</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbStoreOrderReplaceAccept" runat="server"/>确认换货</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbStoreOrderReplaceRefuse" runat="server"/>拒绝换货</label></li>
                            <li style="width: 90px">&nbsp;</li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbStoreOrderReturnAccept" runat="server"/>确认退货</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbStoreOrderReturnRefuse" runat="server"/>拒绝退货</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbStoreOrderRefundAccept" runat="server"/>确认退款</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbStoreOrderRefundRefuse" runat="server"/>拒绝退款</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbStoreOrderPrintPosts" runat="server"/>批量打印快递单</label></li>
                            <li style="width: 90px">&nbsp;</li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbStoreOrderPrintGoods" runat="server"/>批量打印发货单</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbStoreOrderBatchSend" runat="server"/>批量发货</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbStoreOrderUploadGoods" runat="server"/>批量上传发货单</label></li>
                            </ul>
                      </div>
                      
                      <div class="PurviewItem">
                           <ul>
                            <li style="width: 90px; font-weight: 700;"></li>
                               <li>
                                <label>
                                    <asp:CheckBox ID="cbStoreOrderDown" runat="server"/>下载配货单：</label></li>
                              <li>
                                <label>
                                    <asp:CheckBox ID="cbStoreOrderDownSelExcel" runat="server"/>选中订单导出</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbStoreOrderDownTimeExcel" runat="server"/>按时间段导出</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbStoreOrderDownGoodsPick" runat="server"/>商品配货表</label></li>
                        </ul>
                       </div>

                      <div class="PurviewItem">
                           <ul>
                            <li style="width: 90px; font-weight: 700;"></li>
                               <li>
                                <label>
                                    <asp:CheckBox ID="cbStoreOrderClear" runat="server"/>清关订单导出：</label></li>
                               <li>
                                <label>
                                    <asp:CheckBox ID="cbStoreOrderClearSelExcel" runat="server"/>选中订单导出</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbStoreOrderClearTimeExcel" runat="server"/>按时间段导出</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbStoreOrderClearGoodsPick" runat="server"/>商品配货表</label></li>
                        </ul>
                       </div>

                </div>
                
                <div style="clear: both; margin-top: 40px; font-weight: 700; color: #000066">
                    <label>
                        <asp:CheckBox ID="cbSupplierManage" runat="server" />供应商管理：</label>
                </div>
                <div class="PurviewItemDivide"></div>
                <div style="padding-left: 20px;">

                    <div class="PurviewItem">
                        <ul>
                            <li style="width: 90px; font-weight: 700;">订单管理：</li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbSupplierOrder" runat="server"/>订单管理：</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbSupplierOrderView" runat="server"/>查看</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbSupplierOrderAdd" runat="server"/>添加</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbSupplierOrderDelete" runat="server"/>删除</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbSupplierOrderClose" runat="server"/>关闭订单</label></li>
                            <li style="width: 90px">&nbsp;</li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbSupplierOrderSend" runat="server"/>发货</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbSupplierOrderReplaceAccept" runat="server"/>确认换货</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbSupplierOrderReplaceRefuse" runat="server"/>拒绝换货</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbSupplierOrderReturnAccept" runat="server"/>确认退货</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbSupplierOrderReturnRefuse" runat="server"/>拒绝退货</label></li>
                            <li style="width: 90px">&nbsp;</li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbSupplierOrderRefundAccept" runat="server"/>确认退款</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbSupplierOrderRefundRefuse" runat="server"/>拒绝退款</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbSupplierOrderPrintPosts" runat="server"/>批量打印快递单</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbSupplierOrderPrintGoods" runat="server"/>批量打印发货单</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbSupplierOrderBatchSend" runat="server"/>批量发货</label></li>
                            <li style="width: 90px">&nbsp;</li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbSupplierOrderUploadGoods" runat="server"/>批量上传发货单</label></li>
                           <li>
                                <label>
                                    <asp:CheckBox ID="cbSupplierOrderRemark" runat="server" />管理员备注</label></li>
                            </ul>
                      </div>
                      
                      <div class="PurviewItem">
                           <ul>
                            <li style="width: 90px; font-weight: 700;"></li>
                               <li>
                                <label>
                                    <asp:CheckBox ID="cbSupplierOrderDown" runat="server"/>下载配货单：</label></li>
                              <li>
                                <label>
                                    <asp:CheckBox ID="cbSupplierOrderDownSelExcel" runat="server"/>选中订单导出</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbSupplierOrderDownTimeExcel" runat="server"/>按时间段导出</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbSupplierOrderDownGoodsPick" runat="server"/>商品配货表</label></li>
                        </ul>
                       </div>

                      <div class="PurviewItem">
                           <ul>
                            <li style="width: 90px; font-weight: 700;"></li>
                               <li>
                                <label>
                                    <asp:CheckBox ID="cbSupplierOrderClear" runat="server"/>清关订单导出：</label></li>
                               <li>
                                <label>
                                    <asp:CheckBox ID="cbSupplierOrderClearSelExcel" runat="server"/>选中订单导出</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbSupplierOrderClearTimeExcel" runat="server"/>按时间段导出</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbSupplierOrderClearGoodsPick" runat="server"/>商品配货表</label></li>
                        </ul>
                       </div>

            

                    <div class="PurviewItem">
                        <ul>
                            <li style="width: 90px; font-weight: 700;">商品管理：</li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbSupplierProductManage" runat="server"/>商品管理：</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbSupplierProductView" runat="server"/>查看</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbSupplierProductAdd" runat="server"/>添加</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbSupplierProductEdit" runat="server"/>编辑</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbSupplierProductDelete" runat="server"/>删除</label></li>
                            <li style="width: 90px">&nbsp;</li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbSupplierProductInStock" runat="server"/>入库</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbSupplierProductUp" runat="server"/>上架</label></li>
                            <li>
                                <label>
                                    <asp:CheckBox ID="cbSupplierProductDown" runat="server"/>下架</label></li>
                            <li style="width: 90px">&nbsp;</li>
                            <li>&nbsp;</li>
                        </ul>
                    </div>


                </div> 

                 

                <div class="PurviewItemDivide"></div>

                <div style="margin-top: 10px; margin-bottom: 10px;">
                    <asp:Button ID="btnSet1" runat="server" Text="保 存" class="submit_queding"></asp:Button>
                    <input type="button" value="返 回" class="submit_queding" onclick="link()" />
                </div>

            </div>
        </div>-->
    </div>


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <Hi:Script runat="server" Src="/admin/js/PrivilegeInRoles.js" />

    <script type="text/javascript" language="javascript">
        function link() {
            window.location.href = 'Roles.aspx';
        }
    </script>
</asp:Content>
