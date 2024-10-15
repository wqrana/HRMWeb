using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeAide.Common.Helpers;
using TimeAide.Web.Models;

namespace TimeAide.Services
{
    public class ChatConversationService
    {
        public static List<ChatConversation> GetChatConversation(int? companyId, int clientId, int userInformationId)
        {
            TimeAideContext db = new TimeAideContext();
            return db.GetAll<ChatConversationParticipant>(clientId).Where(u => u.CompanyId == companyId && u.ClientId == clientId && u.ParticipantId == userInformationId).Select(u => u.ChatConversation).ToList();
        }

        public static List<ChatConversation> GetChatConversationWithUnreadMessages()
        {
            int? companyId = TimeAide.Common.Helpers.SessionHelper.SelectedCompanyId;
            int clientId = TimeAide.Common.Helpers.SessionHelper.SelectedClientId;
            int userInformationId = TimeAide.Common.Helpers.SessionHelper.LoginId;

            TimeAideContext db = new TimeAideContext();
            var chatList = db.GetAll<ChatConversationParticipant>(clientId).Where(u => u.CompanyId == companyId && u.ClientId == clientId && u.ParticipantId == userInformationId).Select(u => u.ChatConversation).ToList();

            return chatList.Where(c => c.MyUserInformation.PendingMessageCount > 0).ToList();
        }

        public static void SaveChatConversation(int? id, int? LoginId, string name, string message)
        {
            TimeAideContext db = new TimeAideContext();
            var participant = db.ChatConversationParticipant.FirstOrDefault(c => c.ChatConversationId == id && c.ParticipantId == LoginId);
            var ChatMessage = new ChatMessage();
            ChatMessage.ChatConversationParticipantId = participant.Id;
            ChatMessage.ChatConversationId = id ?? 0;
            ChatMessage.ChatMessageText = message;
            db.ChatMessage.Add(ChatMessage);
            db.SaveChanges();
        }


        public static Role GetRole(int roleId)
        {
            TimeAideContext db = new TimeAideContext();
            return db.Role.FirstOrDefault(r => r.Id == roleId);
        }
        public static List<RoleType> GetRoleType()
        {
            TimeAideContext db = new TimeAideContext();
            return db.RoleType.Where(u => (u.DataEntryStatus != 2)).ToList();
        }

    }
}
