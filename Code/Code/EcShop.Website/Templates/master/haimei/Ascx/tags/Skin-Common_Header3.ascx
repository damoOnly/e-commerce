<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.SaleSystem.Tags" Assembly="EcShop.UI.SaleSystem.Tags" %>
<%@ Import Namespace="EcShop.Core" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=5.0,minimum-scale=0.5, user-scalable=yes">
<meta name="apple-mobile-web-app-capable" content="yes">
<meta name="apple-mobile-web-app-status-bar-style" content="black">
<meta name="format-detection" content="telephone=no">
<meta http-equiv="Expires" content="-1">
<meta http-equiv="Cache-Control" content="no-cache">
<meta http-equiv="Pragma" content="no-cache">

<meta http-equiv="Content-Type" content="text/html; charset=UTF-8" /><meta name="renderer" content="webkit"/><Hi:HeadContainer ID="HeadContainer1" runat="server" /> <Hi:PageTitle runat="server" /><Hi:MetaTags runat="server" />
<Hi:Script ID="Script2" runat="server" Src="/utility/jquery-1.6.4.min.js" />
    
    
    <Hi:Script ID="Script1" runat="server" Src="/utility/globals.js" />
    <Hi:TemplateStyle ID="Stylee1" runat="server" Href="/style/style.css?t=20160310"></Hi:TemplateStyle>
    <Hi:TemplateStyle ID="Style1" runat="server" Href="/style/menu.css"></Hi:TemplateStyle>
    <script charset="utf-8" src="http://wpa.b.qq.com/cgi/wpa.php"></script>
      <script type="text/javascript">
       //如果是网站首页直接跳转到WAP端首页，如果是其它页面，且该页面对应WAP端有相应的页面对直接中转到对应的WAP页面，如果没有则不跳转
        var pageurl = document.location.href.toLowerCase();
        var sUserAgent = navigator.userAgent.toLowerCase();
        var bIsIpad = sUserAgent.match(/ipad/i) == "ipad";
        var bIsIphoneOs = sUserAgent.match(/iphone os/i) == "iphone os";
        var bIsMidp = sUserAgent.match(/midp/i) == "midp";
        var bIsUc7 = sUserAgent.match(/rv:1.2.3.4/i) == "rv:1.2.3.4";
        var bIsUc = sUserAgent.match(/ucweb/i) == "ucweb";
        var bIsAndroid = sUserAgent.match(/android/i) == "android";
        var bIsCE = sUserAgent.match(/windows ce/i) == "windows ce";
        var bIsWM = sUserAgent.match(/windows mobile/i) == "windows mobile";
        //bIsWM = true;
        if ((/*bIsIpad ||*/ bIsIphoneOs || bIsMidp || bIsUc7 || bIsUc || bIsAndroid || bIsCE || bIsWM) && HasWapRight) {
            //解决分享到微信时，浏览器跳转到网站首页的问题
            DirectUrl = GetWapUrl();
            if (DirectUrl != ""){
                //location.href = "/Wapshop/" + DirectUrl;
			}
        }
        ///判断是否需要跳转，如果当前页面为首页或者帮助页，文单页则跳转，其它的不需要跳转
        function IsDirect() {

            if (pageurl.indexOf("/default.aspx") > -1) return true;
            if (pageurl.indexOf("/article/") >= -1 && pageurl.indexOf("/help/") >= -1) return false;
            return true;
        }
        ///获取PC端对应Wap端的页面地址，如果没有对应地址则返回空
        function GetWapUrl() {
            var PageKey = "";
            var port = document.location.port;
            var domain = document.domain;
            var param = document.location.search;
            if (port != "80" && port != "") domain = domain + ":" + port;
            domain = "http://" + domain;
            if ((pageurl.length == domain.length || (pageurl.length - 1) == domain.length) && pageurl.indexOf(domain) == 0) { return "default.aspx"; }
            if (pageurl.indexOf("default.aspx") > -1) { return "default.aspx" + param; }
            if (pageurl.indexOf("product_detail-") > -1) { PageKey = GetPageKey("product_detail-", false); return "ProductDetails.aspx?ProductId=" + PageKey; }
            if (pageurl.indexOf("productdetails.aspx") > -1) { return "ProductDetails.aspx" + param; }
            if (pageurl.indexOf("brand_detail-") > -1) { PageKey = GetPageKey("brand_detail-", false); return "BrandDetail.aspx?BrandId=" + PageKey; }
            if (pageurl.indexOf("brand.aspx") > -1) { return "brandlist.aspx" + param;; }
            if (pageurl.indexOf("category.aspx") > -1) { return "productsearch.aspx" + param; }
            if (pageurl.indexOf("browse/category-") > -1) { PageKey = GetPageKey("browse/category-", false); return "ProductList.aspx?categoryId=" + PageKey; }
            if (pageurl.indexOf("subcategory.aspx?keywords=") > -1) { PageKey = GetPageKey("keywords="); return "ProductList.aspx?keyWord=" + PageKey; }
            if (pageurl.indexOf("groupbuyproducts.aspx") > -1) { return "GroupBuyList.aspx" + param;; }
            if (pageurl.indexOf("groupbuyproduct_detail-") > -1) { PageKey = GetPageKey("groupbuyproduct_detail-"); return "GroupBuyProductDetails.aspx?groupbuyId=" + PageKey; }
            if (pageurl.indexOf("login.aspx") > -1) { return "login.aspx" + param; }
            if (pageurl.indexOf("register.aspx") > -1) { return "login.aspx" + param + (param == "" ? "?" : "&") + "&action=register"; }
            if (pageurl.indexOf("userdefault.aspx") > -1) { return "MemberCenter.aspx" + param; }
            if (pageurl.indexOf("userorders.aspx") > -1) { return "MemberOrders.aspx" + param; }
            if (pageurl.indexOf("usershippingaddresses.aspx") > -1) { return "ShippingAddresses.aspx" + param; }
            if (pageurl.indexOf("userprofile.aspx") > -1) { return "UserInfo.aspx" + param; }
            if (pageurl.indexOf("/user/") > -1) { return "MemberCenter.aspx"; }
            if (pageurl.indexOf("articles.aspx") > -1) { return "Articles.aspx" + param; }
            if (pageurl.indexOf("article/show-") > -1) { PageKey = GetPageKey("article/show-"); return "ArticleDetails.aspx?ArticleId=" + PageKey; }
            return "";
        }


        function GetPageKey(pagepre, IsEnd) {
            if (pageurl.indexOf(pagepre) > -1) {
                var endIndex = 0;
                if (!IsEnd) {
                    endIndex = pageurl.indexOf(".aspx");
                    if (endIndex <= -1) endIndex = pageurl.indexOf(".htm")
                }
                else {
                    endIndex = pageurl.length;
                }
                var startIndex = pageurl.indexOf(pagepre) + pagepre.length;
                if (startIndex >= 0 && endIndex > startIndex)
                    return pageurl.substring(startIndex, endIndex);
            }
            return "0";
        }

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
      <script type="text/javascript">
          var _adwq = _adwq || [];
          _adwq.push(['_setAccount', 'ybq8i']);
          _adwq.push(['_setDomainName', '.haimylife.com']);
          _adwq.push(['_trackPageview']);
          (function () {
              var adw = document.createElement('script');
              adw.type = 'text/javascript';
              adw.async = true;
              adw.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://s') + '.emarbox.com/js/adw.js';
              var s = document.getElementsByTagName('script')[0];
              s.parentNode.insertBefore(adw, s);
          })();
     </script> 
      <script type="text/javascript">
          var _mvq = _mvq || [];
          _mvq.push(['$setAccount', 'm-204746-0']);

          _mvq.push(['$logConversion']);
          (function () {
              var mvl = document.createElement('script');
              mvl.type = 'text/javascript'; mvl.async = true;
              mvl.src = ('https:' == document.location.protocol ? 'https://static-ssl.mediav.com/mvl.js' : 'http://static.mediav.com/mvl.js');
              var s = document.getElementsByTagName('script')[0];
              s.parentNode.insertBefore(mvl, s);
          })();

    </script>   
</head>
<body>
	<Hi:Common_OnlineServer ID="Common_OnlineServer1" runat="server"></Hi:Common_OnlineServer>
        <input id="hiid_AdUserId" clientidmode="Static" runat="server" type="hidden" />
    <div id="header">
        <div class="guild-wrap">
            <hi:common_imagenewad adid="newads_001" divcss="guild adv_image cssEdite"  runat="server" imageattr="class='lazyload' " />
            <div class="layout"><a class="guild-close" id="guild-close"></a></div>
        </div> 		
        <div class="top_nav_bg">
            <div class="top_nav1">                
                <div class="top_link fix">               
                    <div class="login_re">                        
                        <Hi:Common_CurrentUser runat="server" ID="lblCurrentUser" />          				 
                        </div>
                        <div class="lg-bar">
                        	<Hi:Common_MyAccountLink ID="linkMyAccount" runat="server" /><Hi:Common_LoginLink ID="Common_Link_Login1" runat="server" /> 
                        </div>
               			 <div class="top_linkLeft"><span id="site" style="display:none"></span>
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
                         <span>
                            <Hi:SiteUrl  UrlName="user_UserOrders" runat="server">我的订单</Hi:SiteUrl></span>
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
                	<img src="/templates/master/haimei/images/logo-help.png">
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
        </div></div>
   
		<div class="tb-wrap" id="tb-wrap">
            <div class="tb-con">                
                <div class="st-wrap"><a href="javascript:void(0);" id="st-btn"><span>回到顶部</span></a></div>
            </div>
         </div>
          
            <script>
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
						//})
    </script>
    </div>
    <script src="../../../../../Utility/switchsite.js?v=20150713" type="text/javascript"></script>
