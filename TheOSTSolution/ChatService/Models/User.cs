using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatService.Models
{
    /// <summary>
    /// 用户
    /// </summary>
    public class User
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        /// <summary>
        /// 自增长编号
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 多客户端登录状态
        /// </summary>
        public ICollection<Connection> Connections { get; set; }
        /// <summary>
        /// 所属组
        /// </summary>
        public ICollection<Group> Groups { get; set; }
    }
}