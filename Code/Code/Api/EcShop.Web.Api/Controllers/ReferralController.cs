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
using EcShop.SaleSystem.Member;
using EcShop.Entities.Members;
using System.Web;
using ThoughtWorks.QRCode.Codec;
namespace EcShop.Web.Api.Controllers
{
    public class ReferralController : EcdevApiController
    {

        /// <summary>
        /// 提交申请单
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult SubmitApply(JObject request)
        {
            Logger.WriterLogger("Referral.SubmitApply, Params: " + request.ToString(), LoggerType.Info);

            ParamReferralApply param = new ParamReferralApply();

            try
            {
                param = request.ToObject<ParamReferralApply>();
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
                if (MemberProcessor.ReferralRequest(member.UserId, param.RealName, param.CellPhone, param.ReferralReason, param.Email))
                {
                    StandardResult<string> result = new StandardResult<string>()
                    {
                        code = 0,
                        msg = "提交成功",
                        data = ""
                    };

                    return base.JsonActionResult(result);
                }

                else
                {
                    StandardResult<string> result = new StandardResult<string>()
                    {
                        code = 1,
                        msg = "提交失败,邮箱已被使用",
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
        /// 佣金列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="accessToken"></param>
        /// <param name="channel"></param>
        /// <param name="platform"></param>
        /// <param name="ver"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetCommissionList(string userId, string accessToken, int channel, int platform, string ver, int pageIndex, int pageSize)
        {
            Logger.WriterLogger("Referral.GetCommissionList, Params: " + string.Format("userId={0}&accessToken={1}&channel={2}&platform={3}&ver={4}&pageIndex={5}&pageSize={6}", userId, accessToken, channel, platform, ver, pageIndex, pageSize), LoggerType.Info);

            // 保存访问信息
            base.SaveVisitInfo(userId, channel, platform, ver);

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), "Referral.GetCommissionList");
            }

            Member member = GetMember(userId.ToSeesionId());

            if (member != null)
            {
                List<CommissionItem> items = new List<CommissionItem>();

                BalanceDetailQuery query = new BalanceDetailQuery();
                query.UserId = member.UserId;
                query.PageIndex = pageIndex;
                query.PageSize = pageSize;

                DbQueryResult dbResult = MemberProcessor.GetMySplittinDetails(query, null);

                DataTable dt = dbResult.Data as DataTable;

                if (dt != null)
                {
                    CommissionItem item = null;

                    foreach (DataRow current in dt.Rows)
                    {
                        item = new CommissionItem();


                        if (current["JournalNumber"] != DBNull.Value)
                        {
                            item.JournalNumber = (long)current["JournalNumber"];
                        }

                        if (current["SubUserName"] != DBNull.Value)
                        {
                            item.SubUserName = (string)current["SubUserName"];
                        }

                        item.TradeType = 0;
                        if (current["TradeType"] != DBNull.Value)
                        {
                            item.TradeType = (int)current["TradeType"];
                        }

                        string balance = "0.00";
                        if (item.TradeType == 4)
                        {
                            if (current["Expenses"] != DBNull.Value)
                            {
                                balance = "-" + Convert.ToDouble(current["Expenses"]).ToString("0.00");
                            }
                        }

                        else
                        {
                            if (current["Income"] != DBNull.Value)
                            {
                                balance = "+" + Convert.ToDouble(current["Income"]).ToString("0.00");
                            }
                        }

                        item.Balance = balance;

                        if (current["OrderId"] != DBNull.Value)
                        {
                            item.OrderId = (string)current["OrderId"];
                        }

                        if (current["TradeDate"] != DBNull.Value)
                        {
                            item.TradeDate = ((DateTime)current["TradeDate"]).ToString("yyyy-MM-dd HH:mm:ss");
                        }

                        if (current["Remark"] != DBNull.Value)
                        {
                            item.Remark = (string)current["Remark"];
                        }

                        if (current["IsUse"] != DBNull.Value)
                        {
                            item.IsUse = (bool)current["IsUse"];
                        }

                        if (current["OrderTotal"] != DBNull.Value)
                        {
                            item.OrderTotal = (decimal)current["OrderTotal"];
                        }





                        items.Add(item);
                    }
                }


                CommissionResult data = new CommissionResult();
                data.TotalNumOfRecords = dbResult.TotalRecords;
                data.HistoryCommission = MemberProcessor.GetUserAllSplittin(member.UserId);
                data.AvailableCommission = MemberProcessor.GetUserUseSplittin(member.UserId);
                data.CommissionList = items;

                return base.JsonActionResult(new StandardResult<CommissionResult>()
                {
                    code = 0,
                    msg = "全部佣金",
                    data = data
                });
            }


            else
            {
                return base.JsonFaultResult(new CommonException(40201).GetMessage(), "Referral.GetCommissionList");
            }
        }



        /// <summary>
        /// 提现申请
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult ApplyWithdraw(JObject request)
        {
            Logger.WriterLogger("Referral.ApplyWithdraw, Params: " + request.ToString(), LoggerType.Info);

            ParamApplyWithdraw param = new ParamApplyWithdraw();

            try
            {
                param = request.ToObject<ParamApplyWithdraw>();
            }
            catch
            {
                // 参数无效
                return base.JsonFaultResult(new CommonException(40100).GetMessage(), request.ToString());
            }

            if (string.IsNullOrEmpty(param.TradePassword))
            {
                return base.JsonActionResult(new StandardResult<string>() { code = 1, msg = "请输入交易密码", data = "" });

            }

            string userId = param.UserId;

            int channel = param.channel;
            int platform = param.platform;
            string ver = param.ver;
            // 验证令牌
            string accessToken = param.accessToken;
            string sessionKey = "";
            string sessionSecret = "";

            string decryptPassword = param.TradePassword;

            int accessTookenCode = VerifyAccessToken(accessToken, out sessionKey, out sessionSecret);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), request.ToString());
            }


            if ((platform == 3 && base.AndroidIsEncryption) || (platform == 2 && base.IOSIsEncryption) || (platform == 1 && base.PCIsEncryption))
            {

                decryptPassword = base.Decrypt(param.TradePassword, sessionKey, sessionSecret);
            }




            // 保存访问信息
            base.SaveVisitInfo(userId, channel, platform, ver);

            Member member = GetMember(userId.ToSeesionId());

            if (member != null)
            {

                DbQueryResult mySplittinDraws = MemberProcessor.GetMySplittinDraws(new BalanceDrawRequestQuery
                {
                    PageIndex = 1,
                    PageSize = 1,
                    UserId = new int?(member.UserId)
                }, new int?(1));
                if (mySplittinDraws.TotalRecords > 0)
                {
                    return base.JsonActionResult(new StandardResult<string>() { code = 1, msg = "上笔提现管理员还没有处理，只有处理完后才能再次申请提现", data = "" });

                }

                decimal availableBalace = MemberProcessor.GetUserUseSplittin(member.UserId);

                if (param.Amount > availableBalace)
                {
                    return base.JsonActionResult(new StandardResult<string>() { code = 1, msg = "可提现佣金不足,请重新输入提现金额", data = "" });

                }

                member.TradePassword = decryptPassword;
                if (!Users.ValidTradePassword(member))
                {
                    return base.JsonActionResult(new StandardResult<string>() { code = 1, msg = "交易密码不正确,请重新输入", data = "" });
                }
                if (MemberProcessor.SplittinDrawRequest(new SplittinDrawInfo
                {
                    UserId = member.UserId,
                    UserName = member.Username,
                    Amount = param.Amount,
                    Account = param.Account,
                    RequestDate = System.DateTime.Now,
                    AuditStatus = 1
                }))
                {
                    return base.JsonActionResult(new StandardResult<string>() { code = 0, msg = "提现申请成功，等待管理员的审核", data = "" });

                }

                else
                {
                    return base.JsonActionResult(new StandardResult<string>() { code = 0, msg = "提现申请失败，请重试", data = "" });
                }
            }

            else
            {
                return base.JsonFaultResult(new CommonException(40201).GetMessage(), request.ToString());
            }
        }


        /// <summary>
        /// 提现记录  提现总额，可用提现未做
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="accessToken"></param>
        /// <param name="channel"></param>
        /// <param name="platform"></param>
        /// <param name="ver"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetWithdrawRecord(string userId, string accessToken, int channel, int platform, string ver, int pageIndex, int pageSize)
        {
            Logger.WriterLogger("Referral.GetWithdrawRecord, Params: " + string.Format("userId={0}&accessToken={1}&channel={2}&platform={3}&ver={4}&pageIndex={5}&pageSize={6}", userId, accessToken, channel, platform, ver, pageIndex, pageSize), LoggerType.Info);

            // 保存访问信息
            base.SaveVisitInfo(userId, channel, platform, ver);

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), "Referral.GetWithdrawRecord");
            }

            Member member = GetMember(userId.ToSeesionId());

            if (member != null)
            {
                List<WithdrawRecordItem> items = new List<WithdrawRecordItem>();

                DbQueryResult dbResult = MemberProcessor.GetMySplittinDraws(new BalanceDrawRequestQuery
                {
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    UserId = member.UserId
                }, null);

                DataTable dt = dbResult.Data as DataTable;

                if (dt != null)
                {
                    WithdrawRecordItem item = null;

                    foreach (DataRow current in dt.Rows)
                    {
                        item = new WithdrawRecordItem();


                        if (current["Amount"] != DBNull.Value)
                        {
                            item.Amount = (decimal)current["Amount"];
                        }

                        int auditStatus = 0;
                        if (current["AuditStatus"] != DBNull.Value)
                        {
                            auditStatus = (int)current["AuditStatus"];
                        }

                        if (auditStatus == 1)
                        {
                            item.AuditStatus = "待处理";
                        }

                        else
                        {
                            item.AuditStatus = "提现成功";
                        }

                        if (current["RequestDate"] != DBNull.Value)
                        {
                            item.RequestDate = ((DateTime)current["RequestDate"]).ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        items.Add(item);
                    }
                }


                WithdrawRecordResult data = new WithdrawRecordResult();
                data.TotalNumOfRecords = dbResult.TotalRecords;
                data.TotalWithdraw = MemberProcessor.GetUserAllSplittinDraws(member.UserId);
                data.AvailableWithdraw = MemberProcessor.GetUserUseSplittin(member.UserId);
                data.WithdrawRecordList = items;

                return base.JsonActionResult(new StandardResult<WithdrawRecordResult>()
                {
                    code = 0,
                    msg = "提现记录",
                    data = data
                });
            }


            else
            {
                return base.JsonFaultResult(new CommonException(40201).GetMessage(), "Referral.GetWithdrawRecord");
            }
        }

        /// <summary>
        /// 获取下级推广员
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="accessToken"></param>
        /// <param name="channel"></param>
        /// <param name="platform"></param>
        /// <param name="ver"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetSubReferral(string userId, string accessToken, int channel, int platform, string ver, int pageIndex, int pageSize, string keyword)
        {
            Logger.WriterLogger("Referral.GetSubReferral, Params: " + string.Format("userId={0}&accessToken={1}&channel={2}&platform={3}&ver={4}&pageIndex={5}&pageSize={6}&keyword={7}", userId, accessToken, channel, platform, ver, pageIndex, pageSize, keyword), LoggerType.Info);

            // 保存访问信息
            base.SaveVisitInfo(userId, channel, platform, ver);

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), "Referral.GetSubReferral");
            }

            Member member = GetMember(userId.ToSeesionId());

            if (member != null)
            {
                List<SubReferralItem> items = new List<SubReferralItem>();

                MemberQuery memberQuery = new MemberQuery();
                memberQuery.PageIndex = pageIndex;
                memberQuery.PageSize = pageSize;
                memberQuery.ReferralStatus = new int?(2);
                if (!string.IsNullOrEmpty(keyword))
                {
                    memberQuery.Username = keyword;
                }

                DbQueryResult dbResult = MemberProcessor.GetMySubUsers(memberQuery, member.UserId);

                DataTable dt = dbResult.Data as DataTable;

                if (dt != null)
                {
                    SubReferralItem item = null;

                    foreach (DataRow current in dt.Rows)
                    {
                        item = new SubReferralItem();


                        if (current["UserId"] != DBNull.Value)
                        {
                            item.SubReferralUserId = (int)current["UserId"];
                        }

                        if (current["UserName"] != DBNull.Value)
                        {
                            item.UserName = (string)current["UserName"];
                        }

                        if (current["SubReferralSplittin"] != DBNull.Value)
                        {
                            item.SubReferralSplittin = (decimal)current["SubReferralSplittin"];
                        }

                        if (current["ReferralOrderNumber"] != DBNull.Value)
                        {
                            item.ReferralOrderNumber = (int)current["ReferralOrderNumber"];
                        }

                        if (current["ReferralAuditDate"] != DBNull.Value)
                        {
                            item.ReferralAuditDate = ((DateTime)current["ReferralAuditDate"]).ToString("yyyy-MM-dd HH:mm:ss");
                        }

                        if (current["LastReferralDate"] != DBNull.Value)
                        {
                            item.LastReferralDate = ((DateTime)current["LastReferralDate"]).ToString("yyyy-MM-dd HH:mm:ss");
                        }

                        if (current["RealName"] != DBNull.Value)
                        {
                            item.RealName = (string)current["RealName"];
                        }

                        if (current["CellPhone"] != DBNull.Value)
                        {
                            item.CellPhone = (string)current["CellPhone"];
                        }

                        if (current["HeadImgUrl"] != DBNull.Value)
                        {
                            item.HeadImgUrl = Util.AppendImageHost((string)current["HeadImgUrl"]);
                        }



                        items.Add(item);
                    }
                }


                ListResult<SubReferralItem> data = new ListResult<SubReferralItem>();
                data.TotalNumOfRecords = dbResult.TotalRecords;
                data.Results = items;

                return base.JsonActionResult(new StandardResult<ListResult<SubReferralItem>>()
                {
                    code = 0,
                    msg = "下级推广员",
                    data = data
                });
            }


            else
            {
                return base.JsonFaultResult(new CommonException(40201).GetMessage(), "Referral.GetSubReferral");
            }
        }





        /// <summary>
        /// 获取下级会员
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="accessToken"></param>
        /// <param name="channel"></param>
        /// <param name="platform"></param>
        /// <param name="ver"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetSubMember(string userId, string accessToken, int channel, int platform, string ver, int pageIndex, int pageSize, string keyword)
        {
            Logger.WriterLogger("Referral.GetSubMember, Params: " + string.Format("userId={0}&accessToken={1}&channel={2}&platform={3}&ver={4}&pageIndex={5}&pageSize={6}&keyword={7}", userId, accessToken, channel, platform, ver, pageIndex, pageSize, keyword), LoggerType.Info);

            // 保存访问信息
            base.SaveVisitInfo(userId, channel, platform, ver);

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), "Referral.GetSubMember");
            }

            Member member = GetMember(userId.ToSeesionId());

            if (member != null)
            {
                List<SubMemberItem> items = new List<SubMemberItem>();
                MemberQuery memberQuery = new MemberQuery();
                memberQuery.PageIndex = pageIndex;
                memberQuery.PageSize = pageSize;
                if (!string.IsNullOrEmpty(keyword))
                {
                    memberQuery.Username = keyword;
                }

                DbQueryResult dbResult = MemberProcessor.GetMySubUsers(memberQuery, member.UserId);

                DataTable dt = dbResult.Data as DataTable;

                if (dt != null)
                {
                    SubMemberItem item = null;

                    foreach (DataRow current in dt.Rows)
                    {
                        item = new SubMemberItem();


                        if (current["UserId"] != DBNull.Value)
                        {
                            item.SubMemberUserId = (int)current["UserId"];
                        }

                        if (current["UserName"] != DBNull.Value)
                        {
                            item.UserName = (string)current["UserName"];
                        }

                        if (current["CreateDate"] != DBNull.Value)
                        {
                            item.CreateDate = ((DateTime)current["CreateDate"]).ToString("yyyy-MM-dd HH:mm:ss");
                        }

                        if (current["OrderNumber"] != DBNull.Value)
                        {
                            item.OrderCount = (int)current["OrderNumber"];
                        }

                        if (current["RealName"] != DBNull.Value)
                        {
                            item.RealName = (string)current["RealName"];
                        }

                        if (current["CellPhone"] != DBNull.Value)
                        {
                            item.CellPhone = (string)current["CellPhone"];
                        }

                        if (current["HeadImgUrl"] != DBNull.Value)
                        {
                            item.HeadImgUrl = Util.AppendImageHost((string)current["HeadImgUrl"]);
                        }

                        items.Add(item);
                    }
                }


                ListResult<SubMemberItem> data = new ListResult<SubMemberItem>();
                data.TotalNumOfRecords = dbResult.TotalRecords;
                data.Results = items;

                return base.JsonActionResult(new StandardResult<ListResult<SubMemberItem>>()
                {
                    code = 0,
                    msg = "下级会员",
                    data = data
                });
            }


            else
            {
                return base.JsonFaultResult(new CommonException(40201).GetMessage(), "Referral.GetSubMember");
            }
        }


        /// <summary>
        /// 推广二维码
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="accessToken"></param>
        /// <param name="channel"></param>
        /// <param name="platform"></param>
        /// <param name="ver"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetExtensionQRCode(string userId, string accessToken, int channel, int platform, string ver)
        {
            Logger.WriterLogger("Referral.GetExtensionQRCode, Params: " + string.Format("userId={0}&accessToken={1}&channel={2}&platform={3}&ver={4}", userId, accessToken, channel, platform, ver), LoggerType.Info);

            // 保存访问信息
            base.SaveVisitInfo(userId, channel, platform, ver);

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), "Referral.GetExtensionQRCode");
            }

            Member member = GetMember(userId.ToSeesionId());

            if (member != null)
            {
                //文件保存目录路径 
                string QRCodeConfigPath = System.Configuration.ConfigurationManager.AppSettings["QRCode_PATH"];
                string QRCodePath = HttpContext.Current.Server.MapPath(QRCodeConfigPath);

                string imgQRCode = "";
                if (System.IO.Directory.Exists(string.Concat(new object[]
			{
				QRCodePath,
				"referral_",
				member.UserId,
				".png"
			})))
                {
                    imgQRCode = string.Concat(new object[]
				{
					QRCodePath,
				"referral_",
				member.UserId,
				".png"
				});

                    QRCodeResult result = new QRCodeResult();
                    result.title = "海美生活";
                    result.summary = "海美生活";
                    result.targetUrl = Util.STORAGE_HOST + "/Register.aspx?ReferralUserId=" + member.UserId;
                    result.imageUrl = Util.AppendImageHost(imgQRCode);
                    return base.JsonActionResult(new StandardResult<QRCodeResult>()
                    {
                        code = 0,
                        msg = "推广二维码",
                        data = result
                    });
                }

                else
                {
                    imgQRCode = this.CreateQRCode(Util.STORAGE_HOST + "/Register.aspx?ReferralUserId=" + member.UserId, member.UserId);
                    QRCodeResult result = new QRCodeResult();
                    result.title = "海美生活";
                    result.summary = "海美生活";
                    result.targetUrl = Util.STORAGE_HOST + "/Register.aspx?ReferralUserId=" + member.UserId;
                    result.imageUrl = Util.AppendImageHost(imgQRCode);

                    return base.JsonActionResult(new StandardResult<QRCodeResult>()
                    {
                        code = 0,
                        msg = "推广二维码",
                        data = result
                    });
                }

            }


            else
            {
                return base.JsonFaultResult(new CommonException(40201).GetMessage(), "Referral.GetExtensionQRCode");
            }
        }


        /// <summary>
        /// 获取提现申请页面数据
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="accessToken"></param>
        /// <param name="channel"></param>
        /// <param name="platform"></param>
        /// <param name="ver"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetApplyWithdrawData(string userId, string accessToken, int channel, int platform, string ver)
        {
            Logger.WriterLogger("Referral.GetApplyWithdrawData, Params: " + string.Format("userId={0}&accessToken={1}&channel={2}&platform={3}&ver={4}", userId, accessToken, channel, platform, ver), LoggerType.Info);

            // 保存访问信息
            base.SaveVisitInfo(userId, channel, platform, ver);

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), "Referral.GetApplyWithdrawData");
            }

            Member member = GetMember(userId.ToSeesionId());

            if (member != null)
            {
                decimal availableWithdraw = MemberProcessor.GetUserUseSplittin(member.UserId);
                SplittinDrawInfo info=MemberProcessor.GetLatestSplittinDrawInfo(member.UserId);
                string latestdate = "";
                if(info==null)
                {
                    latestdate = "您还没有提现记录";

                }

                else
                {
                    latestdate = info.RequestDate.ToString("yyyy-MM-dd HH:mm:ss");
                }

                ApplyWithdrawPageData result = new ApplyWithdrawPageData();
                result.AvailableWithdraw = availableWithdraw;
                result.LatestDate = latestdate;


                return base.JsonActionResult(new StandardResult<ApplyWithdrawPageData>()
                {
                    code = 0,
                    msg = "推广二维码",
                    data = result
                });
              

            }


            else
            {
                return base.JsonFaultResult(new CommonException(40201).GetMessage(), "Referral.GetApplyWithdrawData");
            }
        }

        /// <summary>
        /// 设置交易密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult SetTradePassword(JObject request)
        {
            Logger.WriterLogger("Referral.SetTradePassword, Params: " + request.ToString(), LoggerType.Info);

            ParamTradePassword param = new ParamTradePassword();

            try
            {
                try
                {
                    param = request.ToObject<ParamTradePassword>();
                }
                catch
                {
                    return base.JsonFaultResult(new CommonException(40100).GetMessage(), request.ToString());
                }

                string accessToken = param.accessToken;
                string sessionKey = "";
                string sessionSecret = "";

                // 验证令牌
                int accessTookenCode = VerifyAccessToken(accessToken, out sessionKey, out sessionSecret);
                if (accessTookenCode > 0)
                {
                    return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), request.ToString());
                }

               
                string password = param.TradePassword;
                int channel = param.channel;
                int platform = param.platform;
                string ver = param.ver;

                // 保存访问信息
                base.SaveVisitInfo(param.UserId, channel, platform, ver);

                string decryptPassword = password;

                if ((platform == 3 && base.AndroidIsEncryption) || (platform == 2 && base.IOSIsEncryption))
                {
                    decryptPassword = base.Decrypt(password, sessionKey, sessionSecret);//TODO 解密
                }
                string sessionId = param.UserId.ToSeesionId();

                Member member = base.GetMember(sessionId,false);

                if (member != null)
                {

                    if(member.IsOpenBalance)
                    {
                        return base.JsonActionResult(new StandardResult<string>()
                        {
                            code = 1,
                            msg = "你已经设置过交易密码",
                            data = null
                        });
                    }

                    if (member.OpenBalance(decryptPassword))
                    {

                        return base.JsonActionResult(new StandardResult<string>()
                        {
                            code = 0,
                            msg = "设置交易密码成功",
                            data = null
                        });
                    }

                   else
                    {
                        return base.JsonActionResult(new StandardResult<string>()
                        {
                            code = 1,
                            msg = "设置交易密码失败",
                            data = null
                        });
                    }
                }
                else
                {
                    return base.JsonFaultResult(new FaultInfo(40201, "会员信息不存在"), request.ToString());
                }
            }
            catch (CommonException ex)
            {
                Logger.WriterLogger(ex.GetMessage().Message);
                FaultInfo info = ex.GetMessage();
                return base.JsonActionResult(info);
            }
        }



        /// <summary>
        /// 设置交易密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult EditTradePassword(JObject request)
        {
            Logger.WriterLogger("Referral.EditTradePassword, Params: " + request.ToString(), LoggerType.Info);

            ParamEditTradePassword param = new ParamEditTradePassword();

            try
            {
                try
                {
                    param = request.ToObject<ParamEditTradePassword>();
                }
                catch
                {
                    return base.JsonFaultResult(new CommonException(40100).GetMessage(), request.ToString());
                }

                string accessToken = param.accessToken;
                string sessionKey = "";
                string sessionSecret = "";

                // 验证令牌
                int accessTookenCode = VerifyAccessToken(accessToken, out sessionKey, out sessionSecret);
                if (accessTookenCode > 0)
                {
                    return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), request.ToString());
                }


                string oldpassword = param.OldTradePassword;
                string newpassword = param.NewTradePassword;
                int channel = param.channel;
                int platform = param.platform;
                string ver = param.ver;

                // 保存访问信息
                base.SaveVisitInfo(param.UserId, channel, platform, ver);

                string decryptOldPassword = oldpassword;
                string decryptNewPassword = newpassword;

                if ((platform == 3 && base.AndroidIsEncryption) || (platform == 2 && base.IOSIsEncryption))
                {
                    decryptOldPassword = base.Decrypt(oldpassword, sessionKey, sessionSecret);//TODO 解密

                    decryptNewPassword = base.Decrypt(newpassword, sessionKey, sessionSecret);
                }

                Logger.WriterLogger("Referral.EditTradePassword," + decryptOldPassword + "," + decryptNewPassword);


                string sessionId = param.UserId.ToSeesionId();

                Member member = base.GetMember(sessionId,false);

                if (member != null)
                {
                    if (!member.IsOpenBalance)
                    {
                        return base.JsonActionResult(new StandardResult<string>()
                        {
                            code = 1,
                            msg = "你还未设置过交易密码",
                            data = null
                        });
                    }
                    if (member.ChangeTradePassword(decryptOldPassword,decryptNewPassword))
                    {

                        return base.JsonActionResult(new StandardResult<string>()
                        {
                            code = 0,
                            msg = "修改交易密码成功",
                            data = null
                        });
                    }

                    else
                    {
                        return base.JsonActionResult(new StandardResult<string>()
                        {
                            code = 2,
                            msg = "旧密码错误",
                            data = null
                        });
                    }
                }
                else
                {
                    return base.JsonFaultResult(new FaultInfo(40201, "会员信息不存在"), request.ToString());
                }
            }
            catch (CommonException ex)
            {
                Logger.WriterLogger(ex.GetMessage().Message);
                FaultInfo info = ex.GetMessage();
                return base.JsonActionResult(info);
            }
        }


      /// <summary>
      /// 获取推广员汇总信息
      /// </summary>
      /// <param name="userId"></param>
      /// <param name="accessToken"></param>
      /// <param name="channel"></param>
      /// <param name="platform"></param>
      /// <param name="ver"></param>
      /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetReferralSummary(string userId, string accessToken, int channel, int platform, string ver)
        {
            Logger.WriterLogger("Referral.GetReferralSummary, Params: " + string.Format("userId={0}&accessToken={1}&channel={2}&platform={3}&ver={4}", userId, accessToken, channel, platform, ver), LoggerType.Info);

            // 保存访问信息
            base.SaveVisitInfo(userId, channel, platform, ver);

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), "Referral.GetReferralSummary");
            }

            Member member = GetMember(userId.ToSeesionId(),false);

            if (member != null)
            {
                ReferralSummaryResult result = new ReferralSummaryResult();

                result.IsSetTradePassword = member.IsOpenBalance;


                return base.JsonActionResult(new StandardResult<ReferralSummaryResult>()
                {
                    code = 0,
                    msg = "获取推广员汇总信息",
                    data = result
                });


            }


            else
            {
                return base.JsonFaultResult(new CommonException(40201).GetMessage(), "Referral.GetReferralSummary");
            }
        }



        private string CreateQRCode(string url, int userId)
        {
            string result = "";
            System.Drawing.Bitmap bitmap = new QRCodeEncoder
            {
                QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE,
                QRCodeScale = 10,
                QRCodeVersion = 0,
                QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M
            }.Encode(url);

             string QRCodeConfigPath = System.Configuration.ConfigurationManager.AppSettings["QRCode_PATH"];
             string QRCodePath = HttpContext.Current.Server.MapPath(QRCodeConfigPath);
             string text = QRCodePath;
            if (System.IO.Directory.Exists(text))
            {
                string text2 = "referral_" + userId + ".png";
                bitmap.Save(text + "/" + text2, System.Drawing.Imaging.ImageFormat.Png);
                result = "/Storage/master/QRCode/" + text2;
            }
            else
            {
                System.IO.Directory.CreateDirectory(text);
                string text2 = "referral_" + userId + ".png";
                bitmap.Save(text + "/" + text2, System.Drawing.Imaging.ImageFormat.Png);
                result = "/Storage/master/QRCode/" + text2;
            }
            return result;
        }




    }
}
