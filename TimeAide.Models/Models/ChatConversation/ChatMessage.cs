using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeAide.Common.Helpers;
using TimeAide.Web.Models;

namespace TimeAide.Web.Models
{
    public partial class ChatMessage : BaseCompanyObjects
    {
        [Key]
        [Display(Name = "Chat Message Id")]
        [Column("ChatMessageId")]
        public override int Id { get; set; }
        public int ChatConversationParticipantId { get; set; }
        public int ChatConversationId { get; set; }
        public string ChatMessageText { get; set; }
        public string ChatDocumentFilePath { get; set; }
        public string ChatDocumentName
        {
            get
            {
                return Path.GetFileName(ChatDocumentFilePath);
            }
        }
        public virtual ChatConversationParticipant ChatConversationParticipant { get; set; }
        public virtual ChatConversation ChatConversation { get; set; }

        [NotMapped]
        public bool IsMyMessage
        {
            get
            {
                return ChatConversationParticipant.IsMe;
            }
        }

        [NotMapped]
        public bool IsSelectedUserMessage
        {
            get
            {
                return ChatConversationParticipant.IsSelectedUser;
            }
        }

        [NotMapped]
        public bool IsInitiatedUserMessage
        {
            get
            {
                return ChatConversationParticipant.IsInitiatedUser;
            }
        }
    }
}
