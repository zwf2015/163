using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatService.Models;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

using log4net;

namespace ChatService.Hubs
{
    /// <summary>
    /// 聊天专用Hub
    /// </summary>
    [HubName("chatHub")]
    public class ChatHub : Hub
    {
        //private Models.Entities logDb = new Entities();
        private static readonly ILog log = LogManager.GetLogger(typeof(ChatHub));
        
        /// <summary>
        /// 存储当前登录用户的连接状态
        /// </summary>
        public static List<OnlineUser> onlineUsers = new List<OnlineUser>();

        /// <summary>
        /// 发消息
        /// </summary>
        /// <param name="msgDto"><see cref="MsgDOT"/></param>
        [HubMethodName("sendMsg")]
        public void SendMsg(MsgDOT msgDto)
        {
            using (var db =new ChatServiceContext())
            {
                var _user = db.Users.FirstOrDefault(a => a.UserName == msgDto.ToName);
                if (_user != null)
                {
                    var _msg = new ChatMessage();
                    _msg.From = db.Users.First(a => a.UserName == msgDto.FromName).UserId;
                    _msg.To = _user.UserId;
                    _msg.Message = msgDto.Message;
                    _msg.MessageState = (int)MessageState.newMessage;
                    _msg.SendTime = DateTime.Now;
                    db.ChatMessages.Add(_msg);
                    db.SaveChanges();
                    msgDto.MessageId = _msg.MessageId;
                    msgDto.Time = _msg.SendTime;

                    syncSelf(msgDto);
                    PushMsg(msgDto);
                }
                else
                {
                    string _errorMsg = string.Format("消息“{0}”发送失败：未找到 {1} 。", msgDto.Message, msgDto.ToName);
                    Clients.Caller.sysMsg(_errorMsg);
                    if (log.IsErrorEnabled)
                    {
                        log.Error(_errorMsg);
                    }
                }
            }
        }

        /// <summary>
        /// 发信人其他客户端推送消息
        /// </summary>
        /// <param name="msgDto"><see cref="MsgDOT"/></param>
        private void syncSelf(MsgDOT msgDto) 
        {
            var fromIds = onlineUsers.First(a => a.UserName == msgDto.FromName).Connections.Select(a => a.ConnectionId).ToList();
            fromIds.Remove(Context.ConnectionId);
            if (fromIds.Any())
            {
                Clients.Clients(fromIds).showMsg(msgDto);
            }
        }

        /// <summary>
        /// 推送消息
        /// </summary>
        /// <param name="msgDto"><see cref="MsgDOT"/></param>
        private void PushMsg(MsgDOT msgDto) 
        {
            //发送消息
            var _user = onlineUsers.FirstOrDefault(a => a.UserName == msgDto.ToName);
            if (_user != null)
            {
                var toIds = _user.Connections.Select(a => a.ConnectionId).ToList();
                if (toIds.Any())
                {
                    Clients.Clients(toIds).pushMsg(msgDto);
                }
                else
                {
                    Clients.Caller.sysMsg(string.Format("对方已离线，消息会暂时存储到服务器，TA下次上线会看到 。"));
                }
            }
            else
            {
                Clients.Caller.sysMsg(string.Format("对方已离线，消息会暂时存储到服务器，TA下次上线会看到 。"));
            }
        }

        ///// <summary>
        ///// 组中发言
        ///// </summary>
        ///// <param name="groupName">组名</param>
        ///// <param name="message">信息内容</param>
        //public void SendMsgInGroup(string groupName, string message)
        //{
        //    Clients.Group(groupName, null).pushMsg(message);
        //}

        /// <summary>
        /// 链接服务器成功，记录用户登录状态。
        /// </summary>
        /// <returns></returns>
        public override Task OnConnected()
        {
            List<MsgDOT> historyMsgs = new List<MsgDOT>();
            var _userName = Context.QueryString["userName"];
            using (var db = new ChatServiceContext())
            {
                var user = db.Users.FirstOrDefault(u => u.UserName == _userName);
                if (user == null)
                {
                    user = new User();
                    user.UserName = _userName;
                    db.Users.Add(user);
                    db.SaveChanges();
                }
                var _onlineUser=onlineUsers.FirstOrDefault(a=>a.UserName==_userName);
                if (_onlineUser == null)
                {
                    _onlineUser = new OnlineUser() { UserName = _userName, Connections = new List<Connection>() };
                    onlineUsers.Add(_onlineUser);
                }
                _onlineUser.Connections.Add(new Connection() { ConnectionId = Context.ConnectionId, Connected = true, UserAgent = "" });

                historyMsgs = db.ChatMessages.Where(a => (a.From == user.UserId && a.To == 1) || (a.From == 1 && a.To == user.UserId))
                                             .OrderByDescending(a => a.MessageId)
                                             .Select(a => new MsgDOT()
                                             {
                                                 MessageId = a.MessageId,
                                                 FromName = (a.From == 1 ? "tom" : user.UserName),
                                                 ToName = (a.From != 1 ? "tom" : user.UserName),
                                                 Message = a.Message,
                                                 IsRead = a.MessageState == (int)MessageState.readed,
                                                 Time = a.SendTime
                                             })
                                             .Take(10)
                                             .ToList();
            }
            //发送状态信息
            string _loginMsg = string.Format("{0} 登录成功，当前共有 {1} 处登录。", _userName, onlineUsers.First(a => a.UserName == _userName).Connections.Count);
            Clients.Caller.sysMsg(_loginMsg);
            if (log.IsInfoEnabled)
            {
                log.Info(_loginMsg);
            }
            //发送历史消息
            Clients.Caller.pushHistoryMsg(historyMsgs);
            Clients.Caller.sysMsg("以上是历史消息");

            return base.OnConnected();
        }
        
        /// <summary>
        /// 断开连接时移除连接状态
        /// </summary>
        /// <param name="stopCalled"></param>
        /// <returns></returns>
        public override Task OnDisconnected(bool stopCalled)
        {
            var _userName = Context.QueryString["userName"];

            var _user = onlineUsers.FirstOrDefault(a => a.UserName == _userName);
            if (_user != null)
            {
                _user.Connections.Remove(_user.Connections.First(a => a.ConnectionId == Context.ConnectionId));
            }
            return base.OnDisconnected(stopCalled);
        }

        ///// <summary>
        ///// 添加到组
        ///// </summary>
        ///// <param name="roomName">组名</param>
        //public void AddToRoom(string roomName)
        //{
        //    using (var db = new ChatServiceContext())
        //    {
        //        var room = db.Groups.Find(roomName);
        //        if (room != null)
        //        {
        //            var user = new User()
        //            {
        //                UserName = Guid.NewGuid().ToString().Substring(8, 16),
        //            };
        //            db.Users.Attach(user);
        //            room.Users.Add(user);
        //            db.SaveChanges();

        //            Clients.Group(roomName, null).sysMsg(string.Format("{0} 加入了本组。",Context.User.Identity.Name));

        //            Groups.Add(Context.ConnectionId, roomName);
        //        }
        //    }
        //}

        ///// <summary>
        ///// 退出组
        ///// </summary>
        ///// <param name="roomName">组名</param>
        //public void RemoveFromRoom(string roomName)
        //{
        //    using (var db = new ChatServiceContext())
        //    {
        //        // Retrieve room.
        //        var room = db.Groups.Find(roomName);
        //        if (room != null)
        //        {
        //            var user = new User()
        //            {
        //                UserName = Guid.NewGuid().ToString().Substring(8, 16),
        //            };
        //            db.Users.Attach(user);

        //            room.Users.Remove(user);
        //            db.SaveChanges();

        //            Groups.Remove(Context.ConnectionId, roomName);

        //            Clients.Group(roomName, null).sysMsg(string.Format("{0} 离开了本组。", Context.User.Identity.Name));
        //        }
        //    }
        //}
    }
    /// <summary>
    /// 在线用户对象
    /// </summary>
    public class OnlineUser
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 已经登陆的客户端
        /// </summary>
        public ICollection<Connection> Connections { get; set; }
    }

    /// <summary>
    /// 消息 数据 传输 对象
    /// </summary>
    public class MsgDOT 
    {
        public int MessageId { get; set; }
        public string FromName { get; set; }
        public string ToName { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public DateTime Time { get; set; }
    }
}