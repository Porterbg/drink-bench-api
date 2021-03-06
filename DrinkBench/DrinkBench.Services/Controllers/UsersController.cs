﻿using System;
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
    public class UsersController : BaseApiController
    {
        private const int MinUsernameLength = 3;
        private const int MaxUsernameLength = 40;
        //private const string ValidUsernameCharacters =
        //    "qwertyuioplkjhgfdsazxcvbnmQWERTYUIOPLKJHGFDSAZXCVBNM1234567890_.";
        private const string ValidUsernameCharacters =
            "qwertyuioplkjhgfdsazxcvbnmQWERTYUIOPLKJHGFDSAZXCVBNM1234567890_. -";

        private const string SessionKeyChars =
            "qwertyuioplkjhgfdsazxcvbnmQWERTYUIOPLKJHGFDSAZXCVBNM";
        private static readonly Random rand = new Random();

        private const int SessionKeyLength = 50;

        private const int Sha256Length = 64;

        public UsersController()
        {
        }

        [ActionName("all")]
        [HttpGet]
        public List<FriendsModel> GetAll(string sessionKey)
        {
            var responseMsg = this.PerformOperationAndHandleExceptions(() =>
            {
                var context = new DrinkBenchContext();

                var user = context.Users.FirstOrDefault(usr => usr.SessionKey == sessionKey);
                if (user == null)
                {
                    throw new InvalidOperationException("Invalid sessionkey");
                }

                var usersEntities = context.Users;
                var models =
                    (from userEntity in usersEntities
                     select new FriendsModel()
                     {
                         Id = userEntity.Id,
                         StartTime = userEntity.StartTime,
                         EndTime = userEntity.EndTime,
                         Nickname = userEntity.Nickname,
                         Avatar = userEntity.Avatar
                         
                     });
                return models.ToList();
            });

            return responseMsg;
        }

        [ActionName("user")]
        [HttpGet]
        public UserModel GetById(int id, string sessionKey)
        {
            var responseMsg = this.PerformOperationAndHandleExceptions(() =>
            {
                var context = new DrinkBenchContext();
                using (context)
                {
                    var user = context.Users.FirstOrDefault(usr => usr.Id == id);
                    var sessionUser = context.Users.FirstOrDefault(usr => usr.SessionKey == sessionKey);
                    if (sessionUser == null)
                    {
                        throw new InvalidOperationException("Invalid sessionkey");
                    }

                    var model = new UserModel()
                    {
                        Id = user.Id,
                        Firstname = user.Firstname,
                        Lastname = user.Lastname,
                        Nickname = user.Nickname,
                        StartTime = user.StartTime,
                        EndTime = user.EndTime,
                        Avatar = user.Avatar,
                        Benches = user.Benches==null?null:
                        (   from bench in user.Benches
                            select new BenchModel()
                        {
                            StartTime = bench.StartTime,
                            EndTime = bench.EndTime,
                            Id = bench.Id,
                            Name = bench.Name,
                            Latitude = bench.Latitude,
                            Longitude = bench.Longitude,
                            UsersCount = (bench.Users != null) ? bench.Users.Count : 0
                        }).ToList(),
                        Friends = user.Friends==null?null:
                            (from friend in user.Friends
                             select new FriendsModel() 
                             {
                                 Id = friend.Id,
                                 StartTime = friend.StartTime,
                                 EndTime = friend.EndTime,
                                 Nickname = friend.Nickname,
                                 Avatar = friend.Avatar
                             }).ToList()
                    };
                    //this.GetAll(sessionKey).FirstOrDefault(u => u.Id == id);
                    return model;
                }
            });
            return responseMsg;
        }
        
        [ActionName("register")]
        [HttpPost]
        public HttpResponseMessage PostRegister(RegisterModel model)
        {
            var responseMsg = this.PerformOperationAndHandleExceptions(
                () =>
                {
                    var context = new DrinkBenchContext();
                    using (context)
                    {
                        //this.ValidateName(model.Firstname);
                        //this.ValidateName(model.Lastname);
                        this.ValidateAuthCode(model.AuthCode);
                        var firstnameToLower = model.Firstname.ToLower();
                        var lastnameToLower = model.Lastname.ToLower();
                        var user = context.Users.FirstOrDefault(
                            usr => usr.Firstname.ToLower() == firstnameToLower
                            && usr.Lastname.ToLower() == lastnameToLower
                            && usr.Nickname == model.Nickname);

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
                            Nickname = model.Nickname,
                            StartTime = DateTime.Now,
                            EndTime = DateTime.Now
                        };

                        context.Users.Add(user);
                        context.SaveChanges();

                        user.SessionKey = this.GenerateSessionKey(user.Id);
                        context.SaveChanges();

                        var loggedModel = new LoggedUserModel()
                        {
                            Firstname = user.Firstname,
                            SessionKey = user.SessionKey,
                            Id = user.Id
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
        [HttpPost]
        public HttpResponseMessage PostLogin(RegisterModel model)
        {
            var responseMsg = this.PerformOperationAndHandleExceptions(
                () =>
                {
                    var context = new DrinkBenchContext();
                    using (context)
                    {
                        //this.ValidateName(model.Firstname);
                        //this.ValidateName(model.Lastname);
                        this.ValidateAuthCode(model.AuthCode);
                        var firstnameToLower = model.Firstname.ToLower();
                        var user = context.Users.FirstOrDefault(
                            usr => usr.Firstname.ToLower() == firstnameToLower
                            && usr.AuthCode == model.AuthCode);

                        if (user == null)
                        {
                            var userWithoutOAuth = context.Users.FirstOrDefault(
                            usr => usr.Firstname.ToLower() == firstnameToLower
                            && usr.Nickname == model.Nickname);
                            if (userWithoutOAuth != null)
                            {
                                throw new InvalidOperationException(
                                "Users info incorrect Firstname:" + model.Firstname +
                                "AuthCode:" + model.AuthCode);
                            }
                            else
                            {
                                return PostRegister(model);
                            }
                        }
                        if (user.SessionKey == null)
                        {
                            user.SessionKey = this.GenerateSessionKey(user.Id);
                            context.SaveChanges();
                        }

                        var loggedModel = new LoggedUserModel()
                        {
                            Firstname = user.Firstname,
                            SessionKey = user.SessionKey,
                            Id = user.Id
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
        [HttpPut]
        public HttpResponseMessage PutLogout(string sessionKey)
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
            if (authCode == null || authCode.Length != Sha256Length)
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
