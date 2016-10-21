using System;
using System.Web;
namespace EcShop.Entities.VShop
{
    public class TplCfgInfo
    {
        public int Id
        {
            get
            {
                return this.BannerId;
            }
            set
            {
                this.Id = this.BannerId;
            }
        }

        public int? SupplierId { get; set; }
        public int BannerId
        {
            get;
            set;
        }
        public string ImageUrl
        {
            get;
            set;
        }
        public string ShortDesc
        {
            get;
            set;
        }
        public int DisplaySequence
        {
            get;
            set;
        }
        public int Type
        {
            get;
            set;
        }
        public string Url
        {
            get;
            set;
        }
        public bool IsDisable
        {
            get;
            set;
        }
        public int Client
        {
            get;
            set;
        }
        public LocationType LocationType
        {
            get;
            set;
        }

        public int Depth
        {
            get;
            set;
        }

        public virtual string LoctionUrl
        {
            get
            {
                int port = HttpContext.Current.Request.Url.Port;
                string text = HttpContext.Current.Request.Url.Host + ((port == 80) ? "" : (":" + port.ToString()));
                string text2 = (this.Client == 1) ? "vshop" : ((this.Client == 2) ? "wapshop" : ((this.Client == 3) ? "appshop" : "alioh"));
                string result = string.Empty;
                switch (this.LocationType)
                {
                    case LocationType.Topic:

                            result = string.Format("http://{0}/{2}/Topics.aspx?TopicId={1}", text, this.Url, text2);
                        break;
                    case LocationType.Activity:
                        {
                            string[] array = this.Url.Split(new char[]
					{
						','
					});
                            switch ((LotteryActivityType)System.Enum.Parse(typeof(LotteryActivityType), array[0]))
                            {
                                case LotteryActivityType.Wheel:
                                    result = string.Format("http://{0}/{2}/BigWheel.aspx?activityid={1}", text, array[1], text2);
                                    break;
                                case LotteryActivityType.Scratch:
                                    result = string.Format("http://{0}/{2}/Scratch.aspx?activityid={1}", text, array[1], text2);
                                    break;
                                case LotteryActivityType.SmashEgg:
                                    result = string.Format("http://{0}/{2}/SmashEgg.aspx?activityid={1}", text, array[1], text2);
                                    break;
                                case LotteryActivityType.Ticket:
                                    result = string.Format("http://{0}/{2}/SignUp.aspx?id={1}", text, array[1], text2);
                                    break;
                                case LotteryActivityType.SignUp:
                                    result = string.Format("http://{0}/{2}/Activity.aspx?id={1}", text, array[1], text2);
                                    break;
                            }
                            break;
                        }
                    case LocationType.Home:
                        result = string.Format("http://{0}/{1}/Default.aspx", text, text2);
                        break;
                    case LocationType.Category:
                        if (text2 == "wapshop" || text2 == "vshop")
                        {
                            result = string.Format("/{0}/ProductList.aspx?categoryId={1}&Depth={2}", text2, this.Url, this.Depth);
                        }
                        else
                        {
                            result = string.Format("http://{0}/{1}/{2}", text, text2, this.Url);
                        }
                        break;
                    case LocationType.ImportSourceType:
                        if (text2 == "wapshop" || text2 == "vshop")
                        {
                            result = string.Format("/{0}/ProductList.aspx?importsourceid={1}", text2, this.Url);
                        }
                        else
                        {
                            result = string.Format("http://{0}/{1}/{2}", text, text2, this.Url);
                        }
                        break;
                    case LocationType.ShoppingCart:
                        result = string.Format("http://{0}/{1}/ShoppingCart.aspx", text, text2);
                        break;
                    case LocationType.OrderCenter:
                        result = string.Format("http://{0}/{1}/MemberCenter.aspx", text, text2);
                        break;
                    case LocationType.Link:
                        if (!string.IsNullOrWhiteSpace(this.Url))
                        {
                            result = "http://" + this.Url;
                            if (this.Url.IndexOf("http") > -1)
                            {
                                result = this.Url;
                            }
                        }
                        else
                        {
                            result = (this.Url = "");
                        }
                        break;
                    case LocationType.Phone:
                        result = "tel://" + this.Url;
                        if (this.Url.IndexOf("tel") > -1)
                        {
                            result = this.Url;
                        }
                        break;
                    case LocationType.Address:
                        result = this.Url;
                        break;
                    case LocationType.GroupBuy:
                        result = string.Format("/{0}/GroupBuyList.aspx", text2);
                        break;
                    case LocationType.Brand:
                        if (text2 == "wapshop" || text2 == "vshop")
                        {
                            result = string.Format("/{0}/BrandDetail.aspx?BrandId={1}", text2, this.Url);
                        }
                        else
                        {
                            result = string.Format("/{0}/BrandList.aspx", text2);
                        }
                        break;
                    case LocationType.Article:
                        result = string.Concat(new string[]
					{
						"http://",
						text,
						"/",
						text2,
						"/",
						this.Url
					});
                        break;

                    case LocationType.Product:
                        if (text2 == "wapshop" || text2 == "vshop")
                        {
                            result = string.Format("/{0}/ProductDetails.aspx?ProductId={1}", text2, this.Url);
                        }

                        else
                        {
                            result = this.Url;
                        }
                        break;

                    default:
                        result = this.Url;
                        break;

                }
                return result;
            }
        }
    }
}
