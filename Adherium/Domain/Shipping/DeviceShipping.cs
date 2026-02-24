namespace Adherium.Domain.Shipping;

public class DeviceShipping
{
    public required Guid Id { get; set; }
    
    public required Guid DeviceId { get; set; }
    
    public required string ShippingAddress { get; set; }
    
    public required DateTime ShippedDate { get; set; }
    
    public DateTime? DeliveredDate { get; set; }
}
