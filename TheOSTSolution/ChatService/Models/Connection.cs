
using System.ComponentModel.DataAnnotations.Schema;
namespace ChatService.Models
{
    /// <summary>
    /// 多客户端登录(限制)
    /// </summary>
    public class Connection
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.None)]
        public string ConnectionId { get; set; }
        public string UserAgent { get; set; }
        public bool Connected { get; set; }
    }
}