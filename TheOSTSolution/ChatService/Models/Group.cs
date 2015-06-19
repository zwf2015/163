using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatService.Models
{
    /// <summary>
    /// 用户分组
    /// </summary>
    public class Group
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int GroupId { get; set; }
        public string GroupGuid { get; set; }
        public string GroupName { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}