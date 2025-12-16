using SubscriptionPlatform.Application.Interfaces;
using System;
using System.Threading.Tasks;

namespace SubscriptionPlatform.Infrastructure.Services
{
    // fake ödeme servisi, gerçek bir ödeme sağlayıcısı ile entegrasyon yerine test amaçlı kullanmak için
    public class FakePaymentService : IPaymentService
    {
        // başarılı dönüyor her işlem
        public Task<string> StoreCreditCardAsync(string customerId, string cardToken)
        {
            
            Console.WriteLine($"[FAKE PAYMENT] Müşteri {customerId} için kart saklandı.");

            var permanentToken = $"fake_pm_token_{customerId}_{Guid.NewGuid().ToString().Substring(0, 8)}";
            return Task.FromResult(permanentToken);
        }

        public Task<(bool IsSuccess, string TransactionId, string ErrorMessage)> ProcessPaymentAsync(decimal amount, string currency, string paymentToken)
        {
            Console.WriteLine($"[FAKE PAYMENT] {paymentToken} ile {amount} {currency} tutarında ödeme işleniyor...");

            bool isSuccessful = true; 

            if (isSuccessful)
            {
                var transactionId = $"fake_txn_{Guid.NewGuid()}";
                Console.WriteLine($"[FAKE PAYMENT] Başarılı İşlem: {transactionId}");
                return Task.FromResult((true, transactionId, string.Empty));
            }
            else
            {
                var errorMessage = "Fake Ödeme Başarısız: Kart Reddedildi (Test Hatası)";
                Console.WriteLine($"[FAKE PAYMENT] Başarısız İşlem: {errorMessage}");
                return Task.FromResult((false, string.Empty, errorMessage));
            }
        }
        
        public Task<(bool IsSuccess, string ErrorMessage)> RefundPaymentAsync(string transactionId)
        {
            Console.WriteLine($"[FAKE PAYMENT] İşlem ID {transactionId} için iade işleniyor...");

            return Task.FromResult((true, string.Empty));
            //return Task.FromResult((false, "Fake: İade süresi dolmuş."));
        }
    }
}