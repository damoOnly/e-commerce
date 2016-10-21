using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcShop.Entities
{
   /// <summary>
   /// pc后台菜单
   /// </summary>
   public class PCMenuInfo
    {

        /// <summary>
        /// 菜单ID
        /// </summary>
        public int PrivilegeId { get; set; }
       /// <summary>
       /// 菜单英文名称
       /// </summary>
        public string PrivilegeENName { get; set; }
       /// <summary>
       /// 菜单中文名称
       /// </summary>
        public string PrivilegeName { get; set; }
       /// <summary>
       /// 父级ID
       /// </summary>
        public int ParentId { get; set; }
       /// <summary>
       /// 状态 1正常 非1 禁用
       /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 排序值
        /// </summary>
        public int SortValue { get; set; }
       /// <summary>
       /// URL地址
       /// </summary>
        public string LinkUrl { get; set; }
       /// <summary>
       ///  1 菜单 0 非菜单（按扭）
       /// </summary>
        public int IsMenu { get; set; }
       /// <summary>
       /// 级别
       /// </summary>
        public int levelId { get; set; }
       /// <summary>
       /// 是否选中
       /// </summary>
        public int IsChecked { get; set; }

        public List<PCMenuInfo> SubMenuItem { get; set; }

       /// <summary>
       /// Id级别列表
       /// </summary>
        public string newPrivilegeId { get; set; }

    }
}
