using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace ChatService.SignalRs
{
    /// <summary>
    /// 自定义的Hub管道处理模块
    /// </summary>
    public class NewHubPipelineModule:HubPipelineModule
    {
        /// <summary>
        /// 记录当前登录人的标识
        /// </summary>
        public string _connectionId { get; set; }

        /// <summary>
        /// 重写Hub错误处理
        /// </summary>
        /// <param name="exceptionContext"></param>
        /// <param name="invokerContext"></param>
        protected override void OnIncomingError(ExceptionContext exceptionContext, IHubIncomingInvokerContext invokerContext)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext("chatHub");
            context.Clients.Client(_connectionId).sysMsg(string.Format("服务器内部错误：{0}", exceptionContext.Error));
            base.OnIncomingError(exceptionContext, invokerContext);
        }

        /// <summary>
        /// 获取登录人的标识
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected override bool OnBeforeIncoming(IHubIncomingInvokerContext context)
        {
            _connectionId = context.Hub.Context.ConnectionId;
            return base.OnBeforeIncoming(context);
        }
    }
}