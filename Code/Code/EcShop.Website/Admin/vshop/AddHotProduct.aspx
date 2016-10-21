<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"  Inherits="EcShop.UI.Web.Admin.vshop.AddHotProduct" %>

<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
<%@ Import Namespace="EcShop.Core" %>
<script type="text/javascript">
    var auth = "<%=(Request.Cookies[FormsAuthentication.FormsCookieName]==null ? string.Empty : Request.Cookies[FormsAuthentication.FormsCookieName].Value) %>";
</script>
    <script src="../js/swfupload/swfupload.js" type="text/javascript"></script>
    <script src="../js/UploadHandler.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server" ClientIDMode="Static">
<div class="areacolumn clearfix">
      <div class="columnright">
          <div class="title">
            <em><img src="../images/03.gif" width="32" height="32" /></em>
            <h1 id="h1title">添加热卖商品</h1>
            <span id="spanTitle" class="font">添加热卖商品</span>
      </div>
          <div class="formitem validator2">
            <ul>
              <li><span class="formitemtitle Pw_100">热卖商品描述：</span>
                <asp:TextBox ID="txtBannerDesc" runat="server" Width="600px" CssClass="forminput" />
              </li>              
              <li runat="server" id="liParent"><span class="formitemtitle Pw_100">上传图片：</span>
                <span id="spanButtonPlaceholder"></span>
                                <span id="divFileProgressContainer"></span>
								<%--<div>图片建议尺寸：650px * 320px</div>--%>
              </li>  
              <li id="smallpic" style="display: none;"> 
                  <img id="littlepic" runat="server" src=""/>
                             <!--封面上传后，返回的图片地址，填充下面的input对象。-->
                           </li>      
              <li> <span class="formitemtitle Pw_100">跳转至：</span>
                <asp:DropDownList ID="ddlType" runat="server" CssClass="forminput droptype" ClientIDMode="Static">
                </asp:DropDownList>
                <asp:DropDownList ID="ddlSubType" name="ddlSubType" runat="server"  CssClass="forminput droptype" style="display:none"   ClientIDMode="Static">
                </asp:DropDownList>
                 <asp:DropDownList ID="ddlThridType" name="ddlThridType" runat="server"  CssClass="forminput droptype" style="display:none"   ClientIDMode="Static">
                </asp:DropDownList>
                 <Hi:ProductCategoriesDropDownList ID="dropCategories" runat="server" NullToDisplay="--请选择商品分类--" Width="150" CssClass="forminput droptype" Style="display: none"/>

                 <Hi:ImportSourceTypeDropDownList ID="dropImportSourceType" runat="server" NullToDisplay="--请选择原产地--" Width="200" CssClass="forminput droptype" Style="display: none"/>
                 <Hi:BrandTypeDropDownList ID="dropBrandTypes" runat="server" NullToDisplay="--请选择品牌--" Width="200" CssClass="forminput droptype" Style="display: none"/>
                   <div id="productfield">
                      <a id="linkSelectProduct" style="cursor: pointer; color: blue; font-size: 14px;display:none" runat="server" onclick="ShowAddDiv();">请点此选择商品:</a>
                      <a id="productName" style="display:none" runat="server"></a>
                      <input type="hidden" runat="server" id="productid" />
                   </div>
              <asp:TextBox ID="Tburl" style="display:none; Width:350px" CssClass="forminput" runat="server" ClientIDMode="Static"></asp:TextBox>
                                  <span ID="navigateDesc" runat="server" style="display:none;"><a target="_blank" href="http://www.ecdev.cn/bbs/thread-193492-1-1.html">获取导航地址</a></span>
              </li>
            </ul>
              <ul class="btn Pa_100 clearfix">
                <asp:Button ID="btnAddBanner" runat="server" 
                      OnClientClick="return GetLoctionUrl();" Text="确 定"  
                      CssClass="submit_DAqueding float" onclick="btnAddBanner_Click" />
         </ul>
         <!--隐藏图片地址-->
               <input id="fmSrc" runat="server" clientidmode="Static" type="hidden" value="" />
               <input id="locationUrl" runat="server" clientidmode="Static" type="hidden" value="" />   
              <br />

            <ul>
                <li>图片规则说明：</li>
                <li>大图：商品升序排序第一个商品 为大图，规格是640*任意高度</li>
                <li>小图：商品升序排序第二个起的商品 为大图，规格是320*任意高度,各小图的高度要一致，否则会显示错乱</li>
                <li>图文说明：
                    <img src="../images/hot_sale.jpg" />
                </li>
           </ul> 
          </div>
  </div>    
  </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script src="../js/UploadBanner.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            ShowType();
            BindType();
            // $("#ddlType").val("Link").trigger("change");
            //ShowThirdDropDown();
            var fmSrc = $("#fmSrc").val();
            if (fmSrc == null || fmSrc == "") {
                $("#ddlType").val("Link").trigger("change");
                return;
            }
            else {
                $("#h1title").text("编辑热卖商品");
                $("#spanTitle").html("编辑热卖商品");
                $("#smallpic").show();
            }
        }
        );

        function ShowAddDiv() {
            DialogFrame("./App/SelectOneProduct.aspx", "添加商品", 800, 700);
        }

    </script>
    <script src="../js/AppLocationType.js" type="text/javascript"></script>
</asp:Content>
