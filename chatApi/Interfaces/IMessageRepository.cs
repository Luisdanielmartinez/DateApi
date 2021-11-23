using chatApi.DTOs;
using chatApi.Entities;
using chatApi.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chatApi.Interfaces
{
    public interface IMessageRepository
    {
        void AddMessage(Message message);
        void DeleteMessage(Message message);

        Task<Message> GetMessage(int id);
        Task<PagedList<MessageDto>> GetMessagesForUser();
        Task<IEnumerable<MessageDto>> GetMessageThread(int currentUserId, int recipientId);
        Task<bool> SaveAllAsync();
    }
}
