using EBTP.Repository.IRepositories;
using EBTP.Service.Abstractions.PaymentService;
using EBTP.Service.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBTP.Service.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly VnPaySettings _settings;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PaymentService(IOptions<VnPaySettings> options, IUnitOfWork unitOfWork, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _settings = options.Value;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> CreatePaymentUrl(string transactionNo, decimal totalAmount, HttpContext context)
        {
            var vnpay = new VnPayLibrary();

            vnpay.AddRequestData("vnp_Version", "2.1.0");
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", _settings.TmnCode);
            vnpay.AddRequestData("vnp_Amount", ((int)(totalAmount * 100)).ToString());
            vnpay.AddRequestData("vnp_CreateDate", DateTime.UtcNow.AddHours(7).ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", context.Connection.RemoteIpAddress?.ToString());
            vnpay.AddRequestData("vnp_Locale", "vn");
            vnpay.AddRequestData("vnp_OrderInfo", $"Thanh toán đơn hàng mã {transactionNo}");
            vnpay.AddRequestData("vnp_OrderType", "other");
            vnpay.AddRequestData("vnp_ReturnUrl", _settings.ReturnUrl);

            // ✅ mã giao dịch duy nhất
            vnpay.AddRequestData("vnp_TxnRef", transactionNo);

            var paymentUrl = vnpay.CreateRequestUrl(_settings.PaymentUrl, _settings.HashSecret);
            return paymentUrl;
        }
        public async Task<string> QueryTransactionAsync(string txnRef, string orderInfo, string transactionDate)
        {

            var parameters = new Dictionary<string, string>
     {
         { "vnp_Version", "2.1.0" },
         { "vnp_Command", "querydr" },
         { "vnp_TmnCode",  _settings.TmnCode },
         { "vnp_TxnRef", txnRef },
         { "vnp_OrderInfo", orderInfo },
         { "vnp_TransactionDate", transactionDate },
         { "vnp_CreateDate", DateTime.UtcNow.AddHours(7).ToString("yyyyMMddHHmmss") },
         { "vnp_IpAddr", "127.0.0.1" }
     };

            string rawData = string.Join("&", parameters
                .OrderBy(kvp => kvp.Key)
                .Select(kvp => $"{kvp.Key}={kvp.Value}"));

            parameters.Add("vnp_SecureHash", _settings.HashSecret);

            using var client = new HttpClient();
            var content = new FormUrlEncodedContent(parameters);
            var response = await client.PostAsync(_settings.ReturnUrl, content);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<bool> ValidateReturnData(IQueryCollection query)
        {
            var vnpay = new VnPayLibrary();
            var vnp_SecureHash = query.FirstOrDefault(p => p.Key == "vnp_SecureHash").Value;
            foreach (var kv in query)
            {
                vnpay.AddResponseData(kv.Key, kv.Value);
            }

            return vnpay.ValidateSignature(vnp_SecureHash, _settings.HashSecret);
        }
    }
}
