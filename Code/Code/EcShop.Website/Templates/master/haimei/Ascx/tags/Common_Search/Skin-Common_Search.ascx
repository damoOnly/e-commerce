<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.Common.Controls" Assembly="EcShop.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="EcShop.UI.SaleSystem.Tags" Assembly="EcShop.UI.SaleSystem.Tags" %>
<%@ Import Namespace="EcShop.Core" %>		    
<input type="text" id="txt_Search_Keywords" class="search_input" />
<input type="button" value="" onclick="searchs()" class="search_btn" />
<script type="text/javascript">
    function searchs() {
        var item = $("#drop_Search_Class").val();
        var key = $("#txt_Search_Keywords").val();
        if (key == undefined)
            key = "";

        key = key.replace(/&/g, '&amp;').replace(/"/g, '&quot;').replace(/'/g, '&#39;').replace(/</g, '&lt;').replace(/>/g, '&gt;');

        var url = applicationPath + "/SubCategory.aspx?keywords=" + encodeURIComponent(key) + "&categoryId=" + getParam("categoryId") + "&brand=" + getParam("brand") + "&importsourceid=" + getParam("importsourceid") + "&minSalePrice=" + getParam("minSalePrice") + "&maxSalePrice=" + getParam("maxSalePrice") + "&pp=" + getParams("pp") + "&fl=" + getParams("fl") + "&cd=" + getParams("cd") + "&jg=" + getParams("jg");
        //if (item != undefined)
            //url +=  "&categoryId=" + item;
        window.location.href = url;
    }
    function getParam(paramName) {
        var reg = new RegExp("(^|&)" + paramName + "=([^&]*)(&|$)");
        var r = window.location.search.substr(1).match(reg);
        if (r != null) {
            return unescape(r[2]);
        }
        return '';
    }
    function getParams(paramName) {
        paramValue = "";
        isFound = false;
        paramName = paramName.toLowerCase();
        if (this.location.search.indexOf("?") == 0 && this.location.search.indexOf("=") > 1) {
            if (paramName == "returnurl") {
                var retIndex = this.location.search.toLowerCase().indexOf('returnurl=');
                if (retIndex > -1) {
                    var returnUrl = unescape(this.location.search.substring(retIndex + 10, this.location.search.length));
                    if ((returnUrl.indexOf("http") != 0) && returnUrl != "" && returnUrl.indexOf(location.host.toLowerCase()) == 0) returnUrl = "http://" + returnUrl;
                    return returnUrl;
                }
            }
            else {
                arrSource = this.location.search.substring(1, this.location.search.length).split("&");
            }
            i = 0;
            while (i < arrSource.length && !isFound) {
                if (arrSource[i].indexOf("=") > 0) {
                    if (arrSource[i].split("=")[0].toLowerCase() == paramName.toLowerCase()) {
                        paramValue = arrSource[i].toLowerCase().split(paramName + "=")[1];
                        paramValue = arrSource[i].substr(paramName.length + 1, paramValue.length);
                        isFound = true;
                    }
                }
                i++;
            }
        }
        return paramValue;
    }
    $(document).ready(function () {
        $('#txt_Search_Keywords').keydown(function (e) {
            if (e.keyCode == 13) {
                searchs();
                return false;
            }
        })       
    });
	  
  </script>