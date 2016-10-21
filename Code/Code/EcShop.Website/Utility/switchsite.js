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
            var expires = 0;
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
//收藏商城点击事件
function favPage() {
    _adwq.push([ 
    '_setAction','8ke8d7', 
    $("#hiid_AdUserId").val(), //请填入用户id 
    '' //请填入商品id，选填
    ]); 
    var a = window.location.href, b = document.title;
    document.all ? window.external.AddFavorite(a, b) : window.sidebar && window.sidebar.addPanel ? window.sidebar.addPanel(b, a, "") : alert("\u5bf9\u4e0d\u8d77\uff0c\u60a8\u7684\u6d4f\u89c8\u5668\u4e0d\u652f\u6301\u6b64\u64cd\u4f5c!\n\u8bf7\u60a8\u4f7f\u7528\u83dc\u5355\u680f\u6216Ctrl+D\u6536\u85cf\u672c\u7ad9\u3002"), createCookie("_fv", "1", 30, window.location.href)
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
    setMyCookie("haimeipc_site", siteObj);
}

function GetSitesList() {
    var SiteHtml = "<ul id='UL_SitesId'>";
    $("#slscategory").empty();
    $.ajax({
        url: "/Api/VshopProcess.ashx",
        data: { action: "GetSitesList" },
        async: false,
        type: "post",
        dataType: "json",
        success: function (msg) {
            $.each(msg, function (idx, item) {
                SiteHtml += "<li id=" + item.SitesId + " onclick='ChangeSite(\"" + item.SitesId + "\",\"" + item.SitesName + "\")'>" + item.SitesName + "</li>";
            })
            SiteHtml += "</ul>";
            $('#drops').html(SiteHtml);
        }
     }
	);
}

$(function(){
	function getSites(){
		$.ajax({
			url: "/Handler/DesigSites.ashx",
			async: false,
			type: "post",
			dataType: "json",
			success: function (msg) {
				if (msg.success) {
					ProductFloorJson = msg.Result;
					deSite = msg.Sites;//站点名称
					deSiteId = msg.SitesId; //站点ID
					$("#Hidd_SiteId").val(msg.SitesId);
					$("#site").html(msg.Sites);
					siteObj = { id: deSiteId, name: deSite };
					setMyCookie("haimeipc_site", siteObj);					
				}
			}
		});
	}
	var ProductFloorJson = null;
	var deSite;
	var deSiteId;
	var siteObj = setMyCookie("haimeipc_site");
	if (siteObj) {		
		var curSite = JSON.parse(siteObj);
		if (curSite) {			
			$("#Hidd_SiteId").val(curSite.id);
			$("#site").html(curSite.name);
		}
	}else{
		getSites();
	}
    $("#site").click(function (e) {
        if ($(this).attr('class') == 'up') {
            $(this).removeClass('up');
            $("#drops").hide();
        } else {
            $("#drops").show();
            GetSitesList();
            $(this).addClass("up");
        }
        e.stopPropagation();
    });
	//收藏
	$("#shoucang").click(function (e) {
		favPage();
	})
});