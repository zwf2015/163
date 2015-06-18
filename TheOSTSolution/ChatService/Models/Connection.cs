
using System.ComponentModel.DataAnnotations.Schema;
namespace ChatService.Models
{
    public class Connection
    {

        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.None)]
        public string ConnectionId { get; set; }
        public string UserAgent { get; set; }
        public bool Connected { get; set; }
    }
}