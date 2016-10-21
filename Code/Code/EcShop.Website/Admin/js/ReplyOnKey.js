$(document).ready(function () {
	ShowKey(); 
	$("#chkKeys").bind("click", function () {
		if ($("#chkKefu:checked").length > 0){
			$("#chkKefu").removeAttr("checked");
		}
		ShowKey() 
	}); 
	$("#chkKefu").bind("click",function(){
		if ($("#chkKeys:checked").length > 0){
			$("#chkKeys").removeAttr("checked");
		}
		ShowKey();	
	});
 });
function ShowKey() {
	var  $keys=$("#chkKeys:checked");
	var $kefu=$("#chkKefu:checked");
    if ($keys.length > 0||$kefu.length>0) {
        $(".likey").show();
		if($kefu.length>0){
			$("#liContent").hide();
		}else{
			$("#liContent").show();
		}
    }
    else {
        $(".likey").hide();
		if($kefu.length<=0){
			$("#liContent").show();
		}
    }
}
function CheckKey() {
    if ($("#chkKeys:checked").length > 0) {
        if ($("#txtKeys").val() == "") {
            alert("你选择了关键字回复，请填写关键字！");
            return false;
        }
    }
	    if ($("#chkKefu:checked").length > 0) {
        if ($("#chkKefu").val() == "") {
            alert("你选择了客服回复，请填写关键字！");
            return false;
        }
    }
	if($("#chkKefu:checked").length <=0){
		var content=$("#fcContent");
		if (content.length>0&&content.val().length == 0) {
			alert("回复内容不能为空！");
			return false;
		}
	}
    return true;
}