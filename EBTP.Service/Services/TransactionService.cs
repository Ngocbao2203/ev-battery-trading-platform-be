using AutoMapper;
using EBTP.Repository.IRepositories;
using EBTP.Service.Abstractions.Shared;
using EBTP.Service.DTOs.Transaction;
using EBTP.Service.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBTP.Service.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TransactionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<TransactionDTO>>> GetTransactionsByUserId(Guid userId, int pageIndex, int pageSize)
        {
            var result = _mapper.Map<List<TransactionDTO>>(await _unitOfWork.transactionRepository.GetTransactionsByUserId(userId, pageIndex, pageSize));
            return new Result<List<TransactionDTO>>
            {
                Error = 0,
                Message = "Lấy danh sách giao dịch thành công.",
                Count = result.Count,
                Data = result
            };
        }
    }
}
