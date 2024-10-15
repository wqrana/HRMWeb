using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeAide.Web.Models;

namespace TimeAide.Web.Models
{
    public partial class ChatConversation : BaseCompanyObjects
    {
        public ChatConversation()
        {
            ChatConversationParticipant = new HashSet<ChatConversationParticipant>();
            ChatMessage = new HashSet<ChatMessage>();
        }

        [Key]
        [Display(Name = "Chat Conversation Id")]
        [Column("ChatConversationId")]
        public override int Id { get; set; }

        [Required]
        [StringLength(250)]
        public string ChatConversationTitle { get; set; }

        public int ChatInitiatedById { get; set; }

        public virtual UserInformation ChatInitiatedBy { get; set; }

        public virtual ICollection<ChatConversationParticipant> ChatConversationParticipant { get; set; }
        public virtual ICollection<ChatMessage> ChatMessage { get; set; }
        [NotMapped]
        public bool IsGroupChate
        {
            get
            {
                return ChatConversationParticipant.Count > 2;
            }
        }
        [NotMapped]
        public bool IsIndividualChate
        {
            get
            {
                return ChatConversationParticipant.Count <= 2;
            }
        }
        [NotMapped]
        public virtual ChatConversationParticipant OtherParticipant
        {
            get
            {
                var participoant = ChatConversationParticipant.ToList().Where(c => !c.IsMe).FirstOrDefault();
                if (participoant == null)
                {
                    participoant = new ChatConversationParticipant();
                    participoant.Participant = new UserInformation();
                }
                return participoant;
            }
        }

        [NotMapped]
        public string ChatMessageText { get; set; }

        [NotMapped]
        public virtual ChatConversationParticipant MyUserInformation
        {
            get
            {
                return ChatConversationParticipant.ToList().Where(c => c.IsMe).FirstOrDefault();
            }
        }


        [NotMapped]
        public virtual String Displayname
        {
            get;
            set;
        }

        [NotMapped]
        public virtual ChatMessage LatestMessage
        {
            get
            {
                return ChatMessage.OrderByDescending(m=>m.CreatedDate).FirstOrDefault();
            }
        }
    }
}
