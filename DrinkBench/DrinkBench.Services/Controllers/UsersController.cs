using System.Net.Http;
using DataLayer;
using DrinkBench.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Net;
using System.Text;

namespace DrinkBench.Services.Controllers
{
    public class UsersController : BaseApiController
    {
        private const int MinUsernameLength = 3;
        private const int MaxUsernameLength = 40;
        private const string ValidUsernameCharacters =
            "qwertyuioplkjhgfdsazxcvbnmQWERTYUIOPLKJHGFDSAZXCVBNM1234567890_.";
        private const string ValidDisplaynameCharacters =
            "qwertyuioplkjhgfdsazxcvbnmQWERTYUIOPLKJHGFDSAZXCVBNM1234567890_. -";

        private const string SessionKeyChars =
            "qwertyuioplkjhgfdsazxcvbnmQWERTYUIOPLKJHGFDSAZXCVBNM";
        private static readonly Random rand = new Random();

        private const int SessionKeyLength = 50;

        private const int Sha1Length = 40;

        public UsersController()
        {
        }

        [ActionName("register")]
        public HttpResponseMessage PostRegisterUser(UserModel model)
        {
            var responseMsg = this.PerformOperationAndHandleExceptions(
                () =>
                {
                    var context = new DrinkBenchContext();
                    using (context)
                    {
                        this.ValidateName(model.Firstname);
                        this.ValidateName(model.Lastname);
                        this.ValidateAuthCode(model.AuthCode);
                        var firstnameToLower = model.Firstname.ToLower();
                        var lastnameToLower = model.Lastname.ToLower();
                        var user = context.Users.FirstOrDefault(
                            usr => usr.Firstname.ToLower() == firstnameToLower
                            || usr.Lastname.ToLower() == lastnameToLower);

                        if (user != null)
                        {
                            throw new InvalidOperationException("Users exists");
                        }

                        user = new User()
                        {
                            Avatar = model.Avatar,
                            AuthCode = model.AuthCode,
                            Firstname = model.Firstname,
                            Lastname = model.Lastname,
                            Nickname = model.Nickname
                        };

                        context.Users.Add(user);
                        context.SaveChanges();

                        user.SessionKey = this.GenerateSessionKey(user.Id);
                        context.SaveChanges();

                        var loggedModel = new LoggedUserModel()
                        {
                            Firstname = user.Firstname,
                            SessionKey = user.SessionKey
                        };

                        var response =
                            this.Request.CreateResponse(HttpStatusCode.Created,
                                            loggedModel);
                        return response;
                    }
                });

            return responseMsg;
        }

        [ActionName("login")]
        public HttpResponseMessage PostLoginUser(UserModel model)
        {
            var responseMsg = this.PerformOperationAndHandleExceptions(
                () =>
                {
                    var context = new DrinkBenchContext();
                    using (context)
                    {
                        this.ValidateName(model.Firstname);
                        this.ValidateName(model.Lastname);
                        this.ValidateAuthCode(model.AuthCode);
                        var firstnameToLower = model.Firstname.ToLower();
                        var user = context.Users.FirstOrDefault(
                            usr => usr.Firstname.ToLower() == firstnameToLower
                            && usr.AuthCode == model.AuthCode);

                        if (user != null)
                        {
                            throw new InvalidOperationException("Users exists");
                        }
                        if (user.SessionKey == null)
                        {
                            user.SessionKey = this.GenerateSessionKey(user.Id);
                            context.SaveChanges();
                        }

                        var loggedModel = new LoggedUserModel()
                        {
                            Firstname = user.Firstname,
                            SessionKey = user.SessionKey
                        };

                        var response =
                            this.Request.CreateResponse(HttpStatusCode.Created,
                                            loggedModel);
                        return response;
                    }
                });

            return responseMsg;
        }

        [ActionName("logout")]
        public HttpResponseMessage PutLogoutUser(string sessionKey)
        {
            var responseMsg = this.PerformOperationAndHandleExceptions(
              () =>
              {
                  var context = new DrinkBenchContext();
                  using (context)
                  {
                      var user = context.Users.FirstOrDefault(
                          usr => usr.SessionKey == sessionKey);

                      if (user == null || user.SessionKey == null)
                      {
                          throw new InvalidOperationException("Invalid sessionkey");
                      }

                      user.SessionKey = null;
                      context.SaveChanges();

                      var response =
                          this.Request.CreateResponse(HttpStatusCode.OK);
                      return response;
                  }
              });

            return responseMsg;
        }

        private string GenerateSessionKey(int userId)
        {
            StringBuilder skeyBuilder = new StringBuilder(SessionKeyLength);
            skeyBuilder.Append(userId);
            while (skeyBuilder.Length < SessionKeyLength)
            {
                var index = rand.Next(SessionKeyChars.Length);
                skeyBuilder.Append(SessionKeyChars[index]);
            }
            return skeyBuilder.ToString();
        }
  
        

        //PUT
        //DELETE

        private void ValidateAuthCode(string authCode)
        {
            if (authCode == null || authCode.Length != Sha1Length)
            {
                throw new ArgumentOutOfRangeException("Password should be encrypted");
            }
        }

        private void ValidateName(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("Nick cannot be null");
            }
            else if (name.Length < MinUsernameLength)
            {
                throw new ArgumentOutOfRangeException(
                    string.Format("Username must be at least {0} characters long",
                    MinUsernameLength));
            }
            else if (name.Length > MaxUsernameLength)
            {
                throw new ArgumentOutOfRangeException(
                    string.Format("Username must be less than {0} characters long",
                    MaxUsernameLength));
            }
            else if (name.Any(ch => !ValidUsernameCharacters.Contains(ch)))
            {
                throw new ArgumentOutOfRangeException(
                    "Username must contain only Latin letters, digits .,_");
            }
        }
    }
}
