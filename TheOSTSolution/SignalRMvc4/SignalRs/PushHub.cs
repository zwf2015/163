using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace SignalRMvc4.SignalRs
{
    /// <summary>
    /// 1.添加Hub
    /// </summary>
    [HubName("pb2")]
    public class PushHub : Hub
    {
        /// <summary>
        /// 发送普通消息
        /// </summary>
        /// <param name="message"></param>
        [HubMethodName("pb2_s")]
        public void Send(string name, string message)
        {
            Clients.All.addMsgToPage(new MsgDTO { UserName = name, Message = message });
        }
        /// <summary>
        /// 向客户端广播消息
        /// </summary>
        /// <param name="message"></param>
        [HubMethodName("pb2_p")]
        public void PushMsg(string message) 
        {
            //全部
            Clients.All.pushMsg(message);
            //调用者
            Clients.Caller.pushMsg(message);
            //除了调用者的其他人
            Clients.Others.pushMsg(message);
            //指定Id的客户端
            Clients.Client(Context.ConnectionId).pushMsg(message);
        }

        /// <summary>
        /// 登录校验，然后绑定客户端
        /// </summary>
        /// <param name="userKey"></param>
        public void Bind(string userKey)
        {

        }
        #region Hub生存周期管理
        public override Task OnConnected()
        {
            var id = Context.ConnectionId;
            var c = Clients.Caller;
            return base.OnConnected();
        }
        public override Task OnReconnected()
        {
            //
            return base.OnReconnected();
        }
        public override Task OnDisconnected()
        {
            //
            return base.OnDisconnected();
        }
        #endregion

        #region 组管理
        public Task JoinGroup(string groupName)
        {
            return Groups.Add(Context.ConnectionId, groupName);
        }
        public Task LeaveGroup(string groupName)
        {
            return Groups.Remove(Context.ConnectionId, groupName);
        }
        #endregion

    }

    public class MsgDTO 
    {
        public string UserName { get; set; }
        public string Message { get; set; }
    }
}