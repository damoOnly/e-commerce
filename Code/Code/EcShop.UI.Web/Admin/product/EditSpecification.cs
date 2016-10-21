using EcShop.ControlPanel.Store;
using EcShop.Entities.Store;
using EcShop.UI.ControlPanel.Utility;
using EcShop.UI.Web.Admin.product.ascx;
using System;
namespace EcShop.UI.Web.Admin.product
{
	[PrivilegeCheck(Privilege.EditProductType)]
	public class EditSpecification : AdminPage
	{
		protected SpecificationView specificationView;
	}
}
