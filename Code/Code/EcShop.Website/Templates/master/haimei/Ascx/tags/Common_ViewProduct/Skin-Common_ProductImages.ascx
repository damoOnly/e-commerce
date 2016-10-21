<%@ Control Language="C#" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Validator" Assembly="EcShop.UI.Common.Validator" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.SaleSystem.Tags" Assembly="EcShop.UI.SaleSystem.Tags" %>
<style type="text/css">
.zoom-section{clear:both; /*overflow:hidden;*/}
*html .zoom-section{display:inline;clear:both;}
.zoom-desc{/*padding:12px 0px;*/  margin-top: 10px;overflow:hidden; /*float:left;*/}
.zoom-desc p{overflow:hidden; }
.zoom-desc a{float:left;width:76px; height:76px;/*border-radius:2px;*/  display:block; border:1px solid #d2d2d2; margin-right:5px; margin-bottom:5px; background:#fff;overflow:hidden;text-align:center;}
.zoom-desc a img{max-width:100%;}
.zoom-desc a.hover{border:1px solid #0099cc;}
.zoom-small-image{/*float:left;*/width:504px;/*height:383px;*/border:1px solid #c9c9c9; background:#fff;  }
.zoom-small-image a{min-height:419px;}
.zoom-tiny-image{margin:0px;}
.zoom-tiny-image:hover{border:1px solid #cc000;}
.zoom-small-image table{table-layout:fixed;}
#ProductDetails_common_ProductImages___imgBig{width:504px;}
</style>
<table cellpadding="0" cellspacing="0" border="0">

    <tr class="product_preview3">
        <td>
            <div class="zoom-section">
                <div class="zoom-section">
                	
                    <div class="zoom-small-image">
                        <asp:HyperLink runat="server" CssClass="cloud-zoom" ID='zoom1' rel="adjustX:10, adjustY:-4"
                            ClientIDMode="Static">
                            <table cellspacing="0" cellpadding="0" border="0" width="100%" height="100%">
                                <tr>
                                    <td align="center" valign="middle">
                                        <Hi:HiImage ID="imgBig" runat="server" title="Optional title display" AlternateText="" />
                                    </td>
                                </tr>
                            </table>
                        </asp:HyperLink>
                    </div>
                    <div class="zoom-desc">
                        <p>
                            <asp:HyperLink ID="iptPicUrl1" runat="server" CssClass="cloud-zoom-gallery"  title="">
                                <Hi:HiImage ID="imgSmall1" runat="server" alt="Thumbnail 1" />
                            </asp:HyperLink>
                            <asp:HyperLink ID="iptPicUrl2" runat="server" CssClass="cloud-zoom-gallery"  title="">
                                <Hi:HiImage ID="imgSmall2" runat="server" alt="Thumbnail 2" CssClass="zoom-tiny-image" />
                            </asp:HyperLink>
                            <asp:HyperLink ID="iptPicUrl3" runat="server" CssClass="cloud-zoom-gallery"  title="">
                                <Hi:HiImage ID="imgSmall3" runat="server" alt="Thumbnail 3" CssClass="zoom-tiny-image" />
                            </asp:HyperLink>
                            <asp:HyperLink ID="iptPicUrl4" runat="server" CssClass="cloud-zoom-gallery"  title="">
                                <Hi:HiImage ID="imgSmall4" runat="server" alt="Thumbnail 4" CssClass="zoom-tiny-image" />
                            </asp:HyperLink>
                            <asp:HyperLink ID="iptPicUrl5" runat="server" CssClass="cloud-zoom-gallery" title="">
                                <Hi:HiImage ID="imgSmall5" runat="server" alt="Thumbnail 5" CssClass="zoom-tiny-image" />
                            </asp:HyperLink>
                        </p>
                    </div>
                </div>
            </div>
        </td>
    </tr>
    <tr>
        <td height="10">
        </td>
    </tr>
   
</table>

