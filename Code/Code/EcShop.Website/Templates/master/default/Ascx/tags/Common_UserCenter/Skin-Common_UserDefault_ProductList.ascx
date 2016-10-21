<%@ Control Language="C#" %>
<%@ Import Namespace="EcShop.Core" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<div class="hyzxconterr2">
                <div class="nTab1">
                    <div class="TabTitle">
                        <ul id="myTab1">
                            <li class="active" onmouseover="nTabs(this,0);">猜你喜欢</li>
                            <li class="normal" onmouseover="nTabs(this,1);">昨日热卖冠军</li>
                            <li class="normal" onmouseover="nTabs(this,2);">今日新品推荐</li>
                        </ul>
                    </div>
                    <div class="TabContent">
                        <div class="Display_entity_n_tab rollBox">
                            <div class="Display_entity_n_tab_licon">
                                <img src="/templates/master/default/images/users/hyzx_58.jpg" onmousedown="ISL_GoDown()" onmouseup="ISL_StopDown()"
                                    style="cursor: pointer;" /></div>
                            <div class="Cont" id="ISL_Cont">
                                <div class="ScrCont">
                                    <div id="List1">
                                        <!-- 图片列表 begin -->
                                        <ul id="myTab1_Content0">
                                        <asp:Repeater ID="rp_guest" runat="server">
                                        <ItemTemplate>
                                         <li>
                                            <div class="hyzxpic">
                                            <Hi:ProductDetailsLink ID="ProductDetailsLink1" runat="server"  ProductName='<%# Eval("ProductName") %>'  ProductId='<%# Eval("ProductId") %>' ImageLink="true">
                                                    <Hi:ListImage ID="Common_ProductThumbnail1" runat="server" DataField="ThumbnailUrl160" CustomToolTip="ProductName" />
                                                    </Hi:ProductDetailsLink></div>
                                            <div class="hyzxname">
                                                <Hi:ProductDetailsLink runat="server"  ProductName='<%# Eval("ProductName") %>'  ProductId='<%# Eval("ProductId") %>' ImageLink="true"><%# Eval("ProductName") %></Hi:ProductDetailsLink></div>
                                            <div class="hyzxprice">
                                                ￥<Hi:FormatedMoneyLabel  Money='<%# Eval("SalePrice") %>' runat="server" /></div>
                                            </li>
                                        </ItemTemplate>
                                        </asp:Repeater>
                                        </ul>
                                        <ul id="myTab1_Content1" style="display: none;">
                                        <asp:Repeater ID="rp_hot" runat="server">
                                        <ItemTemplate>
                                         <li>
                                                <div class="hyzxpic">
                                                <Hi:ProductDetailsLink ID="ProductDetailsLink1" runat="server"  ProductName='<%# Eval("ProductName") %>'  ProductId='<%# Eval("ProductId") %>' ImageLink="true">
                                                    <Hi:ListImage runat="server" DataField="ThumbnailUrl160" CustomToolTip="ProductName" />
                                                    </Hi:ProductDetailsLink></div>
                                                <div class="hyzxname">
                                                    <Hi:ProductDetailsLink runat="server"  ProductName='<%# Eval("ProductName") %>'  ProductId='<%# Eval("ProductId") %>' ImageLink="true"><%# Eval("ProductName") %></Hi:ProductDetailsLink></div>
                                                <div class="hyzxprice">
                                                    ￥<Hi:FormatedMoneyLabel Money='<%# Eval("SalePrice") %>' runat="server" /></div>
                                            </li>
                                        </ItemTemplate>
                                         
                                        </asp:Repeater>
                                        </ul>
                                        <ul id="myTab1_Content2" style="display: none;">
                                        <asp:Repeater ID="rp_new" runat="server">
                                        <ItemTemplate>
                                           <li>
                                                <div class="hyzxpic">
                                                <Hi:ProductDetailsLink ID="ProductDetailsLink1" runat="server"  ProductName='<%# Eval("ProductName") %>'  ProductId='<%# Eval("ProductId") %>' ImageLink="true">
                                                    <Hi:ListImage runat="server" DataField="ThumbnailUrl160" CustomToolTip="ProductName" />
                                                    </Hi:ProductDetailsLink></div>
                                                <div class="hyzxname">
                                                    <Hi:ProductDetailsLink runat="server"  ProductName='<%# Eval("ProductName") %>'  ProductId='<%# Eval("ProductId") %>' ImageLink="true"><%# Eval("ProductName") %></Hi:ProductDetailsLink></div>
                                                <div class="hyzxprice">
                                                    ￥<Hi:FormatedMoneyLabel Money='<%# Eval("SalePrice") %>' runat="server" /></div>
                                            </li>
                                        </ItemTemplate>
                                        </asp:Repeater>
                                        </ul>
                                    </div>
                                    <div id="List2">
                                    </div>
                                </div>
                            </div>
                            <div class="Display_entity_n_tab_ricon">
                                <img src="/templates/master/default/images/users/hyzx_61.jpg" onmousedown="ISL_GoUp()" onmouseup="ISL_StopUp()"
                                    onmouseout="ISL_StopUp()" style="cursor: pointer;" /></div>
                        </div>
                    </div>
                </div>
            </div>

            <script type="text/javascript">
                function nTabs(thisObj, Num) {
                    if (thisObj.className == "active") return;
                    var tabObj = thisObj.parentNode.id;
                    var tabList = document.getElementById(tabObj).getElementsByTagName("li");
                    for (i = 0; i < tabList.length; i++) {
                        var tmp = document.getElementById(tabObj + "_Content_copy" + i);

                        if (i == Number(Num)) {
                            thisObj.className = "active";
                            document.getElementById(tabObj + "_Content" + i).style.display = "block";
                            if (tmp != null)
                                tmp.style.display = "block";
                        } else {
                            tabList[i].className = "normal";
                            document.getElementById(tabObj + "_Content" + i).style.display = "none";
                            if (tmp != null)
                                tmp.style.display = "none";
                        }
                    }
                }


                //图片滚动列表 mengjia 070816
                var Speed = 10; //速度(毫秒)
                var Space = 10; //每次移动(px)
                var PageWidth = 190; //翻页宽度
                var fill = 0; //整体移位
                var MoveLock = false;
                var MoveTimeObj;
                var Comp = 0;
                var AutoPlayObj = null;
                GetObj("List2").innerHTML = GetObj("List1").innerHTML.replace("myTab1_Content0", "myTab1_Content_copy0").replace("myTab1_Content1", "myTab1_Content_copy1").replace("myTab1_Content2", "myTab1_Content_copy2").replace("myTab1_Content3", "myTab1_Content_copy3");
                GetObj('ISL_Cont').scrollLeft = fill;
                GetObj("ISL_Cont").onmouseover = function () { clearInterval(AutoPlayObj); }
                GetObj("ISL_Cont").onmouseout = function () { AutoPlay(); }
                AutoPlay();
                function GetObj(objName) {
                    if (document.getElementById) { return eval('document.getElementById("' + objName + '")') } else {
                        return eval

('document.all.' + objName)
                    }
                }
                function AutoPlay() { //自动滚动
                    clearInterval(AutoPlayObj);
                    AutoPlayObj = setInterval('ISL_GoDown();ISL_StopDown();', 3000); //间隔时间
                }
                function ISL_GoUp() { //上翻开始
                    if (MoveLock) return;
                    clearInterval(AutoPlayObj);
                    MoveLock = true;
                    MoveTimeObj = setInterval('ISL_ScrUp();', Speed);
                }
                function ISL_StopUp() { //上翻停止
                    clearInterval(MoveTimeObj);
                    if (GetObj('ISL_Cont').scrollLeft % PageWidth - fill != 0) {
                        Comp = fill - (GetObj('ISL_Cont').scrollLeft % PageWidth);
                        CompScr();
                    } else {
                        MoveLock = false;
                    }
                    AutoPlay();
                }
                function ISL_ScrUp() { //上翻动作
                    if (GetObj('ISL_Cont').scrollLeft <= 0) {
                        GetObj('ISL_Cont').scrollLeft = GetObj

('ISL_Cont').scrollLeft + GetObj('List1').offsetWidth
                    }
                    GetObj('ISL_Cont').scrollLeft -= Space;
                }
                function ISL_GoDown() { //下翻
                    clearInterval(MoveTimeObj);
                    if (MoveLock) return;
                    clearInterval(AutoPlayObj);
                    MoveLock = true;
                    ISL_ScrDown();
                    MoveTimeObj = setInterval('ISL_ScrDown()', Speed);
                }
                function ISL_StopDown() { //下翻停止
                    clearInterval(MoveTimeObj);
                    if (GetObj('ISL_Cont').scrollLeft % PageWidth - fill != 0) {
                        Comp = PageWidth - GetObj('ISL_Cont').scrollLeft % PageWidth + fill;
                        CompScr();
                    } else {
                        MoveLock = false;
                    }
                    AutoPlay();
                }
                function ISL_ScrDown() { //下翻动作
                    if (GetObj('ISL_Cont').scrollLeft >= GetObj('List1').scrollWidth) {
                        GetObj('ISL_Cont').scrollLeft =

GetObj('ISL_Cont').scrollLeft - GetObj('List1').scrollWidth;
                    }
                    GetObj('ISL_Cont').scrollLeft += Space;
                }
                function CompScr() {
                    var num;
                    if (Comp == 0) { MoveLock = false; return; }
                    if (Comp < 0) { //上翻
                        if (Comp < -Space) {
                            Comp += Space;
                            num = Space;
                        } else {
                            num = -Comp;
                            Comp = 0;
                        }
                        GetObj('ISL_Cont').scrollLeft -= num;
                        setTimeout('CompScr()', Speed);
                    } else { //下翻
                        if (Comp > Space) {
                            Comp -= Space;
                            num = Space;
                        } else {
                            num = Comp;
                            Comp = 0;
                        }
                        GetObj('ISL_Cont').scrollLeft += num;
                        setTimeout('CompScr()', Speed);
                    }
                }
 
            </script>