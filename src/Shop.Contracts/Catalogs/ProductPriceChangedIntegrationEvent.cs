namespace Shop.Contracts.Catalogs;

public record ProductPriceChangedIntegrationEvent(int ProductId, decimal NewPrice, decimal OldPrice);