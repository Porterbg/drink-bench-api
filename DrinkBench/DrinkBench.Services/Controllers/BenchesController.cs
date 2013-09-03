using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;
using DataLayer;
using DrinkBench.Models;
using DrinkBench.Services.Models;

namespace DrinkBench.Services.Controllers
{
    public class BenchesController : BaseApiController
    {
        [ActionName("all")]
        [HttpGet]
        public List<BenchModel> GetAll(string sessionKey)
        {
            var responseMsg = this.PerformOperationAndHandleExceptions(() =>
            {
                var context = new DrinkBenchContext();

                var user = context.Users.FirstOrDefault(usr => usr.SessionKey == sessionKey);
                if (user == null)
                {
                    throw new InvalidOperationException("Invalid sessionkey");
                }

                var benchesEntities = context.Benches;
                var models =
                    (from benchEntity in benchesEntities
                        select new BenchModel()
                        {
                            Id = benchEntity.Id,
                            Name = benchEntity.Name,
                            Latitude = benchEntity.Latitude,
                            Longitude = benchEntity.Longitude,
                            Users = (from benchUser in benchEntity.Users
                                         select new UserModel()
                                         {
                                             Id = benchUser.Id,
                                             Firstname = benchUser.Firstname,
                                             Lastname = benchUser.Lastname,
                                             Nickname = benchUser.Nickname,
                                             StartTime = benchUser.StartTime,
                                             EndTime = benchUser.EndTime,
                                             Avatar = benchUser.Avatar
                                         }),
                            UsersCount = benchEntity.Users.Count,
                            StartTime = benchEntity.StartTime,
                            EndTime = benchEntity.EndTime
                        });
                return models.ToList();
            });

            return responseMsg;
        }

        [ActionName("bench")]
        [HttpGet]
        public BenchFullModel GetById(int id, string sessionKey)
        {
            var responseMsg = this.PerformOperationAndHandleExceptions(() =>
            {
                var context = new DrinkBenchContext();

                var user = context.Users.FirstOrDefault(usr => usr.SessionKey == sessionKey);
                if (user == null)
                {
                    throw new InvalidOperationException("Invalid sessionkey");
                }

                var benchesEntities = context.Benches;
                var bench = benchesEntities.FirstOrDefault(b => b.Id == id);

                if (bench == null)
                {
                    throw new InvalidOperationException("Invalid bench id"+id);
                }

                BenchFullModel model = new BenchFullModel()
                        {
                            Id = bench.Id,
                            Name = bench.Name,
                            Latitude = bench.Latitude,
                            Longitude = bench.Longitude,
                            StartTime = bench.StartTime,
                            EndTime = bench.EndTime,
                            Users = (from benchUser in bench.Users
                                     select new FriendsModel()
                                     {
                                         Id = benchUser.Id,
                                         Nickname = benchUser.Nickname,
                                         Avatar = benchUser.Avatar,
                                         StartTime = benchUser.StartTime,
                                         EndTime = benchUser.EndTime
                                     }).ToList()
                        };
                return model;
            });

            return responseMsg;
        }

        [ActionName("newbench")]
        [HttpPost]
        public HttpResponseMessage PostBenches(string sessionKey, BenchModel bench)
        {
            var responseMsg = this.PerformOperationAndHandleExceptions(() =>
            {
                var context = new DrinkBenchContext();

                using (context)
                {
                    var user = context.Users.FirstOrDefault(usr => usr.SessionKey == sessionKey);
                    if (user == null)
                    {
                        throw new InvalidOperationException("Invalid sessionkey");
                    }

                    Bench newBench = new Bench()
                    {
                        Latitude = bench.Latitude,
                        Longitude = bench.Longitude,
                        Name = bench.Name,
                        StartTime = bench.StartTime,
                        EndTime = bench.EndTime
                    };
                    newBench.Users.Add(user);
                    context.Benches.Add(newBench);
                    context.SaveChanges();

                    CreatedBench createdBench = new CreatedBench()
                    {
                        Name = newBench.Name,
                        Id = newBench.Id
                    };

                    var response =
                            this.Request.CreateResponse(HttpStatusCode.Created,
                                            createdBench);
                    return response;
                }
            });

            return responseMsg;
        }

        //[HttpGet]
        //public BenchFullModel GetById(int id)
        //{
        //    if (id <= 0)
        //    {
        //        var errResponse = this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Bench id incorrect: " + id);
        //        throw new HttpResponseException(errResponse);
        //    }

        //    var entity = this.BenchRepository.Get(id);

        //    var model = new BenchFullModel()
        //    {
        //        Id = entity.Id,
        //        Name = entity.Name,
        //        Users = (
        //        from userEntity in entity.Users
        //        select new UserModel()
        //        {
        //            Id = userEntity.Id,
        //            Firstname = userEntity.Firstname,
        //            Lastname = userEntity.Lastname,
        //            Nickname = userEntity.Nickname
        //        }).ToList()
        //    };
        //    return model;
        //}

        //[HttpPost]
        //public HttpResponseMessage PostBench(BenchModel model)
        //{
        //    if (model == null || model.Name == null || model.Name.Length < 3)
        //    {
        //        var errResponse = this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "The name of the Bench should be at least 3 characters");
        //        return errResponse;
        //    }

        //    Bench bench = new Bench()
        //    {

        //    }
        //    var entity = this.BenchRepository.Add(model);

        //    var response =
        //        Request.CreateResponse(HttpStatusCode.Created, entity);

        //    response.Headers.Location = new Uri(Url.Link("DefaultApi",
        //        new { id = entity.Id }));
        //    return response;
        //}

        //PUT
        //DELETE

    }
}
