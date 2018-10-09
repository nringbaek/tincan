using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Tincan.Cryptography;
using Tincan.Entities;
using Tincan.Persistence.EFCore.Context;

namespace Tincan.Persistence.EFCore
{
    public class Repository : IRepository
    {
        private readonly TincanContext _context;

        public Repository(TincanContext context)
        {
            _context = context;
        }

        public async Task<Message> CreateMessage(string key, string content, DateTime expiresAt)
        {
            string id;

            do
            {
                Encryption.GenerateRandomBytes(48, out var idBytes);
                id = Base64UrlTextEncoder.Encode(idBytes);
            }
            while (await _context.Messages.AnyAsync(m => m.Id == id));

            var message = new Message
            {
                Id = id,
                EncryptedContent = Encryption.Encrypt(content, key),
                ExpiresAt = expiresAt,
                CreatedAt = DateTime.UtcNow
            };

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            return message;
        }

        public Task<Message> GetMessage(string id)
        {
            return _context.Messages.FirstOrDefaultAsync(m => m.Id == id);
        }

        public Task<int> DeleteExpiredMessages(DateTime referenceTime)
        {
            _context.Messages.RemoveRange(_context.Messages.Where(m => m.ExpiresAt < referenceTime));
            return _context.SaveChangesAsync();
        }
    }
}
