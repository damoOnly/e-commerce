using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Core.Enums;
using EcShop.Entities;
using EcShop.Entities.Store;
using EcShop.Entities.VShop;
using EcShop.Membership.Context;
using EcShop.SqlDal;
using EcShop.SqlDal.Store;
using EcShop.SqlDal.VShop;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Web;
using System.Xml;
namespace EcShop.ControlPanel.Store
{
    public static class StoreHelper
    {
        public static IList<FriendlyLinksInfo> GetFriendlyLinks()
        {
            return new FriendlyLinkDao().GetFriendlyLinks();
        }
        public static FriendlyLinksInfo GetFriendlyLink(int linkId)
        {
            return new FriendlyLinkDao().GetFriendlyLink(linkId);
        }
        public static bool UpdateFriendlyLink(FriendlyLinksInfo friendlyLink)
        {
            return null != friendlyLink && new FriendlyLinkDao().CreateUpdateDeleteFriendlyLink(friendlyLink, DataProviderAction.Update);
        }
        public static int FriendlyLinkDelete(int linkId)
        {
            return new FriendlyLinkDao().FriendlyLinkDelete(linkId);
        }
        public static bool CreateFriendlyLink(FriendlyLinksInfo friendlyLink)
        {
            return null != friendlyLink && new FriendlyLinkDao().CreateUpdateDeleteFriendlyLink(friendlyLink, DataProviderAction.Create);
        }
        public static void SwapFriendlyLinkSequence(int linkId, int replaceLinkId, int displaySequence, int replaceDisplaySequence)
        {
            new FriendlyLinkDao().SwapFriendlyLinkSequence(linkId, replaceLinkId, displaySequence, replaceDisplaySequence);
        }
        public static void DeleteHotKeywords(int hid)
        {
            new HotkeywordDao().DeleteHotKeywords(hid);
        }
        public static void SwapHotWordsSequence(int hid, int replaceHid, int displaySequence, int replaceDisplaySequence)
        {
            new HotkeywordDao().SwapHotWordsSequence(hid, replaceHid, displaySequence, replaceDisplaySequence);
        }
        public static void UpdateHotWords(int hid, int? categoryId, string hotKeyWords,int supplierId)
        {
            new HotkeywordDao().UpdateHotWords(hid, categoryId, hotKeyWords, supplierId);
        }
        public static void AddHotkeywords(int categoryId, string keywords)
        {
            new HotkeywordDao().AddHotkeywords(categoryId, keywords);
        }

        public static void AddHotkeywords(int? categoryId, string keywords, ClientType clientType, int SupplierId)
        {
            new HotkeywordDao().AddHotkeywords(categoryId, keywords, clientType, SupplierId);
        }
        public static string GetHotkeyword(int id)
        {
            return new HotkeywordDao().GetHotkeyword(id);
        }
        public static System.Data.DataTable GetHotKeywords()
        {
            return new HotkeywordDao().GetHotKeywords();
        }

        public static DataTable GetHotKeywords(ClientType clientType)
        {
            return new HotkeywordDao().GetHotKeywords(clientType);
        }

        public static DataTable GetHotKeywords(ClientType clientType, int categoryId, int hotKeywordsNum, int supplierId)
        {
            return new HotkeywordDao().GetHotKeywords(clientType, categoryId, hotKeywordsNum, supplierId);
        }


        /// <summary>
        /// 获取热搜分页
        /// </summary>
        /// <param name="clientType"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public static DbQueryResult GetHotKeywords(ClientType clientType, Pagination page)
        {
            return new HotkeywordDao().GetHotKeywords(clientType, page);
        }
        public static System.Data.DataSet GetVotes()
        {
            return new VoteDao().GetVotes(null);
        }
        public static int SetVoteIsBackup(long voteId)
        {
            return new VoteDao().SetVoteIsBackup(voteId);
        }
        public static int CreateVote(VoteInfo vote)
        {
            int num = 0;
            VoteDao voteDao = new VoteDao();
            long num2 = voteDao.CreateVote(vote);
            if (num2 > 0L)
            {
                num = 1;
                if (vote.VoteItems != null)
                {
                    foreach (VoteItemInfo current in vote.VoteItems)
                    {
                        current.VoteId = num2;
                        current.ItemCount = 0;
                        num += voteDao.CreateVoteItem(current, null);
                    }
                }
            }
            return num;
        }
        public static bool UpdateVote(VoteInfo vote)
        {
            Database database = DatabaseFactory.CreateDatabase();
            bool result;
            using (System.Data.Common.DbConnection dbConnection = database.CreateConnection())
            {
                VoteDao voteDao = new VoteDao();
                dbConnection.Open();
                System.Data.Common.DbTransaction dbTransaction = dbConnection.BeginTransaction();
                try
                {
                    if (!voteDao.UpdateVote(vote, dbTransaction))
                    {
                        dbTransaction.Rollback();
                        result = false;
                    }
                    else
                    {
                        if (!voteDao.DeleteVoteItem(vote.VoteId, dbTransaction))
                        {
                            dbTransaction.Rollback();
                            result = false;
                        }
                        else
                        {
                            int num = 0;
                            if (vote.VoteItems != null)
                            {
                                foreach (VoteItemInfo current in vote.VoteItems)
                                {
                                    current.VoteId = vote.VoteId;
                                    current.ItemCount = 0;
                                    num += voteDao.CreateVoteItem(current, dbTransaction);
                                }
                                if (num < vote.VoteItems.Count)
                                {
                                    dbTransaction.Rollback();
                                    result = false;
                                    return result;
                                }
                            }
                            dbTransaction.Commit();
                            result = true;
                        }
                    }
                }
                catch
                {
                    dbTransaction.Rollback();
                    result = false;
                }
                finally
                {
                    dbConnection.Close();
                }
            }
            return result;
        }
        public static int DeleteVote(long voteId)
        {
            return new VoteDao().DeleteVote(voteId);
        }
        public static VoteInfo GetVoteById(long voteId)
        {
            return new VoteDao().GetVoteById(voteId);
        }
        public static IList<VoteItemInfo> GetVoteItems(long voteId)
        {
            return new VoteDao().GetVoteItems(voteId);
        }
        public static int GetVoteCounts(long voteId)
        {
            return new VoteDao().GetVoteCounts(voteId);
        }
        public static string BackupData()
        {
            return new BackupRestoreDao().BackupData(HttpContext.Current.Request.MapPath(Globals.ApplicationPath + "/Storage/data/Backup/"));
        }
        public static bool InserBackInfo(string fileName, string version, long fileSize)
        {
            string filename = HiContext.Current.Context.Request.MapPath(Globals.ApplicationPath + "/config/BackupFiles.config");
            bool result;
            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(filename);
                XmlNode xmlNode = xmlDocument.SelectSingleNode("root");
                XmlElement xmlElement = xmlDocument.CreateElement("backupfile");
                xmlElement.SetAttribute("BackupName", fileName);
                xmlElement.SetAttribute("Version", version.ToString());
                xmlElement.SetAttribute("FileSize", fileSize.ToString());
                xmlElement.SetAttribute("BackupTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                xmlNode.AppendChild(xmlElement);
                xmlDocument.Save(filename);
                result = true;
            }
            catch
            {
                result = false;
            }
            return result;
        }
        public static System.Data.DataTable GetBackupFiles()
        {
            System.Data.DataTable dataTable = new System.Data.DataTable();
            dataTable.Columns.Add("BackupName", typeof(string));
            dataTable.Columns.Add("Version", typeof(string));
            dataTable.Columns.Add("FileSize", typeof(string));
            dataTable.Columns.Add("BackupTime", typeof(string));
            string filename = HiContext.Current.Context.Request.MapPath(Globals.ApplicationPath + "/config/BackupFiles.config");
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(filename);
            XmlNodeList childNodes = xmlDocument.SelectSingleNode("root").ChildNodes;
            foreach (XmlNode xmlNode in childNodes)
            {
                XmlElement xmlElement = (XmlElement)xmlNode;
                System.Data.DataRow dataRow = dataTable.NewRow();
                dataRow["BackupName"] = xmlElement.GetAttribute("BackupName");
                dataRow["Version"] = xmlElement.GetAttribute("Version");
                dataRow["FileSize"] = xmlElement.GetAttribute("FileSize");
                dataRow["BackupTime"] = xmlElement.GetAttribute("BackupTime");
                dataTable.Rows.Add(dataRow);
            }
            return dataTable;
        }
        public static bool DeleteBackupFile(string backupName)
        {
            string filename = HiContext.Current.Context.Request.MapPath(Globals.ApplicationPath + "/config/BackupFiles.config");
            bool result;
            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(filename);
                XmlNodeList childNodes = xmlDocument.SelectSingleNode("root").ChildNodes;
                foreach (XmlNode xmlNode in childNodes)
                {
                    XmlElement xmlElement = (XmlElement)xmlNode;
                    if (xmlElement.GetAttribute("BackupName") == backupName)
                    {
                        xmlDocument.SelectSingleNode("root").RemoveChild(xmlNode);
                    }
                }
                xmlDocument.Save(filename);
                result = true;
            }
            catch
            {
                result = false;
            }
            return result;
        }
        public static bool RestoreData(string bakFullName)
        {
            bool result = new BackupRestoreDao().RestoreData(bakFullName);
            new BackupRestoreDao().Restor();
            return result;
        }
        public static IList<MenuInfo> GetMenus(ClientType clientType)
        {
            IList<MenuInfo> list = new List<MenuInfo>();
            MenuDao menuDao = new MenuDao();
            IList<MenuInfo> topMenus = menuDao.GetTopMenus(clientType);
            IList<MenuInfo> result;
            if (topMenus == null)
            {
                result = list;
            }
            else
            {
                foreach (MenuInfo current in topMenus)
                {
                    list.Add(current);
                    IList<MenuInfo> menusByParentId = menuDao.GetMenusByParentId(current.MenuId, clientType);
                    if (menusByParentId != null)
                    {
                        foreach (MenuInfo current2 in menusByParentId)
                        {
                            list.Add(current2);
                        }
                    }
                }
                result = list;
            }
            return result;
        }
        public static IList<MenuInfo> GetMenusByParentId(int parentId, ClientType clientType)
        {
            return new MenuDao().GetMenusByParentId(parentId, clientType);
        }
        public static MenuInfo GetMenu(int menuId)
        {
            return new MenuDao().GetMenu(menuId);
        }
        public static IList<MenuInfo> GetTopMenus(ClientType clientType)
        {
            return new MenuDao().GetTopMenus(clientType);
        }
        public static bool CanAddMenu(int parentId, ClientType clientType)
        {
            IList<MenuInfo> menusByParentId = new MenuDao().GetMenusByParentId(parentId, clientType);
            bool result;
            if (menusByParentId == null || menusByParentId.Count == 0)
            {
                result = true;
            }
            else
            {
                if (parentId == 0)
                {
                    result = (menusByParentId.Count < 3);
                }
                else
                {
                    result = (menusByParentId.Count < 5);
                }
            }
            return result;
        }
        public static bool UpdateMenu(MenuInfo menu)
        {
            return new MenuDao().UpdateMenu(menu);
        }
        public static bool SaveMenu(MenuInfo menu)
        {
            return new MenuDao().SaveMenu(menu);
        }
        public static bool DeleteMenu(int menuId)
        {
            return new MenuDao().DeleteMenu(menuId);
        }
        public static void SwapMenuSequence(int menuId, bool isUp)
        {
            new MenuDao().SwapMenuSequence(menuId, isUp);
        }
        public static IList<MenuInfo> GetInitMenus(ClientType clientType)
        {
            MenuDao menuDao = new MenuDao();
            IList<MenuInfo> topMenus = menuDao.GetTopMenus(clientType);
            foreach (MenuInfo current in topMenus)
            {
                current.Chilren = menuDao.GetMenusByParentId(current.MenuId, clientType);
                if (current.Chilren == null)
                {
                    current.Chilren = new List<MenuInfo>();
                }
            }
            return topMenus;
        }
        public static string UploadLinkImage(HttpPostedFile postedFile)
        {
            string result;
            if (!ResourcesHelper.CheckPostedFile(postedFile))
            {
                result = string.Empty;
            }
            else
            {
                string text = HiContext.Current.GetStoragePath() + "/link/" + ResourcesHelper.GenerateFilename(Path.GetExtension(postedFile.FileName));
                postedFile.SaveAs(HiContext.Current.Context.Request.MapPath(Globals.ApplicationPath + text));
                result = text;
            }
            return result;
        }
        public static string UploadLogo(HttpPostedFile postedFile)
        {
            string result;
            if (!ResourcesHelper.CheckPostedFile(postedFile))
            {
                result = string.Empty;
            }
            else
            {
                string text = HiContext.Current.GetStoragePath() + ResourcesHelper.GenerateFilename(Path.GetExtension(postedFile.FileName));
                postedFile.SaveAs(HiContext.Current.Context.Request.MapPath(Globals.ApplicationPath + text));
                result = text;
            }
            return result;
        }
        public static void DeleteImage(string imageUrl)
        {
            if (!string.IsNullOrEmpty(imageUrl))
            {
                try
                {
                    string path = HiContext.Current.Context.Request.MapPath(Globals.ApplicationPath + imageUrl);
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                }
                catch
                {
                }
            }
        }
        public static bool AddVshopDesignTemplate(VshopDesignTemplate template)
        {
            bool b = false;
            if (template != null)
            {
                b = new VshopDesignTemplateDao().AddVshopDesignTemplate(template);
            }
            return b;
        }
        public static VshopDesignTemplate GetVshopDesignTemplate(int TemplateType, bool IsInUse)
        {
            if (TemplateType == 0)
            {
                return null;
            }
            return new VshopDesignTemplateDao().GetVshopDesignTemplate(TemplateType, IsInUse);
        }

        public static VshopDesignTemplate GetVshopDesignTemplate(int TemplateType, bool IsInUse, int supplierId)
        {
            if (TemplateType == 0)
            {
                return null;
            }
            return new VshopDesignTemplateDao().GetVshopDesignTemplate(TemplateType, IsInUse, supplierId);
        }

        public static bool UpdateVshopDesignTemplate(VshopDesignTemplate template)
        {
            return new VshopDesignTemplateDao().UpdateVshopDesignTemplate(template);
        }
        public static bool ActivateDefaultTemplate(bool IsInUse)
        {
            return new VshopDesignTemplateDao().ActivateDefaultTemplate(IsInUse);
        }

        public static StoreInfo GetStoreByStoreId(int storeId)
        {
            return new StoreDao().GetStoreByStoreId(storeId);
        }
    }
}
