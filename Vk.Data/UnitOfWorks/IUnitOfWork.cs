using Vk.Data.Domain;
using Vk.Data.Repository;

namespace Vk.Data.UnitOfWorks;

public interface IUnitOfWork
{
    void CompleteAsync(CancellationToken cancellationToken);
    void CompleteTransactionAsync(CancellationToken cancellationToken);
    IGenericRepository<Customer> CustomerRepository { get; }
    IGenericRepository<Account> AccountRepository { get; }
    IGenericRepository<AccountTransaction> AccountTransactionRepository { get; }
    IGenericRepository<Address> AddressRepository { get; }
    IGenericRepository<Card> CardRepository { get; }
    IGenericRepository<EftTransaction> EftTransactionRepository { get; }
}
