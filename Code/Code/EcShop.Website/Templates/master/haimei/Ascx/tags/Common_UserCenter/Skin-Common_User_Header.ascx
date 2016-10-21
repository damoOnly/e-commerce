<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.SaleSystem.Tags" Assembly="EcShop.UI.SaleSystem.Tags" %>
<%@ Import Namespace="EcShop.Core" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <Hi:HeadContainer ID="HeadContainer1" runat="server" />
    <Hi:PageTitle ID="PageTitle1" runat="server" />
    <Hi:MetaTags ID="MetaTags1" runat="server" />     
    <Hi:TemplateStyle ID="Stylee1" runat="server" Href="/style/user.css"></Hi:TemplateStyle>
    <Hi:TemplateStyle ID="Stylee2" runat="server" Href="/style/style.css"></Hi:TemplateStyle>
    <Hi:TemplateStyle ID="Style1" runat="server" Href="/style/menu.css"></Hi:TemplateStyle>
    <link href="/Utility/validate/pagevalidator.css" rel="stylesheet" type="text/css" />
    <link href="/style/usertypes.css" rel="stylesheet" type="text/css" />
    <Hi:Script ID="Script2" runat="server" Src="/utility/jquery-1.6.4.min.js" />
    <Hi:Script ID="Script1" runat="server" Src="/utility/globals.js" />
    <script type="text/javascript">
        function searchs() {
            var item = $("#drop_Search_Class").val();
            var key = $("#txt_Search_Keywords").val();
            if (key == undefined)
                key = "";

            key = key.replace(/&/g, '&amp;').replace(/"/g, '&quot;').replace(/'/g, '&#39;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace('/', '');

            var url = applicationPath + "/SubCategory.aspx?keywords=" + encodeURIComponent(key);
            if (item != undefined)
                url += "&categoryId=" + item;
            window.location.href = url;
        }

        $(document).ready(function () {
            $('#txt_Search_Keywords').keydown(function (e) {
                if (e.keyCode == 13) {
                    searchs();
                    return false;
                }
            })
        });
	  
  </script>
  <script>
	var _hmt = _hmt || [];
	(function() {
	  var hm = document.createElement("script");
	  hm.src = "//hm.baidu.com/hm.js?4d35a99853f12e55bb76639cc97344ee";
	  var s = document.getElementsByTagName("script")[0]; 
	  s.parentNode.insertBefore(hm, s);
	})();
	</script>
</head>
<body>

<Hi:Common_OnlineServer ID="Common_OnlineServer1" runat="server"></Hi:Common_OnlineServer>
<div id="header">		
        <div class="top_nav_bg">
            <div class="top_nav1">                
                <div class="top_link fix">                
                    <div class="login_re">
                        <Hi:Common_CurrentUser runat="server" ID="lblCurrentUser" /> 
                        </div>
                		<div class="top_linkLeft"><span id="site" style="display:none;">深圳</span>
                            <div class="drops" id="drops"></div>
                            <input id="Hidd_SiteId" type="hidden" value="-1"/>
                        </div>
    
                                           <script type="text/javascript">
                                               function Showxinren_tab() {
                                                   document.getElementById("xinren_tab").style.display = "block";
                                               }

                                               function Hiddenxinren_tab() {
                                                   document.getElementById("xinren_tab").style.display = "none";
                                               }
</script>

                    <div class="link_list">
                    	<div class="lg-bar">
                        	<Hi:Common_MyAccountLink ID="linkMyAccount" runat="server" /><Hi:Common_LoginLink ID="Common_Link_Login1" runat="server" /> 
                        </div><span>|</span>
                         <span>
                            <%--<Hi:SiteUrl  UrlName="user_UserOrders" runat="server">我的帐户</Hi:SiteUrl>--%>
                            <a href="/user/UserOrders.aspx">我的订单</a>
                         </span>
                        <span>|</span> <span>
                            <Hi:SiteUrl ID="SiteUrl4" UrlName="AllHelps" runat="server">帮助中心</Hi:SiteUrl></span>
                            <span>|</span><div id="shoucang"><Hi:Common_CustomAd runat="server" AdId="28" /></div>
                    </div>
                </div>
            </div>
        </div>
        <div class="display_no"> <Hi:Common_CustomAd runat="server" AdId="17" /></div>
        <div class="header1_p1 fix">
            <div class="logo">                
                <a href="/">
                	<img src="/templates/master/haimei/images/logo-member.png">
                </a>
            </div>
         
         <div class="jf_ad1"><Hi:Common_ImageAd runat="server" AdId="2" /></div>
         <div class="jf_ad3">
               <div class="search_tab">
                <div class="search">
                    <Hi:Common_Search ID="Common_Search" runat="server" />
                </div>  
            </div> 
            
            <div class="remen"><label>热门搜索：</label><Hi:Common_SubjectKeyword ID="Common_SubjectKeyword1" runat="server" CommentId="4" /></div>
            
         </div>
         <div class="jf_ad4">
            <div class="top_buycart fix">
            	<Hi:Common_ShoppingCart_Info ID="Common_ShoppingCart_Info1" runat="server" />
            </div>
            <div class="tel"><Hi:Common_ImageAd runat="server" AdId="4" /></div>
            </div>
        </div>
        <div class="nav_bg nobor">
            <div class="nav1">
          <div class="side_nav">
                    <h3 class="title">
                        <Hi:SiteUrl ID="SiteUrl5" UrlName="Category" runat="server">
                        所有商品分类</Hi:SiteUrl>
                    </h3>
                    <div class="my_left_category">
                        <Hi:Common_CategoriesWithWindow ID="Common_CategoriesWithWindow1" MaxCNum="12" runat="server" />
                    </div>
                </div>
                
                <div class="main_nav" id="ty_menu_title">
                    <ul>
                        <li><a href="/"><span>首页</span></a></li>
                        <Hi:Common_PrimaryClass ID="Common_PrimaryClass1" runat="server" />
                        <Hi:Common_HeaderMune ID="Common_HeaderMune1" runat="server" />
                    </ul>
                </div>
               
            
                
            </div>
        </div>
        </div>
        <div class="tb-wrap" id="tb-wrap">
            <div class="tb-con">                
                <div class="st-wrap"><a href="javascript:void(0);" id="st-btn"><span>回到顶部</span></a></div>
            </div>
         </div>
        <script type="text/javascript">
			 var currenturl = location.href.substr(location.href.lastIndexOf('/') + 1, location.href.length - 20);
                if (currenturl != "Desig_Templete.aspx?skintemp=default") {
                    $(".side_nav").hover(function () {
                        $(".my_left_category").css({ 'display': 'block' });
                    }, function () {
                        $(".my_left_category").css({ 'display': 'none' });
                    });
                } 
				// $(function(){
						    $(".h2_cat").each(function(){
					        $(this).hover(function(){
					           if($(this).hasClass('active_cat')){
					                $(this).removeClass("active_cat");
					            }else{
					                $(this).addClass('active_cat');
					            }
					        });
					    });
        	//购物车 数量为0时 不显示
					function toggleCartNum(){
						var $cartNum = $("#cart-num");
						var val = parseInt($cartNum.text());
						if(val){
							$cartNum.show();
						}
					}
					toggleCartNum();
					$(function(){
						//滚动
						$(window).bind("scroll.tb", function () {
								var $tbWrap = $("#tb-wrap");
								var scrooltop = $(this).scrollTop();
								if (scrooltop > 0) {
									$tbWrap.show();
								} else {
									$tbWrap.hide();
								}					
							})
							//滚动到顶部
							$("#st-btn").bind("click",function(){
								var st = 0;
								$('html,body').stop().animate({ scrollTop: st }, 200);
							})
					})	
        </script>
        <script type="text/javascript" src="/Utility/switchsite.js"></script>        