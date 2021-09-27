using System.Threading.Tasks;

namespace Application.Core
{
    public interface IUnitOfWork
    {
        Task CommitTransactionAsync();
    }
}