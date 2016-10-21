<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Admin/Admin.Master"
    CodeBehind="EditProduct.aspx.cs" Inherits="EcShop.UI.Web.Admin.EditProduct" %>

<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <link id="cssLink" rel="stylesheet" href="../css/style.css?v=20150819" type="text/css" media="screen" />
    <Hi:Script ID="Script2" runat="server" Src="/utility/jquery_hashtable.js" />
    <Hi:Script ID="Script1" runat="server" Src="/utility/jquery-powerFloat-min.js" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <asp:HiddenField runat="server" ID="ArrhidProductRegistrationNumber" />
        <asp:HiddenField runat="server" ID="hidProductRegistrationNumber" />
        <asp:HiddenField runat="server" ID="hidLJNo" />
        <asp:HiddenField ID="hid_Checked" runat="server" />
        <div class="title">
            <em>
                <img src="../images/01.gif" width="32" height="32" /></em>
            <h1>编辑商品信息</h1>
            <span>商品信息修改</span>
        </div>
        <div class="datafrom">
            <div class="formitem validator1">
                <ul>
                    <li>
                        <h2 class="colorE">基本信息</h2>
                    </li>
                    <li><span class="formitemtitle Pw_198">所属商品分类：</span> <span class="colorE float fonts">
                        <asp:Literal runat="server" ID="litCategoryName"></asp:Literal></span> [<asp:HyperLink
                            runat="server" ID="lnkEditCategory" CssClass="a" Text="编辑"></asp:HyperLink>]
                    </li>
                    <li><span class="formitemtitle Pw_198">商品类型：</span>
                        <abbr class="formselect">
                            <Hi:ProductTypeDownList runat="server" CssClass="productType" ID="dropProductTypes" NullToDisplay="--请选择--" /></abbr>
                        品牌：<abbr class="formselect"><Hi:BrandCategoriesDropDownList runat="server" ID="dropBrandCategories" NullToDisplay="--请选择--" /></abbr>
                    </li>
                    <li class=" clearfix"><span class="formitemtitle Pw_198">商品名称：<em>*</em></span>
                        <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtProductName" Width="350px" />
                        <p id="ctl00_contentHolder_txtProductNameTip">
                            限定在60个字符
                        </p>
                    </li>
                    <li class=" clearfix"><span class="formitemtitle Pw_198">商品实际名称：<em>*</em></span>
                        <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtsysProductName" Width="350px" />
                        <p id="ctl00_contentHolder_txtsysProductNameTip">
                            限定在60个字符
                        </p>
                    </li>
                     <li class=" clearfix"><span class="formitemtitle Pw_198">英文名称：</span>
                        <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtEnglishName" Width="350px" />
                        <p id="ctl00_contentHolder_txtEnglishName">
                            限定在60个字符
                        </p>
                    </li>
                    <li class=" clearfix"><span class="formitemtitle Pw_198">商品副标题：</span>
                        <Hi:TrimTextBox runat="server" Rows="6" Height="100px" Columns="76" TextMode="MultiLine"  ID="txtProductTitle" />
                        <p id="ctl00_contentHolder_txtProductTitleTip">
                            限定在500个字符
                        </p>
                    </li>
                    <li id="saleType"><span class="formitemtitle Pw_198">销售类型：<em>*</em></span>
                        <abbr class="formselect" id="selectCombination">
                            <asp:DropDownList ID="dropSaleType" runat="server">
                                <asp:ListItem Value="1" Selected="True">正常</asp:ListItem>
                                <asp:ListItem Value="2">组合</asp:ListItem>
                            </asp:DropDownList>
                        </abbr>
                    </li>
                    <li><span class="formitemtitle Pw_198">提升权重值：<em>*</em></span>
                        <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtAdminFraction" />
                        <p id="ctl00_contentHolder_txtAdminFractionTip">
                            在原权重值上增加的权重值
                        </p>
                    </li>
                    <li><span class="formitemtitle Pw_198">权重值：<em>*</em></span>
                        <Hi:TrimTextBox runat="server" Enabled="false" CssClass="forminput" ID="txtFraction" />
                        <p id="ctl00_contentHolder_txtFractionTip">
                            商品默认显示顺序，越大排在越前
                        </p>
                    </li>
                    <li><span class="formitemtitle Pw_198">排序：<em>*</em></span>
                        <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtDisplaySequence" />
                        <p id="ctl00_contentHolder_txtDisplaySequenceTip">
                            商品显示顺序，越大排在越前
                        </p>
                    </li>
                    <li id="supplierRow"><span class="formitemtitle Pw_198">供货商：<em>*</em></span>
                        <abbr class="formselect">
                            <Hi:SupplierDropDownList runat="server" ID="ddlSupplier" OnClientChange="changeSupplier(this)" />
                        </abbr>
                    </li>
                    <li><span class="formitemtitle Pw_198">商品编码：</span>
                        <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtProductCode" />
                        <p id="ctl00_contentHolder_txtProductCodeTip">
                            长度不能超过30个字符
                        </p>
                    </li>
                    <li><span class="formitemtitle Pw_198">计量单位：<em>*</em></span>
                        <abbr class="formselect">
                            <Hi:UnitDropDownList runat="server" ID="ddlUnit" />
                        </abbr>
                    </li>
                    <li><span class="formitemtitle Pw_198">换算关系：<em>*</em></span>
                        <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtConversionRelation" Text="1" MaxLength="10" />
                        <p id="ctl00_contentHolder_txtConversionRelationTip">
                            换算关系为大于1整数
                        </p>
                    </li>
                    <li><span class="formitemtitle Pw_198">市场价：</span>
                        <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtMarketPrice" />&nbsp;元
                        <p id="ctl00_contentHolder_txtMarketPriceTip">
                            本站会员所看到的商品市场价
                        </p>
                    </li>

                    <li><span class="formitemtitle Pw_198">生产厂家：<em>*</em></span>
                        <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtManufacturer" />
                        <p id="ctl00_contentHolder_txtManufacturerTip">
                            长度不能超过100个字符
                        </p>
                    </li>
                    <li><span class="formitemtitle Pw_198">型号：<em>*</em></span>
                        <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtItemNo" />
                        <p id="ctl00_contentHolder_txtItemNoTip">
                            长度不能超过500个字符 
                        </p>
                    </li>
                    <li><span class="formitemtitle Pw_198">条形码：<em>*</em></span>
                        <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtBarCode" />
                        <p id="ctl00_contentHolder_txtBarCodeTip">
                            长度不能超过50个字符
                        </p>
                    </li>
                    <li><span class="formitemtitle Pw_198">成分：<em>*</em></span>
                        <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtIngredient" />
                        <p id="ctl00_contentHolder_txtIngredientTip">
                            长度不能超过500个字符
                        </p>
                    </li>

                    <li>
                        <h2 class="colorE">扩展属性</h2>
                    </li>
                    <li id="attributeRow" style="display: none;"><span class="formitemtitle Pw_198">商品属性：</span>
                        <div class="attributeContent" id="attributeContent">
                        </div>
                        <Hi:TrimTextBox runat="server" ID="txtAttributes" TextMode="MultiLine" Style="display: none;"></Hi:TrimTextBox>
                    </li>
                    <li id="skuCodeRow"><span class="formitemtitle Pw_198">货号：</span>
                        <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtSku" />
                        <p id="ctl00_contentHolder_txtSkuTip">
                            限定在20个字符，不能输入中文
                        </p>
                    </li>
                    <li id="salePriceRow"><span class="formitemtitle Pw_198">一口价：<em>*</em></span>
                        <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtSalePrice" />&nbsp;元&nbsp;<input type="button"
                            onclick="editProductMemberPrice();" value="编辑会员价" />
                        <Hi:TrimTextBox runat="server" ID="txtMemberPrices" TextMode="MultiLine" Style="display: none;"></Hi:TrimTextBox>
                        <p id="ctl00_contentHolder_txtSalePriceTip">
                            本站会员所看到的商品零售价
                        </p>
                    </li>
                    <li id="costPriceRow"><span class="formitemtitle Pw_198">成本价：<em>*</em></span>
                        <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtCostPrice" />&nbsp;元
                        <p id="ctl00_contentHolder_txtCostPriceTip">
                            商品的成本价
                        </p>
                    </li>
                    <li id="deductFeeRow"><span class="formitemtitle Pw_198">扣点：</span><Hi:TrimTextBox
                        runat="server" CssClass="forminput" ID="txtDeductFee"  Text="0" />
                        <p id="ctl00_contentHolder_txtDeductFeeTip">
                            商品的扣点
                        </p>
                    </li>
                    <li id="qtyRow"><span class="formitemtitle Pw_198">销售库存：<em>*</em></span><Hi:TrimTextBox
                        runat="server" CssClass="forminput" Text="0" ID="txtStock" Enabled="false" />
                        <p id="ctl00_contentHolder_txtStockTip">
                            商品的销售库存<em>当前库存从WMS获取</em>
                        </p>
                    </li>
                    <li id="factQtyRow"><span class="formitemtitle Pw_198">商品库存：<em>*</em></span><Hi:TrimTextBox
                        runat="server" CssClass="forminput" Text="0" ID="txtFactStock" Enabled="false"/>
                        <p id="ctl00_contentHolder_txtFactStockTip">
                            商品库存
                        </p>
                    </li>
                    <li><span class="formitemtitle Pw_198">购买基数：</span><Hi:TrimTextBox
                        runat="server" CssClass="forminput" ID="txtBuyCardinality" Text="1" />
                        <p id="ctl00_contentHolder_txtBuyCardinalityTip">
                            默认值为1，表示任意数量购买；如果设置为2，即为2件起卖，购买数量必须是2的倍数，以此类推。
                        </p>
                    </li>
                    <li id="weightRow"><span class="formitemtitle Pw_198">商品单位净重：</span><Hi:TrimTextBox
                        runat="server" CssClass="forminput" ID="txtWeight" />&nbsp;克</li>

                    <li id="grossweightRow"><span class="formitemtitle Pw_198">商品单位毛重：</span><Hi:TrimTextBox
                        runat="server" CssClass="forminput" ID="txtGrossWeight" />&nbsp;克</li>

                    <li><span class="formitemtitle Pw_198">是否需要清关： </span>
                        <asp:CheckBox ID="ChkisCustomsClearance" runat="server" />
                        &nbsp;</li>
                    <li id="productTaxRateRow"><span class="formitemtitle Pw_198">税率：</span>
                        <abbr class="formselect">
                            <Hi:TaxRateDropDownList runat="server" ID="dropTaxRate" Enabled="false" NullToDisplay="--请选择--" /></abbr>
                        <p>
                            该项由关务归类选择，此处无需操作。
                        </p>
                    </li>
                    <li id="shippingRow"><span class="formitemtitle Pw_198">运费模版：<em>*</em></span>
                        <abbr class="formselect">
                            <Hi:ShippingTemplatesDropDownList runat="server" ID="ddlShipping" />
                        </abbr>
                    </li>
                    <li id="importSourceTypeRow"><span class="formitemtitle Pw_198">原产地：<em>*</em></span>
                        <abbr class="formselect">
                            <Hi:ImportSourceTypeDropDownList runat="server" ID="ddlImportSourceType" NullToDisplay="--请选择原产地--" />
                        </abbr>
                    </li>

                    <li id="ProductStandard"><span class="formitemtitle Pw_198">商品规格：</span>
                        <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtProductStandard" />
                        <p id="ctl00_contentHolder_txtProductStandardTip">
                            未开启多规格时填写
                        </p>
                    </li>
                    <div id="combinamtionShow" style="display: none;">
                        <li id="combinationProducts"><span class="formitemtitle Pw_198">组合商品：</span>
                            <em>*</em><span style="cursor: pointer; color: blue; font-size: 14px" onclick="ShowAddDiv();">请点此选择</span>
                        </li>
                        <li class="binditems">
                            <table width="100%" id="addlist">
                                <tr class="table_title">
                                    <th class="td_right td_left" scope="col">商品名</th>
                                    <th class="td_right td_left" scope="col">sku信息</th>
                                    <th class="td_right td_left" scope="col">价格</th>
                                    <th class="td_right td_left" scope="col">数量</th>
                                    <th class="td_right td_left" scope="col">毛重</th>
                                    <th class="td_right td_left" scope="col">净重</th>
                                    <th class="td_left td_right_fff" scope="col">操作</th>
                                </tr>
                                <asp:Repeater ID="rpCombination" runat="server">
                                    <ItemTemplate>
                                        <tr name='appendlist'>
                                            <td><%#Eval("ProductName")%></td>
                                            <td>
                                                <%#Eval("SKUContent")%>
                                            </td>
                                            <td><input type='text' value='<%# Eval("Price","{0:f}") %>' name='curValue' onblur="checkAllprice()"/></td>
                                            <td>
                                                <input type='text' value='<%#Eval("Quantity") %>' name='txtNum' onblur="checkAllprice()" onchange="checkFactNum(this);"/>
                                                <input type='hidden' value='1' name='hidNumWeight' weight='<%#Eval("Weight") %>' grossweight='<%#Eval("GrossWeight") %>'/>
                                            </td>
                                            <td style='display: none'><%#Eval("SkuId") %>|<%#Eval("ProductId") %>|<%#Eval("ProductName") %>|<%#Eval("ThumbnailsUrl") %>|<%#Eval("Weight") %>|<%#Eval("SKU") %>|<%#Eval("SKUContent") %>|<%#Eval("GrossWeight") %></td>
                                            <td><%#Eval("GrossWeight","{0:F2}") %></td>
                                            <td><%#Eval("Weight","{0:F2}")%></td>
                                            <td><span id='<%#Eval("SkuId") %>' class='ck_SkuId' style='cursor: pointer; color: blue' onclick='Remove(this)'>删除</span></td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </table>
                            <input id="selectProductsinfo" name="selectProductsinfo" type="hidden" />
                        </li>
                    </div>

                    <li><span class="formitemtitle Pw_198">是否促销： </span>
                        <asp:CheckBox ID="ChkisPromotion" runat="server" />
                        &nbsp;</li>

                    <li><span class="formitemtitle Pw_198">是否显示折扣： </span>
                        <asp:CheckBox ID="ChkisDisplayDiscount" runat="server" />
                        &nbsp;</li>


                    <li id="skuTitle" style="display: none;">
                        <h2 class="colorE">商品规格(关闭时商品备案将清除，请谨慎操作)</h2>
                    </li>
                    <li id="enableSkuRow" style="display: none;"><span class="formitemtitle Pw_198">规格：</span><input
                        id="btnEnableSku" type="button" value="开启规格" />
                        开启规格前先填写以上信息，可自动复制信息到每个规格</li>
                    <li id="skuRow" style="display: none;">
                        <p id="skuContent">
                            <input type="button" id="btnshowSkuValue" value="生成部分规格" />
                            <input type="button" id="btnAddItem" value="增加一个规格" />
                            <input type="button" id="btnCloseSku" value="关闭规格" />
                            <input type="button" id="btnGenerateAll" value="生成所有规格" />
                        </p>
                        <p id="skuFieldHolder" style="margin: 0px auto; display: none;">
                        </p>
                        <div id="skuTableHolder">
                        </div>
                        <Hi:TrimTextBox runat="server" ID="txtSkus" TextMode="MultiLine" Style="display: none"></Hi:TrimTextBox>
                        <asp:CheckBox runat="server" ID="chkSkuEnabled" Style="display: none;" />
                    </li>
                    <li>
                        <h2 class="colorE">图片和描述</h2>
                    </li>
                    <li><span class="formitemtitle Pw_198">商品图片：</span>
                        <div class="uploadimages">
                            <Hi:ImageUploader runat="server" ID="uploader1" />
                        </div>
                        <div class="uploadimages">
                            <Hi:ImageUploader runat="server" ID="uploader2" />
                        </div>
                        <div class="uploadimages">
                            <Hi:ImageUploader runat="server" ID="uploader3" />
                        </div>
                        <div class="uploadimages">
                            <Hi:ImageUploader runat="server" ID="uploader4" />
                        </div>
                        <div class="uploadimages">
                            <Hi:ImageUploader runat="server" ID="uploader5" />
                        </div>
                        <p class="Pa_198 clearfix" m_none>
                            图片应小于120k，jpg,gif,jpeg,png或bmp格式。建议为500x500像素
                        </p>
                    </li>
                    <li class="clearfix"><span class="formitemtitle Pw_198">商品简介：</span>
                        <Hi:TrimTextBox runat="server" Rows="6" Height="100px" Columns="76" ID="txtShortDescription"
                            TextMode="MultiLine" />
                        <p class="Pa_198">
                            限定在300个字符以内
                        </p>
                    </li>
                    <li class="clearfix m_none"><span class="formitemtitle Pw_198">商品描述：</span> <span
                        class="tab">
                        <div class="status">
                            <ul>
                                <li style="clear: none;"><a onclick="ShowNotes(1)" class="off" id="tip1" style="cursor: pointer">PC端</a></li>
                                <li style="clear: none;"><a onclick="ShowNotes(2)" id="tip2" style="cursor: pointer">移动端</a></li>
                            </ul>
                        </div>
                    </span><span style="padding-left: 198px;">
                        <div id="notes1">
                            <Kindeditor:KindeditorControl ID="fckDescription" runat="server" Width="830" Height="300px" />
                        </div>
                        <div id="notes2" style="display: none;">
                            <Kindeditor:KindeditorControl ID="fckmobbileDescription" runat="server" Width="830"
                                ImportLib="false" Height="300px" />
                        </div>
                    </span>
                        <p style="color: Red;">
                            <asp:CheckBox runat="server" ID="ckbIsDownPic" Text="是否下载商品描述外站图片" />
                        </p>
                        <p class="Pa_198">
                            如果勾选此选项时，商品里面有外站的图片则会下载到本地，相反则不会，由于要下载图片，如果图片过多或图片很大，需要下载的时间就多，请慎重选择。
                        </p>
                    </li>
                    <li>
                        <h2 class="colorE clear">相关设置</h2>
                    </li>
                     <li>
                           <span class="formitemtitle Pw_198">是否限购：
                           </span>
                           <asp:RadioButtonList ID="Rd_Purchase" runat="server" RepeatDirection="Horizontal" >
                                 <asp:ListItem Value="0" Selected="True" onclick="PurchaseChange(0)">非限购</asp:ListItem>
                                 <asp:ListItem Value="1" onclick="PurchaseChange(1)">限购</asp:ListItem>
                           </asp:RadioButtonList>
                    </li>
                    <li id="li_Purchase"  runat="server" style="display:none">
                        <ul style="padding-left: 58px;">
                            <li><span class="formitemtitle Pw_140"><em >*</em>限购时间：</span>
                                    <asp:TextBox ID="txtSectionDay" runat="server" CssClass="forminput" Text="0"></asp:TextBox>(天)
                              </li>
                              <li><span class="formitemtitle Pw_140"><em >*</em>限购数量：</span>
                                    <asp:TextBox ID="txtMaxCount" runat="server" CssClass="forminput" Text="1"></asp:TextBox>
                                   <p id="P4">限购时间段每个人能购买的数量。</p>
                              </li>
                           </ul>
                    </li>
                    <li><span class="formitemtitle Pw_198">直接推广佣金：</span>
                        <asp:TextBox runat="server" CssClass="forminput" ID="txtReferralDeduct" />&nbsp;%
                        <asp:Literal ID="litReferralDeduct" runat="server" />
                        <p id="txtReferralDeductTip" runat="server">推广员分享链接产生有效订单时享受的佣金比例，不填写则用全站统一比例</p>
                    </li>
                    <li><span class="formitemtitle Pw_198">下级会员佣金：</span>
                        <asp:TextBox ID="txtSubMemberDeduct" CssClass="forminput" runat="server" />&nbsp;%
                        <asp:Literal ID="litSubMemberDeduct" runat="server" />
                        <p id="txtSubMemberDeductTip" runat="server">下级会员未通过分享链接进入商城时产生的订单，推广员可享受的佣金比例，不填写则用全站统一比例</p>
                    </li>
                    <li><span class="formitemtitle Pw_198">下级推广员佣金：</span>
                        <asp:TextBox ID="txtSubReferralDeduct" CssClass="forminput" runat="server" />&nbsp;%
                        <asp:Literal ID="litSubReferralDeduct" runat="server" />
                        <p id="txtSubReferralDeductTip" runat="server">推广员分享链接产生有效订单时，其上级推广员也可获得相应的佣金比例，不填写则用全站统一比例</p>
                    </li>
                    <li><span class="formitemtitle Pw_198">商品销售状态：</span>
                        <asp:RadioButton runat="server" ID="radOnSales" GroupName="SaleStatus" Text="出售中"></asp:RadioButton>
                        <asp:RadioButton runat="server" ID="radUnSales" GroupName="SaleStatus" Text="下架区"></asp:RadioButton>
                        <asp:RadioButton runat="server" ID="radInStock" GroupName="SaleStatus" Text="仓库中"></asp:RadioButton>
                    </li>
                    <li style="display: none"><span class="formitemtitle Pw_198">商品包邮： </span>
                        <asp:CheckBox ID="ChkisfreeShipping" runat="server" />
                        &nbsp;</li>
                    <li class="clearfix" id="l_tags" runat="server"><span class="formitemtitle Pw_198">商品标签定义：<a
                        id="a_addtag" href="javascript:void(0)" onclick="javascript:AddTags()" class="add">添加</a></span>
                        <div id="div_tags">
                            <Hi:ProductTagsLiteral ID="litralProductTag" runat="server"></Hi:ProductTagsLiteral>
                        </div>
                        <div id="div_addtag" style="display: none">
                            <input type="text" id="txtaddtag" /><input type="button" value="保存" onclick="return AddAjaxTags()" />
                        </div>
                        <Hi:TrimTextBox runat="server" ID="txtProductTag" TextMode="MultiLine" Style="display: none;"></Hi:TrimTextBox>
                    </li>
                    <li class="clearfix">
                        <h2 class="colorE">搜索优化</h2>
                    </li>
                    <li><span class="formitemtitle Pw_198">详细页标题：</span>
                        <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtTitle" Width="350px" />
                        <p id="ctl00_contentHolder_txtTitleTip">
                            详细页标题限制在100字符以内
                        </p>
                    </li>
                    <li><span class="formitemtitle Pw_198">详细页描述：</span>
                        <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtMetaDescription" Width="350px" />
                        <p id="ctl00_contentHolder_txtMetaDescriptionTip">
                            详细页描述限制在260字符以内
                        </p>
                    </li>
                    <li><span class="formitemtitle Pw_198">详细页搜索关键字：</span>
                        <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtMetaKeywords" Width="350px" />
                        <p id="ctl00_contentHolder_txtMetaKeywordsTip">
                            详细页搜索关键字限制在160字符以内
                        </p>
                    </li>
                </ul>
                <ul class="btntf Pa_198 clear">
                    <asp:HiddenField ID="hiddenSkus" runat="server"></asp:HiddenField>
                    <asp:Button runat="server" ID="btnSave" Text="保 存" OnClientClick="return doSubmit()&&CollectInfos();"
                        CssClass="submit_DAqueding inbnt" />
                </ul>
            </div>
        </div>
    </div>
    <div class="Pop_up" id="priceBox" style="display: none;">
        <h1>
            <span id="popTitle"></span>
        </h1>
        <div class="img_datala">
            <img src="../images/icon_dalata.gif" alt="关闭" width="38" height="20" />
        </div>
        <div class="mianform ">
            <div id="priceContent">
            </div>
            <div style="margin-top: 10px; text-align: center;">
                <input type="button" value="确定" onclick="doneEditPrice();" />
            </div>
        </div>
    </div>
    <div class="Pop_up" id="skuValueBox" style="display: none;">
        <h1>
            <span>选择要生成的规格</span>
        </h1>
        <div class="img_datala">
            <img src="../images/icon_dalata.gif" alt="关闭" width="38" height="20" />
        </div>
        <div class="mianform ">
            <div id="skuItems">
            </div>
            <div style="margin-top: 10px; text-align: center;">
                <input type="button" value="确定" id="btnGenerate" />
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript" language="javascript">
        $(function () {
            if ($("#ctl00_contentHolder_hid_Checked").val() == "1")
            {
                $("#ctl00_contentHolder_li_Purchase").show();
            }
        })
        function PurchaseChange(obj)
        {
            if (obj == "0") {
                $("#ctl00_contentHolder_li_Purchase").hide();
            }
            else {
                $("#ctl00_contentHolder_li_Purchase").show();
            }
        }
        function ShowNotes(index) {

            document.getElementById("notes1").style.display = "none";
            document.getElementById("notes2").style.display = "none";
            var notesId = "notes" + index;
            document.getElementById(notesId).style.display = "block";

            document.getElementById("tip1").className = "";
            document.getElementById("tip2").className = "";
            var tipId = "tip" + index;
            document.getElementById(tipId).className = "off"
        }
        function InitValidators() {

            initValid(new InputValidator('ctl00_contentHolder_txtProductName', 1, 60, false, null, '商品名称字符长度在1-60之间'));
            initValid(new InputValidator('ctl00_contentHolder_txtsysProductName', 1, 60, false, null, '商品实际名称字符长度在1-60之间'));
            
            initValid(new InputValidator('ctl00_contentHolder_txtProductTitle', 0, 500, true, null, '商品副标题的长度不能超过500个字符'));

            initValid(new InputValidator('ctl00_contentHolder_txtDisplaySequence', 1, 10, false, '-?[0-9]\\d*', '数据类型错误，只能输入整数型数值'))
            appendValid(new NumberRangeValidator('ctl00_contentHolder_txtDisplaySequence', 1, 9999999, '输入的数值超出了系统表示范围'));
            initValid(new InputValidator('ctl00_contentHolder_txtProductCode', 0, 30, true, null, '商家编码的长度不能超过30个字符'));
            initValid(new InputValidator('ctl00_contentHolder_txtSalePrice', 1, 10, false, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '数据类型错误，只能输入实数型数值'))
            appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtSalePrice', 0, 99999999, '输入的数值超出了系统表示范围'));

            if (!$("#ctl00_contentHolder_txtDeductFee").val()) {
                initValid(new InputValidator('ctl00_contentHolder_txtCostPrice', 1, 10, true, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '数据类型错误，只能输入实数型数值'))
                appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtCostPrice', 0, 99999999, '输入的数值超出了系统表示范围'));
            }

            initValid(new InputValidator('ctl00_contentHolder_txtMarketPrice', 1, 10, true, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '数据类型错误，只能输入实数型数值'))
            appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtMarketPrice', 0, 99999999, '输入的数值超出了系统表示范围'));
            initValid(new InputValidator('ctl00_contentHolder_txtSku', 0, 20, true, '^[0-9a-zA-Z\-\_\.]+$', '数据类型错误，不能输入中文'));
            appendValid(new InputValidator('ctl00_contentHolder_txtSku', 0, 20, true, null, '货号的长度不能超过20个字符'));
            initValid(new InputValidator('ctl00_contentHolder_txtStock', 0, 9, false, '-?[0-9]\\d*', '数据类型错误，只能输入整数型数值'))
            appendValid(new NumberRangeValidator('ctl00_contentHolder_txtStock', 0, 9999999, '输入的数值超出了系统表示范围'));

            initValid(new InputValidator('ctl00_contentHolder_txtFactStock', 0, 9, false, '-?[0-9]\\d*', '数据类型错误，只能输入整数型数值'))
            appendValid(new NumberRangeValidator('ctl00_contentHolder_txtFactStock', 0, 9999999, '输入的数值超出了系统表示范围'));

            //initValid(new InputValidator('ctl00_contentHolder_txtUnit', 1, 20, true, '[a-zA-Z\/\u4e00-\u9fa5]*$', '必须限制在20个字符以内且只能是英文和中文例:g/元'))
            initValid(new InputValidator('ctl00_contentHolder_txtWeight', 0, 10, true, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '数据类型错误，只能输入实数型数值'))
            appendValid(new NumberRangeValidator('ctl00_contentHolder_txtWeight', 1, 9999999, '输入的数值超出了系统表示范围'));

            initValid(new InputValidator('ctl00_contentHolder_txtGrossWeight', 0, 10, true, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '数据类型错误，只能输入实数型数值'))
            appendValid(new NumberRangeValidator('ctl00_contentHolder_txtGrossWeight', 1, 9999999, '输入的数值超出了系统表示范围'));

            initValid(new InputValidator('ctl00_contentHolder_txtShortDescription', 0, 300, true, null, '商品简介必须限制在300个字符以内'));
            initValid(new InputValidator('ctl00_contentHolder_txtTitle', 0, 100, true, null, '详细页标题长度限制在100个字符以内'));
            initValid(new InputValidator('ctl00_contentHolder_txtMetaDescription', 0, 260, true, null, '详细页描述长度限制在260个字符以内'));
            initValid(new InputValidator('ctl00_contentHolder_txtMetaKeywords', 0, 160, true, null, '详细页搜索关键字长度限制在160个字符以内'));

            initValid(new InputValidator('ctl00_contentHolder_txtReferralDeduct', 1, 10, true, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '设置直接推广佣金'))
            appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtReferralDeduct', 0, 100, '输入的数值超出了系统表示范围'));
            initValid(new InputValidator('ctl00_contentHolder_txtSubMemberDeduct', 1, 10, true, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '设置下级会员佣金'))
            appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtSubMemberDeduct', 0, 100, '输入的数值超出了系统表示范围'));
            initValid(new InputValidator('ctl00_contentHolder_txtSubReferralDeduct', 1, 10, true, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '设置下级推广员佣金'))
            appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtSubReferralDeduct', 0, 100, '输入的数值超出了系统表示范围'));

           

            //判断是否是组合商品
            if ($("#ctl00_contentHolder_dropSaleType").val() == 2) {
                initValid(new InputValidator('ctl00_contentHolder_txtBarCode', 0, 50, true, null, '长度不能超过50个字符'));
                initValid(new InputValidator('ctl00_contentHolder_txtManufacturer', 0, 100, true, null, '必填项，长度不能超过100个字符'));
                initValid(new InputValidator('ctl00_contentHolder_txtItemNo', 0, 500, true, null, '必填项，长度不能超过500个字符'));
                initValid(new InputValidator('ctl00_contentHolder_txtIngredient', 0, 500, true, null, '必填项，长度不能超过500个字符'));
            }
            else {
                initValid(new InputValidator('ctl00_contentHolder_txtBarCode', 0, 50, false, null, '长度不能超过50个字符'));

                initValid(new InputValidator('ctl00_contentHolder_txtManufacturer', 1, 100, false, null, '必填项，长度不能超过100个字符'));
                initValid(new InputValidator('ctl00_contentHolder_txtItemNo', 1, 500, false, null, '必填项，长度不能超过500个字符'));
                initValid(new InputValidator('ctl00_contentHolder_txtIngredient', 1, 500, false, null, '必填项，长度不能超过500个字符'));
            }
            

            initValid(new InputValidator('ctl00_contentHolder_txtConversionRelation', 0, 9, false, '[1-9]\\d*', '数据类型错误，只能输入整数型数值'));
            appendValid(new NumberRangeValidator('ctl00_contentHolder_txtConversionRelation', 0, 9999999, '输入的数值格式错误'));

            //if (!$("#ctl00_contentHolder_txtCostPrice").val()) {
            //    initValid(new InputValidator('ctl00_contentHolder_txtDeductFee', 1, 10, false, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '数据类型错误，只能输入实数型数值'))
            //    appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtDeductFee', 0, 1, '输入的数值超出了系统表示范围'));
            //}

        }
        function changeSupplier(obj) {
            var value = $(obj).val();
            var oldValue = $('#<%:txtProductCode.ClientID%>').val();
            if (oldValue.indexOf("-") != -1) {
                var separatorIndex = oldValue.indexOf("-");
                oldValue = oldValue.substr(separatorIndex + 1);
            }
            if (value == '' || value == undefined) {
                $('#<%:txtProductCode.ClientID%>').val('0-' + oldValue);
                return;
            }
            var index = value.indexOf("|");
            if (value.substr(index + 1) == '') {
                $('#<%:txtProductCode.ClientID%>').val('0-' + oldValue);
                return;
            }
            $('#<%:txtProductCode.ClientID%>').val(value.substr(index + 1) + '-' + oldValue);
        }
        function autoCalcPrice(obj) {
            var salePrice = $.trim($('#<%:txtSalePrice.ClientID%>').val());
            if (isNaN(salePrice)) {
                return;
            }
            salePrice = parseFloat(salePrice);
            var oid = $(obj).attr('id');
            if (oid == '<%:txtCostPrice.ClientID%>') {//根据一口价与成本价计算扣点
                var costPrice = $('#' + oid).val();
                if (isNaN(costPrice)) {
                    $('#<%:txtDeductFee.ClientID%>').val('');
                    return;
                }
                costPrice = parseFloat(costPrice);
                var fee = (salePrice - costPrice) / salePrice;
                $('#<%:txtDeductFee.ClientID%>').val(isNaN(fee) ? '' : fee.toFixed(2));
            } else {//根据一口价与扣点计算成本价
                var deductFee = $('#' + oid).val();
                if (isNaN(deductFee)) {
                    $('#<%:txtCostPrice.ClientID%>').val('');
                    return;
                }
                deductFee = parseFloat(deductFee);
                var price = salePrice - salePrice * deductFee;
                $('#<%:txtCostPrice.ClientID%>').val(isNaN(price) ? '' : price.toFixed(2));
            }
        }
        $(document).ready(function () {
            changeSupplier('#<%:ddlSupplier.ClientID%>');
            $('#<%:txtCostPrice.ClientID%>').change(function () { autoCalcPrice(this); }).keyup(function () { autoCalcPrice(this); });
            $('#<%:txtDeductFee.ClientID%>').change(function () { autoCalcPrice(this); }).keyup(function () { autoCalcPrice(this); });
            InitValidators();

            //成本价和扣点只能填写一个，因为一种商品只能有一种结算方式
            //$("#ctl00_contentHolder_txtCostPrice").change(function () {
            //    var costPrice = $(this);
            //    if ($.trim(costPrice.val())) {
            //        //$("#deductFeeRow").hide();
            //        //$("#ctl00_contentHolder_txtDeductFee").hide();
            //        $("#ctl00_contentHolder_txtDeductFee").removeAttr("ValidateGroup");
            //        $("#ctl00_contentHolder_txtDeductFee").val("");
            //        $("#ctl00_contentHolder_txtCostPrice").attr("ValidateGroup", "default");
            //    }
            //}); 

            //$("#ctl00_contentHolder_txtDeductFee").change(function () {
            //    var costPrice = $(this);
            //    if ($.trim(costPrice.val())) {
            //        //$("#costPriceRow").hide();
            //        //$("#ctl00_contentHolder_txtCostPrice").hide();
            //        $("#ctl00_contentHolder_txtCostPrice").removeAttr("ValidateGroup");
            //        $("#ctl00_contentHolder_txtCostPrice").val("");
            //        $("#ctl00_contentHolder_txtDeductFee").attr("ValidateGroup", "default");
            //    }
            //});

            //$("input[name='txtNum']").delegate("onchange", function () {
            //    alert($(this).html());
            //        if (num == "" || num == 'undefined') {
            //            alert("组合商品数量只能为正整数!");
            //            flag = false;
            //            $(item).children().eq(3).find("input").focus();
            //            return false;
            //        }
            //        if (isNaN(num) || parseInt(num) <= 0) {
            //            alert("组合商品数量只能为正整数!");
            //            flag = false;
            //            $(item).children().eq(3).find("input").focus();
            //            return false;
            //        }
            //});
            //$("#combinamtionShow").hide();
            //var selectCombination = $("#ctl00_contentHolder_dropSaleType");
            //selectCombination.bind("change", function () {
            //    if (selectCombination.val() == "2") {
            //        $("#combinamtionShow").show();
            //        updateDisplayStatus(false, false);
            //    } else {
            //        $("#combinamtionShow").hide();
            //    }
            //})
        });
    </script>
    <script type="text/javascript" src="attributes.helper.js?v=20160315"></script>
    <script type="text/javascript" src="grade.price.helper.js?v=20160314"></script>
    <script type="text/javascript" src="producttag.helper.js?v=20160314"></script>
    <script type="text/javascript" src="CombinationProduct.js?v=20160314"></script>
    <script type="text/javascript">
        /*$(document).ready(function () {
            //如果成本价有值，那么就隐藏扣点；如果扣点有值，那么隐藏成本价，否则只显示成本价。
            var costPrice = $("#ctl00_contentHolder_txtCostPrice");
            var deductFee = $("#ctl00_contentHolder_txtDeductFee");
            if ($.trim(costPrice.val())) {
                $("#deductFeeRow").css("display", "none");
                deductFee.removeAttr("ValidateGroup");
                deductFee.val("");
            }
            else if ($.trim(deductFee.val())) {
                $("#costPriceRow").css("display", "none");
                costPrice.removeAttr("ValidateGroup");
                costPrice.val("");
            }
            else {
                $("#deductFeeRow").css("display", "none");
                deductFee.hide();
                deductFee.removeAttr("ValidateGroup");
                deductFee.val("");
            }
        });*/
    </script>
</asp:Content>
