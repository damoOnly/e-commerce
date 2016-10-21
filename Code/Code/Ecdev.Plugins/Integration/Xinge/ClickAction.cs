using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Ecdev.Plugins.Integration.Xinge
{
    public enum ClickActionType
    {
        TYPE_ACTIVITY = 1,
	    TYPE_URL = 2,
	    TYPE_INTENT = 3
    }

    public class ClickAction
    {
        public string ToJsonString()
        {
            JObject json = new JObject();
            
            json.Add("action_type", (int)ActionType);

            JObject browser = new JObject();
            browser.Add("url", Url);
            browser.Add("confirm", ConfirmOnUrl);

            json.Add("browser", browser);
            json.Add("activity", Activity);
            json.Add("intent", Intent);

            JObject aty_attr = new JObject();
            aty_attr.Add("if", AtyAttrIntentFlag);
            aty_attr.Add("pf", AtyAttrPendingIntentFlag);

            json.Add("aty_attr", aty_attr);

            return json.ToString();
        }

        public JObject ToJsonObject()
        {
            JObject json = new JObject();

            json.Add("action_type", (int)ActionType);

            JObject browser = new JObject();
            browser.Add("url", Url);
            browser.Add("confirm", ConfirmOnUrl);

            json.Add("browser", browser);
            json.Add("activity", Activity);
            json.Add("intent", Intent);

            JObject aty_attr = new JObject();
            aty_attr.Add("if", AtyAttrIntentFlag);
            aty_attr.Add("pf", AtyAttrPendingIntentFlag);
            json.Add("aty_attr", aty_attr);

            return json;
        }

        public bool IsValid()
        {
            if (ActionType < ClickActionType.TYPE_ACTIVITY || ActionType > ClickActionType.TYPE_INTENT) return false;

            if (ActionType == ClickActionType.TYPE_URL)
            {
                if (string.IsNullOrEmpty(Url) || ConfirmOnUrl < 0 || ConfirmOnUrl > 1) return false;
                return true;
            }
            if (ActionType == ClickActionType.TYPE_INTENT)
            {
                if (string.IsNullOrEmpty(Intent)) return false;
                return true;
            }
            return true;
        }

        public ClickAction()
        {
            Url = "";
            ActionType = ClickActionType.TYPE_ACTIVITY;
            Activity = "";

            AtyAttrIntentFlag = 0;
            AtyAttrPendingIntentFlag = 0;

            PackageDownloadUrl = "";
            ConfirmOnPackageDownloadUrl = 1;
            PackageName = "";
        }

        public ClickActionType ActionType { get; set; }
        public string Url { get; set; }
        public int ConfirmOnUrl { get; set; }
        public string Activity { get; set; }
        public string Intent { get; set; }
        public int AtyAttrIntentFlag { get; set; }
        public int AtyAttrPendingIntentFlag { get; set; }
        public string PackageDownloadUrl { get; set; }
        public int ConfirmOnPackageDownloadUrl { get; set; }
        public string PackageName { get; set; }
    }


}
