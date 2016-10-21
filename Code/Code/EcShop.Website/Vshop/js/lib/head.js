 var ProductFloorJson = null;
 var deSite ;
 var deSiteId ;
 $(document).ready(function ()
 {
  	$.ajax({
		url: "/Api/VshopDesignHandler.ashx",
		async: false,
		type: "post",
		data:{action: "InitJSApi"},
		dataType: "json",
		success:function(dataResult)
		{
			wx.config({
				debug: dataResult.debug, 
				appId: dataResult.appId, 
				timestamp:dataResult.timestamp,
				nonceStr: dataResult.nonceStr,
				signature: dataResult.signature,
				 jsApiList: dataResult.jsApiList
			});
		}
	});
	var siteObj = setMyCookie("DataCache_SitesHtml");
	if (siteObj)
	{
		$("#drops").html(siteObj);
		var curSite = setMyCookie("haimei_site");
		if (curSite) {
			curSite = JSON.parse(curSite);
			$("#Hidd_SiteId").val(curSite.id);
			$("#site").html(curSite.name);
		}
		return;
	}
	$.ajax({
		url: "/Handler/DesigSites.ashx",
		async: false,
		type: "post",
		dataType: "json",
		success: function (msg) 
		{
			if (msg.success)
			{
				ProductFloorJson = msg.Result;
				deSite = msg.Sites;//站点名称
				deSiteId = msg.SitesId; //站点ID
				$("#Hidd_SiteId").val(msg.SitesId);
				$("#site").html(msg.Sites);
				siteObj = {id:deSiteId,name:deSite};
				setMyCookie("haimei_site", siteObj);       
				DataBindValue();
			}
		}
	});
});
//绑定值
function DataBindValue()
{
	var SiteHtml = "<ul id='UL_SitesId'>";
	$("#slscategory").empty();
	var siteObj = setMyCookie("haimei_site");
	$.each(ProductFloorJson, function (idx, item)
	{
		SiteHtml += "<li id=" + item.SitesId + " onclick='ChangeSite(\"" + item.SitesId + "\",\"" + item.SitesName + "\")'>" + item.SitesName + "</li>";
	})
	SiteHtml += "</ul>";
	document.getElementById("drops").innerHTML = SiteHtml;
	setMyCookie("DataCache_SitesHtml", SiteHtml);    
}
//文本切换
function ChangeSite(Id,name)
{
	$("#site").html(name);
	$("#Hidd_SiteId").val(Id);
	$("#drops").hide();
	var siteObj = {id:Id,name:name};
	setMyCookie("haimei_site", siteObj);       
}