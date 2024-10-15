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
    public partial class ChatConversationParticipant : BaseCompanyObjects
    {
        public ChatConversationParticipant()
        {
            ChatMessage = new HashSet<ChatMessage>();
        }

        [Key]
        [Display(Name = "Chat Conversation Participant Id")]
        [Column("ChatConversationParticipantId")]
        public override int Id { get; set; }

        public int ChatConversationId { get; set; }

        public int ParticipantId { get; set; }

        public virtual ChatConversation ChatConversation { get; set; }

        public virtual UserInformation Participant { get; set; }

        public virtual ICollection<ChatMessage> ChatMessage { get; set; }

        public DateTime? LastMessageReadTime { get; set; }

        [NotMapped]
        public bool IsMe
        {
            get
            {
                return ParticipantId == SessionHelper.LoginId;
            }
        }

        [NotMapped]
        public bool IsSelectedUser
        {
            get
            {
                return ParticipantId == SessionHelper.SelectedUserInformationId;
            }
        }

        [NotMapped]
        public bool IsInitiatedUser
        {
            get
            {
                return ChatConversation.ChatInitiatedById == ParticipantId;
            }
        }

        [NotMapped]
        public int PendingMessageCount
        {
            get
            {
                return ChatConversation.ChatMessage.ToList().Where(m => m.CreatedDate >= LastMessageReadTime).ToList().Count;
            }
        }
    }
}
