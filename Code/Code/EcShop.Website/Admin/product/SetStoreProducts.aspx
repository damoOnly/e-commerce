<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="SetStoreProducts.aspx.cs" Inherits="EcShop.UI.Web.Admin.product.SetStoreProducts" Title="无标题页" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript">
    function ShowAddDiv()
    {
        var topicId = $("#ctl00_contentHolder_hdtopic").val().replace(/\s/g, "");
        if (topicId != "" && parseInt(topicId) > 0)
        {
            DialogFrame("./product/SearchStoreProduct.aspx?StoreId=" + topicId, "添加门店商品", 975, null);
        }
    }
    function PrintProduct()
    {
        var v_str = "";
        $("input[type='checkbox'][name='CheckBoxGroup']:checked").each(function (rowIndex, rowItem) {
            v_str += $(rowItem).attr("value") + ",";
        });
        if (v_str.length == 0) {
            alert("请选择商品");
            return "";
        }
        var StoreId = $("#ctl00_contentHolder_hdtopic").val().replace(/\s/g, "");
        if (StoreId == "") {
            alert("参数错误，请重新选择门店！");
            return "";
        }
        if (StoreId != "" && parseInt(StoreId) > 0) {
            DialogFrame("./product/PrintProduct.aspx?StoreId=" + StoreId + "&ProductsId=" + v_str, "商品打印", 975, null);
        }
    }

  
</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <script type="text/javascript" src="producttag.helper.js"></script>
<input type="hidden" id="hdtopic" runat="server" />
<div class="dataarea mainwidth databody">
	  <div class="title"> <em><img src="../images/03.gif" width="32" height="32" /></em>
	    <h1>门店“<asp:Literal runat="server" ID="litPromotionName" />“包括的商品 </h1>
     </div>       
	  <!--结束-->
    <div class="btn">
           <div style="float:right; margin-right:15px;" class="delete"><Hi:ImageLinkButton ID="btnDeleteAll"  runat="server" Text="清空" IsShow="true" DeleteMsg="确定要清空这些关联商品吗？" /></div>
            <a class="submit_jia" href="javascript:void(0)" onclick="ShowAddDiv()">添加关联商品</a>
            <a class="submit_jia" href="javascript:void(0)" onclick="PrintProduct()">打印商品</a>
    </div>
     <div class="batchHandleArea">
        <ul>
            <li class="batchHandleButton"><span class="signicon"></span><span class="allSelect">
                <a href="javascript:void(0)" onclick="SelectAll()">全选</a></span> <span class="reverseSelect">
                <a href="javascript:void(0)" onclick="ReverseSelect()">反选</a></span>   
            </li>
        </ul>
     </div>
	  <!--数据列表区域-->
  <div class="datalist datalist-img" style="clear:both">
	    <UI:Grid ID="grdTopicProducts" runat="server" ShowHeader="true" AutoGenerateColumns="false" DataKeyNames="ProductId" HeaderStyle-CssClass="table_title" GridLines="None" Width="100%">
            <Columns> 
                   <asp:TemplateField ItemStyle-Width="30px" HeaderText="选择" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <input name="CheckBoxGroup" type="checkbox" value='<%#Eval("ProductId") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="商品名称" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
		                    <div style="float:left; margin-right:10px;">
                                <a href='<%#"/ProductDetails.aspx?productId="+Eval("ProductId")%>' target="_blank">
                                        <Hi:ListImage ID="ListImage1"  runat="server" DataField="ThumbnailUrl40"/>      
                                 </a> 
                                 </div>
                                 <div style="float:left;">
                                 <span class="Name"> <a href='<%#"/ProductDetails.aspx?productId="+Eval("ProductId")%>' target="_blank"><%# Eval("ProductName") %></a></span>
                                  <span class="colorC">商家编码：<%# Eval("ProductCode") %></span></div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="二维码" ItemStyle-Width="80" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                     
                                <a href='<%#Eval("QRcode").ToString().Replace("~","") %>' class="tooltip" target="_blank"><img src='<%#Eval("QRcode").ToString().Replace("~","") %>' alt="二维码"  width="50px" height="50px"/></a>

                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="库存"  ItemStyle-Width="10%" HeaderStyle-CssClass="td_right td_left">
                            <itemtemplate>
                             <asp:Label ID="lblStock" runat="server" Text='<%# Eval("Stock") %>' Width="25"></asp:Label>
                          </itemtemplate>
                    </asp:TemplateField>
                    <Hi:MoneyColumnForAdmin HeaderText=" 市场价" ItemStyle-Width="10%"  DataField="MarketPrice" HeaderStyle-CssClass="td_right td_left"  />       
                    <Hi:MoneyColumnForAdmin HeaderText="一口价" ItemStyle-Width="10%"  DataField="SalePrice" HeaderStyle-CssClass="td_right td_left"  />        
                    
                     <asp:TemplateField HeaderText="排序"  ItemStyle-Width="10%" HeaderStyle-CssClass="td_right td_left">
                            <itemtemplate>
                           <asp:TextBox ID="txtSequence" runat="server" Text='<%# Eval("DisplaySequence") %>' Width="60px" />
                          </itemtemplate>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="操作" HeaderStyle-CssClass="td_left td_right_fff" >
                        <ItemStyle />
                        <itemtemplate> 
			                <span class="submit_shanchu"><Hi:ImageLinkButton  runat="server" ID="Delete" Text="删除" IsShow="true" CommandName="Delete" ></Hi:ImageLinkButton></span>
                        </itemtemplate>
                    </asp:TemplateField>
            </Columns>
        </UI:Grid>
    </div>
  <div style="padding:0px 0px 13px 15px">
        <div style="margin:0 auto;width:200px;"><asp:LinkButton ID="btnFinish" 
                runat="server" Text="　　保　存" CssClass="submit_DAqueding inbnt" 
                onclick="btnFinish_Click"></asp:LinkButton></div>
  </div>
  </div>
</asp:Content>
