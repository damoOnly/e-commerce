using EcShop.Web.Api.ApiException;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EcShop.Membership.Context;
using EcShop.Web.Api.Model.Result;
using EcShop.Core.Entities;
using System.Data;
using EcShop.ControlPanel.Commodities;
using Commodities;
using EcShop.Entities;
using EcShop.SaleSystem.Catalog;
using Newtonsoft.Json.Linq;
using EcShop.Web.Api.Model.RequestJsonParams;
using EcShop.ControlPanel.Store;
using Entities;
using EcShop.Web.Api.Model;
using EcShop.Entities.VShop;
using System.Text.RegularExpressions;

namespace EcShop.Web.Api.Controllers
{
    public class SupplierController : EcdevApiController
    {
        /// <summary>
        /// 获取用户店铺收藏列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="accessToken"></param>
        /// <param name="channel"></param>
        /// <param name="platform"></param>
        /// <param name="ver"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult MySupplierCollect(string userId, string accessToken, int channel, int platform, string ver, int pageIndex, int pageSize)
        {
            Logger.WriterLogger("Supplier.MySupplierCollect, Params: " + string.Format("userId={0}&accessToken={1}&channel={2}&platform={3}&ver={4}&pageIndex={5}&pageSize={6}", userId, accessToken, channel, platform, ver, pageIndex, pageSize), LoggerType.Info);

            // 保存访问信息
            base.SaveVisitInfo(userId, channel, platform, ver);

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), "Supplier.MySupplierCollect");
            }

            Member member = GetMember(userId.ToSeesionId());

            if (member != null)
            {
                List<SupplierCollectItem> items = new List<SupplierCollectItem>();

                SupplierCollectQuery query = new SupplierCollectQuery();
                query.UserId = member.UserId;
                query.PageIndex = pageIndex;
                query.PageSize = pageSize;

                DbQueryResult dbResult = SupplierHelper.GetSupplierCollect(query);

                DataTable dt = dbResult.Data as DataTable;

                if (dt != null)
                {
                    SupplierCollectItem item = null;

                    foreach (DataRow current in dt.Rows)
                    {
                        item = new SupplierCollectItem();


                        if (current["Id"] != DBNull.Value)
                        {
                            item.Id = (int)current["Id"];
                        }

                        if (current["SupplierId"] != DBNull.Value)
                        {
                            item.SupplierId = (int)current["SupplierId"];
                        }

                        if (current["ShopName"] != DBNull.Value)
                        {
                            item.SupplierName = (string)current["ShopName"];
                        }

                        item.CreateDate = "";
                        if (current["CreateDate"] != DBNull.Value)
                        {
                            item.CreateDate = ((DateTime)current["CreateDate"]).ToString("yyyy-MM-dd HH:mm:ss");
                        }

                        if (current["ShopOwner"] != DBNull.Value)
                        {
                            item.ShopOwner = (string)current["ShopOwner"];
                        }

                        if (current["County"] != DBNull.Value)
                        {
                            int countyId = (int)current["County"];
                            if (countyId > 0)
                            {
                                item.Province = RegionHelper.GetFullRegion(countyId, ",").Split(',')[0];
                            }
                        }
                        if (current["Logo"] != DBNull.Value)
                        {
                            item.Logo = Util.AppendImageHost((string)current["Logo"]);
                        }

                        if (current["MobileImage"] != DBNull.Value)
                        {
                            item.Background = Util.AppendImageHost((string)current["MobileImage"]);
                        }

                        if (current["CollectCount"] != DBNull.Value)
                        {
                            item.CollectCount = (int)current["CollectCount"];
                        }

                        items.Add(item);
                    }
                }


                SupplierCollectResult data = new SupplierCollectResult();
                data.SupplierCollectCount = dbResult.TotalRecords;
                data.ProductCollectCount = ProductBrowser.GetUserFavoriteCount(member.UserId);
                data.Results = items;

                return base.JsonActionResult(new StandardResult<SupplierCollectResult>()
                {
                    code = 0,
                    msg = "店铺收藏列表",
                    data = data
                });
            }


            else
            {
                return base.JsonFaultResult(new CommonException(40201).GetMessage(), "Supplier.MySupplierCollect");
            }
        }

        /// <summary>
        /// 收藏店铺
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult CollectSupplier(JObject request)
        {
            Logger.WriterLogger("Supplier.CollectSupplier, Params: " + request.ToString(), LoggerType.Info);

            ParamCollectSupplier param = new ParamCollectSupplier();

            try
            {
                param = request.ToObject<ParamCollectSupplier>();
            }
            catch
            {
                // 参数无效
                return base.JsonFaultResult(new CommonException(40100).GetMessage(), request.ToString());
            }

            string accessToken = param.accessToken;

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), request.ToString());
            }


            string userId = param.UserId;

            int channel = param.channel;
            int platform = param.platform;
            string ver = param.ver;

            // 保存访问信息
            base.SaveVisitInfo(userId, channel, platform, ver);

            Member member = GetMember(userId.ToSeesionId());

            if (member != null)
            {
                if (SupplierHelper.GetSupplier(param.SupplierId) == null)
                {
                    StandardResult<string> result = new StandardResult<string>()
                    {
                        code = 1,
                        msg = "供应商不存在",
                        data = ""
                    };

                    return base.JsonActionResult(result);
                }
                if (SupplierHelper.SupplierIsCollect(member.UserId, param.SupplierId))
                {
                    StandardResult<string> result = new StandardResult<string>()
                    {
                        code = 1,
                        msg = "不可重复收藏",
                        data = ""
                    };

                    return base.JsonActionResult(result);
                }

                SupplierCollectInfo info = new SupplierCollectInfo();

                info.Remark = "";
                info.SupplierId = param.SupplierId;
                info.UserId = member.UserId;
                int id = SupplierHelper.CollectSupplier(info);
                if (id > 0)
                {
                    StandardResult<string> result = new StandardResult<string>()
                    {
                        code = 0,
                        msg = "店铺收藏成功",
                        data = ""
                    };

                    return base.JsonActionResult(result);
                }

                else
                {
                    StandardResult<string> result = new StandardResult<string>()
                    {
                        code = 1,
                        msg = "店铺收藏失败",
                        data = ""
                    };

                    return base.JsonActionResult(result);
                }
            }

            else
            {
                return base.JsonFaultResult(new CommonException(40201).GetMessage(), request.ToString());
            }
        }

        /// <summary>
        /// 取消收藏店铺
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult DelCollectSupplier(JObject request)
        {
            Logger.WriterLogger("Supplier.DelCollectSupplier, Params: " + request.ToString(), LoggerType.Info);

            ParamCollectSupplier param = new ParamCollectSupplier();

            try
            {
                param = request.ToObject<ParamCollectSupplier>();
            }
            catch
            {
                // 参数无效
                return base.JsonFaultResult(new CommonException(40100).GetMessage(), request.ToString());
            }

            string accessToken = param.accessToken;

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), request.ToString());
            }


            string userId = param.UserId;

            int channel = param.channel;
            int platform = param.platform;
            string ver = param.ver;

            // 保存访问信息
            base.SaveVisitInfo(userId, channel, platform, ver);

            Member member = GetMember(userId.ToSeesionId());

            if (member != null)
            {

                if (!SupplierHelper.SupplierIsCollect(member.UserId, param.SupplierId))
                {
                    StandardResult<string> result = new StandardResult<string>()
                    {
                        code = 1,
                        msg = "不存在该收藏",
                        data = ""
                    };

                    return base.JsonActionResult(result);
                }


                if (SupplierHelper.DelCollectSupplier(member.UserId, param.SupplierId))
                {
                    StandardResult<string> result = new StandardResult<string>()
                    {
                        code = 0,
                        msg = "店铺取消收藏成功",
                        data = ""
                    };

                    return base.JsonActionResult(result);
                }

                else
                {
                    StandardResult<string> result = new StandardResult<string>()
                    {
                        code = 1,
                        msg = "店铺取消收藏失败",
                        data = ""
                    };

                    return base.JsonActionResult(result);
                }
            }

            else
            {
                return base.JsonFaultResult(new CommonException(40201).GetMessage(), request.ToString());
            }
        }

        /// <summary>
        /// 热门商家
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="accessToken"></param>
        /// <param name="channel"></param>
        /// <param name="platform"></param>
        /// <param name="ver"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetHotSupplier(string userId, string accessToken, int channel, int platform, string ver)
        {
            Logger.WriterLogger("Supplier.GetHotSupplier, Params: " + string.Format("userId={0}&accessToken={1}&channel={2}&platform={3}&ver={4}", userId, accessToken, channel, platform, ver), LoggerType.Info);

            // 保存访问信息
            base.SaveVisitInfo(userId, channel, platform, ver);

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), "Supplier.GetHotSupplier");
            }

            Member member = GetMember(userId.ToSeesionId());

            int referUserId;
            if (member != null)
            {
                referUserId = member.UserId;
            }

            else
            {
                referUserId = 0;
            }
            List<SupplierListItem> items = new List<SupplierListItem>();



            DataTable dt = SupplierConfigHelper.GetConfigSupplier(ClientType.App, SupplierCfgType.Hot, referUserId);

            if (dt != null)
            {
                SupplierListItem item = null;

                foreach (DataRow current in dt.Rows)
                {
                    item = new SupplierListItem();


                    if (current["SupplierId"] != DBNull.Value)
                    {
                        item.SupplierId = (int)current["SupplierId"];
                    }

                    if (current["ShopName"] != DBNull.Value)
                    {
                        item.SupplierName = (string)current["ShopName"];
                    }

                    if (current["ShopOwner"] != DBNull.Value)
                    {
                        item.ShopOwner = (string)current["ShopOwner"];
                    }

                    item.CreateDate = "";
                    if (current["CreateDate"] != DBNull.Value)
                    {
                        item.CreateDate = ((DateTime)current["CreateDate"]).ToString("yyyy-MM-dd HH:mm:ss");
                    }

                    if (current["County"] != DBNull.Value)
                    {
                        int countyId = (int)current["County"];
                        if (countyId > 0)
                        {
                            item.Location = RegionHelper.GetFullRegion(countyId, ",").Split(',')[0];
                        }
                    }

                    if (current["Logo"] != DBNull.Value)
                    {
                        item.Image = Util.AppendImageHost((string)current["Logo"]);
                    }

                    if (current["MobileImage"] != DBNull.Value)
                    {
                        item.Background = Util.AppendImageHost((string)current["MobileImage"]);
                    }

                    if (current["CollectCount"] != DBNull.Value)
                    {
                        item.CollectCount = (int)current["CollectCount"];
                    }

                    if (current["IsCollect"] != DBNull.Value)
                    {
                        item.IsCollect = (int)current["IsCollect"];
                    }

                    items.Add(item);
                }
            }

            return base.JsonActionResult(new StandardResult<List<SupplierListItem>>()
            {
                code = 0,
                msg = "热门商家列表",
                data = items
            });
        }


        /// <summary>
        /// 推荐商家
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="accessToken"></param>
        /// <param name="channel"></param>
        /// <param name="platform"></param>
        /// <param name="ver"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetRecommendSupplier(string userId, string accessToken, int channel, int platform, string ver)
        {
            Logger.WriterLogger("Supplier.GetRecommendSupplier, Params: " + string.Format("userId={0}&accessToken={1}&channel={2}&platform={3}&ver={4}", userId, accessToken, channel, platform, ver), LoggerType.Info);

            // 保存访问信息
            base.SaveVisitInfo(userId, channel, platform, ver);

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), "Supplier.GetRecommendSupplier");
            }

            Member member = GetMember(userId.ToSeesionId());

            int referUserId;
            if (member != null)
            {
                referUserId = member.UserId;
            }

            else
            {
                referUserId = 0;
            }
            List<SupplierListItem> items = new List<SupplierListItem>();



            DataTable dt = SupplierConfigHelper.GetConfigSupplier(ClientType.App, SupplierCfgType.Recommend, referUserId);

            if (dt != null)
            {
                SupplierListItem item = null;

                foreach (DataRow current in dt.Rows)
                {
                    item = new SupplierListItem();


                    if (current["SupplierId"] != DBNull.Value)
                    {
                        item.SupplierId = (int)current["SupplierId"];
                    }

                    if (current["ShopName"] != DBNull.Value)
                    {
                        item.SupplierName = (string)current["ShopName"];
                    }

                    if (current["ShopOwner"] != DBNull.Value)
                    {
                        item.ShopOwner = (string)current["ShopOwner"];
                    }

                    item.CreateDate = "";
                    if (current["CreateDate"] != DBNull.Value)
                    {
                        item.CreateDate = ((DateTime)current["CreateDate"]).ToString("yyyy-MM-dd HH:mm:ss");
                    }

                    if (current["County"] != DBNull.Value)
                    {
                        int countyId = (int)current["County"];
                        if (countyId > 0)
                        {
                            item.Location = RegionHelper.GetFullRegion(countyId, ",").Split(',')[0];
                        }
                    }

                    if (current["Logo"] != DBNull.Value)
                    {
                        item.Image = Util.AppendImageHost((string)current["Logo"]);
                    }

                    if (current["MobileImage"] != DBNull.Value)
                    {
                        item.Background = Util.AppendImageHost((string)current["MobileImage"]);
                    }


                    if (current["CollectCount"] != DBNull.Value)
                    {
                        item.CollectCount = (int)current["CollectCount"];
                    }

                    if (current["IsCollect"] != DBNull.Value)
                    {
                        item.IsCollect = (int)current["IsCollect"];
                    }

                    items.Add(item);
                }
            }

            return base.JsonActionResult(new StandardResult<List<SupplierListItem>>()
            {
                code = 0,
                msg = "推荐商家列表",
                data = items
            });
        }

        /// <summary>
        /// 获取商家列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="accessToken"></param>
        /// <param name="channel"></param>
        /// <param name="platform"></param>
        /// <param name="ver"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="supplierName"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetSupplierList(string userId, string accessToken, int channel, int platform, string ver, int pageIndex, int pageSize, string supplierName)
        {
            Logger.WriterLogger("Supplier.GetSupplierList, Params: " + string.Format("userId={0}&accessToken={1}&channel={2}&platform={3}&ver={4}&pageIndex={5}&pageSize={6}&supplierName={7}", userId, accessToken, channel, platform, ver, pageIndex, pageSize, supplierName), LoggerType.Info);

            // 保存访问信息
            base.SaveVisitInfo(userId, channel, platform, ver);

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), "Supplier.GetRecommendSupplier");
            }

            Member member = GetMember(userId.ToSeesionId());

            int referUserId;
            if (member != null)
            {
                referUserId = member.UserId;
            }

            else
            {
                referUserId = 0;
            }
            List<SupplierListItem> items = new List<SupplierListItem>();


            SupplierQuery query = new SupplierQuery();
            query.UserId = referUserId;
            query.SupplierName = supplierName;
            query.PageIndex = pageIndex;
            query.PageSize = pageSize;
            DbQueryResult dr = SupplierHelper.GetAppSupplier(query);

            DataTable dt = dr.Data as DataTable;
            if (dt != null)
            {
                SupplierListItem item = null;

                foreach (DataRow current in dt.Rows)
                {
                    item = new SupplierListItem();


                    if (current["SupplierId"] != DBNull.Value)
                    {
                        item.SupplierId = (int)current["SupplierId"];
                    }

                    if (current["ShopName"] != DBNull.Value)
                    {
                        item.SupplierName = (string)current["ShopName"];
                    }

                    if (current["ShopOwner"] != DBNull.Value)
                    {
                        item.ShopOwner = (string)current["ShopOwner"];
                    }

                    item.CreateDate = "";
                    if (current["CreateDate"] != DBNull.Value)
                    {
                        item.CreateDate = ((DateTime)current["CreateDate"]).ToString("yyyy-MM-dd HH:mm:ss");
                    }

                    if (current["County"] != DBNull.Value)
                    {
                        int countyId = (int)current["County"];
                        if (countyId > 0)
                        {
                            item.Location = RegionHelper.GetFullRegion(countyId, ",").Split(',')[0];
                        }
                    }

                    if (current["Logo"] != DBNull.Value)
                    {
                        item.Image = Util.AppendImageHost((string)current["Logo"]);
                    }

                    if (current["MobileImage"] != DBNull.Value)
                    {
                        item.Background = Util.AppendImageHost((string)current["MobileImage"]);
                    }


                    if (current["CollectCount"] != DBNull.Value)
                    {
                        item.CollectCount = (int)current["CollectCount"];
                    }

                    if (current["IsCollect"] != DBNull.Value)
                    {
                        item.IsCollect = (int)current["IsCollect"];
                    }

                    items.Add(item);
                }
            }

            ListResult<SupplierListItem> data = new ListResult<SupplierListItem>();
            data.TotalNumOfRecords = dr.TotalRecords;
            data.Results = items;

            return base.JsonActionResult(new StandardResult<ListResult<SupplierListItem>>()
            {
                code = 0,
                msg = "商家列表",
                data = data
            });


        }


        [HttpGet]
        public IHttpActionResult SupplierDefault(string userId, string accessToken, int channel, int platform, string ver, int supplierId)
        {
            Logger.WriterLogger("Supplier.SupplierDefault, Params: " + string.Format("userId={0}&accessToken={1}&channel={2}&platform={3}&ver={4}&supplierId={5}", userId, accessToken, channel, platform, ver, supplierId), LoggerType.Info);

            base.SaveVisitInfo(userId, channel, platform, ver);

            // 验证令牌
            string appId = "";
            int accessTookenCode = VerifyAccessToken(accessToken, out appId);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), "Supplier.SupplierDefault");
            }

            SupplierDefaultResult result = new SupplierDefaultResult();
            result.code = 0;
            result.msg = "店铺首页";
            result.data = new SupplierData();

            // Supplier
            result.data.SupplierDetail = this.GetSupplierDetail(userId, supplierId);


             //Topic
            result.data.Topic = this.GetTopic(supplierId);

            // 热卖
            result.data.HotSale = this.GetHotSale(supplierId);

            // 新品/推荐
            result.data.Recommend = this.GetRecommend(supplierId);

            return base.JsonActionResult(result);


        }

         [HttpGet]
        public IHttpActionResult SupplierListDefault(string userId, string accessToken, int channel, int platform, string ver)
        {
            Logger.WriterLogger("Supplier.SupplierListDefault, Params: " + string.Format("userId={0}&accessToken={1}&channel={2}&platform={3}&ver={4}", userId, accessToken, channel, platform, ver), LoggerType.Info);

            // 保存访问信息
            base.SaveVisitInfo(userId, channel, platform, ver);

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), "Supplier.SupplierListDefault");
            }

            Member member = GetMember(userId.ToSeesionId());

            int referUserId;
            if (member != null)
            {
                referUserId = member.UserId;
            }

            else
            {
                referUserId = 0;
            }

            SupplierDefaultListResult result = new SupplierDefaultListResult();
            result.code = 0;
            result.msg = "";
            result.data = new SupplierDefaultListData();

            // HotSupplier
            result.data.HotSupplier = this.GetHotSupplier(referUserId);

            // RecommendSupplier
            result.data.RecommendSupplier = this.GetRecommendSupplier(referUserId);






            return base.JsonActionResult(result);
        }

        private ListResult<SupplierListItem> GetHotSupplier(int userid)
        {
            List<SupplierListItem> items = new List<SupplierListItem>();



            DataTable dt = SupplierConfigHelper.GetConfigSupplier(ClientType.App, SupplierCfgType.Hot, userid);

            if (dt != null)
            {
                SupplierListItem item = null;

                foreach (DataRow current in dt.Rows)
                {
                    item = new SupplierListItem();


                    if (current["SupplierId"] != DBNull.Value)
                    {
                        item.SupplierId = (int)current["SupplierId"];
                    }

                    if (current["ShopName"] != DBNull.Value)
                    {
                        item.SupplierName = (string)current["ShopName"];
                    }

                    if (current["ShopOwner"] != DBNull.Value)
                    {
                        item.ShopOwner = (string)current["ShopOwner"];
                    }

                    item.CreateDate = "";
                    if (current["CreateDate"] != DBNull.Value)
                    {
                        item.CreateDate = ((DateTime)current["CreateDate"]).ToString("yyyy-MM-dd HH:mm:ss");
                    }

                    if (current["County"] != DBNull.Value)
                    {
                        int countyId = (int)current["County"];
                        if (countyId > 0)
                        {
                            item.Location = RegionHelper.GetFullRegion(countyId, ",").Split(',')[0];
                        }
                    }

                    if (current["Logo"] != DBNull.Value)
                    {
                        item.Image = Util.AppendImageHost((string)current["Logo"]);
                    }

                    if (current["MobileImage"] != DBNull.Value)
                    {
                        item.Background = Util.AppendImageHost((string)current["MobileImage"]);
                    }


                    if (current["CollectCount"] != DBNull.Value)
                    {
                        item.CollectCount = (int)current["CollectCount"];
                    }

                    if (current["IsCollect"] != DBNull.Value)
                    {
                        item.IsCollect = (int)current["IsCollect"];
                    }

                    items.Add(item);
                }
            }


            ListResult<SupplierListItem> data = new ListResult<SupplierListItem>();
            data.TotalNumOfRecords = items.Count; ;
            data.Results = items;

            return data;
        }

        private ListResult<SupplierListItem> GetRecommendSupplier(int userid)
        {
            List<SupplierListItem> items = new List<SupplierListItem>();



            DataTable dt = SupplierConfigHelper.GetConfigSupplier(ClientType.App, SupplierCfgType.Recommend, userid);

            if (dt != null)
            {
                SupplierListItem item = null;

                foreach (DataRow current in dt.Rows)
                {
                    item = new SupplierListItem();


                    if (current["SupplierId"] != DBNull.Value)
                    {
                        item.SupplierId = (int)current["SupplierId"];
                    }

                    if (current["ShopName"] != DBNull.Value)
                    {
                        item.SupplierName = (string)current["ShopName"];
                    }

                    if (current["ShopOwner"] != DBNull.Value)
                    {
                        item.ShopOwner = (string)current["ShopOwner"];
                    }

                    item.CreateDate = "";
                    if (current["CreateDate"] != DBNull.Value)
                    {
                        item.CreateDate = ((DateTime)current["CreateDate"]).ToString("yyyy-MM-dd HH:mm:ss");
                    }

                    if (current["County"] != DBNull.Value)
                    {
                        int countyId = (int)current["County"];
                        if (countyId > 0)
                        {
                            item.Location = RegionHelper.GetFullRegion(countyId, ",").Split(',')[0];
                        }
                    }

                    if (current["Logo"] != DBNull.Value)
                    {
                        item.Image = Util.AppendImageHost((string)current["Logo"]);
                    }

                    if (current["MobileImage"] != DBNull.Value)
                    {
                        item.Background = Util.AppendImageHost((string)current["MobileImage"]);
                    }


                    if (current["CollectCount"] != DBNull.Value)
                    {
                        item.CollectCount = (int)current["CollectCount"];
                    }

                    if (current["IsCollect"] != DBNull.Value)
                    {
                        item.IsCollect = (int)current["IsCollect"];
                    }

                    items.Add(item);
                }
            }

            ListResult<SupplierListItem> data = new ListResult<SupplierListItem>();
            data.TotalNumOfRecords = items.Count; ;
            data.Results = items;

            return data;
        }
        private SupplierListItem GetSupplierDetail(string userId, int supplierId)
        {
            SupplierListItem data = null;

            int intUserId = 0;

            Member member = GetMember(userId.ToSeesionId());

            if (member != null)
            {
                intUserId = member.UserId;
            }

            AppSupplierInfo info = SupplierHelper.GetAppSupplier(supplierId, intUserId);

            if (info != null)
            {
                data = new SupplierListItem();
                data.CollectCount = info.CollectCount;
                data.IsCollect = info.IsCollect;
                data.SupplierId = info.SupplierId;
                data.ShopOwner = info.ShopOwner;
                data.Image = Util.AppendImageHost(info.Logo);
                data.Background = Util.AppendImageHost(info.MobileImage);
                data.SupplierName = info.ShopName;
                if (info.CreateDate.HasValue)
                {
                    data.CreateDate = info.CreateDate.Value.ToString("yyyy-MM-dd HH:mm:ss");
                }

                if (info.County > 0)
                {
                    data.Location = RegionHelper.GetFullRegion(info.County, ",").Split(',')[0];
                }

            }

            return data;
        }

        private ListResult<TopicListItem> GetTopic(int supplierId)
        {
            ListResult<TopicListItem> data = null;

            DataTable dt = VShopHelper.GetHomeTopics(ClientType.App, supplierId);

            List<TopicListItem> items = new List<TopicListItem>();

            if (dt!=null && dt.Rows.Count>0)
            {
                TopicListItem item = null;

                foreach (DataRow current in dt.Rows)
                {
                    item = new TopicListItem();

                    item.TopicId = 0;
                    if (current["TopicId"] != DBNull.Value)
                    {
                        item.TopicId = (int)current["TopicId"];
                    }

                    item.TopicName = "";
                    if (current["Title"] != DBNull.Value)
                    {
                        item.TopicName = (string)current["Title"];
                    }

                    item.Icon = "";
                    if (current["IconUrl"] != DBNull.Value)
                    {
                        item.Icon = Util.AppendImageHost(((string)current["IconUrl"]));
                    }

                    item.Content = "";
                    if (current["Content"] != DBNull.Value)
                    {
                        var regex = new Regex(@"""/Storage/master");

                        item.Content = regex.Replace((string)current["Content"], "\"" + base.STORAGE_HOST + @"/Storage/master");
                    }

                    item.DisplaySequence = 0;
                    if (current["DisplaySequence"] != DBNull.Value)
                    {
                        item.DisplaySequence = (int)current["DisplaySequence"];
                    }

                    item.SupplierId = supplierId;

                    items.Add(item);
                }
              
            }

            data = new ListResult<TopicListItem>();
            data.TotalNumOfRecords = items.Count;
            data.Results = items;

            return data;
        }


        private ListResult<SupplierConfigItem> GetHotSale(int supplierId)
        {
            ListResult<SupplierConfigItem> data = null;

            IList<HotSaleInfo> list = VShopHelper.GetAllHotSale(ClientType.App,supplierId);

            List<SupplierConfigItem> items = new List<SupplierConfigItem>();

            if (list != null)
            {
                SupplierConfigItem item = null;

                foreach (var current in list)
                {
                    item = new SupplierConfigItem();

                    item.Id = current.BannerId;
                    item.Title = current.ShortDesc;
                    item.ImageUrl = Util.AppendImageHost(current.ImageUrl);
                    item.Type = (int)current.LocationType;
                    item.Values = current.Url;
                    item.DisplaySequence = current.DisplaySequence;
                    item.SupplierId = supplierId;

                    items.Add(item);
                }
            }

            data = new ListResult<SupplierConfigItem>();
            data.TotalNumOfRecords = items.Count; ;
            data.Results = items;

            return data;
        }

        private ListResult<SupplierConfigItem> GetRecommend(int supplierId)
        {
            ListResult<SupplierConfigItem> data = null;

           

            IList<RecommendInfo> list = VShopHelper.GetAllRecommend(ClientType.App,supplierId);

            List<SupplierConfigItem> items = new List<SupplierConfigItem>();

            if (list != null)
            {
                SupplierConfigItem item = null;

                foreach (var current in list)
                {
                    item = new SupplierConfigItem();

                    item.Id = current.BannerId;
                    item.Title = current.ShortDesc;
                    item.ImageUrl = Util.AppendImageHost(current.ImageUrl);
                    item.Type = (int)current.LocationType;
                    item.Values = current.Url;
                    item.DisplaySequence = current.DisplaySequence;
                    item.SupplierId = supplierId;
                    items.Add(item);
                }
            }

            data = new ListResult<SupplierConfigItem>();
            data.TotalNumOfRecords = items.Count;
            data.Results = items;

           

            return data;
        }



    }
}
