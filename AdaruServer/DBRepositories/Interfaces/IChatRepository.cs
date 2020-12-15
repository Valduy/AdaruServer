using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdaruServer.Models;
using Task = System.Threading.Tasks.Task;

namespace AdaruServer.DBRepositories.Interfaces
{
    interface IChatRepository
    {
        Task<Chat> GetChat(int clientId1, int clientId2);
        Task AddChat(int clientId1, int clientId2);
    }
}
