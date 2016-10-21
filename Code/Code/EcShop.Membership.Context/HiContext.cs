using EcShop.Core;
using EcShop.Core.Configuration;
using EcShop.Core.Enums;
using EcShop.Membership.Core;
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Web;
namespace EcShop.Membership.Context
{
    public sealed class HiContext
    {
        private const string dataKey = "Ecshop_ContextStore";
        private string _siteUrl = null;
        private NameValueCollection _queryString = null;
        private bool _isUrlReWritten = false;
        private HttpContext _httpContext;
        private string _hostPath = null;
        private HiConfiguration _config = null;
        private SiteSettings currentSettings;
        private string rolesCacheKey = null;
        private IUser _currentUser = null;
        private string verifyCodeKey = "VerifyCode";
        public static HiContext Current
        {
            get
            {
                HttpContext current = HttpContext.Current;
                HiContext hiContext = current.Items[dataKey] as HiContext;
                if (hiContext == null)
                {
                    if (current == null)
                    {
                        throw new System.Exception("No HiContext exists in the Current Application. AutoCreate fails since HttpContext.Current is not accessible.");
                    }
                    hiContext = new HiContext(current);
                    HiContext.SaveContextToStore(hiContext);
                }
                return hiContext;
            }
        }
        public bool IsUrlReWritten
        {
            get
            {
                return this._isUrlReWritten;
            }
            set
            {
                this._isUrlReWritten = value;
            }
        }
        public HttpContext Context
        {
            get
            {
                return this._httpContext;
            }
        }
        public string SiteUrl
        {
            get
            {
                return this._siteUrl;
            }
        }
        public string HostPath
        {
            get
            {
                if (this._hostPath == null)
                {
                    Uri url = this.Context.Request.Url;
                    string text = (url.Port == 80) ? string.Empty : (":" + url.Port.ToString(System.Globalization.CultureInfo.InvariantCulture));
                    this._hostPath = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}://{1}{2}", new object[]
					{
						url.Scheme,
						url.Host,
						text
					});
                }
                return this._hostPath;
            }
        }
        public HiConfiguration Config
        {
            get
            {
                if (this._config == null)
                {
                    this._config = HiConfiguration.GetConfig();
                }
                return this._config;
            }
        }
        public ApplicationType ApplicationType
        {
            get
            {
                return this.Config.AppLocation.CurrentApplicationType;
            }
        }
        public SiteSettings SiteSettings
        {
            get
            {
                if (this.currentSettings == null)
                {
                    this.currentSettings = SettingsManager.GetMasterSettings(true);
                }
                return this.currentSettings;
            }
        }
        public string RolesCacheKey
        {
            get
            {
                return this.rolesCacheKey;
            }
            set
            {
                this.rolesCacheKey = value;
            }
        }
        public IUser User
        {
            get
            {
                if (this._currentUser == null)
                {
                    this._currentUser = Users.GetUser();
                }
                return this._currentUser;
            }
            set
            {
                this._currentUser = value;
            }
        }

        public int ReferralUserId
        {
            get
            {
                int result;
                if (string.Compare(Globals.DomainName, HiContext.Current.SiteSettings.SiteUrl, true, System.Globalization.CultureInfo.InvariantCulture) != 0)
                {
                    result = 0;
                }
                else
                {
                    HttpCookie httpCookie = HttpContext.Current.Request.Cookies["Site_ReferralUser"];
                    if (httpCookie == null || string.IsNullOrEmpty(httpCookie.Value))
                    {
                        result = 0;
                    }
                    else
                    {
                        int num = 0;
                        int.TryParse(httpCookie.Value, out num);
                        result = num;
                    }
                }
                return result;
            }
        }
        private HiContext(HttpContext context)
        {
            this._httpContext = context;
            this.Initialize(new NameValueCollection(context.Request.QueryString), context.Request.Url, context.Request.RawUrl, this.GetSiteUrl());
        }
        public static HiContext Create(HttpContext context)
        {
            return HiContext.Create(context, false);
        }
        public static HiContext Create(HttpContext context, bool isReWritten)
        {
            HiContext hiContext = new HiContext(context);
            hiContext.IsUrlReWritten = isReWritten;
            HiContext.SaveContextToStore(hiContext);
            return hiContext;
        }
        public static HiContext Create(HttpContext context, UrlReWriterDelegate rewriter)
        {
            HiContext hiContext = new HiContext(context);
            HiContext.SaveContextToStore(hiContext);
            if (null != rewriter)
            {
                hiContext.IsUrlReWritten = rewriter(context);
            }
            return hiContext;
        }
        public string GetSkinPath()
        {
            return (Globals.ApplicationPath + "/Templates/master/" + this.SiteSettings.Theme).ToLower(System.Globalization.CultureInfo.InvariantCulture);
        }
        public string GetStoragePath()
        {
            return "/Storage/master";
        }
        public string GetVshopSkinPath(string themeName = null)
        {
            themeName = ((themeName == null) ? this.SiteSettings.VTheme : themeName);
            return (Globals.ApplicationPath + "/templates/vshop/" + themeName).ToLower(System.Globalization.CultureInfo.InvariantCulture);
        }
        public string GetWapshopSkinPath(string themeName = null)
        {
            themeName = ((themeName == null) ? this.SiteSettings.WapTheme : themeName);
            return (Globals.ApplicationPath + "/templates/wapshop/" + themeName).ToLower(System.Globalization.CultureInfo.InvariantCulture);
        }
        public string GetAppshopSkinPath()
        {
            return Globals.ApplicationPath + "/templates/appshop";
        }
        public string GetAliOHshopSkinPath(string themeName = null)
        {
            themeName = ((themeName == null) ? this.SiteSettings.AliOHTheme : themeName);
            return (Globals.ApplicationPath + "/templates/AliOHshop/" + themeName).ToLower(System.Globalization.CultureInfo.InvariantCulture);
        }
        public string CreateVerifyCode(int length)
        {
            string text = string.Empty;
            System.Random random = new System.Random();
            while (text.Length < length)
            {
                int num = random.Next();
                char c;
                if (num % 3 == 0)
                {
                    c = (char)(97 + (ushort)(num % 26));
                }
                else
                {
                    if (num % 4 == 0)
                    {
                        c = (char)(65 + (ushort)(num % 26));
                    }
                    else
                    {
                        c = (char)(48 + (ushort)(num % 10));
                    }
                }
                if (c != '0' && c != 'o' && c != '1' && c != '7' && c != 'l' && c != '9' && c != 'g' && c != 'I')
                {
                    text += c.ToString();
                }
            }
            this.RemoveVerifyCookie();
            HttpCookie httpCookie = new HttpCookie(this.verifyCodeKey);
            httpCookie.Value = HiCryptographer.Encrypt(text);
            HttpContext.Current.Response.Cookies.Add(httpCookie);
            return text;
        }
        public bool CheckVerifyCode(string verifyCode)
        {
            bool result;
            if (HttpContext.Current.Request.Cookies[this.verifyCodeKey] == null)
            {
                this.RemoveVerifyCookie();
                result = false;
            }
            else
            {
                bool flag = string.Compare(HiCryptographer.Decrypt(HttpContext.Current.Request.Cookies[this.verifyCodeKey].Value), verifyCode, true, System.Globalization.CultureInfo.InvariantCulture) == 0;
                this.RemoveVerifyCookie();
                result = flag;
            }
            return result;
        }
        private void RemoveVerifyCookie()
        {
            HttpContext.Current.Response.Cookies[this.verifyCodeKey].Value = null;
            HttpContext.Current.Response.Cookies[this.verifyCodeKey].Expires = new System.DateTime(1911, 10, 12);
        }
        private string GetSiteUrl()
        {
            return this._httpContext.Request.Url.Host;
        }
        private void Initialize(NameValueCollection qs, Uri uri, string rawUrl, string siteUrl)
        {
            this._queryString = qs;
            this._siteUrl = siteUrl.ToLower();
            if (this._queryString != null && this._queryString.Count > 0 && !string.IsNullOrEmpty(this._queryString["ReferralUserId"]))
            {
                HttpCookie httpCookie = HttpContext.Current.Request.Cookies["Site_ReferralUser"];
                if (httpCookie == null)
                {
                    httpCookie = new HttpCookie("Site_ReferralUser");
                }
                httpCookie.Value = this._queryString["ReferralUserId"];
                HttpContext.Current.Response.Cookies.Add(httpCookie);
            }
        }
        private static void SaveContextToStore(HiContext context)
        {
            context.Context.Items[dataKey] = context;
        }

        public string GenerateRandomNumber(int Length)
        {
            char[] constant =   { '0','1','2','3','4','5','6','7','8','9'};
            System.Text.StringBuilder newRandom = new System.Text.StringBuilder(10);
            Random rd = new Random();
            for (int i = 0; i < Length; i++)
            {
                newRandom.Append(constant[rd.Next(10)]);
            }
            return newRandom.ToString();
        }
    }
}
