namespace Basket.Contracts;

public record ProductPriceChangedIntegrationEvent(int ProductId, decimal NewPrice, decimal OldPrice);