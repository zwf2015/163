using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int MessageId { get; set; }
        public string Message { get; set; }
        public int From { get; set; }
        public int To { get; set; }
        public int MessageState { get; set; }
        public DateTime SendTime { get; set; }
    }

    public enum MessageState : int
    {
        /// <summary>
        /// 未读
        /// </summary>
        newMessage = 0,
        /// <summary>
        /// 已读
        /// </summary>
        readed = 1
    }
}