using EcShop.ControlPanel.Store;
using EcShop.Entities.Store;
using EcShop.Membership.Context;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Web.UI.WebControls;

namespace EcShop.UI.Web.Admin.edit
{
    [PrivilegeCheck(Privilege.Supplierindex)]
    public class Supplierindex : AdminPage
    {
    }
}
