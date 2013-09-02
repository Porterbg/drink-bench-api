using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using DataLayer;
using DrinkBench.Models;
using System.Text;
using DrinkBench.Services.Models;

namespace DrinkBench.Services.Controllers
{
    public class FriendsController : BaseApiController
    {
        private const int SessionKeyLength = 50;

        private const int Sha256Length = 40;

        public FriendsController()
        {
        }

        [ActionName("add")]
        [HttpPost]
        public HttpResponseMessage PostAddFreind(string nickname, string sessionKey)
        {
            var responseMsg = this.PerformOperationAndHandleExceptions(
                () =>
                {
                    var context = new DrinkBenchContext();
                    using (context)
                    {
                        if (sessionKey == null)
                        {
                            throw new ArgumentException("SessionKey is null!");
                        }

                        var user = context.Users.FirstOrDefault(
                            usr => usr.SessionKey == sessionKey);

                        if (user == null)
                        {
                            throw new InvalidOperationException(
                            "Users info incorrect SessienKey:" + sessionKey);
                        }


                        var hasFriend = user.Friends.FirstOrDefault(
                            f => f.Nickname == nickname);
                        if (hasFriend != null)
                        {
                            throw new InvalidOperationException(
                                "Users are already set as friends");
                        }

                        var friend = context.Users.FirstOrDefault(
                            usr => usr.Nickname == nickname);
                        Friend newFriend = new Friend()
                        {
                            Nickname = friend.Nickname,
                            StartTime = friend.StartTime,
                            EndTime = friend.EndTime,
                            Avatar = friend.Avatar
                        };
                        newFriend.Users.Add(user);
                        context.Friends.Add(newFriend);
                        user.Friends.Add(newFriend);
                        context.SaveChanges();

                        var response =
                            this.Request.CreateResponse(HttpStatusCode.OK);
                        return response;
                    }
                });

            return responseMsg;
        }

        private void ValidateAuthCode(string authCode)
        {
            if (authCode == null || authCode.Length != Sha256Length)
            {
                throw new ArgumentOutOfRangeException("Password should be encrypted");
            }
        }
    }
}
