namespace SubscriptionPlatform.Application.DTOs.Products
{
    public class RecommendedProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public string MatchingTags { get; set; } 
    }
}