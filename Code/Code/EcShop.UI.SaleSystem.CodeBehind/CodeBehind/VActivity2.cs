using EcShop.UI.Common.Controls;
namespace EcShop.UI.SaleSystem.CodeBehind
{
    [System.Web.UI.ParseChildren(true)]
    public class VActivity2 : VshopTemplatedWebControl
    {

        //private Common_ActivityTime rptProducts;
        protected override void OnInit(System.EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VActivity2.html";
            }
            base.OnInit(e);
        }
        protected override void AttachChildControls()
        {

            PageTitle.AddSiteNameTitle("ÏÞÊ±ÇÀ¹º");
        }
    }
}
