using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace DBRepository.Interfaces
{
    public interface IMessageRepository
    {
        Task<List<Message>> GetMessages(int chatId);
        System.Threading.Tasks.Task AddMessage(Message message);
    }
}
