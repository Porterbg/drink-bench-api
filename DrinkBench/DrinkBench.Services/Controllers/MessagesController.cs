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

namespace DrinkBench.Services.Controllers
{
    public class MessagesController : BaseApiController
    {
        private const int SessionKeyLength = 50;

        private const int Sha256Length = 40;

        public MessagesController()
        {
        }

        [ActionName("all")]
        [HttpGet]
        public IQueryable<MessageModel> GetAll(string sessionKey)
        {
            var responseMsg = this.PerformOperationAndHandleExceptions(() =>
            {
                var context = new DrinkBenchContext();

                var user = context.Users.FirstOrDefault(usr => usr.SessionKey == sessionKey);
                if (user == null)
                {
                    throw new InvalidOperationException("Invalid sessionkey");
                }

                var messageEntities = context.Messages;
                var models =
                    (from messageEntity in messageEntities
                     where messageEntity.User.Id == user.Id
                     select new MessageModel()
                     {
                         id = messageEntity.Id,
                         userId = messageEntity.User.Id,
                         sendDate = messageEntity.SendDate,
                         senderId = messageEntity.Sender.Id,
                         Text = messageEntity.Text,
                         Type = messageEntity.Type
                     });
                return models.OrderByDescending(m => m.sendDate);
            });

            return responseMsg;
        }

        [ActionName("message")]
        [HttpGet]
        public MessageModel GetById(int id, string sessionKey)
        {
            var model = this.GetAll(sessionKey).FirstOrDefault(m => m.id == id);
            return model;
        }

        [ActionName("newmessage")]
        [HttpPost]
        public HttpResponseMessage PostMessage(MessageModel model, string sessionKey)
        {
            var responseMsg = this.PerformOperationAndHandleExceptions(
                () =>
                {
                    var context = new DrinkBenchContext();
                    using (context)
                    {
                        var user = context.Users.FirstOrDefault(
                            usr => usr.SessionKey == sessionKey);

                        if (user == null)
                        {
                            throw new InvalidOperationException("Message user id incorret"+model.userId);
                        }
                        if (user.Id != model.userId)
                        {
                            throw new InvalidOperationException("Message user id and sessienKey don't match "
                                +model.userId+" "+user.Id);
                        }
                        Message newMessage = new Message()
                        {
                            User = context.Users.FirstOrDefault(
                            usr => usr.Id == model.userId),
                            SendDate = model.sendDate,
                            Sender = context.Users.FirstOrDefault(
                            usr => usr.Id == model.senderId),
                            Text = model.Text,
                            Type = model.Type
                        };
                        context.Messages.Add(newMessage);
                        context.SaveChanges();
                        var response =
                            this.Request.CreateResponse(HttpStatusCode.OK);
                        return response;
                    }
                });

            return responseMsg;
        }
    }
}
