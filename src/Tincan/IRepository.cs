using System;
using System.Threading.Tasks;
using Tincan.Entities;

namespace Tincan
{
    public interface IRepository
    {
        Task<Message> GetMessage(string id);
        Task<Message> CreateMessage(string key, string content, DateTime expiresAt);
        Task<int> DeleteExpiredMessages(DateTime referenceTime);
    }
}
