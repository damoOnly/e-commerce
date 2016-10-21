// JavaScript Document

//选择点击一级菜单显示
//selecttype
function ShowMenuLeft(firstnode, secondnode, threenode) {
    $.ajax({
        url: "Menu.xml?date=" + new Date(),
        dataType: "xml",
        type: "GET",
        async: false,
        timeout: 10000,
        error: function (xm, msg) {
            alert("loading xml error");
        },
        success: function (xml) {
            $("#menu_left").html('');
            var curenturl = null;
            curenturl = $(xml).find("Module[Title='" + firstnode.replace(/\s/g, "") + "']").attr("Link");
            if (secondnode != null) {
                curenturl = secondnode;
            }

            $(xml).find("Module[Title='" + firstnode.replace(/\s/g, "") + "'] Item").each(function (i) {
                $menutitle = $('<div class="ecshop_menutitle"></div>');
                $menuaspan = $("<span onclick='ShowSecond(this)'>" + $(this).attr("Title") + "</span>"); //获取二级分类名称
                $menutitle.append($menuaspan);
                $(this).find("PageLink").each(function (k) {
                    var link_href = $(this).attr("Link");
                    var link_title = $(this).attr("Title");
                    $alink = $("<a href='" + link_href + "' target='frammain'>" + link_title + "</a>");

                    if (link_href == curenturl) {
                        $alink.addClass("curent");
                    }
                    $menutitle.append($alink);
                    $menutitle.append('<div class="clean"></div>');
                });
                $("#menu_left").append($menutitle);
            });
            $("#menu_arrow").attr("class", "open_arrow").show();
			$(".ecshop_menu_scroll").show();           
            if (threenode != null) {
                curenturl = threenode;
            }			   
            $("#frammain").attr("src", curenturl);
        }
    });
    $(".ecshop_menu a:contains('" + firstnode + "')").addClass("ecshop_curent").siblings().removeClass("ecshop_curent");
}



//点击左右关闭树
function ExpendMenuLeft() {	
    if ($(".ecshop_menu_scroll").is(":hidden")) {//点击展开
        $("#menu_arrow").attr("class", "open_arrow");
        $(".ecshop_menu_scroll").show();        
    } else {//点击隐藏
        $("#menu_arrow").attr("class", "close_arrow");
        $(".ecshop_menu_scroll").hide();      
    }
}

//点击二级菜单
function ShowSecond(sencond) {
    if ($(sencond).siblings("a:hidden") != null && $(sencond).siblings("a:hidden").length > 0) {
        $(sencond).siblings("a").css("display", "block");
    } else {
        $(sencond).siblings("a").css("display", "none");
    }
}

//自适应高度
function AutoHeight() {
    var clientheight = $(this).height() - 87;
    $(".ecshop_content").height(clientheight);	
}


//窗口变化
$(window).resize(function () {
    AutoHeight();
});

//窗口加载
$(function () {
    AutoHeight();
    chklogin();
	
	$('#menu_left').delegate('.ecshop_menutitle a','click',function(){
		$('#tab-title').show();
		$("#menu_left a").removeClass("curent");
        $(this).addClass("curent");
		var new_tab = $(this).clone();
		
		$('#tab-title li').removeClass('current-tab');
		var bool_test = true;
		var test_txt = $(this).text();
		
		$('#tab-title li').each(function(){
			if($(this).text() == test_txt){
				$(this).addClass('current-tab');
				bool_test = false;
			}
		});
		if(bool_test){
			$('#tab-title').append('<li><span></span></li>');
			$('#tab-title li:last-child').append(new_tab).addClass('current-tab');
		}
	});
	
	$('.ecshop_menu a').click(function(){
		$('#tab-title').show();
	});
	
	$('#tab-title').delegate('li span','click',function(){
		$(this).parent('li').remove();
	})
	
	$('#tab-title').delegate('li','click',function(){
		$(this).addClass('current-tab').siblings().removeClass('current-tab');;
	})

    var urlStr = (window.location.href).split('?');
    var editurl = urlStr[1];
    if (editurl != null) {
        if (editurl == "product/EditProduct.aspx") {
            $(".ecshop_menu").find("a").eq(2).addClass("curent ecshop_curent");
            $(".ecshop_menu").find("a").eq(0).removeClass("ecshop_curent");
        }
        else if (editurl == "sales/OrderDetails.aspx") {
            $(".ecshop_menu").find("a").eq(1).addClass("curent ecshop_curent");
            $(".ecshop_menu").find("a").eq(0).removeClass("ecshop_curent");
        }
        else {
            if ($.cookie("guide") == null || $.cookie("guide") == "undefined" || $.cookie("guide") != 1) {
                DialogFrame('help/index.html', '新手向导', 750, null);
            }
        }
    }

    LoadTopLink();
});

function chklogin() {
    $.ajax({
        url: 'LoginUser.ashx?action=chklogin',
        dataType: 'json',
        type: 'GET',
        timeout: 5000,
        error: function (xm, msg) {
            document.location.href = "/admin/login.aspx";
        },
        success: function (result) {
            if (result.status == "false")
                document.location.href = "/admin/login.aspx";
        }
    });
}

function LoadTopLink() {
    $.ajax({
        url: 'LoginUser.ashx?action=login',
        dataType: 'json',
        type: 'GET',
        timeout: 5000,
        error: function (xm, msg) {
            //alert(msg);
        },
        success: function (siteinfo) {
            $(".ecshop_banneritem a:eq(0)").text(siteinfo.sitename);
            $(".ecshop_banneritem a:eq(1)").text(siteinfo.username);
            $(".ecshop_banneritem a:eq(3)").attr("href", siteinfo.taobaourl);
            $(document).attr("title", siteinfo.sitename);
        }
    });
}
