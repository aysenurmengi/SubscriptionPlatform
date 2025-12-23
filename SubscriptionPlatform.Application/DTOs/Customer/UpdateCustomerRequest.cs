namespace SubscriptionPlatform.Application.DTOs.Customers
{
    public class UpdateCustomerRequest
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; }
    }
}