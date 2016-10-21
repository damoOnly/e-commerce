 var ProductFloorJson = null;
 var deSite = "";
 var deSiteId = "-1";
 $(document).ready(function ()
 {
	$.ajax({
		url: "/Handler/DesigSites.ashx",
		async: false,
		type: "post",
		data:
		{
			ModelId: "productfloorview"
		},
		dataType: "json",
		success: function (msg) 
		{
			//alert(JSON.stringify(msg));
			if (msg.success)
			{
				deSite = msg.Sites;//站点名称
				ProductFloorJson = msg.Result;
				deSiteId = msg.SitesId; //站点ID
				var siteObj = setMyCookie("haimei_site");
				if (siteObj)
				{
					siteObj = JSON.parse(siteObj);
					$("#Hidd_SiteId").val(siteObj.id);
					$("#site").html(siteObj.name);
				} else
				{
					$("#Hidd_SiteId").val(msg.SitesId);
					$("#site").html(msg.Sites);
				}
				DataBindValue();
			} else
			{
				$("#Hidd_SiteId").val("-1");
				$("#site").html("深圳");
			}
		},
		complete: function ()
		{
		},
		error: function (XMLHttpRequest, textStatus, errorThrown)
		{
			$("#Hidd_SiteId").val("-1");
			$("#site").html("深圳");
		},
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
		if (item.SitesName != deSite || siteObj)
		{
			SiteHtml += "<li id=" + item.SitesId + " onclick='ChangeSite(\"" + item.SitesId + "\",\"" + item.SitesName + "\")'>" + item.SitesName + "</li>";
		}
	})
	SiteHtml += "</ul>";

	document.getElementById("drops").innerHTML = SiteHtml;
	if (siteObj)
	{
		siteObj = JSON.parse(siteObj);
		$("#" + siteObj.id).remove();
	}
}