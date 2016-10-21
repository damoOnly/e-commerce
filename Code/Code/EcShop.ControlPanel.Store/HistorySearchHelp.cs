using EcShop.Entities;
using Store;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace EcShop.ControlPanel.Store
{
    public class HistorySearchHelp
    {
        /// <summary>
        /// 新增搜索历史
        /// </summary>
        /// <param name="searchword"></param>
        /// <param name="userId"></param>
        /// <param name="clientType"></param>
        /// <returns></returns>
        public static bool NewSearchHistory(string searchword, int userId, ClientType clientType)
        {
            return new SearchHistoryDao().NewSearchHistory(searchword, userId, clientType);
        }


        /// <summary>
        /// 获取历史搜索记录
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="clientType">终端</param>
        /// <param name="num">数量</param>
        /// <returns></returns>
        public static DataTable GetSearchHistory(int userId, ClientType clientType, int num)
        {
            return new SearchHistoryDao().GetSearchHistory(userId, clientType, num);
        }



        /// <summary>
        /// 删除历史记录
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="clientType"></param>
        /// <returns></returns>
        public static int DeleteSearchHistory(int userId, ClientType clientType)
        {
            return new SearchHistoryDao().DeleteSearchHistory(userId, clientType);
        }



    }
}
