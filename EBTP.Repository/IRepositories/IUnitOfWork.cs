using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBTP.Repository.IRepositories
{
    public interface IUnitOfWork
    {
        IUserRepository userRepository { get; }
        IAuthRepository authRepository { get; }
        IBrandRepository brandRepository { get; }
        IPackageRepository packageRepository { get; }
        IListingRepository listingRepository { get; }
        IListingImageRepository listingImageRepository { get; }
        ITransactionRepository transactionRepository { get; }
        IPaymentRepository paymentRepository { get; }
        public Task<int> SaveChangeAsync();
    }
}
