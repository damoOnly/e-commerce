//var hiddentiem = $("<input type=\"hidden\" id=\"hdstarttime\"/>");
//$(hiddentiem).appendTo("body");
//倒数时间详细页面
function showTimeList(overdata, $ele, isStart, startdata, nowTime) {
    if (!nowTime)
        nowTime = new Date();
    var arr = overdata.split(" ");
    var arr1 = arr[0].split("-");
    var arr2 = arr[1].split(":");
    var endData = new Date(arr1[0], (Number(arr1[1]) - 1), arr1[2], arr2[0], arr2[1], arr2[2]);
    arr = startdata.split(" ");
    arr1 = arr[0].split("-");
    arr2 = arr[1].split(":");
    var StartData = new Date(arr1[0], (Number(arr1[1]) - 1), arr1[2], arr2[0], arr2[1], arr2[2]);

    ///离开始的秒数
    var startTotal = (StartData - nowTime) / 1000;


    // 剩余总秒
    var total = (endData - nowTime) / 1000;
    if (parseInt(total) <= 0) {
        $ele.text("活动已结束.");
        clearInterval(showTime);
		var tempa = $ele.parents(".act-box").find(".buy-now");
        tempa.remove();
        return;
    }

    //还没开始 ，倒计时离开始还有多久
    if (parseInt(startTotal) >= 0) {
        // 计算时间
        var day = parseInt(startTotal / 86400);
        //var hour = parseInt(total / 3600);
        var hour = parseInt(startTotal % 86400 / 3600);
        var min = parseInt((startTotal % 3600) / 60);
        var sec = parseInt((startTotal % 3600) % 60);

        if (hour.toString().length == 1)
            hour = "0" + hour;
        if (min.toString().length == 1)
            min = "0" + min;
        if (sec.toString().length == 1)
            sec = "0" + sec;
        var showTime = "<span>" + day + "</span>天<span>" + hour + "</span>时<span>" + min + "</span>分<span>" + sec + "</span>秒";
        $ele.html("距离：" + showTime);
        var tempa = $ele.parents(".act-box").find(".buy-now");
        tempa.removeAttr('href');
        tempa.addClass("disabled").html("即将开始");
    }
    else {
        // 计算时间
        var day = parseInt(total / 86400);
        //var hour = parseInt(total / 3600);
        var hour = parseInt(total % 86400 / 3600);
        var min = parseInt((total % 3600) / 60);
        var sec = parseInt((total % 3600) % 60);

        if (hour.toString().length == 1)
            hour = "0" + hour;
        if (min.toString().length == 1)
            min = "0" + min;
        if (sec.toString().length == 1)
            sec = "0" + sec;
        var showTime = "<span>" + day + "</span>天<span>" + hour + "</span>时<span>" + min + "</span>分<span>" + sec + "</span>秒";

        // 显示
        if (isStart == "1") {
            $ele.html("还剩：" + showTime);
            var tempa = $ele.parents(".act-box").find(".buy-now");
            tempa.html("立即抢购");
        }
        else
            $ele.html("距开始");
    }
}