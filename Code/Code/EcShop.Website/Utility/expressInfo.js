(function ($) {

    function getExpressData(orderId) {
        var url = '/API/VshopProcess.ashx';
        var expressData;
        $.ajax({
            type: "get",
            url: url,
            data: { action: 'Logistic', orderId: orderId },
            dataType: "json",
            async: false,
            success: function (data) {
                expressData = data;
            }
        });
        return expressData;
    }



    $.fn.expressInfo = function (orderId) {

        var expressData = getExpressData(orderId);

        var html = '<table>';
        if (expressData != null && expressData != "") {
            $(expressData.reverse()).each(function (index, day) {
                html += '<tr><td>' + day.time + '</td><td>' + day.context + '</td></tr>';
            });
        } else {
            html += '<tr><td>没有物流信息！</td></tr>';
        }

        html += '</table>';
        $(this).html(html);
        return $;

    }




})(jQuery);