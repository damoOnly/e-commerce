<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ImportFromLocal.aspx.cs" Inherits="EcShop.UI.Web.Admin.product.ImportFromLocal" Title="无标题页" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.ControlPanel.Utility" Assembly="EcShop.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
<div class="optiongroup mainwidth">
		<ul>
        <li class="menucurrent"><a class="optioncurrentend" ><span class="optioncenter">从本地数据包导入</span></a></li>
			<li class="optionstar"><a href="ImportFromTB.aspx"><span>从淘宝数据包导入</span></a></li>
			<li><a href="ImportFromPP.aspx" class="optionnext"><span>从拍拍数据包导入</span></a></li>
		</ul>
</div>
<div class="dataarea mainwidth databody">

<div class="datafrom">
<div class="formitem">
            <ul>
              <li><h2 class="colorE">数据包信息</h2></li>
              <li> <span style="color:Red;">本地数据包的来源：商品助理导出的文件和文件夹放在同一目录下，以zip的格式打成压缩包</span></li>
              <li>
                <span class="formitemtitle Pw_198">选择要导入的数据包文件： </span>
                <asp:DropDownList runat="server" ID="dropFiles"></asp:DropDownList>
                <span>
                导入之前需要先将数据包文件上传到服务器上；<br/>
                如果上面的下拉框中没有您要导入的数据包文件，请先上传。
                </span>
              </li>
              <li> <span class="formitemtitle Pw_198"></span>
                <asp:FileUpload runat="server" ID="fileUploader" /><asp:Button runat="server" ID="btnUpload" Text="上传" OnClick="btnUpload_Click"/>               
                <span>
                    上传数据包须小于40M，否则可能上传失败
                </span>
              </li>
              </ul>
            <ul class="btntf Pa_198">
                <asp:Button ID="btnImport" runat="server" CssClass="submit_DAqueding inbnt" Text="导 入" />
            </ul>
            <div class="blank12 clearfix"></div>
        </div>
    </div>

</div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
