<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="AddProduct.aspx.cs" Inherits="EcShop.UI.Web.Admin.AddProduct" %>

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
        <div class="title  m_none td_bottom">
            <em>
                <img src="../images/01.gif" width="32" height="32" /></em>
            <h1>�����Ʒ</h1>
        </div>
        <div class="datafrom">
            <div class="formitem validator1">
                <ul>
                    <li>
                        <h2 class="colorE">������Ϣ</h2>
                    </li>
                    <li><span class="formitemtitle Pw_198">������Ʒ���ࣺ</span> <span class="colorE float fonts">
                        <asp:Literal runat="server" ID="litCategoryName"></asp:Literal></span> [<asp:HyperLink
                            runat="server" ID="lnkEditCategory" CssClass="a" Text="�༭"></asp:HyperLink>]
                    </li>
                    <li><span class="formitemtitle Pw_198">��Ʒ���ͣ�</span>
                        <abbr class="formselect">
                            <Hi:ProductTypeDownList runat="server" CssClass="productType" ID="dropProductTypes" NullToDisplay="--��ѡ��--" /></abbr>
                        Ʒ�ƣ�<abbr class="formselect"><Hi:BrandCategoriesDropDownList runat="server" ID="dropBrandCategories" NullToDisplay="--��ѡ��--" /></abbr>
                    </li>
                    <li class=" clearfix"><span class="formitemtitle Pw_198">��Ʒ���ƣ�<em>*</em></span>
                        <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtProductName" Width="350px" />
                        <p id="ctl00_contentHolder_txtProductNameTip">
                            �޶���60���ַ�
                        </p>
                    </li>
                    <li class=" clearfix"><span class="formitemtitle Pw_198">��Ʒʵ�����ƣ�<em>*</em></span>
                        <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtsysProductName" Width="350px" />
                        <p id="ctl00_contentHolder_txtsysProductNameTip">
                            �޶���60���ַ�
                        </p>
                    </li>
                     <li class=" clearfix"><span class="formitemtitle Pw_198">Ӣ�����ƣ�</span>
                        <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtEnglishName" Width="350px" />
                        <p id="ctl00_contentHolder_txtEnglishName">
                            �޶���60���ַ�
                        </p>
                    </li>
                    <li class=" clearfix"><span class="formitemtitle Pw_198">��Ʒ�����⣺</span>
                        <Hi:TrimTextBox runat="server" Rows="6" Height="100px" Columns="76" TextMode="MultiLine"  ID="txtProductTitle" />
                        <p id="ctl00_contentHolder_txtProductTitleTip">
                            �޶���500���ַ�
                        </p>
                    </li>
                    <li id="saleType"><span class="formitemtitle Pw_198">�������ͣ�<em>*</em></span>
                        <abbr class="formselect">
                            <asp:DropDownList ID="dropSaleType" runat="server">
                                <asp:ListItem Value="1" Selected="True">����</asp:ListItem>
                                <asp:ListItem Value="2">���</asp:ListItem>
                            </asp:DropDownList>
                        </abbr>
                    </li>
                    <li id="supplierRow"><span class="formitemtitle Pw_198">�����̣�<em>*</em></span>
                        <abbr class="formselect">
                            <Hi:SupplierDropDownList runat="server" ID="ddlSupplier" OnClientChange="changeSupplier(this)" />
                        </abbr>
                    </li>
                    <li><span class="formitemtitle Pw_198">��Ʒ���룺</span>
                        <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtProductCode" />
                        <p id="ctl00_contentHolder_txtProductCodeTip">
                            ���Ȳ��ܳ���30���ַ�
                        </p>
                    </li>
                    <li><span class="formitemtitle Pw_198">������λ��<em>*</em></span>
                        <abbr class="formselect">
                            <Hi:UnitDropDownList runat="server" ID="ddlUnit" />
                        </abbr>
                    </li>
                    <li><span class="formitemtitle Pw_198">�����ϵ��<em>*</em></span>
                        <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtConversionRelation" Text="1" MaxLength="10" />
                        <p id="ctl00_contentHolder_txtConversionRelationTip">
                            �����ϵΪ����1����
                        </p>
                    </li>

                    <li><span class="formitemtitle Pw_198">�г��ۣ�</span>
                        <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtMarketPrice" />&nbsp;Ԫ
                        <p id="ctl00_contentHolder_txtMarketPriceTip">
                            ��վ��Ա����������Ʒ�г���
                        </p>
                    </li>

                    <li><span class="formitemtitle Pw_198">�������ң�<em>*</em></span>
                        <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtManufacturer" />
                        <p id="ctl00_contentHolder_txtManufacturerTip">
                            ���Ȳ��ܳ���100���ַ�
                        </p>
                    </li>
                    <li><span class="formitemtitle Pw_198">�ͺţ�<em>*</em></span>
                        <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtItemNo" />
                        <p id="ctl00_contentHolder_txtItemNoTip">
                            ���Ȳ��ܳ���500���ַ� 
                        </p>
                    </li>
                    <li><span class="formitemtitle Pw_198">�����룺<em>*</em></span>
                        <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtBarCode" />
                        <p id="ctl00_contentHolder_txtBarCodeTip">
                            ���Ȳ��ܳ���50���ַ�
                        </p>
                    </li>
                    <li><span class="formitemtitle Pw_198">�ɷ֣�<em>*</em></span>
                        <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtIngredient" />
                        <p id="ctl00_contentHolder_txtIngredientTip">
                            ���Ȳ��ܳ���500���ַ�
                        </p>
                    </li>

                    <li>
                        <h2 class="colorE">��չ����</h2>
                    </li>
                    <li id="attributeRow" style="display: none;"><span class="formitemtitle Pw_198">��Ʒ���ԣ�</span>
                        <div class="attributeContent" id="attributeContent">
                        </div>
                        <Hi:TrimTextBox runat="server" ID="txtAttributes" TextMode="MultiLine" Style="display: none;"></Hi:TrimTextBox>
                    </li>
                    <li id="skuCodeRow"><span class="formitemtitle Pw_198">���ţ�</span>
                        <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtSku" />
                        <p id="ctl00_contentHolder_txtSkuTip">
                            �޶���20���ַ���������������
                        </p>
                    </li>
                    <li id="salePriceRow"><span class="formitemtitle Pw_198">һ�ڼۣ�<em>*</em></span>
                        <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtSalePrice" />&nbsp;Ԫ<input
                            type="button" onclick="editProductMemberPrice();" value="�༭��Ա��" style="margin-left: 10px;" />
                        <Hi:TrimTextBox runat="server" ID="txtMemberPrices" TextMode="MultiLine" Style="display: none;"></Hi:TrimTextBox>
                        <p id="ctl00_contentHolder_txtSalePriceTip">
                            ��վ��Ա����������Ʒ���ۼ�
                        </p>
                    </li>
                    <li id="costPriceRow"><span class="formitemtitle Pw_198">�ɱ��ۣ�<em>*</em></span>
                        <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtCostPrice" />&nbsp;Ԫ
                        <p id="ctl00_contentHolder_txtCostPriceTip">
                            ��Ʒ�ĳɱ���
                        </p>
                    </li>
                    <li id="deductFeeRow"><span class="formitemtitle Pw_198">�۵㣺</span><Hi:TrimTextBox
                        runat="server" CssClass="forminput" ID="txtDeductFee" />
                        <p id="ctl00_contentHolder_txtDeductFeeTip">
                            ��Ʒ�Ŀ۵�
                        </p>
                    </li>
                    <li id="qtyRow"><span class="formitemtitle Pw_198">���ۿ�棺<em>*</em></span><Hi:TrimTextBox
                        runat="server" CssClass="forminput" ID="txtStock" Text="0" Enabled="false" />
                        <p id="ctl00_contentHolder_txtStockTip">
                            ��Ʒ�����ۿ��<em>��ǰ����WMS��ȡ</em>
                        </p>
                    </li>
                    <li id="factQtyRow"><span class="formitemtitle Pw_198">��Ʒ��棺<em>*</em></span><Hi:TrimTextBox
                        runat="server" CssClass="forminput" Text="0" ID="txtFactStock" Enabled="false" />
                        <p id="ctl00_contentHolder_txtFactStockTip">
                            ��Ʒ���
                        </p>
                    </li>
                    <li><span class="formitemtitle Pw_198">���������</span><Hi:TrimTextBox
                        runat="server" CssClass="forminput" ID="txtBuyCardinality" Text="1" />
                        <p id="ctl00_contentHolder_txtBuyCardinalityTip">
                            Ĭ��ֵΪ1����ʾ�������������������Ϊ2����Ϊ2����������������������2�ı������Դ����ơ�
                        </p>
                    </li>
                    <li id="weightRow"><span class="formitemtitle Pw_198">��Ʒ��λ���أ�</span><Hi:TrimTextBox
                        runat="server" CssClass="forminput" ID="txtWeight" />&nbsp;��</li>

                    <li id="grossweightRow"><span class="formitemtitle Pw_198">��Ʒ��λë�أ�</span><Hi:TrimTextBox
                        runat="server" CssClass="forminput" ID="txtGrossWeight" />&nbsp;��</li>
                    <li><span class="formitemtitle Pw_198">�Ƿ���Ҫ��أ� </span>
                        <asp:CheckBox ID="ChkisCustomsClearance" runat="server" />
                        &nbsp;</li>
                    <li id="productTaxRateRow"><span class="formitemtitle Pw_198">˰�ʣ�</span>
                        <abbr class="formselect">
                            <Hi:TaxRateDropDownList runat="server" CssClass="productType" Enabled="false" ID="dropTaxRate" /></abbr>
                        <p>
                            �����ɹ������ѡ�񣬴˴����������
                        </p>
                    </li>
                    <li id="shippingRow"><span class="formitemtitle Pw_198">�˷�ģ�棺<em>*</em></span>
                        <abbr class="formselect">
                            <Hi:ShippingTemplatesDropDownList runat="server" ID="ddlShipping" />
                        </abbr>
                    </li>
                    <li id="ImportSourceTypeRow"><span class="formitemtitle Pw_198">ԭ���أ�<em>*</em></span>
                        <abbr class="formselect">
                            <Hi:ImportSourceTypeDropDownList runat="server" ID="ddlImportSourceType" NullToDisplay="--��ѡ��ԭ����--" />
                        </abbr>
                    </li>
                    <li id="ProductStandard"><span class="formitemtitle Pw_198">��Ʒ���</span>
                        <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtProductStandard" />
                        <p id="ctl00_contentHolder_txtProductStandardTip">
                            δ��������ʱ��д
                        </p>
                    </li>
                    <div id="combinamtionShow" style="display: none;">
                        <li id="combinationProducts"><span class="formitemtitle Pw_198">�����Ʒ��</span>
                            <em>*</em><span style="cursor: pointer; color: blue; font-size: 14px" onclick="ShowAddDiv();">����ѡ��</span>
                        </li>
                        <li class="binditems">
                            <table width="100%" id="addlist">
                                <tr class="table_title">
                                    <th class="td_right td_left" scope="col">��Ʒ��</th>
                                    <th class="td_right td_left" scope="col">sku��Ϣ</th>
                                    <th class="td_right td_left" scope="col">�۸�</th>
                                    <th class="td_right td_left" scope="col">����</th>
                                    <th class="td_right td_left" scope="col">ë��</th>
                                    <th class="td_right td_left" scope="col">����</th>
                                    <th class="td_left td_right_fff" scope="col">����</th>
                                </tr>
                            </table>
                            <input id="selectProductsinfo" name="selectProductsinfo" type="hidden" />
                        </li>
                    </div>
                    <li><span class="formitemtitle Pw_198">�Ƿ������ </span>
                        <asp:CheckBox ID="ChkisPromotion" runat="server" />
                        &nbsp;</li>

                    <li><span class="formitemtitle Pw_198">�Ƿ���ʾ�ۿۣ� </span>
                        <asp:CheckBox ID="ChkisDisplayDiscount" runat="server" />
                        &nbsp;</li>

                    <li id="skuTitle" style="display: none;">
                        <h2 class="colorE">��Ʒ���</h2>
                    </li>
                    <li id="enableSkuRow" style="display: none;"><span class="formitemtitle Pw_198">���</span><input
                        id="btnEnableSku" type="button" value="�������" />
                        �������ǰ����д������Ϣ�����Զ�������Ϣ��ÿ�����</li>
                    <li id="skuRow" style="display: none;">
                        <p id="skuContent">
                            <input type="button" id="btnshowSkuValue" value="���ɲ��ֹ��" />
                            <input type="button" id="btnAddItem" value="����һ�����" />
                            <input type="button" id="btnCloseSku" value="�رչ��" />
                            <input type="button" id="btnGenerateAll" value="�������й��" />
                        </p>
                        <p id="skuFieldHolder" style="margin: 0px auto; display: none;">
                        </p>
                        <div id="skuTableHolder">
                        </div>
                        <Hi:TrimTextBox runat="server" ID="txtSkus" TextMode="MultiLine" Style="display: none;"></Hi:TrimTextBox>
                        <asp:CheckBox runat="server" ID="chkSkuEnabled" Style="display: none;" />
                    </li>
                    <li>
                        <h2 class="colorE">ͼƬ������</h2>
                    </li>
                    <li><span class="formitemtitle Pw_198">��ƷͼƬ��</span>
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
                        <p class="Pa_198 clearfix">
                            ͼƬӦС��120k��jpg,gif,jpeg,png��bmp��ʽ������Ϊ500x500����
                        </p>
                    </li>
                    <li class="clearfix"><span class="formitemtitle Pw_198">��Ʒ��飺</span>
                        <Hi:TrimTextBox runat="server" Rows="6" Height="100px" Columns="76" ID="txtShortDescription"
                            TextMode="MultiLine" />
                        <p class="Pa_198">
                            �޶���300���ַ�����
                        </p>
                    </li>
                    <li class="clearfix m_none"><span class="formitemtitle Pw_198">��Ʒ������</span> <span
                        class="tab">
                        <div class="status">
                            <ul>
                                <li style="clear: none;"><a onclick="ShowNotes(1)" class="off" id="tip1" style="cursor: pointer">PC��</a></li>
                                <li style="clear: none;"><a onclick="ShowNotes(2)" id="tip2" style="cursor: pointer">�ƶ���</a></li>
                            </ul>
                        </div>
                    </span><span style="padding-left: 198px;">
                        <div id="notes1">
                            <Kindeditor:KindeditorControl ID="editDescription" runat="server" Width="830" Height="300px" />
                        </div>
                        <div id="notes2" style="display: none;">
                            <Kindeditor:KindeditorControl ID="editmobbileDescription" runat="server" Width="830"
                                ImportLib="false" Height="300px" />
                        </div>
                    </span>
                        <p style="color: Red;">
                            <asp:CheckBox runat="server" ID="ckbIsDownPic" Text="�Ƿ�������Ʒ������վͼƬ" />
                        </p>
                        <p class="Pa_198">�����ѡ��ѡ��ʱ����Ʒ��������վ��ͼƬ������ص����أ��෴�򲻻ᣬ����Ҫ����ͼƬ�����ͼƬ�����ͼƬ�ܴ���Ҫ���ص�ʱ��Ͷ࣬������ѡ��</p>
                    </li>
                    <li>
                        <h2 class="colorE clear">�������</h2>
                    </li>
                    <li>
                           <span class="formitemtitle Pw_198">�Ƿ��޹���</span>
                           <asp:RadioButtonList ID="Rd_Purchase" runat="server" RepeatDirection="Horizontal">
                                 <asp:ListItem Value="0" Selected="True" onclick="PurchaseChange(0)">���޹�</asp:ListItem>
                                 <asp:ListItem Value="1" onclick="PurchaseChange(1)">�޹�</asp:ListItem>
                           </asp:RadioButtonList>
                    </li>
                    <li id="li_Purchase" style="display:none" >
                        <ul style="padding-left: 58px;">
                        	 <li><span class="formitemtitle Pw_140"><em >*</em>�޹�ʱ�䣺</span>
                                    <asp:TextBox ID="txtSectionDay" runat="server" CssClass="forminput" Text="0"></asp:TextBox>(��)
                              </li>
                              <li><span class="formitemtitle Pw_140"><em >*</em>�޹�������</span>
                                    <asp:TextBox ID="txtMaxCount" runat="server" CssClass="forminput" Text="0"></asp:TextBox>
                                   <p id="P4">�޹�ʱ���ÿ�����ܹ����������</p>
                              </li>
                           </ul>
                    </li>
                    <li><span class="formitemtitle Pw_198">ֱ���ƹ�Ӷ��</span>
                        <asp:TextBox runat="server" CssClass="forminput" ID="txtReferralDeduct" />&nbsp;%
                        <asp:Literal ID="litReferralDeduct" runat="server" />
                        <p id="txtReferralDeductTip" runat="server">�ƹ�Ա�������Ӳ�����Ч����ʱ���ܵ�Ӷ�����������д����ȫվͳһ����</p>
                    </li>
                    <li><span class="formitemtitle Pw_198">�¼���ԱӶ��</span>
                        <asp:TextBox ID="txtSubMemberDeduct" CssClass="forminput" runat="server" />&nbsp;%
                        <asp:Literal ID="litSubMemberDeduct" runat="server" />
                        <p id="txtSubMemberDeductTip" runat="server">�¼���Աδͨ���������ӽ����̳�ʱ�����Ķ������ƹ�Ա�����ܵ�Ӷ�����������д����ȫվͳһ����</p>
                    </li>
                    <li><span class="formitemtitle Pw_198">�¼��ƹ�ԱӶ��</span>
                        <asp:TextBox ID="txtSubReferralDeduct" CssClass="forminput" runat="server" />&nbsp;%
                        <asp:Literal ID="litSubReferralDeduct" runat="server" />
                        <p id="txtSubReferralDeductTip" runat="server">�ƹ�Ա�������Ӳ�����Ч����ʱ�����ϼ��ƹ�ԱҲ�ɻ����Ӧ��Ӷ�����������д����ȫվͳһ����</p>
                    </li>
                    <li><span class="formitemtitle Pw_198">��Ʒ����״̬��</span>
                        <asp:RadioButton runat="server" ID="radOnSales" GroupName="SaleStatus" Text="������"></asp:RadioButton>
                        <asp:RadioButton runat="server" ID="radUnSales" GroupName="SaleStatus" Text="�¼���"></asp:RadioButton>
                        <asp:RadioButton runat="server" ID="radInStock" GroupName="SaleStatus" Text="�ֿ���" Checked="true"></asp:RadioButton>
                    </li>
                    <li style="display: none"><span class="formitemtitle Pw_198">��Ʒ���ʣ� </span>
                        <asp:CheckBox ID="ChkisfreeShipping" runat="server" />
                        &nbsp;</li>
                    <li class="clearfix" id="l_tags" runat="server"><span class="formitemtitle Pw_198">��Ʒ��ǩ���壺<br />
                        <a id="a_addtag" href="javascript:void(0)" onclick="javascript:AddTags()" class="add">���</a></span>
                        <div id="div_tags">
                            <Hi:ProductTagsLiteral ID="litralProductTag" runat="server"></Hi:ProductTagsLiteral>
                        </div>
                        <div id="div_addtag" style="display: none">
                            <input type="text" id="txtaddtag" /><input type="button" value="����" onclick="return AddAjaxTags()" />
                        </div>
                        <Hi:TrimTextBox runat="server" ID="txtProductTag" TextMode="MultiLine" Style="display: none;"></Hi:TrimTextBox>
                    </li>
                    <li style="display: none;"><span class="formitemtitle Pw_198">��Ա���ۣ�</span>
                        <Hi:YesNoRadioButtonList runat="server" RepeatLayout="Flow" ID="radlEnableMemberDiscount"
                            YesText="�����Ա����" NoText="�������Ա����" />
                    </li>
                    <li>
                        <h2 class="colorE">�����Ż�</h2>
                    </li>
                    <li><span class="formitemtitle Pw_198">��ϸҳ���⣺</span>
                        <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtTitle" Width="350px" />
                        <p id="ctl00_contentHolder_txtTitleTip">
                            ��ϸҳ����������100�ַ�����
                        </p>
                    </li>
                    <li><span class="formitemtitle Pw_198">��ϸҳ������</span>
                        <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtMetaDescription" Width="350px" />
                        <p id="ctl00_contentHolder_txtMetaDescriptionTip">
                            ��ϸҳ����������260�ַ�����
                        </p>
                    </li>
                    <li><span class="formitemtitle Pw_198">��ϸҳ�����ؼ��֣�</span>
                        <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtMetaKeywords" Width="350px" />
                        <p id="ctl00_contentHolder_txtMetaKeywordsTip">
                            ��ϸҳ�����ؼ���������160�ַ�����
                        </p>
                    </li>
                </ul>
                <ul class="btntf Pa_198 clear">
                    <asp:HiddenField ID="hiddenSkus" runat="server"></asp:HiddenField>
                    <asp:Button runat="server" ID="btnAdd" Text="�� ��" OnClientClick="return doSubmit()&&CollectInfos();"
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
            <img src="../images/icon_dalata.gif" alt="�ر�" width="38" height="20" />
        </div>
        <div class="mianform ">
            <div id="priceContent">
            </div>
            <div style="margin-top: 10px; text-align: center;">
                <input type="button" value="ȷ��" onclick="doneEditPrice();" />
            </div>
        </div>
    </div>
    <div class="Pop_up" id="skuValueBox" style="display: none;">
        <h1>
            <span>ѡ��Ҫ���ɵĹ��</span>
        </h1>
        <div class="img_datala">
            <img src="../images/icon_dalata.gif" alt="�ر�" width="38" height="20" />
        </div>
        <div class="mianform ">
            <div id="skuItems">
            </div>
            <div style="margin-top: 10px; text-align: center;">
                <input type="button" value="ȷ��" id="btnGenerate" />
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript" src="attributes.helper.js?v=20160315"></script>
    <script type="text/javascript" src="grade.price.helper.js?v=20160314"></script>
    <script type="text/javascript" src="producttag.helper.js?v=20160314"></script>
    <script type="text/javascript" src="CombinationProduct.js?v=20160314"></script>
    <script type="text/javascript" language="javascript">
        function PurchaseChange(obj)
        {
            if (obj == "0")
            {
                $("#li_Purchase").hide();
            }
            else {
                $("#li_Purchase").show();
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
            //initValid(new SelectValidator('ctl00_contentHolder_dropProductLines', false, '��ѡ����Ʒ��������Ʒ��'));
            initValid(new InputValidator('ctl00_contentHolder_txtProductName', 1, 60, false, null, '��Ʒ�����ַ�������1-60֮��'));
            initValid(new InputValidator('ctl00_contentHolder_txtsysProductName', 1, 60, false, null, '��Ʒʵ�������ַ�������1-60֮��'));
            initValid(new InputValidator('ctl00_contentHolder_txtProductTitle', 0, 500, true, null, '��Ʒ������ĳ��Ȳ��ܳ���500���ַ�'));

            initValid(new InputValidator('ctl00_contentHolder_txtProductCode', 0, 30, true, null, '�̼ұ���ĳ��Ȳ��ܳ���30���ַ�'));
            if ($("#ctl00_contentHolder_txtSalePrice").attr("readonly") == "readonly") {
               initValid(new InputValidator('ctl00_contentHolder_txtSalePrice', 1, 10, false, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '�������ʹ���ֻ������ʵ������ֵ'));
               appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtSalePrice', 0, 99999999, '�������ֵ������ϵͳ��ʾ��Χ'));
            }

            if (!$("#ctl00_contentHolder_txtDeductFee").val()) {
                initValid(new InputValidator('ctl00_contentHolder_txtCostPrice', 1, 10, true, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '�������ʹ���ֻ������ʵ������ֵ'));
                appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtCostPrice', 0, 99999999, '�������ֵ������ϵͳ��ʾ��Χ'));
            }

            initValid(new InputValidator('ctl00_contentHolder_txtMarketPrice', 1, 10, true, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '�������ʹ���ֻ������ʵ������ֵ'));
            appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtMarketPrice', 0, 99999999, '�������ֵ������ϵͳ��ʾ��Χ'));

            initValid(new InputValidator('ctl00_contentHolder_txtSku', 0, 20, true, '^[0-9a-zA-Z\-\_\.]+$', '�������ʹ��󣬲�����������'));
            appendValid(new InputValidator('ctl00_contentHolder_txtSku', 0, 20, true, null, '���ŵĳ��Ȳ��ܳ���20���ַ�'));

            initValid(new InputValidator('ctl00_contentHolder_txtStock', 0, 9, false, '-?[0-9]\\d*', '�������ʹ���ֻ��������������ֵ'));
            appendValid(new NumberRangeValidator('ctl00_contentHolder_txtStock', 0, 9999999, '�������ֵ������ϵͳ��ʾ��Χ'));

            initValid(new InputValidator('ctl00_contentHolder_txtFactStock', 0, 9, false, '-?[0-9]\\d*', '�������ʹ���ֻ��������������ֵ'));
            appendValid(new NumberRangeValidator('ctl00_contentHolder_txtFactStock', 0, 9999999, '�������ֵ������ϵͳ��ʾ��Χ'));

            //initValid(new InputValidator('ctl00_contentHolder_txtUnit', 1, 20, true, '[a-zA-Z\/\u4e00-\u9fa5]*$', '����������20���ַ�������ֻ����Ӣ�ĺ�������:g/Ԫ'));
            initValid(new InputValidator('ctl00_contentHolder_txtWeight', 0, 10, true, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '�������ʹ���ֻ������ʵ������ֵ'));
            appendValid(new NumberRangeValidator('ctl00_contentHolder_txtWeight', 1, 9999999, '�������ֵ������ϵͳ��ʾ��Χ'));

            initValid(new InputValidator('ctl00_contentHolder_txtGrossWeight', 0, 10, true, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '�������ʹ���ֻ������ʵ������ֵ'));
            appendValid(new NumberRangeValidator('ctl00_contentHolder_txtGrossWeight', 1, 9999999, '�������ֵ������ϵͳ��ʾ��Χ'));

            initValid(new InputValidator('ctl00_contentHolder_txtShortDescription', 0, 300, true, null, '��Ʒ������������300���ַ�����'));
            initValid(new InputValidator('ctl00_contentHolder_txtTitle', 0, 100, true, null, '��ϸҳ���ⳤ��������100���ַ�����'));
            initValid(new InputValidator('ctl00_contentHolder_txtMetaDescription', 0, 260, true, null, '��ϸҳ��������������260���ַ�����'));
            initValid(new InputValidator('ctl00_contentHolder_txtMetaKeywords', 0, 160, true, null, '��ϸҳ�����ؼ��ֳ���������160���ַ�����'));

            initValid(new InputValidator('ctl00_contentHolder_txtReferralDeduct', 1, 10, true, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '����ֱ���ƹ�Ӷ��'))
            appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtReferralDeduct', 0, 100, '�������ֵ������ϵͳ��ʾ��Χ'));
            initValid(new InputValidator('ctl00_contentHolder_txtSubMemberDeduct', 1, 10, true, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '�����¼���ԱӶ��'))
            appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtSubMemberDeduct', 0, 100, '�������ֵ������ϵͳ��ʾ��Χ'));
            initValid(new InputValidator('ctl00_contentHolder_txtSubReferralDeduct', 1, 10, true, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '�����¼��ƹ�ԱӶ��'))
            appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtSubReferralDeduct', 0, 100, '�������ֵ������ϵͳ��ʾ��Χ'));

           
            
            //�ж��Ƿ��������Ʒ
            if ($("#ctl00_contentHolder_dropSaleType").val() == 2) {
                initValid(new InputValidator('ctl00_contentHolder_txtBarCode', 0, 50, true, null, '���Ȳ��ܳ���50���ַ�'));
                initValid(new InputValidator('ctl00_contentHolder_txtManufacturer', 0, 100, true, null, '��������Ȳ��ܳ���100���ַ�'));
                initValid(new InputValidator('ctl00_contentHolder_txtItemNo', 0, 500, true, null, '��������Ȳ��ܳ���500���ַ�'));
                initValid(new InputValidator('ctl00_contentHolder_txtIngredient', 0, 500, true, null, '��������Ȳ��ܳ���500���ַ�'));
            }
            else
            {
                initValid(new InputValidator('ctl00_contentHolder_txtBarCode', 0, 50, false, null, '���Ȳ��ܳ���50���ַ�'));

                initValid(new InputValidator('ctl00_contentHolder_txtManufacturer', 1, 100, false, null, '��������Ȳ��ܳ���100���ַ�'));
                initValid(new InputValidator('ctl00_contentHolder_txtItemNo', 1, 500, false, null, '��������Ȳ��ܳ���500���ַ�'));
                initValid(new InputValidator('ctl00_contentHolder_txtIngredient', 1, 500, false, null, '��������Ȳ��ܳ���500���ַ�'));
            }
            

            initValid(new InputValidator('ctl00_contentHolder_txtConversionRelation', 0, 9, false, '[1-9]\\d*', '�������ʹ���ֻ��������������ֵ'));
            appendValid(new NumberRangeValidator('ctl00_contentHolder_txtConversionRelation', 0, 9999999, '�������ֵ��ʽ����'));

            //if (!$("#ctl00_contentHolder_txtCostPrice").val()) {
            //    initValid(new InputValidator('ctl00_contentHolder_txtDeductFee', 1, 10, false, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '�������ʹ���ֻ������ʵ������ֵ'))
            //    appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtDeductFee', 0, 1, '�������ֵ������ϵͳ��ʾ��Χ'));
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
            if (oid == '<%:txtCostPrice.ClientID%>') {//����һ�ڼ���ɱ��ۼ���۵�
                var costPrice = $('#' + oid).val();
                if (isNaN(costPrice)) {
                    $('#<%:txtDeductFee.ClientID%>').val('');
                    return;
                }
                costPrice = parseFloat(costPrice);
                var fee = (salePrice - costPrice) / salePrice;
                $('#<%:txtDeductFee.ClientID%>').val(isNaN(fee) ? '' : fee.toFixed(2));
            } else {//����һ�ڼ���۵����ɱ���
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

            //�ɱ��ۺͿ۵�ֻ����дһ������Ϊһ����Ʒֻ����һ�ֽ��㷽ʽ
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
            $("#combinamtionShow").hide();
            var selectCombination = $("#ctl00_contentHolder_dropSaleType");
            selectCombination.bind("change", function () {
                if (selectCombination.val() == "2") {
                    $("#combinamtionShow").show();
                    updateDisplayStatus(false, false);

                    editValid(new InputValidator('ctl00_contentHolder_txtBarCode', 0, 50, true, null, '���Ȳ��ܳ���50���ַ�'));
                    initValid(new InputValidator('ctl00_contentHolder_txtManufacturer', 0, 100, true, null, '��������Ȳ��ܳ���100���ַ�'));
                    initValid(new InputValidator('ctl00_contentHolder_txtItemNo', 0, 500, true, null, '��������Ȳ��ܳ���500���ַ�'));
                    initValid(new InputValidator('ctl00_contentHolder_txtIngredient', 0, 500, true, null, '��������Ȳ��ܳ���500���ַ�'));
                } else {
                    $("#combinamtionShow").hide();

                    editValid(new InputValidator('ctl00_contentHolder_txtBarCode', 0, 50, false, null, '���Ȳ��ܳ���50���ַ�'));
                    initValid(new InputValidator('ctl00_contentHolder_txtManufacturer', 1, 100, false, null, '��������Ȳ��ܳ���100���ַ�'));
                    initValid(new InputValidator('ctl00_contentHolder_txtItemNo', 1, 500, false, null, '��������Ȳ��ܳ���500���ַ�'));
                    initValid(new InputValidator('ctl00_contentHolder_txtIngredient', 1, 500, false, null, '��������Ȳ��ܳ���500���ַ�'));
                }
            })

            selectCombinationValue = selectCombination.val();
            if (selectCombinationValue == "2") {
                SetSaleTypeRead();
                $("#combinamtionShow").show();
                updateDisplayStatus(false, false);
            } else {
                RemoveSaleTypeRead();
            }

            var appledList = $("#addlist").find("tr[name='appendlist']");
            if (appledList == null || appledList.length == 0) {
                $("#ctl00_contentHolder_txtWeight").val("");
                $("#ctl00_contentHolder_txtGrossWeight").val("");
                $("#ctl00_contentHolder_txtSalePrice").val("");
            }
        });
    </script>
</asp:Content>
