using EcShop.UI.Common.Controls;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.CodeBehind
{
    [System.Web.UI.ParseChildren(true)]
    public class VActivityShowThree : VshopTemplatedWebControl
    {
        protected override void OnInit(System.EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "active/skin-vactivity-three.html";
            }
            base.OnInit(e);
        }
        protected override void AttachChildControls()
        {
            //PageTitle.AddSiteNameTitle("母婴用品分会场");
            if (!string.IsNullOrWhiteSpace(TitilName))
            {
                PageTitle.AddSiteNameTitle(TitilName);
            }
        }
    }
}
