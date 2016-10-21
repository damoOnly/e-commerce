using System;
using System.Collections.Generic;
using System.Text;

namespace Ecdev.Plugins.Integration.Xinge
{
    public class XingeConfig
    {
        public const string RESTAPI_PUSHSINGLEDEVICE = "http://openapi.xg.qq.com/v2/push/single_device";
	    public const string RESTAPI_PUSHSINGLEACCOUNT = "http://openapi.xg.qq.com/v2/push/single_account";
	    public const string RESTAPI_PUSHACCOUNTLIST = "http://openapi.xg.qq.com/v2/push/account_list";
	    public const string RESTAPI_PUSHALLDEVICE = "http://openapi.xg.qq.com/v2/push/all_device";
	    public const string RESTAPI_PUSHTAGS = "http://openapi.xg.qq.com/v2/push/tags_device";
	    public const string RESTAPI_QUERYPUSHSTATUS = "http://openapi.xg.qq.com/v2/push/get_msg_status";
	    public const string RESTAPI_QUERYDEVICECOUNT = "http://openapi.xg.qq.com/v2/application/get_app_device_num";
	    public const string RESTAPI_QUERYTAGS = "http://openapi.xg.qq.com/v2/tags/query_app_tags";
	    public const string RESTAPI_CANCELTIMINGPUSH = "http://openapi.xg.qq.com/v2/push/cancel_timing_task";
	    public const string RESTAPI_BATCHSETTAG = "http://openapi.xg.qq.com/v2/tags/batch_set";
	    public const string RESTAPI_BATCHDELTAG = "http://openapi.xg.qq.com/v2/tags/batch_del";
	    public const string RESTAPI_QUERYTOKENTAGS = "http://openapi.xg.qq.com/v2/tags/query_token_tags";
	    public const string RESTAPI_QUERYTAGTOKENNUM = "http://openapi.xg.qq.com/v2/tags/query_tag_token_num";
	    public const string RESTAPI_CREATEMULTIPUSH = "http://openapi.xg.qq.com/v2/push/create_multipush";
	    public const string RESTAPI_PUSHACCOUNTLISTMULTIPLE = "http://openapi.xg.qq.com/v2/push/account_list_multiple";
	    public const string RESTAPI_PUSHDEVICELISTMULTIPLE = "http://openapi.xg.qq.com/v2/push/device_list_multiple";
	    public const string RESTAPI_QUERYINFOOFTOKEN = "http://openapi.xg.qq.com/v2/application/get_app_token_info";
	    public const string RESTAPI_QUERYTOKENSOFACCOUNT = "http://openapi.xg.qq.com/v2/application/get_app_account_tokens";

	    public const string HTTP_POST = "POST";
	    public const string HTTP_GET = "GET";
	
	    public const int DEVICE_ALL = 0;
	    public const int DEVICE_BROWSER = 1;
	    public const int DEVICE_PC = 2;
	    public const int DEVICE_ANDROID = 3;
	    public const int DEVICE_IOS = 4;
	    public const int DEVICE_WINPHONE = 5;
	
	    public const int IOSENV_PROD = 1;
	    public const int IOSENV_DEV = 2;
	
	    public const long IOS_MIN_ID = 2200000000L;

        public const int TYPE_NOTIFICATION  = 1;
	    public const int TYPE_MESSAGE = 2;

    }
}
