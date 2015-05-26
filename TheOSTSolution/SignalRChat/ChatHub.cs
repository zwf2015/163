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
        }

    }
}