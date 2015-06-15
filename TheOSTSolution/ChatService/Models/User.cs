using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ChatService.Models
{
    /// <summary>
    /// 用户
    /// </summary>
    public class User
    {
        /// <summary>
        /// 用户数字编号
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 自动生成的Guid编号
        /// </summary>
        [Key]
        public string UserGuid { get; set; }
        /// <summary>
        /// 不重复的用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 链接状态
        /// </summary>
        public ICollection<Connection> Connections { get; set; }
        /// <summary>
        /// 所属组
        /// </summary>
        public ICollection<Group> Groups { get; set; }
    }
}