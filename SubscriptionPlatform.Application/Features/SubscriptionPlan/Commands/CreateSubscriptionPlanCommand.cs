using MediatR;

namespace SubscriptionPlatform.Application.Features.SubscriptionPlan.Commands
{
    public class CreateSubscriptionPlanCommand : IRequest<Guid>
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public List<PlanItemRequest> Items { get; set; }
    }

    public class PlanItemRequest
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}