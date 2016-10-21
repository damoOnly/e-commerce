using EcShop.Core;
using EcShop.Membership.Context;
using EcShop.Membership.Core;
using EcShop.Membership.Core.Enums;
using EcShop.SqlDal.PCMenu;
using System;
using System.Web;
using System.Linq;
using EcShop.Entities;
using System.Collections.Generic;
using EcShop.ControlPanel.Store;
using EcShop.UI.ControlPanel.Utility;

namespace EcShop.UI.Web.API
{
	public class AuthValid : System.Web.IHttpHandler
	{
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
		public void ProcessRequest(System.Web.HttpContext context)
		{
			try
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
				string s = "";
				string text = context.Request["action"];
                int menuId =0;
                int.TryParse(context.Request["menuId"], out menuId);
                string roleId = context.Request["roleId"];
                string menuIds = context.Request["menuIds"];
                if(false &text != "checklogin") // && 
				{
					if (text == "all")
					{
                        string cur = System.Web.HttpContext.Current.Request["Supplier"];
                        if (!string.IsNullOrWhiteSpace(cur))
                        {
                            s = "{\"status\":\"1,1,1,1,1\",\"Supplier\":\"true\"}";
                        }
                        else
                        {
                            s = "{\"status\":\"1,1,1,1,1\"}";
                        }
					}
					else
					{
						s = "{\"status\":\"1\"}";
					}
				}
				else
				{
					string key;
					switch (key = text)
					{
					case "vstore":
						s = "{\"status\":\"" + masterSettings.OpenVstore + "\"}";
						break;
					case "wapshop":
						s = "{\"status\":\"" + masterSettings.OpenWap + "\"}";
						break;
					case "appshop":
						s = "{\"status\":\"" + masterSettings.OpenMobbile + "\"}";
						break;
					case "alioh":
						s = "{\"status\":\"" + masterSettings.OpenAliho + "\"}";
						break;
					case "taobao":
						s = "{\"status\":\"" + masterSettings.OpenTaobao + "\"}";
						break;
                    case "GetFirstMenu":
                        s = this.GetFisrtMenuInfo();
                        break;
                    case "GetMenuById":
                        s = this.GetMenuInfoByFirstId(menuId);
                        break;
                    case "GetAllMenuByRoleId":
                        s = this.GetAllMenuInfoByRoleId(roleId);
                        break;                   
					case "all":
						s = string.Concat(new object[]
						{
							"{\"status\":\"",
							masterSettings.OpenTaobao,
							",",
							masterSettings.OpenVstore,
							",",
							masterSettings.OpenMobbile,
							",",
							masterSettings.OpenWap,
							",",
							masterSettings.OpenAliho,
							"\"}"
						});
						break;
					case "checklogin":
						s = this.ChkLogin();
						break;
					}
				}
				context.Response.ContentType = "application/json";
				context.Response.Write(s);
			}
			catch (System.Exception ex)
			{
				context.Response.ContentType = "application/json";
				context.Response.Write("{\"status\":\"" + ex.Message + "\"}");
			}
		}
		public string ChkLogin()
		{
			IUser user = HiContext.Current.User;
			string result;
			if (user == null || user.UserRole != UserRole.SiteManager)
			{
				result = "{\"status\":\"false\"}";
			}
			else
			{
				result = "{\"status\":\"true\"}";
			}
			return result;
		}
        public string GetFisrtMenuInfo()
        {       
            var user = HttpContext.Current.User;
            List<PCMenuInfo> result = new List<PCMenuInfo>();
            if (user == null || !user.Identity.IsAuthenticated)
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(result);
            }
            PcMenuInfoDao dao = new PcMenuInfoDao();
            var meunList = dao.GetPCFirstMenuInfo(user.Identity.Name);
            return Newtonsoft.Json.JsonConvert.SerializeObject(meunList);
        }
        public string GetMenuInfoByFirstId(int firstMId)
        {
            var user = HttpContext.Current.User;
            PcMenuInfoDao dao = new PcMenuInfoDao();
            List<PCMenuInfo> result = new List<PCMenuInfo>();
            if (user == null || !user.Identity.IsAuthenticated)
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(result);
            }
            var meunList = dao.GetPCMenuInfoByFirstId(user.Identity.Name, firstMId);
            List<PCMenuInfo> firstMenu = new List<PCMenuInfo>();
            if (meunList != null && meunList.Count() > 0)
            {
              firstMenu= meunList.Where(c => c.levelId == 1).Select(c => c).ToList();
            }
            foreach (var item in firstMenu)
            {
                item.SubMenuItem = new List<PCMenuInfo>();
                getSubMenuItem(meunList, item);
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(firstMenu);
        }
        private void getSubMenuItem(List<PCMenuInfo> model, PCMenuInfo pMenuItem)
        {
            if (model != null && model.Count() > 0)
            {
                int count = model.Where(c => c.ParentId == pMenuItem.PrivilegeId).Select(c=>c).Count();
               if (count > 0)
               {
                   pMenuItem.SubMenuItem = new List<PCMenuInfo>();
                   pMenuItem.SubMenuItem = model.Where(c => pMenuItem.PrivilegeId == c.ParentId).Select(c => c).ToList(); 
                   foreach(var subItem in pMenuItem.SubMenuItem)
                   {
                       getSubMenuItem(model, subItem);
                   }
               }
               
            }
            
        }
        public string GetAllMenuInfoByRoleId(string roleId)
        {        
            IUser user = HiContext.Current.User;
            List<PCMenuInfo> result = new List<PCMenuInfo>();
            List<PCMenuInfo> firstMenu = new List<PCMenuInfo>();
            if (user == null)
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(result);
            }
            PcMenuInfoDao dao = new PcMenuInfoDao();
            var meunList = dao.GetPCMenuInfoByRoleId(roleId);

            if (meunList != null && meunList.Count() > 0)
            {
                firstMenu = meunList.Where(c => c.levelId == 1).Select(c => c).ToList();
            }
            foreach (var item in firstMenu)
            {
                item.SubMenuItem = new List<PCMenuInfo>();
                getSubMenuItem(meunList, item);
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(firstMenu);
           
        }
        private void PermissionsSet(string roleId, string menuIds)
        {
            //IUser user = HiContext.Current.User;
            //var item = HttpContext.Current.User;
 
            //#region 判断是否为全法用户            
           
            //if (item == null||!item.Identity.IsAuthenticated)
            //{
            //   return  "{\"status\":\"false\"}";
            //}
            //#endregion        
            ////if (user.UserRole != UserRole.SiteManager)
            ////{        
            ////    return "{\"status\":\"false\"}";
            ////}
            //try
            //{

            //    var arry_menuId = menuIds.Split(',');
            //    var grouping = arry_menuId.GroupBy(c => c);
            //    List<string> list_menuId = new List<string>();
            //    foreach (var g in grouping)
            //    {
            //        list_menuId.Add(g.Key);
            //    }           
            //    //删除了 获取待定的数据 格式 ，
            //    string text = string.Join(",",list_menuId);

            //    RoleHelper.AddPrivilegeInRoles(Guid.Parse(roleId), text);
            //    ManagerHelper.ClearRolePrivilege(Guid.Parse(roleId));
            //    return "{\"status\":\"true\"}";
            //}
            //catch (Exception)
            //{
            //    return "{\"status\":\"false\"}";
            //}

        }

	}
}
