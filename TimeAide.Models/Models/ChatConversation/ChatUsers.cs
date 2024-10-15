using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeAide.Common.Helpers;
using TimeAide.Web.Models;

namespace TimeAide.Web.Models
{
    public partial class ChatUsers : BaseCompanyObjects
    {
        [Key]
        [Display(Name = "Chat User Id")]
        [Column("ChatUserId")]
        public override int Id { get; set; }
        public string UserName { get; set; }
        public string UserID { get; set; }
        public string ConnectionID { get; set; }
        public string UserInformationName { get; set; }
        public int ActiveChatConversationId { get; set; }
    }
}
