using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ChatService.Models
{
    public class Group
    {
        public int GroupId { get; set; }
        [Key]
        public string GroupGuid { get; set; }
        public string GroupName { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}