using System.Collections.Generic;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace SignalRChat
{
    [HubName("hb2")]
    public class ChatHub : Hub
    {
        /// <summary>
        /// 服务端推送消息的方法
        /// </summary>
        /// <param name="name">消息发送者</param>
        /// <param name="message">消息内容</param>
        public void Send(string name, string message)
        {
            //3.服务端把收到的消息推送给所有的客户端。
            Clients.All.broadcastMessage(name, message);

            ////其他消息推送的对象设置
            //Clients.AllExcept(new string[]{"",""}).willReceivceMsg(name, message);
            //Clients.Caller.willReceivceMsg(name, message);
            //Clients.Client("connectionId").willReceivceMsg(name, message);
            //Clients.Clients(new List<string>()).willReceivceMsg(name, message);
            //Clients.Group("groupName", new string[] { "", "" }).willReceivceMsg(name, message);
            //Clients.Groups(new List<string>(), new string[] { "", "" }).willReceivceMsg(name, message);
            //Clients.Others.willReceivceMsg(name, message);
            //Clients.OthersInGroup("groupName").willReceivceMsg(name, message);
            //Clients.OthersInGroups(new List<string>()).willReceivceMsg(name, message);
            //Clients.User("userId").willReceivceMsg(name, message);
        }

    }
}