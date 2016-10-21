<%@ Control Language="C#"%>
<%@ Import Namespace="EcShop.Core" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.SaleSystem.Tags" Assembly="EcShop.UI.SaleSystem.Tags" %><div class="hotclass_list">
 <span class="mainclass"> <a href='<%# Globals.GetSiteUrls().SubCategory(Convert.ToInt32(Eval("CategoryId")), Eval("RewriteName")) %>'><%# Eval("CategoryName")%></a></span>
 <asp:Repeater ID="repHotKeyword" runat="server" DataSource='<%# DataBinder.Eval(Container, "DataItem.relation") %>'>
     <ItemTemplate>
         <span>
             <span>
                <a href=''<%# Globals.GetSiteUrls().SubCategory((int) Eval("CategoryId"), null) + "?keywords=" + Globals.UrlEncode((string)Eval("Keywords")) %>>
                    <Hi:SubStringLabel ID="SubStringLabel" Field="Keywords" runat="server"  />
                </a>
            </span>
            <span>|</span>
         </span>
     </ItemTemplate>
 </asp:Repeater>  
</div>
 <div class="clearboth"></div>