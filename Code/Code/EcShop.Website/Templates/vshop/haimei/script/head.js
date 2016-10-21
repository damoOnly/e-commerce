var deSite ;
var deSiteId ;
var IsOvertime=false;
var flag=true;
var latitude;// 纬度，浮点数，范围为90 ~ -90
var longitude;// 经度，浮点数，范围为180 ~ -180。
var IsLoadCoordinate=false;
var IsTry=false;
 $(document).ready(function ()
 {
	var curSite = setMyCookie("haimei_site");
	if (curSite) {
		curSite = JSON.parse(curSite);
		deSiteId=curSite.id;
		deSite=curSite.name;
		$("#Hidd_SiteId").val(deSiteId);
		$("#site").html(deSite);
		var mydate = new Date();
		var hour=mydate.getHours();
		hour=mydate.getMinutes();//测试
		if((hour-curSite.hour)>30||hour<curSite.hour)//超过一小时再请求服务器
		{
			IsOvertime=true;
			LoadCoordinate();
		}

	}else{
		LoadCoordinate();
	}
});
function LoadCoordinate()//获取经纬度
{
	IsTry=true;
 	wx.ready(function(){
		wx.getLocation({
		success: function (res) {
			latitude = res.latitude; 
			longitude = res.longitude; 
			var speed = res.speed; // 速度，以米/每秒计
			var accuracy = res.accuracy; // 位置精度	
			IsLoadCoordinate=true;
			GetSiteInfo(latitude,longitude);
		},
		cancel: function () { 
			GetSiteInfo();
		}
		});
	});

}
function GetSiteInfo(latitude,longitude)
{
	//latitude='114.10797';
	//longitude='22.541895';
	var msg;
	if(!IsLoadCoordinate&&IsTry){
		GetSiteInfoByIP();
	}
	else{
		GetSiteInfoByCoordinate();
	}
}
function AssignmentSiteInfo(msg)//赋值
{
	if(IsOvertime){
	if(deSiteId!=msg.DefaultSiteId)
	{
		flag = window.confirm("检测到您当前的城市是"+msg.DefaultSite+",是否切换");
	}
	}
	if(flag){
		deSite = msg.DefaultSite;
		deSiteId = msg.DefaultSiteId;
	}
	//alert(deSite);
	$("#Hidd_SiteId").val(deSite);
	$("#site").html(deSite);
	var mydate = new Date();
	var hour=mydate.getHours();
	var minutes=mydate.getMinutes();//测试
	var siteObj = {id:deSiteId,name:deSite,hour:minutes};
	setMyCookie("haimei_site", siteObj); 
}
function GetSiteInfoByCoordinate()//经纬度定位
{
	$.ajax({
	url: "/Api/VshopProcess.ashx",
	data:{action:"GetSiteInfo",latitude:latitude,longitude:longitude},
	async: false,
	type: "post",
	dataType: "json",
	success: function (msg) 
	{
		AssignmentSiteInfo(msg);
	}
	});
}
function GetSiteInfoByIP()//根据IP定位
{
	$.ajax({
	url: "/Handler/DesigSites.ashx",
	async: false,
	type: "post",
	dataType: "json",
	success: function (msg) 
	{
		AssignmentSiteInfo(msg);
	}
	});
}