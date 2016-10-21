using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Ecdev.Components.Validation;

using EcShop.Core;
using EcShop.Core.Entities;

using EcShop.Entities;
using EcShop.Entities.Comments;
using EcShop.Entities.Sales;
using EcShop.Entities.Orders;
using EcShop.Entities.Members;

using EcShop.SaleSystem.Shopping;
using EcShop.SaleSystem.Member;
using EcShop.SaleSystem.Vshop;
using EcShop.SaleSystem.Catalog;
using EcShop.SaleSystem.Comments;

using EcShop.ControlPanel.Sales;

using EcShop.Membership.Context;
using EcShop.Membership.Core;

using EcShop.SqlDal;

using EcShop.Web.Api.ApiException;

using EcShop.Web.Api.Model;
using EcShop.Web.Api.Model.RequestJsonParams;
using EcShop.Web.Api.Model.Result;
using EcShop.Membership.Core.Enums;
using Commodities;
using EcShop.ControlPanel.Commodities;
using EcShop.Entities.YJF;
using System.Configuration;
using EcShop.ControlPanel.Members;
using Members;

namespace EcShop.Web.Api.Controllers
{
    public class MemberController : EcdevApiController
    {
        #region Address
        [HttpGet]
        public IHttpActionResult Addresses(string userId, string accessToken, int channel, int platform, string ver)
        {
            Logger.WriterLogger("Member.Addresses, Params: " + string.Format("userId={0}&accessToken={1}&channel={2}&platform={3}&ver={4}", userId, accessToken, channel, platform, ver), LoggerType.Info);

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), "Addresses: ");
            }

            // 验证参数
            //ThrowParamException(skuId);

            // 保存访问信息
            base.SaveVisitInfo(userId, channel, platform, ver);

            string msg = "";

            Member member = GetMember(userId.ToSeesionId());

            if (member == null)
            {
                return base.JsonFaultResult(new CommonException(40201).GetMessage(), "Member.Messages");
            }

            List<AddressItem> items = new List<AddressItem>();

            List<ShippingAddressInfo> address = EcShop.SaleSystem.Member.MemberProcessor.GetShippingAddresses(member.UserId).ToList();

            if (address != null)
            {
                AddressItem item = null;

                foreach (ShippingAddressInfo current in address)
                {
                    string province = "", city = "", county = "";

                    item = new AddressItem();

                    item.Id = current.ShippingId;
                    item.RegionId = current.RegionId;

                    GetRegion(current.RegionId, out province, out city, out county);

                    item.Province = province;
                    item.City = city;
                    item.District = county;
                    item.Town = "";
                    item.Receiver = current.ShipTo;
                    item.Address = current.Address;
                    item.Zipcode = current.Zipcode;
                    item.Telephone = current.TelPhone;
                    item.Cellphone = current.CellPhone;
                    item.IsDefault = current.IsDefault;
                    item.IdNo = current.IdentityCard;
                    item.IsLatest = false;

                    items.Add(item);
                }
            }

            StandardResult<ListResult<AddressItem>> result = new StandardResult<ListResult<AddressItem>>()
            {
                code = 0,
                msg = "我的收货地址列表",
                data = new ListResult<AddressItem>()
                {
                    TotalNumOfRecords = items.Count,
                    Results = items
                }
            };

            return base.JsonActionResult(result);
        }

        [HttpPost]
        public IHttpActionResult NewAddress(JObject request)
        {
            Logger.WriterLogger("Member.NewAddress, Params: " + request.ToString(), LoggerType.Info);

            ParamAddress param = new ParamAddress();

            try
            {
                param = request.ToObject<ParamAddress>();
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

            // 验证参数
            //ThrowParamException(skuId);

            string userId = param.UserId;
            int regionId = param.RegionId;
            string receiver = param.Receiver;
            string zipcode = param.Zipcode;
            string address = param.Address;
            string telephone = param.Telephone;
            string cellphone = param.Cellphone;
            bool isDefault = param.IsDefault;
            string IdNo = param.IdNo;

            int channel = param.channel;
            int platform = param.platform;
            string ver = param.ver;

            // 保存访问信息
            base.SaveVisitInfo(userId, channel, platform, ver);

            // 验证参数
            if (string.IsNullOrWhiteSpace(telephone) && string.IsNullOrWhiteSpace(cellphone))
            {
                StandardResult<string> result = new StandardResult<string>()
                {
                    code = 1,
                    msg = "电话号码和手机二者必填其一",
                    data = ""
                };

                return base.JsonActionResult(result);
            }
            string patternIdentityCard = "^[1-9]\\d{5}[1-9]\\d{3}((0[1-9])|(1[0-2]))((0[1-9])|([1-2][0-9])|(3[0-1]))\\d{3}(\\d|x|X)";
            System.Text.RegularExpressions.Regex regexIdentityCard = new System.Text.RegularExpressions.Regex(patternIdentityCard);

            if (!string.IsNullOrWhiteSpace(IdNo) && !regexIdentityCard.IsMatch(IdNo.Trim()))
            {
                StandardResult<string> result = new StandardResult<string>()
                {
                    code = 1,
                    msg = "请输入正确的身份证号码",
                    data = ""
                };

                return base.JsonActionResult(result);
            }

            Member member = GetMember(userId.ToSeesionId());

            if (member != null)
            {
                ShippingAddressInfo shippingAddress = new ShippingAddressInfo();
                shippingAddress.UserId = member.UserId;
                shippingAddress.RegionId = regionId;
                shippingAddress.Address = address;
                shippingAddress.Zipcode = zipcode;
                shippingAddress.ShipTo = receiver;

                shippingAddress.TelPhone = telephone;
                shippingAddress.CellPhone = cellphone;

                Regex regMobile = new Regex("^(13|14|15|17|18)\\d{9}$");

                if (!regMobile.IsMatch(cellphone))
                {
                    shippingAddress.CellPhone = telephone;
                }

                shippingAddress.IsDefault = isDefault;
                shippingAddress.IdentityCard = IdNo;

                int shippingAddressId = EcShop.SaleSystem.Member.MemberProcessor.AddShippingAddress(shippingAddress);

                if (shippingAddressId > 0)
                {
                    string province = "", city = "", county = "";
                    GetRegion(regionId, out province, out city, out county);

                    StandardResult<AddressItem> result = new StandardResult<AddressItem>()
                    {
                        code = 0,
                        msg = "收货地址添加成功",
                        data = new AddressItem(shippingAddressId, regionId, province, city, county, "", address, zipcode, receiver, telephone, cellphone, isDefault)
                    };

                    return base.JsonActionResult(result);
                }
            }
            else
            {
                return base.JsonFaultResult(new CommonException(40201).GetMessage(), request.ToString());
            }

            return base.JsonFaultResult(new FaultInfo(40999, ""), request.ToString());
        }

        [HttpPost]
        public IHttpActionResult UpdateAddress(JObject request)
        {
            Logger.WriterLogger("Member.UpdateAddress, Params: " + request.ToString(), LoggerType.Info);

            ParamAddress param = new ParamAddress();

            try
            {
                param = request.ToObject<ParamAddress>();
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

            int id = param.Id;
            string userId = param.UserId;
            int regionId = param.RegionId;
            string receiver = param.Receiver;
            string zipcode = param.Zipcode;
            string address = param.Address;
            string telephone = param.Telephone;
            string cellphone = param.Cellphone;
            bool isDefault = param.IsDefault;
            string IdNo = param.IdNo;

            int channel = param.channel;
            int platform = param.platform;
            string ver = param.ver;

            // 验证参数
            if (string.IsNullOrWhiteSpace(telephone) && string.IsNullOrWhiteSpace(cellphone))
            {
                StandardResult<string> result = new StandardResult<string>()
                {
                    code = 1,
                    msg = "电话号码和手机二者必填其一",
                    data = ""
                };

                return base.JsonActionResult(result);
            }
            string patternIdentityCard = "^[1-9]\\d{5}[1-9]\\d{3}((0[1-9])|(1[0-2]))((0[1-9])|([1-2][0-9])|(3[0-1]))\\d{3}(\\d|x|X)";
            System.Text.RegularExpressions.Regex regexIdentityCard = new System.Text.RegularExpressions.Regex(patternIdentityCard);

            if (!string.IsNullOrWhiteSpace(IdNo) && !regexIdentityCard.IsMatch(IdNo.Trim()))
            {
                StandardResult<string> result = new StandardResult<string>()
                {
                    code = 1,
                    msg = "请输入正确的身份证号码",
                    data = ""
                };

                return base.JsonActionResult(result);
            }

            // 保存访问信息
            base.SaveVisitInfo(userId, channel, platform, ver);

            Member member = GetMember(userId.ToSeesionId());

            if (member != null)
            {
                ShippingAddressInfo shippingAddress = EcShop.SaleSystem.Member.MemberProcessor.GetShippingAddress(id);

                if (shippingAddress == null)
                {
                    StandardResult<string> result = new StandardResult<string>()
                    {
                        code = 1,
                        msg = "收货地址不存在",
                        data = ""
                    };

                    return base.JsonActionResult(result);
                }

                shippingAddress.UserId = member.UserId;
                shippingAddress.RegionId = regionId;
                shippingAddress.Address = address;
                shippingAddress.Zipcode = zipcode;
                shippingAddress.ShipTo = receiver;
                shippingAddress.TelPhone = telephone;
                shippingAddress.CellPhone = cellphone;

                Regex regMobile = new Regex("^(13|14|15|17|18)\\d{9}$");

                if (!regMobile.IsMatch(cellphone))
                {
                    shippingAddress.CellPhone = telephone;
                }

                shippingAddress.IsDefault = isDefault;
                shippingAddress.IdentityCard = IdNo;

                if (isDefault)
                {
                    if (EcShop.SaleSystem.Member.MemberProcessor.SetDefaultShippingAddress(id, member.UserId))
                    {
                        StandardResult<string> result = new StandardResult<string>()
                        {
                            code = 0,
                            msg = "设置默认地址成功",
                            data = ""
                        };

                        return base.JsonActionResult(result);
                    }

                    else
                    {
                        StandardResult<string> result = new StandardResult<string>()
                        {
                            code = 1,
                            msg = "设置默认地址失败",
                            data = ""
                        };

                        return base.JsonActionResult(result);
                    }
                }

                bool ret = EcShop.SaleSystem.Member.MemberProcessor.UpdateShippingAddress(shippingAddress);

                if (ret)
                {
                    string province = "", city = "", county = "";
                    GetRegion(regionId, out province, out city, out county);

                    StandardResult<AddressItem> result = new StandardResult<AddressItem>()
                    {
                        code = 0,
                        msg = "收货地址已更新",
                        data = new AddressItem(id, regionId, province, city, county, "", address, zipcode, receiver, telephone, cellphone, isDefault)
                    };

                    return base.JsonActionResult(result);
                }
            }
            else
            {
                return base.JsonFaultResult(new CommonException(40201).GetMessage(), request.ToString());
            }

            return base.JsonFaultResult(new FaultInfo(40999, ""), request.ToString());
        }

        [HttpPost]
        public IHttpActionResult DeleteAddress(JObject request)
        {
            Logger.WriterLogger("Member.DeleteAddress, Params: " + request.ToString(), LoggerType.Info);

            ParamDeleteAddress param = new ParamDeleteAddress();

            try
            {
                param = request.ToObject<ParamDeleteAddress>();
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

            // 验证参数
            //ThrowParamException(skuId);

            string userId = param.UserId;
            int id = param.Id;

            int channel = param.channel;
            int platform = param.platform;
            string ver = param.ver;

            // 保存访问信息
            base.SaveVisitInfo(userId, channel, platform, ver);

            Member member = GetMember(userId.ToSeesionId());

            if (member != null)
            {
                bool ret = EcShop.SaleSystem.Member.MemberProcessor.DelShippingAddress(id, member.UserId);

                if (ret)
                {
                    StandardResult<string> result = new StandardResult<string>()
                    {
                        code = 0,
                        msg = "收货地址删除成功",
                        data = ""
                    };

                    return base.JsonActionResult(result);
                }
            }
            else
            {
                return base.JsonFaultResult(new CommonException(40201).GetMessage(), request.ToString());
            }

            return base.JsonFaultResult(new FaultInfo(40999, ""), request.ToString());
        }

        [HttpGet]
        public IHttpActionResult Get(string userId, string accessToken, int channel, int platform, string ver)
        {
            Logger.WriterLogger("Member.Get, Params: " + string.Format("userId={0}&accessToken={1}&channel={2}&platform={3}&ver={4}", userId, accessToken, channel, platform, ver), LoggerType.Info);

            // 保存访问信息
            base.SaveVisitInfo(userId, channel, platform, ver);

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), "Member.Get");
            }

            Member member = GetMember(userId.ToSeesionId(),false);

            if (member != null)
            {
                //string avatar = Users.GetUserAvatar(member.UserId);
                string userName, realName, email, cellphone, qq, avatar;
                decimal balance = 0M, expenditure = 0M;

                bool retVal = Users.GetMemberInfo(member.UserId, out userName, out realName, out email, out cellphone, out qq, out avatar, out balance, out expenditure);
                int gender = (int)member.Gender;//性别（0、保密；1、男性；2、女性）
                List<ShippingAddressInfo> address = EcShop.SaleSystem.Member.MemberProcessor.GetShippingAddresses(member.UserId).ToList();
                int addresscount = 0;//收货地址数量
                if (address != null)
                {
                    addresscount = address.Count;
                }
                int referralStatus = member.ReferralStatus;//推广员；0、未申请；1、申请中（未审核）；2、审核通过；3、审核未通过

                bool IsVerify;
                if(member.IsVerify==1)
                {
                    IsVerify = true;
                }

                else
                {
                    IsVerify = false;
                }
                StandardResult<MemberResult> result = new StandardResult<MemberResult>()
                {
                    code = 0,
                    msg = "",
                    
                    //data = new MemberResult(Util.AppendImageHost(avatar), cellphone, email, realName, qq, userName, balance, expenditure, gender, addresscount, referralStatus)

                    data = new MemberResult(Util.AppendImageHost(avatar), cellphone, email, realName, qq, userName, balance, expenditure, gender, addresscount, referralStatus,IsVerify,member.IdentityCard)
                };

                return base.JsonActionResult(result);
            }
            else
            {
                return base.JsonFaultResult(new CommonException(40201).GetMessage(), "Member.Get");
            }
        }

        [HttpPost]
        public IHttpActionResult Update(JObject request)
        {
            Logger.WriterLogger("Member.Update, Params: " + request.ToString(), LoggerType.Info);

            ParamMember param = new ParamMember();

            try
            {
                param = request.ToObject<ParamMember>();
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

            // 验证参数

            string userId = param.UserId;

            int channel = param.channel;
            int platform = param.platform;
            string ver = param.ver;

            // 保存访问信息
            base.SaveVisitInfo(userId, channel, platform, ver);

            Member member = GetMember(userId.ToSeesionId(),false);

            if (member != null)
            {
                //bool ret = Users.UpdateUser(member.UserId, realName, email, cellphone, qq, member.Email ?? "", member.CellPhone ?? "", member.EmailVerification, member.CellPhoneVerification);
                //bool ret = Users.UpdateUser(member);

                string oldphone = member.CellPhone;

                string oldemail = member.Email;

                int oldIsVerify = member.IsVerify;

                string oldRealName = member.RealName;

                string oldIdNo = member.IdentityCard;

                bool newIsVerify = false;

                try
                {
                    if (string.IsNullOrWhiteSpace(param.IdNo))
                    {
                        param.IdNo = "";
                    }

                    if (string.IsNullOrWhiteSpace(oldIdNo))
                    {
                        oldIdNo = "";
                    }

                    if (!string.IsNullOrEmpty(param.IdNo.Trim()) && member.UserId > 0)
                    {
                        Logger.WriterLogger(string.Format("param.IdNo:{0},member.UserId:{1}", param.IdNo, member.UserId), LoggerType.Info);

                        //存在已认证的身份证号码，不能保存!
                        if ((param.IdNo.Trim().ToUpper() != oldIdNo.ToUpper() || oldRealName != param.RealName) && UserHelper.IsExistIdentityCard(param.IdNo, member.UserId) > 0)
                        {
                            return base.JsonActionResult(new StandardResult<string>()
                            {
                                code = 1,
                                msg = "存在已认证的身份证号码，不能保存!",
                                data = null
                            });
                        }
                    }
                }
                catch
                {
                    Logger.WriterLogger(string.Format("catch:param.IdNo:{0},member.UserId:{1}", param.IdNo, member.UserId), LoggerType.Info);
                }


                #region 实名认证

                if (!string.IsNullOrWhiteSpace(param.RealName) && !string.IsNullOrWhiteSpace(param.IdNo))
                {

                    //身份证验证
                    string patternIdentityCard = "^[1-9]\\d{5}[1-9]\\d{3}((0[1-9])|(1[0-2]))((0[1-9])|([1-2][0-9])|(3[0-1]))\\d{3}(\\d|x|X)";
                    System.Text.RegularExpressions.Regex regexIdentityCard = new System.Text.RegularExpressions.Regex(patternIdentityCard);

                    if (!regexIdentityCard.IsMatch(param.IdNo.Trim()))
                    {

                        return base.JsonActionResult(new StandardResult<string>()
                        {
                            code = 1,
                            msg = "请输入正确的身份证号码",
                            data = null
                        });
                    }


                    //if (oldIsVerify == 1)
                    //{

                    //    return base.JsonActionResult(new StandardResult<string>()
                    //    {
                    //        code = 1,
                    //        msg = "你已经成功实名验证",
                    //        data = null
                    //    });

                    //}

                    //else
                    //{
                        int MaxCount = string.IsNullOrEmpty(ConfigurationManager.AppSettings["IsVerifyCount"]) ? 3 : int.Parse(ConfigurationManager.AppSettings["IsVerifyCount"]);
                        int fcount = MemberHelper.CheckErrorCount(member.UserId);
                        if (fcount >= MaxCount)
                        {
                            return base.JsonActionResult(new StandardResult<string>()
                            {
                                code = 1,
                                msg = string.Format("您今天验证身份次数超过{0}次，稍后再试！", MaxCount),
                                data = null
                            });
                           
                        }
                        string returnstr = new CertNoValidHelper().CertNoValid(param.RealName, param.IdNo);

                        if (returnstr == "实名通过")
                        {
                            MemberHelper.AddIsVerifyMsg(member.UserId);
                            newIsVerify = true;
                        }

                        else if (returnstr == "实名未通过")
                        {
                            MemberHelper.AddIsVerifyMsg(member.UserId);
                            return base.JsonActionResult(new StandardResult<string>()
                            {
                                code = 1,
                                msg = string.Format("实名验证不通过，一天只能验证{0}次，你还能验证{1}次,请核实无误后再验证", MaxCount, MaxCount-fcount-1),
                                data = null
                            });
                        }

                        else if (returnstr == "实名查询失败")
                        {
                            return base.JsonActionResult(new StandardResult<string>()
                            {
                                code = 1,
                                msg = "实名查询失败",
                                data = null
                            });
                        }

                        else if (returnstr == "查询异常")
                        {
                            return base.JsonActionResult(new StandardResult<string>()
                            {
                                code = 1,
                                msg = "查询异常",
                                data = null
                            });
                        }

                        else
                        {
                            return base.JsonActionResult(new StandardResult<string>()
                            {
                                code = 1,
                                msg = "转码异常",
                                data = null
                            });
                        }

                       
                    //}

                }

                #endregion

                if (!string.IsNullOrWhiteSpace(param.RealName))
                {
                    member.RealName = param.RealName;
                }


                if (!string.IsNullOrWhiteSpace(param.QQ))
                {
                    member.QQ = param.QQ;
                }



                if (!string.IsNullOrWhiteSpace(param.Email))
                {
                    member.Email = param.Email;
                }

                if (!string.IsNullOrWhiteSpace(param.Mobile))
                {
                    member.CellPhone = param.Mobile;
                }



                if (param.Gender >= 0 && param.Gender <= 2)
                {
                    member.Gender = (Gender)param.Gender;
                }


                bool ret;
                if (!newIsVerify)
                {
                    ret = Users.UpdateUser(member.UserId, member.RealName, member.Email ?? "", member.CellPhone ?? "", member.QQ, oldemail ?? "", oldphone ?? "", member.EmailVerification, member.CellPhoneVerification, (int)member.Gender);
                }
                else
                {
                    ret = Users.UpdateUser(member.UserId, member.RealName, member.Email ?? "", member.CellPhone ?? "", member.QQ, oldemail ?? "", oldphone ?? "", member.EmailVerification, member.CellPhoneVerification, (int)member.Gender,param.IdNo,1,DateTime.Now);
                }
                if (ret)
                {
                    if (!newIsVerify)
                    {
                        return base.JsonActionResult(new StandardResult<string>()
                        {
                            code = 0,
                            msg = "会员信息已更新",
                            data = null
                        });
                    }

                    else
                    {
                        return base.JsonActionResult(new StandardResult<string>()
                        {
                            code = 0,
                            msg = "实名验证通过",
                            data = null
                        });
                    }
                }
            }
            else
            {
                return base.JsonFaultResult(new CommonException(40201).GetMessage(), request.ToString());
            }

            return base.JsonFaultResult(new FaultInfo(40999, "修改失败"), request.ToString());
        }


        [HttpPost]
        public IHttpActionResult ReferralRequest(JObject request)
        {
            Logger.WriterLogger("Member.ReferralRequest, Params: " + request.ToString(), LoggerType.Info);

            ParamReferral param = new ParamReferral();

            try
            {
                param = request.ToObject<ParamReferral>();
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
            string realName = param.RealName;
            string email = param.Email;
            string cellphone = param.Mobile;
            string referralReason = param.ReferralReason;//申请理由

            int channel = param.channel;
            int platform = param.platform;
            string ver = param.ver;

            // 保存访问信息
            base.SaveVisitInfo(userId, channel, platform, ver);

            Member member = GetMember(userId.ToSeesionId());

            if (member != null)
            {
                bool ret = EcShop.SaleSystem.Member.MemberProcessor.ReferralRequest(member.UserId, realName, cellphone, referralReason, email);

                if (ret)
                {
                    StandardResult<string> result = new StandardResult<string>()
                    {
                        code = 0,
                        msg = "申请信息提交成功",
                        data = ""
                    };

                    return base.JsonActionResult(result);
                }
            }
            else
            {
                return base.JsonFaultResult(new CommonException(40201).GetMessage(), request.ToString());
            }

            return base.JsonFaultResult(new FaultInfo(40999, "申请信息提交失败"), request.ToString());
        }



        [HttpGet]
        public IHttpActionResult MyPoints(string userId, int pageIndex, int pageSize, string accessToken, int channel, int platform, string ver)
        {
            Logger.WriterLogger("Member.MyPoints, Params: " + string.Format("userId={0}&accessToken={1}&channel={2}&platform={3}&ver={4}&pageIndex={5}&pageSize={6}", userId, accessToken, channel, platform, ver, pageIndex, pageSize), LoggerType.Info);

            // 保存访问信息
            base.SaveVisitInfo(userId, channel, platform, ver);

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), "Member.MyPoints");
            }

            List<PointListItem> items = new List<PointListItem>();
            int points = 0;
            int availablePoints = 0;

            Member member = GetMember(userId.ToSeesionId());

            if (member != null)
            {
                points = member.Points;
                availablePoints = member.Points;

                DbQueryResult dbResult = TradeHelper.GetUserPoints(member.UserId, pageIndex, pageSize);

                DataTable dt = dbResult.Data as DataTable;

                if (dt != null)
                {
                    PointListItem item = null;

                    foreach (DataRow current in dt.Rows)
                    {
                        item = new PointListItem();

                        item.JournalNumber = 0;
                        if (current["JournalNumber"] != DBNull.Value)
                        {
                            item.JournalNumber = (long)current["JournalNumber"];
                        }

                        item.TradeType = 0;
                        if (current["TradeType"] != DBNull.Value)
                        {
                            item.TradeType = (int)current["TradeType"];
                        }

                        item.OrderId = "";
                        if (current["OrderId"] != DBNull.Value)
                        {
                            item.OrderId = (string)current["OrderId"];
                        }

                        item.TradeDate = "";
                        if (current["TradeDate"] != DBNull.Value)
                        {
                            item.TradeDate = ((DateTime)current["TradeDate"]).ToString("yyyy-MM-dd HH:mm:ss");
                        }

                        item.Increased = 0;
                        if (current["Increased"] != DBNull.Value)
                        {
                            item.Increased = (int)current["Increased"];
                        }

                        item.Reduced = 0;
                        if (current["Reduced"] != DBNull.Value)
                        {
                            item.Reduced = (int)current["Reduced"];
                        }

                        item.Points = 0;
                        if (current["Points"] != DBNull.Value)
                        {
                            item.Points = (int)current["Points"];
                        }

                        item.Remark = "";
                        if (current["Remark"] != DBNull.Value)
                        {
                            item.Remark = (string)current["Remark"];
                        }

                        items.Add(item);
                    }
                }
            }

            PointListResult data = new PointListResult();
            data.Points = points;
            data.AvailablePoints = availablePoints;
            data.TotalNumOfRecords = items.Count; ;
            data.Results = items;

            return base.JsonActionResult(new StandardResult<PointListResult>()
            {
                code = 0,
                msg = "我的积分列表",
                data = data
            });
        }

        [HttpGet]
        public IHttpActionResult MyRecharge(string userId, int tradeType, int pageIndex, int pageSize, string accessToken, int channel, int platform, string ver)
        {
            Logger.WriterLogger("Member.MyRecharge, Params: " + string.Format("userId={0}&accessToken={1}&channel={2}&platform={3}&ver={4}&pageIndex={5}&pageSize={6}&tradeType={7}", userId, accessToken, channel, platform, ver, pageIndex, pageSize, tradeType), LoggerType.Info);

            // 保存访问信息
            base.SaveVisitInfo(userId, channel, platform, ver);

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), "Member.MyRecharge");
            }

            int rows = 0;

            List<RechargeListItem> items = new List<RechargeListItem>();
            decimal balance = 0M;

            Member member = GetMember(userId.ToSeesionId());

            if (member != null)
            {
                balance = member.Balance;

                BalanceDetailQuery balanceDetailQuery = new BalanceDetailQuery();
                balanceDetailQuery.UserId = member.UserId;

                if (tradeType > 0)
                {
                    balanceDetailQuery.TradeType = (TradeTypes)tradeType;
                }
                balanceDetailQuery.PageIndex = pageIndex;
                balanceDetailQuery.PageSize = pageSize;

                DbQueryResult balanceDetails = EcShop.SaleSystem.Member.MemberProcessor.GetBalanceDetails(balanceDetailQuery);

                rows = balanceDetails.TotalRecords;
                DataTable dt = balanceDetails.Data as DataTable;

                if (dt != null)
                {
                    RechargeListItem item = null;

                    foreach (DataRow current in dt.Rows)
                    {
                        item = new RechargeListItem();

                        item.JournalNumber = 0;
                        if (current["JournalNumber"] != DBNull.Value)
                        {
                            item.JournalNumber = (long)current["JournalNumber"];
                        }

                        item.TradeType = 0;
                        if (current["TradeType"] != DBNull.Value)
                        {
                            item.TradeType = (int)current["TradeType"];
                        }

                        item.InpourId = "";
                        if (current["InpourId"] != DBNull.Value)
                        {
                            item.InpourId = (string)current["InpourId"];
                        }

                        item.TradeDate = "";
                        if (current["TradeDate"] != DBNull.Value)
                        {
                            item.TradeDate = ((DateTime)current["TradeDate"]).ToString("yyyy-MM-dd HH:mm:ss");
                        }

                        item.Income = 0;
                        if (current["Income"] != DBNull.Value)
                        {
                            item.Income = (decimal)current["Income"];
                        }

                        item.Expenses = 0;
                        if (current["Expenses"] != DBNull.Value)
                        {
                            item.Expenses = (decimal)current["Expenses"];
                        }

                        item.Balance = 0;
                        if (current["Balance"] != DBNull.Value)
                        {
                            item.Balance = (decimal)current["Balance"];
                        }

                        item.RechargeWay = "";
                        if (current["Remark"] != DBNull.Value)
                        {
                            item.RechargeWay = (string)current["Remark"];
                        }

                        items.Add(item);
                    }
                }
            }

            RechargeListResult data = new RechargeListResult();
            data.Balance = balance;
            data.TotalNumOfRecords = rows;
            data.Results = items;

            return base.JsonActionResult(new StandardResult<RechargeListResult>()
            {
                code = 0,
                msg = "我的充值列表",
                data = data
            });
        }

        #region 弃用代码
        //[HttpGet]
        //public IHttpActionResult MyFavorites(string userId, string accessToken, int channel, int platform, string ver)
        //{
        //    Logger.WriterLogger("Member.MyFavorites, Params: " + string.Format("userId={0}&accessToken={1}&channel={2}&platform={3}&ver={4}", userId, accessToken, channel, platform, ver), LoggerType.Info);

        //    // 保存访问信息
        //    base.SaveVisitInfo(userId, channel, platform, ver);

        //    // 验证令牌
        //    int accessTookenCode = VerifyAccessToken(accessToken);
        //    if (accessTookenCode > 0)
        //    {
        //        return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), "Member.MyFavorites");
        //    }

        //    List<FavoriteListItem> items = new List<FavoriteListItem>();

        //    Member member = GetMember(userId.ToSeesionId());

        //    if (member != null)
        //    {
        //        DataTable dt = ProductBrowser.GetFavorites(member);

        //        if (dt != null)
        //        {
        //            FavoriteListItem item = null;

        //            foreach (DataRow current in dt.Rows)
        //            {
        //                item = new FavoriteListItem();

        //                item.Id = 0;
        //                if (current["FavoriteId"] != DBNull.Value)
        //                {
        //                    item.Id = (int)current["FavoriteId"];
        //                }

        //                item.ProductId = 0;
        //                if (current["ProductId"] != DBNull.Value)
        //                {
        //                    item.ProductId = (int)current["ProductId"];
        //                }

        //                item.Tag = "";
        //                if (current["Tags"] != DBNull.Value)
        //                {
        //                    item.Tag = (string)current["Tags"];
        //                }

        //                item.Remark = "";
        //                if (current["Remark"] != DBNull.Value)
        //                {
        //                    item.Remark = (string)current["Remark"];
        //                }

        //                item.ProductName = "";
        //                if (current["ProductName"] != DBNull.Value)
        //                {
        //                    item.ProductName = (string)current["ProductName"];
        //                }

        //                item.ThumbnailUrl = "";
        //                if (current["ThumbnailUrl60"] != DBNull.Value)
        //                {
        //                    item.ThumbnailUrl = Util.AppendImageHost((string)current["ThumbnailUrl60"]);
        //                }

        //                item.Description = "";
        //                if (current["ShortDescription"] != DBNull.Value)
        //                {
        //                    item.Description = (string)current["ShortDescription"];
        //                }

        //                item.MarketPrice = 0;
        //                if (current["MarketPrice"] != DBNull.Value)
        //                {
        //                    item.MarketPrice = (decimal)current["MarketPrice"];
        //                }

        //                item.Price = 0;
        //                if (current["SalePrice"] != DBNull.Value)
        //                {
        //                    item.Price = (decimal)current["SalePrice"];
        //                }

        //                item.TaxRate = 0;
        //                if (current["TaxRate"] != DBNull.Value)
        //                {
        //                    item.TaxRate = (decimal)current["TaxRate"];
        //                }

        //                items.Add(item);
        //            }
        //        }
        //    }

        //    ListResult<FavoriteListItem> data = new ListResult<FavoriteListItem>();
        //    data.TotalNumOfRecords = items.Count; ;
        //    data.Results = items;

        //    return base.JsonActionResult(
        //        new StandardResult<ListResult<FavoriteListItem>>()
        //        {
        //            code = 0,
        //            msg = "",
        //            data = data
        //        });
        //}
        #endregion

        [HttpGet]
        public IHttpActionResult MyFavorites(string userId, string accessToken, int channel, int platform, string ver, int pageIndex, int pageSize)
        {
            Logger.WriterLogger("Member.MyFavorites, Params: " + string.Format("userId={0}&accessToken={1}&channel={2}&platform={3}&ver={4}&pageIndex={4}&pageSize={5}", userId, accessToken, channel, platform, ver, pageIndex, pageSize), LoggerType.Info);

            // 保存访问信息
            base.SaveVisitInfo(userId, channel, platform, ver);

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), "Member.MyFavorites");
            }

            List<FavoriteListItem> items = new List<FavoriteListItem>();

            Member member = GetMember(userId.ToSeesionId());

            if (member != null)
            {

                ProductFavoriteQuery query = new ProductFavoriteQuery();

                query.PageIndex = pageIndex;

                query.PageSize = pageSize;

                query.UserId = member.UserId;

                query.GradeId = member.GradeId;
                DbQueryResult dr = ProductBrowser.GetFavorites(query);

                DataTable dt = dr.Data as DataTable;

                if (dt != null)
                {
                    FavoriteListItem item = null;

                    foreach (DataRow current in dt.Rows)
                    {
                        item = new FavoriteListItem();

                        item.Id = 0;
                        if (current["FavoriteId"] != DBNull.Value)
                        {
                            item.Id = (int)current["FavoriteId"];
                        }

                        item.ProductId = 0;
                        if (current["ProductId"] != DBNull.Value)
                        {
                            item.ProductId = (int)current["ProductId"];
                        }

                        item.Tag = "";
                        if (current["Tags"] != DBNull.Value)
                        {
                            item.Tag = (string)current["Tags"];
                        }

                        item.Remark = "";
                        if (current["Remark"] != DBNull.Value)
                        {
                            item.Remark = (string)current["Remark"];
                        }

                        item.ProductName = "";
                        if (current["ProductName"] != DBNull.Value)
                        {
                            item.ProductName = (string)current["ProductName"];
                        }

                        item.ThumbnailUrl = "";
                        if (current["ThumbnailUrl220"] != DBNull.Value)
                        {
                            item.ThumbnailUrl = Util.AppendImageHost((string)current["ThumbnailUrl220"]);
                        }

                        item.Description = "";
                        if (current["ShortDescription"] != DBNull.Value)
                        {
                            item.Description = (string)current["ShortDescription"];
                        }

                        item.MarketPrice = 0;
                        if (current["MarketPrice"] != DBNull.Value)
                        {
                            item.MarketPrice = (decimal)current["MarketPrice"];
                        }

                        item.Price = 0;
                        if (current["SalePrice"] != DBNull.Value)
                        {
                            item.Price = (decimal)current["SalePrice"];
                        }

                        item.TaxRate = 0;
                        if (current["TaxRate"] != DBNull.Value)
                        {
                            item.TaxRate = (decimal)current["TaxRate"];
                        }

                        items.Add(item);
                    }
                }


                ProductCollectResult data = new ProductCollectResult();
                data.SupplierCollectCount = SupplierHelper.GetUserSupplierCollectCount(member.UserId);
                data.ProductCollectCount = dr.TotalRecords;
                data.Results = items;

                return base.JsonActionResult(new StandardResult<ProductCollectResult>()
                {
                    code = 0,
                    msg = "商品收藏列表",
                    data = data
                });
            }


            else
            {
                return base.JsonFaultResult(new CommonException(40201).GetMessage(), "Member.MyFavorites");
            }

        }

        [HttpGet]
        public IHttpActionResult MyCoupons(string userId, int useType, string accessToken, int channel, int platform, string ver)
        {
            Logger.WriterLogger("Member.MyCoupons, Params: " + string.Format("userId={0}&accessToken={1}&channel={2}&platform={3}&ver={4}&useType={5}", userId, accessToken, channel, platform, ver, useType), LoggerType.Info);

            // 保存访问信息
            base.SaveVisitInfo(userId, channel, platform, ver);

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), "Member.MyCoupons");
            }

            List<CouponListItem> items = new List<CouponListItem>();

            Member member = GetMember(userId.ToSeesionId());

            if (member != null)
            {
                DataTable userCoupons = EcShop.SaleSystem.Vshop.MemberProcessor.GetUserCoupons(member.UserId, useType);

                if (userCoupons != null)
                {
                    CouponListItem item = null;

                    foreach (DataRow current in userCoupons.Rows)
                    {
                        item = new CouponListItem();

                        item.Code = "";
                        if (current["ClaimCode"] != DBNull.Value)
                        {
                            item.Code = (string)current["ClaimCode"];
                        }

                        item.Name = "";
                        if (current["Name"] != DBNull.Value)
                        {
                            item.Name = (string)current["Name"];
                        }

                        item.Amount = 0;
                        if (current["Amount"] != DBNull.Value)
                        {
                            item.Amount = (decimal)current["Amount"];
                        }

                        item.DiscountValue = 0;
                        if (current["DiscountValue"] != DBNull.Value)
                        {
                            item.DiscountValue = (decimal)current["DiscountValue"];
                        }

                        item.StartTime = "";
                        if (current["StartTime"] != DBNull.Value)
                        {
                            item.StartTime = ((DateTime)current["StartTime"]).ToString("yyyy-MM-dd");
                        }

                        item.ClosingTime = "";
                        if (current["ClosingTime"] != DBNull.Value)
                        {
                            item.ClosingTime = ((DateTime)current["ClosingTime"]).ToString("yyyy-MM-dd");
                        }

                        item.CouponStatus = 0;
                        if (current["CouponStatus"] != DBNull.Value)
                        {
                            item.CouponStatus = (int)current["CouponStatus"];
                        }
                        item.IsExpired = true;
                        if (current["ClosingTime"] != null && !string.IsNullOrWhiteSpace(current["ClosingTime"].ToString()))
                        {
                            item.IsExpired = (DateTime)current["ClosingTime"] > DateTime.Now ? false : true;
                        }

                        items.Add(item);
                    }
                }
            }

            ListResult<CouponListItem> data = new ListResult<CouponListItem>();
            data.TotalNumOfRecords = items.Count; ;
            data.Results = items;

            return base.JsonActionResult(new StandardResult<ListResult<CouponListItem>>()
            {
                code = 0,
                msg = "",
                data = data
            });
        }

        [HttpGet]
        public IHttpActionResult MyInvoices(string userId, string accessToken, int channel, int platform, string ver)
        {
            Logger.WriterLogger("Member.MyInvoices, Params: " + string.Format("userId={0}&accessToken={1}&channel={2}&platform={3}&ver={4}", userId, accessToken, channel, platform, ver), LoggerType.Info);

            // TODO

            // 保存访问信息
            base.SaveVisitInfo(userId, channel, platform, ver);

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), "Member.MyInvoices");
            }

            List<InvoiceListItem> items = new List<InvoiceListItem>();

            Member member = GetMember(userId.ToSeesionId());

            InvoiceListItem item = null;

            item = new InvoiceListItem();
            item.Title = "不要发票";
            item.InvoiceType = 0;
            items.Add(item);

            item = new InvoiceListItem();
            item.Title = "个人";
            item.InvoiceType = 1;
            items.Add(item);

            if (member != null)
            {
                DataTable dt = EcShop.SaleSystem.Member.MemberProcessor.GetUserInvoices(member.UserId);

                if (dt != null)
                {

                    foreach (DataRow current in dt.Rows)
                    {
                        item = new InvoiceListItem();

                        item.Title = "";
                        if (current["InvoiceTitle"] != DBNull.Value)
                        {
                            item.Title = current["InvoiceTitle"].ToString();
                        }
                        item.InvoiceType = 2;

                        if (item.Title != "")
                        {
                            items.Add(item);
                        }
                    }
                }
            }

            ListResult<InvoiceListItem> data = new ListResult<InvoiceListItem>();
            data.TotalNumOfRecords = items.Count; ;
            data.Results = items;

            return base.JsonActionResult(new StandardResult<ListResult<InvoiceListItem>>()
            {
                code = 0,
                msg = "",
                data = data
            });
        }

        [HttpGet]
        public IHttpActionResult Summary(string userId, string accessToken, int channel, int platform, string ver)
        {
            Logger.WriterLogger("Member.Summary, Params: " + string.Format("userId={0}&accessToken={1}&channel={2}&platform={3}&ver={4}", userId, accessToken, channel, platform, ver), LoggerType.Info);

            // 保存访问信息
            base.SaveVisitInfo(userId, channel, platform, ver);

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), "Member.Summary");
            }

            MemberSummaryResult result = new MemberSummaryResult();

            Member member = GetMember(userId.ToSeesionId());

            if (member != null)
            {
                result.Username = member.Username;
                MemberGradeInfo memberGrade = EcShop.SaleSystem.Member.MemberProcessor.GetMemberGrade(member.GradeId);
                if (memberGrade != null)
                {
                    result.Grade = memberGrade.Name;
                }

                OrderQuery orderQuery = new OrderQuery();

                //购物车商品数量
                ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetShoppingCart(member);
                int quantity = 0;
                if (shoppingCart != null)
                {
                    quantity = shoppingCart.GetQuantity();
                }
                result.CartItemCount = quantity;
                result.Balance = member.Balance;
                result.Expenditure = member.Expenditure;
                result.Points = member.Points;
                orderQuery.Status = OrderStatus.All;
                result.OrderCount = EcShop.SaleSystem.Member.MemberProcessor.AppGetUserOrderCount(member.UserId, orderQuery);
                orderQuery.Status = OrderStatus.WaitBuyerPay;
                result.WaitPayOrderCount = EcShop.SaleSystem.Member.MemberProcessor.AppGetUserOrderCount(member.UserId, orderQuery);
                orderQuery.Status = OrderStatus.SellerAlreadySent;
                result.AlreadySentOrderCount = EcShop.SaleSystem.Member.MemberProcessor.AppGetUserOrderCount(member.UserId, orderQuery);

                //已付款，待发货
                orderQuery.Status = OrderStatus.BuyerAlreadyPaid;
                result.WaitSentOrderCount = EcShop.SaleSystem.Member.MemberProcessor.AppGetUserOrderCount(member.UserId, orderQuery);

                //orderQuery.Status = OrderStatus.Finished;
                result.FinishOrderCount = EcShop.SaleSystem.Member.MemberProcessor.GetWaitCommentOrderCount(member.UserId);


                result.RefundCount = EcShop.SaleSystem.Member.MemberProcessor.GetRefundCount(member.UserId);
                result.ReturnCount = EcShop.SaleSystem.Member.MemberProcessor.GetReturnCount(member.UserId);
                result.ReplacementCount = EcShop.SaleSystem.Member.MemberProcessor.GetReplaceCount(member.UserId); ;
                result.UnreadCouponCount = EcShop.SaleSystem.Member.MemberProcessor.GetUserNotReadCoupons(member.UserId);
                result.UnreadVoucherCount = EcShop.SaleSystem.Member.MemberProcessor.GetUserNotReadVouchers(member.UserId);

                string avatar = UserHelper.GetUserAvatar(member.UserId);
                result.Avatar = Util.AppendImageHost(avatar);

                result.ReferralStatus = member.ReferralStatus;//推广员；0、未申请；1、申请中（未审核）；2、审核通过；3、审核未通过
                return base.JsonActionResult(new StandardResult<MemberSummaryResult>()
                {
                    code = 0,
                    msg = "",
                    data = result
                });
            }

            else
            {
                return base.JsonFaultResult(new CommonException(40201).GetMessage(), "Member.Summary");
            }
        }

        [HttpPost]
        public IHttpActionResult UpdateAvatar(JObject request)
        {
            Logger.WriterLogger("Member.UpdateAvatar, Params: " + request.ToString(), LoggerType.Info);

            ParamAvatar param = new ParamAvatar();

            try
            {
                param = request.ToObject<ParamAvatar>();
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

            // 验证参数
            //ThrowParamException(skuId);

            string userId = param.UserId;
            string avatar = param.Avatar;

            int channel = param.channel;
            int platform = param.platform;
            string ver = param.ver;

            // 保存访问信息
            base.SaveVisitInfo(userId, channel, platform, ver);

            Member member = GetMember(userId.ToSeesionId());

            if (member != null)
            {
                bool ret = Users.UpdateUserAvatar(member.UserId, avatar);

                if (ret)
                {
                    StandardResult<string> result = new StandardResult<string>()
                    {
                        code = 0,
                        msg = "会员信息已更新",
                        data = ""
                    };

                    return base.JsonActionResult(result);
                }
            }
            else
            {
                return base.JsonFaultResult(new CommonException(40201).GetMessage(), request.ToString());
            }

            return base.JsonFaultResult(new FaultInfo(40999, ""), request.ToString());
        }


        [HttpPost]
        public Task<IHttpActionResult> UploadAvatar()
        {
            // 检查是否是 multipart/form-data 
            if (!Request.Content.IsMimeMultipartContent("form-data"))
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

            //文件保存目录路径 
            string avatarConfigPath = System.Configuration.ConfigurationManager.AppSettings["AVATAR_PATH"];
            string avatarPath = HttpContext.Current.Server.MapPath(avatarConfigPath);

            // 设置上传目录 
            var provider = new MultipartFormDataStreamProvider(avatarPath);
            //var queryp = Request.GetQueryNameValuePairs();//获得查询字符串的键值集合 

            var task = Request.Content.ReadAsMultipartAsync(provider).
                ContinueWith<IHttpActionResult>(o =>
                {
                    bool isSuccess = false;
                    string msg = "上传出错";

                    //ParamUserBase param = new ParamUserBase();

                    //try
                    //{
                    //    param = request.ToObject<ParamUserBase>();
                    //}
                    //catch
                    //{
                    //    // 参数无效
                    //    return base.JsonFaultResult(new CommonException(40100).GetMessage(), request.ToString());
                    //}

                    //string userId = param.UserId;

                    string avatar = "";

                    if (provider.FileData.Count == 0)
                    {
                        msg = "未选择上传的文件";
                    }
                    else
                    {
                        var file = provider.FileData[0];//provider.FormData 

                        string orfilename = file.Headers.ContentDisposition.FileName.TrimStart('"').TrimEnd('"');

                        FileInfo fileinfo = new FileInfo(file.LocalFileName);

                        //最大文件大小 
                        int maxSize = 1000000;
                        int.TryParse(System.Configuration.ConfigurationManager.AppSettings["AVATAR_MAX_SIZE"], out maxSize);

                        if (fileinfo.Length <= 0)
                        {
                            msg = "请选择上传文件。";
                        }
                        else if (fileinfo.Length > maxSize)
                        {
                            msg = "上传文件大小超过限制。";
                        }
                        else
                        {
                            string fileExt = orfilename.Substring(orfilename.LastIndexOf('.'));

                            //定义允许上传的文件扩展名 
                            String fileTypes = "gif,jpg,jpeg,png,bmp";

                            if (String.IsNullOrEmpty(fileExt) || Array.IndexOf(fileTypes.Split(','), fileExt.Substring(1).ToLower()) == -1)
                            {
                                msg = "上传文件扩展名是不允许的扩展名。";
                            }
                            else
                            {
                                string ymd = DateTime.Now.ToString("yyyyMM", System.Globalization.DateTimeFormatInfo.InvariantInfo);
                                //string newFileName = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", System.Globalization.DateTimeFormatInfo.InvariantInfo);

                                string path = Path.Combine(avatarPath, ymd);
                                if (!Directory.Exists(path))
                                {
                                    Directory.CreateDirectory(path);
                                }

                                avatar = Guid.NewGuid().ToString().ToLower().Replace("-", "") + fileExt;

                                fileinfo.CopyTo(Path.Combine(path, avatar), true);
                                fileinfo.Delete();

                                //Member member = GetMember(userId.ToSeesionId());

                                //if (member != null)
                                //{
                                avatar = System.Configuration.ConfigurationManager.AppSettings["AVATAR_URL_BASE"] + ymd + "/" + avatar;

                                //EcShop.SaleSystem.Member.MemberProcessor.UpdateAtavar(member.UserId, avatar);
                                //}

                                isSuccess = true;
                            }
                        }
                    }

                    if (isSuccess)
                    {
                        StandardResult<string> result = new StandardResult<string>();
                        result.code = 0;
                        result.msg = "头像上传成功";
                        result.data = avatar;

                        return base.JsonActionResult(result);
                    }
                    else
                    {
                        StandardResult<string> errorResult = new StandardResult<string>();
                        errorResult.code = 40204;
                        errorResult.msg = msg;

                        return base.JsonActionResult(errorResult);
                    }
                });

            return task;
        }

        [HttpGet]
        public IHttpActionResult MyOrders(string userId, int orderStatus, int pageIndex, int pageSize, string accessToken, int channel, int platform, string ver)
        {
            Logger.WriterLogger("Member.MyOrders, Params: " + string.Format("userId={0}&accessToken={1}&channel={2}&platform={3}&ver={4}&orderStatus={5}&pageIndex={6}&pageSize={7}", userId, accessToken, channel, platform, ver, orderStatus, pageIndex, pageSize), LoggerType.Info);

            // 保存访问信息
            base.SaveVisitInfo(userId, channel, platform, ver);

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), "Member.MyOrders");
            }

            int num = 0;
            List<OrderItemResult> items = new List<OrderItemResult>();

            Member member = GetMember(userId.ToSeesionId());

            if (member != null)
            {
                #region 执行
                OrderQuery orderQuery = new OrderQuery();

                orderQuery.Status = (OrderStatus)orderStatus;
                orderQuery.PageIndex = pageIndex;
                orderQuery.PageSize = pageSize;
                orderQuery.UserId = member.UserId;
                orderQuery.SortOrder = Core.Enums.SortAction.Desc;
                orderQuery.SortBy = "OrderDate";

                Globals.EntityCoding(orderQuery, true);


                //app不做修改，这里将完成的订单筛选为待评论的订单
                if (orderQuery.Status == OrderStatus.Finished)
                {
                    orderQuery.WaitToComment = true;
                }
                int delaytime = string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["OrderRefunTime"]) ? 30 : int.Parse(System.Configuration.ConfigurationManager.AppSettings["OrderRefunTime"].ToString());

                DataSet tradeOrders = OrderHelper.GetTradeOrders(orderQuery, out num, delaytime);

                Dictionary<string, int> orderIds = new Dictionary<string, int>();

                foreach (DataRow row in tradeOrders.Tables[0].Rows)
                {
                    orderIds.Add(row["OrderId"].ToString(), 0);
                }

                string orderList = "";

                foreach (var current in orderIds)
                {
                    orderList += string.Format("{0}{1}", orderList.Length > 0 ? "," : "", current.Key);
                }

                DataTable productReviewAll = ProductBrowser.GetOrderReviewAll(orderList, member.UserId);
                if (productReviewAll != null)
                {
                    foreach (DataRow current in productReviewAll.Rows)
                    {
                        string orderId = current["OrderId"].ToString();
                        int skuQuantity = (int)current["SkuQuantity"];

                        if (orderIds.ContainsKey(orderId))
                        {
                            orderIds[orderId] = skuQuantity;
                        }
                    }
                }

                #region 组装数据
                foreach (DataRow row in tradeOrders.Tables[0].Rows)
                {
                    OrderItemResult item = new OrderItemResult();

                    item.OrderId = row["OrderId"].ToString();
                    item.OrderDate = ((DateTime)row["OrderDate"]).ToString("yyyy-MM-dd HH:mm:ss");
                    item.OrderStatus = (int)row["OrderStatus"];
                    item.OrderSource = (int)row["SourceOrder"];
                    item.Address = row["ShippingRegion"].ToString() + " " + row["Address"].ToString() + " " + row["ZipCode"].ToString();
                    item.Reciver = row["ShipTo"].ToString();
                    item.Telephone = row["TelPhone"].ToString();
                    item.Cellphone = row["CellPhone"].ToString();
                    item.ShipToDate = row["ShipToDate"].ToString();
                    item.ShippingDate = "";
                    if (row["ShippingDate"] != DBNull.Value)
                    {
                        item.ShippingDate = ((DateTime)row["ShippingDate"]).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    item.ShippingModeName = row["ModeName"].ToString();
                    item.PaymentTypeId = int.Parse(row["PaymentTypeId"].ToString());
                    item.PaymentTypeName = row["PaymentType"].ToString();
                    item.IsNeedInvoice = !string.IsNullOrWhiteSpace(row["InvoiceTitle"].ToString());
                    item.InvoiceTitle = row["InvoiceTitle"].ToString();
                    item.BuyQuantity = int.Parse(row["Nums"].ToString());
                    item.Total = decimal.Parse(row["OrderTotal"].ToString());
                    //item.Amount = decimal.Parse(row["Amount"].ToString());
                    item.Amount = item.Total;


                    item.PayCharge = decimal.Parse(row["PayCharge"].ToString());
                    item.Freight = decimal.Parse(row["AdjustedFreight"].ToString());
                    item.Tax = decimal.Parse(row["Tax"].ToString());
                    item.Discount = decimal.Parse(row["AdjustedDiscount"].ToString());
                    item.Point = int.Parse(row["OrderPoint"].ToString());
                    item.Gateway = row["Gateway"].ToString();
                    if (row["GatewayOrderId"] != DBNull.Value)
                    {
                        item.GatewayOrderId = row["GatewayOrderId"].ToString();
                    }
                    item.Remark = row["Remark"].ToString();

                    item.SourceOrderId = row["SourceOrderId"].ToString();
                    item.IsRefund =int.Parse(row["IsRefund"].ToString());
                    item.IsCancelOrder = int.Parse(row["IsCancelOrder"].ToString());

                    List<OrderSkuItem> skuItems = new List<OrderSkuItem>();

                    DataRow[] childRows = row.GetChildRows("OrderRelation");

                    item.IsCanReview = true;
                    if (orderIds.ContainsKey(item.OrderId))
                    {
                        item.IsCanReview = orderIds[item.OrderId] < childRows.Length;
                    }

                    for (int i = 0; i < childRows.Length; i++)
                    {
                        DataRow skuRow = childRows[i];
                        string skuContent = Globals.HtmlEncode(skuRow["SKUContent"].ToString());
                        string thumbnailsUrl = Util.AppendImageHost(skuRow["ThumbnailsUrl"].ToString());
                        if (!string.IsNullOrEmpty(thumbnailsUrl))
                        {
                            thumbnailsUrl = thumbnailsUrl.Replace("40/40", "180/180");
                        }

                        OrderSkuItem skuItem = new OrderSkuItem();

                        skuItem.SkuId = skuRow["SkuId"].ToString();
                        skuItem.ProductId = int.Parse(skuRow["ProductId"].ToString());
                        skuItem.SkuCode = skuRow["SKU"].ToString();
                        skuItem.Quantity = int.Parse(skuRow["Quantity"].ToString());
                        skuItem.Price = decimal.Parse(skuRow["ItemListPrice"].ToString());
                        skuItem.AdjustedPrice = decimal.Parse(skuRow["ItemAdjustedPrice"].ToString());
                        skuItem.Description = skuRow["ItemDescription"].ToString();
                        skuItem.ThumbnailsUrl = thumbnailsUrl;
                        skuItem.Weight = decimal.Parse(skuRow["ItemWeight"].ToString());
                        skuItem.SkuContent = skuContent;
                        skuItem.PromotionName = skuRow["PromotionName"].ToString();
                        skuItem.TaxRate = decimal.Parse(skuRow["TaxRate"].ToString());
                        skuItem.SubTotal = decimal.Parse(skuRow["ItemAdjustedPrice"].ToString()) * int.Parse(skuRow["Quantity"].ToString());

                        skuItem.ShareUrl = string.Format(base.PRODUCT_SHARE_URL_BASE, skuItem.ProductId);

                        skuItems.Add(skuItem);
                    }

                    item.Items = skuItems;

                    items.Add(item);
                }
                #endregion

                #endregion
            }
            OrderQuery orderQuery1 = new OrderQuery();

            orderQuery1.Status = OrderStatus.BuyerAlreadyPaid;

            int nonDeliveryCount = EcShop.SaleSystem.Member.MemberProcessor.AppGetUserOrderCount(member.UserId, orderQuery1);

            orderQuery1.Status = OrderStatus.SellerAlreadySent;

            int deliveryCount = EcShop.SaleSystem.Member.MemberProcessor.AppGetUserOrderCount(member.UserId, orderQuery1);

            MyOrderResult<OrderItemResult> data = new MyOrderResult<OrderItemResult>();
            data.TotalNumOfRecords = num;
            data.Results = items;
            data.NonDeliveryCount = nonDeliveryCount;
            data.DeliveryCount = deliveryCount;


            StandardResult<MyOrderResult<OrderItemResult>> result = new StandardResult<MyOrderResult<OrderItemResult>>()
            {
                code = 0,
                msg = "",
                data = data
            };

            return base.JsonActionResult(result);

        }


        [HttpGet]
        public IHttpActionResult MyOrdersV2(string userId, int orderStatus, int pageIndex, int pageSize, string accessToken, int channel, int platform, string ver)
        {
            Logger.WriterLogger("Member.MyOrders, Params: " + string.Format("userId={0}&accessToken={1}&channel={2}&platform={3}&ver={4}&orderStatus={5}&pageIndex={6}&pageSize={7}", userId, accessToken, channel, platform, ver, orderStatus, pageIndex, pageSize), LoggerType.Info);

            // 保存访问信息
            base.SaveVisitInfo(userId, channel, platform, ver);

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), "Member.MyOrders");
            }

            int num = 0;
            List<OrderItemResult> items = new List<OrderItemResult>();

            Member member = GetMember(userId.ToSeesionId());

            if (member != null)
            {
                OrderQuery orderQuery = new OrderQuery();

                orderQuery.Status = (OrderStatus)orderStatus;
                orderQuery.PageIndex = pageIndex;
                orderQuery.PageSize = pageSize;
                orderQuery.UserId = member.UserId;
                orderQuery.SortOrder = Core.Enums.SortAction.Desc;
                orderQuery.SortBy = "OrderDate";

                Globals.EntityCoding(orderQuery, true);


                //app不做修改，这里将完成的订单筛选为待评论的订单
                if (orderQuery.Status == OrderStatus.Finished)
                {
                    orderQuery.WaitToComment = true;
                }
                int delaytime = string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["OrderRefunTime"]) ? 30 : int.Parse(System.Configuration.ConfigurationManager.AppSettings["OrderRefunTime"].ToString());
                DataSet tradeOrders = OrderHelper.GetTradeOrders(orderQuery, out num, delaytime);


                Dictionary<string, int> orderIds = new Dictionary<string, int>();

                foreach (DataRow row in tradeOrders.Tables[0].Rows)
                {
                    orderIds.Add(row["OrderId"].ToString(), 0);
                }

                string orderList = "";

                foreach (var current in orderIds)
                {
                    orderList += string.Format("{0}{1}", orderList.Length > 0 ? "," : "", current.Key);
                }

                DataTable productReviewAll = ProductBrowser.GetOrderReviewAll(orderList, member.UserId);
                if (productReviewAll != null)
                {
                    foreach (DataRow current in productReviewAll.Rows)
                    {
                        string orderId = current["OrderId"].ToString();
                        int skuQuantity = (int)current["SkuQuantity"];

                        if (orderIds.ContainsKey(orderId))
                        {
                            orderIds[orderId] = skuQuantity;
                        }
                    }
                }

                foreach (DataRow row in tradeOrders.Tables[0].Rows)
                {
                    OrderItemResult item = new OrderItemResult();

                    item.OrderId = row["OrderId"].ToString();
                    item.OrderDate = ((DateTime)row["OrderDate"]).ToString("yyyy-MM-dd HH:mm:ss");
                    item.OrderStatus = (int)row["OrderStatus"];
                    item.OrderSource = (int)row["SourceOrder"];
                    item.Address = row["ShippingRegion"].ToString() + " " + row["Address"].ToString() + " " + row["ZipCode"].ToString();
                    item.Reciver = row["ShipTo"].ToString();
                    item.Telephone = row["TelPhone"].ToString();
                    item.Cellphone = row["CellPhone"].ToString();
                    item.ShipToDate = row["ShipToDate"].ToString();
                    item.ShippingDate = "";
                    if (row["ShippingDate"] != DBNull.Value)
                    {
                        item.ShippingDate = ((DateTime)row["ShippingDate"]).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    item.ShippingModeName = row["ModeName"].ToString();
                    item.PaymentTypeId = int.Parse(row["PaymentTypeId"].ToString());
                    item.PaymentTypeName = row["PaymentType"].ToString();
                    item.IsNeedInvoice = !string.IsNullOrWhiteSpace(row["InvoiceTitle"].ToString());
                    item.InvoiceTitle = row["InvoiceTitle"].ToString();
                    item.BuyQuantity = int.Parse(row["Nums"].ToString());
                    item.Total = decimal.Parse(row["OrderTotal"].ToString());
                    item.Amount = decimal.Parse(row["Amount"].ToString());
                    item.PayCharge = decimal.Parse(row["PayCharge"].ToString());
                    item.Freight = decimal.Parse(row["AdjustedFreight"].ToString());
                    item.Tax = decimal.Parse(row["Tax"].ToString());
                    item.Discount = decimal.Parse(row["AdjustedDiscount"].ToString());
                    item.Point = int.Parse(row["OrderPoint"].ToString());
                    item.Gateway = row["Gateway"].ToString();
                    if (row["GatewayOrderId"] != DBNull.Value)
                    {
                        item.GatewayOrderId = row["GatewayOrderId"].ToString();
                    }
                    item.Remark = row["Remark"].ToString();
                    item.SourceOrderId = row["SourceOrderId"].ToString();
                    item.IsRefund = int.Parse(row["IsRefund"].ToString());
                    item.IsCancelOrder = int.Parse(row["IsCancelOrder"].ToString());
                    List<OrderSkuItem> skuItems = new List<OrderSkuItem>();

                    DataRow[] childRows = row.GetChildRows("OrderRelation");

                    item.IsCanReview = true;
                    if (orderIds.ContainsKey(item.OrderId))
                    {
                        item.IsCanReview = orderIds[item.OrderId] < childRows.Length;
                    }

                    for (int i = 0; i < childRows.Length; i++)
                    {
                        DataRow skuRow = childRows[i];
                        string skuContent = Globals.HtmlEncode(skuRow["SKUContent"].ToString());
                        string thumbnailsUrl = Util.AppendImageHost(skuRow["ThumbnailsUrl"].ToString());
                        if (!string.IsNullOrEmpty(thumbnailsUrl))
                        {
                            thumbnailsUrl = thumbnailsUrl.Replace("40/40", "180/180");
                        }

                        OrderSkuItem skuItem = new OrderSkuItem();

                        skuItem.SkuId = skuRow["SkuId"].ToString();
                        skuItem.ProductId = int.Parse(skuRow["ProductId"].ToString());
                        skuItem.SkuCode = skuRow["SKU"].ToString();
                        skuItem.Quantity = int.Parse(skuRow["Quantity"].ToString());
                        skuItem.Price = decimal.Parse(skuRow["ItemListPrice"].ToString());
                        skuItem.AdjustedPrice = decimal.Parse(skuRow["ItemAdjustedPrice"].ToString());
                        skuItem.Description = skuRow["ItemDescription"].ToString();
                        skuItem.ThumbnailsUrl = thumbnailsUrl;
                        skuItem.Weight = decimal.Parse(skuRow["ItemWeight"].ToString());
                        skuItem.SkuContent = skuContent;
                        skuItem.PromotionName = skuRow["PromotionName"].ToString();
                        skuItem.TaxRate = decimal.Parse(skuRow["TaxRate"].ToString());
                        skuItem.SubTotal = decimal.Parse(skuRow["ItemAdjustedPrice"].ToString()) * int.Parse(skuRow["Quantity"].ToString());

                        skuItem.ShareUrl = string.Format(base.PRODUCT_SHARE_URL_BASE, skuItem.ProductId);

                        skuItems.Add(skuItem);
                    }

                    item.Items = skuItems;

                    items.Add(item);
                }
            }

            OrderQuery orderQuery1 = new OrderQuery();

            orderQuery1.Status = OrderStatus.BuyerAlreadyPaid;

            int nonDeliveryCount = EcShop.SaleSystem.Member.MemberProcessor.AppGetUserOrderCount(member.UserId, orderQuery1);

            orderQuery1.Status = OrderStatus.SellerAlreadySent;

            int deliveryCount = EcShop.SaleSystem.Member.MemberProcessor.AppGetUserOrderCount(member.UserId, orderQuery1);

            MyOrderResult<OrderItemResult> data = new MyOrderResult<OrderItemResult>();
            data.TotalNumOfRecords = num;
            data.Results = items;
            data.NonDeliveryCount = nonDeliveryCount;
            data.DeliveryCount = deliveryCount;


            StandardResult<MyOrderResult<OrderItemResult>> result = new StandardResult<MyOrderResult<OrderItemResult>>()
            {
                code = 0,
                msg = "",
                data = data
            };

            return base.JsonActionResult(result);

        }



        [HttpGet]
        public IHttpActionResult MyServiceOrders(string userId, string orderStatus, int pageIndex, int pageSize, string accessToken, int channel, int platform, string ver)
        {
            Logger.WriterLogger("Member.MyOrders, Params: " + string.Format("userId={0}&accessToken={1}&channel={2}&platform={3}&ver={4}&orderStatus={5}&pageIndex={6}&pageSize={7}", userId, accessToken, channel, platform, ver, orderStatus, pageIndex, pageSize), LoggerType.Info);

            // 保存访问信息
            base.SaveVisitInfo(userId, channel, platform, ver);

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), "Member.MyServiceOrders");
            }

            int num = 0;
            List<ServiceOrderItemResult> items = new List<ServiceOrderItemResult>();

            Member member = GetMember(userId.ToSeesionId());

            if (member != null)
            {
                ServiceOrderQuery orderQuery = new ServiceOrderQuery();

                if (string.IsNullOrEmpty(orderStatus))
                {
                    orderQuery.Status.Add((OrderStatus)6);
                    orderQuery.Status.Add((OrderStatus)7);
                    orderQuery.Status.Add((OrderStatus)8);
                    orderQuery.Status.Add((OrderStatus)9);
                    orderQuery.Status.Add((OrderStatus)10);
                }
                else
                {
                    orderQuery.Status = new List<OrderStatus>();

                    string[] orderStatusList = orderStatus.Split(new char[] { ',' });
                    foreach (string current in orderStatusList)
                    {
                        int status = -1;

                        if (int.TryParse(current, out status))
                        {
                            if (status >= 6 && status <= 10)
                            {
                                OrderStatus currentStauts = (OrderStatus)status;
                                if (!(orderQuery.Status.Contains(currentStauts)))
                                {
                                    orderQuery.Status.Add(currentStauts);
                                }
                            }
                        }
                    }

                }

                orderQuery.PageIndex = pageIndex;
                orderQuery.PageSize = pageSize;
                orderQuery.UserId = member.UserId;
                orderQuery.SortOrder = Core.Enums.SortAction.Desc;
                orderQuery.SortBy = "OrderDate";

                Globals.EntityCoding(orderQuery, true);

                DataSet serviceOrders = OrderHelper.GetServiceOrders(orderQuery, out num);

                foreach (DataRow row in serviceOrders.Tables[0].Rows)
                {
                    ServiceOrderItemResult item = new ServiceOrderItemResult();

                    item.OrderId = row["OrderId"].ToString();
                    item.OrderDate = ((DateTime)row["OrderDate"]).ToString("yyyy-MM-dd HH:mm:ss");
                    item.OrderStatus = (int)row["OrderStatus"];
                    item.OrderSource = (int)row["SourceOrder"];
                    item.Address = row["ShippingRegion"].ToString() + " " + row["Address"].ToString() + " " + row["ZipCode"].ToString();
                    item.Reciver = row["ShipTo"].ToString();
                    item.Telephone = row["TelPhone"].ToString();
                    item.Cellphone = row["CellPhone"].ToString();
                    item.ShipToDate = row["ShipToDate"].ToString();
                    item.ShippingDate = "";
                    if (row["ShippingDate"] != DBNull.Value)
                    {
                        item.ShippingDate = ((DateTime)row["ShippingDate"]).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    item.ShippingModeName = row["ModeName"].ToString();
                    item.PaymentTypeId = int.Parse(row["PaymentTypeId"].ToString());
                    item.PaymentTypeName = row["PaymentType"].ToString();
                    item.IsNeedInvoice = !string.IsNullOrWhiteSpace(row["InvoiceTitle"].ToString());
                    item.InvoiceTitle = row["InvoiceTitle"].ToString();
                    item.BuyQuantity = int.Parse(row["Nums"].ToString());
                    item.Total = decimal.Parse(row["OrderTotal"].ToString());
                    item.Amount = decimal.Parse(row["Amount"].ToString());
                    item.PayCharge = decimal.Parse(row["PayCharge"].ToString());
                    item.Freight = decimal.Parse(row["AdjustedFreight"].ToString());
                    item.Tax = decimal.Parse(row["Tax"].ToString());
                    item.Discount = decimal.Parse(row["AdjustedDiscount"].ToString());
                    item.Point = int.Parse(row["OrderPoint"].ToString());
                    //item.Remark = row["Remark"].ToString();
                    item.Gateway = row["Gateway"].ToString();
                    if (row["GatewayOrderId"] != DBNull.Value)
                    {
                        item.GatewayOrderId = row["GatewayOrderId"].ToString();
                    }

                    item.ApplyId = 0;
                    if (row["ApplyId"] != DBNull.Value)
                    {
                        item.ApplyId = int.Parse(row["ApplyId"].ToString());
                    }
                    item.ApplyType = 0;
                    if (row["ApplyType"] != DBNull.Value)
                    {
                        item.ApplyType = int.Parse(row["ApplyType"].ToString());
                    }
                    item.ApplyForTime = "";
                    if (row["ApplyForTime"] != DBNull.Value)
                    {
                        item.ApplyForTime = ((DateTime)row["ApplyForTime"]).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    item.Comments = row["Comments"].ToString();
                    item.AdminRemark = row["AdminRemark"].ToString();
                    item.HandleStatus = 0;
                    if (row["HandleStatus"] != DBNull.Value)
                    {
                        item.HandleStatus = int.Parse(row["HandleStatus"].ToString());
                    }
                    item.HandleTime = "";
                    if (row["HandleTime"] != DBNull.Value)
                    {
                        item.HandleTime = ((DateTime)row["HandleTime"]).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    item.RefundMoney = 0;
                    if (row["RefundMoney"] != DBNull.Value)
                    {
                        item.RefundMoney = decimal.Parse(row["RefundMoney"].ToString());
                    }
                    item.RefundType = 0;
                    if (row["RefundType"] != DBNull.Value)
                    {
                        item.RefundType = int.Parse(row["RefundType"].ToString());
                    }

                    item.ExpressCompany = row["ExpressCompany"].ToString();
                    item.TrackingNumber = row["TrackingNumber"].ToString();

                    List<ServiceOrderSkuItem> skuItems = new List<ServiceOrderSkuItem>();

                    DataRow[] childRows = row.GetChildRows("OrderRelation");

                    for (int i = 0; i < childRows.Length; i++)
                    {
                        DataRow skuRow = childRows[i];
                        string skuContent = Globals.HtmlEncode(skuRow["SKUContent"].ToString());
                        string thumbnailsUrl = Util.AppendImageHost(skuRow["ThumbnailsUrl"].ToString());
                        if (!string.IsNullOrEmpty(thumbnailsUrl))
                        {
                            thumbnailsUrl = thumbnailsUrl.Replace("40/40", "180/180");
                        }

                        ServiceOrderSkuItem skuItem = new ServiceOrderSkuItem();

                        skuItem.SkuId = skuRow["SkuId"].ToString();
                        skuItem.ProductId = int.Parse(skuRow["ProductId"].ToString());
                        skuItem.Quantity = int.Parse(skuRow["Quantity"].ToString());
                        skuItem.Price = decimal.Parse(skuRow["ItemAdjustedPrice"].ToString());
                        skuItem.Description = skuRow["ItemDescription"].ToString();
                        skuItem.ThumbnailsUrl = Util.AppendImageHost(thumbnailsUrl);
                        skuItem.Weight = decimal.Parse(skuRow["ItemWeight"].ToString());
                        skuItem.SkuContent = skuContent;
                        skuItem.PromotionName = skuRow["PromotionName"].ToString();
                        skuItem.TaxRate = decimal.Parse(skuRow["TaxRate"].ToString());
                        skuItem.SubTotal = decimal.Parse(skuRow["ItemAdjustedPrice"].ToString()) * int.Parse(skuRow["Quantity"].ToString());

                        skuItems.Add(skuItem);
                    }

                    item.Items = skuItems;

                    items.Add(item);
                }

            }

            ListResult<ServiceOrderItemResult> data = new ListResult<ServiceOrderItemResult>();
            data.TotalNumOfRecords = num;
            data.Results = items;

            StandardResult<ListResult<ServiceOrderItemResult>> result = new StandardResult<ListResult<ServiceOrderItemResult>>()
            {
                code = 0,
                msg = "",
                data = data
            };

            return base.JsonActionResult(result);

        }

        [HttpPost]
        public IHttpActionResult FinishOrder(JObject request)
        {
            Logger.WriterLogger("Member.FinishOrder, Params: " + request.ToString(), LoggerType.Info);

            ParamFinishOrder param = new ParamFinishOrder();

            try
            {
                param = request.ToObject<ParamFinishOrder>();
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

            // 验证参数
            //ThrowParamException(skuId);

            string userId = param.UserId;
            string orderId = param.OrderId;

            int channel = param.channel;
            int platform = param.platform;
            string ver = param.ver;

            // 保存访问信息
            base.SaveVisitInfo(userId, channel, platform, ver);

            OrderInfo orderInfo = EcShop.SaleSystem.Shopping.ShoppingProcessor.GetOrderInfo(orderId);
            if (orderInfo != null && EcShop.SaleSystem.Vshop.MemberProcessor.ConfirmOrderFinish(orderInfo))
            {
                return base.JsonActionResult(new StandardResult<string>()
                {
                    code = 0,
                    msg = "确认收货完成",
                    data = ""
                });
            }

            return base.JsonActionResult(new StandardResult<string>()
            {
                code = 40309,
                msg = "订单当前状态不允许完成",
                data = ""
            });
        }

        public IHttpActionResult CancelOrder(JObject request)
        {
            Logger.WriterLogger("Member.CancelOrder, Params: " + request.ToString(), LoggerType.Info);

            ParamFinishOrder param = new ParamFinishOrder();

            try
            {
                param = request.ToObject<ParamFinishOrder>();
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

            // 验证参数
            //ThrowParamException(skuId);

            string userId = param.UserId;
            string orderId = param.OrderId;

            int channel = param.channel;
            int platform = param.platform;
            string ver = param.ver;

            // 保存访问信息
            base.SaveVisitInfo(userId, channel, platform, ver);

            OrderInfo orderInfo = EcShop.SaleSystem.Shopping.ShoppingProcessor.GetOrderInfo(orderId);
            if (orderInfo != null && EcShop.SaleSystem.Vshop.MemberProcessor.ConfirmOrderCancel(orderInfo))
            {
                return base.JsonActionResult(new StandardResult<string>()
                {
                    code = 0,
                    msg = "订单已取消",
                    data = ""
                });
            }

            return base.JsonActionResult(new StandardResult<string>()
            {
                code = 40310,
                msg = "订单当前状态不允许取消",
                data = ""
            });
        }

        #endregion

        #region 充值

        public IHttpActionResult RechargeRequest(JObject request)
        {
            Logger.WriterLogger("Member.RecharegRequest, Params: " + request.ToString(), LoggerType.Info);

            ParamRechargeRequest param = new ParamRechargeRequest();

            try
            {
                param = request.ToObject<ParamRechargeRequest>();
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

            // 验证参数
            //ThrowParamException(skuId);

            string userId = param.UserId;
            int paymentTypeId = param.PaymentTypeId;
            decimal amount = param.Amount;

            int channel = param.channel;
            int platform = param.platform;
            string ver = param.ver;

            // 保存访问信息
            base.SaveVisitInfo(userId, channel, platform, ver);

            Member member = GetMember(userId.ToSeesionId());

            if (member == null)
            {
                return base.JsonFaultResult(new CommonException(40201).GetMessage(), request.ToString());
            }

            InpourRequestInfo inpourRequestInfo = new InpourRequestInfo
            {
                InpourId = this.GenerateInpourId(),
                TradeDate = System.DateTime.Now,
                InpourBlance = amount,
                UserId = member.UserId,
                PaymentId = paymentTypeId
            };

            if (EcShop.SaleSystem.Member.MemberProcessor.AddInpourBlance(inpourRequestInfo))
            {
                return base.JsonActionResult(new StandardResult<string>()
                {
                    code = 0,
                    msg = "充值申请成功",
                    data = inpourRequestInfo.InpourId
                });
            }

            return base.JsonFaultResult(new CommonException(40380).GetMessage(), request.ToString());
        }
        public IHttpActionResult Recharge(JObject request)
        {
            Logger.WriterLogger("Member.Recharge, Params: " + request.ToString(), LoggerType.Info);

            ParamRecharge param = new ParamRecharge();

            try
            {
                param = request.ToObject<ParamRecharge>();
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

            // 验证参数
            //ThrowParamException(skuId);

            string userId = param.UserId;
            decimal amount = param.Amount;
            string orderId = param.OrderId;

            int channel = param.channel;
            int platform = param.platform;
            string ver = param.ver;

            // 保存访问信息
            base.SaveVisitInfo(userId, channel, platform, ver);

            Member member = GetMember(userId.ToSeesionId());

            if (member == null)
            {
                return base.JsonFaultResult(new CommonException(40201).GetMessage(), request.ToString());
            }

            InpourRequestInfo inpourRequest = EcShop.SaleSystem.Member.MemberProcessor.GetInpourBlance(orderId);
            if (inpourRequest == null)
            {
                return base.JsonFaultResult(new CommonException(40382).GetMessage(), request.ToString());
            }
            amount = inpourRequest.InpourBlance;
            PaymentModeInfo paymode = TradeHelper.GetPaymentMode(inpourRequest.PaymentId);
            if (paymode == null)
            {
                return base.JsonFaultResult(new CommonException(40396).GetMessage(), request.ToString());
            }

            System.DateTime now = System.DateTime.Now;
            TradeTypes tradeType = TradeTypes.SelfhelpInpour;

            decimal balance = member.Balance + inpourRequest.InpourBlance;

            BalanceDetailInfo balanceDetailInfo = new BalanceDetailInfo();
            balanceDetailInfo.UserId = inpourRequest.UserId;
            balanceDetailInfo.UserName = member.Username;
            balanceDetailInfo.TradeDate = now;
            balanceDetailInfo.TradeType = tradeType;
            balanceDetailInfo.Income = new decimal?(inpourRequest.InpourBlance);
            balanceDetailInfo.Balance = balance;
            balanceDetailInfo.InpourId = inpourRequest.InpourId;

            if (paymode != null)
            {
                balanceDetailInfo.Remark = "充值支付方式：" + paymode.Name;
            }

            if (EcShop.SaleSystem.Member.MemberProcessor.Recharge(balanceDetailInfo))
            {
                return base.JsonActionResult(new StandardResult<string>()
                {
                    code = 0,
                    msg = "充值成功",
                    data = inpourRequest.InpourId
                });
            }

            return base.JsonFaultResult(new CommonException(40381).GetMessage(), request.ToString());
        }

        #endregion

        #region 提现

        [HttpPost]
        public IHttpActionResult WithdrawRequest(JObject request)
        {
            Logger.WriterLogger("Member.WithdrawRequest, Params: " + request.ToString(), LoggerType.Info);

            ParamWithdrawRequest param = new ParamWithdrawRequest();

            try
            {
                param = request.ToObject<ParamWithdrawRequest>();
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

            // 验证参数
            //ThrowParamException(skuId);

            string userId = param.UserId;
            string bankName = param.BankName;
            string accountName = param.AccountName;
            string account = param.Account;
            decimal amount = param.Amount;
            string remark = param.Remark;

            int channel = param.channel;
            int platform = param.platform;
            string ver = param.ver;

            // 保存访问信息
            base.SaveVisitInfo(userId, channel, platform, ver);

            Member member = GetMember(userId.ToSeesionId());

            if (member == null)
            {
                return base.JsonFaultResult(new CommonException(40201).GetMessage(), request.ToString());
            }

            if (member.RequestBalance > 0m)
            {
                //上笔提现管理员还没有处理，只有处理完后才能再次申请提现
                return base.JsonFaultResult(new CommonException(40383).GetMessage(), request.ToString());
            }

            BalanceDrawRequestInfo balanceDrawRequest = new BalanceDrawRequestInfo();
            balanceDrawRequest.UserId = member.UserId;
            balanceDrawRequest.UserName = member.Username;
            balanceDrawRequest.RequestTime = System.DateTime.Now;
            balanceDrawRequest.BankName = bankName;
            balanceDrawRequest.AccountName = accountName;
            balanceDrawRequest.MerchantCode = account;
            balanceDrawRequest.Amount = amount;
            balanceDrawRequest.Remark = remark;

            if (!this.ValidateBalanceDrawRequest(balanceDrawRequest))
            {
                return base.JsonFaultResult(new CommonException(40384).GetMessage(), request.ToString());
            }

            if (EcShop.SaleSystem.Member.MemberProcessor.BalanceDrawRequest(balanceDrawRequest))
            {
                return base.JsonActionResult(new StandardResult<string>()
                {
                    code = 0,
                    msg = "提现申请成功",
                    data = ""
                });
            }

            return base.JsonFaultResult(new CommonException(40385).GetMessage(), request.ToString());
        }

        #endregion

        #region 消息

        [HttpGet]
        public IHttpActionResult Messages(string userId, int pageIndex, int pageSize, string accessToken, int channel, int platform, string ver)
        {
            Logger.WriterLogger("Member.Messages, Params: " + string.Format("userId={0}&accessToken={1}&channel={2}&platform={3}&ver={4}&pageIndex={5}&pageSize={6}", userId, accessToken, channel, platform, ver, pageIndex, pageSize), LoggerType.Info);

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), "Member.Messages: ");
            }

            // 验证参数
            //ThrowParamException(skuId);

            // 保存访问信息
            base.SaveVisitInfo(userId, channel, platform, ver);

            string msg = "";

            Member member = GetMember(userId.ToSeesionId());

            if (member == null)
            {
                return base.JsonFaultResult(new CommonException(40201).GetMessage(), "Member.Messages");
            }

            MessageBoxQuery messageBoxQuery = new MessageBoxQuery();
            messageBoxQuery.PageIndex = pageIndex;
            messageBoxQuery.PageSize = pageSize;
            messageBoxQuery.Accepter = member.Username;

            DbQueryResult memberReceivedMessages = CommentBrowser.GetMemberReceivedMessages(messageBoxQuery);

            int totalRecords = 0;

            List<MessageListItem> items = new List<MessageListItem>();

            if (memberReceivedMessages != null)
            {
                totalRecords = memberReceivedMessages.TotalRecords;

                DataTable dt = memberReceivedMessages.Data as DataTable;

                if (dt != null)
                {
                    MessageListItem item = null;

                    foreach (DataRow current in dt.Rows)
                    {
                        item = new MessageListItem();

                        item.Id = (long)current["MessageId"];
                        item.Accepter = (string)current["Accepter"];
                        item.Sender = (string)current["Sernder"];
                        item.Title = (string)current["Title"];
                        item.Content = (string)current["Content"];
                        item.SendDate = ((DateTime)current["Date"]).ToString("yyyy-MM-dd HH:mm:ss");
                        item.IsRead = (bool)current["IsRead"];

                        items.Add(item);
                    }
                }

                CommentBrowser.SetMemberMessageIsRead(member.Username);
            }

            StandardResult<ListResult<MessageListItem>> result = new StandardResult<ListResult<MessageListItem>>()
            {
                code = 0,
                msg = "我的消息列表",
                data = new ListResult<MessageListItem>()
                {
                    TotalNumOfRecords = totalRecords,
                    Results = items
                }
            };

            return base.JsonActionResult(result);
        }

        [HttpGet]
        public IHttpActionResult UnreadMessageCount(string userId, string accessToken, int channel, int platform, string ver)
        {
            Logger.WriterLogger("Member.UnreadMessageCount, Params: " + string.Format("userId={0}&accessToken={1}&channel={2}&platform={3}&ver={4}", userId, accessToken, channel, platform, ver), LoggerType.Info);

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), "Member.UnreadMessageCount: ");
            }

            // 验证参数
            //ThrowParamException(skuId);

            // 保存访问信息
            base.SaveVisitInfo(userId, channel, platform, ver);

            Member member = GetMember(userId.ToSeesionId());

            if (member == null)
            {
                return base.JsonFaultResult(new CommonException(40201).GetMessage(), "Member.UnreadMessageCount");
            }

            int count = CommentBrowser.GetUnreadMessageCount(member.Username);

            StandardResult<int> result = new StandardResult<int>()
            {
                code = 0,
                msg = "我的未读消息数量",
                data = count
            };

            return base.JsonActionResult(result);
        }

        [HttpGet]
        public IHttpActionResult Message(string userId, long messageId, string accessToken, int channel, int platform, string ver)
        {
            Logger.WriterLogger("Member.Message, Params: " + string.Format("userId={0}&accessToken={1}&channel={2}&platform={3}&ver={4}&id={5}", userId, accessToken, channel, platform, ver, messageId), LoggerType.Info);

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), "Member.Message: ");
            }

            // 验证参数
            //ThrowParamException(skuId);

            // 保存访问信息
            base.SaveVisitInfo(userId, channel, platform, ver);

            string msg = "";

            Member member = GetMember(userId.ToSeesionId());

            if (member == null)
            {
                return base.JsonFaultResult(new CommonException(40201).GetMessage(), "Member.Message");
            }

            MessageBoxInfo message = CommentBrowser.GetMemberMessage(messageId);

            MessageListItem item = null;

            if (message != null)
            {
                item = new MessageListItem();

                item.Id = message.MessageId;
                item.Accepter = message.Accepter;
                item.Sender = message.Sernder;
                item.Title = message.Title;
                item.Content = message.Content;
                item.SendDate = (message.Date).ToString("yyyy-MM-dd HH:mm:ss");
                item.IsRead = message.IsRead;

                if (!message.IsRead)
                {
                    CommentBrowser.PostMemberMessageIsRead(messageId);
                }
            }

            StandardResult<MessageListItem> result = new StandardResult<MessageListItem>()
            {
                code = 0,
                msg = "",
                data = item
            };

            return base.JsonActionResult(result);
        }


        public IHttpActionResult DelMessage(JObject request)
        {
            Logger.WriterLogger("Member.DelMessage, Params: " + request.ToString(), LoggerType.Info);

            ParamDelMessage param = new ParamDelMessage();

            try
            {
                param = request.ToObject<ParamDelMessage>();
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

            // 验证参数
            //ThrowParamException(skuId);

            string userId = param.UserId;
            int channel = param.channel;
            int platform = param.platform;
            string ver = param.ver;

            // 保存访问信息
            base.SaveVisitInfo(userId, channel, platform, ver);

            Member member = GetMember(userId.ToSeesionId());

            if (member == null)
            {
                return base.JsonFaultResult(new CommonException(40201).GetMessage(), request.ToString());
            }

            try
            {
                if (CommentBrowser.DeleteMessage(param.id, member.Username))
                {
                    return base.JsonActionResult(new StandardResult<string>()
                    {
                        code = 0,
                        msg = "删除成功",
                        data = ""
                    });
                }

                else
                {
                    return base.JsonActionResult(new StandardResult<string>()
                    {
                        code = 0,
                        msg = "该消息不存在",
                        data = ""
                    });
                }
            }

            catch (Exception ex)
            {
                Logger.WriterLogger("Member.DelMessage", ex, LoggerType.Error);

                return base.JsonFaultResult(new CommonException(40999).GetMessage(), request.ToString());
            }



        }

        #endregion

        #region Private

        private void GetRegion(int regionId, out string province, out string city, out string county)
        {
            province = "";
            city = "";
            county = "";

            string region = RegionHelper.GetFullRegion(regionId, ",");

            string[] regionList = region.Split(new char[] { ',' });

            if (regionList.Length == 3)
            {
                province = regionList[0];
                city = regionList[1];
                county = regionList[2];
            }
            else if (regionList.Length == 2)
            {
                province = regionList[0];
                city = regionList[1];
            }
            else if (regionList.Length == 1)
            {
                province = regionList[0];
            }

        }

        private string GenerateInpourId()
        {
            string text = string.Empty;
            System.Random random = new System.Random();
            for (int i = 0; i < 7; i++)
            {
                int num = random.Next();
                text += ((char)(48 + (ushort)(num % 10))).ToString();
            }
            return System.DateTime.Now.ToString("yyyyMMdd") + text;
        }

        private bool ValidateBalanceDrawRequest(BalanceDrawRequestInfo balanceDrawRequest)
        {
            ValidationResults validationResults = Validation.Validate<BalanceDrawRequestInfo>(balanceDrawRequest, new string[]
			{
				"ValBalanceDrawRequestInfo"
			});

            return validationResults.IsValid;
        }

        #endregion
    }
}
