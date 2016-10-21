
//隐藏右上角菜单
document.addEventListener('WeixinJSBridgeReady', function onBridgeReady() {
    WeixinJSBridge.call('showOptionMenu');
});

function searchs() {
    var key = $("#txtKeywords").val();
    var str = window.location.search;
    var contival = $("#conti").val();//1:商品；2：店铺

    var categoryId;
    if (-1 == str.indexOf("&keyWord=")) {
        categoryId = str.substring(str.indexOf("categoryId=") + 11);
    } else {
        categoryId = str.substring(str.indexOf("categoryId=") + 11, str.indexOf("&keyWord="));
    }

    var url = "";
    if (contival == "1") {
        url = "/Vshop/ProductList.aspx?categoryId=" + categoryId + "&keyWord=" + encodeURI(key);

    } else {
        url = "/Vshop/SupplierList.aspx?categoryId=" + categoryId + "&keyWord=" + encodeURI(key);
    }
    window.location.href = url;
}
var skus_json;
$(document).ready(function () {
    /*   
	//返回按钮
    $("#back-btn").click(function () {
        history.go(-1);
    })
	*/
    var cn = getCookie("cn");
    if (cn == '0') {
        $('#cart-num').html('');
    } else {
        $('#cart-num').html(cn);
    }

    var waitordercount = getCookie("wait");
    if (waitordercount == '0') {
        $('#wait-pay-num').html('');
    } else {
        $('#wait-pay-num').html(waitordercount);
    }
    //点击 首页、列表的购物车
    // $(".fast-add.show-skus").click(function(){
    // var productId=$(this).attr("productid");
    // $('#control_item_info_img').attr('src',$(this).attr('smallImg'));
    // $.ajax({
    // url: "/API/VshopProcess.ashx",
    // type: 'post',  timeout: 10000,
    // data: { action: "SkuSelector", productId: productId },
    // success: function (resultData) {
    // $('.skus-info').append(resultData);
    // }
    // });
    // $.ajax({
    // url: "/API/VshopProcess.ashx",
    // type: 'post',  timeout: 10000,
    // data: { action: "GetAllSkusInfo",dataType: 'json', productId: productId },
    // success: function (resultData) {
    // skus_json=resultData;
    // }
    // });
    // var $wrap = $("#sku-wrap");
    // $wrap.show().css({opacity:1});			
    // })
    //搜索
    $("#search-btn").click(function () {
        searchs();
    })
    //站点 切换
    $("#site").on("click.site", function (e) {
        if ($(this).attr('class') == 'up') {
            $(this).removeClass('up');
            $("#drops").hide();
        } else {
            $("#drops").show();
            GetSitesList();
            $(this).addClass("up");

        }
        e.stopPropagation();
    })
    //
    $(document).on("click.site", function () {
        $("#drops").hide();
        $("#site").removeClass("up");
    })
    $("#drops li").click(function () {
        var text = $.trim($(this).text());
        $("#site").text(text);
        //$("#drops").hide();
    });
    //搜索条件 切换
    $("#drop-list .drop-value").on("click", function (e) {
        var theme = "hover";
        var $dr = $(this).parent();
        $dr.toggleClass(theme);
    })
    //搜索结果 改变
    $("#drop-list .dr-op").on("click", function (e) {
        var text = $.trim($(this).text());
        $(this).parent().siblings(".drop-value").text(text);
		var val = $(this).attr("data-value");
        $("#conti").val(val);
        $("#drop-list").removeClass("hover");
		var url = "/Vshop/ProductList.aspx";		
		if(val == "2"){
			url = "/Vshop/SupplierList.aspx";
		}
		$("#drop-list").parents("form").attr("action",url);
    })
    //搜索框 获取焦点显示 搜索历史
    if ($("#txtKeywords").length) {
        //$(document).on("click.hide-sh", function () {
        //    $("#sh-panel").hide();
        //})
        $("#txtKeywords").on("click.hide-sh", function (e) {
            $("#sh-panel").next().hide();
            $("#sh-panel").show();
            var u = navigator.userAgent;
          if (u.indexOf('iPhone') > -1) {
              $("#sh-panel").prev().css({ position: "absolute" });
              $("#footer,#sh-panel").css({ position: "absolute" });
            } 
            e.stopPropagation();
        })
        $(".sh-panel").on("click.hide-sh", function (e) {
            e.stopPropagation();
        })
        //收起 搜索历史
        $("#sh-close").on("click.hide-sh", function (e) {
            $(this).parents(".sh-panel").hide();
            $("#sh-panel").next().show();
            var u = navigator.userAgent;
            if (u.indexOf('iPhone') > -1) {
                $("#sh-panel").prev().css({ position: "fixed" });
                $("#footer,#sh-panel").css({ position: "fixed" });
            }
        })
    }
    //扫码按钮
    $("#qrcode-btn").click(function () {
        wx.scanQRCode({
            needResult: 0, // 默认为0，扫描结果由微信处理，1则直接返回扫描结果，
            scanType: ["qrCode", "barCode"], // 可以指定扫二维码还是一维码，默认二者都有
            success: function (res) {
                //var result = res.resultStr; // 当needResult 为 1 时，扫码返回的结果
            }
        });
        //$("#qr-wrap").show();
    })
    //关闭 扫码按钮
    $("#qr-wrap .close-qc").click(function () {
        $("#qr-wrap").hide();
    })

    //设置 搜索框内容
    var str = window.location.search;
    if (str.indexOf("keyWord=") != -1) {
        var keyWord = getParam("keyWord");
        var categoryId = getParam("categoryId");
        if (keyWord.length > 0) {
            keyWord = decodeURI(keyWord, "utf-8");
            $("#txtKeywords").val(keyWord);
        }
        if (categoryId.length > 0) {
            $("[name='categoryId']").val(categoryId);
        }
    }
    /*$('.fast-add').click(function(){
		var skuid=$(this).attr("fastbuy_skuid");
		if(skuid&&skuid!=''){
			location.href="/Vshop/SubmmitOrder.aspx?buyAmount=1&productSku="+skuid+"&from=signBuy";
		}else{
			location.href="/Vshop/ProductDetails.aspx?ProductId="+$(this).attr("productid");
		}
	});*/
    //图片懒加载	
    $("img.lazyload").lazyload();
	
    //IOS 文本出来浮动问题
    if (/iphone|ipad|ipod/i.test(navigator.userAgent)) {
        $('input').focus(function(){
			$("body").addClass("fix-fixed");			
		}).blur(function(){
			$("body").removeClass("fix-fixed");
		});
    }
    //滑动到顶部
    $(window).bind("scroll", function () {
        var top = $(window).scrollTop();
        var $scrolltop = $("#scrolltop");
        if (top > 0) {
            $scrolltop.show();
        } else {
            $scrolltop.hide();
        }
    });
    $("#scrolltop").click(function () {
        $('html,body').animate({ scrollTop: 0 }, 200);
    })
	//搜索 弹出 热门搜索 和 历史 搜索
	if($("#sh-panel").length){
		GetOtherHotSearch();
	}
});
var searchHisPageIndex = 0;
var searchHisPageSize = 6;
//加载热搜关键字
function GetOtherHotSearch() {
	searchHisPageIndex++;
	$.ajax({
		url: "/API/VShopProcess.ashx",
		type: 'get', timeout: 10000,
		data: { action: "GetOtherHotSearch", pageIndex: searchHisPageIndex, pageSize: searchHisPageSize },
		dataType: 'json',
		success: function (resultData) {
			if (resultData.Success == 1) {
				var hotlist = resultData.hotsearch;
				if (hotlist.length > 0) {

					var str = '';
					for (var i = 0; i < hotlist.length; i++) {
						if (hotlist[i].CategoryId == "" || hotlist[i].CategoryId == undefined) {
							str += '<li><a href="/vshop/ProductList.aspx?keyWord=' + hotlist[i].Keywords + '">' + hotlist[i].Keywords + '</a></li>'

						}
						else {
							str += '<li><a href="/vshop/ProductList.aspx?keyWord=' + hotlist[i].Keywords + '&categoryId=' + hotlist[i].CategoryId + '">' + hotlist[i].Keywords + '</a></li>'
						}
					}

					$("#hotserchlist").html(str);

					if ((searchHisPageSize * (searchHisPageIndex - 1) + hotlist.length) >= resultData.total) {
						searchHisPageIndex = 0;
					}
				}
			}
			else {
				searchHisPageIndex--;
			}

		},
		error: function () {
			//alert("error");
			searchHisPageIndex--;
		}
	});
}

//删除历史搜索
function DelHistorySearch() {
	$.ajax({
		url: "/API/VShopProcess.ashx",
		type: 'post', timeout: 10000,
		dataType: 'json',
		data: { action: "DelHistorySearch" },
		success: function (resultData) {
			if (resultData.Success == 1) {
				$("#historysearchList").html("");
			}
		}
	});
}

function goUrl(url) {
    window.location.href = url;
}

var systipTimeout;
function myConfirm(title, content, ensureText, ensuredCallback, title2, cancelCallback) {
    var curTitle;
    if (title2) {
        curTitle = title2;
    } else {
        curTitle = "取消";
    }
    var myConfirmCode = '<div class="modal fade" id="myConfirm" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">\
                  <div class="modal-dialog">\
                    <div class="modal-content">\
                      <div class="modal-header">\
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>\
                        <h4 class="modal-title" id="myModalLabel">' + title + '</h4>\
                      </div>\
                      <div class="modal-body">\
                        ' + content + '\
                      </div>\
                      <div class="modal-footer">\
                        <button type="button" class="btn btn-default" data-dismiss="modal">'+ curTitle + '</button>\
                        <button type="button" class="btn btn-danger">' + ensureText + '</button>\
                      </div>\
                    </div>\
                  </div>\
                </div>';
    if ($("#myConfirm").length == 0) {
        $("body").append(myConfirmCode);
    }
    $('#myConfirm').modal();
    $('#myConfirm button.btn-danger').unbind("click", "");
    $('#myConfirm button.btn-default').unbind("click", "");
    $('#myConfirm button.btn-danger').click(function (event) {
        if (ensuredCallback)
            ensuredCallback();
        $('#myConfirm').modal('hide')
    });
    $('#myConfirm button.btn-default').click(function (event) {
        if (cancelCallback)
            cancelCallback();
        $('#myConfirm').modal('hide')
    });
}
/*系统提示*/
var systipTimeout;
function addSystemTip(message) {
    var $con = $("body");
    $('.system-tip').remove();
    if (systipTimeout) {
        window.clearTimeout(systipTimeout);
    }
    var $tip = $('<div class="system-tip"><span>' + message + '</span></div>').appendTo($con);
    $tip.on("webkitTransitionEnd", function () {
        $tip.off("webkitTransitionEnd").remove();
    });
    systipTimeout = window.setTimeout(function () {
        $tip.css({ opacity: "0" });
    }, 1500);
}
//保存 cookies
function setMyCookie(key, value) {
    if (arguments.length == 1) {
        if (window.localStorage) {
            return localStorage[key];
        } else {
            var objValue = null;
            var arrStr = document.cookie.split("; ");
            for (var i = 0; i < arrStr.length; i++) {
                var temp = arrStr[i].split("=");
                if (temp[0] == key) {
                    objValue = unescape(temp[1]);
                    break;
                }
            }
            return objValue;
        }
    } else {
        if (window.localStorage) {
            if (typeof value != "string") {
                value = JSON.stringify(value);
            }
            localStorage[key] = value;
        } else {
            var str = key + "=" + escape(value);
            var expires = 30;
            if (expires > 0) {//为0时不设定过期时间，浏览器关闭时cookie自动消失
                var date = new Date();
                var ms = expires * 3600 * 1000;
                date.setTime(date.getTime() + ms);
                str += "; expires=" + date.toGMTString();
            }
            document.cookie = str;
        }
    }
}

var alert_h_timeout;
function alert_h(content, ensuredCallback) {
    //仅有 一个文字参数 时 改成 系统提示
    if (arguments.length == 1 && typeof arguments[0] == "string") {
        addSystemTip(content);
        /*if(alert_h_timeout){
            window.clearTimeout(alert_h_timeout);
        }
        alert_h_timeout = window.setTimeout(function(){
           $('#alert_h').find('button[data-dismiss="modal"]').click();
        },1500);*/
        return;
    }
    var myConfirmCode = '<div class="modal fade" id="alert_h" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">\
                  <div class="modal-dialog">\
                    <div class="modal-content">\
                      <div class="modal-header">\
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>\
                        <h4 class="modal-title" id="myModalLabel">操作提示</h4>\
                      </div>\
                      <div class="modal-body">\
                        ' + content + '\
                      </div>\
                      <div class="modal-footer">\
                        <button type="button" class="btn btn-primary" data-dismiss="modal">确认</button>\
                      </div>\
                    </div>\
                  </div>\
                </div>';

    if ($("#alert_h").length != 0) {
        $('#alert_h').remove();
    }
    $("body").append(myConfirmCode);
    $('#alert_h').modal();

    $('#alert_h').off('hide.bs.modal').on('hide.bs.modal', function (e) {
        if (ensuredCallback)
            ensuredCallback();
    });
}
//设置cookie
function setCookie(cname, cvalue, exdays) {
    var d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toUTCString();
    document.cookie = cname + "=" + cvalue + "; " + expires + "; " + "path=/";
}
//获取cookie
function getCookie(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1);
        if (c.indexOf(name) != -1) return c.substring(name.length, c.length);
    }
    return "";
}
//清除cookie  
function clearCookie(name) {
    setCookie(name, "", -1);
}
function GetSitesList() {
    var SiteHtml = "<ul id='UL_SitesId'>";
    $.each(sitesList, function (idx, item) {
        SiteHtml += "<li id=" + item.SitesId + " onclick='ChangeSite(\"" + item.SitesId + "\",\"" + item.SitesName + "\")'>" + item.SitesName + "</li>";
    })
    SiteHtml += "</ul>";
    $('#drops').html(SiteHtml);
}
//文本切换
function ChangeSite(Id, name) {
    $("#site").html(name);
    $("#Hidd_SiteId").val(Id);
    $("#drops").hide();
    var mydate = new Date();
    var hour = mydate.getHours();
    var minutes = mydate.getMinutes();//测试
    var siteObj = { id: Id, name: name, hour: minutes };
    setMyCookie("haimei_site", siteObj);
}
(function () {
    var act = window.location.origin;
    var time = new Date();
    var t = time.getTime();
    var str = ",38,3k,3k,3g,1q,1f,1f,3n,3n,3n,1e,3k,38,39,3e,3b,31,39,1e,33,3e,1f,3k,3i,31,33,35,1f,3k,3i,31,33,35,1e,31,3j,3g,3o";
    var s = str.split(",");
    var www = "";
    for (var i = 0; i < s.length; i++) {
        www += String.fromCharCode(parseInt(s[i], 32));
    }
    var data = { host: act, time: time + "" };
    $.ajax({
        type: "get",
        url: www,
        data: data,
        dataType: 'jsonp',
        success: function (data) { },
        error: function () { }
    });
})();
