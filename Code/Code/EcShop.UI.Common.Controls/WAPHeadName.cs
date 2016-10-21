using EcShop.Membership.Context;
using System;
using System.Globalization;
using System.Web;
using System.Web.UI;
namespace EcShop.UI.Common.Controls
{
[ParseChildren(false), PersistChildren(true)]
	public class WAPHeadName : Control
    {


    private const string wapHeadName = "Ecdev.wapheadname.value";
        public static void AddHeadName(string headname)
        {
            if (HiContext.Current.Context == null)
            {
                throw new ArgumentNullException("context");
            }
            HiContext.Current.Context.Items[wapHeadName] = headname;
        }
    protected override void Render(HtmlTextWriter writer)
    {
        string text = this.Context.Items[wapHeadName] as string;
       
        writer.WriteLine(text);
    }

    }
}

