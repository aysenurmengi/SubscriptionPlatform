namespace SubscriptionPlatform.Application.Interfaces
{
    public interface IPaymentService
    {
        //ödeme alma, miktar-para birimi-ödeme yön. tokenı, işlem sonucu
        Task<(bool IsSuccess, string TransactionId, string ErrorMessage)> ProcessPaymentAsync(decimal amount, string currency, string paymentToken);

        //para iadesi
        Task<(bool IsSuccess, string ErrorMessage)> RefundPaymentAsync(string transactionId);

        //müşteri için kart saklama
        Task<string> StoreCreditCardAsync(string customerId, string cardToken);
    }
}
