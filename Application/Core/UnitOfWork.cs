using System.Threading.Tasks;
using Persistence.Context;

namespace Application.Core
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;

        public UnitOfWork(DataContext context)
        {
            _context = context;
        }
        
        public async Task CommitTransactionAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}