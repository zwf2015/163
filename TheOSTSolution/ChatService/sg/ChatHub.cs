using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatService.Models;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace ChatService.Hubs
{
    [HubName("chatHub")]
    public class ChatHub : Hub
    {
        [HubMethodName("sendMsg")]
        public void SendMsg(string from,string to, string message)
        {
            using (var db =new ChatServiceContext())
            {
                string _userId = db.Users.Where(a => a.UserName == to).Select(a => a.UserGuid).FirstOrDefault();
                if (_userId != null)
                {
                    Clients.User(_userId).pushMsg(from, message);
                }
                else
                {
                    Clients.Caller.sysMsg(string.Format("消息“{0}”发送失败：未找到 {1} 。", message, to));
                }
            }
        }

        public void SendMsgInGroup(string groupName, string message)
        {
            Clients.Group(groupName, null).pushMsg(message);
        }

        public override Task OnConnected()
        {
            var _userName = Context.QueryString["userName"];
            using (var db = new ChatServiceContext())
            {
                var user = db.Users.FirstOrDefault(u => u.UserName == _userName);
                if (user == null)
                {
                    user = new User()
                    {
                        UserId = 0,
                        UserName = _userName
                    };
                    db.Users.Add(user);
                }
                else
                {
                    // Add to each assigned group.
                    foreach (var item in user.Groups)
                    {
                        Groups.Add(Context.ConnectionId, item.GroupName);
                    }
                }
                user.UserGuid = Context.ConnectionId;
                db.SaveChanges();
            }
            Clients.Caller.sysMsg(string.Format("{0} 登录成功。", _userName));
            return base.OnConnected();
        }

        public void AddToRoom(string roomName)
        {
            using (var db = new ChatServiceContext())
            {
                var room = db.Groups.Find(roomName);
                if (room != null)
                {
                    var user = new User()
                    {
                        UserId = (db.Users.Max(a => a.UserId)) + 1,
                        UserName = Guid.NewGuid().ToString().Substring(8, 16),
                        UserGuid = Context.ConnectionId
                    };
                    db.Users.Attach(user);

                    room.Users.Add(user);
                    db.SaveChanges();

                    Clients.Group(roomName, null).sysMsg(string.Format("{0} 加入了本组。",Context.User.Identity.Name));

                    Groups.Add(Context.ConnectionId, roomName);
                }
            }
        }

        public void RemoveFromRoom(string roomName)
        {
            using (var db = new ChatServiceContext())
            {
                // Retrieve room.
                var room = db.Groups.Find(roomName);
                if (room != null)
                {
                    var user = new User()
                    {
                        UserId = (db.Users.Max(a => a.UserId)) + 1,
                        UserName = Guid.NewGuid().ToString().Substring(8, 16),
                        UserGuid = Context.ConnectionId
                    };
                    db.Users.Attach(user);

                    room.Users.Remove(user);
                    db.SaveChanges();

                    Groups.Remove(Context.ConnectionId, roomName);

                    Clients.Group(roomName, null).sysMsg(string.Format("{0} 离开了本组。", Context.User.Identity.Name));
                }
            }
        }

    }
}