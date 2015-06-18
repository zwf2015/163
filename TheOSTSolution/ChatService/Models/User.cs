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
        /// 用户数字编号
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 自动生成的编号
        /// </summary>
        public string UserGuid { get; set; }
        /// <summary>
        /// 用户名
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