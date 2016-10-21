<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"  Inherits="EcShop.UI.Web.Admin.AddBrandTag" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" Runat="Server">
    <div class="areacolumn clearfix">
      <div class="columnright">
          <div class="title">
            <em><img src="../images/03.gif" width="32" height="32" /></em>
            <h1>添加品牌标签</h1>
            <span>管理各个品牌，对品牌进行标记 </span>
          </div>
      <div class="formitem validator2">
        <ul>
          <li> <span class="formitemtitle Pw_100"><em >*</em>品牌标签名称：</span>
            <asp:TextBox ID="txtBrandName" CssClass="forminput" runat="server" />
            <p id="ctl00_contentHolder_txtBrandNameTip">品牌标签名称不能为空，长度限制在50个字符以内</p>
          </li>          
          <li> <span class="formitemtitle Pw_100">关联品牌：</span>
                  <span style="float:left;"><Hi:BrandCategoriesCheckBoxList runat="server" ID="chlistBrand" /></span>            
                </li>
      </ul>
      <ul class="btntf Pa_100 clear">
        <asp:Button ID="btnSave" OnClientClick="return PageIsValid();" Text="保 存" CssClass="submit_DAqueding float" runat="server"/>
        <asp:Button ID="btnAddBrandCategory" OnClientClick="return PageIsValid();" Text="保存并继续添加" CssClass="submit_jixu" runat="server"/>
        </ul>
      </div>

      </div>
  </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" Runat="Server">
     <script type="text/javascript" language="javascript">
            function InitValidators() {
                initValid(new InputValidator('ctl00_contentHolder_txtBrandName', 1, 50, false, null, '品牌标签名称不能为空，长度限制在50个字符以内'));
        
            }
            $(document).ready(function() { InitValidators(); });
       </script>
</asp:Content>

