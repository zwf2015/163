using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChatService.Models
{
    /// <summary>
    /// 聊天内容
    /// </summary>
    public class ChatMessage
    {
        [Key]
        public int MessageId { get; set; }
        public string Message { get; set; }
        public int From { get; set; }
        public int To { get; set; }
        public int MessageState { get; set; }
        public DateTime SendTime { get; set; }
    }

    public enum MessageState
    {
        /// <summary>
        /// 系统消息
        /// </summary>
        sys = 0,
        /// <summary>
        /// 买家发给卖家
        /// </summary>
        b2s = 1,
        /// <summary>
        /// 买家互发
        /// </summary>
        b2b = 2,
        /// <summary>
        /// 卖家给买家
        /// </summary>
        s2b = 4,
        /// <summary>
        /// 卖家互发
        /// </summary>
        s2s = 8,
        /// <summary>
        /// 买家发到组
        /// </summary>
        b2g = 16,
        /// <summary>
        /// 卖家发到组
        /// </summary>
        s2g = 32
    }
}