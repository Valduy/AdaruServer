using System.Collections.Generic;
using System.Threading.Tasks;
using Models;
using Task = System.Threading.Tasks.Task;

namespace DBRepository.Interfaces
{
    public interface IChatRepository
    {
        Task<Chat> GetChat(int chatId);
        Task<Chat> GetChat(int clientId1, int clientId2);
        Task<List<Chat>> GetChats(int id);
        Task AddChat(Chat chat);
    }
}
