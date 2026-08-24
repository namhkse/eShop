using System.ComponentModel.DataAnnotations;
using Ordering.Domain.SeedWork;

namespace Ordering.Domain.OrderAggregate;

public class OrderItem : Entity
{
    [Required] public string ProductName { get; private set; }
    public string PictureUrl { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal Discount { get; private set; }
    public int Units { get; private set; }
    public int ProductId { get; private set; }

    protected OrderItem()
    {
    }

    public OrderItem(int productId,
        string productName,
        decimal unitPrice,
        decimal discount,
        string pictureUrl,
        int units = 1)
    {
        if (units <= 0)
            throw new ArgumentOutOfRangeException("Invalid number of units");

        if ((unitPrice * units) < discount)
            throw new ArgumentOutOfRangeException("The total of order item is lower than applied discount");

        ProductId = productId;
        ProductName = productName;
        UnitPrice = unitPrice;
        Discount = discount;
        PictureUrl = pictureUrl;
        Units = units;
    }

    public void SetNewDiscount(decimal discount)
    {
        if (discount <= 0)
            throw new ArgumentOutOfRangeException("Discount is not valid");
        
        Discount = discount;
    }

    public void AddUnits(int units)
    {
        if (units <= 0)
            throw new ArgumentOutOfRangeException("Invalid number of units");
        
        Units += units;
    }
}